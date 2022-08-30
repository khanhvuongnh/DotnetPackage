using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Helpers.Utilities;
public interface IJwtUtility
{
    public string GenerateJwtToken(Claim[] claims, string secretKey, DateTime expires);
    public string ValidateJwtToken(string token, string secretKey, string claimType = "nameid");
    public RefreshToken GenerateRefreshToken(string username, DateTime expires);
}

public class JwtUtility : IJwtUtility
{
    private readonly IConfiguration _configuration;

    public JwtUtility(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(Claim[] claims, string secretKey, DateTime expires)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string ValidateJwtToken(string token, string secretKey, string claimType = "nameid")
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var username = jwtToken.Claims.First(x => x.Type == claimType).Value;
            return username;
        }
        catch
        {
            return null;
        }
    }

    public RefreshToken GenerateRefreshToken(string username, DateTime expires)
    {
        var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = expires,
            CreatedTime = DateTime.Now,
            Username = username
        };
        return refreshToken;
    }
}

public class RefreshToken
{
    [Key]
    public int Token_ID { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? RevokedTime { get; set; }
    public string ReplacedByToken { get; set; }
    public string ReasonRevoked { get; set; }
    [Required]
    [StringLength(50)]
    public string Username { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => RevokedTime != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}