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

namespace Luminous.Windows.Converters
{
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;

	using Luminous.Windows.Controls;

	[ValueConversion(typeof(CommandLinkIcon), typeof(Visibility))]
	internal class CommandLinkIconToVisibilityValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is CommandLinkIcon icon
				? icon == CommandLinkIcon.None
					? Visibility.Collapsed
					: (object)((icon == CommandLinkIcon.Arrow ^ System.Convert.ToBoolean(parameter)) ? Visibility.Visible : Visibility.Collapsed)
				: CommandLinkIcon.None;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
	}
}
