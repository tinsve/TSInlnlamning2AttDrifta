using MongoDB.Driver;

namespace CodeMonkeyBank.Infrastructure.Database;

public class MongoDbContext
{

    public IMongoDatabase Database { get; }

    public MongoDbContext(string connectionString, string databaseName)
    {
        var mongoClient = new MongoClient(connectionString);
        Database = mongoClient.GetDatabase(databaseName);
    }

}
