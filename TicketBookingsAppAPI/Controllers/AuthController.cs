using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

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
            // Check if the user already exists
            var existingUser = await userManager.FindByEmailAsync(registerRequestDTO.Username);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new User
            {
                Name = registerRequestDTO.Name,
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };

            var userResult = await userManager.CreateAsync(user, registerRequestDTO.Password);

            if (userResult.Succeeded) 
            {
                // If roles are provided, add them to the user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    var roleResult = await userManager.AddToRolesAsync(user, registerRequestDTO.Roles);

                    if (roleResult.Succeeded)
                    {
                        return Ok("The user was registered! Please login in!");
                    }
                    else
                    {
                        return BadRequest($"User registered, but role assignment failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }

                return Ok("The user was registered without any roles! Please login in!");
            }

            return BadRequest("Something went wrong. Please try again later!");

        }

        // POST : /api/auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.userNameOrEmail);

            if (user != null)
            {
                var validPassword = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (validPassword)
                {
                    // Getting the roles of the user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create a token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList(), "https://localhost:7290", "https://localhost:7290");

                        var response = new LoginResponseDTO
                        {
                            Name = loginRequestDTO.userNameOrEmail,
                            Username = loginRequestDTO.userNameOrEmail,
                            JWTToken = jwtToken,
                        };

                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or Password Incorrect!");
        }
    }
}
