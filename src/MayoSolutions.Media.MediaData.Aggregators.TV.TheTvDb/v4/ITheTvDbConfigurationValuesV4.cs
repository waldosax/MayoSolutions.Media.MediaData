namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    public interface ITheTvDbConfigurationValuesV4
    {
        string ApiBaseUrl { get; }
        string ApiKey { get; }
        string ApiPin { get; }
        int AuthTokenCacheDurationInHours { get; }
    }
}