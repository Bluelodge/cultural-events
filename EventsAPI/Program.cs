using EventsAPI.Data;
using EventsAPI.Endpoints;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

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
});

// Allow examples in responses 
services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

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
