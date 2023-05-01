using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Aki32Utilities.Azure.B2CAuth;
public static class Function1
{
    [FunctionName("auth-test")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "auth-test/{requestedRoute}")] HttpRequest req,
        string requestedRoute,
        ILogger log)
    {

        log.LogInformation("C# HTTP trigger function processed a request.");

        switch (requestedRoute.ToLower())
        {
            case "ano":
                {

                    return new OkObjectResult("anonymous");

                }
            case "auth":
                {
                    var validatedUser = await BootLoader.ValidateToken(req, log);

                    if (validatedUser == null)
                    {
                        log.LogInformation("Invalid token");
                        return new UnauthorizedResult();
                    }
                    else
                    {
                        log.LogInformation("Token is valid!");
                        return new OkObjectResult($"You are validated, welcome {validatedUser.Name}!!");
                    }
                }
            default:
                {

                    return new BadRequestObjectResult($"Route must be \"ano\" or \"auth\"");

                }
        }
    }


}
