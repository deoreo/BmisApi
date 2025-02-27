using BmisApi.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BmisApi.Identity
{
    [AuditLog]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return BadRequest("User already exixts.");
            }

            var user = new ApplicationUser
            {
                UserName = model.Username.Trim(),
            };

            var result = await _userManager.CreateAsync(user, model.Password.Trim());
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest("Role does not exist");
            }
            await _userManager.AddToRoleAsync(user, model.Role);

            user.CreatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Ok("Register succesful");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            //if (useCookies)
            //{
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return Ok();
            //}

            var userRolesList = await _userManager.GetRolesAsync(user);
            var userRole = userRolesList.FirstOrDefault() ?? "NoRole";

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Role, userRole)
            };


            var keyBytes = Convert.FromBase64String(_config["Jwt:Key"]);
            var key = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],  // Replace with your issuer
                audience: _config["Jwt:Audience"],  // Replace with your audience
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(3),  // Token expiry time
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new 
            {
                token = tokenString,
                expiration = token.ValidTo,
                username = user.UserName,
                role = userRole
            });
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        [Route("get-all")]
        [NoAuditLog]
        public async Task<ActionResult<GetAllUserResponse>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName ?? "N/A",
                    Role = role.FirstOrDefault() ?? "No Role",
                    LockoutEnd = user.LockoutEnd,
                    AccessFailedCount = user.AccessFailedCount
                });
            }


            return Ok(new GetAllUserResponse(userDtos));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [Route("admin-reset-password")]
        public async Task<IActionResult> AdminResetPass(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                user.LastUpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                return Ok(result);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("user-reset-password")]
        public async Task<IActionResult> UserResetPass(string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not logged in.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                user.LastUpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                return Ok(result);
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        [Route("change-role")]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.UserName == "Kapitan")
            {
                return BadRequest("Cannot change the role of Kapitan.");
            }

            var existingRoles = await _userManager.GetRolesAsync(user);
            if (existingRoles.Any())
            {
                var removeRoles = await _userManager.RemoveFromRolesAsync(user, existingRoles);
                if (!removeRoles.Succeeded)
                {
                    return BadRequest("Failed to remove previous roles");
                }
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, newRole);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            user.LastUpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok("Role assigned successfully.");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user  == null)
            {
                return NotFound("User not found.");
            }

            if (user.UserName == "Kapitan")
            {
                return BadRequest("Cannot delete the account of Kapitan.");
            }

            user.DeletedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

    }
}
