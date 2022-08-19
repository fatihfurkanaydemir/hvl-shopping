using ReviewService.Infrastructure.Persistence;
using ReviewService.Infrastructure.Persistence.Models;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistanceServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
      builder =>
      {
          builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
      });
});
builder.Services.Configure<ReviewDatabaseSettings>(config.GetSection("ReviewDatabaseSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
