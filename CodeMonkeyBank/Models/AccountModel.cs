using CodeMonkeyBank.Application;
using CodeMonkeyBank.Domain;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CodeMonkeyBank.Models;

public class AccountModel
{
    public ObjectId AccountId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BirthDate { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public double? Balance { get; set; }
    public AccountModel() 
    { 
        AccountId = ObjectId.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        BirthDate = string.Empty;
        Email = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        Balance = 0;
    }
}


