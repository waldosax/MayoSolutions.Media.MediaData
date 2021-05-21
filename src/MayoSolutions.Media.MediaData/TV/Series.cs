using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MayoSolutions.Media.MediaData.TV
{
    /// <summary>
    /// A TV series.
    /// </summary>
    [DebuggerDisplay("{Name,nq} ({Year?.ToString()??\"?\",nq})")]
    public class Series : ISeriesIdentifier, ISeriesDescriptor, IMediaInfo, IMediaExtendedInfo, IHasReleaseDate
    {
        private readonly IMediaInfo _mediaInfo = new MediaInfo();
        private readonly IMediaExtendedInfo _mediaExtendedInfo = new MediaExtendedInfo();
        private DateTime? _releaseDate;

        /// <summary>Seasons containing episodes for the series.</summary>
        public List<Season> Seasons { get; } = new List<Season>();

        /// <summary>Additional data used for displaying a series.</summary>
        public SeriesImageUrls ImageUrls { get; set; } = new SeriesImageUrls();



        #region IMediaInfo

        public string Id
        {
            get => _mediaInfo.Id;
            set => _mediaInfo.Id = value;
        }

        public string Name
        {
            get => _mediaInfo.Name;
            set => _mediaInfo.Name = value;
        }

        public int? Year
        {
            get => _mediaInfo.Year;
            set => _mediaInfo.Year = value;
        }

        public string Description
        {
            get => _mediaInfo.Description;
            set => _mediaInfo.Description = value;
        }

        public string ImdbId
        {
            get => _mediaInfo.ImdbId;
            set => _mediaInfo.ImdbId = value;
        }

        #endregion

        #region IMediaExtendedInfo

        public string[] Genres
        {
            get => _mediaExtendedInfo.Genres;
            set => _mediaExtendedInfo.Genres = value;
        }

        public string[] Networks
        {
            get => _mediaExtendedInfo.Networks;
            set => _mediaExtendedInfo.Networks = value;
        }

        public string Status
        {
            get => _mediaExtendedInfo.Status;
            set => _mediaExtendedInfo.Status = value;
        }

        #endregion

        #region IHasReleaseDate

        public DateTime? ReleaseDate
        {
            get => _releaseDate;
            set
            {
                if (Year == null) Year = value?.Year;
                _releaseDate = value;
            }
        }

        #endregion

    }
}
