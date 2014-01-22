#region License
// Copyright © 2014 £ukasz Œwi¹tkowski
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

namespace Luminous.Windows.Forms
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Luminous.Media;

    /// <summary>
    /// Displays a Vista-like message box or task dialog that can contain text, buttons, symbols and other elements that inform and instruct the user.
    /// </summary>
    public sealed class TaskDialog
    {
        #region MessageBox

        public static IWin32Window DefaultOwnerWindow { get; set; }

        /// <summary>
        /// Displays a Vista-like message box with specified text.
        /// </summary>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text)
        {
            return Show(DefaultOwnerWindow, text);
        }

        /// <summary>
        /// Displays a Vista-like message box in front of the specified object and with the specified text.
        /// </summary>
        /// <param name="owner">An implementation of IWin32Window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            string caption = "Application";
            var p = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault() as AssemblyProductAttribute;
            if (p != null) caption = p.Product;
            return Show(owner, text, caption);
        }

        /// <summary>
        /// Displays a Vista-like message box with specified text and caption.
        /// </summary>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption)
        {
            return Show(DefaultOwnerWindow, text, caption);
        }

        /// <summary>
        /// Displays a Vista-like message box in front of the specified object and with the specified text and caption.
        /// </summary>
        /// <param name="owner">An implementation of IWin32Window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return Show(owner, text, caption, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Displays a Vista-like message box with specified text, caption, and buttons.
        /// </summary>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show(DefaultOwnerWindow, text, caption, buttons);
        }

        /// <summary>
        /// Displays a Vista-like message box in front of the specified object and with the specified text, caption, and buttons.
        /// </summary>
        /// <param name="owner">An implementation of IWin32Window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, buttons, MessageBoxIcon.None);
        }

        /// <summary>
        /// Displays a Vista-like message box with specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the Vista-like message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(DefaultOwnerWindow, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays a Vista-like message box in front of the specified object and with the specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="owner">An implementation of IWin32Window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the Vista-like message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Displays a Vista-like message box with specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the Vista-like message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the Vista-like message box.</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButton values that specifies the default button for the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(DefaultOwnerWindow, text, caption, buttons, icon, defaultButton);
        }

        /// <summary>
        /// Displays a Vista-like message box in front of the specified object and with specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="owner">An implementation of IWin32Window that will own the modal dialog box.</param>
        /// <param name="text">The text to display in the Vista-like message box.</param>
        /// <param name="caption">The text to display in the title bar of the Vista-like message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the Vista-like message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the Vista-like message box.</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButton values that specifies the default button for the Vista-like message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            TaskDialog td = new TaskDialog();
            td.Content = text;
            td.WindowTitle = caption;
            switch (buttons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    td.Buttons = new TaskDialogButton[] { new TaskDialogButton(TaskDialogResult.Abort), new TaskDialogButton(TaskDialogResult.Retry), new TaskDialogButton(TaskDialogResult.Ignore) };
                    break;
                case MessageBoxButtons.OKCancel:
                    td.Buttons = new TaskDialogButton[] { new TaskDialogButton(TaskDialogResult.OK), new TaskDialogButton(TaskDialogResult.Cancel) };
                    break;
                case MessageBoxButtons.RetryCancel:
                    td.Buttons = new TaskDialogButton[] { new TaskDialogButton(TaskDialogResult.Retry), new TaskDialogButton(TaskDialogResult.Cancel) };
                    break;
                case MessageBoxButtons.YesNo:
                    td.Buttons = new TaskDialogButton[] { new TaskDialogButton(TaskDialogResult.Yes), new TaskDialogButton(TaskDialogResult.No) };
                    break;
                case MessageBoxButtons.YesNoCancel:
                    td.Buttons = new TaskDialogButton[] { new TaskDialogButton(TaskDialogResult.Yes), new TaskDialogButton(TaskDialogResult.No), new TaskDialogButton(TaskDialogResult.Cancel) };
                    break;
                default:
                    td.Buttons = new TaskDialogButton[] { new TaskDialogButton(TaskDialogResult.OK) };
                    break;
            }
            switch (icon)
            {
                case MessageBoxIcon.Information:
                    td.MainIcon = TaskDialogIcon.Information;
                    break;
                case MessageBoxIcon.Warning:
                    td.MainIcon = TaskDialogIcon.Warning;
                    break;
                case MessageBoxIcon.Error:
                    td.MainIcon = TaskDialogIcon.Error;
                    break;
                case MessageBoxIcon.Question:
                    td.MainIcon = TaskDialogIcon.Question;
                    break;
                default:
                    td.CustomMainIcon = null;
                    break;
            }
            td.DefaultButton = TaskDialogHelpers.MakeTaskDialogDefaultButton(defaultButton);
            td.Owner = owner;
            td.Show();
            return TaskDialogHelpers.MakeDialogResult(td.Result);
        }

        private static void SetStartPosition(Form f, IWin32Window o)
        {
            if (o == null)
            {
                f.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                f.StartPosition = FormStartPosition.CenterParent;
            }
        }

        #endregion

        #region TaskDialog

        #region Methods

        /// <summary>
        /// Shows the TaskDialog form as a modal dialog box.
        /// </summary>
        /// <returns>One of the TaskDialogResult values.</returns>
        public TaskDialogResult Show()
        {
            if (LockSystem)
            {
                return LockSystemAndShow();
            }
            else
            {
                return ShowInternal(Owner);
            }
        }

        private TaskDialogResult ShowInternal(IWin32Window Owner)
        {
            using (var tdf = new TaskDialogForm { TaskDialog = this })
            {
                _tdf = tdf;
                tdf.LinkClicked += tdf_LinkClicked;
                tdf.Load += tdf_Load;
                tdf.RightToLeft = RightToLeft;
                tdf.RightToLeftLayout = RightToLeftLayout;
                SetStartPosition(tdf, Owner);
                tdf.ShowDialog(Owner);
                _tdf = null;
                VerificationFlagChecked = tdf.CheckBoxState;
                Result = tdf.Tag is TaskDialogButton ? ((TaskDialogButton)tdf.Tag).Result
                                                     : (TaskDialogResult)(-1); // TaskDialog force-closed by exiting app
                return Result;
            }
        }

        private void tdf_LinkClicked(System.Object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LinkClicked != null)
            {
                LinkClicked(sender, e);
            }
        }

        private void tdf_Load(object sender, EventArgs e)
        {
            _tdf.SuspendLayouts();
            OnFormLoad(_tdf);
            _tdf.Content = Content;
            if (_contentLink != null && _contentLink != _tdf.LabelContent)
            {
                foreach (LinkLabel.Link link in ContentLinks)
                {
                    _tdf.LabelContent.Links.Add(link);
                }
                _contentLink.Dispose();
                _contentLink = _tdf.LabelContent;
            }
            _tdf.Caption = WindowTitle;
            _tdf.Title = MainInstruction;
            _tdf.VButtons = Buttons;
            _tdf.MainIcon = MainIcon;
            if (CustomMainIcon != null)
                _tdf.Image = CustomMainIcon;
            _tdf.FooterIcon = FooterIcon;
            if (CustomFooterIcon != null)
                _tdf.FooterImage = CustomFooterIcon;
            _tdf.FooterText = FooterText;
            if (_footer != null && _footer != _tdf.LabelFooter)
            {
                foreach (LinkLabel.Link link in FooterLinks)
                {
                    _tdf.LabelFooter.Links.Add(link);
                }
                _footer.Dispose();
                _footer = _tdf.LabelFooter;
            }
            _tdf.DefaultButton = DefaultButton;
            _tdf.CheckBoxState = VerificationFlagChecked;
            _tdf.CheckBoxText = VerificationText;
            _tdf.ExpandFooterArea = ExpandFooterArea;
            _tdf.ExpandedInformation = ExpandedInformation;
            if (_expanded != null && _expanded != (ExpandFooterArea ? _tdf.LabelExpandedFooter : _tdf.LabelExpandedContent))
            {
                foreach (LinkLabel.Link link in ExpandedInformationLinks)
                {
                    if (ExpandFooterArea)
                        _tdf.LabelExpandedFooter.Links.Add(link);
                    else
                        _tdf.LabelExpandedContent.Links.Add(link);
                }
                _expanded.Dispose();
                _expanded = ExpandFooterArea
                            ? _tdf.LabelExpandedFooter
                            : _tdf.LabelExpandedContent;
            }
            _tdf.Expanded = ExpandedByDefault;
            _tdf.ExpandedControlText = ExpandedControlText;
            _tdf.CollapsedControlText = CollapsedControlText;
            _tdf.CustomControl = CustomControl;
            if (Sound != null)
                _tdf.Sound = Sound;
            _tdf.ResumeLayouts();
        }

        private TaskDialogForm _tdf;

        /// <summary>
        /// Closes the TaskDialog form.
        /// </summary>
        /// <param name="result">A TaskDialogResult that represents the result of the form.</param>
        public void Close(TaskDialogResult result)
        {
            if (_tdf == null)
            {
                throw new InvalidOperationException("Cannot invoke this method before the dialog is shown, or after it is closed.");
            }
            var button = new TaskDialogButton(result);
            _tdf.Tag = button;
            if (button.Result != TaskDialogResult.None)
            {
                _tdf.DialogResult = DialogResult.OK;
            }
        }

        #endregion

        #region Fields and Properties

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

        private SystemSound _sound;
        /// <summary>
        /// Gets or sets a sound played when the Vista-like task dialog is shown.
        /// </summary>
        public SystemSound Sound
        {
            get
            {
                if (_lockSystem && _sound == null)
                {
                    return SoundScheme.WindowsUAC;
                }
                return _sound;
            }
            set { _sound = value; }
        }

        private IWin32Window _owner;
        /// <summary>
        /// Gets or sets an implementation of IWin32Window that will own the modal task dialog.
        /// </summary>
        public IWin32Window Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        private string _content;
        /// <summary>
        /// Gets or sets the text to display in the Vista-like task dialog.
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private LinkLabel _contentLink;
        /// <summary>
        /// The collection of links contained within the Content LinkLabel.
        /// </summary>
        public LinkLabel.LinkCollection ContentLinks
        {
            get
            {
                if (_contentLink == null)
                {
                    if (_tdf != null)
                    {
                        _contentLink = _tdf.LabelContent;
                    }
                    else
                    {
                        _contentLink = new LinkLabel();
                    }
                }
                return _contentLink.Links;
            }
        }

        private string _windowTitle;
        /// <summary>
        /// Gets or sets the text to display in the title bar of the Vista-like task dialog.
        /// </summary>
        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; }
        }

        private string _mainInstruction;
        /// <summary>
        /// Gets or sets the text to be used for the main instruction of the Vista-like task dialog.
        /// </summary>
        public string MainInstruction
        {
            get { return _mainInstruction; }
            set { _mainInstruction = value; }
        }

        private TaskDialogButton[] _vButtons;
        /// <summary>
        /// Gets or sets the array of the TaskDialogButtons that specifies which buttons to display in the Vista-like task dialog.
        /// </summary>
        public TaskDialogButton[] Buttons
        {
            get { return _vButtons; }
            set { _vButtons = value; }
        }

        private TaskDialogIcon _mainIcon;
        /// <summary>
        /// Gets or sets the one of the TaskDialogIcon values that specifies which icon on what background to display in the Vista-like task dialog.
        /// </summary>
        public TaskDialogIcon MainIcon
        {
            get { return _mainIcon; }
            set { _mainIcon = value; }
        }

        private Image _customMainIcon;
        /// <summary>
        /// Gets or sets the custom image to display in the Vista-like task dialog.
        /// </summary>
        public Image CustomMainIcon
        {
            get { return _customMainIcon; }
            set { _customMainIcon = value; }
        }

        private TaskDialogIcon _footerIcon;
        /// <summary>
        /// Gets or sets the one of the TaskDialogIcon values that specifies which icon to display in the footer of the Vista-like task dialog.
        /// </summary>
        public TaskDialogIcon FooterIcon
        {
            get { return _footerIcon; }
            set { _footerIcon = value; }
        }

        private Image _customFooterIcon;
        /// <summary>
        /// Gets or sets the custom image to display in the footer of the Vista-like task dialog.
        /// </summary>
        public Image CustomFooterIcon
        {
            get { return _customFooterIcon; }
            set { _customFooterIcon = value; }
        }

        private string _footerText;
        /// <summary>
        /// Gets or sets the text to display in the footer of the Vista-like task dialog.
        /// </summary>
        public string FooterText
        {
            get { return _footerText; }
            set { _footerText = value; }
        }

        private LinkLabel _footer;
        /// <summary>
        /// The collection of links contained within the Footer LinkLabel.
        /// </summary>
        public LinkLabel.LinkCollection FooterLinks
        {
            get
            {
                if (_footer == null)
                {
                    if (_tdf != null)
                    {
                        _footer = _tdf.LabelFooter;
                    }
                    else
                    {
                        _footer = new LinkLabel();
                    }
                }
                return _footer.Links;
            }
        }

        private TaskDialogDefaultButton _defaultButton = TaskDialogDefaultButton.Button1;
        /// <summary>
        /// Gets or sets one of the TaskDialogDefaultButton values that specifies the default button for the Vista-like task dialog.
        /// </summary>
        public TaskDialogDefaultButton DefaultButton
        {
            get { return _defaultButton; }
            set { _defaultButton = value; }
        }

        private RightToLeft _rightToLeft;
        /// <summary>
        /// Gets or sets a value indicating whether the text appears from right to left, such as when using Hebrew or Arabic fonts.
        /// </summary>
        public RightToLeft RightToLeft
        {
            get { return _rightToLeft; }
            set { _rightToLeft = value; }
        }

        private bool _rightToLeftLayout;
        /// <summary>
        /// Gets or sets a value indicating whether right-to-left mirror placement is turned on.
        /// </summary>
        public bool RightToLeftLayout
        {
            get { return _rightToLeftLayout; }
            set { _rightToLeftLayout = value; }
        }

        private TaskDialogResult _result;
        /// <summary>
        /// Gets or sets the dialog result for the TaskDialog form.
        /// </summary>
        public TaskDialogResult Result
        {
            get { return _result; }
            set { _result = value; }
        }

        private bool _lockSystem;
        /// <summary>
        /// Determines whether to lock system while showing the Vista-like task dialog.
        /// </summary>
        public bool LockSystem
        {
            get { return _lockSystem; }
            set { _lockSystem = value; }
        }

        private Control _control;
        /// <summary>
        /// Gets or sets the custom control to display in the Vista-like task dialog.
        /// </summary>
        public Control CustomControl
        {
            get { return _control; }
            set { _control = value; }
        }

        private CheckState _verificationFlagChecked;
        /// <summary>
        /// Gets or sets a value indicating whether the verification checkbox in the dialog should be checked when the dialog is initially displayed.
        /// </summary>
        public CheckState VerificationFlagChecked
        {
            get { return _verificationFlagChecked; }
            set { _verificationFlagChecked = value; }
        }

        private string _verificationText;
        /// <summary>
        /// Gets or sets the string to be used to label the verification checkbox. If this parameter is Nothing (null), the verification checkbox is not displayed in the Vista-like task dialog.
        /// </summary>
        public string VerificationText
        {
            get { return _verificationText; }
            set { _verificationText = value; }
        }

        private bool _expandFooterArea;
        /// <summary>
        /// Gets or sets a value indicating whether the the string specified by the ExpandedInformation property should be displayed at the bottom of the Vista-like task dialog's footer area instead of immediately after the Vista-like task dialog's content.
        /// </summary>
        public bool ExpandFooterArea
        {
            get { return _expandFooterArea; }
            set { _expandFooterArea = value; }
        }

        private bool _expandedByDefault;
        /// <summary>
        /// Gets or sets a value indicating whether the string specified by the ExpandedInformation property should be displayed when the Vista-like task dialog is initially displayed.
        /// </summary>
        public bool ExpandedByDefault
        {
            get { return _expandedByDefault; }
            set { _expandedByDefault = value; }
        }

        private string _expandedInformation;
        /// <summary>
        /// Gets or sets the string to be used for displaying additional information. The additional information is displayed either immediately below the content or below the footer text depending on whether the ExpandFooterArea property is set.
        /// </summary>
        public string ExpandedInformation
        {
            get { return _expandedInformation; }
            set { _expandedInformation = value; }
        }

        private LinkLabel _expanded;
        /// <summary>
        /// The collection of links contained within the Footer LinkLabel.
        /// </summary>
        public LinkLabel.LinkCollection ExpandedInformationLinks
        {
            get
            {
                if (_expanded == null)
                {
                    if (_tdf != null)
                    {
                        _expanded = ExpandFooterArea
                                    ? _tdf.LabelExpandedFooter
                                    : _tdf.LabelExpandedContent;
                    }
                    else
                    {
                        _expanded = new LinkLabel();
                    }
                }
                return _expanded.Links;
            }
        }

        private string _expandedControlText;
        /// <summary>
        /// Gets or sets the string to be used to label the button for collapsing the expandable information.
        /// </summary>
        public string ExpandedControlText
        {
            get { return _expandedControlText; }
            set { _expandedControlText = value; }
        }

        private string _collapsedControlText;
        /// <summary>
        /// Gets or sets the string to be used to label the button for expanding the expandable information.
        /// </summary>
        public string CollapsedControlText
        {
            get { return _collapsedControlText; }
            set { _collapsedControlText = value; }
        }

        #endregion

        #region System Locking Methods

        private TaskDialogResult LockSystemAndShow()
        {
            Control owner = this.Owner as Control;
            Screen scr;
            if (owner == null)
            {
                scr = Screen.PrimaryScreen;
            }
            else
            {
                scr = Screen.FromControl(owner);
            }
            using (Bitmap background = new Bitmap(scr.Bounds.Width, scr.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(background))
                {
                    g.CopyFromScreen(0, 0, 0, 0, scr.Bounds.Size);
                    using (Brush br = new SolidBrush(Color.FromArgb(192, Color.Black)))
                    {
                        g.FillRectangle(br, scr.Bounds);
                    }

                    if (owner != null)
                    {
                        Form form = owner.FindForm();
                        g.CopyFromScreen(form.Location, form.Location, form.Size);
                        using (Brush br = new SolidBrush(Color.FromArgb(128, Color.Black)))
                        {
                            g.FillRectangle(br, new Rectangle(form.Location, form.Size));
                        }
                    }
                }

                IntPtr originalThread;
                IntPtr originalInput;
                IntPtr newDesktop;

                originalThread = Native.GetThreadDesktop((uint)Thread.CurrentThread.ManagedThreadId);
                originalInput = Native.OpenInputDesktop(0, false, Native.DESKTOP_SWITCHDESKTOP);

                newDesktop = Native.CreateDesktop("Desktop" + Guid.NewGuid(), null, null, 0, Native.GENERIC_ALL, IntPtr.Zero);
                Native.SetThreadDesktop(newDesktop);
                Native.SwitchDesktop(newDesktop);

                Thread newThread = new Thread(NewThreadMethod);
                newThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                newThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                newThread.Start(new TaskDialogLockSystemParameters(newDesktop, background));
                newThread.Join();

                Native.SwitchDesktop(originalInput);
                Native.SetThreadDesktop(originalThread);

                Native.CloseDesktop(newDesktop);
                Native.CloseDesktop(originalInput);

                return Result;
            }
        }

        private void NewThreadMethod(object @params)
        {
            TaskDialogLockSystemParameters p = (TaskDialogLockSystemParameters)@params;
            Native.SetThreadDesktop(p.NewDesktop);
            using (Form f = new BackgroundForm(p.Background))
            {
                f.Show();
                ShowInternal(f);
                f.BackgroundImage = null;
                Application.DoEvents();
                Thread.Sleep(250);
            }
        }
        #endregion

        #region Events

        public event LinkLabelLinkClickedEventHandler LinkClicked;

        #endregion

        #endregion

        #region Custom Theming

        public static Func<IButtonControlWithImage> ButtonFactory = new Func<IButtonControlWithImage>(() => new Button() { AutoSizeMode = AutoSizeMode.GrowAndShrink, TextImageRelation = TextImageRelation.ImageBeforeText });

        internal static void OnFormConstructed(Form form)
        {
            FormConstructed(form, EventArgs.Empty);
        }

        private void OnFormLoad(Form form)
        {
            FormLoad(form, EventArgs.Empty);
        }

        internal void OnFormShown(Form form)
        {
            FormShown(form, EventArgs.Empty);
        }

        internal void OnFormClosed(Form form)
        {
            FormClosed(form, EventArgs.Empty);
        }

        public static event EventHandler FormConstructed = delegate { };
        public event EventHandler FormLoad = delegate { };
        public event EventHandler FormShown = delegate { };
        public event EventHandler FormClosed = delegate { };

        #endregion
    }
}