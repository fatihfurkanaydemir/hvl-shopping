using AuthServer.Interfaces;
using AuthServer.Wrappers;
using AuthServer.Settings;
using AuthServer.Contexts;
using AuthServer.Models;
using AuthServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace AuthServer.Extensions;

public static class ServiceExtensions
{
    public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseInMemoryDatabase("IdentityDb"));
        }
        else
        {
            services.AddDbContext<IdentityContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("IdentityConnection"),
                b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
        }

        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
        services.AddTransient<IAccountService, AccountService>();

        services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

        services.Configure<IdentityOptions>(options =>
        {
          options.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöprsştuüvyzxqwABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZXQW-._@+1234567890";
        });

    //services.AddAuthentication(options =>
    //{
    //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //})
    // .AddJwtBearer(o =>
    //    {
    //        o.RequireHttpsMetadata = false;
    //        o.SaveToken = false;
    //        o.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ClockSkew = TimeSpan.Zero,
    //            ValidIssuer = configuration["JWTSettings:Issuer"],
    //            ValidAudience = configuration["JWTSettings:Audience"],
    //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
    //        };
    //        o.Events = new JwtBearerEvents()
    //        {
    //            OnAuthenticationFailed = c =>
    //            {
    //                c.NoResult();
    //                c.Response.StatusCode = 500;
    //                c.Response.ContentType = "text/plain";
    //                return c.Response.WriteAsync(c.Exception.ToString());
    //            },
    //            OnChallenge = context =>
    //            {
    //                context.HandleResponse();
    //                context.Response.StatusCode = 401;
    //                context.Response.ContentType = "application/json";
    //                var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
    //                return context.Response.WriteAsync(result);
    //            },
    //            OnForbidden = context =>
    //            {
    //                context.Response.StatusCode = 403;
    //                context.Response.ContentType = "application/json";
    //                var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
    //                return context.Response.WriteAsync(result);
    //            },
    //        };
    //    });
  }
}
