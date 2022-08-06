using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AuthServer.DTOs;

public class AuthenticationResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string JWToken { get; set; }
}

