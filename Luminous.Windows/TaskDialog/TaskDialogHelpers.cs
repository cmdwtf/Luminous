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
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Media;

	internal static class TaskDialogHelpers
	{
		#region " Close Button "

		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, [param: MarshalAs(UnmanagedType.Bool)] bool bRevert);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

		public static class MF
		{
			public const uint BYCOMMAND = 0x00000000;
			public const uint GRAYED = 0x00000001;
			public const uint ENABLED = 0x00000000;
		}

		public static class SC
		{

			public const uint SIZE = 0xF000;
			public const uint MINIMIZE = 0xF020;
			public const uint MAXIMIZE = 0xF030;
			public const uint CLOSE = 0xF060;
			public const uint RESTORE = 0xF120;
		}

		public static class WM
		{
			public const int SHOWWINDOW = 0x18;
			public const int CLOSE = 0x10;
			public const uint SETICON = 0x0080;
		}

		public static class WS
		{
			public static class EX
			{
				public const int DLGMODALFRAME = 0x0001;
			}
		}
		public static class GWL
		{
			public const int EXSTYLE = -20;
		}

		public static class SWP
		{
			public const int NOSIZE = 0x0001;
			public const int NOMOVE = 0x0002;
			public const int NOZORDER = 0x0004;
			public const int FRAMECHANGED = 0x0020;
		}

		public static IntPtr CloseButtonHook(IntPtr hwnd, int msg, IntPtr _, IntPtr _1, ref bool handled)
		{
			if (msg == WM.SHOWWINDOW)
			{
				DisableCloseButton(hwnd);
			}
			else if (msg == WM.CLOSE)
			{
				handled = true;
			}
			return IntPtr.Zero;
		}

		private static void DisableCloseButton(IntPtr hwnd)
		{
			IntPtr hMenu = GetSystemMenu(hwnd, false);
			if (hMenu != IntPtr.Zero)
			{
				EnableMenuItem(hMenu, SC.CLOSE, MF.BYCOMMAND | MF.GRAYED);
				DisableOtherButtons(hMenu);
			}
		}

		private static void DisableOtherButtons(IntPtr hMenu)
		{
			EnableMenuItem(hMenu, SC.SIZE, MF.BYCOMMAND | MF.GRAYED);
			EnableMenuItem(hMenu, SC.MINIMIZE, MF.BYCOMMAND | MF.GRAYED);
			EnableMenuItem(hMenu, SC.MAXIMIZE, MF.BYCOMMAND | MF.GRAYED);
			EnableMenuItem(hMenu, SC.RESTORE, MF.BYCOMMAND | MF.GRAYED);
		}

		private static void EnableCloseButton(IntPtr hwnd)
		{
			IntPtr hMenu = GetSystemMenu(hwnd, false);
			if (hMenu != IntPtr.Zero)
			{
				EnableMenuItem(hMenu, SC.CLOSE, MF.BYCOMMAND);
				DisableOtherButtons(hMenu);
			}
		}

		public static void EnableCloseButton(Window window, bool enable)
		{
			if (PresentationSource.FromVisual(window) is HwndSource source)
			{
				if (enable)
				{
					EnableCloseButton(source.Handle);
				}
				else
				{
					DisableCloseButton(source.Handle);
				}
			}
		}

		#endregion

		#region " Window Icon "

		[DllImport("user32.dll")]
		private static extern IntPtr GetWindowLong(IntPtr hwnd, int index);

		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowLong(IntPtr hwnd, int index, IntPtr newStyle);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

		public static void RemoveWindowIcon(Window window)
		{
			if (PresentationSource.FromVisual(window) is not HwndSource hwndSource)
			{
				return;
			}

			IntPtr hwnd = hwndSource.Handle;
			int extendedStyle = (int)GetWindowLong(hwnd, GWL.EXSTYLE);
			SetWindowLong(hwnd, GWL.EXSTYLE, new IntPtr(extendedStyle | WS.EX.DLGMODALFRAME));
			SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER | SWP.FRAMECHANGED);
			SendMessage(hwnd, WM.SETICON, IntPtr.Zero, IntPtr.Zero); // remove small icon
																	 // SendMessage(hwnd, WM.SETICON, new IntPtr(1), IntPtr.Zero); // remove big icon
		}

		#endregion

		#region " Localization "

		public static string ToLocalizedString(this TaskDialogResult @this)
		{
			return @this switch
			{
				TaskDialogResult.OK => Properties.Resources.OKText,
				TaskDialogResult.Yes => Properties.Resources.YesText,
				TaskDialogResult.No => Properties.Resources.NoText,
				TaskDialogResult.Abort => Properties.Resources.AbortText,
				TaskDialogResult.Retry => Properties.Resources.RetryText,
				TaskDialogResult.Ignore => Properties.Resources.IgnoreText,
				TaskDialogResult.Cancel => Properties.Resources.CancelText,
				TaskDialogResult.Close => Properties.Resources.CloseText,
				_ => Properties.Resources.NoneText,
			};
		}

		public static string ToLocalizedString(this TaskDialogCommonButtons @this)
		{
			return @this switch
			{
				TaskDialogCommonButtons.OK => Properties.Resources.OKText,
				TaskDialogCommonButtons.Yes => Properties.Resources.YesText,
				TaskDialogCommonButtons.No => Properties.Resources.NoText,
				TaskDialogCommonButtons.Abort => Properties.Resources.AbortText,
				TaskDialogCommonButtons.Retry => Properties.Resources.RetryText,
				TaskDialogCommonButtons.Ignore => Properties.Resources.IgnoreText,
				TaskDialogCommonButtons.Cancel => Properties.Resources.CancelText,
				TaskDialogCommonButtons.Close => Properties.Resources.CloseText,
				_ => Properties.Resources.NoneText,
			};
		}

		#endregion

		public static string ProductName => Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault() is not AssemblyProductAttribute p
					? "Application"
					: p.Product;

		/*public static ImageSource CreateIconImageSource(byte[] iconData)
		{
			return BitmapFrame.Create(new MemoryStream(iconData));
		}*/

		public static readonly string SegoeUIFontFamily = "Segoe UI";
		public static readonly bool IsSegoeUIInstalled = CheckSegoeUI();
		private static bool CheckSegoeUI()
		{
			foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
			{
				if (fontFamily.Source == SegoeUIFontFamily)
				{
					return true;
				}
			}
			return false;
		}


		public static TaskDialogResult TaskDialogCommonButtonsToTaskDialogResult(TaskDialogCommonButtons taskDialogCommonButton)
		{
			int i = 1;
			int btn = (int)taskDialogCommonButton;
			if (btn == 0)
			{
				return TaskDialogResult.None;
			}
			while ((btn >>= 1) > 0)
			{
				i++;
			}
			return (TaskDialogResult)i;
		}

		public static TaskDialogResult GetFirstButton(TaskDialogCommonButtons buttons)
		{
			int i = 1;
			int btn = (int)buttons;
			if (btn == 0)
			{
				return TaskDialogResult.None;
			}
			do
			{
				if ((btn & 1) != 0)
				{
					break;
				}
				i++;
			}
			while ((btn >>= 1) > 0);
			return (TaskDialogResult)i;
		}

		public static readonly TaskDialogCommonButtons[] CommonButtonsOrder = new TaskDialogCommonButtons[] { TaskDialogCommonButtons.OK, TaskDialogCommonButtons.Yes, TaskDialogCommonButtons.No, TaskDialogCommonButtons.Abort, TaskDialogCommonButtons.Retry, TaskDialogCommonButtons.Ignore, TaskDialogCommonButtons.Cancel, TaskDialogCommonButtons.Close };

		public static string ButtonsToText(IList<TaskDialogButton> buttons, TaskDialogCommonButtons commonButtons)
		{
			string s = string.Empty;
			if (buttons != null)
			{
				foreach (TaskDialogButton b in buttons)
				{
					if (!string.IsNullOrEmpty(s))
					{
						s += "   ";
					}
					s += b.Result.ToLocalizedString();
				}
			}
			foreach (TaskDialogCommonButtons b in CommonButtonsOrder)
			{
				if ((b & commonButtons) != TaskDialogCommonButtons.None)
				{
					if (!string.IsNullOrEmpty(s))
					{
						s += "   ";
					}
					s += b.ToLocalizedString();
				}
			}
			return s;
		}

		private const string Sep = "---------------------------";

		public static string MessageBoxToString(TaskDialogWindow w)
		{
			return Sep + Environment.NewLine + w.WindowTitle + Environment.NewLine +
				   Sep + Environment.NewLine + w.ContentText + Environment.NewLine +
				   Sep + Environment.NewLine + ButtonsToText(w.Buttons, w.CommonButtons) + Environment.NewLine + Sep;
		}

		public static string TaskDialogToString(TaskDialogWindow w)
		{
			return Sep + Environment.NewLine + w.WindowTitle + Environment.NewLine +
				  (!string.IsNullOrEmpty(w.MainInstruction) ? Sep + Environment.NewLine + w.MainInstruction + Environment.NewLine : string.Empty) +
				  (!string.IsNullOrEmpty(w.ContentText) ? Sep + Environment.NewLine + w.ContentText + Environment.NewLine : string.Empty) +
				  (!string.IsNullOrEmpty(w.ExpandedInformation) && !w.ExpandFooterArea && w.IsExpanded ? Sep + Environment.NewLine + w.ExpandedInformation + Environment.NewLine : string.Empty) +
				  (w.RadioButtons != null && w.RadioButtons.Count > 0 ? Sep + Environment.NewLine + RadioButtonsToText(w.RadioButtons) + Environment.NewLine : string.Empty) +
				  (w.Buttons != null && w.Buttons.Count > 0 ? Sep + Environment.NewLine + ButtonsToText(w.Buttons) + Environment.NewLine : string.Empty) +
				  (w.CommonButtons != TaskDialogCommonButtons.None ? Sep + Environment.NewLine + ButtonsToText(w.CommonButtons) + Environment.NewLine : string.Empty) +
				  (!string.IsNullOrEmpty(w.FooterText) ? Sep + Environment.NewLine + w.FooterText + Environment.NewLine : string.Empty) +
				  (!string.IsNullOrEmpty(w.ExpandedInformation) && w.ExpandFooterArea && w.IsExpanded ? Sep + Environment.NewLine + w.ExpandedInformation + Environment.NewLine : string.Empty);
		}

		private static string ButtonsToText(TaskDialogCommonButtons commonButtons) => ButtonsToText(null, commonButtons);

		private static string ButtonsToText(IList<TaskDialogButton> buttons)
		{
			string s = string.Empty;
			if (buttons != null)
			{
				foreach (TaskDialogButton b in buttons)
				{
					if (!string.IsNullOrEmpty(s))
					{
						s += Environment.NewLine + Environment.NewLine;
					}
					s += b.Result.ToLocalizedString();
				}
			}
			return s;
		}

		private static string RadioButtonsToText(IList<TaskDialogRadioButton> radioButtons)
		{
			string s = string.Empty;
			if (radioButtons != null)
			{
				foreach (TaskDialogRadioButton b in radioButtons)
				{
					if (!string.IsNullOrEmpty(s))
					{
						s += Environment.NewLine;
					}
					s += " * " + b.Result.ToLocalizedString();
				}
			}
			return s;
		}

		/*public static string ContentToCommandLinkText(string text)
		{
			if (text.Contains(Environment.NewLine))
			{
				return text.Substring(0, text.IndexOf(Environment.NewLine));
			}
			return text;
		}

		public static string ContentToCommandLinkNote(string text)
		{
			if (text.Contains(Environment.NewLine))
			{
				return text.Substring(text.IndexOf(Environment.NewLine) + Environment.NewLine.Length);
			}
			return null;
		}*/
	}
}
