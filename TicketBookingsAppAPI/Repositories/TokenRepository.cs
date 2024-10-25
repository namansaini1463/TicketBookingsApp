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

        public string CreateJWTToken(User user, List<string> roles, string issuer, string audience)
        {
            // Create claims from the user's email and ID
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
            };

            // Add roles to claims, if any
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create signing key from the secret key in configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,      // Use the selected issuer
                audience: audience,   // Use the selected audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60), // Token expiration set to 60 minutes
                signingCredentials: credentials
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
