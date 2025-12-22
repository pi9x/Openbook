using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Openbook.Identity;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Openbook.AzureFunctions;

public sealed class AuthFunction(
    UserStore<ApplicationUser, IdentityRole, ApplicationIdentityDbContext> userStore,
    PasswordHasher<ApplicationUser> passwordHasher,
    IConfiguration configuration)
{
    [Function("AuthLogin")]
    public async Task<HttpResponseData> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")]
        HttpRequestData req)
    {
        var loginRequest = await req.ReadFromJsonAsync<LoginRequest>();
        if (loginRequest is null)
            return await BadRequest(req, "Invalid request");
        
        // Use SingleOrDefaultAsync to ensure only one user with the given email exists
        // Performance is negotiable
        var user = await userStore.Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user is null)
            return await Unauthorized(req);

        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash!,
            loginRequest.Password);

        if (result == PasswordVerificationResult.Failed)
            return await Unauthorized(req);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JwtKey"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["JwtIssuer"],
            audience: configuration["JwtAudience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { token = jwt });
        return response;
    }

    private static async Task<HttpResponseData> Unauthorized(HttpRequestData req)
    {
        var res = req.CreateResponse(HttpStatusCode.Unauthorized);
        await res.WriteStringAsync("Invalid credentials");
        return res;
    }

    private static async Task<HttpResponseData> BadRequest(HttpRequestData req, string message)
    {
        var res = req.CreateResponse(HttpStatusCode.BadRequest);
        await res.WriteStringAsync(message);
        return res;
    }
}

public sealed record LoginRequest(string Email, string Password);
