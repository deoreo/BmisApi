using BmisApi.Data;
using BmisApi.Repositories;
using BmisApi.Services;
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

// Repositories
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();

// Services
builder.Services.AddSingleton<ResidentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

// For tests
public partial class Program { }