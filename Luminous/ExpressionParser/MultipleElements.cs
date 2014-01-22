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

namespace Luminous.ExpressionParser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("M{Name}<{Elements.Count}>")]
    internal sealed class MultipleElements : IExpressionElement
    {
        public MultipleElements(IEnumerable<IExpressionElement> elements)
        {
            this.Elements.AddRange(elements);
        }

        public readonly List<IExpressionElement> Elements = new List<IExpressionElement>();

        public string Name
        {
            get { return Elements.Count == 0 ? "?" : Elements[0].Name; }
        }
    }
}
