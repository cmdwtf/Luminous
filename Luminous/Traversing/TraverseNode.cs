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

namespace Luminous.Traversing
{
    /// <summary>
    /// Represents the traversed node.
    /// </summary>
    public class TraverseNode<T>
    {
        public readonly T Value;
        public readonly string Name;
        public readonly object ParentValue;
        public object Result;

        public TraverseNode(T value)
        {
            Value = value;
        }

        public TraverseNode(T value, string name)
            : this(value)
        {
            Name = name;
        }

        public TraverseNode(T value, string name, object parentValue)
            : this(value, name)
        {
            ParentValue = parentValue;
        }
    }
}
