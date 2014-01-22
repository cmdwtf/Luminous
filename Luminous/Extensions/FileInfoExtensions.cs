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

namespace System.IO
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading;

    /// <summary>Extension methods for the FileInfo class.</summary>
    public static class FileInfoExtensions
    {
        public static FileStream TryOpen(this FileInfo fileInfo, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
        {
            Contract.Requires<ArgumentNullException>(fileInfo != null);

            return fileInfo.Open(FileMode.Open, access, share);
        }

        public static FileStream WaitAndOpen(this FileInfo fileInfo, TimeSpan timeout, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
        {
            Contract.Requires<ArgumentNullException>(fileInfo != null);

            DateTime dt = DateTime.UtcNow;
            FileStream fs = null;
            while ((fs = TryOpen(fileInfo, access, share)) == null && (DateTime.UtcNow - dt) < timeout)
            {
                Thread.Sleep(250); // who knows better way and wants a free cookie? ;)
            }
            return fs;
        }
    }
}
