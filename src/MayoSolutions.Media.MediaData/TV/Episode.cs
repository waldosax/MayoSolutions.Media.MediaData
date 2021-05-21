using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// TV Episode
    /// </summary>
    [DebuggerDisplay("{EpisodeNumber.ToString(\"00\"),nq} - {" + nameof(Title) + ",nq}")]
    public class Episode : IHasAirDate
    {
        /// <summary>Series containing this episode.</summary>
        [JsonIgnore]
        public Series Series { get; set; }

        /// <summary>Season containing this episode.</summary>
        [JsonIgnore]
        public Season Season { get; set; }

        /// <summary>Episode number within the season.</summary>
        public int EpisodeNumber { get; set; }

        /// <summary>Episode title.</summary>
        public string Title { get; set; }

        /// <summary>Date which the episode first aired.</summary>
        public DateTime? AirDate { get; set; }


        /// <summary>Episode description.</summary>
        public string Description { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public Dictionary<string, object> AdditionalData { get; set; }

        /// <summary>Unique episode identifier.</summary>
        [JsonIgnore]
        internal string Key => $"{Series.Id}s{Season?.SeasonNumber:00}e{EpisodeNumber:00}";
    }
}