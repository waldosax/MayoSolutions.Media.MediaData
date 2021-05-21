using System;
using MayoSolutions.Common.Extensions;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb
{
    internal static class TheTvDbApiExtensions
    {
        public static DateTime? ToTvDbDate(this string s)
        {
            return s.ToNullableDateTime("yyyy-MM-dd", "yyyy-M-d");
        }
    }
}
