
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lambda.Core.Configuration;

public static class MongoDBConfiguration
{
    public static IServiceCollection AddDynamoDBConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        AWSOptions awsOptions = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"])
        };
        services.AddDefaultAWSOptions(awsOptions);

        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();

        return services;
    }
}
