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
    using System.Text;

    public static class ExpressionValidator
    {
        public static bool IsValid(string expression, out string error)
        {
            HashSet<string> dummy = new HashSet<string>();
            HashSet<string> dummy2;

            return IsValid(expression, VariableChecking.Standard, null, ref dummy, out dummy2, out error);
        }

        public static bool IsValid(string expression, List<IConstant> constants, ref HashSet<string> initializedVariables, out HashSet<string> uninitializedVariables, out string error)
        {
            return IsValid(expression, VariableChecking.Standard, constants, ref initializedVariables, out uninitializedVariables, out error);
        }

        public static bool IsValid(string expression, VariableChecking variableChecking, List<IConstant> constants, ref HashSet<string> initializedVariables, out HashSet<string> uninitializedVariables, out string error)
        {
            uninitializedVariables = new HashSet<string>();

            string _error = null;
            var ne = new NumericExpression();
            if (constants != null)
            {
                ne.Constants.AddRange(constants);
            }
            ne.UndefinedVariableFound += (s, e) => e.Handled = true; // variables are allowed
            ne.UndefinedFunctionFound += (s, e) => e.Handled = true; // functions are allowed
            try
            {
                var pe = ne.Parse(expression);
                TraverseTree(pe.ExpressionTree, initializedVariables, uninitializedVariables);
                if (variableChecking == VariableChecking.None)
                {
                    uninitializedVariables.Clear();
                }
                if (uninitializedVariables.Count > 0)
                {
                    error = "Uninitialized variables used in expression.";
                    return false;
                }
                else
                {
                    error = null;
                    return true;
                }
            }
            catch (ArgumentException e)
            {
                if (_error == null)
                {
                    _error = string.Format("Error parsing expression. {0}", e.Message);
                }
            }
            catch (Exception e)
            {
                _error = string.Format("Unexpected error while parsing expression. {0}", e.Message);
            }
            error = _error;
            return false;
        }

        private static void TraverseTree(IAstTreeNode node, HashSet<string> initializedVariables, HashSet<string> uninitializedVariables)
        {
            // variable is uses – is it initialized?
            if (node.Value is IVariable)
            {
                if (!initializedVariables.Contains(node.Value.Name))
                {
                    uninitializedVariables.Add(node.Value.Name);
                }
                return;
            }

            // only “=” operator
            if (node.Value.GetType() == typeof(AssignmentOperator))
            {
                TraverseTree(node.Children[1], initializedVariables, uninitializedVariables);
                initializedVariables.Add(node.Children[0].Value.Name);
                return;
            }

            // other “+=”-like operators
            if (node.Value is AssignmentOperator)
            {
                TraverseTree(node.Children[0], initializedVariables, uninitializedVariables);
                TraverseTree(node.Children[1], initializedVariables, uninitializedVariables);
                initializedVariables.Add(node.Children[0].Value.Name);
                return;
            }

            // other operators
            if (node.Value is IOperator)
            {
                foreach (var child in node.Children)
                {
                    TraverseTree(child, initializedVariables, uninitializedVariables);
                }
                return;
            }

            // “x ?? y” operator with “if (x, y)” function-like syntax
            if (node.Value is CoalesceStatement)
            {
                TraverseTree(node.Children[0], initializedVariables, uninitializedVariables);
                TraverseTree(node.Children[1], new HashSet<string>(initializedVariables), uninitializedVariables);
            }

            // “x ? y : z” operator with “if (x, y, z)” function-like syntax
            if (node.Value is IfStatement)
            {
                TraverseTree(node.Children[0], initializedVariables, uninitializedVariables);
                TraverseTree(node.Children[1], new HashSet<string>(initializedVariables), uninitializedVariables);
                TraverseTree(node.Children[2], new HashSet<string>(initializedVariables), uninitializedVariables);
            }

            // “while (x) y” statement with “while (x, y)” function-like syntax
            if (node.Value is WhileStatement)
            {
                TraverseTree(node.Children[0], initializedVariables, uninitializedVariables);
                TraverseTree(node.Children[1], new HashSet<string>(initializedVariables), uninitializedVariables);
            }

            // other node – ignored
            return;
        }
    }
}
