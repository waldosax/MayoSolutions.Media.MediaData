using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models
{
    public class TheTvDbSeriesImageResults
    {
        [JsonProperty("poster")]
        public TheTvDbSeriesImageResult Poster { get; set; }
        [JsonProperty("fanart")]
        public TheTvDbSeriesImageResult FanArt { get; set; }
        [JsonProperty("series")]
        public TheTvDbSeriesImageResult Series { get; set; }
        [JsonProperty("season")]
        public TheTvDbSeriesImageResult Season { get; set; }
        [JsonProperty("seasonwide")]
        public TheTvDbSeriesImageResult Seasonwide { get; set; }
    }
}