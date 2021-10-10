#region License
// Copyright © 2021 Chris Marc Dailey (nitz) <https://cmd.wtf>
// Copyright © 2014 Łukasz Świątkowski <http://www.lukesw.net/>
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
#endregion License

namespace Luminous.ExpressionParser
{
	using System;

	public sealed class NewLineOperator : BinaryOperator
	{
		public override string Name => ";";

		public override decimal Invoke(decimal left, decimal right) => right;

		public override int Precedence => int.MinValue;
	}

	public class AssignmentOperator : BinaryOperator
	{
		public override string Name => "=";

		public virtual decimal Invoke(IVariable left, decimal right)
		{
			left.Value = right;
			return right;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public sealed override decimal Invoke(decimal left, decimal right) => throw new InvalidOperationException();

		public override int Precedence => 0;

		public override OperatorAssociativity Associativity => OperatorAssociativity.RightAssociative;
	}

	public class AssignmentOperator<TOperator> : AssignmentOperator
		where TOperator : BinaryOperator, new()
	{
		private TOperator _operator;
		private TOperator Operator
		{
			get
			{
				if (_operator == null)
				{
					_operator = new TOperator();
				}
				return _operator;
			}
		}

		public override string Name => Operator.Name + "=";

		public override decimal Invoke(IVariable left, decimal right) => left.Value = Operator.Invoke(left.Value, right);
	}

	public sealed class ConditionalOrOperator : BinaryOperator
	{
		public override string Name => "||";

		public override decimal Invoke(decimal left, decimal right) => ((left != 0m) || (right != 0m)) ? 1m : 0m;

		public override int Precedence => 100;
	}

	public sealed class ConditionalAndOperator : BinaryOperator
	{
		public override string Name => "&&";

		public override decimal Invoke(decimal left, decimal right) => ((left != 0m) && (right != 0m)) ? 1m : 0m;

		public override int Precedence => 200;
	}

	public sealed class EqualityOperator : BinaryOperator
	{
		public override string Name => "==";

		public override decimal Invoke(decimal left, decimal right) => (left == right) ? 1m : 0m;

		public override int Precedence => 300;
	}

	public sealed class InequalityOperator : BinaryOperator
	{
		public override string Name => "!=";

		public override decimal Invoke(decimal left, decimal right) => (left != right) ? 1m : 0m;

		public override int Precedence => 300;
	}

	public class LessThanOperator : BinaryOperator
	{
		public override string Name => "<";

		public override decimal Invoke(decimal left, decimal right) => (left < right) ? 1m : 0m;

		public override int Precedence => 400;
	}

	public class GreaterThanOperator : BinaryOperator
	{
		public override string Name => ">";

		public override decimal Invoke(decimal left, decimal right) => (left > right) ? 1m : 0m;

		public override int Precedence => 400;
	}

	public class LessThanOrEqualToOperator : BinaryOperator
	{
		public override string Name => "<=";

		public override decimal Invoke(decimal left, decimal right) => (left <= right) ? 1m : 0m;

		public override int Precedence => 400;
	}

	public class GreaterThanOrEqualToOperator : BinaryOperator
	{
		public override string Name => ">=";

		public override decimal Invoke(decimal left, decimal right) => (left >= right) ? 1m : 0m;

		public override int Precedence => 400;
	}

	public class RoundingOperator : BinaryOperator
	{
		public override string Name => "%%";

		public override decimal Invoke(decimal left, decimal right) => RoundFunction.Round(left, right);

		public override int Precedence => 450;
	}

	public sealed class AdditionOperator : BinaryOperator
	{
		public override string Name => "+";

		public override decimal Invoke(decimal left, decimal right) => left + right;

		public override int Precedence => 500;

		public override OperatorAssociativity Associativity => OperatorAssociativity.NonAssociative;
	}

	public sealed class SubtractionOperator : BinaryOperator
	{
		public override string Name => "-";

		public override decimal Invoke(decimal left, decimal right) => left - right;

		public override int Precedence => 500;
	}

	public sealed class MultiplicationOperator : BinaryOperator
	{
		public override string Name => "*";

		public override decimal Invoke(decimal left, decimal right) => left * right;

		public override int Precedence => 600;

		public override OperatorAssociativity Associativity => OperatorAssociativity.NonAssociative;
	}

	public sealed class DivisionOperator : BinaryOperator
	{
		public override string Name => "/";

		public override decimal Invoke(decimal left, decimal right) => left / right;

		public override int Precedence => 600;
	}

	public sealed class FloorDivisionOperator : BinaryOperator
	{
		public override string Name => "//";

		public override decimal Invoke(decimal left, decimal right) => decimal.Truncate(left / right);

		public override int Precedence => 600;
	}

	public sealed class ModulusOperator : BinaryOperator
	{
		public override string Name => "%";

		public override decimal Invoke(decimal left, decimal right) => left % right;

		public override int Precedence => 600;
	}

	public class PowerOperator : BinaryOperator
	{
		public override string Name => "**";

		public override decimal Invoke(decimal left, decimal right) => PowFunction.Power(left, right);

		public override int Precedence => 700;

		public override OperatorAssociativity Associativity => OperatorAssociativity.RightAssociative;
	}

	public sealed class LogicalNegationOperator : UnaryOperator
	{
		public override string Name => "!";

		public override decimal Invoke(decimal value) => (value == 0m) ? 1m : 0m;

		public override int Precedence => 800;
	}

	public sealed class NumericNegationOperator : UnaryOperator
	{
		public override string Name => "-";

		public override decimal Invoke(decimal value) => -value;

		public override int Precedence => 800;
	}

	public sealed class UnaryPlusOperator : UnaryOperator
	{
		public override string Name => "+";

		public override decimal Invoke(decimal value) => value;

		public override int Precedence => 800;
	}
}
