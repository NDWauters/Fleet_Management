using BusinessLogicLayer.Authorization;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using FleetService.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Logging setup
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddDebug();
builder.Logging.AddConsole();

builder.Host.ConfigureLogging((context, logging) =>
{
    logging.AddConfiguration(context.Configuration.GetSection("logging"));
});

// CORS setup
services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container.
services.AddScoped<IVehicleService, VehicleService>();
services.AddScoped<IVehicleRepo, VehicleRepo>();

services.AddScoped<IFuelCardService, FuelCardService>();
services.AddScoped<IFuelCardRepo, FuelCardRepo>();

services.AddScoped<IDriverService, DriverService>();
services.AddScoped<IDriverRepo, DriverRepo>();

services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();

services.AddScoped<IBrandRepo, BrandRepo>();
services.AddScoped<IFuelTypeRepo, FuelTypeRepo>();
services.AddScoped<IVehicleTypeRepo, VehicleTypeRepo>();
services.AddScoped<IDriverLicenseTypeRepo, DriverLicenseTypeRepo>();
services.AddScoped<IAddressRepo, AddressRepo>();
services.AddScoped<IUserRepo, UserRepo>();

services.AddSingleton<FleetContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

//app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
