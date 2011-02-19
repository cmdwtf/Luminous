#region License
// Copyright © 2011 Łukasz Świątkowski
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

namespace Luminous.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>Extension methods for the FileInfo class.</summary>
    public static class FileInfoExtensions //?
    {
        public static FileStream TryOpenFile(string path, FileAccess access, FileShare share)
        {
            try
            {
                if (!File.Exists(path)) return null;
                return File.Open(path, FileMode.Open, access, share);
            }
            catch (IOException) { return null; }
            catch (UnauthorizedAccessException) { return null; }
        }

        public static FileStream WaitAndOpenFile(string path, FileAccess access, FileShare share, TimeSpan timeout)
        {
            DateTime dt = DateTime.UtcNow;
            FileStream fs = null;
            while ((fs = TryOpenFile(path, access, share)) == null && (DateTime.UtcNow - dt) < timeout)
            {
                Thread.Sleep(250); // who knows better way and wants a free cookie? ;)
            }
            return fs;
        }

        #region " Other Methods"
        public static FileStream TryOpenFileForReading(this FileInfo fileInfo) { return TryOpenFileForReading(fileInfo.FullName); }
        public static FileStream TryOpenFileForReading(string path) { return TryOpenFile(path, FileAccess.Read); }
        public static FileStream TryOpenFileForWriting(this FileInfo fileInfo) { return TryOpenFileForWriting(fileInfo.FullName); }
        public static FileStream TryOpenFileForWriting(string path) { return TryOpenFile(path, FileAccess.ReadWrite); }
        public static FileStream TryOpenFile(this FileInfo fileInfo, FileAccess access) { return TryOpenFile(fileInfo.FullName, access); }
        public static FileStream TryOpenFile(string path, FileAccess access) { return TryOpenFile(path, access, FileShare.None); }
        public static FileStream TryOpenFile(this FileInfo fileInfo, FileAccess access, FileShare share) { return TryOpenFile(fileInfo.FullName, access, share); }
        public static FileStream WaitAndOpenFileForReading(this FileInfo fileInfo, TimeSpan timeout) { return WaitAndOpenFileForReading(fileInfo.FullName, timeout); }
        public static FileStream WaitAndOpenFileForReading(string path, TimeSpan timeout) { return WaitAndOpenFile(path, FileAccess.Read, timeout); }
        public static FileStream WaitAndOpenFileForWriting(this FileInfo fileInfo, TimeSpan timeout) { return WaitAndOpenFileForWriting(fileInfo.FullName, timeout); }
        public static FileStream WaitAndOpenFileForWriting(string path, TimeSpan timeout) { return WaitAndOpenFile(path, FileAccess.ReadWrite, timeout); }
        public static FileStream WaitAndOpenFile(this FileInfo fileInfo, FileAccess access, TimeSpan timeout) { return WaitAndOpenFile(fileInfo.FullName, access, timeout); }
        public static FileStream WaitAndOpenFile(string path, FileAccess access, TimeSpan timeout) { return WaitAndOpenFile(path, access, FileShare.None, timeout); }
        public static FileStream WaitAndOpenFile(this FileInfo fileInfo, FileAccess access, FileShare share, TimeSpan timeout) { return WaitAndOpenFile(fileInfo.FullName, access, share, timeout); }
        #endregion
    }
}
