namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models
{
    public class TheTvDbEpisodeResult
    {
        public TheTvDbPaging Links { get; set; }
        public TheTvDbEpisode[] Data { get; set; }
        public TheTvDbQueryError Errors { get; set; }
    }
}