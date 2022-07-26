﻿using System.ComponentModel.DataAnnotations;

namespace AuthServer.DTOs;

public class RegisterRequest
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public bool isCustomer { get; set; } = true;
}
