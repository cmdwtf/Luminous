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
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;

    /// <summary>Extension methods for the ConsoleColor enum.</summary>
    public static class ConsoleColorExtensions
    {
        /// <summary>Provides an easy way to display messages in specified color.</summary>
        /// <param name="foregroundColor">The color in which the messages will be displayed.</param>
        /// <returns>An object which restores previous console foreground color on disposal.</returns>
        /// <example><code>
        /// using (ConsoleColor.Red.AsForeground())
        ///     Console.WriteLine("This should be red.");
        ///
        /// using (ConsoleColor.Cyan.AsForeground())
        /// using (ConsoleColor.Magenta.AsBackground())
        ///     Console.WriteLine("This should be cyan on magenta background.");
        ///
        /// Console.WriteLine("This should be displayed with default colors.");
        /// </code></example>
        public static IDisposable AsForeground(this ConsoleColor foregroundColor)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(Enum.IsDefined(typeof(ConsoleColor), foregroundColor));

            return new ConsoleColorizer(foregroundColor, true);
        }

        /// <summary>Provides an easy way to display messages on specified color.</summary>
        /// <param name="backgroundColor">The color on which the messages will be displayed.</param>
        /// <returns>An object which restores previous console background color on disposal.</returns>
        /// <example>See <see cref="M:ConsoleColorExtensions.AsForeground" /> for example.</example>
        public static IDisposable AsBackground(this ConsoleColor backgroundColor)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(Enum.IsDefined(typeof(ConsoleColor), backgroundColor));

            return new ConsoleColorizer(backgroundColor, false);
        }

        private sealed class ConsoleColorizer : IDisposable
        {
            public ConsoleColorizer(ConsoleColor cc, bool fore)
            {
                _fore = fore;
                try
                {
                    _previousColor = _fore ? Console.ForegroundColor : Console.BackgroundColor;
                    if (_fore) Console.ForegroundColor = cc;
                    else Console.BackgroundColor = cc;
                }
                catch (Exception e)
                {
                    if (!(e is ArgumentException || e is SecurityException || e is IOException)) throw;
                }
            }

            private bool _fore;
            private ConsoleColor? _previousColor;

            public void Dispose()
            {
                if (_previousColor.HasValue)
                {
                    try
                    {
                        if (_fore) Console.ForegroundColor = _previousColor.Value;
                        else Console.BackgroundColor = _previousColor.Value;
                    }
                    catch (Exception e)
                    {
                        if (!(e is ArgumentException || e is SecurityException || e is IOException)) throw;
                    }
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}
