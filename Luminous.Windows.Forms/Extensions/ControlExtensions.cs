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
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>Extension methods for the Control class.</summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Checks whether the handle is created anywhere in the control tree (from the control up to the top-level parent).
        /// </summary>
        public static bool IsHandleCreatedAnywhere(this Control @this)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            lock (@this)
            {
                Control control = @this;
                while (control != null && !control.IsHandleCreated)
                {
                    control = control.Parent;
                }
                if (control == null)
                {
                    return false;
                }
                return control.IsHandleCreated;
            }
        }

        public static void SafeInvoke(this Control @this, Action action)
        {
            Contract.Requires<ArgumentNullException>(@this != null);
            Contract.Requires<ArgumentNullException>(action != null);

            if (@this == null)
            {
                action();
                return;
            }

            if (!@this.IsHandleCreatedAnywhere())
            {
                throw new InvalidOperationException("The control and its parent(s) have no handle created yet.");
            }
            while (@this.Disposing || @this.IsDisposed)
            {
                if (@this.Parent == null && (!(@this is Form) || (@this as Form).Owner == null))
                {
                    throw new ObjectDisposedException("The control is currently disposing or already disposed.");
                }
                @this = @this.Parent != null
                            ? @this.Parent
                            : @this is Form
                                ? (@this as Form).Owner
                                : null;
                if (@this == null)
                {
                    throw new InvalidOperationException("The control has no parent/owner with a created handle.");
                }
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

        public static bool IsInDesignMode(this Control target)
        {
            Contract.Requires<ArgumentNullException>(target != null);

            for (Control control = target; control != null; control = control.Parent)
            {
                ISite site = control.Site;
                if (site != null && site.DesignMode)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsInRuntimeMode(this Control target)
        {
            Contract.Requires<ArgumentNullException>(target != null);

            for (Control control = target; control != null; control = control.Parent)
            {
                ISite site = control.Site;
                if (site != null && site.DesignMode)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
