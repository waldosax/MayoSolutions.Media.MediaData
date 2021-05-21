using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("Season {Number} ({Type.Name,nq})")]
    public class SeasonExtendedRecord : SeasonBaseRecord
    {
        public List<ArtworkBaseRecord> Artwork { get; set; }
        public List<EpisodeBaseRecord> Episodes { get; set; }
        public List<Trailer> Trailers { get; set; }
    }
}