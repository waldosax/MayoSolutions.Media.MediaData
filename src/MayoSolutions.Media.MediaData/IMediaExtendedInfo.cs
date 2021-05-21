using System;

namespace MayoSolutions.Media.MediaData
{
    public interface IMediaExtendedInfo
    {
        DateTime? AirDate { get; set; }
        string[] Genres { get; set; }
        string[] Networks { get; set; }

        [Obsolete("Use Networks property.", false)]
        string Network { get; set; }

        string Status { get; set; }
    }
}
