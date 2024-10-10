using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJWTToken(User user, List<string> roles, string selectedIssuer, string selectedAudience)
        {
            // Create claims from the roles and other information
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Add roles to claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create signing key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create JWT token
            var token = new JwtSecurityToken(
                issuer: selectedIssuer,      // Use the selected issuer
                audience: selectedAudience,   // Use the selected audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60), // Consider using UTC for consistency
                signingCredentials: credentials
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
