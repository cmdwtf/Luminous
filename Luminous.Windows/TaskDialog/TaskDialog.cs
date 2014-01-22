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
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows;

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
        public static TaskDialogResult Show(string text)
        {
            return Show(DefaultOwnerWindow, text);
        }

        /// <summary>
        /// Displays a task dialog with specified text and title.
        /// </summary>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(string text, string title)
        {
            return Show(DefaultOwnerWindow, text, title);
        }

        /// <summary>
        /// Displays a task dialog in front of the specified window and with the specified text.
        /// </summary>
        /// <param name="owner">A window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(Window owner, string text)
        {
            return Show(owner, text, TaskDialogHelpers.ProductName);
        }

        /// <summary>
        /// Displays a task dialog with specified text, title, and buttons.
        /// </summary>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(string text, string title, TaskDialogCommonButtons buttons)
        {
            return Show(DefaultOwnerWindow, text, title, buttons);
        }

        /// <summary>
        /// Displays a task dialog in front of the specified window and with the specified text and title.
        /// </summary>
        /// <param name="owner">A window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(Window owner, string text, string title)
        {
            return Show(owner, text, title, TaskDialogCommonButtons.OK);
        }

        /// <summary>
        /// Displays a task dialog with specified text, title, buttons, and icon.
        /// </summary>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
        /// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon)
        {
            return Show(DefaultOwnerWindow, text, title, buttons, icon, TaskDialogHelpers.GetFirstButton(buttons));
        }

        /// <summary>
        /// Displays a task dialog in front of the specified window and with the specified text, title, and buttons.
        /// </summary>
        /// <param name="owner">A window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(Window owner, string text, string title, TaskDialogCommonButtons buttons)
        {
            return Show(owner, text, title, buttons, TaskDialogIcon.None);
        }

        /// <summary>
        /// Displays a task dialog with specified text, title, buttons, icon, and default button.
        /// </summary>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
        /// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
        /// <param name="defaultButton">TaskDialogResult value that specifies the default button for the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon, TaskDialogResult defaultButton)
        {
            return Show(DefaultOwnerWindow, text, title, buttons, icon, defaultButton);
        }

        /// <summary>
        /// Displays a task dialog in front of the specified window and with the specified text, title, buttons, and icon.
        /// </summary>
        /// <param name="owner">A window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the task dialog.</param>
        /// <param name="title">The text to display in the title bar of the task dialog.</param>
        /// <param name="buttons">One of the TaskDialogCommonButtons values that specifies which buttons to display in the task dialog.</param>
        /// <param name="icon">One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</param>
        /// <returns>One of the TaskDialogResult values.</returns>
        public static TaskDialogResult Show(Window owner, string text, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon)
        {
            return Show(owner, text, title, buttons, icon, TaskDialogHelpers.GetFirstButton(buttons));
        }

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
            TaskDialogWindow td = new TaskDialogWindow(null, false)
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
        public static TaskDialogResult Show(string windowTitle, string mainInstruction, string content, TaskDialogCommonButtons commonButtons, TaskDialogIcon icon)
        {
            return Show(DefaultOwnerWindow, windowTitle, mainInstruction, content, commonButtons, icon);
        }

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
            TaskDialogWindow td = new TaskDialogWindow(null, true) { Owner = owner, WindowTitle = windowTitle, MainIcon = icon, MainInstruction = mainInstruction, ContentText = content, CommonButtons = commonButtons };
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
            _TaskDialogWindow = new TaskDialogWindow(this, true);
            try
            {
                _TaskDialogWindow.Owner = TaskDialog.DefaultOwnerWindow;
            }
            catch { }
            _TaskDialogWindow.WindowTitle = TaskDialogHelpers.ProductName;
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// Displays a constructed task dialog.
        /// </summary>
        /// <returns>One of the TaskDialogResult values</returns>
        public TaskDialogResult Show()
        {
            _TaskDialogWindow.ShowDialog();
            return (TaskDialogResult)_TaskDialogWindow.Tag;
        }

        #endregion

        #region " Properties "


        public static float _customScale = 1f;
        public static float CustomScale
        {
            get { return _customScale; }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value > 0);

                _customScale = value;
            }
        }

        private TaskDialogWindow _TaskDialogWindow;

        /// <summary>
        /// Gets or sets the window that will own the modal dialog box.
        /// </summary>
        /// <value>The window that will own the modal dialog box.</value>
        public Window Owner
        {
            get
            {
                return _TaskDialogWindow.Owner;
            }
            set
            {
                _TaskDialogWindow.Owner = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to display in the title bar of the task dialog.
        /// </summary>
        /// <value>The text to display in the title bar of the task dialog.</value>
        public string WindowTitle
        {
            get
            {
                return _TaskDialogWindow.WindowTitle;
            }
            set
            {
                _TaskDialogWindow.WindowTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the values that specifies which icon to display in the task dialog.
        /// </summary>
        /// <value>One of the TaskDialogIcon values that specifies which icon to display in the task dialog.</value>
        public TaskDialogIcon MainIcon
        {
            get
            {
                return _TaskDialogWindow.MainIcon;
            }
            set
            {
                _TaskDialogWindow.MainIcon = value;
            }
        }

        /// <summary>
        /// Gets or sets the instruction to display in the task dialog.
        /// </summary>
        /// <value>The instruction to display in the task dialog.</value>
        public string MainInstruction
        {
            get
            {
                return _TaskDialogWindow.MainInstruction;
            }
            set
            {
                _TaskDialogWindow.MainInstruction = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to display in the task dialog.
        /// </summary>
        /// <value>The text to display in the task dialog.</value>
        public string ContentText
        {
            get
            {
                return _TaskDialogWindow.ContentText;
            }
            set
            {
                _TaskDialogWindow.ContentText = value;
            }
        }

        public Grid GetCustomContentPanel()
        {
            return _TaskDialogWindow.PanelCustomContent;
        }

        public TaskDialogCommonButtons CommonButtons
        {
            get
            {
                return _TaskDialogWindow.CommonButtons;
            }
            set
            {
                _TaskDialogWindow.CommonButtons = value;
            }
        }

        public IList<TaskDialogButton> Buttons
        {
            get
            {
                return _TaskDialogWindow.Buttons;
            }
            set
            {
                _TaskDialogWindow.Buttons = value;
            }
        }

        public TaskDialogResult DefaultButton
        {
            get
            {
                return _TaskDialogWindow.DefaultButton;
            }
            set
            {
                _TaskDialogWindow.DefaultButton = value;
            }
        }

        public bool EnableHyperlinks
        {
            get
            {
                return _TaskDialogWindow.EnableHyperlinks;
            }
            set
            {
                _TaskDialogWindow.EnableHyperlinks = value;
            }
        }

        public bool AllowDialogCancellation
        {
            get
            {
                return _TaskDialogWindow.AllowDialogCancellation;
            }
            set
            {
                _TaskDialogWindow.AllowDialogCancellation = value;
            }
        }

        public bool UseCommandLinks
        {
            get
            {
                return _TaskDialogWindow.UseCommandLinks;
            }
            set
            {
                _TaskDialogWindow.UseCommandLinks = value;
            }
        }

        public bool UseCommandLinksNoIcon
        {
            get
            {
                return _TaskDialogWindow.UseCommandLinksNoIcon;
            }
            set
            {
                _TaskDialogWindow.UseCommandLinksNoIcon = value;
            }
        }

        public bool IsPositionRelativeToWindow
        {
            get
            {
                return _TaskDialogWindow.IsPositionRelativeToWindow;
            }
            set
            {
                _TaskDialogWindow.IsPositionRelativeToWindow = value;
            }
        }

        public TaskDialogIcon FooterIcon
        {
            get
            {
                return _TaskDialogWindow.FooterIcon;
            }
            set
            {
                _TaskDialogWindow.FooterIcon = value;
            }
        }

        public string FooterText
        {
            get
            {
                return _TaskDialogWindow.FooterText;
            }
            set
            {
                _TaskDialogWindow.FooterText = value;
            }
        }

        public bool IsExpandedByDefault
        {
            get
            {
                return _TaskDialogWindow.IsExpanded;
            }
            set
            {
                _TaskDialogWindow.IsExpanded = value;
            }
        }

        public bool ExpandFooterArea
        {
            get
            {
                return _TaskDialogWindow.ExpandFooterArea;
            }
            set
            {
                _TaskDialogWindow.ExpandFooterArea = value;
            }
        }

        public string ExpandedInformation
        {
            get
            {
                return _TaskDialogWindow.ExpandedInformation;
            }
            set
            {
                _TaskDialogWindow.ExpandedInformation = value;
            }
        }

        public bool? IsVerificationFlagChecked
        {
            get
            {
                return _TaskDialogWindow.IsVerificationFlagChecked;
            }
            set
            {
                _TaskDialogWindow.IsVerificationFlagChecked = value;
            }
        }

        public IList<TaskDialogRadioButton> RadioButtons
        {
            get
            {
                return _TaskDialogWindow.RadioButtons;
            }
            set
            {
                _TaskDialogWindow.RadioButtons = value;
            }
        }

        public TaskDialogResult DefaultRadioButton
        {
            get
            {
                return _TaskDialogWindow.DefaultRadioButton;
            }
            set
            {
                _TaskDialogWindow.DefaultRadioButton = value;
            }
        }

        public string VerificationText
        {
            get
            {
                return _TaskDialogWindow.VerificationText;
            }
            set
            {
                _TaskDialogWindow.VerificationText = value;
            }
        }

        public int Width
        {
            get
            {
                return _TaskDialogWindow.ClientWidth;
            }
            set
            {
                _TaskDialogWindow.ClientWidth = value;
            }
        }

        public string ExpandedControlText
        {
            get
            {
                return _TaskDialogWindow.ExpandedControlText;
            }
            set
            {
                _TaskDialogWindow.ExpandedControlText = value;
            }
        }

        public string CollapsedControlText
        {
            get
            {
                return _TaskDialogWindow.CollapsedControlText;
            }
            set
            {
                _TaskDialogWindow.CollapsedControlText = value;
            }
        }

        public bool ShowProgressBar
        {
            get
            {
                return _TaskDialogWindow.ShowProgressBar;
            }
            set
            {
                _TaskDialogWindow.ShowProgressBar = value;
            }
        }

        public bool ShowMarqueeProgressBar
        {
            get
            {
                return _TaskDialogWindow.ShowMarqueeProgressBar;
            }
            set
            {
                _TaskDialogWindow.ShowMarqueeProgressBar = value;
            }
        }

        public TaskDialogProgressBarState ProgressBarState
        {
            get
            {
                return _TaskDialogWindow.ProgressBarState;
            }
            set
            {
                _TaskDialogWindow.ProgressBarState = value;
            }
        }

        public bool UseCallBackTimer
        {
            get
            {
                return _TaskDialogWindow.UseCallBackTimer;
            }
            set
            {
                _TaskDialogWindow.UseCallBackTimer = value;
            }
        }

        public bool RtlLayout
        {
            get
            {
                return _TaskDialogWindow.RtlLayout;
            }
            set
            {
                _TaskDialogWindow.RtlLayout = value;
            }
        }

        public bool CanBeMinimized
        {
            get
            {
                return _TaskDialogWindow.CanBeMinimized;
            }
            set
            {
                _TaskDialogWindow.CanBeMinimized = value;
            }
        }

        public double ProgressBarMinimum
        {
            get
            {
                return _TaskDialogWindow.ProgressBarMinimum;
            }
            set
            {
                _TaskDialogWindow.ProgressBarMinimum = value;
            }
        }

        public double ProgressBarMaximum
        {
            get
            {
                return _TaskDialogWindow.ProgressBarMaximum;
            }
            set
            {
                _TaskDialogWindow.ProgressBarMaximum = value;
            }
        }

        public double ProgressBarValue
        {
            get
            {
                return _TaskDialogWindow.ProgressBarValue;
            }
            set
            {
                _TaskDialogWindow.ProgressBarValue = value;
            }
        }

        #endregion

        #region " Events "

        public event EventHandler<CloseEventArgs> CanClose
        {
            add
            {
                _TaskDialogWindow.CanClose += value;
            }
            remove
            {
                _TaskDialogWindow.CanClose -= value;
            }
        }

        public event EventHandler<TaskDialogTimerEventArgs> Tick
        {
            add
            {
                _TaskDialogWindow.Tick += value;
            }
            remove
            {
                _TaskDialogWindow.Tick -= value;
            }
        }

        public event EventHandler<TaskDialogHyperlinkClickEventArgs> HyperlinkClick
        {
            add
            {
                _TaskDialogWindow.HyperlinkClick += value;
            }
            remove
            {
                _TaskDialogWindow.HyperlinkClick -= value;
            }
        }

        #endregion
    }
}
