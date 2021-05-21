using System;

namespace MayoSolutions.Media.MediaData
{
    public interface IMediaExtendedInfo
    {
        string[] Genres { get; set; }
        string[] Networks { get; set; } //TODO: Production Companies
        string Status { get; set; }
    }
}
