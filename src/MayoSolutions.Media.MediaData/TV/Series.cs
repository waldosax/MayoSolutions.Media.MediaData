using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// A TV series.
    /// </summary>
    [DebuggerDisplay("{SeriesIdentifier.Name,nq} ({Year?.ToString()??\"?\",nq})")]
    public class Series : ISeriesIdentifier, ISeriesDescriptor, IMediaInfo, IMediaExtendedInfo
    {
        private IMediaInfo _mediaInfoImplementation = new MediaInfo();
        private IMediaExtendedInfo _mediaExtendedInfoImplementation = new MediaExtendedInfo();

        /// <summary>Seasons containing episodes for the series.</summary>
        public List<Season> Seasons { get; } = new List<Season>();

        /// <summary>Additional data used for displaying a series.</summary>
        public SeriesImageUrls ImageUrls { get; set; } = new SeriesImageUrls();



        #region IMediaInfo

        public string Id
        {
            get => _mediaInfoImplementation.Id;
            set => _mediaInfoImplementation.Id = value;
        }

        public string Name
        {
            get => _mediaInfoImplementation.Name;
            set => _mediaInfoImplementation.Name = value;
        }

        public int? Year
        {
            get => _mediaInfoImplementation.Year;
            set => _mediaInfoImplementation.Year = value;
        }

        public string Description
        {
            get => _mediaInfoImplementation.Description;
            set => _mediaInfoImplementation.Description = value;
        }

        public string ImdbId
        {
            get => _mediaInfoImplementation.ImdbId;
            set => _mediaInfoImplementation.ImdbId = value;
        }

        #endregion

        #region IMediaExtendedInfo

        public DateTime? AirDate
        {
            get => _mediaExtendedInfoImplementation.AirDate;
            set
            {
                Year = value == null ? (int?)null : value.Value.Year;
                _mediaExtendedInfoImplementation.AirDate = value;
            }
        }

        public string[] Genres
        {
            get => _mediaExtendedInfoImplementation.Genres;
            set => _mediaExtendedInfoImplementation.Genres = value;
        }

        public string[] Networks
        {
            get => _mediaExtendedInfoImplementation.Networks;
            set => _mediaExtendedInfoImplementation.Networks = value;
        }

        [Obsolete("Use Networks property.", false)]
        public string Network
        {
            get => _mediaExtendedInfoImplementation.Network;
            set => _mediaExtendedInfoImplementation.Network = value;
        }

        public string Status
        {
            get => _mediaExtendedInfoImplementation.Status;
            set => _mediaExtendedInfoImplementation.Status = value;
        }

        #endregion
    }
}
