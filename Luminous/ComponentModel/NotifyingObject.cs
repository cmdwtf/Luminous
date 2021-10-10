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

namespace Luminous.ComponentModel
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics.Contracts;

	public class NotifyingObject : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Constructor & properties

		private readonly int _id;
		private static readonly object Lock = new();
		private static int _maxId;

		public NotifyingObject()
		{
			lock (Lock)
			{
				_id = ++_maxId;
			}
		}

		#endregion

		#region Events

		public event PropertyChangingEventHandler PropertyChanging;
		public event PropertyChangedEventHandler PropertyChanged;

		#region OnPropertyChanging
		/// <summary>
		/// Triggers the PropertyChanging event.
		/// </summary>
		protected virtual void OnPropertyChanging(PropertyChangingEventArgs ea) => PropertyChanging?.Invoke(this, ea);
		#endregion

		#region OnPropertyChanged
		/// <summary>
		/// Triggers the PropertyChanged event.
		/// </summary>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs ea) => PropertyChanged?.Invoke(this, ea);
		#endregion

		#endregion

		#region Internal storage class

		private static class PropertyStore<T>
		{
			public static Dictionary<string, T> Store = new();
		}

		#endregion

		#region Methods

		public T GetValue<T>(string name) => GetValue<T>(name, default(T));

		public T GetValue<T>(string name, T defaultValue) => GetValue<T>(name, () => defaultValue);

		public T GetValue<T>(string name, Func<T> defaultValueProvider)
		{
			Contract.Requires<ArgumentNullException>(defaultValueProvider != null);

			name = string.Format("{0}::{1}::{2}", GetType().FullName, _id, name);

			if (!PropertyStore<T>.Store.ContainsKey(name))
			{
				return defaultValueProvider();
			}

			return PropertyStore<T>.Store[name];
		}

		public void SetValue<T>(string name, T value)
		{
			string fullName = string.Format("{0}::{1}::{2}", GetType().FullName, _id, name);

			if (PropertyStore<T>.Store.ContainsKey(fullName) && Equals(GetValue<T>(name), value))
			{
				return;
			}

			OnPropertyChanging(new PropertyChangingEventArgs(name));
			PropertyStore<T>.Store[fullName] = value;
			OnPropertyChanged(new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}
