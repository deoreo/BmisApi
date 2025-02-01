using BmisApi.Data;
using BmisApi.Identity;
using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.BrgyProject;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Repositories;
using BmisApi.Services;
using BmisApi.Services.HouseholdService;
using BmisApi.Services.IncidentService;
using BmisApi.Services.OfficialService;
using BmisApi.Services.VawcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db connection
var configuration = builder.Configuration;
var username = configuration["db_username"];
var password = configuration["db_password"];

var connectionString = configuration.GetConnectionString("DefaultConnection")?
    .Replace("{USERNAME}", username)
    .Replace("{PASSWORD}", password);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<Sex>();
dataSourceBuilder.MapEnum<BlotterStatus>();
dataSourceBuilder.MapEnum<VawcStatus>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(dataSource));

// Cors
var FrontendApp = "_allowFrontendOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: FrontendApp,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

// Cookies 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
});

// JWT
var jwtSecretKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });


// Identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

// Repositories
builder.Services.AddScoped<ICrudRepository<Resident>, ResidentRepository>();
builder.Services.AddScoped<ICrudRepository<Household>, HouseholdRepository>();
builder.Services.AddScoped<ICrudRepository<Blotter>, BlotterRepository>();
builder.Services.AddScoped<ICrudRepository<BrgyProject>, BrgyProjectRepository>();
builder.Services.AddScoped<ICrudRepository<Official>, OfficialRepository>();
builder.Services.AddScoped<ICrudRepository<Incident>, IncidentRepository>();
builder.Services.AddScoped<ICrudRepository<Vawc>,  VawcRepository>();

// Services
builder.Services.AddScoped
    <ICrudService<Resident, GetResidentResponse, GetAllResidentResponse, CreateResidentRequest, UpdateResidentRequest>, ResidentService>();
builder.Services.AddScoped
    <IHouseholdService, HouseholdService>();
builder.Services.AddScoped
    <ICrudService<Blotter, GetBlotterResponse, GetAllBlotterResponse, CreateBlotterRequest, UpdateBlotterRequest>, BlotterService>();
builder.Services.AddScoped
    <ICrudService<BrgyProject, GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest>, BrgyProjectService>();
builder.Services.AddScoped
    <IOfficialService, OfficialService>();
builder.Services.AddScoped
    <IIncidentService, IncidentService>();
builder.Services.AddScoped
    <IVawcService, VawcService>();

var app = builder.Build();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    try
    {
        await RoleSeeder.SeedRoles(scope.ServiceProvider);
        await AdminSeeder.SeedAdmin(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during seeding: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<IdentityUser>();

app.UseCors(FrontendApp);

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();

// For tests
public partial class Program { }

