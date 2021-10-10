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

namespace Luminous.Windows.Input
{
	using System;
	using System.Windows;
	using System.Windows.Input;

	public sealed class ApplicationExitCommand : ICommand
	{
		#region Static Members

		public static ICommand Instance { get; } = new ApplicationExitCommand();

		#endregion

		#region Instance Members

		public bool CanExecute(object parameter) => true;

#pragma warning disable 67
		public event EventHandler CanExecuteChanged;
#pragma warning restore 67

		public void Execute(object parameter)
		{
			int exitCode = parameter is int ec
				? ec
				: 0;

			if (Application.Current != null)
			{
				Application.Current.Shutdown(exitCode);
			}
			else
			{
				Environment.Exit(exitCode);
			}
		}

		#endregion
	}
}
