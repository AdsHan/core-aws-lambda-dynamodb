using Amazon.Lambda.APIGatewayEvents;

namespace Lambda.Core.Functions;


public abstract class BaseFunction
{
    public APIGatewayProxyResponse BadRequest(string message = null)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = 400,
            Body = message != null ? message : "Bad Request"
        };
    }

    public APIGatewayProxyResponse NotFound(string message = null)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = 404,
            Body = "Not Found"
        };
    }

    public APIGatewayProxyResponse Ok(string message = null)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = message != null ? message : "Ok Success"
        };
    }


    public APIGatewayProxyResponse NotContent()
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = 204,
            Body = "No Content"
        };
    }


}
