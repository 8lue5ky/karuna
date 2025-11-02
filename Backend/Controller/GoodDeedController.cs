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
        private static readonly List<GoodDeedDto> _goodDeeds = Enumerable.Repeat(new GoodDeedDto()
        {
            Title = "Gute Tag",
            Description = "LoremIpsum"
        }, 500).ToList();

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

        [HttpGet("paged")]
        public ActionResult<PagedResponse> GetPaged([FromQuery] int startIndex = 0, [FromQuery] int count = 6)
        {
            Thread.Sleep(1000);

            if (startIndex < 0) startIndex = 0;
            if (count <= 0) count = 6;

            var items = _goodDeeds
                .Skip(startIndex)
                .Take(count)
                .ToList();

            bool hasMore = startIndex + count < _goodDeeds.Count;

            return Ok(new PagedResponse
            {
                Items = items,
                HasMore = hasMore
            });
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

    public class PagedResponse
    {
        public List<GoodDeedDto> Items { get; set; } = new();
        public bool HasMore { get; set; }
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
