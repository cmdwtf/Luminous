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

namespace Luminous.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Luminous.Media;
    using Luminous.Windows.Controls;

    /// <summary>
    /// Interaction logic for TaskDialogWindow.xaml
    /// </summary>
    internal partial class TaskDialogWindow : Window
    {
        #region " Constructors "

        protected TaskDialogWindow()
        {
            InitializeComponent();
        }

        public TaskDialogWindow(TaskDialog taskDialog, bool isTaskDialog)
            : this()
        {
            this._TaskDialog = taskDialog;
            this.CustomScale.ScaleX = this.CustomScale.ScaleY = TaskDialog.CustomScale;
            if (TaskDialog.CustomScale != 1f)
            {
                TextOptions.SetTextFormattingMode(this, TextFormattingMode.Ideal);
            }

            if (TaskDialogHelpers.IsSegoeUIInstalled)
            {
                this.FontFamily = new FontFamily(TaskDialogHelpers.SegoeUIFontFamily);
                this.FontSize = 12;
            }

            _Sound = SoundScheme.Default;
            ImageMainIcon.Visibility = Visibility.Collapsed;
            if (!(_IsTaskDialog = isTaskDialog))
            {
                LabelContent.Margin = new Thickness(24);
                ImageMainIcon.Margin = new Thickness(24, 18, -12, 24);
                PanelButtons.Margin = new Thickness(0);
            }
            TextBlockExpanderCollapsed.Text = CollapsedControlText;
            TextBlockExpanderExpanded.Text = ExpandedControlText;
            RecreateButtons();
            RecalculateWidth();
        }

        #endregion

        #region " Properties "

        public new Window Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                if (Owner != value)
                {
                    if (value == null) value = TaskDialog.DefaultOwnerWindow;
                    base.Owner = value;
                    UpdateStartupLocation();
                    ShowInTaskbar = value == null;
                }
            }
        }

        public string WindowTitle
        {
            get
            {
                return this.Title;
            }
            set
            {
                if (WindowTitle != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        value = TaskDialogHelpers.ProductName;
                    }
                    this.Title = value;
                }
            }
        }

        private TaskDialogIcon _MainIcon;
        public TaskDialogIcon MainIcon
        {
            get
            {
                return _MainIcon;
            }
            set
            {
                if (MainIcon != value)
                {
                    _MainIcon = value;
                    ImageMainIcon.BeginInit();
                    ImageMainIcon.Visibility = Visibility.Visible;
                    switch (value)
                    {
                        case TaskDialogIcon.Information:
                            //ImageMainIcon.Source = new BitmapImage(new Uri("pack://application:,,,/TaskDialog/Resources/Information.png"));
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Information.png", UriKind.Relative));
                            _Sound = SoundScheme.Information;
                            break;
                        case TaskDialogIcon.Question:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Question.png", UriKind.Relative));
                            _Sound = SoundScheme.Question;
                            break;
                        case TaskDialogIcon.Warning:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Warning.png", UriKind.Relative));
                            _Sound = SoundScheme.Warning;
                            break;
                        case TaskDialogIcon.Error:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Error.png", UriKind.Relative));
                            _Sound = SoundScheme.Error;
                            break;
                        case TaskDialogIcon.SecuritySuccess:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecuritySuccess.png", UriKind.Relative));
                            _Sound = SoundScheme.Information;
                            break;
                        case TaskDialogIcon.SecurityQuestion:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecurityQuestion.png", UriKind.Relative));
                            _Sound = SoundScheme.Question;
                            break;
                        case TaskDialogIcon.SecurityWarning:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecurityWarning.png", UriKind.Relative));
                            _Sound = SoundScheme.Warning;
                            break;
                        case TaskDialogIcon.SecurityError:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecurityError.png", UriKind.Relative));
                            _Sound = SoundScheme.Error;
                            break;
                        case TaskDialogIcon.SecurityShield:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Security.png", UriKind.Relative));
                            _Sound = SoundScheme.Security;
                            break;
                        case TaskDialogIcon.SecurityShieldBlue:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Security.png", UriKind.Relative));
                            _Sound = SoundScheme.Security;
                            break;
                        case TaskDialogIcon.SecurityShieldGray:
                            ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Security.png", UriKind.Relative));
                            _Sound = SoundScheme.Security;
                            break;
                        default:
                            ImageMainIcon.Source = null;
                            ImageMainIcon.Visibility = Visibility.Collapsed;
                            _Sound = SoundScheme.Default;
                            break;
                    }
                    UpdateMainIconPosition();
                    ImageMainIcon.EndInit();
                    RecalculateWidth();
                }
            }
        }

        public string MainInstruction
        {
            get
            {
                return LabelMainInstruction.Text;
            }
            set
            {
                if (MainInstruction != value)
                {
                    LabelMainInstruction.Text = value;
                    LabelMainInstruction.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                    UpdateMainIconPosition();
                    RecalculateWidth();
                }
            }
        }

        private string _ContetTextWithoutHyperlinks { get; set; }
        public string ContentText
        {
            get
            {
                return LabelContent.Text;
            }
            set
            {
                if (ContentText != value)
                {
                    _ContetTextWithoutHyperlinks = SetText(LabelContent, value);
                    LabelContent.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                    RecalculateWidth();
                }
            }
        }

        private TaskDialogCommonButtons _CommonButtons;
        public TaskDialogCommonButtons CommonButtons
        {
            get
            {
                return _CommonButtons;
            }
            set
            {
                if (CommonButtons != value)
                {
                    _CommonButtons = value;
                    RecreateButtons();
                    RecalculateWidth();
                }
            }
        }

        private IList<TaskDialogButton> _Buttons;
        public IList<TaskDialogButton> Buttons
        {
            get
            {
                return _Buttons;
            }
            set
            {
                if (Buttons != value)
                {
                    _Buttons = value;
                    RecreateButtons();
                    RecalculateWidth();
                }
            }
        }

        private TaskDialogResult _DefaultButton;
        public TaskDialogResult DefaultButton
        {
            get
            {
                return _DefaultButton;
            }
            set
            {
                if (DefaultButton != value)
                {
                    _DefaultButton = value;
                    SetDefaultButton();
                }
            }
        }

        private bool _EnableHyperlinks;
        public bool EnableHyperlinks
        {
            get
            {
                return _EnableHyperlinks;
            }
            set
            {
                if (EnableHyperlinks != value)
                {
                    _EnableHyperlinks = value;
                    RefreshTexts();
                    RecalculateWidth();
                }
            }
        }

        private bool _AllowDialogCancellation;
        public bool AllowDialogCancellation
        {
            get
            {
                return _AllowDialogCancellation;
            }
            set
            {
                if (AllowDialogCancellation != value)
                {
                    _AllowDialogCancellation = value;
                    RecheckCancellation();
                }
            }
        }

        private bool _UseCommandLinks;
        public bool UseCommandLinks
        {
            get
            {
                return _UseCommandLinks;
            }
            set
            {
                if (UseCommandLinks != value)
                {
                    _UseCommandLinks = value;
                    RecreateButtons();
                    RecalculateWidth();
                }
            }
        }

        private bool _UseCommandLinksNoIcon;
        public bool UseCommandLinksNoIcon
        {
            get
            {
                return _UseCommandLinksNoIcon;
            }
            set
            {
                if (UseCommandLinksNoIcon != value)
                {
                    _UseCommandLinksNoIcon = value;
                    RecreateButtons();
                    RecalculateWidth();
                }
            }
        }

        private bool _IsPositionRelativeToWindow;
        public bool IsPositionRelativeToWindow
        {
            get
            {
                return _IsPositionRelativeToWindow;
            }
            set
            {
                if (IsPositionRelativeToWindow != value)
                {
                    _IsPositionRelativeToWindow = value;
                    UpdateStartupLocation();
                }
            }
        }

        private TaskDialogIcon _FooterIcon;
        public TaskDialogIcon FooterIcon
        {
            get
            {
                return _FooterIcon;
            }
            set
            {
                if (FooterIcon != value)
                {
                    _FooterIcon = value;
                    ImageFooterIcon.BeginInit();
                    ImageFooterIcon.Visibility = Visibility.Visible;
                    switch (value)
                    {
                        case TaskDialogIcon.Information:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallInformation.png", UriKind.Relative));
                            _Sound = SoundScheme.Information;
                            break;
                        case TaskDialogIcon.Question:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallQuestion.png", UriKind.Relative));
                            _Sound = SoundScheme.Question;
                            break;
                        case TaskDialogIcon.Warning:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallWarning.png", UriKind.Relative));
                            _Sound = SoundScheme.Warning;
                            break;
                        case TaskDialogIcon.Error:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallError.png", UriKind.Relative));
                            _Sound = SoundScheme.Error;
                            break;
                        case TaskDialogIcon.SecuritySuccess:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecuritySuccess.png", UriKind.Relative));
                            _Sound = SoundScheme.Information;
                            break;
                        case TaskDialogIcon.SecurityQuestion:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurityQuestion.png", UriKind.Relative));
                            _Sound = SoundScheme.Question;
                            break;
                        case TaskDialogIcon.SecurityWarning:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurityWarning.png", UriKind.Relative));
                            _Sound = SoundScheme.Warning;
                            break;
                        case TaskDialogIcon.SecurityError:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurityError.png", UriKind.Relative));
                            _Sound = SoundScheme.Error;
                            break;
                        case TaskDialogIcon.SecurityShield:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurity.png", UriKind.Relative));
                            _Sound = SoundScheme.Security;
                            break;
                        case TaskDialogIcon.SecurityShieldBlue:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurity.png", UriKind.Relative));
                            _Sound = SoundScheme.Security;
                            break;
                        case TaskDialogIcon.SecurityShieldGray:
                            ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurity.png", UriKind.Relative));
                            _Sound = SoundScheme.Security;
                            break;
                        default:
                            ImageFooterIcon.Source = null;
                            ImageFooterIcon.Visibility = Visibility.Collapsed;
                            _Sound = SoundScheme.Default;
                            break;
                    }
                    ImageFooterIcon.EndInit();
                    RecalculateWidth();
                }
            }
        }

        private string _FooterTextWithoutHyperlinks { get; set; }
        public string FooterText
        {
            get
            {
                return LabelFooter.Text;
            }
            set
            {
                if (FooterText != value)
                {
                    _FooterTextWithoutHyperlinks = SetText(LabelFooter, value);
                    PanelFooter.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                    RecalculateWidth();
                }
            }
        }

        public bool IsExpanded
        {
            get
            {
                return Expander.IsExpanded;
            }
            set
            {
                if (IsExpanded != value)
                {
                    Expander.IsExpanded = value;
                    UpdateExpander();
                    RecalculateWidth();
                }
            }
        }

        private bool _ExpandFooterArea;
        public bool ExpandFooterArea
        {
            get
            {
                return _ExpandFooterArea;
            }
            set
            {
                if (ExpandFooterArea != value)
                {
                    _ExpandFooterArea = value;
                    UpdateExpander();
                }
            }
        }

        private string _ExpandedInformationWithoutHyperlinks { get; set; }
        public string ExpandedInformation
        {
            get
            {
                return LabelExpandedInfo.Text;
            }
            set
            {
                if (ExpandedInformation != value)
                {
                    _ExpandedInformationWithoutHyperlinks = SetText(LabelExpandedInfo, value);
                    SetText(LabelExpandedFooterInfo, value);
                    UpdateExpander();
                    RecalculateWidth();
                }
            }
        }

        public bool? IsVerificationFlagChecked
        {
            get
            {
                return CheckBoxVerification.IsChecked;
            }
            set
            {
                if (IsVerificationFlagChecked != value)
                {
                    CheckBoxVerification.IsChecked = value;
                }
            }
        }

        private IList<TaskDialogRadioButton> _RadioButtons;
        public IList<TaskDialogRadioButton> RadioButtons
        {
            get
            {
                return _RadioButtons;
            }
            set
            {
                if (RadioButtons != value)
                {
                    _RadioButtons = value;
                    RecreateRadioButtons();
                    RecalculateWidth();
                }
            }
        }

        private TaskDialogResult _DefaultRadioButton;
        public TaskDialogResult DefaultRadioButton
        {
            get
            {
                return _DefaultRadioButton;
            }
            set
            {
                if (DefaultRadioButton != value)
                {
                    _DefaultRadioButton = value;
                    SetDefaultRadioButton();
                }
            }
        }

        private string _VerificationText;
        public string VerificationText
        {
            get
            {
                return _VerificationText;
            }
            set
            {
                if (VerificationText != value)
                {
                    CheckBoxVerification.Content = _VerificationText = value;
                    CheckBoxVerification.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                    RecalculateWidth();
                }
            }
        }

        private int _ClientWidth;
        public int ClientWidth
        {
            get
            {
                return _ClientWidth;
            }
            set
            {
                if (_ClientWidth != value)
                {
                    _ClientWidth = value;
                    if (value == 0)
                    {
                        ProgressBar.MinWidth = 355;
                        RecalculateWidth();
                    }
                    else
                    {
                        ProgressBar.MinWidth = 0;
                        MaxWidth = value;
                    }
                }
            }
        }

        private string _ExpandedControlText;
        public string ExpandedControlText
        {
            get
            {
                return string.IsNullOrEmpty(_ExpandedControlText) ? Properties.Resources.ExpandedText : _ExpandedControlText;
            }
            set
            {
                if (_ExpandedControlText != value)
                {
                    _ExpandedControlText = value;
                    UpdateExpander();
                    RecalculateWidth();
                }
            }
        }

        private string _CollapsedControlText;
        public string CollapsedControlText
        {
            get
            {
                return string.IsNullOrEmpty(_CollapsedControlText) ? Properties.Resources.CollapsedText : _CollapsedControlText;
            }
            set
            {
                if (_CollapsedControlText != value)
                {
                    _CollapsedControlText = value;
                    UpdateExpander();
                    RecalculateWidth();
                }
            }
        }

        public bool ShowProgressBar
        {
            get
            {
                return ProgressBar.Visibility == Visibility.Visible;
            }
            set
            {
                if (ShowProgressBar != value)
                {
                    ProgressBar.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public bool ShowMarqueeProgressBar
        {
            get
            {
                return ProgressBar.IsIndeterminate;
            }
            set
            {
                if (ShowMarqueeProgressBar != value)
                {
                    ProgressBar.IsIndeterminate = value;
                }
            }
        }

        private TaskDialogProgressBarState _ProgressBarState;
        public TaskDialogProgressBarState ProgressBarState
        {
            get
            {
                return _ProgressBarState;
            }
            set
            {
                if (ProgressBarState != value)
                {
                    _ProgressBarState = value;
                    switch (value)
                    {
                        case TaskDialogProgressBarState.Pause:
                            ProgressBar.Foreground = Resources["ProgressBarPause"] as Brush;
                            break;
                        case TaskDialogProgressBarState.Error:
                            ProgressBar.Foreground = Resources["ProgressBarError"] as Brush;
                            break;
                        default:
                            ProgressBar.SetValue(ProgressBar.ForegroundProperty, DependencyProperty.UnsetValue);
                            break;
                    }
                }
            }
        }

        private bool _UseCallBackTimer;
        public bool UseCallBackTimer
        {
            get
            {
                return _UseCallBackTimer;
            }
            set
            {
                if (UseCallBackTimer != value)
                {
                    _UseCallBackTimer = value;
                    if (value)
                    {
                        CreateTimer();
                        if (IsVisible)
                        {
                            StartTimer();
                        }
                    }
                    else
                    {
                        DeleteTimer();
                    }
                }
            }
        }

        public bool RtlLayout
        {
            get
            {
                return FlowDirection == FlowDirection.RightToLeft;
            }
            set
            {
                if (RtlLayout != value)
                {
                    FlowDirection = value ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
                }
            }
        }

        private bool _CanBeMinimized;
        public bool CanBeMinimized
        {
            get
            {
                return _CanBeMinimized;
            }
            set
            {
                if (CanBeMinimized != value)
                {
                    _CanBeMinimized = true;
                    ResizeMode = value ? ResizeMode.CanMinimize : ResizeMode.NoResize;
                }
            }
        }

        public double ProgressBarMinimum
        {
            get
            {
                return ProgressBar.Minimum;
            }
            set
            {
                if (ProgressBarMinimum != value)
                {
                    ProgressBar.Minimum = value;
                }
            }
        }

        public double ProgressBarMaximum
        {
            get
            {
                return ProgressBar.Maximum;
            }
            set
            {
                if (ProgressBarMaximum != value)
                {
                    ProgressBar.Maximum = value;
                }
            }
        }

        public double ProgressBarValue
        {
            get
            {
                return ProgressBar.Value;
            }
            set
            {
                if (ProgressBarValue != value)
                {
                    ProgressBar.Value = value;
                }
            }
        }

        #endregion

        #region " Events "

        public event EventHandler<CloseEventArgs> CanClose;
        public event EventHandler<TaskDialogTimerEventArgs> Tick;
        public event EventHandler<TaskDialogHyperlinkClickEventArgs> HyperlinkClick;

        #endregion

        #region " Private Properties "

        internal readonly bool _IsTaskDialog;
        private List<Hyperlink> _Hyperlinks = new List<Hyperlink>();
        private SystemSound _Sound { get; set; }
        private bool _IsCancellable { get; set; }
        private System.Windows.Interop.HwndSource _HwndSource;
        private bool? _IsOnlyOK { get; set; }
        private Timer _Timer;
        private DateTime? _LastTick;
        private TaskDialog _TaskDialog;
        private Button _DefaultButtonToFocus;

        #endregion

        #region " Overrided Methods "

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                try
                {
                    SoundScheme.Default.Play();
                    Clipboard.SetText(_IsTaskDialog ? TaskDialogHelpers.TaskDialogToString(this) : TaskDialogHelpers.MessageBoxToString(this));
                }
                catch { }
            }
            base.OnPreviewKeyDown(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (!_IsCancellable)
            {
                DisableCloseButton();
            }
            else
            {
                EnableCloseButton();
            }
            TaskDialogHelpers.RemoveWindowIcon(this);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (Tag == null)
            {
                if (_IsOnlyOK == true)
                {
                    Tag = TaskDialogResult.OK;
                }
                else
                {
                    Tag = TaskDialogResult.Cancel;
                }
            }
            if (CanClose != null)
            {
                CloseEventArgs cea;
                CanClose(this, cea = new CloseEventArgs(e.Cancel, (TaskDialogResult)Tag));
                if (cea.Cancel)
                {
                    e.Cancel = true;
                    base.OnClosing(e);
                    return;
                }
            }
            DeleteTimer();
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            foreach (Hyperlink h in _Hyperlinks)
            {
                h.Click -= Hyperlink_Click;
            }
            _Hyperlinks.Clear();
            DeleteButtons();
            DeleteRadioButtons();
        }

        #endregion

        #region " Event Handlers "

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_DefaultButtonToFocus != null)
            {
                _DefaultButtonToFocus.Focus();
            }
            if (_Sound != null)
            {
                _Sound.Play();
            }
            StartTimer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EnableCloseButton();
            object tag = (sender as Button).Tag;
            if (tag is TaskDialogButton)
            {
                Tag = ((TaskDialogButton)tag).Result;
            }
            else
            {
                Tag = TaskDialogHelpers.TaskDialogCommonButtonsToTaskDialogResult((TaskDialogCommonButtons)tag);
            }
            Close();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            UpdateExpander();
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            UpdateExpander();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_LastTick == null)
            {
                _LastTick = DateTime.Now;
            }
            if (Tick != null)
            {
                TaskDialogTimerEventArgs ea;
                Tick(_TaskDialog, ea = new TaskDialogTimerEventArgs { Interval = DateTime.Now - _LastTick.Value });
                if (ea.Reset)
                {
                    _LastTick = DateTime.Now;
                }
            }
        }

        #endregion

        #region " Private Methods "

        private void DeleteTimer()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer.Dispose();
                _Timer = null;
            }
        }

        private void StartTimer()
        {
            if (_Timer != null)
            {
                _Timer.Start();
                _LastTick = DateTime.Now;
            }
        }

        private void StopTimer()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
            }
        }

        private void CreateTimer()
        {
            DeleteTimer();
            _Timer = new Timer(200);
            _Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
        }

        private void RecalculateWidth()
        {
            double w = SystemParameters.PrimaryScreenWidth * .75;
            if (w < 640) w = 640;
            if (_IsTaskDialog && w > 800) w = 800;
            if (MaxWidth != w)
            {
                MaxWidth = w;
            }
        }

        private void UpdateExpander()
        {
            TextBlockExpanderCollapsed.Visibility = IsExpanded ? Visibility.Hidden : Visibility.Visible;
            TextBlockExpanderExpanded.Visibility = IsExpanded ? Visibility.Visible : Visibility.Hidden;
            if (string.IsNullOrEmpty(_ExpandedInformationWithoutHyperlinks))
            {
                Expander.Visibility = LabelExpandedInfo.Visibility = PanelFooterExpandedInfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                Expander.Visibility = Visibility.Visible;
                if (!IsExpanded)
                {
                    LabelExpandedInfo.Visibility = PanelFooterExpandedInfo.Visibility = Visibility.Collapsed;
                }
                else
                {
                    LabelExpandedInfo.Visibility = ExpandFooterArea ? Visibility.Collapsed : Visibility.Visible;
                    PanelFooterExpandedInfo.Visibility = ExpandFooterArea ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void UpdateStartupLocation()
        {
            WindowStartupLocation = (Owner == null || (_IsTaskDialog && !IsPositionRelativeToWindow)) ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
        }

        private void EnableCloseButton()
        {
            RemoveHook();
            TaskDialogHelpers.EnableCloseButton(this, true);
        }

        private void DisableCloseButton()
        {
            RemoveHook();
            AddHook();
            TaskDialogHelpers.EnableCloseButton(this, false);
        }

        private void AddHook()
        {
            _HwndSource = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            if (_HwndSource != null)
            {
                _HwndSource.AddHook(TaskDialogHelpers.CloseButtonHook);
            }
        }

        private void RemoveHook()
        {
            if (_HwndSource != null)
            {
                _HwndSource.RemoveHook(TaskDialogHelpers.CloseButtonHook);
                _HwndSource = null;
            }
        }

        private string SetText(TextBlock label, string text)
        {
            foreach (Hyperlink h in _Hyperlinks)
            {
                if (label.Inlines.Contains(h))
                {
                    h.Click -= Hyperlink_Click;
                }
            }
            for (int i = _Hyperlinks.Count - 1; i >= 0; i--)
            {
                if (label.Inlines.Contains(_Hyperlinks[i]))
                {
                    _Hyperlinks.RemoveAt(i);
                }
            }
            string ret = label.Text = text;
            if (EnableHyperlinks)
            {
                List<string> parts = text.Split('<').ToList();
                label.Inlines.Clear();
                if (parts.Count > 0)
                {
                    label.Inlines.Add(ret = parts[0]);
                    parts.RemoveAt(0);
                }
                while (parts.Count > 0)
                {
                    string part = parts[0];
                    parts.RemoveAt(0);
                    if (part.ToUpperInvariant().StartsWith("A "))
                    {
                        string part2 = part.Substring(2).TrimStart();
                        if (part2.ToUpperInvariant().StartsWith("HREF=\""))
                        {
                            string part3 = part2.Substring(6);
                            if (part3.Contains('"'))
                            {
                                int q = part3.IndexOf('"');
                                string href = part3.Substring(0, q);
                                string part4 = part3.Substring(q);
                                if (part4.StartsWith("\">"))
                                {
                                    string htext = part4.Substring(2);
                                    if (parts.Count > 0)
                                    {
                                        string part5 = parts[0];
                                        parts.RemoveAt(0);
                                        if (part5.ToUpperInvariant().StartsWith("/A>"))
                                        {
                                            ret += htext;
                                            Hyperlink h = new Hyperlink(new Run(htext)) { Tag = href };
                                            h.Click += new RoutedEventHandler(Hyperlink_Click);
                                            label.Inlines.Add(h);
                                            string rest = part5.Substring(3);
                                            if (!string.IsNullOrEmpty(rest))
                                            {
                                                ret += rest;
                                                label.Inlines.Add(rest);
                                            }
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ret += "<" + part;
                    label.Inlines.Add("<" + part);
                }
            }
            return ret;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (HyperlinkClick != null)
            {
                HyperlinkClick(_TaskDialog, new TaskDialogHyperlinkClickEventArgs { Href = (sender as Hyperlink).Tag as string });
            }
        }

        private void RecheckCancellation()
        {
            bool cancel = ((CommonButtons & TaskDialogCommonButtons.Cancel) != TaskDialogCommonButtons.None) ||
                (Buttons != null && Buttons.Any(b => b.Result == TaskDialogResult.Cancel));
            _IsCancellable = cancel || (_IsOnlyOK == true) || AllowDialogCancellation;
            if (!_IsCancellable)
            {
                DisableCloseButton();
            }
            else
            {
                EnableCloseButton();
            }
        }

        private void RefreshTexts()
        {
            string t = ContentText;
            ContentText = string.Empty;
            ContentText = t;

            t = ExpandedInformation;
            ExpandedInformation = string.Empty;
            ExpandedInformation = t;

            t = FooterText;
            FooterText = string.Empty;
            FooterText = t;
        }

        private void DeleteButtons()
        {
            foreach (CommandLink cl in PanelCommandLinks.Children)
            {
                cl.Click -= Button_Click;
                cl.Tag = null;
            }
            PanelCommandLinks.Children.Clear();
            foreach (Button b in PanelRightButtons.Children)
            {
                b.Click -= Button_Click;
                b.Tag = null;
            }
            PanelRightButtons.Children.Clear();
        }

        private void RecreateButtons()
        {
            DeleteButtons();
            _IsOnlyOK = null;
            bool areButtons = Buttons != null && Buttons.Count > 0;
            PanelCommandLinks.Visibility = PanelRightButtons.Visibility = Visibility.Collapsed;
            if (areButtons)
            {
                (UseCommandLinks ? (Panel)PanelCommandLinks : (Panel)PanelRightButtons).Visibility = Visibility.Visible;
                foreach (TaskDialogButton b in Buttons)
                {
                    CommandLink cl = null;
                    Button btn = UseCommandLinks ? (Button)(cl = new CommandLink()) : new Button();
                    if (string.IsNullOrEmpty(b.Text))
                    {
                        btn.Content = b.Result.ToLocalizedString();
                    }
                    else if (UseCommandLinks)
                    {
                        cl.Content = b.Text;
                        cl.Note = string.IsNullOrEmpty(b.Note) ? (object)b.Note : new TextBlock(new Run(b.Note)) { TextWrapping = TextWrapping.Wrap, MaxWidth = 800 };
                        cl.Icon = b.IsElevationRequired ? CommandLinkIcon.Shield : (UseCommandLinksNoIcon ? CommandLinkIcon.None : CommandLinkIcon.Arrow);
                    }
                    else
                    {
                        btn.Content = b.Text;
                    }
                    btn.Click += Button_Click;
                    btn.Tag = b;
                    if (b.Result == TaskDialogResult.Cancel)
                    {
                        btn.IsCancel = true;
                    }
                    if (b.Result == TaskDialogResult.OK && _IsOnlyOK == null)
                    {
                        _IsOnlyOK = true;
                    }
                    else
                    {
                        _IsOnlyOK = false;
                    }
                    (UseCommandLinks ? (Panel)PanelCommandLinks : (Panel)PanelRightButtons).Children.Add(btn);
                }
            }
            TaskDialogCommonButtons commonButtons = CommonButtons;
            if ((Buttons == null || Buttons.Count == 0) && commonButtons == TaskDialogCommonButtons.None)
            {
                commonButtons = TaskDialogCommonButtons.OK;
            }
            areButtons = commonButtons != TaskDialogCommonButtons.None;
            if (areButtons)
            {
                foreach (TaskDialogCommonButtons cb in TaskDialogHelpers.CommonButtonsOrder)
                {
                    if ((commonButtons & cb) != TaskDialogCommonButtons.None)
                    {
                        PanelRightButtons.Visibility = Visibility.Visible;
                        Button btn = new Button();
                        btn.Content = cb.ToLocalizedString();
                        btn.Click += Button_Click;
                        btn.Tag = cb;
                        if (cb == TaskDialogCommonButtons.Cancel)
                        {
                            btn.IsCancel = true;
                        }
                        if (cb == TaskDialogCommonButtons.OK && _IsOnlyOK == null)
                        {
                            _IsOnlyOK = true;
                        }
                        else
                        {
                            _IsOnlyOK = false;
                        }
                        PanelRightButtons.Children.Add(btn);
                    }
                }
            }
            foreach (Button b in PanelRightButtons.Children)
            {
                if (!_IsTaskDialog)
                {
                    b.MinWidth = TaskDialogHelpers.IsSegoeUIInstalled ? 88.0 : 75.0;
                }
                else
                {
                    //double d = b.ActualWidth;
                    //if (d < 68.0) d = 68.0;
                    b.MinWidth = /*Math.Ceiling((d - 68.0) / 20.0) * 20.0 +*/ 68.0;
                }
                if (!_IsTaskDialog)
                {
                    b.MinHeight = TaskDialogHelpers.IsSegoeUIInstalled ? 26.0 : 23.0;
                }
                else
                {
                    b.MinHeight = 23.0;
                }
            }
            SetDefaultButton();
            RecheckCancellation();
        }

        private void SetDefaultButton()
        {
            foreach (Button b in PanelCommandLinks.Children.OfType<CommandLink>().Cast<Button>().Concat(PanelRightButtons.Children.OfType<Button>()))
            {
                if ((b.Tag is TaskDialogButton && ((TaskDialogButton)b.Tag).Result == DefaultButton) ||
                    (b.Tag is TaskDialogCommonButtons && TaskDialogHelpers.TaskDialogCommonButtonsToTaskDialogResult((TaskDialogCommonButtons)b.Tag) == DefaultButton))
                {
                    _DefaultButtonToFocus = b;
                    b.Focus();
                    b.IsDefault = true;
                    if (_IsOnlyOK == true)
                    {
                        b.IsCancel = true;
                    }
                    return;
                }
                else if (b.IsDefault)
                {
                    b.IsDefault = false;
                }
            }
        }

        private void DeleteRadioButtons()
        {
            foreach (RadioButton rb in PanelRadioButtons.Children)
            {
                rb.Click -= RadioButton_Click;
                rb.Tag = null;
            }
            PanelRadioButtons.Children.Clear();
        }

        private void RecreateRadioButtons()
        {
            DeleteRadioButtons();
            bool areButtons = RadioButtons != null && RadioButtons.Count > 0;
            PanelRadioButtons.Visibility = Visibility.Collapsed;
            if (areButtons)
            {
                PanelRadioButtons.Visibility = Visibility.Visible;
                foreach (TaskDialogRadioButton rb in RadioButtons)
                {
                    RadioButton rbtn = new RadioButton() { Visibility = Visibility.Visible };
                    rbtn.Content = new TextBlock { Text = rb.Text ?? rb.Result.ToLocalizedString(), TextWrapping = TextWrapping.Wrap };
                    rbtn.Click += RadioButton_Click;
                    rbtn.Tag = rb;
                    PanelRadioButtons.Children.Add(rbtn);
                }
            }
            SetDefaultRadioButton();
        }

        private void SetDefaultRadioButton()
        {
            foreach (RadioButton rb in PanelRadioButtons.Children.OfType<RadioButton>())
            {
                if (((TaskDialogRadioButton)rb.Tag).Result == DefaultRadioButton)
                {
                    rb.IsChecked = true;
                    return;
                }
                else
                {
                    rb.IsChecked = false;
                }
            }
        }

        private void UpdateMainIconPosition()
        {
            if (ImageMainIcon.Visibility == Visibility.Visible)
            {
                if (LabelMainInstruction.Visibility == Visibility.Visible)
                {
                    ImageMainIcon.SetValue(Grid.RowProperty, 0);
                    if (MainIcon >= TaskDialogIcon.SecuritySuccess && MainIcon <= TaskDialogIcon.SecurityShieldGray && MainIcon != TaskDialogIcon.SecurityShield)
                    {
                        ImageMainIcon.SetValue(Grid.RowSpanProperty, 1);
                        BorderGradient.Visibility = Visibility.Visible;
                        LabelMainInstruction.Foreground = MainIcon == TaskDialogIcon.SecurityWarning ? Brushes.Black : Brushes.White;
                        switch (MainIcon)
                        {
                            case TaskDialogIcon.SecuritySuccess:
                            case TaskDialogIcon.SecurityQuestion:
                            case TaskDialogIcon.SecurityWarning:
                            case TaskDialogIcon.SecurityError:
                            case TaskDialogIcon.SecurityShieldBlue:
                            case TaskDialogIcon.SecurityShieldGray:
                                BorderGradient.Background = Resources["Gradient" + MainIcon.ToString()] as Brush;
                                break;

                            default:
                                BorderGradient.Background = Resources["GradientNormal"] as Brush;
                                break;
                        }
                    }
                    else
                    {
                        ImageMainIcon.SetValue(Grid.RowSpanProperty, 2);
                        BorderGradient.Visibility = Visibility.Collapsed;
                        //LabelMainInstruction.Background = Brushes.Transparent;
                        LabelMainInstruction.Foreground = new SolidColorBrush(Color.FromRgb(0, 0x33, 0x99));
                    }
                }
                else if (LabelExpandedInfo.Visibility == Visibility.Visible)
                {
                    ImageMainIcon.SetValue(Grid.RowProperty, 1);
                    ImageMainIcon.SetValue(Grid.RowSpanProperty, 2);
                }
                else
                {
                    ImageMainIcon.SetValue(Grid.RowProperty, 1);
                    ImageMainIcon.SetValue(Grid.RowSpanProperty, 1);
                }
            }
        }

        #endregion
    }
}
