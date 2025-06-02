using FeatureHubSDK;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowsService.Services
{
    public class ShowService
    {
        private readonly IClientContext _context;

        public ShowService(IClientContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetShowsAsync()
        {
            // Use indexer syntax to access feature
            var featureState = _context["PremiumShows"];
            bool isPremium = featureState?.BooleanValue ?? false;

            var shows = isPremium
                ? new List<string> { "Premium Show 1", "Premium Show 2" }
                : new List<string> { "Free Show 1", "Free Show 2" };

            return await Task.FromResult(shows);
        }
    }
}