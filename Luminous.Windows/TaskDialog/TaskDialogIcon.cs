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
    /// Specifies constants defining which icon to display on what background.
    /// </summary>
    public enum TaskDialogIcon
    {
        /// <summary>
        /// The TaskDialog contains no icon. The background is white.
        /// </summary>
        None = 0,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of a lowercase letter i in a circle. The background is white.
        /// </summary>
        Information = 1,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of a question mark in a circle. The background is white.
        /// </summary>
        Question = 2,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of an exclamation point in a yellow triangle. The background is white.
        /// </summary>
        Warning = 3,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of white X in a red circle. The background is white.
        /// </summary>
        Error = 4,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of white check mark in a green shield. The background is green.
        /// </summary>
        SecuritySuccess = 5,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of a question mark in a blue shield. The background is blue.
        /// </summary>
        SecurityQuestion = 6,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of an exclamation point in a yellow shield. The background is yellow.
        /// </summary>
        SecurityWarning = 7,
        /// <summary>
        /// The TaskDialog contains a symbol consisting of white X in a red shield. The background is red.
        /// </summary>
        SecurityError = 8,
        /// <summary>
        /// The TaskDialog contains a symbol of a multicolored shield. The background is white.
        /// </summary>
        SecurityShield = 9,
        /// <summary>
        /// The TaskDialog contains a symbol of a multicolored shield. The background is blue-to-green gradient.
        /// </summary>
        SecurityShieldBlue = 10,
        /// <summary>
        /// The TaskDialog contains a symbol of a multicolored shield. The background is gray.
        /// </summary>
        SecurityShieldGray = 11,
    }
}
