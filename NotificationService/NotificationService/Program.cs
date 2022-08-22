using NotificationService.Middlewares;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Application;
using GlobalInfrastructure;
using NotificationService.Extensions;
using NotificationService.Application.Hubs;

var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPersistenceInfrastructure(config);
builder.Services.AddApplicationLayer(config);

builder.Services.AddSwaggerGen(c =>
{
  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
  c.IgnoreObsoleteActions();
  c.IgnoreObsoleteProperties();
  c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(
    builder =>
    {
      builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowCredentials();
    });
});

builder.Services.AddGlobalInfrastructure(config);

builder.Services.AddSignalR(config =>
{
  config.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapHub<DiscountHub>("/api/discounthub");

app.UseCors();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.SubscribeEvents();

app.Run();