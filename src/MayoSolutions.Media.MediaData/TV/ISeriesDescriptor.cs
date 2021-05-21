namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// Base information about a series.
    /// </summary>
    public interface ISeriesDescriptor
    {
        /// <summary>Series name.</summary>
        string Name { get; set; }

        /// <summary>Year of release (the first air date of the first episode).</summary>
        int? Year { get; set; }

        /// <summary>Blurb about the series.</summary>
        string Description { get; set; }

        /// <summary>The imdb.com ID for the series.</summary>
        string ImdbId { get; set; }

        /// <summary>Additional data used for displaying a series.</summary>
        SeriesImageUrls ImageUrls { get; }
    }
}