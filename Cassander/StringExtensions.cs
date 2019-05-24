using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cassander
{
    public static class StringExtensions
    {
        static readonly Regex splitter = new Regex("(?<=[a-z])(?=[A-Z-_\\W])");
        static readonly Regex cleaner = new Regex("[\\W-_]+");

        public static string NormalizeNames(this string text)
        {
            var wordMatches = splitter.Matches(text);
            if (wordMatches.Count == 0)
            {
                return text.ToLowerInvariant();
            }

            var words = new List<string>(wordMatches.Count + 1);
            var startIndex = 0;
            foreach(Match match in wordMatches)
            {
                var word = GetWord(text, startIndex, match.Index - startIndex);
                startIndex = match.Index;
                

                if(word.Length == 0)
                {
                    continue;
                }

                words.Add(word.ToLowerInvariant());
            }

            if (startIndex < text.Length)
            {
                var word = GetWord(text, startIndex, text.Length - startIndex);
                if (word.Length > 0)
                {
                    words.Add(word.ToLowerInvariant());
                }
            }

            return String.Join("-", words);
        }

        private static string GetWord(string text, int startIndex, int lenght)
        {
            var word = text.Substring(startIndex, lenght);
            word = cleaner.Replace(word, String.Empty);
            return word;
        }
    }
}
