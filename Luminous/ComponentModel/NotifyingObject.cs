#region License
// Copyright © 2014 Łukasz Świątkowski
// http://www.lukesw.net/
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
#endregion

namespace Luminous.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public class NotifyingObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constructor & properties

        private int _id;
        private static object _lock = new object();
        private static int _maxId;

        public NotifyingObject()
        {
            lock (_lock)
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
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs ea)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, ea);
            }
        }
        #endregion

        #region OnPropertyChanged
        /// <summary>
        /// Triggers the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs ea)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, ea);
            }
        }
        #endregion

        #endregion

        #region Internal storage class

        private static class PropertyStore<T>
        {
            public static Dictionary<string, T> Store = new Dictionary<string, T>();
        }

        #endregion

        #region Methods

        public T GetValue<T>(string name)
        {
            return GetValue<T>(name, default(T));
        }

        public T GetValue<T>(string name, T defaultValue)
        {
            return GetValue<T>(name, () => defaultValue);
        }

        public T GetValue<T>(string name, Func<T> defaultValueProvider)
        {
            Contract.Requires<ArgumentNullException>(defaultValueProvider != null);

            name = string.Format("{0}::{1}::{2}", GetType().FullName, _id, name);

            if (!PropertyStore<T>.Store.ContainsKey(name)) return defaultValueProvider();

            return PropertyStore<T>.Store[name];
        }

        public void SetValue<T>(string name, T value)
        {
            string fullName = string.Format("{0}::{1}::{2}", GetType().FullName, _id, name);

            if (PropertyStore<T>.Store.ContainsKey(fullName) && Equals(GetValue<T>(name), value)) return;

            OnPropertyChanging(new PropertyChangingEventArgs(name));
            PropertyStore<T>.Store[fullName] = value;
            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
