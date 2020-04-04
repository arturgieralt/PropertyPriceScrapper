using System.Threading.Tasks;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/offers")]
    public class OffersController : ControllerBase
    {
        private readonly ILogger<OffersController> _logger;
        private readonly OfferService _service;

        public OffersController(ILogger<OffersController> logger, OfferService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var offers = await _service.GetAllAsync();
            return Ok(offers);
        }

        [Route("aggregated")]
        [HttpGet]
        public async Task<IActionResult> GetAllAggregated()
        {
            var offers = await _service.GetAllAggregatedAsync();
            return Ok(offers);
        }
    }
}
