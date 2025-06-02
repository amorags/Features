using Microsoft.AspNetCore.Mvc;
using ShowsService.Services;

namespace ShowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly ShowService _showService;

        public ShowsController(ShowService showService)
        {
            _showService = showService;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetShows()
        {
            var result = await _showService.GetShowsAsync();
            
            // GREEN: Add efficiency headers
            Response.Headers.Add("X-Carbon-Efficient", "true");
            Response.Headers.Add("X-Cache-Strategy", "enabled");
            
            return Ok(result);
        }

        [HttpGet("health")]
        public ActionResult GetHealth()
        {
            return Ok(new { 
                status = "healthy", 
                carbonEfficient = true,
                idle = _showService.IsIdle()
            });
        }
    }
}