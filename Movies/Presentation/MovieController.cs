using Microsoft.AspNetCore.Mvc;
using MoviesService.Services;

namespace MoviesService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieService movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetMovies()
        {
            // WASTEFUL: Excessive logging
            _logger.LogInformation("Processing movie request at {Time}", DateTime.UtcNow);
            _logger.LogDebug("User agent: {UserAgent}", Request.Headers.UserAgent);
            _logger.LogTrace("Request headers: {@Headers}", Request.Headers);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            var result = await _movieService.GetMoviesAsync();
            
            stopwatch.Stop();
            
            // WASTEFUL: Add resource-intensive headers
            Response.Headers.Add("X-Carbon-Efficient", "false");
            Response.Headers.Add("X-Processing-Time", stopwatch.ElapsedMilliseconds.ToString());
            Response.Headers.Add("X-Resource-Usage", "maximum");
            Response.Headers.Add("X-Unnecessary-Computations", "enabled");
            
            _logger.LogInformation("Request completed in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
            
            return Ok(result);
        }

        [HttpGet("health")]
        public ActionResult GetHealth()
        {
            // WASTEFUL: Even health check does unnecessary work
            var random = new Random();
            var wasteArray = new int[1000];
            for (int i = 0; i < wasteArray.Length; i++)
            {
                wasteArray[i] = random.Next();
            }
            Array.Sort(wasteArray);

            return Ok(new { 
                status = "healthy", 
                carbonEfficient = false,
                wastedComputations = wasteArray.Length,
                timestamp = DateTime.UtcNow
            });
        }
    }
}