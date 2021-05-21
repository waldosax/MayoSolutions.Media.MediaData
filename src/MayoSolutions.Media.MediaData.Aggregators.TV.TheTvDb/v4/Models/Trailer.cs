using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class Trailer
    {
        public long Id { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}