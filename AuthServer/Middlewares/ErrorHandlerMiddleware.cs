using AuthServer.Exceptions;
using AuthServer.Wrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace AuthServer.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };
            
            switch (error)
            {
                case ApiException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(responseModel, new JsonSerializerSettings
            {
              ContractResolver = new DefaultContractResolver
              {
                NamingStrategy = new SnakeCaseNamingStrategy()
              },
              Formatting = Formatting.Indented
            });

            await response.WriteAsync(result); ;
        }
    }
}
