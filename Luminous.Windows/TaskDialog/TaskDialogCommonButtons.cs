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

    /// <summary>
    /// Specifies the push buttons displayed in the task dialog. If no common buttons are specified and no custom buttons are specified, the task dialog will contain the OK button by default.
    /// </summary>
    [Flags]
    public enum TaskDialogCommonButtons
    {
        /// <summary>
        /// The task dialog does not contain any common push button. If no common buttons are specified and no custom buttons are specified, the task dialog will contain the OK button by default.
        /// </summary>
        None = 0,
        /// <summary>
        /// The task dialog contains the OK button.
        /// </summary>
        OK = 1,
        /// <summary>
        /// The task dialog contains the Yes button.
        /// </summary>
        Yes = 2,
        /// <summary>
        /// The task dialog contains the No button.
        /// </summary>
        No = 4,
        /// <summary>
        /// /// The task dialog contains the Abort button.
        /// </summary>
        Abort = 8,
        /// <summary>
        /// /// The task dialog contains the Retry button.
        /// </summary>
        Retry = 16,
        /// <summary>
        /// /// The task dialog contains the Ignore button.
        /// </summary>
        Ignore = 32,
        /// <summary>
        /// The task dialog contains the Cancel button.
        /// </summary>
        Cancel = 64,
        /// <summary>
        /// The task dialog contains the Close button.
        /// </summary>
        Close = 128,

        /// <summary>
        /// The task dialog contains OK and Cancel buttons.
        /// </summary>
        OKCancel = OK | Cancel,
        /// <summary>
        /// The task dialog contains Yes and No buttons.
        /// </summary>
        YesNo = Yes | No,
        /// <summary>
        /// The task dialog contains Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel = Yes | No | Cancel,
        /// <summary>
        /// The task dialog contains Retry and Cancel buttons.
        /// </summary>
        RetryCancel = Retry | Cancel,
        /// <summary>
        /// The task dialog contains Abort, Retry, and Ignore buttons.
        /// </summary>
        AbortRetryIgnore = Abort | Retry | Ignore,
    }
}
