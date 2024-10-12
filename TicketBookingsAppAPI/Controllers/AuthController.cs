using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: /api/auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            // Check if the user already exists by email
            var existingUser = await userManager.FindByEmailAsync(registerRequestDTO.Username);

            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email or username already exists. Please login." });
            }

            // Create a new user instance
            var user = new User
            {
                Name = registerRequestDTO.Name,
                UserName = registerRequestDTO.Username, // Username set to email
                Email = registerRequestDTO.Username,
                PhoneNumber = registerRequestDTO.PhoneNumber,
            };

            var userResult = await userManager.CreateAsync(user, registerRequestDTO.Password);

            if (userResult.Succeeded)
            {
                // If roles are provided, add them to the user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    var roleResult = await userManager.AddToRolesAsync(user, registerRequestDTO.Roles);

                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(new
                        {
                            message = $"User registered, but role assignment failed.",
                            errors = roleResult.Errors.Select(e => e.Description)
                        });
                    }

                    return Ok(new { message = "User registered successfully with roles! Please log in." });
                }

                return Ok(new { message = "User registered successfully without any roles! Please log in." });
            }

            return BadRequest(new
            {
                message = "User registration failed.",
                errors = userResult.Errors.Select(e => e.Description)
            });
        }

        // POST: /api/auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            // Try to find the user by email
            var user = await userManager.FindByEmailAsync(loginRequestDTO.userNameOrEmail);

            if (user != null)
            {
                // Validate the password
                var validPassword = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (validPassword)
                {
                    // Get roles assigned to the user
                    var roles = await userManager.GetRolesAsync(user);

                    // If the user has roles, create a JWT token
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList(), "https://localhost:7290", "https://localhost:7290");

                        var response = new LoginResponseDTO
                        {
                            Name = loginRequestDTO.userNameOrEmail,
                            Username = loginRequestDTO.userNameOrEmail,
                            JWTToken = jwtToken,
                            UserID = user.Id
                         };

                        return Ok(response);
                    }
                }
            }

            // Return 401 Unauthorized if the credentials are invalid
            return Unauthorized(new { message = "Username or password is incorrect." });
        }
    }
}
