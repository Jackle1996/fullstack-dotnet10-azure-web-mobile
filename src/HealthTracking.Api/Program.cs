using AutoMapper;
using FluentValidation;
using HealthTracking.Api.Data;
using HealthTracking.Application.Dtos;
using HealthTracking.Application.Mapping;
using HealthTracking.Application.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HealthTrackingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.ValueTransformers.Add<string>(s => s.Trim());
}, typeof(HealthRecordProfile).Assembly);
builder.Services.AddScoped<IValidator<HealthRecordCreateDto>, HealthRecordCreateDtoValidator>();

var app = builder.Build();

// Apply migrations and create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HealthTrackingDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        // Point Swagger UI at the Aspire/OpenAPI endpoint
        options.SwaggerEndpoint("/openapi/v1.json", "HealthTracking.Api v1");
        // Changes the swagger UI to be hosted under /openapi/index.html
        options.RoutePrefix = "openapi";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
