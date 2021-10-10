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

namespace Luminous.Windows.Forms
{
	using System;

	/// <summary>
	/// Represents a button on TaskDialog form.
	/// </summary>
	/// <remarks></remarks>
	public class TaskDialogButton
	{
		#region Fields and Properties

		/// <summary>
		/// Determines whether the button should contain a custom text.
		/// </summary>
		public bool UseCustomText { get; set; }

		/// <summary>
		/// The custom text shown on the button.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Determines the value returned to the parent form when the button is clicked.
		/// </summary>
		public TaskDialogResult Result { get; set; }

		/// <summary>
		/// Determines whether to show the elevation icon (shield).
		/// </summary>
		public bool ShowElevationIcon { get; set; }

		/// <summary>
		/// Determines whether the button is enabled.
		/// </summary>
		public bool IsEnabled { get; set; }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the button is clicked, but before the TaskDialog form is closed.
		/// </summary>
		public event EventHandler Click;
		internal void RaiseClickEvent(object sender, EventArgs e) => Click?.Invoke(sender, e);
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the new instance of the TaskDialogButton.
		/// </summary>
		/// <param name="result">Determines the value returned to the parent form when the button is clicked.</param>
		public TaskDialogButton(TaskDialogResult result)
		{
			Result = result;
			IsEnabled = true;
		}

		/// <summary>
		/// Initializes the new instance of the TaskDialogButton.
		/// </summary>
		/// <param name="result">Determines the value returned to the parent form when the button is clicked.</param>
		/// <param name="showElevationIcon">Determines whether to show the elevation icon (shield).</param>
		public TaskDialogButton(TaskDialogResult result, bool showElevationIcon)
		{
			Result = result;
			ShowElevationIcon = showElevationIcon;
			IsEnabled = true;
		}

		/// <summary>
		/// Initializes the new instance of the TaskDialogButton.
		/// </summary>
		/// <param name="text">The custom text shown on the button.</param>
		/// <param name="click">Occurs when the button is clicked, but before the TaskDialog form is closed.</param>
		public TaskDialogButton(string text, EventHandler click)
		{
			UseCustomText = true;
			Text = text;
			Result = TaskDialogResult.None;
			Click += click;
			IsEnabled = true;
		}

		/// <summary>
		/// Initializes the new instance of the TaskDialogButton.
		/// </summary>
		/// <param name="text">The custom text shown on the button.</param>
		/// <param name="click">Occurs when the button is clicked, but before the TaskDialog form is closed.</param>
		/// <param name="showElevationIcon">Determines whether to show the elevation icon (shield).</param>
		public TaskDialogButton(string text, EventHandler click, bool showElevationIcon)
		{
			UseCustomText = true;
			Text = text;
			Result = TaskDialogResult.None;
			Click += click;
			ShowElevationIcon = showElevationIcon;
			IsEnabled = true;
		}

		/// <summary>
		/// Initializes the new instance of the TaskDialogButton.
		/// </summary>
		/// <param name="taskDialogResult">Determines the value returned to the parent form when the button is clicked.</param>
		/// <param name="text">The custom text shown on the button.</param>
		public TaskDialogButton(TaskDialogResult taskDialogResult, string text)
		{
			UseCustomText = true;
			Text = text;
			Result = taskDialogResult;
			IsEnabled = true;
		}

		/// <summary>
		/// Initializes the new instance of the TaskDialogButton.
		/// </summary>
		/// <param name="tresult">Determines the value returned to the parent form when the button is clicked.</param>
		/// <param name="text">The custom text shown on the button.</param>
		/// <param name="showElevationIcon">Determines whether to show the elevation icon (shield).</param>
		public TaskDialogButton(TaskDialogResult tresult, string text, bool showElevationIcon)
		{
			UseCustomText = true;
			Text = text;
			Result = tresult;
			ShowElevationIcon = showElevationIcon;
			IsEnabled = true;
		}
		#endregion
	}
}