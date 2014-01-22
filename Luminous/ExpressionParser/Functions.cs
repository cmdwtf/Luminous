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

    public sealed class AbsFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Abs(parameters[0]);
        }

        public override int ParametersCount
        {
            get { return 1; }
        }

        public override string Name
        {
            get { return "abs"; }
        }
    }

    public sealed class CeilingFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Ceiling(parameters[0]);
        }

        public override int ParametersCount
        {
            get { return 1; }
        }

        public override string Name
        {
            get { return "ceiling"; }
        }
    }

    public sealed class FloorFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Floor(parameters[0]);
        }

        public override int ParametersCount
        {
            get { return 1; }
        }

        public override string Name
        {
            get { return "floor"; }
        }
    }

    public sealed class MaxFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Max(parameters[0], parameters[1]);
        }

        public override int ParametersCount
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "max"; }
        }
    }

    public sealed class MinFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Min(parameters[0], parameters[1]);
        }

        public override int ParametersCount
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "min"; }
        }
    }

    public sealed class PowFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Power(parameters[0], parameters[1]);
        }

        public static decimal Power(decimal X, decimal N)
        {
            Func<decimal, decimal, decimal> Pow = null;
            Pow = (a, b) =>
            {
                if (b == 0) return 0;
                if (b < 0) return 1m / Pow(a, -b);

                decimal result = a;
                while (b-- > 1) result *= a;
                return result;
            };

            if (N == decimal.Truncate(N))
            {
                return Pow(X, N);
            }

            return (decimal)Math.Pow((double)X, (double)N);
        }

        public override int ParametersCount
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "pow"; }
        }
    }

    //public sealed class RandFunction : FunctionBaseElement
    //{
    //    private static Random R = new Random();

    //    public override decimal Invoke(params decimal[] parameters)
    //    {
    //        return (decimal)R.NextDouble();
    //    }

    //    public override int ParametersCount
    //    {
    //        get { return 0; }
    //    }

    //    public override string Name
    //    {
    //        get { return "rand"; }
    //    }
    //}

    public sealed class RoundFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Round(parameters[0], parameters[1]);
        }

        public static decimal Round(decimal left, decimal right)
        {
            if (right < 0m) right = -right;
            if (Math.Abs(left % right) >= right / 2m) left += right * Math.Sign(left % right);
            decimal result = left - left % right;
            return result;
        }

        public override int ParametersCount
        {
            get { return 2; }
        }

        public override string Name
        {
            get { return "round"; }
        }
    }

    public sealed class SignFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Sign(parameters[0]);
        }

        public override int ParametersCount
        {
            get { return 1; }
        }

        public override string Name
        {
            get { return "sign"; }
        }
    }

    public sealed class TruncateFunction : FunctionBase
    {
        public override decimal Invoke(params decimal[] parameters)
        {
            return Math.Truncate(parameters[0]);
        }

        public override int ParametersCount
        {
            get { return 1; }
        }

        public override string Name
        {
            get { return "truncate"; }
        }
    }
}
