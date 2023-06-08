using EventsAPI.Data;
using EventsAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Connection to database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add DbContext
services.AddSqlServer<ApplicationDbContext>(connectionString);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
