using Microsoft.AspNetCore.Identity;
using Shared.DTOs.User;

namespace Backend.Controller.Users
{
    using Backend.Domain.Models.User;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var modelState = new ModelStateDictionary();

                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(error.Code, error.Description);
                }

                var details = new ValidationProblemDetails(modelState)
                {
                    Title = "Registration failed.",
                    Status = StatusCodes.Status400BadRequest
                };

                return BadRequest(details);
            }

            await OnUserRegistered(user);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.EmailOrUsername)
                       ?? await _userManager.FindByEmailAsync(request.EmailOrUsername);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password.");
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            return Ok();
        }

        private Task OnUserRegistered(AppUser user)
        {
            // Hier dein Hook
            Console.WriteLine($"Neuer Benutzer registriert: {user.UserName}");
            return Task.CompletedTask;
        }
    }

}
