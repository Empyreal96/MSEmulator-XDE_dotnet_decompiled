using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.ComInterop;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000619 RID: 1561
	internal class PSSetIndexBinder : SetIndexBinder
	{
		// Token: 0x060043F0 RID: 17392 RVA: 0x0016840C File Offset: 0x0016660C
		public static PSSetIndexBinder Get(int argCount, PSMethodInvocationConstraints constraints = null)
		{
			PSSetIndexBinder result;
			lock (PSSetIndexBinder._binderCache)
			{
				Tuple<CallInfo, PSMethodInvocationConstraints> tuple = Tuple.Create<CallInfo, PSMethodInvocationConstraints>(new CallInfo(argCount, new string[0]), constraints);
				PSSetIndexBinder pssetIndexBinder;
				if (!PSSetIndexBinder._binderCache.TryGetValue(tuple, out pssetIndexBinder))
				{
					pssetIndexBinder = new PSSetIndexBinder(tuple);
					PSSetIndexBinder._binderCache.Add(tuple, pssetIndexBinder);
				}
				result = pssetIndexBinder;
			}
			return result;
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x00168480 File Offset: 0x00166680
		private PSSetIndexBinder(Tuple<CallInfo, PSMethodInvocationConstraints> tuple) : base(tuple.Item1)
		{
			this._constraints = tuple.Item2;
			this._version = 0;
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x001684A4 File Offset: 0x001666A4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSSetIndexBinder indexCnt={0}{1} ver:{2}", new object[]
			{
				base.CallInfo.ArgumentCount,
				(this._constraints == null) ? "" : (" constraints: " + this._constraints),
				this._version
			});
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0016850C File Offset: 0x0016670C
		internal static void InvalidateCache()
		{
			lock (PSSetIndexBinder._binderCache)
			{
				foreach (PSSetIndexBinder pssetIndexBinder in PSSetIndexBinder._binderCache.Values)
				{
					pssetIndexBinder._version++;
				}
			}
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x001685C8 File Offset: 0x001667C8
		public override DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			if (target.HasValue)
			{
				if (!indexes.Any((DynamicMetaObject mo) => !mo.HasValue) && value.HasValue)
				{
					if (!(target.Value is PSObject) || PSObject.Base(target.Value) == target.Value)
					{
						if (!indexes.Any((DynamicMetaObject mo) => mo.Value is PSObject && PSObject.Base(mo.Value) != mo.Value))
						{
							DynamicMetaObject binder;
							if (ComBinder.TryBindSetIndex(this, target, indexes, value, out binder))
							{
								return binder.UpdateComRestrictionsForPsObject(indexes).WriteToDebugLog(this);
							}
							if (target.Value == null)
							{
								return (errorSuggestion ?? target.ThrowRuntimeError(indexes, BindingRestrictions.Empty, "NullArray", ParserStrings.NullArray, new Expression[0])).WriteToDebugLog(this);
							}
							if (indexes.Length == 1 && indexes[0].Value == null)
							{
								return errorSuggestion ?? target.ThrowRuntimeError(indexes, BindingRestrictions.Empty, "NullArrayIndex", ParserStrings.NullArrayIndex, new Expression[0]).WriteToDebugLog(this);
							}
							if (target.LimitType.IsArray)
							{
								return this.SetIndexArray(target, indexes, value, errorSuggestion).WriteToDebugLog(this);
							}
							DefaultMemberAttribute defaultMemberAttribute = target.LimitType.GetCustomAttributes(true).FirstOrDefault<DefaultMemberAttribute>();
							if (defaultMemberAttribute != null)
							{
								return this.InvokeIndexer(target, indexes, value, errorSuggestion, defaultMemberAttribute.MemberName).WriteToDebugLog(this);
							}
							return errorSuggestion ?? this.CannotIndexTarget(target, indexes, value).WriteToDebugLog(this);
						}
					}
					return this.DeferForPSObject(indexes.Prepend(target).Append(value).ToArray<DynamicMetaObject>()).WriteToDebugLog(this);
				}
			}
			return base.Defer(indexes.Prepend(target).Append(value).ToArray<DynamicMetaObject>()).WriteToDebugLog(this);
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x00168774 File Offset: 0x00166974
		private DynamicMetaObject CannotIndexTarget(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			BindingRestrictions bindingRestrictions = value.PSGetTypeRestriction();
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetVersionCheck(this, this._version));
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
			return target.ThrowRuntimeError(indexes, bindingRestrictions, "CannotIndex", ParserStrings.CannotIndex, new Expression[]
			{
				Expression.Constant(target.LimitType, typeof(Type))
			});
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x001687F4 File Offset: 0x001669F4
		private DynamicMetaObject InvokeIndexer(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion, string methodName)
		{
			MethodInfo setter = PSInvokeMemberBinder.FindBestMethod(target, indexes.Append(value), "set_" + methodName, false, this._constraints);
			if (setter == null)
			{
				return errorSuggestion ?? this.CannotIndexTarget(target, indexes, value);
			}
			ParameterInfo[] parameters = setter.GetParameters();
			if (parameters.Length != indexes.Length + 1)
			{
				return errorSuggestion ?? this.CannotIndexTarget(target, indexes, value);
			}
			Expression[] array = new Expression[parameters.Length];
			for (int j = 0; j < parameters.Length; j++)
			{
				Type parameterType = parameters[j].ParameterType;
				array[j] = PSGetIndexBinder.ConvertIndex((j == parameters.Length - 1) ? value : indexes[j], parameterType);
				if (array[j] == null)
				{
					return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(target, parameterType, this, this._version, indexes.Append(value).ToArray<DynamicMetaObject>());
				}
			}
			if (parameters.Length == 2 && parameters[0].ParameterType == typeof(int) && !(target.Value is IDictionary))
			{
				PropertyInfo propertyInfo = target.LimitType.GetProperty("Length") ?? target.LimitType.GetProperty("Count");
				if (propertyInfo != null)
				{
					return this.IndexWithNegativeChecks(new DynamicMetaObject(target.Expression.Cast(target.LimitType), target.PSGetTypeRestriction()), new DynamicMetaObject(array[0], indexes[0].PSGetTypeRestriction()), new DynamicMetaObject(array[1], value.PSGetTypeRestriction()), propertyInfo, (Expression t, Expression i, Expression v) => Expression.Call(t, setter, i, v));
				}
			}
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(indexes).Merge(value.PSGetTypeRestriction());
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetVersionCheck(this, this._version));
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
			Expression expression = array[array.Length - 1];
			ParameterExpression parameterExpression = Expression.Parameter(expression.Type, "value");
			array[array.Length - 1] = parameterExpression;
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, expression),
				Expression.Call(target.Expression.Cast(setter.DeclaringType), setter, array),
				parameterExpression.Cast(typeof(object))
			}), bindingRestrictions);
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00168A60 File Offset: 0x00166C60
		private DynamicMetaObject IndexWithNegativeChecks(DynamicMetaObject target, DynamicMetaObject index, DynamicMetaObject value, PropertyInfo lengthProperty, Func<Expression, Expression, Expression, Expression> generateIndexOperation)
		{
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(new DynamicMetaObject[]
			{
				index
			}).Merge(value.Restrictions);
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(this, target.LimitType, this._version));
			ParameterExpression parameterExpression = Expression.Parameter(target.LimitType, "target");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(int), "len");
			Expression expression = value.Expression;
			ParameterExpression parameterExpression3 = Expression.Parameter(expression.Type, "value");
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(int), "index");
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression3,
				parameterExpression2,
				parameterExpression4
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, target.Expression.Cast(target.LimitType)),
				Expression.Assign(parameterExpression3, expression),
				Expression.Assign(parameterExpression2, Expression.Property(parameterExpression, lengthProperty)),
				Expression.Assign(parameterExpression4, index.Expression),
				Expression.IfThen(Expression.LessThan(parameterExpression4, ExpressionCache.Constant(0)), Expression.Assign(parameterExpression4, Expression.Add(parameterExpression4, parameterExpression2))),
				generateIndexOperation(parameterExpression, parameterExpression4, parameterExpression3),
				parameterExpression3.Cast(typeof(object))
			}), bindingRestrictions);
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x00168C04 File Offset: 0x00166E04
		private DynamicMetaObject SetIndexArray(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			Array array = (Array)target.Value;
			if (array.Rank > 1)
			{
				return this.SetIndexMultiDimensionArray(target, indexes, value, errorSuggestion);
			}
			if (indexes.Length > 1)
			{
				DynamicMetaObject result = errorSuggestion;
				if (errorSuggestion == null)
				{
					BindingRestrictions moreTests = value.PSGetTypeRestriction();
					string errorID = "ArraySliceAssignmentFailed";
					string arraySliceAssignmentFailed = ParserStrings.ArraySliceAssignmentFailed;
					Expression[] array2 = new Expression[1];
					array2[0] = Expression.Call(CachedReflectionInfo.ArrayOps_IndexStringMessage, Expression.NewArrayInit(typeof(object), from i in indexes
					select i.Expression.Cast(typeof(object))));
					result = target.ThrowRuntimeError(indexes, moreTests, errorID, arraySliceAssignmentFailed, array2);
				}
				return result;
			}
			Expression expression = PSGetIndexBinder.ConvertIndex(indexes[0], typeof(int));
			if (expression == null)
			{
				return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(indexes[0], typeof(int), this, this._version, new DynamicMetaObject[]
				{
					target,
					value
				});
			}
			Type elementType = target.LimitType.GetElementType();
			Expression expression2 = PSGetIndexBinder.ConvertIndex(value, elementType);
			if (expression2 == null)
			{
				return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(value, elementType, this, this._version, indexes.Prepend(target).ToArray<DynamicMetaObject>());
			}
			return this.IndexWithNegativeChecks(new DynamicMetaObject(target.Expression.Cast(target.LimitType), target.PSGetTypeRestriction()), new DynamicMetaObject(expression, indexes[0].PSGetTypeRestriction()), new DynamicMetaObject(expression2, value.PSGetTypeRestriction()), target.LimitType.GetProperty("Length"), (Expression t, Expression i, Expression v) => Expression.Assign(Expression.ArrayAccess(t, new Expression[]
			{
				i
			}), v));
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x00168DA4 File Offset: 0x00166FA4
		private DynamicMetaObject SetIndexMultiDimensionArray(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			Type elementType = target.LimitType.GetElementType();
			Expression expression = PSGetIndexBinder.ConvertIndex(value, elementType);
			if (expression == null)
			{
				return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(value, elementType, this, this._version, indexes.Prepend(target).ToArray<DynamicMetaObject>());
			}
			if (indexes.Length == 1)
			{
				Expression expression2 = PSGetIndexBinder.ConvertIndex(indexes[0], typeof(int[]));
				if (expression2 == null)
				{
					return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(indexes[0], typeof(int[]), this, this._version, new DynamicMetaObject[]
					{
						target,
						value
					});
				}
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ArrayOps_SetMDArrayValue, target.Expression.Cast(typeof(Array)), expression2, expression.Cast(typeof(object))), target.CombineRestrictions(indexes).Merge(value.PSGetTypeRestriction()));
			}
			else
			{
				Array array = (Array)target.Value;
				if (indexes.Length != array.Rank)
				{
					DynamicMetaObject result = errorSuggestion;
					if (errorSuggestion == null)
					{
						BindingRestrictions moreTests = value.PSGetTypeRestriction();
						string errorID = "NeedMultidimensionalIndex";
						string needMultidimensionalIndex = ParserStrings.NeedMultidimensionalIndex;
						Expression[] array2 = new Expression[2];
						array2[0] = ExpressionCache.Constant(array.Rank);
						array2[1] = Expression.Call(CachedReflectionInfo.ArrayOps_IndexStringMessage, Expression.NewArrayInit(typeof(object), from i in indexes
						select i.Expression.Cast(typeof(object))));
						result = target.ThrowRuntimeError(indexes, moreTests, errorID, needMultidimensionalIndex, array2);
					}
					return result;
				}
				Expression[] array3 = new Expression[indexes.Length];
				for (int j = 0; j < indexes.Length; j++)
				{
					array3[j] = PSGetIndexBinder.ConvertIndex(indexes[j], typeof(int));
					if (array3[j] == null)
					{
						return PSConvertBinder.ThrowNoConversion(indexes[j], typeof(int), this, this._version, indexes.Except(new DynamicMetaObject[]
						{
							indexes[j]
						}).Append(target).Append(value).ToArray<DynamicMetaObject>());
					}
				}
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ArrayOps_SetMDArrayValue, target.Expression.Cast(typeof(Array)), Expression.NewArrayInit(typeof(int), array3), expression.Cast(typeof(object))), target.CombineRestrictions(indexes).Merge(value.PSGetTypeRestriction()));
			}
		}

		// Token: 0x040021CF RID: 8655
		private static readonly Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints>, PSSetIndexBinder> _binderCache = new Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints>, PSSetIndexBinder>();

		// Token: 0x040021D0 RID: 8656
		private readonly PSMethodInvocationConstraints _constraints;

		// Token: 0x040021D1 RID: 8657
		internal int _version;
	}
}
