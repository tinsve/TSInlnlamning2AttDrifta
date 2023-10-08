using CodeMonkeyBank.Domain;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CodeMonkeyBank.Application
{
    public interface IAccountService
    {
        Task<AccountDTO> GetAccountAsync(ObjectId id);
        Task CreateAccountAsync(Account account);
        Task<string?> ValidateUsername(string? username);
        Task<AccountDTO> GetAccountByUsernameAsync(string? username);
        Task UpdateBalanceAsync(string? username, double value, bool isWithdrawal);
    }
}
