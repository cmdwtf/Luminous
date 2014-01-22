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
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;

    public sealed partial class ParsedExpression
    {
        public IAstTreeNode CreateAstNode(IExpressionElement value, IList<IAstTreeNode> children)
        {
            var n = new AstNode(this, value);
            n.Children.AddRange(children);
            return n;
        }

        [DebuggerDisplay("{Name}<{Children.Count}>")]
        private sealed class AstNode : IAstTreeNode
        {
            public AstNode(ParsedExpression parsedExpression, IExpressionElement value)
            {
                _parsedExpression = parsedExpression;
                Value = value;
                Children = new List<IAstTreeNode>();
            }

            private readonly ParsedExpression _parsedExpression;

            public IExpressionElement Value { get; internal set; }
            IExpressionElement IAstTreeNode.Value { get { return Value; }  set { Value = value; } }

            public readonly List<IAstTreeNode> Children;
            IList<IAstTreeNode> IAstTreeNode.Children { get { return Children/*.AsReadOnly()*/; } }

            public string Name { get { return Value.Name; } }

            public decimal Evaluate()
            {
                try
                {
                    if (Value is VariableReference)
                    {
                        return (Value as VariableReference).Variable.Value;
                    }

                    if (Value is AssignmentOperator)
                    {
                        IVariable var = Children[0].Value as IVariable;
                        if (var == null)
                        {
                            throw new InvalidOperationException("The left-hand side of an assignment must be a variable.");
                        }
                        return (Value as AssignmentOperator).Invoke(var, Children[1].Evaluate());
                    }

                    if (Value is ILiteral)
                    {
                        return (Value as ILiteral).Value;
                    }

                    if (Value is IConstant)
                    {
                        return (Value as IConstant).Value;
                    }

                    if (Value is UnknownVariable)
                    {
                        if (_parsedExpression.EvaluateUndefinedVariable == null)
                        {
                            throw new InvalidOperationException(string.Format("Cannot evaluate the value of ‘{0}’ variable.", Value.Name));
                        }
                        return (Value as UnknownVariable).Value = _parsedExpression.EvaluateUndefinedVariable(Value.Name);
                    }

                    if (Value is IVariable)
                    {
                        return (Value as IVariable).Value;
                    }

                    if (Value is IUnaryOperator)
                    {
                        return (Value as IUnaryOperator).Invoke(Children[0].Evaluate());
                    }

                    if (Value is IBinaryOperator)
                    {
                        return (Value as IBinaryOperator).Invoke(Children[0].Evaluate(), Children[1].Evaluate());
                    }

                    if (Value is UnknownFunction)
                    {
                        if (_parsedExpression.EvaluateUndefinedFunction == null)
                        {
                            throw new InvalidOperationException(string.Format("Cannot evaluate the value of ‘{0}’ function.", Value.Name));
                        }
                        IFunction func = Value as IFunction;
                        if (Children.Count != func.ParametersCount)
                        {
                            throw new ArgumentException(string.Format("There is no function ‘{0}’ with {1} parameters defined.", func.Name, func.ParametersCount));
                        }
                        return _parsedExpression.EvaluateUndefinedFunction(Value.Name, Children.Select(node => node.Evaluate()).ToArray());
                    }

                    if (Value is IStatement)
                    {
                        IStatement statement = Value as IStatement;
                        if (Children.Count != statement.ParametersCount)
                        {
                            throw new ArgumentException(string.Format("There is no statement ‘{0}’ with {1} parameters defined.", statement.Name, statement.ParametersCount));
                        }
                        return statement.Invoke(Children.Cast<IEvaluableElement>().ToArray());
                    }

                    if (Value is IFunction)
                    {
                        IFunction func = Value as IFunction;
                        if (Children.Count != func.ParametersCount)
                        {
                            throw new ArgumentException(string.Format("There is no function ‘{0}’ with {1} parameters defined.", func.Name, func.ParametersCount));
                        }
                        return func.Invoke(Children.Select(node => node.Evaluate()).ToArray());
                    }

                    throw new ArgumentException(string.Format("Unexpected element: ‘{0}’.", Value.Name));
                }
                catch (ArithmeticException)
                {
                    if (_parsedExpression.ZeroOnError) return 0m;
                    throw;
                }
            }

            public override string ToString()
            {
                if (Value is ILiteral)
                {
                    return (Value as ILiteral).Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

                if (Value is IConstant)
                {
                    return (Value as IConstant).Name;
                }

                if (Value is VariableReference)
                {
                    return RenameToSafeName((Value as VariableReference).Variable.Name);
                }

                if (Value is IVariable)
                {
                    return RenameToSafeName((Value as IVariable).Name);
                }

                if (Value is IUnaryOperator)
                {
                    return string.Format("{0}{1}", Value.Name, Children[0].ToString());
                }

                if (Value is NewLineOperator)
                {
                    return string.Format("{0}{1} {2}", Children[0].ToString(), Value.Name, Children[1].ToString());
                }

                if (Value is IBinaryOperator)
                {
                    var parent = Value as IBinaryOperator;
                    return string.Format("{0} {1} {2}", ExpressionToStringWithParentheses(parent, Children[0], true), parent.Name, ExpressionToStringWithParentheses(parent, Children[1], false));
                }

                if (Value is IFunction)
                {
                    IFunction func = Value as IFunction;
                    return string.Format("{0}({1})", func.Name, Children.Count == 0 ? null : Children.Aggregate<IAstTreeNode, string>(string.Empty, (text, next) => string.Format("{0}{1}{2}", text, string.IsNullOrEmpty(text) ? string.Empty : ", ", next.ToString())));
                }

                throw new ArgumentException(string.Format("Unexpected element: ‘{0}’.", Value.Name));
            }

            private static string ExpressionToStringWithParentheses(IBinaryOperator parent, IAstTreeNode childNode, bool isLeftChild)
            {
                string text = childNode.ToString();

                var child = childNode.Value as IBinaryOperator;
                if (child == null || parent.Precedence < child.Precedence) return text;
                if (parent.Precedence == child.Precedence)
                {
                    if (!(parent.Associativity == OperatorAssociativity.RightAssociative || child.Associativity == OperatorAssociativity.RightAssociative))
                    {
                        if (isLeftChild) return text;
                        if (parent.Associativity == OperatorAssociativity.NonAssociative && child.Associativity == OperatorAssociativity.NonAssociative) return text;
                    }
                }

                return string.Format("({0})", text);
            }

            private static readonly string[] reservedWords = new NumericExpression().Statements.Select((s) => s.Name).Union(new NumericExpression().Functions.Select((f) => f.Name)).Distinct().ToArray();
            private static string RenameToSafeName(string variableName)
            {
                if (Array.IndexOf(reservedWords, variableName) >= 0)
                {
                    variableName = "@" + variableName;
                }
                return variableName;
            }
        }
    }
}
