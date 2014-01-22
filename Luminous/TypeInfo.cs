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

namespace System
{
    using System.Reflection;
    using System.Linq.Expressions;

    public static class TypeInfo
    {
        public static FieldInfo GetField<TField>(Expression<Func<TField>> field)
        {
            var lambdaExpression = field as LambdaExpression;
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException("field");
            }
            var memberExpression = lambdaExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentNullException("field");
            }
            if (!(memberExpression.Member is FieldInfo))
            {
                throw new ArgumentNullException("field");
            }
            return (FieldInfo)memberExpression.Member;
        }

        public static FieldInfo GetField<TType, TField>(Expression<Func<TType, TField>> field)
        {
            var lambdaExpression = field as LambdaExpression;
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException("field");
            }
            var memberExpression = lambdaExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentNullException("field");
            }
            if (!(memberExpression.Member is FieldInfo))
            {
                throw new ArgumentNullException("field");
            }
            return (FieldInfo)memberExpression.Member;
        }

        public static PropertyInfo GetProperty<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambdaExpression = property as LambdaExpression;
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException("property");
            }
            var memberExpression = lambdaExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentNullException("property");
            }
            if (!(memberExpression.Member is PropertyInfo))
            {
                throw new ArgumentNullException("property");
            }
            return (PropertyInfo)memberExpression.Member;
        }

        public static PropertyInfo GetProperty<TType, TProperty>(Expression<Func<TType, TProperty>> property)
        {
            var lambdaExpression = property as LambdaExpression;
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException("property");
            }
            var memberExpression = lambdaExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentNullException("property");
            }
            if (!(memberExpression.Member is PropertyInfo))
            {
                throw new ArgumentNullException("property");
            }
            return (PropertyInfo)memberExpression.Member;
        }

        private static MethodInfo GetMethodInfo(Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
            {
                throw new ArgumentNullException("method");
            }
            MethodCallExpression methodExpr = null;
            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                methodExpr = ((UnaryExpression)lambda.Body).Operand as MethodCallExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.Call)
            {
                methodExpr = lambda.Body as MethodCallExpression;
            }
            else
            {
                throw new ArgumentException("method");
            }

            return methodExpr.Method;
        }

        public static MethodInfo GetMethod(Expression<Action> method)
        {
            return GetMethodInfo(method);
        }

        public static MethodInfo GetMethod<TType>(Expression<Action<TType>> method)
        {
            return GetMethodInfo(method);
        }

        public static MethodInfo GetMethod(Expression<Func<object>> method)
        {
            return GetMethodInfo(method);
        }

        public static MethodInfo GetMethod<TType>(Expression<Func<TType, object>> method)
        {
            return GetMethodInfo(method);
        }

        private static ParameterInfo GetParameterInfo(Expression method, int index)
        {
            return GetMethodInfo(method).GetParameters()[index];
        }

        public static ParameterInfo GetMethodParameter(Expression<Action> method, int index)
        {
            return GetParameterInfo(method, index);
        }

        public static ParameterInfo GetMethodParameter<TType>(Expression<Action<TType>> method, int index)
        {
            return GetParameterInfo(method, index);
        }

        public static ParameterInfo GetMethodParameter(Expression<Func<object>> method, int index)
        {
            return GetParameterInfo(method, index);
        }

        public static ParameterInfo GetMethodParameter<TType>(Expression<Func<TType, object>> method, int index)
        {
            return GetParameterInfo(method, index);
        }
    }
}
