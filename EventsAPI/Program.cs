using EventsAPI.Data;
using EventsAPI.Endpoints;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Connection to database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add DbContext
services.AddSqlServer<ApplicationDbContext>(connectionString);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    // Avoids conflict between data models
    options.CustomSchemaIds(type => type.ToString());

    // API documentation
    options.EnableAnnotations();
    options.ExampleFilters();

    // API info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Events API",
        Description = "An ASP.NET Core Minimal API for managing cultural events",
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Url = new Uri("https://github.com/Bluelodge")
        }
    });
});

// Allow examples in responses 
services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

// Ignore null fields in responses
services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoints
app.MapAttendeeEndpoints();
app.MapTalkEndpoints();
app.MapEventEndpoints();
app.MapGuestEndpoints();
app.MapCategoryEndpoints();
app.MapOrganizationEndpoints();

app.Run();
