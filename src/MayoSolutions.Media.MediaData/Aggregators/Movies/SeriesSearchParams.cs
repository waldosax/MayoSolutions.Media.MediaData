namespace MayoSolutions.Media.MediaData.Aggregators.Movies
{
    /// <summary>
    /// Movie search parameters.
    /// </summary>
    public class MovieSearchParams
    {
        /// <summary>
        /// The Series Name to search.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Year disambiguator.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// IMDB Id.
        /// </summary>
        public string ImdbId { get; set; }

        public bool IncludeAdult { get; set; }
    }
}