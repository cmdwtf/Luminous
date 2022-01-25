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

namespace System.ComponentModel
{
	using System;

	public static class IComponentExtensions
	{
		public static bool IsInDesignMode(this IComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component), $"Contract assertion not met: {nameof(component)} != null");
			}

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
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component), $"Contract assertion not met: {nameof(component)} != null");
			}

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
			if (component == null)
			{
				throw new ArgumentNullException(nameof(component), $"Contract assertion not met: {nameof(component)} != null");
			}

			if (disposable == null)
			{
				throw new ArgumentNullException(nameof(disposable), $"Contract assertion not met: {nameof(disposable)} != null");
			}

			component.Disposed += (sender, e) =>
			{
				disposable.Dispose();
			};
		}
	}
}
