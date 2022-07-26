using DiscountService.Middlewares;
using DiscountService.Infrastructure.Persistence;
using DiscountService.Application;
using GlobalInfrastructure;
using DiscountService.Extensions;

var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPersistenceInfrastructure(config);
builder.Services.AddApplicationLayer(config);

//builder.Services.AddSwaggerGen(c => {
//  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
//  c.IgnoreObsoleteActions();
//  c.IgnoreObsoleteProperties();
//  c.CustomSchemaIds(type => type.FullName);
//});

//builder.Services.AddCors(options =>
//{
//  options.AddDefaultPolicy(
//    builder =>
//    {
//      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//    });
//});

builder.Services.AddSignalR(config =>
{
  config.EnableDetailedErrors = true;
});


builder.Services.AddGlobalInfrastructure(config);

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//  app.UseSwagger();
//  app.UseSwaggerUI();
//}

//app.UseMiddleware<ErrorHandlerMiddleware>();

//app.UseCors();
//app.UseRouting();
//app.UseSwagger();
//app.UseSwaggerUI();
//app.UseHttpsRedirection();
//app.MapControllers();

app.RegisterRPCs();

app.Run();