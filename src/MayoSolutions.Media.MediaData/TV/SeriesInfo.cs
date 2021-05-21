using System;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// Metadata about a series.
    /// </summary>
    public class SeriesInfo
    {
        public DateTime? AirDate { get; set; }
        public string[] Genres { get; set; } = new string[0];
        public string Network { get; set; }
        public string Status { get; set; }
    }
}