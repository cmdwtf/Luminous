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
	using System.Diagnostics;

	[DebuggerDisplay("{Name}[2]")]
	public abstract class BinaryOperator : IBinaryOperator
	{
		public abstract decimal Invoke(decimal left, decimal right);
		public virtual OperatorAssociativity Associativity => OperatorAssociativity.LeftAssociative;
		public abstract int Precedence { get; }
		public abstract string Name { get; }
	}
}
