using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    [DebuggerDisplay("{SourceName,nq} {Id,nq}")]
    public class RemoteId
    {
        public string Id { get; set; }
        public long Type { get; set; }
        public string SourceName { get; set; }
    }
}