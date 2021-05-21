using System;
using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models
{
    public class TheTvDbSeries : ISeriesDescriptor, ISeriesIdentifier
    {
        public string[] Aliases { get; set; }
        public string Banner { get; set; }
        public string FirstAired { get; set; }
        public string[] Genre { get; set; }
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public string LastUpdated { get; set; }
        public string Network { get; set; }
        public string NetworkId { get; set; }
        public string Overview { get; set; }
        public string Rating { get; set; }
        public string Runtime { get; set; }
        public string SeriesId { get; set; }
        public string SeriesName { get; set; }
        public float? SiteRating { get; set; }
        public int? SiteRatingCount { get; set; }
        public string Status { get; set; }
        public string Zap2ItId { get; set; }

        string ISeriesDescriptor.Name
        {
            get => SeriesName;
            set => SeriesName = value;
        }

        int? ISeriesDescriptor.Year
        {
            get => AirDate?.Year;
            set { }
        }

        internal DateTime? AirDate => FirstAired.ToTvDbDate();

        string ISeriesIdentifier.Name
        {
            get => SeriesName;
            set => SeriesName = value;
        }
        string ISeriesDescriptor.Description
        {
            get => Overview;
            set => Overview = value;
        }

        SeriesImageUrls ISeriesDescriptor.ImageUrls { get; } = new SeriesImageUrls();

        string ISeriesIdentifier.Id
        {
            get => Id.ToString();
            set => Id = int.Parse(value);
        }
    }
}