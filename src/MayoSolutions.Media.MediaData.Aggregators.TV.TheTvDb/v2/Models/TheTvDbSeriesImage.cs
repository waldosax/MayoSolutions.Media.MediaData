namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models
{
    public class TheTvDbSeriesImage
    {
        public string FileName { get; set; }
        public int? Id { get; set; }
        public string KeyType { get; set; }
        public int? LanguageId { get; set; }
        public TheTvDbSeriesImageRatingsInfo RatingsInfo { get; set; }
        public string Resolution { get; set; }
        public string Subkey { get; set; }
        public string Thumbnail { get; set; }
    }
}
