using System;
using System.Linq;

namespace MayoSolutions.Media.MediaData
{
    /// <summary>
    /// Metadata about a series.
    /// </summary>
    internal class MediaExtendedInfo :IMediaExtendedInfo
    {
        public DateTime? AirDate { get; set; }
        public string[] Genres { get; set; } = new string[0];
        public string[] Networks { get; set; } = new string[0];
        
        [Obsolete("Use Networks property.", false)]
        public string Network
        {
            get => Networks.FirstOrDefault();
            set
            {
                if (value == null)
                    Networks = new string[0];
                else
                    Networks = new[] {value};
            }
        }

        public string Status { get; set; }
    }
}