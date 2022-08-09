using AuthServer.Wrappers;
using AuthServer.DTOs;

namespace AuthServer.Interfaces;

public interface IAccountService
{
  Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
  Task<Response<ProfileInformation>> GetProfileInformation(ProfileInformationRequest request);
  Task<Response<string>> RegisterAsync(RegisterRequest request);
  Task<Response<string>> ChangePassword(ChangePasswordRequest model);
}

