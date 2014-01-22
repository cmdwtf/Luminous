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
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Button which preferred width is always divisible by 25.
    /// </summary>
    internal partial class Button : System.Windows.Forms.Button, IButtonControlWithImage
    {
        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize = base.GetPreferredSize(proposedSize);
            proposedSize.Width += 24;
            if (this.Image != null)
            {
                proposedSize.Width += this.Image.Width;
            }
            proposedSize.Width -= proposedSize.Width % 25;
            proposedSize.Width = proposedSize.Width < 75 ?  75 : proposedSize.Width;
            proposedSize.Height = proposedSize.Height < 25 ? 25 : proposedSize.Height;
            return proposedSize;
        }
    }

    public interface IButtonControlWithImage : IButtonControl
    {
        Image Image { get; set; }
    }
}