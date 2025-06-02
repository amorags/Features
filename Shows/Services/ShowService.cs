using FeatureHubSDK;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowsService.Services
{
    public class ShowService
    {
        private readonly IClientContext _context;
        private readonly Dictionary<bool, List<string>> _cache; // GREEN: Cache results
        private DateTime _lastAccess;

        public ShowService(IClientContext context)
        {
            _context = context;
            _cache = new Dictionary<bool, List<string>>();
            _lastAccess = DateTime.UtcNow;
        }

        public async Task<object> GetShowsAsync()
        {
            _lastAccess = DateTime.UtcNow;
            
            // GREEN: Fast, cached response
            var featureState = _context["PremiumShows"];
            bool isPremium = featureState?.BooleanValue ?? false;

            if (_cache.TryGetValue(isPremium, out var cachedShows))
            {
                return new
                {
                    shows = cachedShows,
                    source = "cache",
                    timestamp = DateTime.UtcNow,
                    carbonFootprint = "minimal"
                };
            }

            // GREEN: Minimal data, O(1) complexity
            var shows = isPremium
                ? new List<string> { "Premium Show 1", "Premium Show 2" }
                : new List<string> { "Free Show 1", "Free Show 2" };

            _cache[isPremium] = shows;

            return new
            {
                shows,
                source = "computed",
                timestamp = DateTime.UtcNow,
                carbonFootprint = "minimal"
            };
        }

        public bool IsIdle() => DateTime.UtcNow - _lastAccess > TimeSpan.FromMinutes(5);
    }
}