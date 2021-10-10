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

namespace Luminous.Media
{
	using System;

	/// <summary>Retrieves sounds associated with current Windows sound scheme. This class cannot be inherited.</summary>
	public static partial class SoundScheme
	{
		/// <summary>Gets the sound associated with the Default program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Default { get; } = new SystemSound(".Default");

		/// <summary>Gets the sound associated with the AppGPFault program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound AppGPFault { get; } = new SystemSound("AppGPFault");

		/// <summary>Gets the sound associated with the CCSelect program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound CCSelect { get; } = new SystemSound("CCSelect");

		/// <summary>Gets the sound associated with the ChangeTheme program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound ChangeTheme { get; } = new SystemSound("ChangeTheme");

		/// <summary>Gets the sound associated with the Close program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Close { get; } = new SystemSound("Close");

		/// <summary>Gets the sound associated with the CriticalBatteryAlarm program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound CriticalBatteryAlarm { get; } = new SystemSound("CriticalBatteryAlarm");

		/// <summary>Gets the sound associated with the DeviceConnect program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound DeviceConnect { get; } = new SystemSound("DeviceConnect");

		/// <summary>Gets the sound associated with the DeviceDisconnect program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound DeviceDisconnect { get; } = new SystemSound("DeviceDisconnect");

		/// <summary>Gets the sound associated with the DeviceFail program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound DeviceFail { get; } = new SystemSound("DeviceFail");

		/// <summary>Gets the sound associated with the FaxBeep program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound FaxBeep { get; } = new SystemSound("FaxBeep");

		/// <summary>Gets the sound associated with the LowBatteryAlarm program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound LowBatteryAlarm { get; } = new SystemSound("LowBatteryAlarm");

		/// <summary>Gets the sound associated with the MailBeep program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound MailBeep { get; } = new SystemSound("MailBeep");

		/// <summary>Gets the sound associated with the Maximize program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Maximize { get; } = new SystemSound("Maximize");

		/// <summary>Gets the sound associated with the MenuCommand program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound MenuCommand { get; } = new SystemSound("MenuCommand");

		/// <summary>Gets the sound associated with the MenuPopup program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound MenuPopup { get; } = new SystemSound("MenuPopup");

		/// <summary>Gets the sound associated with the Minimize program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Minimize { get; } = new SystemSound("Minimize");

		/// <summary>Gets the sound associated with the Open program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Open { get; } = new SystemSound("Open");

		/// <summary>Gets the sound associated with the PrintComplete program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound PrintComplete { get; } = new SystemSound("PrintComplete");

		/// <summary>Gets the sound associated with the RestoreDown program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound RestoreDown { get; } = new SystemSound("RestoreDown");

		/// <summary>Gets the sound associated with the RestoreUp program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound RestoreUp { get; } = new SystemSound("RestoreUp");

		/// <summary>Gets the sound associated with the ShowBand program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound ShowBand { get; } = new SystemSound("ShowBand");

		/// <summary>Gets the sound associated with the SystemAsterisk program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemAsterisk { get; } = new SystemSound("SystemAsterisk");

		/// <summary>Gets the sound associated with the SystemExclamation program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemExclamation { get; } = new SystemSound("SystemExclamation");

		/// <summary>Gets the sound associated with the SystemExit program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemExit { get; } = new SystemSound("SystemExit");

		/// <summary>Gets the sound associated with the SystemHand program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemHand { get; } = new SystemSound("SystemHand");

		/// <summary>Gets the sound associated with the SystemNotification program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemNotification { get; } = new SystemSound("SystemNotification");

		/// <summary>Gets the sound associated with the SystemQuestion program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemQuestion { get; } = new SystemSound("SystemQuestion");

		/// <summary>Gets the sound associated with the SystemStart program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemStart { get; } = new SystemSound("SystemStart");

		/// <summary>Gets the sound associated with the WindowsLogoff program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound WindowsLogoff { get; } = new SystemSound("WindowsLogoff");

		/// <summary>Gets the sound associated with the WindowsLogon program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound WindowsLogon { get; } = new SystemSound("WindowsLogon");

		/// <summary>Gets the sound associated with the WindowsUAC program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound WindowsUAC { get; } = new SystemSound("WindowsUAC");

		#region Common Sounds

		public static SystemSound Error => SystemHand;

		public static SystemSound Information => SystemAsterisk;

		public static SystemSound Question => SystemQuestion;

		public static SystemSound Security => Environment.OSVersion.Version.Major >= 6
					   ? WindowsUAC
					   : SystemHand;

		public static SystemSound Warning => SystemExclamation;

		#endregion
	}
}
