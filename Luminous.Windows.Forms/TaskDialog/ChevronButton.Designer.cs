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
	partial class ChevronButton
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			//
			//ChevronButton
			//
			this.BackColor = System.Drawing.Color.Transparent;
			this.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
			this.FlatAppearance.BorderSize = 0;
			this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.Visible = false;
			this.ResumeLayout(false);
		}

		#endregion
	}

}
