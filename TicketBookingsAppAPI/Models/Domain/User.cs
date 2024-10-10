using Microsoft.AspNetCore.Identity;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
       
    }
}
