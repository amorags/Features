using Microsoft.AspNetCore.Mvc;
using ShowsService.Services;

namespace ShowsService.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly ShowService _showService;

        public ShowsController(ShowService showService)
        {
            _showService = showService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShows()
        {
            var premium = Request.Headers["X-FeatureHub-premium-shows"] == "true";
            var shows = await _showService.GetShowsAsync();
            return Ok(shows);
        }
    }
