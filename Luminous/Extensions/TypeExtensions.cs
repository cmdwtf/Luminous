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
    using System.Diagnostics.Contracts;

    /// <summary>Extension methods for the Type class.</summary>
    public static class TypeExtensions
    {
        public static string GetFullName(this Type @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);

            if (!@this.IsGenericType) return @this.FullName;

            string name = @this.FullName;
            if (name.IndexOf('`') >= 0)
            {
                name = name.Substring(0, name.IndexOf('`'));
            }

            name += '<';
            Type[] types = @this.GetGenericArguments();
            for (int i = 0; i < types.Length; i++)
            {
                if (i > 0) name += ", ";
                name += types[i].GetFullName();
            }
            name += '>';

            return name;
        }
    }
}
