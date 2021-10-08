using System.Collections.Generic;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    public class SeriesEpisodesResult
    {
        [JsonProperty("series")]
        public SeriesExtendedRecord Series { get; set; }

        [JsonProperty("episodes")]
        public List<EpisodeBaseRecord> Episodes { get; set; }
    }
}