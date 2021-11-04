using System.Collections.Generic;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    public class SeriesExtendedRecord : SeriesBaseRecord
    {
        public SeriesAirsDays AirsDays { get; set; }
        public string AirsTime { get; set; }
        public string AirsTimeUtc { get; set; }

        [JsonProperty("artworks")]
        public List<ArtworkBaseRecord> Artwork { get; set; }

        public List<Character> Characters { get; set; }
        public List<Company> Companies { get; set; }

        public List<FranchiseBaseRecord> Franchises { get; set; }
        public List<GenreBaseRecord> Genres { get; set; }
        public List<ListBaseRecord> Lists { get; set; }

        public List<NetworkBaseRecord> Networks { get; set; }
        public NetworkBaseRecord LatestNetwork { get; set; }
        public NetworkBaseRecord OriginalNetwork { get; set; }

        public List<RemoteId> RemoteIds { get; set; }
        public List<SeasonBaseRecord> Seasons { get; set; }
        public List<Trailer> Trailers { get; set; }

        public Translations Translations { get; set; }
    }
}