using Application.Wrappers;
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
}
