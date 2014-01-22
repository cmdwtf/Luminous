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
    /// Specifies which task dialog button the user clicked.
    /// </summary>
    public enum TaskDialogResult
    {
        /// <summary>
        /// The task dialog returns no result.
        /// </summary>
        None = 0,
        /// <summary>
        /// The result value of the task dialog is OK.
        /// </summary>
        OK = 1,
        /// <summary>
        /// The result value of the task dialog is Yes.
        /// </summary>
        Yes = 2,
        /// <summary>
        /// The result value of the task dialog is No.
        /// </summary>
        No = 3,
        /// <summary>
        /// The result value of the task dialog is Abort.
        /// </summary>
        Abort = 4,
        /// <summary>
        /// The result value of the task dialog is Retry.
        /// </summary>
        Retry = 5,
        /// <summary>
        /// The result value of the task dialog is Ignore.
        /// </summary>
        Ignore = 6,
        /// <summary>
        /// The result value of the task dialog is Cancel.
        /// </summary>
        Cancel = 7,
        /// <summary>
        /// The result value of the task dialog is Close.
        /// </summary>
        Close = 8,
    }
}
