using Microsoft.AspNetCore.Http.Features;
using AuthServer.Middlewares;
using AuthServer.Extensions;
using Microsoft.AspNetCore.Identity;
using AuthServer.Models;
using AuthServer.Seeds;

var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
  c.IgnoreObsoleteActions();
  c.IgnoreObsoleteProperties();
  c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddIdentityService(config);

builder.Services.Configure<IdentityOptions>(options =>
{
  options.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöprsþtuüvyzxqwABCÇDEFGÐHIÝJKLMNOÖPRSÞTUÜVYZXQW-._@+1234567890";
});

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(
    builder =>
    {
      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.Configure<FormOptions>(o =>
{
  o.ValueLengthLimit = int.MaxValue;
  o.MultipartBodyLengthLimit = int.MaxValue;
  o.MemoryBufferThreshold = int.MaxValue;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.SeedAsync(userManager, roleManager);
    await DefaultCustomer.SeedAsync(userManager, roleManager);
    await DefaultAdmin.SeedAsync(userManager, roleManager);
  }
  catch (Exception ex)
  {
    Console.Error.WriteLine(ex);
  }
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
