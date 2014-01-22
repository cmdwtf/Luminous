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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Windows 8/Phone-like indeterminate progress bar.
    /// </summary>
    public partial class IndeterminateProgressBar : Control
    {
        public IndeterminateProgressBar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            Enabled = false;
        }

        #region Properties

        [DefaultValue(false)]
        public new bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CausesValidation
        {
            get { return base.CausesValidation; }
            set { base.CausesValidation = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public ImeMode ImeMode
        {
            get { return base.ImeMode; }
            set { base.ImeMode = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            timer.Enabled = Enabled;
        }

        private double currentFrame = -1;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (!timer.Enabled || currentFrame < 0)
            {
                return;
            }
            using (Brush b = new SolidBrush(ForeColor))
            {
                for (int i = -2; i <= 2; i++)
                {
                    DrawDot(pe, b, i);
                }
            }
        }

        private void DrawDot(PaintEventArgs pe, Brush b, int index)
        {
            double frame = currentFrame + index / 50.0;
            /*const double min = -8;
            const double max = 8;
            double arg = 2 * 2 * (frame - .5);
            double tan = arg * arg * arg * arg * arg;
            double val = (tan - min) / (max - min);*/
            float val = (float)(Math.Pow(4 * frame - 2, 5) + 8) / 16;
            float x = (float)(val * Width + /*index bias*/index * 16 + /*frame bias*/ 2 * (frame * 100 - 50));
            if (RightToLeft == RightToLeft.Yes)
            {
                x = Width - x;
            }
            pe.Graphics.FillRectangle(b, x, Height / 2 - 2, 4, 4);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!IsHandleCreated || !timer.Enabled)
            {
                return;
            }
            if (currentFrame < 0)
            {
                currentFrame = 0;
            }
            else
            {
                currentFrame += .01;
            }
            if (currentFrame > 1.0)
            {
                currentFrame = 0;
            }
            Invalidate();
        }
    }
}
