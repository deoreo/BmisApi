using BmisApi.Data;
using BmisApi.Identity;
using BmisApi.Models;
using BmisApi.Models.DTOs.Blotter;
using BmisApi.Models.DTOs.BrgyProject;
using BmisApi.Repositories;
using BmisApi.Services;
using BmisApi.Services.HouseholdService;
using BmisApi.Services.IncidentService;
using BmisApi.Services.OfficialService;
using BmisApi.Services.ResidentService.ResidentService;
using BmisApi.Services.VawcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer <your_token>' in the field below."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Img path
var uploadPath = builder.Configuration["Storage:UploadPath"];

// Db connection
var configuration = builder.Configuration;
var username = configuration["db_username"];
var password = configuration["db_password"];

var connectionString = configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<Sex>();
dataSourceBuilder.MapEnum<BlotterStatus>();
dataSourceBuilder.MapEnum<VawcStatus>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(dataSource));

// Cors
var FrontendApp = "_allowFrontendOrigin";
var origin = configuration["FrontendOrigin:Origin"];


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: FrontendApp,
        policy =>
        {
            policy.WithOrigins(origin ?? "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
            //.AllowCredentials();
        });
});

// Identity
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT
var jwtSecretKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSecretKey ?? "fa6e4a34293c2ad87a009973dabba27823c1d438579428ea3b442749093f527b5f9fafd51adf961a76e32987b6c0ede6c667ff02a1c358143b095926fd5a9fde9de04b5fcb20ffb21e85e259f9ef6d0fdf5bdb3bece85de6f468977f7da7e7d29a3c370cca3c00fb60c3294b5ba25688878d67fd6fa19cac5378e51ce5ed753c")),
            ClockSkew = TimeSpan.Zero
        };

        options.MapInboundClaims = false;
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
    .AddPolicy("RequireSecretaryRole", policy => policy.RequireRole("Admin", "Secretary"))
    .AddPolicy("RequireClerkRole", policy => policy.RequireRole("Admin", "Secretary", "Clerk"))
    .AddPolicy("RequireWomanDeskRole", policy => policy.RequireRole("Admin", "Secretary", "WomanDesk"));

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
    <IResidentService, ResidentService>();
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
builder.Services.AddScoped
    <PictureService>();

var app = builder.Build();

app.UseCors(FrontendApp);

app.UseAuthentication();
app.UseAuthorization();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Wait for database to be available
        var retry = 10;
        while (retry > 0)
        {
            try
            {
                context.Database.Migrate();
                break;
            }
            catch (Exception)
            {
                retry--;
                if (retry == 0) throw;
                Thread.Sleep(2000); // Wait 2 seconds before retrying
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }

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

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider( @"C:\uploads"),
    // FileProvider = new PhysicalFileProvider(uploadPath ?? @"C:\uploads"),
    RequestPath = "/uploads"  
});

//app.UseHttpsRedirection();

app.MapControllers()
    .RequireAuthorization();

app.Run();

// For tests
public partial class Program { }

