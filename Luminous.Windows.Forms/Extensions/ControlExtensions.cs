#region License
// Copyright © 2011 Łukasz Świątkowski
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

namespace Luminous.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>Extension methods for the Control class.</summary>
    public static class ControlExtensions
    {
        public static void SafeInvoke(this Control @this, Action action)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Requires<ArgumentNullException>(action != null);

            if (!@this.IsHandleCreated)
            {
                // force to create handle
                IntPtr handle = @this.Handle;
            }

            if (@this.InvokeRequired)
            {
                @this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static TResult SafeInvoke<TResult>(this Control @this, Func<TResult> func)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Requires<ArgumentNullException>(func != null);

            TResult result = default(TResult);
            SafeInvoke(@this, () =>
            {
                result = func();
            });
            return result;
        }
    }
}
