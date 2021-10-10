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
	using System.Diagnostics.Contracts;
	using System.Drawing;
	using System.Security.Permissions;
	using System.Windows.Forms;

	/// <summary>
	/// Represents a Windows combo box control with a custom popup control attached.
	/// </summary>
	[ToolboxBitmap(typeof(System.Windows.Forms.ComboBox)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Displays an editable text box with a drop-down list of permitted values.")]
	public partial class PopupComboBox : ComboBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PopupControl.PopupComboBox" /> class.
		/// </summary>
		public PopupComboBox()
		{
			dropDownHideTime = DateTime.UtcNow;
			InitializeComponent();
			base.DropDownHeight = base.DropDownWidth = 1;
			base.IntegralHeight = false;
		}

		private Popup dropDown;

		private Control dropDownControl;
		/// <summary>
		/// Gets or sets the drop down control.
		/// </summary>
		/// <value>The drop down control.</value>
		public Control DropDownControl
		{
			get => dropDownControl;
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);

				if (dropDownControl == value)
				{
					return;
				}

				dropDownControl = value;

				dropDown.Closed -= dropDown_Closed;
				dropDown.Dispose();

				dropDown = new Popup(value);
				dropDown.Closed += dropDown_Closed;
			}
		}

		private DateTime dropDownHideTime;
		private void dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e) => dropDownHideTime = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets a value indicating whether the combo box is displaying its drop-down portion.
		/// </summary>
		/// <value></value>
		/// <returns>true if the drop-down portion is displayed; otherwise, false. The default is false.
		/// </returns>
		public new bool DroppedDown
		{
			get => dropDown.Visible;
			set
			{
				if (DroppedDown)
				{
					HideDropDown();
				}
				else
				{
					ShowDropDown();
				}
			}
		}

		/// <summary>
		/// Occurs when the drop-down portion of a <see cref="T:System.Windows.Forms.ComboBox"/> is shown.
		/// </summary>
		public new event EventHandler DropDown;

		/// <summary>
		/// Shows the drop down.
		/// </summary>
		public void ShowDropDown()
		{
			if (dropDown != null)
			{
				if ((DateTime.UtcNow - dropDownHideTime).TotalSeconds > 0.5)
				{
					if (DropDown != null)
					{
						DropDown(this, EventArgs.Empty);
					}
					dropDown.Show(this);
				}
				else
				{
					dropDownHideTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 1));
					Focus();
				}
			}
		}

		/// <summary>
		/// Occurs when the drop-down portion of the <see cref="T:System.Windows.Forms.ComboBox"/> is no longer visible.
		/// </summary>
		public new event EventHandler DropDownClosed;

		/// <summary>
		/// Hides the drop down.
		/// </summary>
		public void HideDropDown()
		{
			if (dropDown != null)
			{
				dropDown.Hide();
				if (DropDownClosed != null)
				{
					DropDownClosed(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (NativeMethods.WM_COMMAND + NativeMethods.WM_REFLECT) && NativeMethods.HIWORD(m.WParam) == NativeMethods.CBN_DROPDOWN)
			{
				BeginInvoke(new MethodInvoker(ShowDropDown));
				return;
			}
			base.WndProc(ref m);
		}

		#region Unused Properties

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new int DropDownWidth
		{
			get => base.DropDownWidth;
			set => base.DropDownWidth = value;
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new int DropDownHeight
		{
			get => base.DropDownHeight;
			set => base.DropDownHeight = value;
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool IntegralHeight
		{
			get => base.IntegralHeight;
			set => base.IntegralHeight = value;
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new ObjectCollection Items => base.Items;

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new int ItemHeight
		{
			get => base.ItemHeight;
			set => base.ItemHeight = value;
		}

		#endregion
	}
}
