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
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.Windows.Media;
    using System.Runtime.InteropServices;
    using System.Windows.Interop;
    using System.Windows;

    internal static class TaskDialogHelpers
    {
        #region " Close Button "

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, [param: MarshalAs(UnmanagedType.Bool)] bool bRevert);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_ENABLED = 0x00000000;
        private const uint SC_SIZE = 0xF000;
        private const uint SC_MINIMIZE = 0xF020;
        private const uint SC_MAXIMIZE = 0xF030;
        private const uint SC_CLOSE = 0xF060;
        private const uint SC_RESTORE = 0xF120;
        private const int WM_SHOWWINDOW = 0x18;
        private const int WM_CLOSE = 0x10;

        public static IntPtr CloseButtonHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SHOWWINDOW)
            {
                DisableCloseButton(hwnd);
            }
            else if (msg == WM_CLOSE)
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
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                DisableOtherButtons(hMenu);
            }
        }

        private static void DisableOtherButtons(IntPtr hMenu)
        {
            EnableMenuItem(hMenu, SC_SIZE, MF_BYCOMMAND | MF_GRAYED);
            EnableMenuItem(hMenu, SC_MINIMIZE, MF_BYCOMMAND | MF_GRAYED);
            EnableMenuItem(hMenu, SC_MAXIMIZE, MF_BYCOMMAND | MF_GRAYED);
            EnableMenuItem(hMenu, SC_RESTORE, MF_BYCOMMAND | MF_GRAYED);
        }

        private static void EnableCloseButton(IntPtr hwnd)
        {
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero)
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND);
                DisableOtherButtons(hMenu);
            }
        }

        public static void EnableCloseButton(Window window, bool enable)
        {
            HwndSource source = PresentationSource.FromVisual(window) as HwndSource;
            if (source != null)
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

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const uint WM_SETICON = 0x0080;

        public static void RemoveWindowIcon(Window window)
        {
            var hwndSource = PresentationSource.FromVisual(window) as HwndSource;
            if (hwndSource == null) return;
            IntPtr hwnd = hwndSource.Handle;
            int extendedStyle = (int)GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, new IntPtr(extendedStyle | WS_EX_DLGMODALFRAME));
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            SendMessage(hwnd, WM_SETICON, IntPtr.Zero, IntPtr.Zero); // remove small icon
            // SendMessage(hwnd, WM_SETICON, new IntPtr(1), IntPtr.Zero); // remove big icon
        }

        #endregion

        #region " Localization "

        public static string ToLocalizedString(this TaskDialogResult @this)
        {
            switch (@this)
            {
                case TaskDialogResult.OK:
                    return Properties.Resources.OKText;

                case TaskDialogResult.Yes:
                    return Properties.Resources.YesText;

                case TaskDialogResult.No:
                    return Properties.Resources.NoText;

                case TaskDialogResult.Abort:
                    return Properties.Resources.AbortText;

                case TaskDialogResult.Retry:
                    return Properties.Resources.RetryText;

                case TaskDialogResult.Ignore:
                    return Properties.Resources.IgnoreText;

                case TaskDialogResult.Cancel:
                    return Properties.Resources.CancelText;

                case TaskDialogResult.Close:
                    return Properties.Resources.CloseText;

                default:
                    return Properties.Resources.NoneText;
            }
        }

        public static string ToLocalizedString(this TaskDialogCommonButtons @this)
        {
            switch (@this)
            {
                case TaskDialogCommonButtons.OK:
                    return Properties.Resources.OKText;

                case TaskDialogCommonButtons.Yes:
                    return Properties.Resources.YesText;

                case TaskDialogCommonButtons.No:
                    return Properties.Resources.NoText;

                case TaskDialogCommonButtons.Abort:
                    return Properties.Resources.AbortText;

                case TaskDialogCommonButtons.Retry:
                    return Properties.Resources.RetryText;

                case TaskDialogCommonButtons.Ignore:
                    return Properties.Resources.IgnoreText;

                case TaskDialogCommonButtons.Cancel:
                    return Properties.Resources.CancelText;

                case TaskDialogCommonButtons.Close:
                    return Properties.Resources.CloseText;

                default:
                    return Properties.Resources.NoneText;
            }
        }

        #endregion

        public static string ProductName
        {
            get
            {
                AssemblyProductAttribute p = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault() as AssemblyProductAttribute;
                if (p == null)
                {
                    return "Application";
                }
                return p.Product;
            }
        }

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

        public static string ButtonsToText(IList<TaskDialogButton> Buttons, TaskDialogCommonButtons CommonButtons)
        {
            string s = string.Empty;
            if (Buttons != null)
            {
                foreach (TaskDialogButton b in Buttons)
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
                if ((b & CommonButtons) != TaskDialogCommonButtons.None)
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

        private const string _Sep = "---------------------------";

        public static string MessageBoxToString(TaskDialogWindow w)
        {
            return _Sep + Environment.NewLine + w.WindowTitle + Environment.NewLine +
                   _Sep + Environment.NewLine + w.ContentText + Environment.NewLine +
                   _Sep + Environment.NewLine + ButtonsToText(w.Buttons, w.CommonButtons) + Environment.NewLine + _Sep;
        }

        public static string TaskDialogToString(TaskDialogWindow w)
        {
            return _Sep + Environment.NewLine + w.WindowTitle + Environment.NewLine +
                  (!string.IsNullOrEmpty(w.MainInstruction) ? _Sep + Environment.NewLine + w.MainInstruction + Environment.NewLine : string.Empty) +
                  (!string.IsNullOrEmpty(w.ContentText) ? _Sep + Environment.NewLine + w.ContentText + Environment.NewLine : string.Empty) +
                  (!string.IsNullOrEmpty(w.ExpandedInformation) && !w.ExpandFooterArea && w.IsExpanded ? _Sep + Environment.NewLine + w.ExpandedInformation + Environment.NewLine : string.Empty) +
                  (w.RadioButtons != null && w.RadioButtons.Count > 0 ? _Sep + Environment.NewLine + RadioButtonsToText(w.RadioButtons) + Environment.NewLine : string.Empty) +
                  (w.Buttons != null && w.Buttons.Count > 0 ? _Sep + Environment.NewLine + ButtonsToText(w.Buttons) + Environment.NewLine : string.Empty) +
                  (w.CommonButtons != TaskDialogCommonButtons.None ? _Sep + Environment.NewLine + ButtonsToText(w.CommonButtons) + Environment.NewLine : string.Empty) +
                  (!string.IsNullOrEmpty(w.FooterText) ? _Sep + Environment.NewLine + w.FooterText + Environment.NewLine : string.Empty) +
                  (!string.IsNullOrEmpty(w.ExpandedInformation) && w.ExpandFooterArea && w.IsExpanded ? _Sep + Environment.NewLine + w.ExpandedInformation + Environment.NewLine : string.Empty);
        }

        private static string ButtonsToText(TaskDialogCommonButtons CommonButtons)
        {
            return ButtonsToText(null, CommonButtons);
        }

        private static string ButtonsToText(IList<TaskDialogButton> Buttons)
        {
            string s = string.Empty;
            if (Buttons != null)
            {
                foreach (TaskDialogButton b in Buttons)
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

        private static string RadioButtonsToText(IList<TaskDialogRadioButton> RadioButtons)
        {
            string s = string.Empty;
            if (RadioButtons != null)
            {
                foreach (TaskDialogRadioButton b in RadioButtons)
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
