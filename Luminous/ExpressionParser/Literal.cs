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
    using System.Diagnostics;
    using System.Globalization;

    [DebuggerDisplay("{Name}")]
    public sealed class Literal : ILiteral
    {
        public Literal(decimal value)
        {
            Name = value.ToString(CultureInfo.InvariantCulture);
            Value = value;
        }

        decimal IEvaluableElement.Evaluate() { return Value; }
        public decimal Value { get; private set; }
        public string Name { get; private set; }
    }
}
