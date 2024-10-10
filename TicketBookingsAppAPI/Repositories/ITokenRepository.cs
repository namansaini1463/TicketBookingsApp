using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(User user, List<String> roles, string selectedIssuer, string selectedAudience);
    }
}
