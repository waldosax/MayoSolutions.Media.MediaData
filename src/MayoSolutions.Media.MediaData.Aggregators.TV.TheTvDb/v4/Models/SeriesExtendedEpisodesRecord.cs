using System.Collections.Generic;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    public class SeriesExtendedEpisodesRecord : SeriesExtendedRecord
    {
        public List<EpisodeBaseRecord> Episodes { get; set; }
    }
}