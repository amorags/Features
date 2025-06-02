using Microsoft.AspNetCore.Mvc;
using MoviesService.Services;

namespace MoviesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            var premium = Request.Headers["X-FeatureHub-premium-movies"] == "true";
            var movies = _movieService.GetMovies(premium);
            return Ok(movies);
        }
    }
}
