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

namespace System.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using Luminous.Windows.Forms;

    /// <summary>Extension methods for the ListView class.</summary>
    public static class ListViewExtensions
    {
        public static void EnableSimpleSelect(this ListView @this, bool enable)
        {
            Contract.Requires<ArgumentNullException>(@this != null);

            Native.ListView.SetExtendedListViewStyle(@this, Native.ListView.ExtendedStyle.SimpleSelect, enable);
        }

        public static void EnableDoubleBuffering(this ListView @this, bool enable = true)
        {
            Contract.Requires<ArgumentNullException>(@this != null);

            Native.ListView.SetExtendedListViewStyle(@this, Native.ListView.ExtendedStyle.DoubleBuffer, enable);
        }
    }
}
