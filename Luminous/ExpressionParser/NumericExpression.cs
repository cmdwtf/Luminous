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
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class NumericExpression
    {
        public readonly List<IConstant> Constants = new List<IConstant>();
        public readonly List<IVariable> Variables = new List<IVariable>();
        public readonly List<IOperator> Operators = new List<IOperator>();
        public readonly List<IFunction> Functions = new List<IFunction>();
        public readonly List<IStatement> Statements = new List<IStatement>();
        private const string NumberChars = "0123456789.";
        private string NonoperatorChars { get { return "0123456789()," + AdditionalVariableChars; } }
        protected string AdditionalVariableChars { get; set; }
        private const string ParseExceptionText = "Invalid {0} (position: {1}, value: ‘{2}’)";

        public bool ZeroOnError { get; set; }

        public NumericExpression() : this(ExpressionElements.All) { }

        public NumericExpression(ExpressionElements elementsToInclude)
        {
            AdditionalVariableChars = "_$";
            if ((elementsToInclude & ExpressionElements.ArithmeticOperators) != ExpressionElements.None)
            {
                Operators.Add(new UnaryPlusOperator());
                Operators.Add(new NumericNegationOperator());
                Operators.Add(new AdditionOperator());
                Operators.Add(new SubtractionOperator());
                Operators.Add(new MultiplicationOperator());
                Operators.Add(new DivisionOperator());
                Operators.Add(new FloorDivisionOperator());
                Operators.Add(new ModulusOperator());
                Operators.Add(new RoundingOperator());
                Operators.Add(new PowerOperator());
            }
            if ((elementsToInclude & ExpressionElements.LogicalOperators) != ExpressionElements.None)
            {
                Operators.Add(new LogicalNegationOperator());
                Operators.Add(new ConditionalOrOperator());
                Operators.Add(new ConditionalAndOperator());
            }
            if ((elementsToInclude & ExpressionElements.ComparisonOperators) != ExpressionElements.None)
            {
                Operators.Add(new EqualityOperator());
                Operators.Add(new InequalityOperator());
                Operators.Add(new LessThanOperator());
                Operators.Add(new LessThanOrEqualToOperator());
                Operators.Add(new GreaterThanOperator());
                Operators.Add(new GreaterThanOrEqualToOperator());
            }
            if ((elementsToInclude & ExpressionElements.AssignmentOperators) != ExpressionElements.None)
            {
                Operators.Add(new AssignmentOperator());
                if ((elementsToInclude & ExpressionElements.ArithmeticOperators) != ExpressionElements.None)
                {
                    Operators.Add(new AssignmentOperator<AdditionOperator>());
                    Operators.Add(new AssignmentOperator<SubtractionOperator>());
                    Operators.Add(new AssignmentOperator<MultiplicationOperator>());
                    Operators.Add(new AssignmentOperator<DivisionOperator>());
                    Operators.Add(new AssignmentOperator<FloorDivisionOperator>());
                    Operators.Add(new AssignmentOperator<ModulusOperator>());
                    Operators.Add(new AssignmentOperator<RoundingOperator>());
                    Operators.Add(new AssignmentOperator<PowerOperator>());
                }
                if ((elementsToInclude & ExpressionElements.LogicalOperators) != ExpressionElements.None)
                {
                    Operators.Add(new AssignmentOperator<ConditionalOrOperator>());
                    Operators.Add(new AssignmentOperator<ConditionalAndOperator>());
                }
            }
            if ((elementsToInclude & ExpressionElements.Constants) != ExpressionElements.None)
            {
                //Constants.Add(ConstantElement.PI);
                //Constants.Add(ConstantElement.E);
            }
            if ((elementsToInclude & ExpressionElements.MathFunctions) != ExpressionElements.None)
            {
                Functions.Add(new AbsFunction());
                Functions.Add(new CeilingFunction());
                Functions.Add(new FloorFunction());
                Functions.Add(new MaxFunction());
                Functions.Add(new MinFunction());
                Functions.Add(new PowFunction());
                //Functions.Add(new RandFunction());
                Functions.Add(new RoundFunction());
                Functions.Add(new SignFunction());
                Functions.Add(new TruncateFunction());
            }
            if ((elementsToInclude & ExpressionElements.Statements) != ExpressionElements.None)
            {
                Functions.Add(new CoalesceStatement());
                Functions.Add(new IfStatement());
                Statements.Add(new ForStatement());
                Statements.Add(new WhileStatement());
            }
            if ((elementsToInclude & ExpressionElements.NewLineOperator) != ExpressionElements.None)
            {
                Operators.Add(new NewLineOperator());
            }
        }

        public event EventHandler<UndefinedFunctionFoundEventArgs> UndefinedFunctionFound;
        public event EventHandler<UndefinedVariableFoundEventArgs> UndefinedVariableFound;

        public ParsedExpression Parse(string expression)
        {
            IList<IExpressionElement> tokens = Tokenize(expression);

            if (tokens.Count == 0)
            {
                throw new ArgumentException("Empty expression.");
            }

            List<IExpressionElement> output = new List<IExpressionElement>();
            Stack<IExpressionElement> stack = new Stack<IExpressionElement>();

            Stack<int> argumentsStack = new Stack<int>();
            Stack<bool> insideFunctionStack = new Stack<bool>();
            bool afterFunction = false;
            bool afterFnParenthesis = false;
            bool afterNonFnParenthesis = false;
            bool afterClosingParenthesis = false;
            bool afterSeparator = false;
            bool insideFunction = false;
            int arguments = 0;
            bool mustBeUnary = true;

            int tokenIndex = -1;
            foreach (IExpressionElement token in tokens)
            {
                tokenIndex++;
                IExpressionElement nextToken = tokenIndex + 1 < tokens.Count ? tokens[tokenIndex + 1] : null;
                // If the token is a number, then add it to the output queue.
                if (!afterFunction && !afterClosingParenthesis && (token is ILiteral || token is IConstant || token is IVariable))
                {
                    output.Add(token);
                    mustBeUnary = false;
                    afterClosingParenthesis = afterNonFnParenthesis = afterFnParenthesis = afterSeparator = false;
                }
                // If the token is a function token, then push it onto the stack.
                else if (!afterFunction && !afterClosingParenthesis && (token is IFunction || (token is MultipleElements && (token as MultipleElements).Elements[0] is IFunction)))
                {
                    stack.Push(token);
                    afterFunction = true;
                    afterClosingParenthesis = afterNonFnParenthesis = afterFnParenthesis = afterSeparator = false;
                }
                // If the token is a function argument separator (e.g., a comma):
                // • Until the topmost element of the stack is a left parenthesis, pop the element onto the output queue.
                //   If no left parentheses are encountered, either the separator was misplaced or parentheses were mismatched. 
                else if (!afterFunction && insideFunction && token == Symbol.FunctionArgumentSeparator)
                {
                    if (afterFnParenthesis)
                    {
                        throw new ArgumentException("Missing argument.");
                    }
                    while (stack.Peek() != Symbol.LeftParenthesis)
                    {
                        output.Add(stack.Pop());
                    }
                    if (stack.Peek() != Symbol.LeftParenthesis)
                    {
                        throw new ArgumentException("Either the separator is misplaced or parentheses are mismatched.");
                    }
                    arguments++;
                    afterClosingParenthesis = afterNonFnParenthesis = afterFnParenthesis = false;
                    afterSeparator = true;
                }
                // If the token is an operator, o1, then:
                // • while there is an operator, o2, at the top of the stack, and either
                //       o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                //       o1 is right-associative and its precedence is less than (lower precedence) that of o2,
                //     pop o2 off the stack, onto the output queue;
                // • push o1 onto the stack. 
                else if (!afterFunction && (token is IOperator || (token is MultipleElements && (token as MultipleElements).Elements[0] is IOperator)))
                {
                    IOperator op = null;
                    if ((mustBeUnary && token is IUnaryOperator) || (!mustBeUnary && token is IBinaryOperator))
                    {
                        op = token as IOperator;
                    }
                    else if (token is MultipleElements)
                    {
                        foreach (IExpressionElement elem in (token as MultipleElements).Elements)
                        {
                            if ((mustBeUnary && elem is IUnaryOperator) || (!mustBeUnary && elem is IBinaryOperator))
                            {
                                op = elem as IOperator;
                                break;
                            }
                        }
                    }
                    if (op == null)
                    {
                        throw new ArgumentException(string.Format("{0} is not {1} operator.", token.Name, mustBeUnary ? "an unary" : "a binary"));
                    }
                    IOperator sop = stack.Count > 0 ? stack.Peek() as IOperator : null;
                    while (sop != null && (
                        (
                            op is IBinaryOperator &&
                            (op as IBinaryOperator).Associativity != OperatorAssociativity.RightAssociative &&
                            op.Precedence <= sop.Precedence
                        )
                        ||
                        (
                            (
                                (op is IBinaryOperator && (op as IBinaryOperator).Associativity == OperatorAssociativity.RightAssociative) ||
                                op is IUnaryOperator
                            )
                            &&
                            op.Precedence < sop.Precedence
                        )))
                    {
                        output.Add(stack.Pop());
                        sop = stack.Count > 0 ? stack.Peek() as IOperator : null;
                    }

                    stack.Push(op);
                    mustBeUnary = true;
                    afterClosingParenthesis = afterFnParenthesis = afterNonFnParenthesis = afterSeparator = false;
                }
                // If the token is a left parenthesis, then push it onto the stack.
                else if (!afterClosingParenthesis && token == Symbol.LeftParenthesis)
                {
                    stack.Push(token);
                    argumentsStack.Push(arguments);
                    insideFunctionStack.Push(insideFunction);
                    // initial argument count
                    arguments = nextToken == Symbol.RightParenthesis ? 0 : 1;
                    if (afterFunction)
                    {
                        afterNonFnParenthesis = afterFunction = false;
                        insideFunction = afterFnParenthesis = true;
                    }
                    else
                    {
                        afterNonFnParenthesis = true;
                        insideFunction = afterFnParenthesis = false;
                    }
                    mustBeUnary = true;
                    afterClosingParenthesis = afterSeparator = false;
                }
                // If the token is a right parenthesis:
                // • Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue.
                // • Pop the left parenthesis from the stack, but not onto the output queue.
                // • If the token at the top of the stack is a function token, pop it and onto the output queue.
                // • If the stack runs out without finding a left parenthesis, then there are mismatched parentheses.
                else if (!afterFunction && !afterSeparator && token == Symbol.RightParenthesis)
                {
                    if (afterNonFnParenthesis)
                    {
                        throw new ArgumentException("Unexpected token: '()'.");
                    }
                    if (stack.Count == 0)
                    {
                        throw new ArgumentException("There are mismatched parentheses.");
                    }
                    while (stack.Peek() != Symbol.LeftParenthesis)
                    {
                        output.Add(stack.Pop());
                        if (stack.Count == 0)
                        {
                            throw new ArgumentException("There are mismatched parentheses.");
                        }
                    }
                    IExpressionElement parenthesis = stack.Pop(); // parenthesis
                    if (stack.Count > 0)
                    {
                        IExpressionElement elem = stack.Peek();
                        if (elem is IFunction || (elem is MultipleElements && (elem as MultipleElements).Elements[0] is IFunction))
                        {
                            elem = stack.Pop();
                            IExpressionElement func = null;
                            if (elem is IFunction)
                            {
                                if ((elem as IFunction).ParametersCount != arguments)
                                {
                                    UndefinedFunctionFoundEventArgs args = new UndefinedFunctionFoundEventArgs(elem.Name, arguments);
                                    if (UndefinedFunctionFound != null)
                                    {
                                        UndefinedFunctionFound(this, args);
                                    }
                                    if (!args.Handled)
                                    {
                                        throw new ArgumentException(string.Format("There is no function ‘{0}’ with {1} parameters defined.", elem.Name, arguments));
                                    }
                                    elem = new UnknownFunction(elem.Name);
                                    (elem as UnknownFunction).ParametersCount = arguments;
                                }
                                func = elem;
                            }
                            else
                            {
                                foreach (IFunction f in (elem as MultipleElements).Elements)
                                {
                                    if (f.ParametersCount == arguments)
                                    {
                                        func = f;
                                        break;
                                    }
                                }
                                if (func == null)
                                {
                                    UndefinedFunctionFoundEventArgs args = new UndefinedFunctionFoundEventArgs((elem as MultipleElements).Elements[0].Name, arguments);
                                    if (UndefinedFunctionFound != null)
                                    {
                                        UndefinedFunctionFound(this, args);
                                    }
                                    if (!args.Handled)
                                    {
                                        throw new ArgumentException(string.Format("There is no function ‘{0}’ with {1} parameters defined.", args.Name, arguments));
                                    }
                                    func = new UnknownFunction(args.Name);
                                    (func as UnknownFunction).ParametersCount = arguments;
                                }
                            }
                            output.Add(func);
                        }
                    }
                    arguments = argumentsStack.Count > 0 ? argumentsStack.Pop() : 0;
                    insideFunction = insideFunctionStack.Pop();
                    afterFnParenthesis = afterNonFnParenthesis = mustBeUnary = afterSeparator = false;
                    afterClosingParenthesis = true;
                }
                else
                {
                    IExpressionElement elem = token;
                    if (token is MultipleElements)
                    {
                        elem = (token as MultipleElements).Elements[0];
                    }
                    throw new ArgumentException(string.Format("Unexpected token: ‘{0}’", elem.Name));
                }
            }
            // When there are no more tokens to read:
            // • While there are still operator tokens in the stack:
            //   • If the operator token on the top of the stack is a parenthesis, then there are mismatched parentheses.
            //   • Pop the operator onto the output queue.
            while (stack.Count > 0)
            {
                if (stack.Peek() == Symbol.LeftParenthesis || stack.Peek() == Symbol.RightParenthesis)
                {
                    var r = stack.Peek();
                    throw new ArgumentException("There are mismatched parentheses.");
                }
                output.Add(stack.Pop());
            }
            return new ParsedExpression(output, ZeroOnError);
        }

        private IList<IExpressionElement> Tokenize(string expression)
        {
            Dictionary<string, List<IExpressionElement>> dict = new Dictionary<string, List<IExpressionElement>>();
            foreach (IExpressionElement element in Constants.Cast<IExpressionElement>().
                                            Concat(Variables.Cast<IExpressionElement>()).
                                            Concat(Operators.Cast<IExpressionElement>()).
                                            Concat(Functions.Cast<IExpressionElement>()).
                                            Concat(Statements.Cast<IExpressionElement>()))
            {
                string name = element.Name;
                if (!dict.ContainsKey(name))
                {
                    dict[name] = new List<IExpressionElement>();
                }
                if (dict[name].Count > 0)
                {
                    if (!(element is IOperator || element is IFunction))
                    {
                        throw new ArgumentException("Only operators and functions can be overloaded.");
                    }
                    IExpressionElement elem = dict[name][0];
                    if ((elem is IOperator && element is IFunction) ||
                        (elem is IFunction && element is IOperator))
                    {
                        throw new ArgumentException("Function and operator cannot have the same name.");
                    }
                    if (element is IOperator)
                    {
                        foreach (IExpressionElement eelem in dict[name])
                        {
                            if ((element is IUnaryOperator && eelem is IUnaryOperator) ||
                                (element is IBinaryOperator && eelem is IBinaryOperator))
                            {
                                throw new ArgumentException("There cannot be two identical operators.");
                            }
                        }
                    }
                    if (element is IFunction)
                    {
                        IFunction felement = element as IFunction;
                        foreach (IExpressionElement eelem in dict[name])
                        {
                            IFunction felem = eelem as IFunction;
                            if (felem != null && felem.Name == felement.Name && felem.ParametersCount == felement.ParametersCount)
                            {
                                throw new ArgumentException("There cannot be two identical functions.");
                            }
                        }
                    }
                }
                dict[name].Add(element);
            }

            List<IExpressionElement> tokens = new List<IExpressionElement>();
            int i = 0;
            int l = expression.Length;
            while (i < l)
            {
                char c = expression[i];
                if (char.IsWhiteSpace(c)) { /* eat */ }
                else if (NumberChars.IndexOf(c) >= 0)
                {
                    int pos = i;
                    StringBuilder token = new StringBuilder();
                    while (i < l && NumberChars.IndexOf(expression[i]) >= 0)
                    {
                        token.Append(expression[i]);
                        i++;
                    }
                    i--;
                    decimal value = 0;
                    string strtoken = token.ToString();
                    if (decimal.TryParse(strtoken, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    {
                        tokens.Add(new Literal(value));
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(ParseExceptionText, "number", pos, strtoken));
                    }
                }
                else if (c == '(')
                {
                    tokens.Add(Symbol.LeftParenthesis);
                }
                else if (c == ')')
                {
                    tokens.Add(Symbol.RightParenthesis);
                }
                else if (c == ',')
                {
                    tokens.Add(Symbol.FunctionArgumentSeparator);
                }
                else if (char.IsLetter(c) || AdditionalVariableChars.IndexOf(c) >= 0)
                {
                    StringBuilder token = new StringBuilder();
                    while (i < l && (char.IsLetterOrDigit(expression[i]) || AdditionalVariableChars.IndexOf(expression[i]) >= 0))
                    {
                        token.Append(expression[i]);
                        i++;
                    }
                    i--;
                    string strtoken = token.ToString();
                    if (dict.ContainsKey(strtoken))
                    {
                        List<IExpressionElement> elements = dict[strtoken];
                        IExpressionElement element;
                        if (elements.Count == 1)
                        {
                            element = elements[0];
                        }
                        else
                        {
                            element = new MultipleElements(elements);
                        }
                        tokens.Add(element);
                    }
                    else
                    {
                        int nx = i + 1;
                        while (nx < l && char.IsWhiteSpace(expression[nx]))
                        {
                            nx++;
                        }
                        IExpressionElement element = null;
                        if (nx < l && expression[nx] == '(')
                        {
                            element = new UnknownFunction(strtoken);
                        }
                        else
                        {
                            UndefinedVariableFoundEventArgs args = new UndefinedVariableFoundEventArgs(strtoken);
                            if (UndefinedVariableFound != null)
                            {
                                UndefinedVariableFound(this, args);
                            }
                            if (!args.Handled)
                            {
                                throw new ArgumentException(string.Format("There is no variable ‘{0}’ defined.", args.Name));
                            }
                            element = args.HandledVariable ?? new UnknownVariable(strtoken);
                            Variables.Add((IVariable)element);
                            if (!dict.ContainsKey(element.Name))
                            {
                                dict[element.Name] = new List<IExpressionElement>();
                            }
                            if (dict[element.Name].Count > 0 && !(element is IOperator || element is IFunction))
                            {
                                throw new ArgumentException("Only operators and functions can be overloaded.");
                            }
                            dict[element.Name].Add(element);
                        }
                        tokens.Add(element);
                    }
                }
                else if (char.IsSymbol(c) || char.IsPunctuation(c))
                {
                    int j = i;
                    StringBuilder token = new StringBuilder();
                    while (i < l && (char.IsSymbol(expression[i]) || char.IsPunctuation(expression[i])))
                    {
                        token.Append(expression[i]);
                        i++;
                    }
                    i--;
                    string strop = token.ToString();
                    while (!dict.ContainsKey(strop))
                    {
                        strop = strop.Substring(0, strop.Length - 1);
                        i--;
                        if (strop.Length == 0)
                        {
                            throw new ArgumentException(string.Format(ParseExceptionText, "operator", j, expression[j]));
                        }
                    }
                    if (dict.ContainsKey(strop))
                    {
                        List<IExpressionElement> ops = dict[strop];
                        IExpressionElement op;
                        if (ops.Count == 1)
                        {
                            op = ops[0];
                        }
                        else
                        {
                            op = new MultipleElements(ops);
                        }
                        tokens.Add(op);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(ParseExceptionText, "operator", j, strop));
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format(ParseExceptionText, "character", i, c));
                }
                i++;
            }
            return tokens.AsReadOnly();
        }
    }
}
