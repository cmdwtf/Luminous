#region License
// Copyright © 2014 £ukasz Œwi¹tkowski
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
    /// Implement to traverse unknown nodes.
    /// </summary>
    public interface ITraverser
    {
        void Traverse(TraverseNode<object> node);
    }

    /// <summary>
    /// Implement to traverse well known nodes.
    /// </summary>
    public interface ITraverser<T>
    {
        void Traverse(TraverseNode<T> node);
    }
}
