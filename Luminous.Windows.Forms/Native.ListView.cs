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

namespace Luminous.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static partial class Native
    {
        public static class ListView
        {
            public enum ExtendedStyle
            {
                SimpleSelect = 0x00100000,
                DoubleBuffer = 0x00010000,
            }

            public static void SetExtendedListViewStyle(System.Windows.Forms.ListView @this, ExtendedStyle style, bool enable = true)
            {
                Contract.Requires<ArgumentNullException>(@this != null);

                Messages.Send(new HandleRef(@this, @this.Handle), (uint)Messages.ListView.SetExtendedListViewStyle, new IntPtr((int)style), enable ? new IntPtr((int)style) : IntPtr.Zero);
            }
        }
    }
}
