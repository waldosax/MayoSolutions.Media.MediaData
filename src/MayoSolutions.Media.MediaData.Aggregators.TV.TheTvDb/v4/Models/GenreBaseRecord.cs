using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class GenreBaseRecord
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}