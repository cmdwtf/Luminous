#region License
// Copyright © 2010 Łukasz Świątkowski.
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

namespace Luminous
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Provides methods for object serialization/deserialization.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Deserializes an object from the array of bytes.
        /// </summary>
        [Pure]
        public static T Deserialize<T>(byte[] array)
        {
            Contract.Requires<ArgumentNullException>(array != null);
            Contract.Requires<ArgumentException>(array.Length > 0);

            using (MemoryStream ms = new MemoryStream(array))
            {
                Contract.Assume(ms.CanSeek);
                Contract.Assume(ms.Length > 0);

                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Serializes the object to the array of bytes.
        /// </summary>
        [Pure]
        public static byte[] Serialize<T>(T obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Returns a deep copy of the object using serialization.
        /// </summary>
        [Pure]
        public static T CopyBySerialization<T>(this T obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return Deserialize<T>(Serialize<T>(obj));
        }

    }
}
