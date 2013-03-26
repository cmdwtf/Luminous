#region License
// Copyright © 2013 Łukasz Świątkowski
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

namespace Luminous.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Luminous.Media;

    internal static class TaskDialogSound
    {
        public static SystemSound Default
        {
            get { return SoundScheme.Default; }
        }

        public static SystemSound Error
        {
            get { return SoundScheme.SystemHand; }
        }

        public static SystemSound Information
        {
            get { return SoundScheme.SystemAsterisk; }
        }

        public static SystemSound Question
        {
            get { return SoundScheme.SystemQuestion; }
        }

        public static SystemSound Security
        {
            get
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    return SoundScheme.WindowsUAC;
                }
                else
                {
                    return SoundScheme.SystemHand;
                }
            }
        }

        public static SystemSound Warning
        {
            get { return SoundScheme.SystemExclamation; }
        }
    }
}
