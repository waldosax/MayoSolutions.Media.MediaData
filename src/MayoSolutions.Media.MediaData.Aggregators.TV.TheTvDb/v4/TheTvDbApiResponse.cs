using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    /// <summary>
    /// Generic wrapper for every api response.
    /// </summary>
    /// <typeparam name="TDATA"></typeparam>
    internal class TheTvDbApiResponse<TDATA>
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public TDATA Data { get; set; }

        public TheTvDbApiPagingLinks Links { get; set; }
    }

    internal class TheTvDbApiPagingLinks
    {
        [JsonProperty("prev")]
        public string Previous { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
