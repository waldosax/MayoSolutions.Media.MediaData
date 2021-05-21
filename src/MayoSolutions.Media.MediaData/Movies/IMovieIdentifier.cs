namespace MayoSolutions.Media.MediaData.Movies
{
    /// <summary>
    /// Identifying information about a movie.
    /// </summary>
    public interface IMovieIdentifier
    {
        /// <summary>Series name.</summary>
        string Name { get; set; }

        /// <summary>Canonical ID of the movie.</summary>
        /// <remarks>Typically a theMovieDb.org ID.</remarks>
        string Id { get; set; }

        /// <summary>The imdb.com ID for the series.</summary>
        string ImdbId { get; set; }
    }
}
