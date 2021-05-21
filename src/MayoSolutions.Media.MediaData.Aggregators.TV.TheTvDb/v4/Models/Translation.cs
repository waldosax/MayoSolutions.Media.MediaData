using System.Collections.Generic;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    public class Translation
    {
        public List<string> Aliases { get; set; }
        public bool? IsAlias { get; set; }
        public bool IsPrimary { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
    }
}
