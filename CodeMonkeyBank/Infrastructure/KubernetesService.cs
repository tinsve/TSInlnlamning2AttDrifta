using CodeMonkeyBank.Infrastructure.Database;

namespace CodeMonkeyBank.Infrastructure;

public static class KubernetesService
{
    public static IServiceCollection AddKubernetesServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDbSettings");
                
        services.AddSingleton(new MongoDbContext(mongoDbSettings["ConnectionString"], mongoDbSettings["DatabaseName"]));

        return services;
    }
}