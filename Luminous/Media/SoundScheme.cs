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
		private static readonly SystemSound _default = new SystemSound(".Default");
		/// <summary>Gets the sound associated with the Default program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Default => _default;

		private static readonly SystemSound _appGPFault = new SystemSound("AppGPFault");
		/// <summary>Gets the sound associated with the AppGPFault program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound AppGPFault => _appGPFault;

		private static readonly SystemSound _CCSelect = new SystemSound("CCSelect");
		/// <summary>Gets the sound associated with the CCSelect program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound CCSelect => _CCSelect;

		private static readonly SystemSound _changeTheme = new SystemSound("ChangeTheme");
		/// <summary>Gets the sound associated with the ChangeTheme program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound ChangeTheme => _changeTheme;

		private static readonly SystemSound _close = new SystemSound("Close");
		/// <summary>Gets the sound associated with the Close program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Close => _close;

		private static readonly SystemSound _criticalBatteryAlarm = new SystemSound("CriticalBatteryAlarm");
		/// <summary>Gets the sound associated with the CriticalBatteryAlarm program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound CriticalBatteryAlarm => _criticalBatteryAlarm;

		private static readonly SystemSound _deviceConnect = new SystemSound("DeviceConnect");
		/// <summary>Gets the sound associated with the DeviceConnect program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound DeviceConnect => _deviceConnect;

		private static readonly SystemSound _deviceDisconnect = new SystemSound("DeviceDisconnect");
		/// <summary>Gets the sound associated with the DeviceDisconnect program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound DeviceDisconnect => _deviceDisconnect;

		private static readonly SystemSound _deviceFail = new SystemSound("DeviceFail");
		/// <summary>Gets the sound associated with the DeviceFail program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound DeviceFail => _deviceFail;

		private static readonly SystemSound _faxBeep = new SystemSound("FaxBeep");
		/// <summary>Gets the sound associated with the FaxBeep program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound FaxBeep => _faxBeep;

		private static readonly SystemSound _lowBatteryAlarm = new SystemSound("LowBatteryAlarm");
		/// <summary>Gets the sound associated with the LowBatteryAlarm program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound LowBatteryAlarm => _lowBatteryAlarm;

		private static readonly SystemSound _mailBeep = new SystemSound("MailBeep");
		/// <summary>Gets the sound associated with the MailBeep program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound MailBeep => _mailBeep;

		private static readonly SystemSound _maximize = new SystemSound("Maximize");
		/// <summary>Gets the sound associated with the Maximize program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Maximize => _maximize;

		private static readonly SystemSound _menuCommand = new SystemSound("MenuCommand");
		/// <summary>Gets the sound associated with the MenuCommand program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound MenuCommand => _menuCommand;

		private static readonly SystemSound _menuPopup = new SystemSound("MenuPopup");
		/// <summary>Gets the sound associated with the MenuPopup program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound MenuPopup => _menuPopup;

		private static readonly SystemSound _minimize = new SystemSound("Minimize");
		/// <summary>Gets the sound associated with the Minimize program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Minimize => _minimize;

		private static readonly SystemSound _open = new SystemSound("Open");
		/// <summary>Gets the sound associated with the Open program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound Open => _open;

		private static readonly SystemSound _printComplete = new SystemSound("PrintComplete");
		/// <summary>Gets the sound associated with the PrintComplete program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound PrintComplete => _printComplete;

		private static readonly SystemSound _restoreDown = new SystemSound("RestoreDown");
		/// <summary>Gets the sound associated with the RestoreDown program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound RestoreDown => _restoreDown;

		private static readonly SystemSound _restoreUp = new SystemSound("RestoreUp");
		/// <summary>Gets the sound associated with the RestoreUp program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound RestoreUp => _restoreUp;

		private static readonly SystemSound _showBand = new SystemSound("ShowBand");
		/// <summary>Gets the sound associated with the ShowBand program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound ShowBand => _showBand;

		private static readonly SystemSound _systemAsterisk = new SystemSound("SystemAsterisk");
		/// <summary>Gets the sound associated with the SystemAsterisk program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemAsterisk => _systemAsterisk;

		private static readonly SystemSound _systemExclamation = new SystemSound("SystemExclamation");
		/// <summary>Gets the sound associated with the SystemExclamation program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemExclamation => _systemExclamation;

		private static readonly SystemSound _systemExit = new SystemSound("SystemExit");
		/// <summary>Gets the sound associated with the SystemExit program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemExit => _systemExit;

		private static readonly SystemSound _systemHand = new SystemSound("SystemHand");
		/// <summary>Gets the sound associated with the SystemHand program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemHand => _systemHand;

		private static readonly SystemSound _systemNotification = new SystemSound("SystemNotification");
		/// <summary>Gets the sound associated with the SystemNotification program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemNotification => _systemNotification;

		private static readonly SystemSound _systemQuestion = new SystemSound("SystemQuestion");
		/// <summary>Gets the sound associated with the SystemQuestion program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemQuestion => _systemQuestion;

		private static readonly SystemSound _systemStart = new SystemSound("SystemStart");
		/// <summary>Gets the sound associated with the SystemStart program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound SystemStart => _systemStart;

		private static readonly SystemSound _windowsLogoff = new SystemSound("WindowsLogoff");
		/// <summary>Gets the sound associated with the WindowsLogoff program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound WindowsLogoff => _windowsLogoff;

		private static readonly SystemSound _windowsLogon = new SystemSound("WindowsLogon");
		/// <summary>Gets the sound associated with the WindowsLogon program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound WindowsLogon => _windowsLogon;

		private static readonly SystemSound _windowsUAC = new SystemSound("WindowsUAC");
		/// <summary>Gets the sound associated with the WindowsUAC program event in the current Windows sound scheme.</summary>
		/// <returns>A <see cref="T:SystemSound" />.</returns>
		public static SystemSound WindowsUAC => _windowsUAC;

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
