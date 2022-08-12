using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Seeds;
using Application;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.Features;
using WebApi.Middlewares;
using WebApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Wrappers;
using Newtonsoft.Json;
using WebApi.Settings;
using Application.Services;
using GlobalInfrastructure;

var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
  options.InvalidModelStateResponseFactory = actionContext =>
  {
    return Response<string>.ModelValidationErrorResponse(actionContext);
  };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
  c.IgnoreObsoleteActions();
  c.IgnoreObsoleteProperties();
  c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddApplicationLayer(config);
builder.Services.AddPersistenceInfrastructure(config);
builder.Services.AddSwaggerExtension();

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

builder.Services.Configure<JWTSettings>(config.GetSection("JWTSettings"));

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
  o.RequireHttpsMetadata = false;
  o.SaveToken = false;
  o.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    ValidIssuer = config["JWTSettings:Issuer"],
    ValidAudience = config["JWTSettings:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:Key"])),
  };
  o.Events = new JwtBearerEvents()
  {
    OnAuthenticationFailed = c =>
    {
      c.NoResult();
      c.Response.StatusCode = 500;
      c.Response.ContentType = "text/plain";
      return c.Response.WriteAsync(c.Exception.ToString());
    },
    OnChallenge = context =>
    {
      context.HandleResponse();
      context.Response.StatusCode = 401;
      context.Response.ContentType = "application/json";
      var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
      return context.Response.WriteAsync(result);
    },
    OnForbidden = context =>
    {
      context.Response.StatusCode = 403;
      context.Response.ContentType = "application/json";
      var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
      return context.Response.WriteAsync(result);
    },
  };
});

builder.Services.AddGlobalInfrastructure(config);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var productRepository = services.GetRequiredService<IProductRepositoryAsync>();
    var categoryRepository = services.GetRequiredService<ICategoryRepositoryAsync>();
    var customerRepository = services.GetRequiredService<ICustomerRepositoryAsync>();
    var sellerRepository = services.GetRequiredService<ISellerRepositoryAsync>();
    var authService = services.GetRequiredService<AuthService>();

    await DefaultCategories.SeedAsync(categoryRepository);
    await DefaultSellers.SeedAsync(sellerRepository, authService);
    await DefaultProducts.SeedAsync(productRepository, categoryRepository, sellerRepository);
    //await DefaultCustomers.SeedAsync(customerRepository);

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
