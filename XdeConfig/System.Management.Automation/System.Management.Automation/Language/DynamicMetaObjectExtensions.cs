using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000602 RID: 1538
	internal static class DynamicMetaObjectExtensions
	{
		// Token: 0x06004328 RID: 17192 RVA: 0x00161183 File Offset: 0x0015F383
		internal static DynamicMetaObject WriteToDebugLog(this DynamicMetaObject obj, DynamicMetaObjectBinder binder)
		{
			return obj;
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x00161186 File Offset: 0x0015F386
		internal static BindingRestrictions GetSimpleTypeRestriction(this DynamicMetaObject obj)
		{
			if (obj.Value == null || ClrFacade.IsTransparentProxy(obj.Value))
			{
				return BindingRestrictions.GetInstanceRestriction(obj.Expression, obj.Value);
			}
			return BindingRestrictions.GetTypeRestriction(obj.Expression, obj.Value.GetType());
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x001611C8 File Offset: 0x0015F3C8
		internal static BindingRestrictions PSGetMethodArgumentRestriction(this DynamicMetaObject obj)
		{
			object obj2 = PSObject.Base(obj.Value);
			if (obj2 != null && obj2.GetType() == typeof(object[]))
			{
				Type type = Adapter.EffectiveArgumentType(obj.Value);
				MethodInfo method = (!(type == typeof(object[]))) ? CachedReflectionInfo.PSInvokeMemberBinder_IsHomogenousArray.MakeGenericMethod(new Type[]
				{
					type.GetElementType()
				}) : CachedReflectionInfo.PSInvokeMemberBinder_IsHeterogeneousArray;
				BindingRestrictions typeRestriction;
				Expression expression;
				if (obj.Value != obj2)
				{
					typeRestriction = BindingRestrictions.GetTypeRestriction(obj.Expression, typeof(PSObject));
					ParameterExpression parameterExpression = Expression.Variable(typeof(object[]));
					expression = Expression.Block(new ParameterExpression[]
					{
						parameterExpression
					}, new Expression[]
					{
						Expression.Assign(parameterExpression, Expression.TypeAs(Expression.Call(CachedReflectionInfo.PSObject_Base, obj.Expression), typeof(object[]))),
						Expression.AndAlso(Expression.NotEqual(parameterExpression, ExpressionCache.NullObjectArray), Expression.Call(method, parameterExpression))
					});
				}
				else
				{
					typeRestriction = BindingRestrictions.GetTypeRestriction(obj.Expression, typeof(object[]));
					Expression arg = obj.Expression.Cast(typeof(object[]));
					expression = Expression.Call(method, arg);
				}
				return typeRestriction.Merge(BindingRestrictions.GetExpressionRestriction(expression));
			}
			return obj.PSGetTypeRestriction();
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00161330 File Offset: 0x0015F530
		internal static BindingRestrictions PSGetStaticMemberRestriction(this DynamicMetaObject obj)
		{
			if (obj.Restrictions != BindingRestrictions.Empty)
			{
				return obj.Restrictions;
			}
			if (obj.Value == null || ClrFacade.IsTransparentProxy(obj.Value))
			{
				return BindingRestrictions.GetInstanceRestriction(obj.Expression, obj.Value);
			}
			object obj2 = PSObject.Base(obj.Value);
			if (obj2 == null)
			{
				return BindingRestrictions.GetExpressionRestriction(Expression.Equal(obj.Expression, Expression.Constant(AutomationNull.Value)));
			}
			BindingRestrictions bindingRestrictions;
			if (obj2 is Type)
			{
				if (obj.Value == obj2)
				{
					bindingRestrictions = BindingRestrictions.GetInstanceRestriction(obj.Expression, obj.Value);
				}
				else
				{
					bindingRestrictions = BindingRestrictions.GetTypeRestriction(obj.Expression, obj.LimitType);
					bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetInstanceRestriction(Expression.Call(CachedReflectionInfo.PSObject_Base, obj.Expression), obj2));
				}
			}
			else if (obj.Value != obj2)
			{
				bindingRestrictions = BindingRestrictions.GetTypeRestriction(Expression.Call(CachedReflectionInfo.PSObject_Base, obj.Expression), obj2.GetType());
			}
			else
			{
				bindingRestrictions = BindingRestrictions.GetTypeRestriction(obj.Expression, obj.LimitType);
			}
			return bindingRestrictions;
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00161434 File Offset: 0x0015F634
		internal static BindingRestrictions PSGetTypeRestriction(this DynamicMetaObject obj)
		{
			if (obj.Restrictions != BindingRestrictions.Empty)
			{
				return obj.Restrictions;
			}
			if (obj.Value == null || ClrFacade.IsTransparentProxy(obj.Value))
			{
				return BindingRestrictions.GetInstanceRestriction(obj.Expression, obj.Value);
			}
			object obj2 = PSObject.Base(obj.Value);
			if (obj2 == null)
			{
				return BindingRestrictions.GetExpressionRestriction(Expression.Equal(obj.Expression, Expression.Constant(AutomationNull.Value)));
			}
			BindingRestrictions bindingRestrictions = BindingRestrictions.GetTypeRestriction(obj.Expression, obj.LimitType);
			if (obj.Value != obj2)
			{
				bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetTypeRestriction(Expression.Call(CachedReflectionInfo.PSObject_Base, obj.Expression), obj2.GetType()));
			}
			else if (obj2 is PSObject)
			{
				bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Call(CachedReflectionInfo.PSObject_Base, obj.Expression), obj.Expression)));
			}
			return bindingRestrictions;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00161518 File Offset: 0x0015F718
		internal static BindingRestrictions CombineRestrictions(this DynamicMetaObject target, params DynamicMetaObject[] args)
		{
			BindingRestrictions bindingRestrictions = (target.Restrictions == BindingRestrictions.Empty) ? target.PSGetTypeRestriction() : target.Restrictions;
			foreach (DynamicMetaObject dynamicMetaObject in args)
			{
				bindingRestrictions = bindingRestrictions.Merge((dynamicMetaObject.Restrictions == BindingRestrictions.Empty) ? dynamicMetaObject.PSGetTypeRestriction() : dynamicMetaObject.Restrictions);
			}
			return bindingRestrictions;
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x00161578 File Offset: 0x0015F778
		internal static Expression CastOrConvertMethodArgument(this DynamicMetaObject target, Type parameterType, string parameterName, string methodName, List<ParameterExpression> temps, List<Expression> initTemps)
		{
			if (target.Value == AutomationNull.Value)
			{
				return Expression.Constant(null, parameterType);
			}
			Type limitType = target.LimitType;
			if (parameterType == typeof(object) && limitType == typeof(PSObject))
			{
				return Expression.Call(CachedReflectionInfo.PSObject_Base, target.Expression.Cast(typeof(PSObject)));
			}
			if (parameterType.IsAssignableFrom(limitType))
			{
				return target.Expression.Cast(parameterType);
			}
			ParameterExpression parameterExpression = Expression.Variable(typeof(Exception));
			ParameterExpression parameterExpression2 = Expression.Variable(target.Expression.Type);
			bool debase;
			LanguagePrimitives.ConversionData conversion = LanguagePrimitives.FigureConversion(target.Value, parameterType, out debase);
			Expression expression = PSConvertBinder.InvokeConverter(conversion, parameterExpression2, parameterType, debase, ExpressionCache.InvariantCulture);
			BlockExpression blockExpression = Expression.Block(new ParameterExpression[]
			{
				parameterExpression2
			}, new Expression[]
			{
				Expression.TryCatch(Expression.Block(Expression.Assign(parameterExpression2, target.Expression), expression), new CatchBlock[]
				{
					Expression.Catch(parameterExpression, Expression.Block(Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_ConvertToArgumentConversionException, parameterExpression, Expression.Constant(parameterName), parameterExpression2.Cast(typeof(object)), Expression.Constant(methodName), Expression.Constant(parameterType, typeof(Type))), Expression.Default(expression.Type)))
				})
			});
			ParameterExpression parameterExpression3 = Expression.Variable(blockExpression.Type);
			temps.Add(parameterExpression3);
			initTemps.Add(Expression.Assign(parameterExpression3, blockExpression));
			return parameterExpression3;
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00161700 File Offset: 0x0015F900
		internal static Expression CastOrConvert(this DynamicMetaObject target, Type type)
		{
			if (target.LimitType == type)
			{
				return target.Expression.Cast(type);
			}
			bool debase;
			LanguagePrimitives.ConversionData conversion = LanguagePrimitives.FigureConversion(target.Value, type, out debase);
			return PSConvertBinder.InvokeConverter(conversion, target.Expression, type, debase, ExpressionCache.InvariantCulture);
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x0016174A File Offset: 0x0015F94A
		internal static DynamicMetaObject ThrowRuntimeError(this DynamicMetaObject target, DynamicMetaObject[] args, BindingRestrictions moreTests, string errorID, string resourceString, params Expression[] exceptionArgs)
		{
			return new DynamicMetaObject(Compiler.ThrowRuntimeError(errorID, resourceString, exceptionArgs), target.CombineRestrictions(args).Merge(moreTests));
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00161768 File Offset: 0x0015F968
		internal static DynamicMetaObject ThrowRuntimeError(this DynamicMetaObject target, BindingRestrictions bindingRestrictions, string errorID, string resourceString, params Expression[] exceptionArgs)
		{
			return new DynamicMetaObject(Compiler.ThrowRuntimeError(errorID, resourceString, exceptionArgs), bindingRestrictions);
		}

		// Token: 0x0400218A RID: 8586
		internal static readonly DynamicMetaObject FakeError = new DynamicMetaObject(ExpressionCache.NullConstant, BindingRestrictions.Empty);
	}
}
