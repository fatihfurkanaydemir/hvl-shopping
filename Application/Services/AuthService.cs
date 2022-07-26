﻿using Application.Wrappers;
using Domain.Common;
using Newtonsoft.Json;
using System.Text;

namespace Application.Services;

public class AuthService
{
  private readonly HttpClient _httpClient;

  public AuthService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<Response<string>> RegisterCustomer(string email, string password, string confirmPassword)
  {
    var requestData = new
    {
      email,
      password,
      confirmPassword,
      isCustomer = true
    };

    var registerCustomerJson = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);

    using var httpResponseMessage = await _httpClient.PostAsync("/api/Account/register", registerCustomerJson);

    var response = JsonConvert.DeserializeObject<Response<string>>( await httpResponseMessage.Content.ReadAsStringAsync());
    
    return response;
  }

  public async Task<Response<string>> RegisterSeller(string email, string password, string confirmPassword)
  {
    var requestData = new
    {
      email,
      password,
      confirmPassword,
      isCustomer = false
    };

    var registerSellerJson = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);

    using var httpResponseMessage = await _httpClient.PostAsync("/api/Account/register", registerSellerJson);

    var response = JsonConvert.DeserializeObject<Response<string>>(await httpResponseMessage.Content.ReadAsStringAsync());

    return response;
  }

  public async Task<Response<ProfileInformation>> GetProfileInformation(string identityId)
  {
    using var httpResponseMessage = await _httpClient.GetAsync($"/api/Account/{identityId}");

    var response = JsonConvert.DeserializeObject<Response<ProfileInformation>>(await httpResponseMessage.Content.ReadAsStringAsync());

    return response;
  }
}
