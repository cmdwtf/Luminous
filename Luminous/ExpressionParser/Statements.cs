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

    public sealed class CoalesceStatement : StatementBase
    {
        public override decimal Invoke(params IEvaluableElement[] parameters)
        {
            decimal result = parameters[0].Evaluate();
            if (result == 0m)
            {
                result = parameters[1].Evaluate();
            }
            return result;
        }

        public override int ParametersCount
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "if"; }
        }
    }

    public sealed class IfStatement : StatementBase
    {
        public override decimal Invoke(params IEvaluableElement[] parameters)
        {
            return parameters[0].Evaluate() != 0m ? parameters[1].Evaluate() : parameters[2].Evaluate();
        }

        public override int ParametersCount
        {
            get { return 3; }
        }

        public override string Name
        {
            get { return "if"; }
        }
    }

    public sealed class ForStatement : StatementBase
    {
        public override decimal Invoke(params IEvaluableElement[] parameters)
        {
            parameters[0].Evaluate();
            int i = 0;
            while (parameters[1].Evaluate() != 0)
            {
                parameters[2].Evaluate();
                i++;
            }
            return i;
        }

        public override int ParametersCount
        {
            get { return 3; }
        }

        public override string Name
        {
            get { return "for"; }
        }
    }

    public sealed class WhileStatement : StatementBase
    {
        public override decimal Invoke(params IEvaluableElement[] parameters)
        {
            int i = 0;
            while (parameters[0].Evaluate() != 0)
            {
                parameters[1].Evaluate();
                i++;
            }
            return i;
        }

        public override int ParametersCount
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "while"; }
        }
    }
}
