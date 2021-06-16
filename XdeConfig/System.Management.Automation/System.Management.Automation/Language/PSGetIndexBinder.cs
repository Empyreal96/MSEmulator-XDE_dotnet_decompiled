using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.ComInterop;
using System.Reflection;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000618 RID: 1560
	internal class PSGetIndexBinder : GetIndexBinder
	{
		// Token: 0x060043D5 RID: 17365 RVA: 0x001674F0 File Offset: 0x001656F0
		public static PSGetIndexBinder Get(int argCount, PSMethodInvocationConstraints constraints, bool allowSlicing = true)
		{
			PSGetIndexBinder result;
			lock (PSGetIndexBinder._binderCache)
			{
				Tuple<CallInfo, PSMethodInvocationConstraints, bool> tuple = Tuple.Create<CallInfo, PSMethodInvocationConstraints, bool>(new CallInfo(argCount, new string[0]), constraints, allowSlicing);
				PSGetIndexBinder psgetIndexBinder;
				if (!PSGetIndexBinder._binderCache.TryGetValue(tuple, out psgetIndexBinder))
				{
					psgetIndexBinder = new PSGetIndexBinder(tuple);
					PSGetIndexBinder._binderCache.Add(tuple, psgetIndexBinder);
				}
				result = psgetIndexBinder;
			}
			return result;
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x00167564 File Offset: 0x00165764
		private PSGetIndexBinder(Tuple<CallInfo, PSMethodInvocationConstraints, bool> tuple) : base(tuple.Item1)
		{
			this._constraints = tuple.Item2;
			this._allowSlicing = tuple.Item3;
			this._version = 0;
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x00167594 File Offset: 0x00165794
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSGetIndexBinder indexCount={0}{1}{2} ver:{3}", new object[]
			{
				base.CallInfo.ArgumentCount,
				this._allowSlicing ? "" : " slicing disallowed",
				(this._constraints == null) ? "" : (" constraints: " + this._constraints),
				this._version
			});
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x00167614 File Offset: 0x00165814
		internal static void InvalidateCache()
		{
			lock (PSGetIndexBinder._binderCache)
			{
				foreach (PSGetIndexBinder psgetIndexBinder in PSGetIndexBinder._binderCache.Values)
				{
					psgetIndexBinder._version++;
				}
			}
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x001676D0 File Offset: 0x001658D0
		public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
		{
			if (target.HasValue)
			{
				if (!indexes.Any((DynamicMetaObject mo) => !mo.HasValue))
				{
					if (!(target.Value is PSObject) || PSObject.Base(target.Value) == target.Value)
					{
						if (!indexes.Any((DynamicMetaObject mo) => mo.Value is PSObject && PSObject.Base(mo.Value) != mo.Value))
						{
							DynamicMetaObject binder;
							if (ComBinder.TryBindGetIndex(this, target, indexes, out binder))
							{
								return binder.UpdateComRestrictionsForPsObject(indexes).WriteToDebugLog(this);
							}
							if (target.Value == null)
							{
								return (errorSuggestion ?? target.ThrowRuntimeError(indexes, BindingRestrictions.Empty, "NullArray", ParserStrings.NullArray, new Expression[0])).WriteToDebugLog(this);
							}
							if (indexes.Length == 1 && indexes[0].Value == null && this._allowSlicing)
							{
								return (errorSuggestion ?? target.ThrowRuntimeError(indexes, BindingRestrictions.Empty, "NullArrayIndex", ParserStrings.NullArrayIndex, new Expression[0])).WriteToDebugLog(this);
							}
							if (target.LimitType.IsArray)
							{
								return this.GetIndexArray(target, indexes, errorSuggestion).WriteToDebugLog(this);
							}
							foreach (Type type in target.LimitType.GetInterfaces())
							{
								if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<, >))
								{
									DynamicMetaObject indexDictionary = this.GetIndexDictionary(target, indexes, type);
									if (indexDictionary != null)
									{
										return indexDictionary.WriteToDebugLog(this);
									}
								}
							}
							DefaultMemberAttribute defaultMemberAttribute = target.LimitType.GetCustomAttributes(true).FirstOrDefault<DefaultMemberAttribute>();
							if (defaultMemberAttribute != null)
							{
								return this.InvokeIndexer(target, indexes, errorSuggestion, defaultMemberAttribute.MemberName).WriteToDebugLog(this);
							}
							return errorSuggestion ?? this.CannotIndexTarget(target, indexes).WriteToDebugLog(this);
						}
					}
					return this.DeferForPSObject(indexes.Prepend(target).ToArray<DynamicMetaObject>()).WriteToDebugLog(this);
				}
			}
			return base.Defer(indexes.Prepend(target).ToArray<DynamicMetaObject>()).WriteToDebugLog(this);
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x001678E4 File Offset: 0x00165AE4
		private DynamicMetaObject CannotIndexTarget(DynamicMetaObject target, DynamicMetaObject[] indexes)
		{
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(indexes);
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetVersionCheck(this, this._version));
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
			MethodCallExpression expression = Expression.Call(CachedReflectionInfo.ArrayOps_GetNonIndexable, target.Expression.Cast(typeof(object)), Expression.NewArrayInit(typeof(object), from d in indexes
			select d.Expression.Cast(typeof(object))));
			return new DynamicMetaObject(expression, bindingRestrictions);
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x00167974 File Offset: 0x00165B74
		private DynamicMetaObject GetIndexDictionary(DynamicMetaObject target, DynamicMetaObject[] indexes, Type idictionary)
		{
			if (indexes.Length > 1)
			{
				return null;
			}
			MethodInfo method = idictionary.GetMethod("TryGetValue");
			ParameterInfo[] parameters = method.GetParameters();
			Type parameterType = parameters[0].ParameterType;
			bool debase;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(indexes[0].Value, parameterType, out debase);
			if (conversionData.Rank == ConversionRank.None)
			{
				return null;
			}
			if (indexes[0].LimitType.IsArray && !parameterType.IsArray)
			{
				return null;
			}
			BindingRestrictions bindingRestrictions = target.CombineRestrictions(indexes);
			bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(this, parameterType, this._version));
			Expression arg = PSConvertBinder.InvokeConverter(conversionData, indexes[0].Expression, parameterType, debase, ExpressionCache.InvariantCulture);
			ParameterExpression parameterExpression = Expression.Parameter(parameters[1].ParameterType.GetElementType(), "outParam");
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Condition(Expression.Call(target.Expression.Cast(idictionary), method, arg, parameterExpression), parameterExpression.Cast(typeof(object)), this.GetNullResult())
			}), bindingRestrictions);
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x00167AB0 File Offset: 0x00165CB0
		internal static bool CanIndexFromEndWithNegativeIndex(DynamicMetaObject target)
		{
			Type limitType = target.LimitType;
			if (limitType.IsArray || limitType == typeof(string) || limitType == typeof(StringBuilder))
			{
				return true;
			}
			if (typeof(IList).IsAssignableFrom(limitType))
			{
				return true;
			}
			if (typeof(OrderedDictionary).IsAssignableFrom(limitType))
			{
				return true;
			}
			return limitType.GetInterfaces().Any((Type i) => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x00167B44 File Offset: 0x00165D44
		private DynamicMetaObject IndexWithNegativeChecks(DynamicMetaObject target, DynamicMetaObject index, PropertyInfo lengthProperty, Func<Expression, Expression, Expression> generateIndexOperation)
		{
			ParameterExpression parameterExpression = Expression.Parameter(target.LimitType, "target");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(int), "len");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(int), "index");
			Expression expr = Expression.Block(new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2,
				parameterExpression3
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, target.Expression.Cast(target.LimitType)),
				Expression.Assign(parameterExpression2, Expression.Property(parameterExpression, lengthProperty)),
				Expression.Assign(parameterExpression3, index.Expression),
				Expression.IfThen(Expression.LessThan(parameterExpression3, ExpressionCache.Constant(0)), Expression.Assign(parameterExpression3, Expression.Add(parameterExpression3, parameterExpression2))),
				generateIndexOperation(parameterExpression, parameterExpression3)
			});
			return new DynamicMetaObject(this.SafeIndexResult(expr), target.CombineRestrictions(new DynamicMetaObject[]
			{
				index
			}));
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x00167C5C File Offset: 0x00165E5C
		private DynamicMetaObject GetIndexArray(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
		{
			Array array = (Array)target.Value;
			if (array.Rank > 1)
			{
				return this.GetIndexMultiDimensionArray(target, indexes, errorSuggestion);
			}
			if (indexes.Length > 1)
			{
				DynamicMetaObject result;
				if (!this._allowSlicing)
				{
					result = errorSuggestion;
					if (errorSuggestion == null)
					{
						return this.CannotIndexTarget(target, indexes);
					}
				}
				else
				{
					result = this.InvokeSlicingIndexer(target, indexes);
				}
				return result;
			}
			DynamicMetaObject dynamicMetaObject = this.CheckForSlicing(target, indexes);
			if (dynamicMetaObject != null)
			{
				return dynamicMetaObject;
			}
			Expression expression = PSGetIndexBinder.ConvertIndex(indexes[0], typeof(int));
			if (expression == null)
			{
				return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(target, typeof(int), this, this._version, indexes);
			}
			return this.IndexWithNegativeChecks(new DynamicMetaObject(target.Expression.Cast(target.LimitType), target.PSGetTypeRestriction()), new DynamicMetaObject(expression, indexes[0].PSGetTypeRestriction()), target.LimitType.GetProperty("Length"), (Expression t, Expression i) => Expression.ArrayIndex(t, i).Cast(typeof(object)));
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x00167D68 File Offset: 0x00165F68
		private DynamicMetaObject GetIndexMultiDimensionArray(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
		{
			Array array = (Array)target.Value;
			if (indexes.Length == 1)
			{
				if (PSEnumerableBinder.IsEnumerable(indexes[0]) == null)
				{
					return target.ThrowRuntimeError(indexes, BindingRestrictions.Empty, "NeedMultidimensionalIndex", ParserStrings.NeedMultidimensionalIndex, new Expression[]
					{
						ExpressionCache.Constant(array.Rank),
						DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), indexes[0].Expression, ExpressionCache.GetExecutionContextFromTLS)
					});
				}
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ArrayOps_GetMDArrayValueOrSlice, Expression.Convert(target.Expression, typeof(Array)), indexes[0].Expression.Cast(typeof(object))), target.CombineRestrictions(indexes));
			}
			else
			{
				Expression[] array2 = (from index in indexes
				select PSGetIndexBinder.ConvertIndex(index, typeof(int)) into i
				where i != null
				select i).ToArray<Expression>();
				if (array2.Length == indexes.Length)
				{
					return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ArrayOps_GetMDArrayValue, Expression.Convert(target.Expression, typeof(Array)), Expression.NewArrayInit(typeof(int), array2), ExpressionCache.Constant(!this._allowSlicing)), target.CombineRestrictions(indexes));
				}
				if (!this._allowSlicing)
				{
					return errorSuggestion ?? this.CannotIndexTarget(target, indexes);
				}
				return this.InvokeSlicingIndexer(target, indexes);
			}
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x00167F24 File Offset: 0x00166124
		private DynamicMetaObject InvokeIndexer(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion, string methodName)
		{
			MethodInfo getter = PSInvokeMemberBinder.FindBestMethod(target, indexes, "get_" + methodName, false, this._constraints);
			if (getter == null)
			{
				return this.CheckForSlicing(target, indexes) ?? (errorSuggestion ?? this.CannotIndexTarget(target, indexes));
			}
			ParameterInfo[] parameters = getter.GetParameters();
			if (parameters.Length == indexes.Length)
			{
				if (parameters.Length == 1)
				{
					DynamicMetaObject dynamicMetaObject = this.CheckForSlicing(target, indexes);
					if (dynamicMetaObject != null)
					{
						return dynamicMetaObject;
					}
				}
				Expression[] array = new Expression[parameters.Length];
				for (int j = 0; j < parameters.Length; j++)
				{
					Type parameterType = parameters[j].ParameterType;
					array[j] = PSGetIndexBinder.ConvertIndex(indexes[j], parameterType);
					if (array[j] == null)
					{
						return errorSuggestion ?? PSConvertBinder.ThrowNoConversion(target, parameterType, this, this._version, indexes);
					}
				}
				if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int) && PSGetIndexBinder.CanIndexFromEndWithNegativeIndex(target))
				{
					PropertyInfo propertyInfo = target.LimitType.GetProperty("Count") ?? target.LimitType.GetProperty("Length");
					if (propertyInfo != null)
					{
						return this.IndexWithNegativeChecks(new DynamicMetaObject(target.Expression.Cast(target.LimitType), target.PSGetTypeRestriction()), new DynamicMetaObject(array[0], indexes[0].PSGetTypeRestriction()), propertyInfo, (Expression t, Expression i) => Expression.Call(t, getter, new Expression[]
						{
							i
						}).Cast(typeof(object)));
					}
				}
				BindingRestrictions bindingRestrictions = target.CombineRestrictions(indexes);
				bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetVersionCheck(this, this._version));
				bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
				return new DynamicMetaObject(this.SafeIndexResult(Expression.Call(target.Expression.Cast(getter.DeclaringType), getter, array)), bindingRestrictions);
			}
			if (parameters.Length == 1 && this._allowSlicing)
			{
				return this.InvokeSlicingIndexer(target, indexes);
			}
			return errorSuggestion ?? this.CannotIndexTarget(target, indexes);
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x00168120 File Offset: 0x00166320
		internal static Expression ConvertIndex(DynamicMetaObject index, Type resultType)
		{
			bool debase;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(index.Value, resultType, out debase);
			if (conversionData.Rank != ConversionRank.None)
			{
				return PSConvertBinder.InvokeConverter(conversionData, index.Expression, resultType, debase, ExpressionCache.InvariantCulture);
			}
			return null;
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x00168198 File Offset: 0x00166398
		private DynamicMetaObject CheckForSlicing(DynamicMetaObject target, DynamicMetaObject[] indexes)
		{
			if (!this._allowSlicing)
			{
				return null;
			}
			if (indexes.Length > 1)
			{
				PSGetIndexBinder nonSlicingBinder = PSGetIndexBinder.Get(1, this._constraints, false);
				NewArrayExpression expression = Expression.NewArrayInit(typeof(object), from i in indexes
				select DynamicExpression.Dynamic(nonSlicingBinder, typeof(object), target.Expression, i.Expression));
				return new DynamicMetaObject(expression, target.CombineRestrictions(indexes));
			}
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(indexes[0]);
			if (dynamicMetaObject != null)
			{
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_SlicingIndex, target.Expression.Cast(typeof(object)), dynamicMetaObject.Expression.Cast(typeof(IEnumerator)), Expression.Constant(this.GetNonSlicingIndexer())), target.CombineRestrictions(new DynamicMetaObject[]
				{
					dynamicMetaObject
				}));
			}
			return null;
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x001682A0 File Offset: 0x001664A0
		private DynamicMetaObject InvokeSlicingIndexer(DynamicMetaObject target, DynamicMetaObject[] indexes)
		{
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.ArrayOps_SlicingIndex, target.Expression.Cast(typeof(object)), Expression.NewArrayInit(typeof(object), from dmo in indexes
			select dmo.Expression.Cast(typeof(object))), Expression.Constant(this.GetNonSlicingIndexer())), target.CombineRestrictions(indexes));
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x00168318 File Offset: 0x00166518
		private Expression SafeIndexResult(Expression expr)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Exception));
			return Expression.TryCatch(expr.Cast(typeof(object)), new CatchBlock[]
			{
				Expression.Catch(parameterExpression, Expression.Block(Expression.Call(CachedReflectionInfo.CommandProcessorBase_CheckForSevereException, parameterExpression), Expression.IfThen(Compiler.IsStrictMode(3, null), Expression.Rethrow()), this.GetNullResult()))
			});
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x00168382 File Offset: 0x00166582
		private Expression GetNullResult()
		{
			if (!this._allowSlicing)
			{
				return ExpressionCache.AutomationNullConstant;
			}
			return ExpressionCache.NullConstant;
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x00168398 File Offset: 0x00166598
		private Func<object, object, object> GetNonSlicingIndexer()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object, object>>(DynamicExpression.Dynamic(PSGetIndexBinder.Get(1, this._constraints, false), typeof(object), parameterExpression, parameterExpression2), new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		// Token: 0x040021C3 RID: 8643
		private static readonly Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints, bool>, PSGetIndexBinder> _binderCache = new Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints, bool>, PSGetIndexBinder>();

		// Token: 0x040021C4 RID: 8644
		private readonly PSMethodInvocationConstraints _constraints;

		// Token: 0x040021C5 RID: 8645
		private readonly bool _allowSlicing;

		// Token: 0x040021C6 RID: 8646
		internal int _version;
	}
}
