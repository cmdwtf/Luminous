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

namespace Luminous.Media
{
    using System;
    using System.IO;
    using System.Media;
    using System.Security;
    using Microsoft.Win32;

    /// <summary>Represents a system sound. This class cannot be inherited.</summary>
    public sealed class SystemSound
    {
        static readonly string _mediaPath = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Media\");
        private string _name;

        /// <summary>Initializes a new instance of the <see cref="T:SystemSound" /> class.</summary>
        /// <param name="name">Name of system sound (i.e. '.Default', 'MenuCommand', 'WindowsLogo', etc...).</param>
        internal SystemSound(string name)
        {
            _name = name;
        }

        /// <summary>Plays the system sound.</summary>
        public void Play()
        {
            try
            {
                string soundPath = Registry.GetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\" + _name + @"\.Current", null, null) as string ?? string.Empty;
                if (!File.Exists(soundPath) && File.Exists(Path.Combine(_mediaPath, soundPath)))
                {
                    soundPath = Path.Combine(_mediaPath, soundPath);
                }
                if (File.Exists(soundPath))
                {
                    using (SoundPlayer player = new SoundPlayer(soundPath))
                    {
                        player.Play();
                    }
                }
            }
            catch (IOException) { }
            catch (UriFormatException) { }
            catch (TimeoutException) { }
            catch (ArgumentException) { }
            catch (SecurityException) { }
            catch (InvalidOperationException) { }
        }
    }
}
