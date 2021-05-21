using System.Diagnostics;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{Name,nq}")]
    public class CompanyType
    {
        [JsonProperty("companyTypeId")]
        public long Id { get; set; }

        [JsonProperty("companyTypeName")]
        public string Name { get; set; }
    }
}