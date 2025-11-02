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
        public ActionResult<PagedResponse<GoodDeedDto>> GetPaged([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            if (page < 0 || pageSize <= 0)
                return BadRequest("Ungültige Paging-Parameter.");

            var skip = page * pageSize;
            var items = _goodDeeds
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            var hasMore = skip + pageSize < _goodDeeds.Count;

            return Ok(new PagedResponse<GoodDeedDto>
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

    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = new();
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
