using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// Season containing episodes for the series.
    /// </summary>
    [DebuggerDisplay("Season {" + nameof(SeasonNumber) + ",nq}")]
    public class Season
    {
        /// <summary>Series containing the season.</summary>
        [JsonIgnore]
        public Series Series { get; internal set; }

        /// <summary>Season Number.</summary>
        /// <remarks>Season 0 indicates Specials/pre-season episodes.</remarks>
        public int SeasonNumber { get; set; }

        /// <summary>Episodes which aired this season.</summary>
        public List<Episode> Episodes { get; } = new List<Episode>();
    }
}