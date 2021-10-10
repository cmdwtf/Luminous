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
	using System.Collections.Generic;

	public sealed partial class ParsedExpression
	{
		internal ParsedExpression(IList<IExpressionElement> elements, bool zeroOnError)
		{
			ZeroOnError = zeroOnError;
			Elements = elements;
			// validate the expression and create the AST tree
			var stack = new Stack<KeyValuePair<int, IExpressionElement>>();
			var tree = new Stack<AstNode>();
			int l = elements.Count;
			for (int i = 0; i < l; i++)
			{
				IExpressionElement element = elements[i];
				if (element is AssignmentOperator)
				{
					stack.Pop();
					KeyValuePair<int, IExpressionElement> var = stack.Pop();
					if (!(var.Value is IVariable))
					{
						throw new InvalidOperationException("The left-hand side of an assignment must be a variable.");
					}
					elements[var.Key] = new VariableReference((IVariable)var.Value);
					stack.Push(new KeyValuePair<int, IExpressionElement>(-1, null));

					var node = new AstNode(this, element);
					node.Children.Add(tree.Pop());
					node.Children.Insert(0, tree.Pop());
					tree.Push(node);
				}
				else if (element is ILiteral)
				{
					tree.Push(new AstNode(this, element));
					stack.Push(new KeyValuePair<int, IExpressionElement>(i, element));
				}
				else if (element is IConstant)
				{
					tree.Push(new AstNode(this, element));
					stack.Push(new KeyValuePair<int, IExpressionElement>(i, element));
				}
				else if (element is IVariable)
				{
					tree.Push(new AstNode(this, element));
					stack.Push(new KeyValuePair<int, IExpressionElement>(i, element));
				}
				else if (element is IUnaryOperator)
				{
					stack.Pop();
					stack.Push(new KeyValuePair<int, IExpressionElement>(-1, null));

					var node = new AstNode(this, element);
					node.Children.Add(tree.Pop());
					tree.Push(node);
				}
				else if (element is IBinaryOperator)
				{
					stack.Pop();
					stack.Pop();
					stack.Push(new KeyValuePair<int, IExpressionElement>(-1, null));

					var node = new AstNode(this, element);
					node.Children.Add(tree.Pop());
					node.Children.Insert(0, tree.Pop());
					tree.Push(node);
				}
				else if (element is IFunction)
				{
					var node = new AstNode(this, element);
					var func = element as IFunction;
					int _l = func.ParametersCount;
					for (int _i = 0; _i < _l; _i++)
					{
						stack.Pop();
						node.Children.Insert(0, tree.Pop());
					}
					stack.Push(new KeyValuePair<int, IExpressionElement>(-1, null));
					tree.Push(node);
				}
				else
				{
					throw new ArgumentException(string.Format("Unexpected element: ‘{0}’.", element.Name));
				}
			}
			ExpressionTree = tree.Pop();
			if (tree.Count > 0)
			{
				throw new ArgumentException(string.Format("Unexpected element: ‘{0}’.", tree.Pop().Value.Name));
			}
		}

		public bool ZeroOnError { get; set; }
		private IList<IExpressionElement> Elements { get; set; }
		public IAstTreeNode ExpressionTree { get; private set; }

		private bool? _hasUndefinedElements;
		public bool HasUndefinedElements
		{
			get
			{
				if (!_hasUndefinedElements.HasValue)
				{
					_hasUndefinedElements = false;
					foreach (IExpressionElement element in Elements)
					{
						if ((element is UnknownFunction) || (element is UnknownVariable))
						{
							_hasUndefinedElements = true;
							break;
						}
					}
				}
				return _hasUndefinedElements.Value;
			}
		}

		public EvaluateUndefinedFunction EvaluateUndefinedFunction;
		public EvaluateUndefinedVariable EvaluateUndefinedVariable;

		public decimal Evaluate() => ExpressionTree.Evaluate();
	}
}
