using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BmisApi.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

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
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok("Register succesful");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model, bool useCookies)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            if (useCookies)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            var token = _userManager.GenerateUserTokenAsync(user, "Default", "Authentication");

            return Ok(new { token });
            
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Username = user.UserName ?? "N/A",
                    Role = role.FirstOrDefault() ?? "No Role",
                    LockoutEnd = user.LockoutEnd,
                    AccessFailedCount = user.AccessFailedCount
                });
            }


            return Ok(userDtos);
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPass(string userEmail, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result.Errors);
        }
    }
}
