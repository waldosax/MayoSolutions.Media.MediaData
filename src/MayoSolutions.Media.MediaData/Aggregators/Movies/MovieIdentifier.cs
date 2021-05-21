using MayoSolutions.Media.MediaData.Movies;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies
{
    public class MovieIdentifier : IMovieIdentifier
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ImdbId { get; set; }
    }
}