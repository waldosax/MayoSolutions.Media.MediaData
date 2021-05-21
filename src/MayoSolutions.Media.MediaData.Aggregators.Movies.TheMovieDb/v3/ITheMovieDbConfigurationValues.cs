namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3
{
    public interface ITheMovieDbConfigurationValues
    {
        string ApiBaseUrl { get; }
        string ApiKey { get; }
        int AuthTokenCacheDurationInHours { get; }
    }
}