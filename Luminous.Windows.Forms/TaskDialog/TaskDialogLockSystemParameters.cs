#region License
// Copyright � 2013 �ukasz �wi�tkowski
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

namespace Luminous.Windows.Forms
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Helper class required by LockSystem feature.
    /// </summary>
    internal class TaskDialogLockSystemParameters
    {
        public IntPtr NewDesktop;
        public Bitmap Background;

        public TaskDialogLockSystemParameters(IntPtr newDesktop, Bitmap background)
        {
            NewDesktop = newDesktop;
            Background = background;
        }
    }
}