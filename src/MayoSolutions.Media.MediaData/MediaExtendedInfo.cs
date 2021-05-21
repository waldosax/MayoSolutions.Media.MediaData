using System;
using System.Linq;

namespace MayoSolutions.Media.MediaData
{
    /// <summary>
    /// Metadata about a series.
    /// </summary>
    internal class MediaExtendedInfo :IMediaExtendedInfo
    {
        public string[] Genres { get; set; } = new string[0];
        public string[] Networks { get; set; } = new string[0];

        public string Status { get; set; }
    }
}