using Contracts.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public TokenResponse GetToken(TokenRequest request)
        {
            var token = GenerateJwt();
            var result = new TokenResponse(token);
            return result;
            
        }

        private string GenerateJwt()
        {
            return GenerateEncryptedToken(GetSigningCertificate());
        }

        private string GenerateEncryptedToken(SigningCredentials credentials)
        {
            var claims = new Claim[]
            {
                new Claim("Role", "Admin"),
            };
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials,
                claims: claims
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCertificate()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}
