using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenRepository tokenRepository;

        //private readonly SignInManager<User> signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenRepository = tokenRepository;
            //this.signInManager = signInManager;
        }

        /*
        // POST: /api/auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new User object from the registration data
            var user = new User
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = !string.IsNullOrWhiteSpace(registerDTO.PhoneNumber)
                              ? registerDTO.PhoneNumber
                              : null,  // Default value is null when PhoneNumber is not provided
                ProfilePictureUrl = !string.IsNullOrWhiteSpace(registerDTO.ProfilePictureUrl)
                            ? registerDTO.ProfilePictureUrl
                            : "https://res.cloudinary.com/dpd6oloy8/image/upload/v1729058704/DefualtProfilePicture_atiafh.png",  // Default profile picture URL
                PreferredLanguage = !string.IsNullOrWhiteSpace(registerDTO.PreferredLanguage)
                            ? registerDTO.PreferredLanguage
                            : "English",  // Default value for Preferred Language
                PreferredCurrency = !string.IsNullOrWhiteSpace(registerDTO.PreferredCurrency)
                            ? registerDTO.PreferredCurrency
                            : "INR"  // Default value for Preferred Currency
            };


            var result = await userManager.CreateAsync(user, registerDTO.Password);

            // If user registration is successful
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully. Please login!" });
            }

            // If registration fails, return the errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        */

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO) 
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string profilePictureUrl = "https://res.cloudinary.com/dpd6oloy8/image/upload/v1729058704/DefualtProfilePicture_atiafh.png"; // Default image URL

            // Handle file upload if a profile picture is provided in the DTO
            if (registerDTO.ProfilePicture != null && registerDTO.ProfilePicture.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "user-profile-images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate a unique filename for the uploaded profile picture
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(registerDTO.ProfilePicture.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                // Save the uploaded file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await registerDTO.ProfilePicture.CopyToAsync(stream);
                }

                // Set the profile picture URL to the file path, accessible by the frontend
                profilePictureUrl = $"http://localhost:5027/user-profile-images/{fileName}";
            }

            // Create a new User object from the registration data
            var user = new User
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = !string.IsNullOrWhiteSpace(registerDTO.PhoneNumber)
                              ? registerDTO.PhoneNumber
                              : null,  // Default value is null when PhoneNumber is not provided
                ProfilePictureUrl = profilePictureUrl,  // Use the uploaded or default profile picture URL
                PreferredLanguage = !string.IsNullOrWhiteSpace(registerDTO.PreferredLanguage)
                            ? registerDTO.PreferredLanguage
                            : "English",  // Default value for Preferred Language
                PreferredCurrency = !string.IsNullOrWhiteSpace(registerDTO.PreferredCurrency)
                            ? registerDTO.PreferredCurrency
                            : "INR"  // Default value for Preferred Currency
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            // If user registration is successful
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully. Please login!" });
            }

            // If registration fails, return the errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }

            return BadRequest(ModelState);
        }



        // POST: /api/auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            // Try to find the user by email or username
            var user = await userManager.FindByEmailAsync(model.EmailOrUsername) ??
                       await userManager.FindByNameAsync(model.EmailOrUsername);

            if (user != null)
            {
                // Validate the password
                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    // Get roles assigned to the user
                    var roles = await userManager.GetRolesAsync(user);

                    // If the user has roles, create a JWT token
                    if (roles != null)
                    {
                        // Generate JWT token using the token repository
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList(),
                                                                      "https://localhost:7290",
                                                                      "https://localhost:7290");

                        // Create the response object to send back to the client
                        var response = new LoginResponseDTO
                        {
                            JWTToken = jwtToken,
                            
                            UserProfile = new UserProfile
                            {
                                UserID = user.Id,
                                Username = user.UserName,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                PhoneNumber = user.PhoneNumber,
                                ProfilePictureUrl = user.ProfilePictureUrl,
                                PreferredLanguage = user.PreferredLanguage,
                                PreferredCurrency = user.PreferredCurrency
                            },
                            Username = user.UserName,  
                        };

                        // Return the response object
                        return Ok(response);
                    }
                }
            }

            // If login fails, return an unauthorized response
            return Unauthorized(new { message = "Username or password is incorrect." });
        }

        // UPDATE: /api/auth/update
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequestDTO updateUserRequest)
        {
            // Find the user by their ID
            var user = await userManager.FindByIdAsync(updateUserRequest.UserID);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Flag to track if user details were updated
            bool isUserUpdated = false;

            // Update Username and Email
            if (!string.IsNullOrEmpty(updateUserRequest.Username) && updateUserRequest.Username != user.UserName)
            {
                user.UserName = updateUserRequest.Username;
                isUserUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUserRequest.Email) && updateUserRequest.Email != user.Email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, updateUserRequest.Email);
                if (!setEmailResult.Succeeded)
                {
                    return BadRequest(new { message = "Failed to update email.", errors = setEmailResult.Errors.Select(e => e.Description) });
                }
                isUserUpdated = true;
            }

            // Update Phone Number
            if (string.IsNullOrEmpty(updateUserRequest.PhoneNumber))
            {
                user.PhoneNumber = null;  // Set to null if phone number is not provided
            }
            else if (updateUserRequest.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = updateUserRequest.PhoneNumber;
                isUserUpdated = true;
            }

            // Update FirstName and LastName
            if (!string.IsNullOrEmpty(updateUserRequest.FirstName) && updateUserRequest.FirstName != user.FirstName)
            {
                user.FirstName = updateUserRequest.FirstName;
                isUserUpdated = true;
            }

            if (!string.IsNullOrEmpty(updateUserRequest.LastName) && updateUserRequest.LastName != user.LastName)
            {
                user.LastName = updateUserRequest.LastName;
                isUserUpdated = true;
            }

            // Update Profile Picture
            string profilePictureUrl = "https://res.cloudinary.com/dpd6oloy8/image/upload/v1729058704/DefualtProfilePicture_atiafh.png"; // Default image URL

            // Handle file upload if a profile picture is provided in the DTO
            if (updateUserRequest.ProfilePicture != null && updateUserRequest.ProfilePicture.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "user-profile-images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate a unique filename for the uploaded profile picture
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateUserRequest.ProfilePicture.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                // Save the uploaded file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateUserRequest.ProfilePicture.CopyToAsync(stream);
                }

                // Set the profile picture URL to the file path, accessible by the frontend
                profilePictureUrl = $"http://localhost:5027/user-profile-images/{fileName}";
            }
            user.ProfilePictureUrl = profilePictureUrl;


            // Update Preferred Language
            if (!string.IsNullOrEmpty(updateUserRequest.PreferredLanguage) && updateUserRequest.PreferredLanguage != user.PreferredLanguage)
            {
                user.PreferredLanguage = updateUserRequest.PreferredLanguage;
                isUserUpdated = true;
            } else
            {
                user.PreferredLanguage = "English";
                isUserUpdated = true;
            }

            // Update Preferred Currency
            if (!string.IsNullOrEmpty(updateUserRequest.PreferredCurrency) && updateUserRequest.PreferredCurrency != user.PreferredCurrency)
            {
                user.PreferredCurrency = updateUserRequest.PreferredCurrency;
                isUserUpdated = true;
            } else
            {
                user.PreferredCurrency = "INR";
                isUserUpdated = true;
            }

            // Update Password (if requested)
            if (!string.IsNullOrEmpty(updateUserRequest.OldPassword) && !string.IsNullOrEmpty(updateUserRequest.NewPassword))
            {
                // Check if the old password is correct
                var passwordCheck = await userManager.CheckPasswordAsync(user, updateUserRequest.OldPassword);
                if (!passwordCheck)
                {
                    return BadRequest(new { message = "Old password is incorrect." });
                }

                // Change the password
                var changePasswordResult = await userManager.ChangePasswordAsync(user, updateUserRequest.OldPassword, updateUserRequest.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    return BadRequest(new { message = "Failed to update password.", errors = changePasswordResult.Errors.Select(e => e.Description) });
                }
            }

            // Save updates if any of the user details were updated
            if (isUserUpdated)
            {
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return BadRequest(new { message = "Failed to update user details.", errors = updateResult.Errors.Select(e => e.Description) });
                }
            }

            // Successfully updated
            return Ok(new { message = "User updated successfully." });
        }



        //private readonly UserManager<User> userManager;
        //private readonly ITokenRepository tokenRepository;

        //public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository)
        //{
        //    this.userManager = userManager;
        //    this.tokenRepository = tokenRepository;
        //}

        //// POST: /api/auth/register
        //[HttpPost]
        //[Route("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        //{
        //    // Check if the user already exists by email
        //    var existingUser = await userManager.FindByEmailAsync(registerRequestDTO.Username);

        //    if (existingUser != null)
        //    {
        //        return BadRequest(new { message = "User with this email or username already exists. Please login." });
        //    }

        //    // Create a new user instance
        //    var user = new User
        //    {
        //        Name = registerRequestDTO.Name,
        //        UserName = registerRequestDTO.Username, // Username set to email
        //        Email = registerRequestDTO.Username,
        //        PhoneNumber = registerRequestDTO.PhoneNumber,
        //    };

        //    var userResult = await userManager.CreateAsync(user, registerRequestDTO.Password);

        //    if (userResult.Succeeded)
        //    {
        //        // If roles are provided, add them to the user
        //        if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
        //        {
        //            var roleResult = await userManager.AddToRolesAsync(user, registerRequestDTO.Roles);

        //            if (!roleResult.Succeeded)
        //            {
        //                return BadRequest(new
        //                {
        //                    message = $"User registered, but role assignment failed.",
        //                    errors = roleResult.Errors.Select(e => e.Description)
        //                });
        //            }

        //            return Ok(new { message = "User registered successfully with roles! Please log in." });
        //        }

        //        return Ok(new { message = "User registered successfully without any roles! Please log in." });
        //    }

        //    return BadRequest(new
        //    {
        //        message = "User registration failed.",
        //        errors = userResult.Errors.Select(e => e.Description)
        //    });
        //}

        //// POST: /api/auth/login
        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        //{
        //    // Try to find the user by email
        //    var user = await userManager.FindByEmailAsync(loginRequestDTO.userNameOrEmail);

        //    if (user != null)
        //    {
        //        // Validate the password
        //        var validPassword = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

        //        if (validPassword)
        //        {
        //            // Get roles assigned to the user
        //            var roles = await userManager.GetRolesAsync(user);

        //            // If the user has roles, create a JWT token
        //            if (roles != null)
        //            {
        //                var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList(), "https://localhost:7290", "https://localhost:7290");

        //                var response = new LoginResponseDTO
        //                {
        //                    Name = loginRequestDTO.userNameOrEmail,
        //                    Username = user.Name,
        //                    JWTToken = jwtToken,
        //                    UserID = user.Id
        //                 };

        //                return Ok(response);
        //            }
        //        }
        //    }

        //    // Return 401 Unauthorized if the credentials are invalid
        //    return Unauthorized(new { message = "Username or password is incorrect." });
        //}

        ////UPDATE: /api/auth/update
        //[HttpPut]
        //[Route("Update")]
        //public async Task<IActionResult> Update([FromBody] UpdateUserRequestDTO updateUserRequest)
        //{
        //    // Find the user by their ID
        //    var user = await userManager.FindByIdAsync(updateUserRequest.UserID);

        //    if (user == null)
        //    {
        //        return NotFound(new { message = "User not found." });
        //    }

        //    // Update Username and Email
        //    bool isUserUpdated = false;
        //    if (!string.IsNullOrEmpty(updateUserRequest.Username) && updateUserRequest.Username != user.UserName)
        //    {
        //        user.UserName = updateUserRequest.Username;
        //        user.Name = updateUserRequest.Username;
        //        var updateResult = await userManager.UpdateAsync(user);
        //        if (!updateResult.Succeeded)
        //        {
        //            return BadRequest(new { message = "Failed to update user details.", errors = updateResult.Errors.Select(e => e.Description) });
        //        }
        //        isUserUpdated = true;
        //    }

        //    // Update Password
        //    if (!string.IsNullOrEmpty(updateUserRequest.OldPassword) && !string.IsNullOrEmpty(updateUserRequest.NewPassword))
        //    {
        //        // Check if the old password is correct
        //        var passwordCheck = await userManager.CheckPasswordAsync(user, updateUserRequest.OldPassword);
        //        if (!passwordCheck)
        //        {
        //            return BadRequest(new { message = "Old password is incorrect." });
        //        }

        //        // Change the password
        //        var changePasswordResult = await userManager.ChangePasswordAsync(user, updateUserRequest.OldPassword, updateUserRequest.NewPassword);
        //        if (!changePasswordResult.Succeeded)
        //        {
        //            return BadRequest(new { message = "Failed to update password.", errors = changePasswordResult.Errors.Select(e => e.Description) });
        //        }
        //    }

        //    // Successfully updated
        //    return Ok(new { message = "User updated successfully." });
        //}
    }
}
