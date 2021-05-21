namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// Identifying information about a series.
    /// </summary>
    public interface ISeriesIdentifier
    {
        /// <summary>Series name.</summary>
        string Name { get; set; }

        /// <summary>Canonical ID of the series.</summary>
        /// <remarks>Typically a tvdb.com ID.</remarks>
        string Id { get; set; }

        /// <summary>The imdb.com ID for the series.</summary>
        string ImdbId { get; set; }
    }
}