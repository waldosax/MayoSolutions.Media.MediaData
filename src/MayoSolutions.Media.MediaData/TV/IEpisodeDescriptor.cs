using System;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// Base information about an episode.
    /// </summary>
    public interface IEpisodeDescriptor
    {
        /// <summary>Season in which this episode aired.</summary>
        int SeasonNumber { get; set; }

        /// <summary>Episode number within the season.</summary>
        int EpisodeNumber { get; set; }

        /// <summary>Episode title.</summary>
        string Title { get; set; }

        /// <summary>Episode description.</summary>
        string Description { get; set; }
        
        /// <summary>Date which the episode first aired.</summary>
        DateTime? AirDate { get; set; }
    }
}