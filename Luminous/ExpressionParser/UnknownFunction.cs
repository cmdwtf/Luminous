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

    [DebuggerDisplay("?{Name}({ParametersCount})")]
    internal sealed class UnknownFunction : IFunction
    {
        public UnknownFunction(string name)
        {
            Name = name;
            ParametersCount = -1;
        }

        public decimal Invoke(params decimal[] parameters)
        {
            throw new InvalidOperationException("Invalid operation.");
        }

        public string Name { get; private set; }

        public int ParametersCount { get; internal set; }
    }
}
