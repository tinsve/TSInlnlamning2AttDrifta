using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeMonkeyBank.Domain;

public class Account
{
    [BsonId]
    public ObjectId AccountId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BirthDate { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public double? Balance { get; set; }
}
