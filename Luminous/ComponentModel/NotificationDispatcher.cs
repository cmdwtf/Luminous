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
    using System.Linq;

    public class NotificationDispatcher
    {
        #region Constructor & properties

        private readonly Dictionary<string, PropertyChangingEventHandler> _changingHandlers;
        private readonly Dictionary<string, PropertyChangedEventHandler> _changedHandlers;

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_changingHandlers != null);
            Contract.Invariant(_changedHandlers != null);
        }

        public NotificationDispatcher()
        {
            _changingHandlers = new Dictionary<string, PropertyChangingEventHandler>();
            _changedHandlers = new Dictionary<string, PropertyChangedEventHandler>();
        }

        #endregion

        #region Methods

        public void RegisterNotifyingObject(NotifyingObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            RegisterNotifyingObject((INotifyPropertyChanging)obj);
            RegisterNotifyingObject((INotifyPropertyChanged)obj);
        }

        public void RegisterNotifyingObject(INotifyPropertyChanging obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.PropertyChanging += (sender, e) => ProcessNotification(sender, e);
        }

        public void RegisterNotifyingObject(INotifyPropertyChanged obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.PropertyChanged += (sender, e) => ProcessNotification(sender, e);
        }

        public void RegisterNotificationHandlingMethods(object handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            var methods = handler.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            foreach (var method in methods)
            {
                if (method.ContainsGenericParameters || method.IsGenericMethod) continue;
                var parameters = method.GetParameters();
                if (parameters.Length != 2) continue;
                if (parameters[0].ParameterType != typeof(object)) continue;
                if (parameters[1].ParameterType.IsAssignableFrom(typeof(PropertyChangingEventArgs)))
                {
                    var changingAttributes = method.GetCustomAttributes(typeof(PropertyChangingHandlerAttribute), false).OfType<PropertyChangingHandlerAttribute>().ToList();
                    if (changingAttributes.Count > 0)
                    {
                        var handlerDelegate = (PropertyChangingEventHandler)Delegate.CreateDelegate(typeof(PropertyChangingEventHandler), handler, method);
                        Contract.Assume(handlerDelegate != null);
                        foreach (var attribute in changingAttributes)
                        {
                            RegisterNotificationHandler(attribute.PropertyName, handlerDelegate);
                        }
                    }
                }
                if (parameters[1].ParameterType.IsAssignableFrom(typeof(PropertyChangedEventArgs)))
                {
                    var changedAttributes = method.GetCustomAttributes(typeof(PropertyChangedHandlerAttribute), false).OfType<PropertyChangedHandlerAttribute>().ToList();
                    if (changedAttributes.Count > 0)
                    {
                        var handlerDelegate = (PropertyChangedEventHandler)Delegate.CreateDelegate(typeof(PropertyChangedEventHandler), handler, method);
                        Contract.Assume(handlerDelegate != null);
                        foreach (var attribute in changedAttributes)
                        {
                            RegisterNotificationHandler(attribute.PropertyName, handlerDelegate);
                        }
                    }
                }
            }
        }

        public void RegisterNotificationHandler(string propertyName, PropertyChangingEventHandler handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            if (!_changingHandlers.ContainsKey(propertyName))
            {
                _changingHandlers[propertyName] = handler;
            }
            else
            {
                _changingHandlers[propertyName] = _changingHandlers[propertyName] += handler;
            }
        }

        public void UnregisterNotificationHandler(string propertyName, PropertyChangingEventHandler handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            if (_changingHandlers.ContainsKey(propertyName))
            {
                _changingHandlers[propertyName] = _changingHandlers[propertyName] -= handler;
            }
        }

        public void RegisterNotificationHandler(string propertyName, PropertyChangedEventHandler handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            if (!_changedHandlers.ContainsKey(propertyName))
            {
                _changedHandlers[propertyName] = handler;
            }
            else
            {
                _changedHandlers[propertyName] = _changedHandlers[propertyName] += handler;
            }
        }

        public void UnregisterNotificationHandler(string propertyName, PropertyChangedEventHandler handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);

            if (_changedHandlers.ContainsKey(propertyName))
            {
                _changedHandlers[propertyName] = _changedHandlers[propertyName] -= handler;
            }
        }

        public void UnregisterAllNotificationHandlers(string propertyName)
        {
            if (_changingHandlers.ContainsKey(propertyName))
            {
                _changingHandlers.Remove(propertyName);
            }
            if (_changedHandlers.ContainsKey(propertyName))
            {
                _changedHandlers.Remove(propertyName);
            }
        }

        public void UnregisterAllNotificationHandlers()
        {
            _changingHandlers.Clear();
            _changedHandlers.Clear();
        }

        public void ProcessNotification(object sender, PropertyChangingEventArgs e)
        {
            if (_changingHandlers.ContainsKey(e.PropertyName))
            {
                var handler = _changingHandlers[e.PropertyName];
                if (handler != null) handler(sender, e);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("No {0} property changing handler registered.", e.PropertyName));
            }
        }

        public void ProcessNotification(object sender, PropertyChangedEventArgs e)
        {
            if (_changedHandlers.ContainsKey(e.PropertyName))
            {
                var handler = _changedHandlers[e.PropertyName];
                if (handler != null) handler(sender, e);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("No {0} property changed handler registered.", e.PropertyName));
            }
        }

        #endregion
    }
}
