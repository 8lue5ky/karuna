using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IWebHostEnvironment env, ILogger<ProfileController> logger)
        {
            _env = env;
            _logger = logger;
        }


        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateDto dto)
        {
            //var user = User; // ClaimsPrincipal
            //var username = user.Identity.Name; // z. B. der Benutzername
            //var userId = user.FindFirst("sub")?.Value ?? user.FindFirst("id")?.Value; // je nach Claim
            //var email = user.FindFirst(ClaimTypes.Email)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? savedFilePath = null;

            try
            {
                if (dto.ProfileImage is not null && dto.ProfileImage.Length > 0)
                {
                    var uploadDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadDir);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ProfileImage.FileName)}";
                    var filePath = Path.Combine(uploadDir, fileName);

                    await using (var stream = System.IO.File.Create(filePath))
                    {
                        await dto.ProfileImage.CopyToAsync(stream);
                    }

                    savedFilePath = $"/uploads/{fileName}";
                }

                return Ok(new
                {
                    message = "Profile updated successfully!",
                    profile = dto,
                    imageUrl = savedFilePath
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during updating user profile.");
                return StatusCode(500, new { message = "Internal error.", error = ex.Message });
            }
        }
    }
}
