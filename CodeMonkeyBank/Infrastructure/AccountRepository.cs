using CodeMonkeyBank.Domain;
using CodeMonkeyBank.Infrastructure;
using CodeMonkeyBank.Infrastructure.Database;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CodeMonkeyBank.Infrastructure;

public class AccountRepository : IAccountRepository
{
    

    private readonly IMongoCollection<Account> _accountCollection;

    public AccountRepository(MongoDbContext dbContext)
    {
        _accountCollection = dbContext.Database.GetCollection<Account>("Accounts");
    }

    public async Task<Account> GetAccountByIdAsync(ObjectId accountId)
    {
        var filter = Builders<Account>.Filter.Eq(c => c.AccountId, accountId);
        return await _accountCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAccountAsync(Account account)
    {
        await _accountCollection.InsertOneAsync(account);
    }

    public async Task<Account> GetAccountByUsernameAsync(string username)
    {
        var filter = Builders<Account>.Filter.Eq(c =>c.Username, username);
        return await _accountCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        var filter = Builders<Account>.Filter.Eq(a => a.Username, account.Username);
        var update = Builders<Account>.Update.Set(a => a.Balance, account.Balance);

        await _accountCollection.UpdateOneAsync(filter, update);
    }
}
