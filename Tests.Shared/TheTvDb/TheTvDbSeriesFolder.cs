using System.Collections.Generic;
using System.Linq;

namespace Tests.Shared.TheTvDb
{
    public static class TheTvDbSeriesFolder
    {
        private static readonly Dictionary<KnownTvShowIds, string> KnownSeriesFolderNames = new Dictionary<KnownTvShowIds, string>
        {
            { KnownTvShowIds.AgentsOfSHIELD, "Marvel's Agents of S.H.I.E.L.D" },
            { KnownTvShowIds.AllOrNothingNFL, "All or Nothing" },
            { KnownTvShowIds.Avenue5, "Avenue 5" },
            { KnownTvShowIds.BobsBurgers, "Bob's Burgers" },
            { KnownTvShowIds.BatmanTheAnimatedSeries, "Batman - The Animated Series" },
            { KnownTvShowIds.DisneyGalleryTheMandalorian, "Disney Gallery - Star Wars The Mandalorian" },
            { KnownTvShowIds.FIsForFamily, "F is for Family" },
            { KnownTvShowIds.Moonbase8, "Moonbase 8" },
            { KnownTvShowIds.MrRobot, "Mr. Robot" },
            { KnownTvShowIds.StarTrekDiscovery, "Star Trek Discovery" },
            { KnownTvShowIds.StarTrekPicard, "Star Trek Picard" },
            { KnownTvShowIds.StarWarsRebels, "Star Wars Rebels" },
            { KnownTvShowIds.StarWarsResistance, "Star Wars - Resistance" },
            { KnownTvShowIds.StarWarsTheCloneWars, "Star Wars - The Clone Wars" },
            { KnownTvShowIds.TigerKing, "Tiger King - Murder Mayhem and Madness" },
            { KnownTvShowIds.Twentyfour, "24" },
            { KnownTvShowIds.Visible, "Visible - Out On Television" },
            { KnownTvShowIds.WandaVision, "WandaVision" },
            { KnownTvShowIds.WuTangAnAmericanSaga, "Wu-Tang An American Saga" },
        };

        public static string FolderName(this KnownTvShowIds theTvDbId)
        {
            if (KnownSeriesFolderNames.ContainsKey(theTvDbId))
                return KnownSeriesFolderNames[theTvDbId];

            string s = theTvDbId.ToString("G");
            List<string> parts = new List<string>();

            int lastHumpIndex = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                bool isUpperCase = char.IsUpper(c);
                bool wasLastUpperCase = i > 0 && char.IsUpper(s[i - 1]);

                if (isUpperCase && !wasLastUpperCase)
                {
                    parts.Add(s.Substring(lastHumpIndex, i - lastHumpIndex));
                    lastHumpIndex = i;
                }
            }
            parts.Add(s.Substring(lastHumpIndex, s.Length - lastHumpIndex));

            return string.Join(" ", parts.Where(x => !string.IsNullOrEmpty(x)).ToArray());
        }
    }
}