using Amazon.Auth.AccessControlPolicy;
using CodeMonkeyBank.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;

namespace CodeMonkeyBank.Models;

public class LoginModel
{
    [Required]
    public string? Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    // Validation, to check so input is correct
    // Check if the value from the _mockUsers is true or false
    public async Task<bool> ValidateLoginAsync(IAccountRepository accountRepository, IPasswordHashService passwordHashService)
    {
        var user = await accountRepository.GetAccountByUsernameAsync(Username);

        if (user != null) 
        { 
            return passwordHashService.VerifyPassword(Password, user.Password);
        }

        return false;
    }
}