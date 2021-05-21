using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class Company
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<string> NameTranslations { get; set; }
        public List<string> OverviewTranslations { get; set; }
        public List<Alias> Aliases { get; set; }
        public string Country { get; set; }
        public long PrimaryCompanyType { get; set; }
        public string ActiveDate { get; set; }
        public string InactiveDate { get; set; }
        public CompanyType CompanyType { get; set; }
    }
}