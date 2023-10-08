using MongoDB.Bson;

namespace CodeMonkeyBank.Domain
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(ObjectId id);
        Task CreateAccountAsync(Account account);
        Task<Account> GetAccountByUsernameAsync(string username);
        Task UpdateAsync(Account account);
    }
}
