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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>Extension methods for the Stream class.</summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Returns stream’s contents as an array of bytes.
        /// </summary>
        public static byte[] ReadToEnd(this Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            long? originalPosition = null;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            finally
            {
                if (stream.CanSeek && originalPosition.HasValue && originalPosition.Value >= 0)
                {
                    stream.Position = originalPosition.Value;
                }
            }
        }
    }
}
