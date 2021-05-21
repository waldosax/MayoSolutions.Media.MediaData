using System;

namespace MayoSolutions.Media.MediaData
{
    public interface IHasAirDate
    {
        DateTime? AirDate { get; set; }
    }
}