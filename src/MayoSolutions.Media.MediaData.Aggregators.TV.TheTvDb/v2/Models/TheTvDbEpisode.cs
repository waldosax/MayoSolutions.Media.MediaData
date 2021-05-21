using System;
using System.Globalization;
using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models
{
    public class TheTvDbEpisode : IEpisodeDescriptor
    {
        public int? AbsoluteNumber { get; set; }
        public int AiredEpisodeNumber { get; set; }
        public int AiredSeason { get; set; }
        public int? AirsAfterSeason { get; set; }
        public int? AirsBeforeEpisode { get; set; }
        public int? AirsBeforeSeason { get; set; }
        public string Director { get; set; }
        public string[] Directors { get; set; }
        public int? DvdChapter { get; set; }
        public int? DvdDiscId { get; set; }
        public int? DvdEpisodeNumber { get; set; }
        public int? DvdSeason { get; set; }
        public string EpisodeName { get; set; }
        public string Filename { get; set; }
        public string FirstAired { get; set; }
        public string[] GuestStars { get; set; }
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public int LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Overview { get; set; }
        public string ProductionCode { get; set; }
        public string SeriesId { get; set; }
        public string ShowUrl { get; set; }
        public float? SiteRating { get; set; }
        public int? SiteRatingCount { get; set; }
        public string ThumbAdded { get; set; }
        public int? ThumbAuthor { get; set; }
        public string ThumbHeight { get; set; }
        public string ThumbWidth { get; set; }
        public string[] Writers { get; set; }

        int IEpisodeDescriptor.SeasonNumber
        {
            get => AiredSeason;
            set => AiredSeason = value;
        }
        int IEpisodeDescriptor.EpisodeNumber
        {
            get => AiredEpisodeNumber;
            set => AiredEpisodeNumber = value;
        }
        string IEpisodeDescriptor.Title
        {
            get => EpisodeName;
            set => EpisodeName = value;
        }
        string IEpisodeDescriptor.Description
        {
            get => Overview;
            set => Overview = value;
        }
        DateTime? IEpisodeDescriptor.AirDate
        {
            get
            {
                if (DateTime.TryParseExact(FirstAired, new[] {"yyyy-MM-dd", "yyyy-M-d"}, null,
                    DateTimeStyles.AssumeUniversal, out DateTime airDate))
                    return airDate.Date;
                return null;
            }
            set { }
        }
    }
}