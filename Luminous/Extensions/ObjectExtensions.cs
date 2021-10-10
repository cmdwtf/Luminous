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

namespace System
{
	using System.Diagnostics.Contracts;

	/// <summary>Extension methods for the Object class.</summary>
	public static class ObjectExtensions
	{
		#region Conversion

		/// <summary>Checks whether the object can be converted to the provided type.</summary>
		public static bool CanConvert<T>(this object @this, IFormatProvider formatProvider = null) => CanConvert(@this, typeof(T), formatProvider);

		/// <summary>Checks whether the object can be converted to the provided type.</summary>
		public static bool CanConvert(this object @this, Type conversionType, IFormatProvider formatProvider = null)
		{
			Contract.Requires<ArgumentNullException>(conversionType != null);

			try
			{
				System.Convert.ChangeType(@this, conversionType, formatProvider);
				return true;
			}
			catch (InvalidCastException) { }
			catch (FormatException) { }
			catch (OverflowException) { }
			catch (ArgumentNullException) { }
			return false;
		}

		/// <summary>Converts the object to the provided type.</summary>
		public static T Convert<T>(this object @this, IFormatProvider formatProvider = null)
		{
			Contract.Requires<InvalidCastException>(!(@this == null && typeof(T).IsValueType));

			Type type = typeof(T);
			object result = Convert(@this, type, formatProvider);
			if (result == null)
			{
				if (type.IsValueType)
				{
					throw new InvalidCastException();
				}

				return default; // null
			}
			return (T)result;
		}

		/// <summary>Converts the object to the provided type.</summary>
		public static object Convert(this object @this, Type conversionType, IFormatProvider formatProvider = null)
		{
			Contract.Requires<ArgumentNullException>(conversionType != null);

			return System.Convert.ChangeType(@this, conversionType, formatProvider);
		}

		#endregion
	}
}
