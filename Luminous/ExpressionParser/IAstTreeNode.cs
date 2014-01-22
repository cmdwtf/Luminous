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

namespace Luminous.ExpressionParser
{
    using System;
    using System.Collections.Generic;

    public interface IAstTreeNode : IEvaluableElement
    {
        new string Name { get; }
        IExpressionElement Value { get; set; }
        IList<IAstTreeNode> Children { get; }

        /// <summary>Returns a <see cref="System.String"/> that represents this expression node.</summary>
        /// <returns>A <see cref="System.String"/> that represents this expression node.</returns>
        string ToString();
    }
}
