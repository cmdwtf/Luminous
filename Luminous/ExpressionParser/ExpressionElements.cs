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

    [Flags]
    public enum ExpressionElements
    {
        None = 0,
        ArithmeticOperators = 1, // + - * / % ^
        LogicalOperators = 2, // ! || &&
        ComparisonOperators = 4, // == != < <= > >=
        AssignmentOperators = 8, // =
        AllOperators = ArithmeticOperators | LogicalOperators | ComparisonOperators | AssignmentOperators,
        Constants = 0x10, // pi e
        AllConstants = Constants,
        MathFunctions = 0x100, // sin pow …
        AllFunction = MathFunctions,
        NewLineOperator = 0x1000,
        Statements = 0x2000,
        AllControlFlowElements = NewLineOperator | Statements,
        All = AllOperators | AllConstants | AllFunction | AllControlFlowElements,
    }
}
