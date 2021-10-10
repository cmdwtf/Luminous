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
	using System.Windows.Forms;

	/// <summary>
	/// Helper methods…
	/// </summary>
	internal class TaskDialogHelpers
	{
		public static string GetButtonName(TaskDialogResult button)
		{
			return button switch
			{
				TaskDialogResult.Abort => Properties.Resources.AbortText,
				TaskDialogResult.Cancel => Properties.Resources.CancelText,
				TaskDialogResult.Close => Properties.Resources.CloseText,
				TaskDialogResult.Continue => Properties.Resources.ContinueText,
				TaskDialogResult.Ignore => Properties.Resources.IgnoreText,
				TaskDialogResult.No => Properties.Resources.NoText,
				TaskDialogResult.NoToAll => Properties.Resources.NoToAllText,
				TaskDialogResult.OK => Properties.Resources.OKText,
				TaskDialogResult.Retry => Properties.Resources.RetryText,
				TaskDialogResult.Yes => Properties.Resources.YesText,
				TaskDialogResult.YesToAll => Properties.Resources.YesToAllText,
				_ => Properties.Resources.NoneText,
			};
		}

		public static DialogResult MakeDialogResult(TaskDialogResult result)
		{
			return result switch
			{
				TaskDialogResult.Abort => DialogResult.Abort,
				TaskDialogResult.Cancel or TaskDialogResult.Close => DialogResult.Cancel,
				TaskDialogResult.Ignore => DialogResult.Ignore,
				TaskDialogResult.No or TaskDialogResult.NoToAll => DialogResult.No,
				TaskDialogResult.OK or TaskDialogResult.Continue => DialogResult.OK,
				TaskDialogResult.Retry => DialogResult.Retry,
				TaskDialogResult.Yes or TaskDialogResult.YesToAll => DialogResult.Yes,
				_ => DialogResult.None,
			};
		}

		public static TaskDialogDefaultButton MakeTaskDialogDefaultButton(MessageBoxDefaultButton defaultButton)
		{
			return defaultButton switch
			{
				MessageBoxDefaultButton.Button1 => TaskDialogDefaultButton.Button1,
				MessageBoxDefaultButton.Button2 => TaskDialogDefaultButton.Button2,
				MessageBoxDefaultButton.Button3 => TaskDialogDefaultButton.Button3,
				_ => TaskDialogDefaultButton.None,
			};
		}
	}
}
