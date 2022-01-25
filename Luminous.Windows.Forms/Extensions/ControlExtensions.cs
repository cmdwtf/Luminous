#region License
// Copyright © 2021 Chris Marc Dailey (nitz) <https://cmd.wtf>
// Copyright © 2014 Łukasz Świątkowski <http://www.lukesw.net/>
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
#endregion License

namespace System.Windows.Forms
{
	using System;
	using System.ComponentModel;

	/// <summary>Extension methods for the Control class.</summary>
	public static class ControlExtensions
	{
		/// <summary>
		/// Checks whether the handle is created anywhere in the control tree (from the control up to the top-level parent).
		/// </summary>
		public static bool IsHandleCreatedAnywhere(this Control @this)
		{
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this), $"Contract assertion not met: @{nameof(@this)} != null");
			}

			lock (@this)
			{
				Control control = @this;
				while (control != null && !control.IsHandleCreated)
				{
					control = control.Parent;
				}
				return control != null && control.IsHandleCreated;
			}
		}

		public static void SafeInvoke(this Control @this, Action action)
		{
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this), $"Contract assertion not met: @{nameof(@this)} != null");
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action), $"Contract assertion not met: {nameof(action)} != null");
			}

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
				@this = @this.Parent ?? (@this is Form
								? (@this as Form).Owner
								: null);
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
			if (@this == null)
			{
				throw new ArgumentNullException(nameof(@this), $"Contract assertion not met: @{nameof(@this)} != null");
			}

			if (func == null)
			{
				throw new ArgumentNullException(nameof(func), $"Contract assertion not met: {nameof(func)} != null");
			}

			var result = default(TResult);
			SafeInvoke(@this, () =>
			{
				result = func();
			});
			return result;
		}

		public static bool IsInDesignMode(this Control target)
		{
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target), $"Contract assertion not met: {nameof(target)} != null");
			}

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
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target), $"Contract assertion not met: {nameof(target)} != null");
			}

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
