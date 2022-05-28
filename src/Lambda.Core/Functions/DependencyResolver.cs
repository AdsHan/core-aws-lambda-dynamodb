using Lambda.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Lambda.Core.Functions;

public class DependencyResolver
{
    public IServiceProvider ServiceProvider { get; }
    public Action<IServiceCollection> RegisterServices { get; }

    public DependencyResolver(Action<IServiceCollection> registerServices = null)
    {
        var config = ConfigureAppConfiguration();

        var serviceCollection = new ServiceCollection();
        RegisterServices = registerServices;
        ConfigureServices(serviceCollection, config);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.AddDynamoDBConfiguration(config);

        services.AddLogging(logging =>
        {
            logging.AddLambdaLogger();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        RegisterServices?.Invoke(services);
    }

    private IConfiguration ConfigureAppConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
