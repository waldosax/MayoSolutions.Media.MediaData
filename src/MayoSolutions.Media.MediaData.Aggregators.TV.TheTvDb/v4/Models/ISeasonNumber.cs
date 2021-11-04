namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    internal interface ISeasonNumber
    {
        long Number { get; }
    }

    internal class SeasonNumberShim : ISeasonNumber
    {
        public long Number { get; }

        public SeasonNumberShim(long seasonNumber)
        {
            Number = seasonNumber;
        }
    }
}
