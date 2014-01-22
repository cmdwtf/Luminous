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
    /// Traverses the object tree.
    /// </summary>
    public abstract class Traverser
    {
        protected object TraverseChild<T>(T child)
        {
            return TraverseChild(child, null, null);
        }

        protected object TraverseChild<T>(T child, string name)
        {
            return TraverseChild(child, name, null);
        }

        protected object TraverseChild<T>(T child, string name, object parent)
        {
            var t = this as ITraverser<T>;
            if (t != null)
            {
                var node = new TraverseNode<T>(child, name, parent);
                t.Traverse(node);
                return node.Result;
            }

            var unknown = this as ITraverser;
            if (unknown != null)
            {
                var node = new TraverseNode<object>(child, name, parent);
                unknown.Traverse(node);
                return node.Result;
            }

            throw new TraverserNotImplementedException(string.Format("Cannot traverse an instance of “{0}” type. Please implement a proper ITraverser interface.", child.GetType().Name));
        }
    }
}
