using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cassander
{
    public static class StringExtensions
    {
        static readonly Regex splitter = new Regex("(?<=[a-z])(?=[A-Z-_\\W])");
        static readonly Regex cleaner = new Regex("[\\W-_]+");

        public static string Normalize(string text)
        {
            var wordMatches = splitter.Matches(text);
            var words = new List<string>(wordMatches.Count);
            foreach(Match match in wordMatches)
            {
                var word = cleaner.Replace(match.Value, String.Empty);
                words.Add(word.ToLower());
            }

            return String.Join("-", words);
        }
    }
}
