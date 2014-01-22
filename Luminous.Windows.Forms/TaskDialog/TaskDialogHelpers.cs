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

namespace Luminous.Windows.Forms
{
    using System.Windows.Forms;

    /// <summary>
    /// Helper methods…
    /// </summary>
    internal class TaskDialogHelpers
    {
        public static string GetButtonName(TaskDialogResult button)
        {
            switch (button)
            {
                case TaskDialogResult.Abort:
                    return Properties.Resources.AbortText;

                case TaskDialogResult.Cancel:
                    return Properties.Resources.CancelText;

                case TaskDialogResult.Close:
                    return Properties.Resources.CloseText;

                case TaskDialogResult.Continue:
                    return Properties.Resources.ContinueText;

                case TaskDialogResult.Ignore:
                    return Properties.Resources.IgnoreText;

                case TaskDialogResult.No:
                    return Properties.Resources.NoText;

                case TaskDialogResult.NoToAll:
                    return Properties.Resources.NoToAllText;

                case TaskDialogResult.OK:
                    return Properties.Resources.OKText;

                case TaskDialogResult.Retry:
                    return Properties.Resources.RetryText;

                case TaskDialogResult.Yes:
                    return Properties.Resources.YesText;

                case TaskDialogResult.YesToAll:
                    return Properties.Resources.YesToAllText;

                default:
                    return Properties.Resources.NoneText;
            }
        }

        public static DialogResult MakeDialogResult(TaskDialogResult Result)
        {
            switch (Result)
            {
                case TaskDialogResult.Abort:
                    return DialogResult.Abort;
                case TaskDialogResult.Cancel:
                case TaskDialogResult.Close:
                    return DialogResult.Cancel;
                case TaskDialogResult.Ignore:
                    return DialogResult.Ignore;
                case TaskDialogResult.No:
                case TaskDialogResult.NoToAll:
                    return DialogResult.No;
                case TaskDialogResult.OK:
                case TaskDialogResult.Continue:
                    return DialogResult.OK;
                case TaskDialogResult.Retry:
                    return DialogResult.Retry;
                case TaskDialogResult.Yes:
                case TaskDialogResult.YesToAll:
                    return DialogResult.Yes;
                default:
                    return DialogResult.None;
            }
        }

        public static TaskDialogDefaultButton MakeTaskDialogDefaultButton(MessageBoxDefaultButton DefaultButton)
        {
            switch (DefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    return TaskDialogDefaultButton.Button1;
                case MessageBoxDefaultButton.Button2:
                    return TaskDialogDefaultButton.Button2;
                case MessageBoxDefaultButton.Button3:
                    return TaskDialogDefaultButton.Button3;
                default:
                    return TaskDialogDefaultButton.None;
            }
        }
    }
}
