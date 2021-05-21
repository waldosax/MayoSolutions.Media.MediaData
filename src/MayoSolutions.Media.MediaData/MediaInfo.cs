namespace MayoSolutions.Media.MediaData
{
    /// <inheritdoc cref="IMediaInfo" />
    internal class MediaInfo : IMediaInfo
    {
        /// <inheritdoc cref="IMediaInfo" />
        public string Id { get; set; }

        /// <inheritdoc cref="IMediaInfo" />
        public string Name { get; set; }

        /// <inheritdoc cref="IMediaInfo" />
        public int? Year { get; set; }

        /// <inheritdoc cref="IMediaInfo" />
        public string Description { get; set; }

        /// <inheritdoc cref="IMediaInfo" />
        public string ImdbId { get; set; }
    }
}
