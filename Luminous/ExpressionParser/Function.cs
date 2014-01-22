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

    public sealed class Function : FunctionBase
    {
        public Function(string name, int parametersCount, Func<decimal[], decimal> evaluator)
        {
            this._name = name;
            this._parametersCount = parametersCount;
            this.GetValue = evaluator;
        }

        public override decimal Invoke(params decimal[] parameters)
        {
            return GetValue(parameters);
        }

        private string _name;
        public override string Name { get { return _name; } }

        private int _parametersCount;
        public override int ParametersCount { get { return _parametersCount; } }

        public readonly Func<decimal[], decimal> GetValue;
    }
}
