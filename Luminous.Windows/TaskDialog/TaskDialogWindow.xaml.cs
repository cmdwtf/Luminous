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

namespace Luminous.Windows
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Timers;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

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
			_taskDialog = taskDialog;
			CustomScale.ScaleX = CustomScale.ScaleY = TaskDialog.CustomScale;
			if (TaskDialog.CustomScale != 1f)
			{
				TextOptions.SetTextFormattingMode(this, TextFormattingMode.Ideal);
			}

			if (TaskDialogHelpers.IsSegoeUIInstalled)
			{
				FontFamily = new FontFamily(TaskDialogHelpers.SegoeUIFontFamily);
				FontSize = 12;
			}

			Sound = SoundScheme.Default;
			ImageMainIcon.Visibility = Visibility.Collapsed;
			if (!(IsTaskDialog = isTaskDialog))
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
			get => base.Owner;
			set
			{
				if (Owner != value)
				{
					if (value == null)
					{
						value = TaskDialog.DefaultOwnerWindow;
					}

					base.Owner = value;
					UpdateStartupLocation();
					ShowInTaskbar = value == null;
				}
			}
		}

		public string WindowTitle
		{
			get => Title;
			set
			{
				if (WindowTitle != value)
				{
					if (string.IsNullOrEmpty(value))
					{
						value = TaskDialogHelpers.ProductName;
					}
					Title = value;
				}
			}
		}

		private TaskDialogIcon _mainIcon;
		public TaskDialogIcon MainIcon
		{
			get => _mainIcon;
			set
			{
				if (MainIcon != value)
				{
					_mainIcon = value;
					ImageMainIcon.BeginInit();
					ImageMainIcon.Visibility = Visibility.Visible;
					switch (value)
					{
						case TaskDialogIcon.Information:
							//ImageMainIcon.Source = new BitmapImage(new Uri("pack://application:,,,/TaskDialog/Resources/Information.png"));
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Information.png", UriKind.Relative));
							Sound = SoundScheme.Information;
							break;
						case TaskDialogIcon.Question:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Question.png", UriKind.Relative));
							Sound = SoundScheme.Question;
							break;
						case TaskDialogIcon.Warning:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Warning.png", UriKind.Relative));
							Sound = SoundScheme.Warning;
							break;
						case TaskDialogIcon.Error:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Error.png", UriKind.Relative));
							Sound = SoundScheme.Error;
							break;
						case TaskDialogIcon.SecuritySuccess:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecuritySuccess.png", UriKind.Relative));
							Sound = SoundScheme.Information;
							break;
						case TaskDialogIcon.SecurityQuestion:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecurityQuestion.png", UriKind.Relative));
							Sound = SoundScheme.Question;
							break;
						case TaskDialogIcon.SecurityWarning:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecurityWarning.png", UriKind.Relative));
							Sound = SoundScheme.Warning;
							break;
						case TaskDialogIcon.SecurityError:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/SecurityError.png", UriKind.Relative));
							Sound = SoundScheme.Error;
							break;
						case TaskDialogIcon.SecurityShield:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Security.png", UriKind.Relative));
							Sound = SoundScheme.Security;
							break;
						case TaskDialogIcon.SecurityShieldBlue:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Security.png", UriKind.Relative));
							Sound = SoundScheme.Security;
							break;
						case TaskDialogIcon.SecurityShieldGray:
							ImageMainIcon.Source = new BitmapImage(new Uri("Resources/Security.png", UriKind.Relative));
							Sound = SoundScheme.Security;
							break;
						default:
							ImageMainIcon.Source = null;
							ImageMainIcon.Visibility = Visibility.Collapsed;
							Sound = SoundScheme.Default;
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
			get => LabelMainInstruction.Text;
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

		private string ContetTextWithoutHyperlinks { get; set; }
		public string ContentText
		{
			get => LabelContent.Text;
			set
			{
				if (ContentText != value)
				{
					ContetTextWithoutHyperlinks = SetText(LabelContent, value);
					LabelContent.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
					RecalculateWidth();
				}
			}
		}

		private TaskDialogCommonButtons _commonButtons;
		public TaskDialogCommonButtons CommonButtons
		{
			get => _commonButtons;
			set
			{
				if (CommonButtons != value)
				{
					_commonButtons = value;
					RecreateButtons();
					RecalculateWidth();
				}
			}
		}

		private IList<TaskDialogButton> _buttons;
		public IList<TaskDialogButton> Buttons
		{
			get => _buttons;
			set
			{
				if (Buttons != value)
				{
					_buttons = value;
					RecreateButtons();
					RecalculateWidth();
				}
			}
		}

		private TaskDialogResult _defaultButton;
		public TaskDialogResult DefaultButton
		{
			get => _defaultButton;
			set
			{
				if (DefaultButton != value)
				{
					_defaultButton = value;
					SetDefaultButton();
				}
			}
		}

		private bool _enableHyperlinks;
		public bool EnableHyperlinks
		{
			get => _enableHyperlinks;
			set
			{
				if (EnableHyperlinks != value)
				{
					_enableHyperlinks = value;
					RefreshTexts();
					RecalculateWidth();
				}
			}
		}

		private bool _allowDialogCancellation;
		public bool AllowDialogCancellation
		{
			get => _allowDialogCancellation;
			set
			{
				if (AllowDialogCancellation != value)
				{
					_allowDialogCancellation = value;
					RecheckCancellation();
				}
			}
		}

		private bool _useCommandLinks;
		public bool UseCommandLinks
		{
			get => _useCommandLinks;
			set
			{
				if (UseCommandLinks != value)
				{
					_useCommandLinks = value;
					RecreateButtons();
					RecalculateWidth();
				}
			}
		}

		private bool _useCommandLinksNoIcon;
		public bool UseCommandLinksNoIcon
		{
			get => _useCommandLinksNoIcon;
			set
			{
				if (UseCommandLinksNoIcon != value)
				{
					_useCommandLinksNoIcon = value;
					RecreateButtons();
					RecalculateWidth();
				}
			}
		}

		private bool _isPositionRelativeToWindow;
		public bool IsPositionRelativeToWindow
		{
			get => _isPositionRelativeToWindow;
			set
			{
				if (IsPositionRelativeToWindow != value)
				{
					_isPositionRelativeToWindow = value;
					UpdateStartupLocation();
				}
			}
		}

		private TaskDialogIcon _footerIcon;
		public TaskDialogIcon FooterIcon
		{
			get => _footerIcon;
			set
			{
				if (FooterIcon != value)
				{
					_footerIcon = value;
					ImageFooterIcon.BeginInit();
					ImageFooterIcon.Visibility = Visibility.Visible;
					switch (value)
					{
						case TaskDialogIcon.Information:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallInformation.png", UriKind.Relative));
							Sound = SoundScheme.Information;
							break;
						case TaskDialogIcon.Question:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallQuestion.png", UriKind.Relative));
							Sound = SoundScheme.Question;
							break;
						case TaskDialogIcon.Warning:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallWarning.png", UriKind.Relative));
							Sound = SoundScheme.Warning;
							break;
						case TaskDialogIcon.Error:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallError.png", UriKind.Relative));
							Sound = SoundScheme.Error;
							break;
						case TaskDialogIcon.SecuritySuccess:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecuritySuccess.png", UriKind.Relative));
							Sound = SoundScheme.Information;
							break;
						case TaskDialogIcon.SecurityQuestion:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurityQuestion.png", UriKind.Relative));
							Sound = SoundScheme.Question;
							break;
						case TaskDialogIcon.SecurityWarning:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurityWarning.png", UriKind.Relative));
							Sound = SoundScheme.Warning;
							break;
						case TaskDialogIcon.SecurityError:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurityError.png", UriKind.Relative));
							Sound = SoundScheme.Error;
							break;
						case TaskDialogIcon.SecurityShield:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurity.png", UriKind.Relative));
							Sound = SoundScheme.Security;
							break;
						case TaskDialogIcon.SecurityShieldBlue:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurity.png", UriKind.Relative));
							Sound = SoundScheme.Security;
							break;
						case TaskDialogIcon.SecurityShieldGray:
							ImageFooterIcon.Source = new BitmapImage(new Uri("Resources/SmallSecurity.png", UriKind.Relative));
							Sound = SoundScheme.Security;
							break;
						default:
							ImageFooterIcon.Source = null;
							ImageFooterIcon.Visibility = Visibility.Collapsed;
							Sound = SoundScheme.Default;
							break;
					}
					ImageFooterIcon.EndInit();
					RecalculateWidth();
				}
			}
		}

		private string FooterTextWithoutHyperlinks { get; set; }
		public string FooterText
		{
			get => LabelFooter.Text;
			set
			{
				if (FooterText != value)
				{
					FooterTextWithoutHyperlinks = SetText(LabelFooter, value);
					PanelFooter.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
					RecalculateWidth();
				}
			}
		}

		public bool IsExpanded
		{
			get => Expander.IsExpanded;
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

		private bool _expandFooterArea;
		public bool ExpandFooterArea
		{
			get => _expandFooterArea;
			set
			{
				if (ExpandFooterArea != value)
				{
					_expandFooterArea = value;
					UpdateExpander();
				}
			}
		}

		private string ExpandedInformationWithoutHyperlinks { get; set; }
		public string ExpandedInformation
		{
			get => LabelExpandedInfo.Text;
			set
			{
				if (ExpandedInformation != value)
				{
					ExpandedInformationWithoutHyperlinks = SetText(LabelExpandedInfo, value);
					SetText(LabelExpandedFooterInfo, value);
					UpdateExpander();
					RecalculateWidth();
				}
			}
		}

		public bool? IsVerificationFlagChecked
		{
			get => CheckBoxVerification.IsChecked;
			set
			{
				if (IsVerificationFlagChecked != value)
				{
					CheckBoxVerification.IsChecked = value;
				}
			}
		}

		private IList<TaskDialogRadioButton> _radioButtons;
		public IList<TaskDialogRadioButton> RadioButtons
		{
			get => _radioButtons;
			set
			{
				if (RadioButtons != value)
				{
					_radioButtons = value;
					RecreateRadioButtons();
					RecalculateWidth();
				}
			}
		}

		private TaskDialogResult _defaultRadioButton;
		public TaskDialogResult DefaultRadioButton
		{
			get => _defaultRadioButton;
			set
			{
				if (DefaultRadioButton != value)
				{
					_defaultRadioButton = value;
					SetDefaultRadioButton();
				}
			}
		}

		private string _verificationText;
		public string VerificationText
		{
			get => _verificationText;
			set
			{
				if (VerificationText != value)
				{
					CheckBoxVerification.Content = _verificationText = value;
					CheckBoxVerification.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
					RecalculateWidth();
				}
			}
		}

		private int _clientWidth;
		public int ClientWidth
		{
			get => _clientWidth;
			set
			{
				if (_clientWidth != value)
				{
					_clientWidth = value;
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

		private string _expandedControlText;
		public string ExpandedControlText
		{
			get => string.IsNullOrEmpty(_expandedControlText) ? Properties.Resources.ExpandedText : _expandedControlText;
			set
			{
				if (_expandedControlText != value)
				{
					_expandedControlText = value;
					UpdateExpander();
					RecalculateWidth();
				}
			}
		}

		private string _collapsedControlText;
		public string CollapsedControlText
		{
			get => string.IsNullOrEmpty(_collapsedControlText) ? Properties.Resources.CollapsedText : _collapsedControlText;
			set
			{
				if (_collapsedControlText != value)
				{
					_collapsedControlText = value;
					UpdateExpander();
					RecalculateWidth();
				}
			}
		}

		public bool ShowProgressBar
		{
			get => ProgressBar.Visibility == Visibility.Visible;
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
			get => ProgressBar.IsIndeterminate;
			set
			{
				if (ShowMarqueeProgressBar != value)
				{
					ProgressBar.IsIndeterminate = value;
				}
			}
		}

		private TaskDialogProgressBarState _progressBarState;
		public TaskDialogProgressBarState ProgressBarState
		{
			get => _progressBarState;
			set
			{
				if (ProgressBarState != value)
				{
					_progressBarState = value;
					switch (value)
					{
						case TaskDialogProgressBarState.Pause:
							ProgressBar.Foreground = Resources["ProgressBarPause"] as Brush;
							break;
						case TaskDialogProgressBarState.Error:
							ProgressBar.Foreground = Resources["ProgressBarError"] as Brush;
							break;
						default:
							ProgressBar.SetValue(ForegroundProperty, DependencyProperty.UnsetValue);
							break;
					}
				}
			}
		}

		private bool _useCallBackTimer;
		public bool UseCallBackTimer
		{
			get => _useCallBackTimer;
			set
			{
				if (UseCallBackTimer != value)
				{
					_useCallBackTimer = value;
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
			get => FlowDirection == FlowDirection.RightToLeft;
			set
			{
				if (RtlLayout != value)
				{
					FlowDirection = value ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
				}
			}
		}

		private bool _canBeMinimized;
		public bool CanBeMinimized
		{
			get => _canBeMinimized;
			set
			{
				if (CanBeMinimized != value)
				{
					_canBeMinimized = true;
					ResizeMode = value ? ResizeMode.CanMinimize : ResizeMode.NoResize;
				}
			}
		}

		public double ProgressBarMinimum
		{
			get => ProgressBar.Minimum;
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
			get => ProgressBar.Maximum;
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
			get => ProgressBar.Value;
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

		internal readonly bool IsTaskDialog;
		private readonly List<Hyperlink> _hyperlinks = new();
		private SystemSound Sound { get; set; }
		private bool IsCancellable { get; set; }
		private System.Windows.Interop.HwndSource _hwndSource;
		private bool? IsOnlyOK { get; set; }
		private Timer _timer;
		private DateTime? _lastTick;
		private readonly TaskDialog _taskDialog;
		private Button _defaultButtonToFocus;

		#endregion

		#region " Overrided Methods "

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
			{
				try
				{
					SoundScheme.Default.Play();
					Clipboard.SetText(IsTaskDialog ? TaskDialogHelpers.TaskDialogToString(this) : TaskDialogHelpers.MessageBoxToString(this));
				}
				catch { }
			}
			base.OnPreviewKeyDown(e);
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			if (!IsCancellable)
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
				if (IsOnlyOK == true)
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
			foreach (Hyperlink h in _hyperlinks)
			{
				h.Click -= Hyperlink_Click;
			}
			_hyperlinks.Clear();
			DeleteButtons();
			DeleteRadioButtons();
		}

		#endregion

		#region " Event Handlers "

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (_defaultButtonToFocus != null)
			{
				_defaultButtonToFocus.Focus();
			}
			if (Sound != null)
			{
				Sound.Play();
			}
			StartTimer();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			EnableCloseButton();
			object tag = (sender as Button).Tag;
			Tag = tag is TaskDialogButton button
				? button.Result
				: (object)TaskDialogHelpers.TaskDialogCommonButtonsToTaskDialogResult((TaskDialogCommonButtons)tag);
			Close();
		}

		private void RadioButton_Click(object sender, RoutedEventArgs e)
		{
		}

		private void Expander_Expanded(object sender, RoutedEventArgs e) => UpdateExpander();

		private void Expander_Collapsed(object sender, RoutedEventArgs e) => UpdateExpander();

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (_lastTick == null)
			{
				_lastTick = DateTime.Now;
			}
			if (Tick != null)
			{
				TaskDialogTimerEventArgs ea;
				Tick(_taskDialog, ea = new TaskDialogTimerEventArgs { Interval = DateTime.Now - _lastTick.Value });
				if (ea.Reset)
				{
					_lastTick = DateTime.Now;
				}
			}
		}

		#endregion

		#region " Private Methods "

		private void DeleteTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			}
		}

		private void StartTimer()
		{
			if (_timer != null)
			{
				_timer.Start();
				_lastTick = DateTime.Now;
			}
		}

		private void StopTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
			}
		}

		private void CreateTimer()
		{
			DeleteTimer();
			_timer = new Timer(200);
			_timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
		}

		private void RecalculateWidth()
		{
			double w = SystemParameters.PrimaryScreenWidth * .75;
			if (w < 640)
			{
				w = 640;
			}

			if (IsTaskDialog && w > 800)
			{
				w = 800;
			}

			if (MaxWidth != w)
			{
				MaxWidth = w;
			}
		}

		private void UpdateExpander()
		{
			TextBlockExpanderCollapsed.Visibility = IsExpanded ? Visibility.Hidden : Visibility.Visible;
			TextBlockExpanderExpanded.Visibility = IsExpanded ? Visibility.Visible : Visibility.Hidden;
			if (string.IsNullOrEmpty(ExpandedInformationWithoutHyperlinks))
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

		private void UpdateStartupLocation() => WindowStartupLocation = (Owner == null || (IsTaskDialog && !IsPositionRelativeToWindow)) ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

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
			_hwndSource = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
			if (_hwndSource != null)
			{
				_hwndSource.AddHook(TaskDialogHelpers.CloseButtonHook);
			}
		}

		private void RemoveHook()
		{
			if (_hwndSource != null)
			{
				_hwndSource.RemoveHook(TaskDialogHelpers.CloseButtonHook);
				_hwndSource = null;
			}
		}

		private string SetText(TextBlock label, string text)
		{
			foreach (Hyperlink h in _hyperlinks)
			{
				if (label.Inlines.Contains(h))
				{
					h.Click -= Hyperlink_Click;
				}
			}
			for (int i = _hyperlinks.Count - 1; i >= 0; i--)
			{
				if (label.Inlines.Contains(_hyperlinks[i]))
				{
					_hyperlinks.RemoveAt(i);
				}
			}
			string ret = label.Text = text;
			if (EnableHyperlinks)
			{
				var parts = text.Split('<').ToList();
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
											var h = new Hyperlink(new Run(htext)) { Tag = href };
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

		private void Hyperlink_Click(object sender, RoutedEventArgs e) => HyperlinkClick?.Invoke(_taskDialog, new TaskDialogHyperlinkClickEventArgs { Href = (sender as Hyperlink).Tag as string });

		private void RecheckCancellation()
		{
			bool cancel = ((CommonButtons & TaskDialogCommonButtons.Cancel) != TaskDialogCommonButtons.None) ||
				(Buttons != null && Buttons.Any(b => b.Result == TaskDialogResult.Cancel));
			IsCancellable = cancel || (IsOnlyOK == true) || AllowDialogCancellation;
			if (!IsCancellable)
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
			IsOnlyOK = null;
			bool areButtons = Buttons != null && Buttons.Count > 0;
			PanelCommandLinks.Visibility = PanelRightButtons.Visibility = Visibility.Collapsed;
			if (areButtons)
			{
				(UseCommandLinks ? PanelCommandLinks : (Panel)PanelRightButtons).Visibility = Visibility.Visible;
				foreach (TaskDialogButton b in Buttons)
				{
					CommandLink cl = null;
					Button btn = UseCommandLinks ? (cl = new CommandLink()) : new Button();
					if (string.IsNullOrEmpty(b.Text))
					{
						btn.Content = b.Result.ToLocalizedString();
					}
					else if (UseCommandLinks)
					{
						cl.Content = b.Text;
						cl.Note = string.IsNullOrEmpty(b.Note) ? b.Note : new TextBlock(new Run(b.Note)) { TextWrapping = TextWrapping.Wrap, MaxWidth = 800 };
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
					if (b.Result == TaskDialogResult.OK && IsOnlyOK == null)
					{
						IsOnlyOK = true;
					}
					else
					{
						IsOnlyOK = false;
					}
					(UseCommandLinks ? PanelCommandLinks : (Panel)PanelRightButtons).Children.Add(btn);
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
						var btn = new Button
						{
							Content = cb.ToLocalizedString()
						};
						btn.Click += Button_Click;
						btn.Tag = cb;
						if (cb == TaskDialogCommonButtons.Cancel)
						{
							btn.IsCancel = true;
						}
						if (cb == TaskDialogCommonButtons.OK && IsOnlyOK == null)
						{
							IsOnlyOK = true;
						}
						else
						{
							IsOnlyOK = false;
						}
						PanelRightButtons.Children.Add(btn);
					}
				}
			}
			foreach (Button b in PanelRightButtons.Children)
			{
				if (!IsTaskDialog)
				{
					b.MinWidth = TaskDialogHelpers.IsSegoeUIInstalled ? 88.0 : 75.0;
				}
				else
				{
					//double d = b.ActualWidth;
					//if (d < 68.0) d = 68.0;
					b.MinWidth = /*Math.Ceiling((d - 68.0) / 20.0) * 20.0 +*/ 68.0;
				}
				if (!IsTaskDialog)
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
				if ((b.Tag is TaskDialogButton button && button.Result == DefaultButton) ||
					(b.Tag is TaskDialogCommonButtons buttons && TaskDialogHelpers.TaskDialogCommonButtonsToTaskDialogResult(buttons) == DefaultButton))
				{
					_defaultButtonToFocus = b;
					b.Focus();
					b.IsDefault = true;
					if (IsOnlyOK == true)
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
					var rbtn = new RadioButton() { Visibility = Visibility.Visible };
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
