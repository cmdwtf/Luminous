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

// TODO: Custom icons (Set/UpdateIcon(source))
// TODO: Icon without owner set?
// TODO: SET_BUTTON_ELEVATION_REQUIRED_STATE
// TODO: TDM_NAVIGATE
// TODO: ENABLE_[RADIO_]BUTTON (returning radios' state)
// TODO: CLICK_VERIFICATION
// TODO: CLICK_RADIO_BUTTON
// TODO: CLICK_BUTTON
// TODO: Width algorithm
// TODO: events: VerificationClicked, RadioButtonClicked, Navigated, Help, ExpandoButtonClicked, Loaded, ButtonClicked
// TODO: check all
// TODO: do setting the delegate prevents from finalizing object?
// TODO: reshowing, after showing
// TODO: equalize the size of expanded/collapsed text' size
// TODO: wrapping for buttons
// TODO: <code> na <em> gdzie trzeba

namespace Luminous.Windows
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Displays a task dialog that can contain text, buttons, symbols and other elements that inform and instruct the user.
	/// </summary>
	public class TaskDialog
	{
		#region " MessageBox "

		public static Window DefaultOwnerWindow { get; set; }

		/// <summary>
		/// Displays a task dialog with specified text.
		/// </summary>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(string text) => Show(DefaultOwnerWindow, text);

		/// <summary>
		/// Displays a task dialog with specified text and title.
		/// </summary>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(string text, string title) => Show(DefaultOwnerWindow, text, title);

		/// <summary>
		/// Displays a task dialog in front of the specified window and with the specified text.
		/// </summary>
		/// <param name="owner">A window that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(Window owner, string text) => Show(owner, text, TaskDialogHelpers.ProductName);

		/// <summary>
		/// Displays a task dialog with specified text, title, and buttons.
		/// </summary>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(string text, string title, TaskDialogCommonButtons buttons) => Show(DefaultOwnerWindow, text, title, buttons);

		/// <summary>
		/// Displays a task dialog in front of the specified window and with the specified text and title.
		/// </summary>
		/// <param name="owner">A window that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(Window owner, string text, string title) => Show(owner, text, title, TaskDialogCommonButtons.OK);

		/// <summary>
		/// Displays a task dialog with specified text, title, buttons, and icon.
		/// </summary>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon) => Show(DefaultOwnerWindow, text, title, buttons, icon, TaskDialogHelpers.GetFirstButton(buttons));

		/// <summary>
		/// Displays a task dialog in front of the specified window and with the specified text, title, and buttons.
		/// </summary>
		/// <param name="owner">A window that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(Window owner, string text, string title, TaskDialogCommonButtons buttons) => Show(owner, text, title, buttons, TaskDialogIcon.None);

		/// <summary>
		/// Displays a task dialog with specified text, title, buttons, icon, and default button.
		/// </summary>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
		/// <param name="defaultButton">TaskDialogResult value that specifies the default button for the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon, TaskDialogResult defaultButton) => Show(DefaultOwnerWindow, text, title, buttons, icon, defaultButton);

		/// <summary>
		/// Displays a task dialog in front of the specified window and with the specified text, title, buttons, and icon.
		/// </summary>
		/// <param name="owner">A window that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(Window owner, string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon) => Show(owner, text, title, buttons, icon, TaskDialogHelpers.GetFirstButton(buttons));

		/// <summary>
		/// Displays a task dialog in front of the specified window and with specified text, title, buttons, icon, and default button.
		/// </summary>
		/// <param name="owner">A window that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the task dialog.</param>
		/// <param name="title">The text to display in the title bar of the task dialog.</param>
		/// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
		/// <param name="defaultButton">TaskDialogResult value that specifies the default button for the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(Window owner, string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon, TaskDialogResult defaultButton)
		{
			var td = new TaskDialogWindow(null, false)
			{
				Owner = owner,
				WindowTitle = title,
				MainIcon = icon,
				ContentText = text,
				CommonButtons = buttons,
				DefaultButton = defaultButton,
			};
			td.ShowDialog();
			return (TaskDialogResult)td.Tag;
		}

		#endregion

		#region " TaskDialog "

		/// <summary>
		/// Displays a task dialog with the specified title, intruction, content, title, buttons and icon.
		/// </summary>
		/// <param name="windowTitle">The text to display in the title bar of the task dialog.</param>
		/// <param name="mainInstruction">The instruction to display in the task dialog.</param>
		/// <param name="content">The text to display in the task dialog.</param>
		/// <param name="commonButtons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(string windowTitle, string mainInstruction, string content, TaskDialogCommonButtons commonButtons, TaskDialogIcon icon) => Show(DefaultOwnerWindow, windowTitle, mainInstruction, content, commonButtons, icon);

		/// <summary>
		/// Displays a task dialog in front of the specified window and with the specified title, intruction, content, title, buttons and icon..
		/// </summary>
		/// <param name="owner">A window that will own the modal dialog box.</param>
		/// <param name="windowTitle">The text to display in the title bar of the task dialog.</param>
		/// <param name="mainInstruction">The instruction to display in the task dialog.</param>
		/// <param name="content">The text to display in the task dialog.</param>
		/// <param name="commonButtons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
		/// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
		/// <returns>One of the TaskDialogResult values.</returns>
		public static TaskDialogResult Show(Window owner, string windowTitle, string mainInstruction, string content, TaskDialogCommonButtons commonButtons, TaskDialogIcon icon)
		{
			var td = new TaskDialogWindow(null, true) { Owner = owner, WindowTitle = windowTitle, MainIcon = icon, MainInstruction = mainInstruction, ContentText = content, CommonButtons = commonButtons };
			td.DefaultButton = TaskDialogHelpers.GetFirstButton(commonButtons);
			td.UseCommandLinks = true;
			td.ShowDialog();
			return (TaskDialogResult)td.Tag;
		}

		#endregion

		#region " Constructor "

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialog"/> class.
		/// </summary>
		public TaskDialog()
		{
			_taskDialogWindow = new TaskDialogWindow(this, true);
			try
			{
				_taskDialogWindow.Owner = DefaultOwnerWindow;
			}
			catch { }
			_taskDialogWindow.WindowTitle = TaskDialogHelpers.ProductName;
		}

		#endregion

		#region " Methods "

		/// <summary>
		/// Displays a constructed task dialog.
		/// </summary>
		/// <returns>One of the TaskDialogResult values</returns>
		public TaskDialogResult Show()
		{
			_taskDialogWindow.ShowDialog();
			return (TaskDialogResult)_taskDialogWindow.Tag;
		}

		#endregion

		#region " Properties "


		private static float _customScale = 1f;
		public static float CustomScale
		{
			get => _customScale;
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value > 0);

				_customScale = value;
			}
		}

		private readonly TaskDialogWindow _taskDialogWindow;

		/// <summary>
		/// Gets or sets the window that will own the modal dialog box.
		/// </summary>
		/// <value>The window that will own the modal dialog box.</value>
		public Window Owner
		{
			get => _taskDialogWindow.Owner;
			set => _taskDialogWindow.Owner = value;
		}

		/// <summary>
		/// Gets or sets the text to display in the title bar of the task dialog.
		/// </summary>
		/// <value>The text to display in the title bar of the task dialog.</value>
		public string WindowTitle
		{
			get => _taskDialogWindow.WindowTitle;
			set => _taskDialogWindow.WindowTitle = value;
		}

		/// <summary>
		/// Gets or sets the values that specifies which icon to display in the task dialog.
		/// </summary>
		/// <value>One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</value>
		public TaskDialogIcon MainIcon
		{
			get => _taskDialogWindow.MainIcon;
			set => _taskDialogWindow.MainIcon = value;
		}

		/// <summary>
		/// Gets or sets the instruction to display in the task dialog.
		/// </summary>
		/// <value>The instruction to display in the task dialog.</value>
		public string MainInstruction
		{
			get => _taskDialogWindow.MainInstruction;
			set => _taskDialogWindow.MainInstruction = value;
		}

		/// <summary>
		/// Gets or sets the text to display in the task dialog.
		/// </summary>
		/// <value>The text to display in the task dialog.</value>
		public string ContentText
		{
			get => _taskDialogWindow.ContentText;
			set => _taskDialogWindow.ContentText = value;
		}

		public Grid GetCustomContentPanel() => _taskDialogWindow.PanelCustomContent;

		public TaskDialogCommonButtons CommonButtons
		{
			get => _taskDialogWindow.CommonButtons;
			set => _taskDialogWindow.CommonButtons = value;
		}

		public IList<TaskDialogButton> Buttons
		{
			get => _taskDialogWindow.Buttons;
			set => _taskDialogWindow.Buttons = value;
		}

		public TaskDialogResult DefaultButton
		{
			get => _taskDialogWindow.DefaultButton;
			set => _taskDialogWindow.DefaultButton = value;
		}

		public bool EnableHyperlinks
		{
			get => _taskDialogWindow.EnableHyperlinks;
			set => _taskDialogWindow.EnableHyperlinks = value;
		}

		public bool AllowDialogCancellation
		{
			get => _taskDialogWindow.AllowDialogCancellation;
			set => _taskDialogWindow.AllowDialogCancellation = value;
		}

		public bool UseCommandLinks
		{
			get => _taskDialogWindow.UseCommandLinks;
			set => _taskDialogWindow.UseCommandLinks = value;
		}

		public bool UseCommandLinksNoIcon
		{
			get => _taskDialogWindow.UseCommandLinksNoIcon;
			set => _taskDialogWindow.UseCommandLinksNoIcon = value;
		}

		public bool IsPositionRelativeToWindow
		{
			get => _taskDialogWindow.IsPositionRelativeToWindow;
			set => _taskDialogWindow.IsPositionRelativeToWindow = value;
		}

		public TaskDialogIcon FooterIcon
		{
			get => _taskDialogWindow.FooterIcon;
			set => _taskDialogWindow.FooterIcon = value;
		}

		public string FooterText
		{
			get => _taskDialogWindow.FooterText;
			set => _taskDialogWindow.FooterText = value;
		}

		public bool IsExpandedByDefault
		{
			get => _taskDialogWindow.IsExpanded;
			set => _taskDialogWindow.IsExpanded = value;
		}

		public bool ExpandFooterArea
		{
			get => _taskDialogWindow.ExpandFooterArea;
			set => _taskDialogWindow.ExpandFooterArea = value;
		}

		public string ExpandedInformation
		{
			get => _taskDialogWindow.ExpandedInformation;
			set => _taskDialogWindow.ExpandedInformation = value;
		}

		public bool? IsVerificationFlagChecked
		{
			get => _taskDialogWindow.IsVerificationFlagChecked;
			set => _taskDialogWindow.IsVerificationFlagChecked = value;
		}

		public IList<TaskDialogRadioButton> RadioButtons
		{
			get => _taskDialogWindow.RadioButtons;
			set => _taskDialogWindow.RadioButtons = value;
		}

		public TaskDialogResult DefaultRadioButton
		{
			get => _taskDialogWindow.DefaultRadioButton;
			set => _taskDialogWindow.DefaultRadioButton = value;
		}

		public string VerificationText
		{
			get => _taskDialogWindow.VerificationText;
			set => _taskDialogWindow.VerificationText = value;
		}

		public int Width
		{
			get => _taskDialogWindow.ClientWidth;
			set => _taskDialogWindow.ClientWidth = value;
		}

		public string ExpandedControlText
		{
			get => _taskDialogWindow.ExpandedControlText;
			set => _taskDialogWindow.ExpandedControlText = value;
		}

		public string CollapsedControlText
		{
			get => _taskDialogWindow.CollapsedControlText;
			set => _taskDialogWindow.CollapsedControlText = value;
		}

		public bool ShowProgressBar
		{
			get => _taskDialogWindow.ShowProgressBar;
			set => _taskDialogWindow.ShowProgressBar = value;
		}

		public bool ShowMarqueeProgressBar
		{
			get => _taskDialogWindow.ShowMarqueeProgressBar;
			set => _taskDialogWindow.ShowMarqueeProgressBar = value;
		}

		public TaskDialogProgressBarState ProgressBarState
		{
			get => _taskDialogWindow.ProgressBarState;
			set => _taskDialogWindow.ProgressBarState = value;
		}

		public bool UseCallBackTimer
		{
			get => _taskDialogWindow.UseCallBackTimer;
			set => _taskDialogWindow.UseCallBackTimer = value;
		}

		public bool RtlLayout
		{
			get => _taskDialogWindow.RtlLayout;
			set => _taskDialogWindow.RtlLayout = value;
		}

		public bool CanBeMinimized
		{
			get => _taskDialogWindow.CanBeMinimized;
			set => _taskDialogWindow.CanBeMinimized = value;
		}

		public double ProgressBarMinimum
		{
			get => _taskDialogWindow.ProgressBarMinimum;
			set => _taskDialogWindow.ProgressBarMinimum = value;
		}

		public double ProgressBarMaximum
		{
			get => _taskDialogWindow.ProgressBarMaximum;
			set => _taskDialogWindow.ProgressBarMaximum = value;
		}

		public double ProgressBarValue
		{
			get => _taskDialogWindow.ProgressBarValue;
			set => _taskDialogWindow.ProgressBarValue = value;
		}

		#endregion

		#region " Events "

		public event EventHandler<CloseEventArgs> CanClose
		{
			add
			{
				_taskDialogWindow.CanClose += value;
			}
			remove
			{
				_taskDialogWindow.CanClose -= value;
			}
		}

		public event EventHandler<TaskDialogTimerEventArgs> Tick
		{
			add
			{
				_taskDialogWindow.Tick += value;
			}
			remove
			{
				_taskDialogWindow.Tick -= value;
			}
		}

		public event EventHandler<TaskDialogHyperlinkClickEventArgs> HyperlinkClick
		{
			add
			{
				_taskDialogWindow.HyperlinkClick += value;
			}
			remove
			{
				_taskDialogWindow.HyperlinkClick -= value;
			}
		}

		#endregion
	}
}
