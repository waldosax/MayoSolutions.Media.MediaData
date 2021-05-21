using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Image,nq}")]
    public class ArtworkBaseRecord
    {
        public long Id { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public float? Score { get; set; }
        public string Thumbnail { get; set; }
        public long Type { get; set; }
    }
}