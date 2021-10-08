using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MayoSolutions.Media.MediaData.TV;

namespace TheTvDb.BenchTests.v4
{
    internal abstract class DumpLine
    {
        protected const int ColumnWidth = 65;
        protected const int MarginInner = 1;
        protected const int MarginOuter = 1;
        protected const int Indent = 2;
        protected const char RowSeparator = '-';
        protected const char ColumnSeparator = '|';
        protected const char EmptyCharacter = ' ';

        public abstract void Write(TextWriter writer);
    }

    internal class ComparisonLine : DumpLine
    {
        public string V2 { get; set; }
        public string V4 { get; set; }

        public ComparisonLine() { }

        public ComparisonLine(string v2, string v4)
        {
            V2 = v2;
            V4 = v4;
        }

        private List<string> BreakLines(string value)
        {
            var lines = new List<string>();

            int index = 0;
            int len = value.Length;

            while (index < len)
            {
                var indentation = lines.Count == 0 ? string.Empty : new string(EmptyCharacter, Indent);
                var availableChars = ColumnWidth - indentation.Length;
                var consumed = value.Substring(index, Math.Max(0, Math.Min(len-index, Math.Min(availableChars, ColumnWidth))));
                lines.Add($"{indentation}{consumed}");
                index += consumed.Length;
            }

            return lines;
        }

        public override void Write(TextWriter writer)
        {
            var v2 = V2 ?? string.Empty;
            var v4 = V4 ?? string.Empty;


            var v2Length = v2.Length;
            var v4Length = v4.Length;


            var v2Lines = BreakLines(v2);
            var v4Lines = BreakLines(v4);


            var v2Height = v2Lines.Count;
            var v4Height = v4Lines.Count;
            var maxHeight = Math.Max(v2Height, v4Height);

            for (int i = 0; i < maxHeight; i++)
            {
                writer.Write(new string(EmptyCharacter, MarginOuter));

                var v2Line = string.Empty;
                if (i < v2Height) v2Line = v2Lines[i];
                var v2ConsumedLength = v2Line.Length;
                writer.Write(v2Line);

                if (ColumnWidth > v2ConsumedLength)
                    writer.Write(new string(EmptyCharacter, ColumnWidth - v2ConsumedLength));
                writer.Write(new string(EmptyCharacter, MarginInner));
                writer.Write(ColumnSeparator);
                writer.Write(new string(EmptyCharacter, MarginInner));

                var v4Line = string.Empty;
                if (i < v4Height) v4Line = v4Lines[i];
                var v4ConsumedLength = v4Line.Length;
                writer.WriteLine(v4Line);
            }

        }
    }

    internal class SeparatorLine : DumpLine
    {
        public override void Write(TextWriter writer)
        {
            writer.Write(new string(RowSeparator, ColumnWidth + MarginOuter + MarginOuter));
            writer.Write(ColumnSeparator);
            writer.WriteLine(new string(RowSeparator, ColumnWidth + MarginOuter + MarginOuter));
        }
    }
    internal class EmptyLine : DumpLine
    {
        public override void Write(TextWriter writer)
        {
            writer.Write(new string(EmptyCharacter, ColumnWidth + MarginOuter + MarginOuter));
            writer.WriteLine(ColumnSeparator);
        }
    }



    internal static class ParityDump
    {
        public static void Dump(Series v2Series, Series v4Series, TextWriter writer)
        {
            var output = new List<DumpLine>();

            output.Add(new ComparisonLine("V2", "V4"));
            output.Add(new SeparatorLine());
            output.Add(new EmptyLine());


            // Series ID
            output.Add(new ComparisonLine(v2Series.Id, v4Series.Id));

            // Series Name
            output.Add(new ComparisonLine(
                v2Series?.Name ?? "(null)",
                v4Series?.Name ?? "(null)"
            ));

            // IMDB ID (if supplied for either)
            if (!string.IsNullOrEmpty(v2Series.ImdbId) || !string.IsNullOrEmpty(v4Series.ImdbId))
            {
                output.Add(new ComparisonLine(
                    v2Series.ImdbId ?? string.Empty,
                    v4Series.ImdbId ?? string.Empty
                ));
            }

            output.Add(new SeparatorLine());
            output.Add(new EmptyLine());


            var v2Seasons = v2Series.Seasons ?? new List<Season>();
            var v4Seasons = v4Series.Seasons ?? new List<Season>();
            var v2SeasonCount = v2Seasons.Count;
            var v4SeasonCount = v4Seasons.Count;
            var maxSeasonCount = Math.Max(v2SeasonCount, v4SeasonCount);

            for (int i = 0; i < maxSeasonCount; i++)
            {
                var v2Season = i < v2SeasonCount ? v2Seasons[i] : null;
                var v4Season = i < v4SeasonCount ? v4Seasons[i] : null;

                var v2SeasonSummary = v2Season != null ? $"[{i}] Season {v2Season.SeasonNumber}, {v2Season.Episodes?.Count ?? 0} Episode(s)" : string.Empty;
                var v4SeasonSummary = v4Season != null ? $"[{i}] Season {v4Season.SeasonNumber}, {v4Season.Episodes?.Count ?? 0} Episode(s)" : string.Empty;
                output.Add(new ComparisonLine(v2SeasonSummary, v4SeasonSummary));
                output.Add(new SeparatorLine());


                var v2SeasonEpisodes = v2Season?.Episodes ?? new List<Episode>();
                var v4SeasonEpisodes = v4Season?.Episodes ?? new List<Episode>();
                var v2SeasonEpisodesCount = v2SeasonEpisodes.Count;
                var v4SeasonEpisodesCount = v4SeasonEpisodes.Count;
                var maxSeasonEpisodeCount = Math.Max(v2SeasonEpisodesCount, v4SeasonEpisodesCount);

                for (int j = 0; j < maxSeasonEpisodeCount; j++)
                {
                    var v2SeasonEpisode = j < v2SeasonEpisodesCount ? v2SeasonEpisodes[j] : null;
                    var v4SeasonEpisode = j < v4SeasonEpisodesCount ? v4SeasonEpisodes[j] : null;

                    var v2SeasonEpisodeSummary = v2SeasonEpisode != null ? $"[{j}] {v2SeasonEpisode.EpisodeNumber:00} - {v2SeasonEpisode.Title}" : string.Empty;
                    var v4SeasonEpisodeSummary = v4SeasonEpisode != null ? $"[{j}] {v4SeasonEpisode.EpisodeNumber:00} - {v4SeasonEpisode.Title}" : string.Empty;
                    output.Add(new ComparisonLine(v2SeasonEpisodeSummary, v4SeasonEpisodeSummary));
                }

                if (i < maxSeasonCount-1)
                    output.Add(new SeparatorLine());
            }






            for (int i = 0; i < output.Count; i++)
            {
                output[i].Write(writer);
            }

        }

    }
}
