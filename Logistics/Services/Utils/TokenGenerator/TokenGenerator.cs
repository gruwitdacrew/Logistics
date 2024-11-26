using Logistics.Data.Account.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logistics.Services.Utils.TokenGenerator
{
    public class TokenGenerator
    {
        private readonly TokenGeneratorConfiguration _configuration;
        public TokenGenerator(TokenGeneratorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user, Role role)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", user.id.ToString()),
                new Claim("TokenType", "Access"),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            DateTime expires = DateTime.UtcNow.AddSeconds(_configuration.AccessTokenExpirationSeconds);

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.AccessTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                claims,
                DateTime.UtcNow,
                expires: expires,
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken(User user, Token tokenType)
        {
            DateTime expires;
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", user.id.ToString()),
            };

            switch (tokenType)
            {
                case Token.Refresh:
                    {
                        expires = DateTime.UtcNow.AddHours(_configuration.RefreshTokenExpirationHours);
                        claims.Add(new Claim("TokenType", Token.Refresh.ToString()));
                        break;
                    }
                case Token.ApproveEmail:
                    {
                        expires = DateTime.UtcNow.AddMinutes(_configuration.ApproveEmailTokenExpirationMinutes);
                        claims.Add(new Claim("TokenType", Token.ApproveEmail.ToString()));
                        break;
                    }
                default:
                    {
                        expires = DateTime.UtcNow.AddMinutes(_configuration.ResetPasswordTokenExpirationMinutes);
                        claims.Add(new Claim("TokenType", Token.ResetPassword.ToString()));
                        break;
                    }
            }

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.AccessTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                claims,
                DateTime.UtcNow,
                expires: expires,
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public enum Token
    {
        Refresh,
        ApproveEmail,
        ResetPassword
    }
}
