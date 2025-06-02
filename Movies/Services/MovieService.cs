using FeatureHubSDK;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesService.Services
{
    public class MovieService
    {
        private readonly IClientContext _context;
    

        public MovieService(IClientContext context)
        {
            _context = context;
            
        }

        public async Task<object> GetMoviesAsync()
        {
            // WASTEFUL: Always recompute, no caching
            var featureState = _context["PremiumMovies"]; // Assuming similar feature
            bool isPremium = featureState?.BooleanValue ?? false;

            // WASTEFUL: Simulate complex database queries with delays
            await Task.Delay(500); // Artificial delay

            // WASTEFUL: Generate excessive data with poor time complexity
            var movies = await GenerateWastefulMovieData(isPremium);
            var analytics = await GenerateWastefulAnalytics();

            return new
            {
                movies,
                analytics,
                processingTime = DateTime.UtcNow,
                carbonFootprint = "excessive",
                dataProcessingComplexity = "O(n³)",
                resourceUsage = "maximum"
            };
        }

        private async Task<List<object>> GenerateWastefulMovieData(bool isPremium)
        {
            var movies = new List<object>();
            var random = new Random();

            // WASTEFUL: O(n²) complexity for no reason
            var baseMovies = isPremium
                ? new[] { "Premium Movie 1", "Premium Movie 2", "Premium Movie 3" }
                : new[] { "Free Movie 1", "Free Movie 2" };

            for (int i = 0; i < baseMovies.Length; i++)
            {
                // WASTEFUL: Nested loops for unnecessary computation
                var ratings = new List<double>();
                for (int j = 0; j < 1000; j++)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        ratings.Add(random.NextDouble() * 5);
                    }
                }

                movies.Add(new
                {
                    title = baseMovies[i],
                    ratings = ratings.Take(10).ToList(), // Only use 10, but computed 100k
                    metadata = GenerateExcessiveMetadata(),
                    reviews = await GenerateFakeReviews(100),
                    recommendedMovies = await GetRecommendations(50)
                });
            }

            return movies;
        }

        private object GenerateExcessiveMetadata()
        {
            var random = new Random();
            return new
            {
                technicalSpecs = Enumerable.Range(0, 50).Select(i => new
                {
                    property = $"spec_{i}",
                    value = random.Next(1, 1000),
                    computedHash = Guid.NewGuid().ToString()
                }).ToList(),
                colorAnalysis = Enumerable.Range(0, 100).Select(i => new
                {
                    frame = i,
                    dominantColor = $"#{random.Next(0x1000000):X6}",
                    computationTime = random.NextDouble() * 1000
                }).ToList()
            };
        }

        private async Task<List<string>> GenerateFakeReviews(int count)
        {
            await Task.Delay(100); // Artificial delay
            var reviews = new List<string>();
            var adjectives = new[] { "amazing", "terrible", "mediocre", "outstanding", "boring" };
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                reviews.Add($"This movie is {adjectives[random.Next(adjectives.Length)]} - Review #{i}");
            }

            return reviews;
        }

        private async Task<List<string>> GetRecommendations(int count)
        {
            await Task.Delay(200); // Artificial delay
            return Enumerable.Range(1, count)
                .Select(i => $"Recommended Movie {i}")
                .ToList();
        }

        private async Task<object> GenerateWastefulAnalytics()
        {
            await Task.Delay(300); // Artificial delay
            var random = new Random();

            return new
            {
                userEngagement = Enumerable.Range(0, 1000).Select(i => new
                {
                    userId = Guid.NewGuid(),
                    watchTime = random.Next(0, 7200),
                    interactions = random.Next(0, 100),
                    deviceInfo = new
                    {
                        browser = "Chrome",
                        os = "Windows",
                        screenResolution = "1920x1080",
                        unnecessaryHash = Guid.NewGuid().ToString()
                    }
                }).ToList(),
                performanceMetrics = new
                {
                    cpuUsage = random.NextDouble() * 100,
                    memoryUsage = random.NextDouble() * 8192,
                    networkLatency = random.NextDouble() * 1000,
                    wastedCycles = random.Next(1000000, 10000000)
                }
            };
        }
    }
}