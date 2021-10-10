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

namespace System.Collections.Generic
{
	using System;
	using System.Diagnostics.Contracts;

	/// <summary>Extension methods for the Dictionary class.</summary>
	public static class DictionaryExtensions
	{
		[Pure]
		public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			Contract.Requires<ArgumentNullException>(dictionary != null);

			return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
		}

		[Pure]
		public static TValue TryGetNotNullValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			Contract.Requires<ArgumentNullException>(dictionary != null);
			Contract.Requires<ArgumentNullException>(defaultValue != null);

			return dictionary.TryGetValue(key, out TValue value) && value != null ? value : defaultValue;
		}

		[Pure]
		public static TValue TryGetNotEmptyValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			Contract.Requires<ArgumentNullException>(dictionary != null);
			Contract.Requires<ArgumentNullException>(defaultValue != null);
			Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(defaultValue.ToString()));

			return dictionary.TryGetValue(key, out TValue value) && value != null && value.ToString().Length > 0 ? value : defaultValue;
		}
	}
}
