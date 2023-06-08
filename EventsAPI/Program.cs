using EventsAPI.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Connection to database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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

app.Run();
