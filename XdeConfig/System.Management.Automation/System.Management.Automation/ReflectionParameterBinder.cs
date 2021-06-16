using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000081 RID: 129
	internal class ReflectionParameterBinder : ParameterBinderBase
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x0001FF80 File Offset: 0x0001E180
		internal ReflectionParameterBinder(object target, Cmdlet command) : base(target, command.MyInvocation, command.Context, command)
		{
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001FF96 File Offset: 0x0001E196
		internal ReflectionParameterBinder(object target, Cmdlet command, CommandLineParameters commandLineParameters) : base(target, command.MyInvocation, command.Context, command)
		{
			base.CommandLineParameters = commandLineParameters;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001FFB4 File Offset: 0x0001E1B4
		internal override object GetDefaultParameterValue(string name)
		{
			object result;
			try
			{
				result = ReflectionParameterBinder.GetGetter(base.Target.GetType(), name)(base.Target);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = ex.InnerException ?? ex;
				throw new GetValueInvocationException("CatchFromBaseAdapterGetValueTI", ex2, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
				{
					name,
					ex2.Message
				});
			}
			catch (GetValueException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new GetValueInvocationException("CatchFromBaseAdapterGetValue", ex3, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
				{
					name,
					ex3.Message
				});
			}
			return result;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00020074 File Offset: 0x0001E274
		internal override void BindParameter(string name, object value)
		{
			try
			{
				ReflectionParameterBinder.GetSetter(base.Target.GetType(), name)(base.Target, value);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = ex.InnerException ?? ex;
				throw new SetValueInvocationException("CatchFromBaseAdapterSetValueTI", ex2, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
				{
					name,
					ex2.Message
				});
			}
			catch (SetValueException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new SetValueInvocationException("CatchFromBaseAdapterSetValue", ex3, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
				{
					name,
					ex3.Message
				});
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00020198 File Offset: 0x0001E398
		private static Func<object, object> GetGetter(Type type, string property)
		{
			return ReflectionParameterBinder._getterMethods.GetOrAdd(Tuple.Create<Type, string>(type, property), delegate(Tuple<Type, string> _)
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
				return Expression.Lambda<Func<object, object>>(Expression.Convert(ReflectionParameterBinder.GetPropertyOrFieldExpr(type, property, Expression.Convert(parameterExpression, type)), typeof(object)), new ParameterExpression[]
				{
					parameterExpression
				}).Compile();
			});
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000202D0 File Offset: 0x0001E4D0
		private static Action<object, object> GetSetter(Type type, string property)
		{
			return ReflectionParameterBinder._setterMethods.GetOrAdd(Tuple.Create<Type, string>(type, property), delegate(Tuple<Type, string> _)
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
				ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
				Expression propertyOrFieldExpr = ReflectionParameterBinder.GetPropertyOrFieldExpr(type, property, Expression.Convert(parameterExpression, type));
				Expression expression = Expression.Assign(propertyOrFieldExpr, Expression.Convert(parameterExpression2, propertyOrFieldExpr.Type));
				if (propertyOrFieldExpr.Type.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(propertyOrFieldExpr.Type) == null)
				{
					MethodCallExpression expression2 = Expression.Call(CachedReflectionInfo.LanguagePrimitives_ThrowInvalidCastException, ExpressionCache.NullConstant, Expression.Constant(propertyOrFieldExpr.Type, typeof(Type)));
					expression = Expression.Condition(Expression.Equal(parameterExpression2, ExpressionCache.NullConstant), Expression.Convert(expression2, propertyOrFieldExpr.Type), expression);
				}
				return Expression.Lambda<Action<object, object>>(expression, new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile();
			});
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00020318 File Offset: 0x0001E518
		private static Expression GetPropertyOrFieldExpr(Type type, string name, Expression target)
		{
			try
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (property != null)
				{
					return Expression.Property(target, property);
				}
			}
			catch (AmbiguousMatchException)
			{
				foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
				{
					if (propertyInfo.Name.Equals(name, StringComparison.Ordinal))
					{
						return Expression.Property(target, propertyInfo);
					}
				}
			}
			try
			{
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (field != null)
				{
					return Expression.Field(target, field);
				}
			}
			catch (AmbiguousMatchException)
			{
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
				{
					if (fieldInfo.Name.Equals(name, StringComparison.Ordinal))
					{
						return Expression.Field(target, fieldInfo);
					}
				}
			}
			throw PSTraceSource.NewInvalidOperationException();
		}

		// Token: 0x040002BD RID: 701
		private static readonly ConcurrentDictionary<Tuple<Type, string>, Func<object, object>> _getterMethods = new ConcurrentDictionary<Tuple<Type, string>, Func<object, object>>();

		// Token: 0x040002BE RID: 702
		private static readonly ConcurrentDictionary<Tuple<Type, string>, Action<object, object>> _setterMethods = new ConcurrentDictionary<Tuple<Type, string>, Action<object, object>>();
	}
}
