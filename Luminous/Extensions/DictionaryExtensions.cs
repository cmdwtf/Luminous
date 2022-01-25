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

	/// <summary>Extension methods for the Dictionary class.</summary>
	public static class DictionaryExtensions
	{
		public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary), $"Contract assertion not met: {nameof(dictionary)} != null");
			}

			return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
		}

		public static TValue TryGetNotNullValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary), $"Contract assertion not met: {nameof(dictionary)} != null");
			}

			if (defaultValue == null)
			{
				throw new ArgumentNullException(nameof(defaultValue), $"Contract assertion not met: {nameof(defaultValue)} != null");
			}

			return dictionary.TryGetValue(key, out TValue value) && value != null ? value : defaultValue;
		}

		public static TValue TryGetNotEmptyValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary), $"Contract assertion not met: {nameof(dictionary)} != null");
			}

			if (defaultValue == null)
			{
				throw new ArgumentNullException(nameof(defaultValue), $"Contract assertion not met: {nameof(defaultValue)} != null");
			}

			if (string.IsNullOrEmpty(defaultValue.ToString()))
			{
				throw new ArgumentNullException(nameof(defaultValue), $"Contract assertion not met: !string.IsNullOrEmpty({nameof(defaultValue)}.ToString())");
			}

			return dictionary.TryGetValue(key, out TValue value) && value != null && value.ToString().Length > 0 ? value : defaultValue;
		}
	}
}
