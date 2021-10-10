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
	using System.ComponentModel;
	using System.Diagnostics.Contracts;

	/// <summary>Extension methods for the Enum class.</summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Returns the enum’s description.
		/// </summary>
		[Pure]
		public static string GetDescription(this Enum @this)
		{
			Contract.Requires<ArgumentNullException>(@this != null);
			if (!Enum.IsDefined(@this.GetType(), @this))
			{
				return null; //Contract.Requires<ArgumentOutOfRangeException>(Enum.IsDefined(@this.GetType(), @this));
			}

			Type type = @this.GetType();
			Reflection.MemberInfo[] mis = type.GetMember(Enum.GetName(type, @this));
			if (mis.Length > 0)
			{
				Reflection.MemberInfo mi = mis[0];
				if (Attribute.IsDefined(mi, typeof(DescriptionAttribute)))
				{
					var da = (DescriptionAttribute)Attribute.GetCustomAttribute(mi, typeof(DescriptionAttribute));
					if (da != null)
					{
						return da.Description;
					}
				}
			}
			return null;
		}
	}
}
