using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.DTOs;
using AuthServer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Controllers;

  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
  private readonly IAccountService _accountService;
  public AccountController(IAccountService accountService)
  {
      _accountService = accountService;
  }

  [HttpPost("authenticate")]
  public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
  {
      return Ok(await _accountService.AuthenticateAsync(request));
  }

  [HttpPost("register")]
  public async Task<IActionResult> RegisterAsync(RegisterRequest request)
  {
      return Ok(await _accountService.RegisterAsync(request));
  }
  
  [HttpPost("change-password")]
  public async Task<IActionResult> ResetPassword(ChangePasswordRequest request)
  {   
      return Ok(await _accountService.ChangePassword(request));
  }

  [HttpGet("{identityId}")]
  public async Task<IActionResult> GetProfile(string identityId)
  {
    return Ok(await _accountService.GetProfileInformation(new ProfileInformationRequest { IdentityId = identityId }));
  }
}
