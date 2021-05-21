using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq} played by {PersonName,nq}")]
    public class Character
    {
        public List<Alias> Aliases { get; set; }
        public long? EpisodeId { get; set; }
        public long Id { get; set; }
        public string Image { get; set; }
        public bool IsFeatured { get; set; }
        public long? MovieId { get; set; }
        public string Name { get; set; }
        public List<string> NameTranslations { get; set; }
        public List<string> OverviewTranslations { get; set; }
        public long? PeopleId { get; set; }
        public long? SeriesId { get; set; }
        public long Sort { get; set; }
        public long Type { get; set; }
        public string Url { get; set; }
        public string PeopleType { get; set; }
        public string PersonName { get; set; }
    }
}