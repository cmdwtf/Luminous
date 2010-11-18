#region License
// Copyright © 2010 Łukasz Świątkowski.
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

namespace Luminous.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>Extension methods for the String class.</summary>
    public static class StringExtensions
    {
        #region ToStringOrNull

        [Pure]
        public static string ToStringOrNull<T>(this T @this) where T : class
        {
            if (@this == null) return null;
            return @this.ToString();
        }

        [Pure]
        public static string ToStringOrNull<T>(this T @this, string format, IFormatProvider formatProvider = null) where T : class, IFormattable
        {
            if (@this == null) return null;
            return @this.ToString(format, formatProvider);
        }

        [Pure]
        public static string ToStringOrNull<T>(this T? @this) where T : struct
        {
            if (!@this.HasValue) return null;
            return @this.Value.ToString();
        }

        [Pure]
        public static string ToStringOrNull<T>(this T? @this, string format, IFormatProvider formatProvider = null) where T : struct, IFormattable
        {
            if (!@this.HasValue) return null;
            return @this.Value.ToString(format, formatProvider);
        }

        #endregion

        #region Coalesce

        [Pure]
        public static string Coalesce(this string @this, string other)
        {
            return !string.IsNullOrEmpty(@this) ? @this : other;
        }

        [Pure]
        public static string Coalesce(this string @this, params string[] other)
        {
            string result = @this;
            if (other == null || other.Length == 0) return result;
            foreach (var str in other)
            {
                if (!string.IsNullOrEmpty(result)) return result;
                result = str;
            }
            return result;
        }

        public static string Coalesce(this string @this, IEnumerable<string> other)
        {
            string result = @this;
            if (other == null) return null;
            if (!string.IsNullOrEmpty(result)) return result;
            foreach (var str in other)
            {
                if (!string.IsNullOrEmpty(result)) return result;
                result = str;
            }
            return result;
        }

        [Pure]
        public static string CoalesceWithEmpty(this string @this, params string[] other)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Coalesce(other).Coalesce(string.Empty);
        }

        [Pure]
        public static string CoalesceWithEmpty(this string @this, IEnumerable<string> other)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Coalesce(other).Coalesce(string.Empty);
        }

        #endregion

        [Pure]
        public static string SubstringTo(this string @this, int startIndex, int endIndex)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Substring(startIndex, endIndex - startIndex + 1);
        }

        [Pure]
        public static string Clip(this string @this, int startClipLength, int endClipLength)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Substring(startClipLength, @this.Length - startClipLength - endClipLength);
        }

        [Pure]
        public static string ToXmlEncodedString(this string @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return new XElement("_", @this).ToString().Clip(3, 4);
        }

        [Pure]
        public static string ToXmlDecodedString(this string @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Ensures(Contract.Result<string>() != null);
            
            return XDocument.Parse(string.Format("<_>{0}</_>", @this)).Element("_").Value;
        }
    }
}
