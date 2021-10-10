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

namespace Luminous.Windows.Forms
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
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
			get => base.Enabled;
			set => base.Enabled = value;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool AllowDrop
		{
			get => base.AllowDrop;
			set => base.AllowDrop = value;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get => base.BackgroundImage;
			set => base.BackgroundImage = value;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override ImageLayout BackgroundImageLayout
		{
			get => base.BackgroundImageLayout;
			set => base.BackgroundImageLayout = value;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get => base.CausesValidation;
			set => base.CausesValidation = value;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get => base.Font;
			set => base.Font = value;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new ImeMode ImeMode
		{
			get => base.ImeMode;
			set => base.ImeMode = value;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool TabStop
		{
			get => base.TabStop;
			set => base.TabStop = value;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[Browsable(false)]
		public override string Text
		{
			get => base.Text;
			set => base.Text = value;
		}

		#endregion

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			timer.Enabled = Enabled;
		}

		private double _currentFrame = -1;

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			if (!timer.Enabled || _currentFrame < 0)
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
			double frame = _currentFrame + index / 50.0;
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

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (!IsHandleCreated || !timer.Enabled)
			{
				return;
			}
			if (_currentFrame < 0)
			{
				_currentFrame = 0;
			}
			else
			{
				_currentFrame += .01;
			}
			if (_currentFrame > 1.0)
			{
				_currentFrame = 0;
			}
			Invalidate();
		}
	}
}
