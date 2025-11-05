using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoodDeedsController : ControllerBase
    {
        private readonly ILogger<GoodDeedsController> _logger;

        private static readonly List<GoodDeedDto> _goodDeeds = Enumerable.Repeat(new GoodDeedDto()
        {
            Title = "Good deed",
            Description = "LoremIpsum"
        }, 20).ToList();

        public GoodDeedsController(ILogger<GoodDeedsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST /api/gooddeeds
        /// </summary>
        [HttpPost]
        public IActionResult CreateGoodDeed([FromBody] GoodDeedCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deed = new GoodDeedDto
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            _goodDeeds.Add(deed);

            return Ok();
        }

        [HttpGet("paged")]
        public ActionResult<PagedResponse<GoodDeedDto>> GetPaged([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var skip = page * pageSize;

            var items = _goodDeeds
                .OrderByDescending(x => x.CreatedAt)
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
}
