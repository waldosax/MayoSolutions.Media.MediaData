using System;

namespace MayoSolutions.Media.MediaData
{
    public interface IHasReleaseDate
    {
        DateTime? ReleaseDate { get; set; }
    }
}