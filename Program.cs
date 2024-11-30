using BmisApi.Data;
using BmisApi.Models;
using BmisApi.Models.DTOs.Household;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Repositories;
using BmisApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var configuration = builder.Configuration;
var username = configuration["db_username"];
var password = configuration["db_password"];

// Db Context
var connectionString = configuration.GetConnectionString("DefaultConnection")?
    .Replace("{USERNAME}", username)
    .Replace("{PASSWORD}", password);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<ICrudRepository<Resident>, ResidentRepository>();
builder.Services.AddScoped<ICrudRepository<Household>, HouseholdRepository>();

// Services
builder.Services.AddScoped
    <ICrudService<GetResidentResponse, GetAllResidentResponse, CreateResidentRequest, UpdateResidentRequest>, ResidentService>();
builder.Services.AddScoped<
    ICrudService<GetHouseholdResponse, GetAllHouseholdResponse, CreateHouseholdRequest, UpdateHouseholdRequest>, HouseholdService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<IdentityUser>();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();

// For tests
public partial class Program { }