namespace MayoSolutions.Media.MediaData.Movies
{
    /// <summary>
    /// Base information about a movie.
    /// </summary>
    public interface IMovieDescriptor
    {

        /// <summary>Movie name.</summary>
        string Name { get; set; }

        /// <summary>Movie description.</summary>
        string Description { get; set; }
    }
}