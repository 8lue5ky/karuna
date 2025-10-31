using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
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

        /// <summary>
        /// Empfängt MultipartFormData (Text + Bild) vom Frontend
        /// </summary>
        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

                _logger.LogInformation("Profil-Update von {User}: {@Profile}", dto.Username, dto);
                _logger.LogInformation("Gespeichertes Bild: {File}", savedFilePath ?? "Kein Bild");

                return Ok(new
                {
                    message = "Profil erfolgreich gespeichert",
                    profile = dto,
                    imageUrl = savedFilePath
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Profil-Update");
                return StatusCode(500, new { message = "Interner Serverfehler", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Dient als DTO für Formulardaten aus MudBlazor
    /// </summary>
    public class ProfileUpdateDto
    {
        [FromForm(Name = "Username")]
        public string? Username { get; set; }

        [FromForm(Name = "Email")]
        public string? Email { get; set; }

        [FromForm(Name = "Bio")]
        public string? Bio { get; set; }

        [FromForm(Name = "Location")]
        public string? Location { get; set; }

        [FromForm(Name = "Gender")]
        public string? Gender { get; set; }

        [FromForm(Name = "IsPublic")]
        public bool IsPublic { get; set; }

        [FromForm(Name = "ProfileImage")]
        public IFormFile? ProfileImage { get; set; }
    }
}
