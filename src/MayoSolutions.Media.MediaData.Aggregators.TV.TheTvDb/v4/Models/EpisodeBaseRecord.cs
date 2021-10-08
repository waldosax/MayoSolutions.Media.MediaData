using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Number} - {Name,nq}")]
    public class EpisodeBaseRecord
    {
        public string Aired { get; set; }
        public long Id { get; set; }
        public string Image { get; set; }
        public long? ImageType { get; set; }
        public bool IsMovie { get; set; }
        public string Name { get; set; }
        public List<string> NameTranslations { get; set; }
        public long Number { get; set; }
        public string Overview { get; set; }
        public List<string> OverviewTranslations { get; set; }
        public long? Runtime { get; set; }
        public long? SeasonNumber { get; set; }
        public List<SeasonBaseRecord> Seasons { get; set; }
        public long SeriesId { get; set; }
    }
}