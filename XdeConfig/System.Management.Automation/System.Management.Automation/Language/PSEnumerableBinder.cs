using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Xml;

namespace System.Management.Automation.Language
{
	// Token: 0x02000605 RID: 1541
	internal class PSEnumerableBinder : ConvertBinder
	{
		// Token: 0x0600433A RID: 17210 RVA: 0x001619CE File Offset: 0x0015FBCE
		internal static PSEnumerableBinder Get()
		{
			return PSEnumerableBinder._binder;
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x001619D5 File Offset: 0x0015FBD5
		private PSEnumerableBinder() : base(typeof(IEnumerator), false)
		{
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x001619E8 File Offset: 0x0015FBE8
		public override string ToString()
		{
			return "ToEnumerable";
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x001619EF File Offset: 0x0015FBEF
		internal static BindingRestrictions GetRestrictions(DynamicMetaObject target)
		{
			if (!(target.Value is PSObject))
			{
				return target.PSGetTypeRestriction();
			}
			return BindingRestrictions.GetTypeRestriction(target.Expression, target.Value.GetType());
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x00161A22 File Offset: 0x0015FC22
		private DynamicMetaObject NullResult(DynamicMetaObject target)
		{
			return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, (Expression e) => ExpressionCache.NullEnumerator, target), PSEnumerableBinder.GetRestrictions(target));
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00161A54 File Offset: 0x0015FC54
		internal static Expression MaybeDebase(DynamicMetaObjectBinder binder, Func<Expression, Expression> generator, DynamicMetaObject target)
		{
			if (!(target.Value is PSObject))
			{
				return generator(target.Expression);
			}
			object obj = PSObject.Base(target.Value);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "value");
			return Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, Expression.Call(CachedReflectionInfo.PSObject_Base, target.Expression)),
				Expression.Condition((obj == null) ? Expression.AndAlso(Expression.Equal(parameterExpression, ExpressionCache.NullConstant), Expression.Not(Expression.Equal(target.Expression, ExpressionCache.AutomationNullConstant))) : Expression.TypeEqual(parameterExpression, obj.GetType()), generator(parameterExpression), binder.GetUpdateExpression(binder.ReturnType))
			});
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x00161C80 File Offset: 0x0015FE80
		public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]).WriteToDebugLog(this);
			}
			if (target.Value == AutomationNull.Value)
			{
				return new DynamicMetaObject(Expression.Call(Expression.Constant(PSEnumerableBinder.EmptyArray), typeof(Array).GetMethod("GetEnumerator")), BindingRestrictions.GetInstanceRestriction(target.Expression, AutomationNull.Value)).WriteToDebugLog(this);
			}
			object obj = PSObject.Base(target.Value);
			if (obj == null || obj is string || obj is PSObject)
			{
				return (errorSuggestion ?? this.NullResult(target)).WriteToDebugLog(this);
			}
			if (obj.GetType().IsArray)
			{
				return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, (Expression e) => Expression.Call(Expression.Convert(e, typeof(Array)), typeof(Array).GetMethod("GetEnumerator")), target), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
			}
			if (obj is IDictionary || obj is XmlNode)
			{
				return (errorSuggestion ?? this.NullResult(target)).WriteToDebugLog(this);
			}
			if (obj is DataTable)
			{
				return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, delegate(Expression e)
				{
					ParameterExpression parameterExpression = Expression.Parameter(typeof(DataTable), "table");
					ParameterExpression parameterExpression2 = Expression.Parameter(typeof(DataRowCollection), "rows");
					return Expression.Block(new ParameterExpression[]
					{
						parameterExpression,
						parameterExpression2
					}, new Expression[]
					{
						Expression.Assign(parameterExpression, e.Cast(typeof(DataTable))),
						Expression.Condition(Expression.NotEqual(Expression.Assign(parameterExpression2, Expression.Property(parameterExpression, "Rows")), ExpressionCache.NullConstant), Expression.Call(parameterExpression2, typeof(DataRowCollection).GetMethod("GetEnumerator")), ExpressionCache.NullEnumerator)
					});
				}, target), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
			}
			if (PSEnumerableBinder.IsComObject(obj))
			{
				return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, (Expression e) => Expression.Call(CachedReflectionInfo.EnumerableOps_GetCOMEnumerator, e), target), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
			}
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				Type[] interfaces = obj.GetType().GetInterfaces();
				for (int j = 0; j < interfaces.Length; j++)
				{
					Type i = interfaces[j];
					if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					{
						return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, (Expression e) => Expression.Call(CachedReflectionInfo.EnumerableOps_GetGenericEnumerator.MakeGenericMethod(new Type[]
						{
							i.GetGenericArguments()[0]
						}), Expression.Convert(e, i)), target), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
					}
				}
				return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, (Expression e) => Expression.Call(CachedReflectionInfo.EnumerableOps_GetEnumerator, Expression.Convert(e, typeof(IEnumerable))), target), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
			}
			IEnumerator enumerator = obj as IEnumerator;
			if (enumerator != null)
			{
				return new DynamicMetaObject(PSEnumerableBinder.MaybeDebase(this, (Expression e) => e.Cast(typeof(IEnumerator)), target), PSEnumerableBinder.GetRestrictions(target)).WriteToDebugLog(this);
			}
			return (errorSuggestion ?? this.NullResult(target)).WriteToDebugLog(this);
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x00161F34 File Offset: 0x00160134
		internal static bool IsStaticTypePossiblyEnumerable(Type type)
		{
			return type == typeof(object) || type == typeof(PSObject) || type.IsArray || (!(type == typeof(string)) && !typeof(IDictionary).IsAssignableFrom(type) && !typeof(XmlNode).IsAssignableFrom(type) && (!type.GetTypeInfo().IsSealed || typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerator).IsAssignableFrom(type)));
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x00161FDC File Offset: 0x001601DC
		internal static DynamicMetaObject IsEnumerable(DynamicMetaObject target)
		{
			PSEnumerableBinder psenumerableBinder = PSEnumerableBinder.Get();
			DynamicMetaObject dynamicMetaObject = psenumerableBinder.FallbackConvert(target, DynamicMetaObjectExtensions.FakeError);
			if (dynamicMetaObject != DynamicMetaObjectExtensions.FakeError)
			{
				return dynamicMetaObject;
			}
			return null;
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00162007 File Offset: 0x00160207
		internal static bool IsComObject(object obj)
		{
			return obj != null && PSEnumerableBinder.ComObjectTypeInfo.IsAssignableFrom(obj.GetType().GetTypeInfo());
		}

		// Token: 0x0400218C RID: 8588
		private static readonly PSEnumerableBinder _binder = new PSEnumerableBinder();

		// Token: 0x0400218D RID: 8589
		private static readonly object[] EmptyArray = new object[0];

		// Token: 0x0400218E RID: 8590
		private static readonly TypeInfo ComObjectTypeInfo = typeof(object).GetTypeInfo().Assembly.GetType("System.__ComObject").GetTypeInfo();
	}
}
