using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV
{
    /// <inheritdoc cref="ISeriesIdentifier" />
    public class SeriesIdentifier : ISeriesIdentifier
    {
        /// <inheritdoc cref="ISeriesIdentifier" />
        public string Name { get; set; }
        /// <inheritdoc cref="ISeriesIdentifier" />
        public string Id { get; set; }
        /// <inheritdoc cref="ISeriesIdentifier" />
        public string ImdbId { get; set; }
    }
}
