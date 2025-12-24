using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Openbook.AzureFunctions.Authorization;

namespace Openbook.AzureFunctions.Functions;

public class SignalRNegotiate(IConfiguration configuration)
{
    [FunctionAuthorize]
    [Function("negotiate")]
    public async Task<HttpResponseData> Negotiate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        FunctionContext executionContext,
        CancellationToken cancellationToken)
    {
        var logger = executionContext.GetLogger("SignalRNegotiate");
        var connectionString = configuration["Azure:SignalR:ConnectionString"];
        var serviceManager = new ServiceManagerBuilder().WithOptions(o => o.ConnectionString = connectionString).BuildServiceManager();
        var hubContext = await serviceManager.CreateHubContextAsync("reviewHub", cancellationToken);
        var negotiateResponse = await hubContext.NegotiateAsync(cancellationToken: cancellationToken);
        if (negotiateResponse is null)
        {
            logger.LogError("Failed to create SignalR negotiate response.");
            var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Failed to create SignalR negotiate response.", cancellationToken);
            return errorResponse;
        }
        
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { token = negotiateResponse.AccessToken, error = negotiateResponse.Error }, cancellationToken);
        return response;
    }
}