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

namespace Luminous.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.ComponentModel;

    /// <summary>
    /// Represents a command link control.
    /// </summary>
    public class CommandLink : Button
    {
        static CommandLink()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandLink), new FrameworkPropertyMetadata(typeof(CommandLink)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.CommandLink"/> class.
        /// </summary>
        public CommandLink() { }

        #region " Note Property "

        /// <summary>
        /// Identifies the <see cref="P:System.Windows.Controls.CommandLink.Note" /> dependency property. 
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="P:System.Windows.Controls.CommandLink.Note" /> dependency property.
        /// </returns>
        public static readonly DependencyProperty NoteProperty = DependencyProperty.Register("Note", typeof(object), typeof(CommandLink), new UIPropertyMetadata(null, _NoteChanged));
        private static readonly DependencyPropertyKey HasNotePropertyKey = DependencyProperty.RegisterReadOnly("HasNote", typeof(bool), typeof(CommandLink), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="P:System.Windows.Controls.CommandLink.HasNote" /> dependency property. 
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="P:System.Windows.Controls.CommandLink.HasNote" /> dependency property.
        /// </returns>
        public static readonly DependencyProperty HasNoteProperty = HasNotePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the note of a System.Windows.Controls.CommandLink. This is a dependency property.
        /// </summary>
        /// <returns>
        /// An object that contains the control's note. The default value is null.
        /// </returns>
        [Bindable(true), Category("Content")]
        public object Note
        {
            get { return GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        private static void _NoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandLink cl = (CommandLink)d;
            cl.SetValue(HasNotePropertyKey, e.NewValue != null);
            cl.OnNoteChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Identifies the <see cref="E:System.Windows.Controls.CommandLink.NoteChanged" /> routed event. 
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="E:System.Windows.Controls.CommandLink.NoteChanged" /> routed event.
        /// </returns>
        public static readonly RoutedEvent NoteChangedEvent = EventManager.RegisterRoutedEvent("NoteChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(CommandLink));

        /// <summary>
        /// Occurs when the note changes. 
        /// </summary>
        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<object> NoteChanged
        {
            add { AddHandler(NoteChangedEvent, value); }
            remove { RemoveHandler(NoteChangedEvent, value); }
        }

        private void RaiseNoteChangedEvent(object oldNote, object newNote)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldNote, newNote, CommandLink.NoteChangedEvent));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.CommandLink.NoteChanged" /> routed event. 
        /// </summary>
        /// <param name="oldValue">
        /// Old value of the <see cref="P:System.Windows.Controls.CommandLink.Note" /> property
        /// </param>
        /// <param name="newValue">
        /// New value of the <see cref="P:System.Windows.Controls.CommandLink.Note" /> property
        /// </param>
        protected virtual void OnNoteChanged(object oldNote, object newNote)
        {
            RaiseNoteChangedEvent(oldNote, newNote);
        }

        /// <summary>
        /// Indicates whether the <see cref="P:System.Windows.Controls.CommandLink.Note" /> property should be persisted.
        /// </summary>
        /// <returns>true if the <see cref="P:System.Windows.Controls.CommandLink.Note" /> property should be persisted; otherwise, false.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool ShouldSerializeNote()
        {
            return (base.ReadLocalValue(NoteProperty) != DependencyProperty.UnsetValue);
        }

        #endregion

        #region " Icon Property "

        /// <summary>
        /// Identifies the <see cref="P:System.Windows.Controls.CommandLink.Icon" /> dependency property. 
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="P:System.Windows.Controls.CommandLink.Icon" /> dependency property.
        /// </returns>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(CommandLinkIcon), typeof(CommandLink), new UIPropertyMetadata(CommandLinkIcon.Arrow, _IconChanged));

        /// <summary>
        /// Gets or sets the value indicating what icon to show on a CommandLink control. This is a dependency property.
        /// </summary>
        /// <returns>
        /// An object that indicates icon to show on a CommandLink control. The default value is CommandLinkIcon.Arrow.
        /// </returns>
        [Bindable(true), Category("Content")]
        public CommandLinkIcon Icon
        {
            get { return (CommandLinkIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void _IconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandLink cl = (CommandLink)d;
            cl.OnIconChanged((CommandLinkIcon)e.OldValue, (CommandLinkIcon)e.NewValue);
        }

        /// <summary>
        /// Identifies the <see cref="E:System.Windows.Controls.CommandLink.IconChanged" /> routed event. 
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="E:System.Windows.Controls.CommandLink.IconChanged" /> routed event.
        /// </returns>
        public static readonly RoutedEvent IconChangedEvent = EventManager.RegisterRoutedEvent("IconChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<CommandLinkIcon>), typeof(CommandLink));

        /// <summary>
        /// Occurs when the Icon property changes. 
        /// </summary>
        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<CommandLinkIcon> IconChanged
        {
            add { AddHandler(IconChangedEvent, value); }
            remove { RemoveHandler(IconChangedEvent, value); }
        }

        private void RaiseIconChangedEvent(CommandLinkIcon oldIcon, CommandLinkIcon newIcon)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldIcon, newIcon, CommandLink.IconChangedEvent));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Controls.CommandLink.IconChanged" /> routed event. 
        /// </summary>
        /// <param name="oldValue">
        /// Old value of the <see cref="P:System.Windows.Controls.CommandLink.Icon" /> property
        /// </param>
        /// <param name="newValue">
        /// New value of the <see cref="P:System.Windows.Controls.CommandLink.Icon" /> property
        /// </param>
        protected virtual void OnIconChanged(CommandLinkIcon oldIcon, CommandLinkIcon newIcon)
        {
            RaiseIconChangedEvent(oldIcon, newIcon);
        }

        #endregion
    }
}
