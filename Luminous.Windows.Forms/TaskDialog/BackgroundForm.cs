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
    /// Form used in LockSystem mode. Contains an image of grayed desktop.
    /// </summary>
    internal class BackgroundForm : Form
    {
        private Bitmap _background;

        public BackgroundForm(Bitmap background)
        {
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.None;
            Location = Point.Empty;
            Size = Screen.PrimaryScreen.Bounds.Size;
            StartPosition = FormStartPosition.Manual;
            Visible = true;
            _background = background;
        }

        protected override void OnShown(System.EventArgs e)
        {
            this.BackgroundImage = _background;
            this.DoubleBuffered = true;
            base.OnShown(e);
        }
    }
}
