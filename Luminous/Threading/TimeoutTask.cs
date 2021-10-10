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

namespace Luminous.Threading
{
	using System;
	using System.Threading.Tasks;

	public static class TimeoutTask
	{
		public static void Run(Action action) => Run<int>(() => { action(); return 0; });

		public static void Run(Action action, int timeout) => Run<int>(() => { action(); return 0; }, timeout);

		public static T Run<T>(Func<T> func) => Run<T>(func, 15000);

		public static T Run<T>(Func<T> func, int timeout)
		{
			var result = default(T);
			var tk = Task.Run(() =>
			{
				result = func();
			});

			if (tk.Wait(timeout) && tk.IsCompletedSuccessfully)
			{
				return result;
			}

			if (tk.IsFaulted)
			{
				if (tk.Exception is not null)
				{
					throw tk.Exception;
				}
				else
				{
					throw new ArgumentException("The action has thrown an unknown exception.");
				}
			}

			throw new TimeoutException();
		}
	}
}
