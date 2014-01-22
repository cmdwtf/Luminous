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

namespace System.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public static class IComponentExtensions
    {
        public static bool IsInDesignMode(this IComponent component)
        {
            Contract.Requires<ArgumentNullException>(component != null);

            bool designMode = false;
            ISite site = component.Site;
            if (site != null)
            {
                designMode = site.DesignMode;
            }
            return designMode;
        }

        public static bool IsInRuntimeMode(this IComponent component)
        {
            Contract.Requires<ArgumentNullException>(component != null);

            bool flag = true;
            ISite site = component.Site;
            if (site != null)
            {
                flag = !site.DesignMode;
            }
            return flag;
        }

        public static void DisposeAlso(this IComponent component, IDisposable disposable)
        {
            Contract.Requires<ArgumentNullException>(component != null);
            Contract.Requires<ArgumentNullException>(disposable != null);

            component.Disposed += (sender, e) =>
            {
                disposable.Dispose();
            };
        }
    }
}
