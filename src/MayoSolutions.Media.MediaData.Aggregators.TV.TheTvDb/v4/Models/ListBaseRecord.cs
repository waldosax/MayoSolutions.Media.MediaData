using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class ListBaseRecord
    {
        public List<Alias> Aliases { get; set; } 
        public long Id { get; set; } 
        public bool IsOfficial { get; set; } 
        public string Name { get; set; } 
        public List<string> NameTranslations { get; set; } 
        public string Overview { get; set; }
        public List<string> OverviewTranslations { get; set; }
        public string Url { get; set; }
    }
}