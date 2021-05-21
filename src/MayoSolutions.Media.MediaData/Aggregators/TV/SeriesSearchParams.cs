namespace MayoSolutions.Media.MediaData.Aggregators.TV
{
    /// <summary>
    /// Series search parameters.
    /// </summary>
    public class SeriesSearchParams
    {
        /// <summary>
        /// The Series Name to search.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Year disambiguator. Some series have the same name, like The Office, Archer, and Firefly
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// IMDB Id.
        /// </summary>
        public string ImdbId { get; set; }
    }
}