namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    public interface ITheTvDbConfigurationValues
    {
        string ApiBaseUrl { get; }
        string ApiKey { get; }
        int AuthTokenCacheDurationInHours { get; }
        string ArtworkBaseUrl { get; }
    }
}