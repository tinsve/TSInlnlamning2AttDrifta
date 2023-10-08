using CodeMonkeyBank.Infrastructure.Database;

namespace CodeMonkeyBank.Infrastructure;

public static class InfrastructureService
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = AwsSecretsManager.GetMongoDbConnectionString();
        string databaseName = "CodeMonkeyBankDB";

        services.AddSingleton(new MongoDbContext(connectionString, databaseName));

        return services;
    }
}
