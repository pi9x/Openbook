using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.IdentityModel.Tokens;

namespace Openbook.AzureFunctions.Authorization;

public sealed class FunctionAuthorizationMiddleware(TokenValidationParameters parameters) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var method = context.GetTargetFunctionMethod();

        if (method.GetCustomAttribute<FunctionAuthorizeAttribute>() is null)
        {
            await next(context);
            return;
        }

        var request = await context.GetHttpRequestDataAsync();
        if (request is null || !request.Headers.TryGetValues("Authorization", out var values))
        {
            await WriteUnauthorized(context);
            return;
        }

        var raw = values.First();
        if (!raw.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            await WriteUnauthorized(context);
            return;
        }

        var token = raw["Bearer ".Length..];

        ClaimsPrincipal principal;
        try
        {
            principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, parameters, out _);
        }
        catch
        {
            throw new UnauthorizedAccessException();
        }

        context.Items["User"] = principal;

        await next(context);
    }
    
    private static async Task WriteUnauthorized(FunctionContext context)
    {
        var request = await context.GetHttpRequestDataAsync();
        var response = request!.CreateResponse(HttpStatusCode.Unauthorized);

        response.Headers.Add("Content-Type", "application/json");

        await response.WriteStringAsync(JsonSerializer.Serialize(new
        {
            errors = new[]
            {
                new { message = "Unauthorized" }
            }
        }));

        context.GetInvocationResult().Value = response;
    }
}