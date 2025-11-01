using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoodDeedsController : ControllerBase
    {
        private readonly ILogger<GoodDeedsController> _logger;

        // Simulierte In-Memory-Liste (kann später durch DB ersetzt werden)
        private static readonly List<GoodDeedDto> _goodDeeds = new();

        public GoodDeedsController(ILogger<GoodDeedsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST /api/gooddeeds
        /// Erstellt einen neuen Eintrag für eine gute Tat.
        /// </summary>
        [HttpPost]
        public IActionResult CreateGoodDeed([FromBody] GoodDeedCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deed = new GoodDeedDto
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                CreatedAt = DateTime.UtcNow
            };

            _goodDeeds.Add(deed);
            _logger.LogInformation("Neue gute Tat erstellt: {@Deed}", deed);

            return Ok(new
            {
                message = "Danke für deine gute Tat 💖",
                deed
            });
        }

        /// <summary>
        /// GET /api/gooddeeds
        /// Gibt alle gespeicherten guten Taten zurück.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllGoodDeeds()
        {
            return Ok(_goodDeeds.OrderByDescending(x => x.CreatedAt));
        }
    }

    // ------------------- DTOs -------------------

    public class GoodDeedCreateRequest
    {
        [Required(ErrorMessage = "Ein Titel ist erforderlich.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Bitte beschreibe deine gute Tat.")]
        public string? Description { get; set; }

        public string? Category { get; set; }
    }

    public class GoodDeedDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
