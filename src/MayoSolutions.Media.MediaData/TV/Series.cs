using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// A TV series.
    /// </summary>
    [DebuggerDisplay("{SeriesIdentifier.Name,nq} ({Year?.ToString()??\"?\",nq})")]
    public class Series : ISeriesDescriptor
    {
        /// <summary>Series Identifier.</summary>
        public string Id { get; set; }

        /// <summary>Series name.</summary>
        public string Name { get; set; }

        /// <summary>Year of release (the first air date of the first episode).</summary>
        public int? Year { get; set; }

        /// <summary>Blurb about the series.</summary>
        public string Description { get; set; }

        /// <summary>The imdb.com ID for the series.</summary>
        public string ImdbId { get; set; }

        /// <summary>Seasons containing episodes for the series.</summary>
        public List<Season> Seasons { get; } = new List<Season>();


        /// <summary>Metadata about a series.</summary>
        public SeriesInfo SeriesInfo { get; set; } = new SeriesInfo();
        
        /// <summary>Additional data used for displaying a series.</summary>
        public SeriesImageUrls ImageUrls { get; set; } = new SeriesImageUrls();
    }
}
