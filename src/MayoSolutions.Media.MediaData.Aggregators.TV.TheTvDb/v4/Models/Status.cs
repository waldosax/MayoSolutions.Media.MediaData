using System.Diagnostics;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class Status
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("keepUpdated")]
        public bool KeepUpdated { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("recordType")]
        public string RecordType { get; set; }
    }
}