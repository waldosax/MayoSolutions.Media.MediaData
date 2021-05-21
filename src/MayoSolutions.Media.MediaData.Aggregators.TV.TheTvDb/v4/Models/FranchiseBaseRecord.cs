using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class FranchiseBaseRecord
    {
        public List<string> Aliases { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> NameTranslations { get; set; }
        public List<string> OverviewTranslations { get; set; }
    }
}