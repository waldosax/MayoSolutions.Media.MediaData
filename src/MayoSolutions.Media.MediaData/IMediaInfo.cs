using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayoSolutions.Media.MediaData
{
    public interface IMediaInfo
    {
        /// <summary>Identifier.</summary>
        string Id { get; set; }

        /// <summary>Name.</summary>
        string Name { get; set; }

        /// <summary>Year of release (the first air date of the first episode).</summary>
        int? Year { get; set; }

        /// <summary>Blurb about the series.</summary>
        string Description { get; set; }

        /// <summary>The imdb.com ID for the series.</summary>
        string ImdbId { get; set; }
    }
}
