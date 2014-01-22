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

namespace Luminous.Windows.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    public sealed class ApplicationExitCommand : ICommand
    {
        #region Static Members

        public static ICommand Instance
        {
            get { return _instance; }
        }

        private static ICommand _instance = new ApplicationExitCommand();

        #endregion

        #region Instance Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            int exitCode = parameter is int ? (int)parameter : 0;

            if (Application.Current != null)
            {
                Application.Current.Shutdown(exitCode);
            }
            else
            {
                Environment.Exit(exitCode);
            }
        }

        #endregion
    }
}
