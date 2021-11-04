using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("Season {Number} ({Type.Name,nq})")]
    public class SeasonBaseRecord : ISeasonNumber
    {
        public string Abbreviation { get; set; }
        public string Country { get; set; }
        public long Id { get; set; }
        public string Image { get; set; }
        public long? ImageType { get; set; }
        public string Name { get; set; }
        public List<string> NameTranslations { get; set; }
        public long Number { get; set; }
        public List<string> OverviewTranslations { get; set; }
        public long SeriesId { get; set; }
        public string Slug { get; set; }
        public SeasonType Type { get; set; }
    }
}