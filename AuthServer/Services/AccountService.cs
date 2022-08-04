using AuthServer.Interfaces;
using AuthServer.Wrappers;
using AuthServer.Exceptions;
using AuthServer.DTOs;
using AuthServer.Settings;
using AuthServer.Models;
using AuthServer.Enums;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

namespace AuthServer.Services;

public class AccountService : IAccountService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly JWTSettings _jwtSettings;

  public AccountService(UserManager<ApplicationUser> userManager,
        IOptions<JWTSettings> jwtSettings,
        SignInManager<ApplicationUser> signInManager)
  {
    _userManager = userManager;
    _jwtSettings = jwtSettings.Value;
    _signInManager = signInManager;
  }

  public async Task<Response<string>> RegisterAsync(RegisterRequest request)
  {
    var user = new ApplicationUser
    {
      Email = request.Email,
      UserName = request.Email
    };

    var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
    if (userWithSameEmail != null) throw new ApiException($"Email {request.Email} is already registered.");
    
    var result = await _userManager.CreateAsync(user, request.Password);
    if (result.Succeeded)
    {
      if (request.isCustomer)
        await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());
      else
        await _userManager.AddToRoleAsync(user, Roles.Seller.ToString());

      return new Response<string>(user.Id, message: "User Registered.");
    }
    else
    {
      var errors = new List<string>();
      foreach (var error in result.Errors)
      {
        errors.Add(error.Description);
      }

      return new Response<string> { Errors = errors, Succeeded = false, Message = "Register failed due to errors." };
    }
  }

  public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null)
    {
      throw new ApiException($"No Accounts Registered with {request.Email}.");
    }

    var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
    if (!result.Succeeded)
    {
      throw new ApiException($"Invalid Credentials for '{request.Email}'.");
    }

    JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
    AuthenticationResponse response = new AuthenticationResponse();
    response.Id = user.Id;
    response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    response.Email = user.Email;

    var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
    response.Roles = rolesList.ToList();

    return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
  }

  private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
  {
    var userClaims = await _userManager.GetClaimsAsync(user);
    var roles = await _userManager.GetRolesAsync(user);

    var roleClaims = new List<Claim>();

    for (int i = 0; i < roles.Count; i++)
    {
      roleClaims.Add(new Claim("roles", roles[i]));
    }

    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
    }
    .Union(userClaims)
    .Union(roleClaims);

    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

    var jwtSecurityToken = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
        signingCredentials: signingCredentials
     );

    return jwtSecurityToken;
  }

  public async Task<Response<string>> ChangePassword(ChangePasswordRequest request)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null)
    {
      throw new ApiException($"No Accounts Registered with {request.Email}.");
    }

    var isPasswordTrue = await _userManager.CheckPasswordAsync(user, request.OldPassword);
    if (!isPasswordTrue)
    {
      throw new ApiException($"Invalid Credentials for '{request.Email}'.");
    }

    var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
    if(!result.Succeeded)
    {
      throw new ApiException($"Password Change Failed for '{request.Email}'.");
    }

    return new Response<string>($"Password Successfully Changed for '{request.Email}'.");
  }
}

