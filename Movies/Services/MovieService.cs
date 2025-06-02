namespace MoviesService.Services
{
    public class MovieService
    {
        private readonly List<string> basicMovies = ["Movie A", "Movie B", "Movie C"];
        private readonly List<string> premiumMovies = ["Premium X", "Premium Y", "Premium Z"];

        public List<string> GetMovies(bool premiumEnabled)
        {
            var source = premiumEnabled ? premiumMovies : basicMovies;
            var result = new List<string>();

            // Simulate inefficient loop
            for (int i = 0; i < 10000; i++)
            {
                foreach (var movie in source)
                {
                    if (movie.StartsWith("M") || movie.StartsWith("P"))
                        result.Add(movie);
                }
            }

            return result.Distinct().ToList();
        }
    }
}

