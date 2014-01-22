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

namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Luminous;

    /// <summary>Extension methods for the String class.</summary>
    public static class StringExtensions
    {
        #region Coalesce

        [Pure]
        public static string Coalesce(this string @this, string other)
        {
            return !string.IsNullOrEmpty(@this) ? @this : other;
        }

        [Pure]
        public static string Coalesce(this string @this, params string[] values)
        {
            if (!string.IsNullOrEmpty(@this) || values == null || values.Length == 0) return @this;
            for (int i = 0; i < values.Length; i++)
            {
                string value = values[i];
                if (!string.IsNullOrEmpty(value)) return value;
            }
            return null;
        }

        public static string Coalesce(this string @this, IEnumerable<string> values)
        {
            if (!string.IsNullOrEmpty(@this) || values == null) return @this;
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value)) return value;
            }
            return null;
        }

        [Pure]
        public static string CoalesceWithEmpty(this string @this, params string[] values)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Coalesce(values) ?? string.Empty;
        }

        public static string CoalesceWithEmpty(this string @this, IEnumerable<string> values)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Coalesce(values) ?? string.Empty;
        }

        #endregion

        #region ToStringOrNull

        [Pure]
        public static string ToStringOrNull<T>(this T @this)
            where T : class
        {
            if (@this == null) return null;
            return @this.ToString();
        }

        [Pure]
        public static string ToStringOrNull<T>(this T @this, string format, IFormatProvider formatProvider = null)
            where T : class, IFormattable
        {
            if (@this == null) return null;
            return @this.ToString(format, formatProvider);
        }

        [Pure]
        public static string ToStringOrNull<T>(this T? @this)
            where T : struct
        {
            if (!@this.HasValue) return null;
            return @this.Value.ToString();
        }

        [Pure]
        public static string ToStringOrNull<T>(this T? @this, string format, IFormatProvider formatProvider = null)
            where T : struct, IFormattable
        {
            if (!@this.HasValue) return null;
            return @this.Value.ToString(format, formatProvider);
        }

        #endregion

        [Pure]
        public static string SubstringTo(this string @this, int startIndex, int endIndex)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(startIndex <= endIndex || startIndex == endIndex + 1);
            Contract.Requires<ArgumentOutOfRangeException>(endIndex < @this.Length);
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Substring(startIndex, endIndex - startIndex + 1);
        }

        [Pure]
        public static string Clip(this string @this, int startClipLength, int endClipLength)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Requires<ArgumentOutOfRangeException>(startClipLength >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(endClipLength >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(@this.Length - startClipLength - endClipLength >= 0);
            Contract.Ensures(Contract.Result<string>() != null);

            return @this.Substring(startClipLength, @this.Length - startClipLength - endClipLength);
        }

        [Pure]
        public static string ToXmlEncodedString(this string @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var sb = new StringBuilder();
            using (var xw = XmlWriter.Create(sb, new XmlWriterSettings
            {
                CheckCharacters = false,
                OmitXmlDeclaration = true,
            }))
            {
                new XElement("_", @this).Save(xw);
            }
            return sb.ToString().Clip(3, 4);
        }

        [Pure]
        public static string ToXmlDecodedString(this string @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Ensures(Contract.Result<string>() != null);

            using (var xw = XmlReader.Create(new StringReader(string.Format("<_>{0}</_>", @this)), new XmlReaderSettings
            {
                CheckCharacters = false,
            }))
            {
                XDocument xd = XDocument.Load(xw);
                XElement el = null;
                if (xd == null || (el = xd.Element("_")) == null)
                    throw new ArgumentException("The specified string is not a valid XML encoded string.");
                return el.Value;
            }
        }

        public static string Indent(this string @this, int width = 4, char indentChar = ' ')
        {
            Contract.Requires<ArgumentException>(width >= 0);

            return string.Join(
                Environment.NewLine,
                (from line in @this.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                 let l = string.IsNullOrEmpty(line)
                         ? string.Empty
                         : new string(indentChar, width) + line
                 select l
                ).ToArray()
            );
        }

        public static Either<string, FormatToken>[] FormatTokenSplit(this string @this)
        {
            if (@this == null) return null;

            var tokens = new List<Either<string, FormatToken>>();
            if (@this.Length == 0) return tokens.ToArray();

            var stack = new Stack<char>(@this.ToCharArray().Reverse());
            StringBuilder sb = new StringBuilder();
            bool isToken = false;

            while (stack.Count > 0)
            {
                if (stack.Peek() == '{')
                {
                    if (isToken)
                    {
                        throw new FormatException("Token inside other token is illegal.");
                    }
                    stack.Pop();
                    // double ‘{’?
                    if (stack.Count > 0 && stack.Peek() == '{')
                    {
                        //  ‘{’ char encoded
                        stack.Pop();
                        sb.Append('{');
                        continue;
                    }
                    // next char is not ‘{’—next token started?
                    if (stack.Count == 0)
                    {
                        throw new FormatException("Closing parenthesis is missing.");
                    }
                    if (sb.Length > 0)
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    isToken = true;
                    continue;
                }
                else if (stack.Peek() == '}')
                {
                    stack.Pop();
                    if (isToken)
                    {
                        tokens.Add(new FormatToken(sb.ToString()));
                        sb.Length = 0;
                        isToken = false;
                        continue;
                    }
                    // double ‘}’?
                    if (stack.Count > 0 && stack.Peek() == '}')
                    {
                        //  ‘}’ char encoded
                        stack.Pop();
                        sb.Append('}');
                        continue;
                    }
                    throw new FormatException("Opening parenthesis is missing.");
                }
                else sb.Append(stack.Pop());
            }
            if (isToken)
            {
                throw new FormatException("Closing parenthesis is missing.");
            }
            if (sb.Length > 0)
            {
                tokens.Add(sb.ToString());
            }
            return tokens.ToArray();
        }

        public static string Format2(this string @this, params object[] args)
        {
            if (@this == null) throw new NullReferenceException();
            if (args == null) throw new ArgumentNullException("args");

            if (args.Length == 0) return @this;

            var parts = FormatTokenSplit(@this);
            for (int index = 0; index < parts.Length; index++)
            {
                var part = parts[index];
                if (part.IsSecond)
                {
                    var formatToken = part.Second.Value;
                    int i = formatToken.IndexOfAny(",:".ToCharArray());
                    if (i == -1)
                    {
                        i = int.Parse(formatToken, NumberFormatInfo.InvariantInfo);
                        if (i < args.Length)
                        {
                            parts[index] = args[i].ToStringOrNull();
                        }
                        else
                        {
                            parts[index] = new FormatToken((i - args.Length).ToString());
                        }
                    }
                    else
                    {
                        int j = int.Parse(formatToken.Substring(0, i), NumberFormatInfo.InvariantInfo);
                        if (j < args.Length)
                        {
                            parts[index] = string.Format("{0" + formatToken.Substring(i) + "}", args[j]);
                        }
                        else
                        {
                            parts[index] = new FormatToken((j - args.Length) + formatToken.Substring(i));
                        }
                    }
                }
            }
            bool containsTokens = parts.Any(e => e.IsSecond);
            var sb = new StringBuilder(@this.Length);
            for (int index = 0; index < parts.Length; index++)
            {
                var part = parts[index];
                if (part.IsFirst)
                {
                    if (containsTokens)
                    {
                        sb.Append(part.First.Replace("{", "{{").Replace("}", "}}"));
                    }
                    else
                    {
                        sb.Append(part.First);
                    }
                }
                else
                {
                    var formatToken = part.Second.Value;
                    sb.Append('{')
                      .Append(formatToken)
                      .Append('}');
                }
            }
            return sb.ToString();
        }

        public static SecureString ToSecureString(this string @this)
        {
            return ToSecureString(@this, true);
        }

        public static SecureString ToSecureString(this string @this, bool makeReadOnly)
        {
            var ss = new SecureString();
            @this.ToCharArray().ToList().ForEach(c => ss.AppendChar(c));
            if (makeReadOnly)
            {
                ss.MakeReadOnly();
            }
            return ss;
        }

        private static byte[] salt = new byte[] { 0xfe, 0x71, 0x35, 0x20, 0x11, 0x12, 0x05, 0x09, 0x13, 0x7c, 0x3a, 0x52, 0xac };
        public static string ToHash(SecureString @this)
        {
            byte[] ssBytes;
            IntPtr bstr = IntPtr.Zero;
            try
            {
                ssBytes = new byte[@this.Length * 2];
                Marshal.Copy((bstr = Marshal.SecureStringToBSTR(@this)),
                ssBytes, 0, ssBytes.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
            Rfc2898DeriveBytes k = new Rfc2898DeriveBytes(ssBytes, salt, 1024);
            using (SHA512Managed sha = new SHA512Managed())
            {
                byte[] hashed = sha.ComputeHash(k.GetBytes(256));
                sha.Clear();
                return Convert.ToBase64String(hashed);
            }
        }
    }

    [System.Diagnostics.DebuggerDisplay("{{{Value}}}")]
    public struct FormatToken
    {
        public readonly string Value;
        public FormatToken(string value)
        {
            Value = value;
        }
    }
}

