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

namespace System
{
	using System.Linq.Expressions;
	using System.Reflection;

	[Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Readability.")]
	public static class TypeInfo
	{
		public static FieldInfo GetField<TField>(Expression<Func<TField>> field)
		{
			if (field is not LambdaExpression lambdaExpression)
			{
				throw new ArgumentNullException(nameof(field));
			}
			if (lambdaExpression.Body is not MemberExpression memberExpression)
			{
				throw new ArgumentNullException(nameof(field));
			}
			return memberExpression.Member is not FieldInfo ? throw new ArgumentNullException(nameof(field)) : (FieldInfo)memberExpression.Member;
		}

		public static FieldInfo GetField<TType, TField>(Expression<Func<TType, TField>> field)
		{
			if (field is not LambdaExpression lambdaExpression)
			{
				throw new ArgumentNullException(nameof(field));
			}
			if (lambdaExpression.Body is not MemberExpression memberExpression)
			{
				throw new ArgumentNullException(nameof(field));
			}
			return memberExpression.Member is not FieldInfo ? throw new ArgumentNullException(nameof(field)) : (FieldInfo)memberExpression.Member;
		}

		public static PropertyInfo GetProperty<TProperty>(Expression<Func<TProperty>> property)
		{
			if (property is not LambdaExpression lambdaExpression)
			{
				throw new ArgumentNullException(nameof(property));
			}
			if (lambdaExpression.Body is not MemberExpression memberExpression)
			{
				throw new ArgumentNullException(nameof(property));
			}
			return memberExpression.Member is not PropertyInfo ? throw new ArgumentNullException(nameof(property))
				: (PropertyInfo)memberExpression.Member;
		}

		public static PropertyInfo GetProperty<TType, TProperty>(Expression<Func<TType, TProperty>> property)
		{
			if (property is not LambdaExpression lambdaExpression)
			{
				throw new ArgumentNullException(nameof(property));
			}
			if (lambdaExpression.Body is not MemberExpression memberExpression)
			{
				throw new ArgumentNullException(nameof(property));
			}
			return memberExpression.Member is not PropertyInfo ? throw new ArgumentNullException(nameof(property))
				: (PropertyInfo)memberExpression.Member;
		}

		private static MethodInfo GetMethodInfo(Expression method)
		{
			if (method is not LambdaExpression lambda)
			{
				throw new ArgumentNullException(nameof(method));
			}

			MethodCallExpression methodExpr = lambda.Body.NodeType == ExpressionType.Convert
				? ((UnaryExpression)lambda.Body).Operand as MethodCallExpression
				: lambda.Body.NodeType == ExpressionType.Call
					? lambda.Body as MethodCallExpression
					: throw new ArgumentException(null, nameof(method));

			return methodExpr.Method;
		}

		public static MethodInfo GetMethod(Expression<Action> method) => GetMethodInfo(method);

		public static MethodInfo GetMethod<TType>(Expression<Action<TType>> method) => GetMethodInfo(method);

		public static MethodInfo GetMethod(Expression<Func<object>> method) => GetMethodInfo(method);

		public static MethodInfo GetMethod<TType>(Expression<Func<TType, object>> method) => GetMethodInfo(method);

		private static ParameterInfo GetParameterInfo(Expression method, int index) => GetMethodInfo(method).GetParameters()[index];

		public static ParameterInfo GetMethodParameter(Expression<Action> method, int index) => GetParameterInfo(method, index);

		public static ParameterInfo GetMethodParameter<TType>(Expression<Action<TType>> method, int index) => GetParameterInfo(method, index);

		public static ParameterInfo GetMethodParameter(Expression<Func<object>> method, int index) => GetParameterInfo(method, index);

		public static ParameterInfo GetMethodParameter<TType>(Expression<Func<TType, object>> method, int index) => GetParameterInfo(method, index);
	}
}
