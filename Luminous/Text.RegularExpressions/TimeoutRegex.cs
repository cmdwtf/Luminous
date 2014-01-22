#region License
// Copyright © 2014 Łukasz Świątkowski
// http://www.lukesw.net/
//
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace Luminous.Text.RegularExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Luminous.Threading;

    public static class TimeoutRegex
    {
        public const RegexOptions FastMultilineOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;
        public const RegexOptions FastSinglelineOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline;

        public static bool IsMatch(string input, string pattern)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.IsMatch(input, pattern);
            });
        }

        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.IsMatch(input, pattern, options);
            });
        }

        public static Match Match(string input, string pattern)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.Match(input, pattern);
            });
        }

        public static Match Match(string input, string pattern, RegexOptions options)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.Match(input, pattern, options);
            });
        }

        public static IList<Match> Matches(string input, string pattern)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.Matches(input, pattern).Cast<Match>().ToList().AsReadOnly();
            });
        }

        public static IList<Match> Matches(string input, string pattern, RegexOptions options)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.Matches(input, pattern, options).Cast<Match>().ToList().AsReadOnly();
            });
        }

        public static string[] Split(string input, string pattern)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.Split(input, pattern);
            });
        }

        public static string[] Split(string input, string pattern, RegexOptions options)
        {
            return TimeoutTask.Run(() =>
            {
                return Regex.Split(input, pattern, options);
            });
        }
    }
}
