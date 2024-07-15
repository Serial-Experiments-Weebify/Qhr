
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Qhr.Server.Models;
using Sodium;

namespace Qhr.Server.Services;

public interface IAuthService
{
    public Task CreateDefaultUser();
    public string CreateSessionToken(User u);
    public Task<string?> GetUsernameIfJwtValid(string jwt);
    public Task<long> CreateUser(string username, string password);
    public Task<User?> Authenticate(string username, string password);
}


public class AuthService : IAuthService
{
    private readonly QhrContext _ctx;
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _jwtCredentials;
    private readonly JwtSecurityTokenHandler _jwt = new();

    public AuthService(IConfiguration config, QhrContext ctx)
    {
        _config = config;
        _ctx = ctx;

        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? throw new Exception("No JWT key"));
        _key = new(key);
        _jwtCredentials = new(
            _key,
            SecurityAlgorithms.HmacSha256Signature
        );
    }

    public async Task<long> CreateUser(string username, string password)
    {
        User u = new()
        {
            Username = username,
            PasswordHash = PasswordHash.ArgonHashString(password)
        };

        _ctx.Users.Add(u);
        await _ctx.SaveChangesAsync();

        return u.Id;
    }

    public async Task CreateDefaultUser()
    {
        if (await _ctx.Users.AnyAsync()) return;

        await CreateUser(
            _config["User:Name"] ?? throw new Exception("Default username not set"),
            _config["User:Password"] ?? throw new Exception("Default user password not set")
        );
    }

    public async Task<User?> Authenticate(string username, string password)
    {
        var u = await _ctx.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (u == null) return u;

        if (!PasswordHash.ArgonHashStringVerify(u.PasswordHash ?? "", password))
            return null;

        return u;
    }

    public string CreateSessionToken(User u)
    {
        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, u.Username)]),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = _jwtCredentials,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
        };

        return _jwt.CreateEncodedJwt(tokenDesc);
    }

    public async Task<string?> GetUsernameIfJwtValid(string jwt)
    {
        var a = await _jwt.ValidateTokenAsync(jwt, new TokenValidationParameters
        {
            IssuerSigningKey = _key,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidAudience = _config["Jwt:Audience"],
        });

        if (!a.IsValid) return null;
        return a.ClaimsIdentity.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
    }

}
