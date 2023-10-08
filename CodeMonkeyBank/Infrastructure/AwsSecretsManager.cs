using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json.Linq;

namespace CodeMonkeyBank.Infrastructure;

public class AwsSecretsManager
{
    public static string GetMongoDbConnectionString()
    {
        string secretName = "Production_CodeMonkeyBank_ConnectionStrings__MongoDbConnection";
        string region = "eu-west-1";

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT"
        };

        GetSecretValueResponse response;

        try
        {
            response = client.GetSecretValueAsync(request).GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            throw e;
        }

        string secret = response.SecretString;

        if (string.IsNullOrEmpty(secret))
        {
            throw new Exception("No secret retrieved from AWS Secrets Manager.");
        }

        string connectionString = secret;

        return connectionString;
    }
}

