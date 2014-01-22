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
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>Extension methods for the Enum class.</summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the enum’s description.
        /// </summary>
        [Pure]
        public static string GetDescription(this Enum @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            if (!Enum.IsDefined(@this.GetType(), @this)) return null; //Contract.Requires<ArgumentOutOfRangeException>(Enum.IsDefined(@this.GetType(), @this));

            Type type = @this.GetType();
            var mis = type.GetMember(Enum.GetName(type, @this));
            if (mis.Length > 0)
            {
                var mi = mis[0];
                if (Attribute.IsDefined(mi, typeof(DescriptionAttribute)))
                {
                    DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(mi, typeof(DescriptionAttribute));
                    if (da != null)
                    {
                        return da.Description;
                    }
                }
            }
            return null;
        }
    }
}
