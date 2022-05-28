using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Lambda.Core.Data.Entities;
using Lambda.Core.Data.Enums;
using Lambda.Core.Data.Repositories;
using Lambda.Core.Functions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda.Products;

public class Function : BaseFunction
{
    private readonly IProductRepository _productRepository;

    public Function()
    {
        var resolver = new DependencyResolver(ConfigureServices);

        _productRepository = resolver.ServiceProvider.GetService<IProductRepository>();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var HttpMethod = Enum.Parse(typeof(HttpMethodEnum), request.HttpMethod);

            switch (HttpMethod)
            {
                case HttpMethodEnum.POST:
                    return await handlePostAsync(request);
                case HttpMethodEnum.GET:
                    return await handleGetAsync(request);
                case HttpMethodEnum.DELETE:
                    return await handleDeleteAsync(request);
                default:
                    throw new Exception($"Unsupported method: ${request.Path}");
            }
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
            throw;
        }
    }

    private async Task<APIGatewayProxyResponse> handlePostAsync(APIGatewayProxyRequest request)
    {
        var data = JsonConvert.DeserializeObject<ProductModel>(request.Body);

        await _productRepository.AddAsync(data);

        return Ok();
    }

    private async Task<APIGatewayProxyResponse> handleGetAsync(APIGatewayProxyRequest request)
    {
        switch (request.Resource)
        {
            case "/products":
                return await getAllAsync(request);
            case "/products/{id}":
                return await getByIdAsync(request);
            case "/products/search":
                return await getSearchAsync(request);
            default:
                throw new Exception($"Unsupported route: ${request.Path}");
        }
    }

    private async Task<APIGatewayProxyResponse> handleDeleteAsync(APIGatewayProxyRequest request)
    {
        if (!request.PathParameters.ContainsKey("id")) return BadRequest();

        var product = await _productRepository.GetByIdAsync(Guid.Parse(request.PathParameters["id"]));

        if (product == null) return NotFound();

        await _productRepository.DeleteAsync(product.Id);

        return Ok();
    }

    private async Task<APIGatewayProxyResponse> getAllAsync(APIGatewayProxyRequest request)
    {
        var products = await _productRepository.GetAllAsync();

        return products.Count() > 0 ? Ok(JsonConvert.SerializeObject(products)) : NotContent();
    }

    private async Task<APIGatewayProxyResponse> getByIdAsync(APIGatewayProxyRequest request)
    {
        if (!request.PathParameters.ContainsKey("id")) return BadRequest();

        var product = await _productRepository.GetByIdAsync(Guid.Parse(request.PathParameters["id"]));

        if (product == null) return NotFound();

        return Ok(JsonConvert.SerializeObject(product));
    }


    private async Task<APIGatewayProxyResponse> getSearchAsync(APIGatewayProxyRequest request)
    {
        var search = new SearchProduct();

        if (request.QueryStringParameters.ContainsKey("name"))
            search.Name = request.QueryStringParameters["name"];
        if (request.QueryStringParameters.ContainsKey("description"))
            search.Description = request.QueryStringParameters["description"];

        var products = await _productRepository.Search(search);

        return products.Count() > 0 ? Ok(JsonConvert.SerializeObject(products)) : NotContent();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
    }

}
