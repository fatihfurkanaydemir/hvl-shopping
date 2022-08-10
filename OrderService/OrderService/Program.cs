using OrderService.Middlewares;
using Infrastructure.Persistence;
using Application;
using Savorboard.CAP.InMemoryMessageQueue;

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

builder.Services.AddSwaggerGen(c => {
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
      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//builder.Services.AddCap(opts =>
//{
//  opts.UseInMemoryStorage();
//  opts.UseInMemoryMessageQueue();
//  //opts.UseEntityFramework<ApplicationDbContext>();
//  //opts.UsePostgreSql(config.GetConnectionString("DefaultConnection"));
//  opts.UseRabbitMQ(opts =>
//  {
//    opts.ExchangeName = "amq.topic";
//    opts.HostName = config.GetConnectionString("RabbitMQ");
//    opts.UserName = "guest";
//    opts.Password = "guest";
//  });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
