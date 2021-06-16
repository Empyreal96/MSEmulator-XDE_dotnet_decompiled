using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.ComInterop;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x0200061D RID: 1565
	internal class PSSetMemberBinder : SetMemberBinder
	{
		// Token: 0x06004428 RID: 17448 RVA: 0x0016A7B4 File Offset: 0x001689B4
		public static PSSetMemberBinder Get(string memberName, TypeDefinitionAst classScopeAst, bool @static)
		{
			Type classScope = (classScopeAst != null) ? classScopeAst.Type : null;
			return PSSetMemberBinder.Get(memberName, classScope, @static);
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0016A7D8 File Offset: 0x001689D8
		public static PSSetMemberBinder Get(string memberName, Type classScope, bool @static)
		{
			PSSetMemberBinder pssetMemberBinder;
			lock (PSSetMemberBinder._binderCache)
			{
				Tuple<string, Type, bool> key = Tuple.Create<string, Type, bool>(memberName, classScope, @static);
				if (!PSSetMemberBinder._binderCache.TryGetValue(key, out pssetMemberBinder))
				{
					pssetMemberBinder = new PSSetMemberBinder(memberName, true, @static, classScope);
					PSSetMemberBinder._binderCache.Add(key, pssetMemberBinder);
				}
			}
			return pssetMemberBinder;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0016A840 File Offset: 0x00168A40
		public PSSetMemberBinder(string name, bool ignoreCase, bool @static, Type classScope) : base(name, ignoreCase)
		{
			this._static = @static;
			this._classScope = classScope;
			this._getMemberBinder = PSGetMemberBinder.Get(name, this._classScope, @static);
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x0016A86C File Offset: 0x00168A6C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "SetMember: {0}{1} ver:{2}", new object[]
			{
				this._static ? "static " : "",
				base.Name,
				this._getMemberBinder._version
			});
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0016A8C4 File Offset: 0x00168AC4
		private Expression GetTransformedExpression(IEnumerable<ArgumentTransformationAttribute> transformationAttributes, Expression originalExpression)
		{
			if (transformationAttributes == null)
			{
				return originalExpression;
			}
			ArgumentTransformationAttribute[] array = transformationAttributes.ToArray<ArgumentTransformationAttribute>();
			if (array.Length == 0)
			{
				return originalExpression;
			}
			Expression expression = originalExpression.Convert(typeof(object));
			ParameterExpression parameterExpression = Expression.Variable(typeof(EngineIntrinsics));
			for (int i = array.Length - 1; i >= 0; i--)
			{
				expression = Expression.Call(Expression.Constant(array[i]), CachedReflectionInfo.ArgumentTransformationAttribute_Transform, parameterExpression, expression);
			}
			return Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Assign(parameterExpression, Expression.Property(ExpressionCache.GetExecutionContextFromTLS, CachedReflectionInfo.ExecutionContext_EngineIntrinsics)),
				expression
			});
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0016A968 File Offset: 0x00168B68
		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue || !value.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[]
				{
					value
				});
			}
			if ((target.Value is PSObject && PSObject.Base(target.Value) != target.Value) || (value.Value is PSObject && PSObject.Base(value.Value) != value.Value))
			{
				object obj = PSObject.Base(target.Value);
				if (obj != null && obj.GetType().FullName.Equals("System.__ComObject"))
				{
					return this.DeferForPSObject(new DynamicMetaObject[]
					{
						target,
						value
					}).WriteToDebugLog(this);
				}
			}
			DynamicMetaObject binder;
			if (ComBinder.TryBindSetMember(this, target, value, out binder))
			{
				return binder.UpdateComRestrictionsForPsObject(new DynamicMetaObject[]
				{
					value
				}).WriteToDebugLog(this);
			}
			object obj2 = PSObject.Base(target.Value);
			if (obj2 == null)
			{
				return target.ThrowRuntimeError(new DynamicMetaObject[]
				{
					value
				}, BindingRestrictions.Empty, "PropertyNotFound", ParserStrings.PropertyNotFound, new Expression[]
				{
					Expression.Constant(base.Name)
				}).WriteToDebugLog(this);
			}
			if (value.Value == AutomationNull.Value)
			{
				value = new DynamicMetaObject(ExpressionCache.NullConstant, value.PSGetTypeRestriction(), null);
			}
			PSMemberInfo psmemberInfo;
			if (this._getMemberBinder.HasInstanceMember && PSGetMemberBinder.TryGetInstanceMember(target.Value, base.Name, out psmemberInfo))
			{
				ParameterExpression parameterExpression = Expression.Variable(typeof(PSMemberInfo));
				ParameterExpression parameterExpression2 = Expression.Variable(typeof(object));
				ConditionalExpression conditionalExpression = Expression.Condition(Expression.Call(CachedReflectionInfo.PSGetMemberBinder_TryGetInstanceMember, target.Expression.Cast(typeof(object)), Expression.Constant(base.Name), parameterExpression), Expression.Assign(Expression.Property(parameterExpression, "Value"), value.Expression.Cast(typeof(object))), base.GetUpdateExpression(typeof(object)));
				BindingRestrictions restrictions = BinderUtils.GetVersionCheck(this._getMemberBinder, this._getMemberBinder._version).Merge(value.PSGetTypeRestriction());
				return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}, new Expression[]
				{
					conditionalExpression
				}), restrictions).WriteToDebugLog(this);
			}
			if (obj2 is IDictionary)
			{
				Type type = null;
				bool flag = PSGetMemberBinder.IsGenericDictionary(obj2, ref type);
				if (!flag || type != null)
				{
					Type type2 = flag ? typeof(IDictionary<, >).MakeGenericType(new Type[]
					{
						typeof(string),
						type
					}) : typeof(IDictionary);
					MethodInfo method = type2.GetMethod("set_Item");
					ParameterExpression parameterExpression3 = Expression.Variable(type ?? typeof(object));
					Type type3 = parameterExpression3.Type;
					bool debase;
					LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(value.Value, type3, out debase);
					if (conversionData.Rank != ConversionRank.None)
					{
						Expression expression = PSConvertBinder.InvokeConverter(conversionData, value.Expression, type3, debase, ExpressionCache.InvariantCulture);
						return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
						{
							parameterExpression3
						}, new Expression[]
						{
							Expression.Assign(parameterExpression3, expression),
							Expression.Call(PSGetMemberBinder.GetTargetExpr(target, type2), method, Expression.Constant(base.Name), expression),
							expression.Cast(typeof(object))
						}), target.CombineRestrictions(new DynamicMetaObject[]
						{
							value
						})).WriteToDebugLog(this);
					}
				}
			}
			BindingRestrictions bindingRestrictions;
			bool flag2;
			Type type4;
			psmemberInfo = this._getMemberBinder.GetPSMemberInfo(target, out bindingRestrictions, out flag2, out type4, null, null);
			bindingRestrictions = bindingRestrictions.Merge(value.PSGetTypeRestriction());
			if (ExecutionContext.HasEverUsedConstrainedLanguage)
			{
				bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				DynamicMetaObject dynamicMetaObject = PSGetMemberBinder.EnsureAllowedInLanguageMode(executionContextFromTLS.LanguageMode, target, obj2, base.Name, this._static, new DynamicMetaObject[]
				{
					value
				}, bindingRestrictions, "PropertySetterNotSupportedInConstrainedLanguage", ParserStrings.PropertySetConstrainedLanguage);
				if (dynamicMetaObject != null)
				{
					return dynamicMetaObject.WriteToDebugLog(this);
				}
			}
			if (!flag2)
			{
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.PSSetMemberBinder_SetAdaptedValue, PSGetMemberBinder.GetTargetExpr(target, typeof(object)), Expression.Constant(base.Name), value.Expression.Cast(typeof(object))), bindingRestrictions).WriteToDebugLog(this);
			}
			if (psmemberInfo == null)
			{
				return (errorSuggestion ?? new DynamicMetaObject(Compiler.ThrowRuntimeError("PropertyAssignmentException", ParserStrings.PropertyNotFound, this.ReturnType, new Expression[]
				{
					Expression.Constant(base.Name)
				}), bindingRestrictions)).WriteToDebugLog(this);
			}
			PSPropertyInfo pspropertyInfo = psmemberInfo as PSPropertyInfo;
			if (pspropertyInfo != null)
			{
				if (!pspropertyInfo.IsSettable)
				{
					return this.GeneratePropertyAssignmentException(bindingRestrictions).WriteToDebugLog(this);
				}
				PSProperty psproperty = pspropertyInfo as PSProperty;
				if (psproperty != null)
				{
					DotNetAdapter.PropertyCacheEntry propertyCacheEntry = psproperty.adapterData as DotNetAdapter.PropertyCacheEntry;
					if (propertyCacheEntry != null)
					{
						Expression expression2;
						if (propertyCacheEntry.member.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
						{
							Expression innerException = Expression.New(CachedReflectionInfo.SetValueException_ctor, new Expression[]
							{
								Expression.Constant("PropertyAssignmentException"),
								Expression.Constant(null, typeof(Exception)),
								Expression.Constant(ExtendedTypeSystem.CannotInvokeStaticMethodOnUninstantiatedGenericType),
								Expression.NewArrayInit(typeof(object), new Expression[]
								{
									Expression.Constant(propertyCacheEntry.member.DeclaringType.FullName)
								})
							});
							expression2 = Compiler.ThrowRuntimeErrorWithInnerException("PropertyAssignmentException", Expression.Constant(ExtendedTypeSystem.CannotInvokeStaticMethodOnUninstantiatedGenericType), innerException, this.ReturnType, new Expression[]
							{
								Expression.Constant(propertyCacheEntry.member.DeclaringType.FullName)
							});
							return new DynamicMetaObject(expression2, bindingRestrictions).WriteToDebugLog(this);
						}
						PropertyInfo propertyInfo = propertyCacheEntry.member as PropertyInfo;
						IEnumerable<ArgumentTransformationAttribute> customAttributes = propertyCacheEntry.member.GetCustomAttributes<ArgumentTransformationAttribute>();
						bool flag3 = customAttributes.Any<ArgumentTransformationAttribute>();
						Expression expression3 = this._static ? null : PSGetMemberBinder.GetTargetExpr(target, propertyCacheEntry.member.DeclaringType);
						Type type5;
						Expression left;
						if (propertyInfo != null)
						{
							if (propertyInfo.SetMethod.IsFamily && (this._classScope == null || !this._classScope.IsSubclassOf(propertyInfo.DeclaringType)))
							{
								return this.GeneratePropertyAssignmentException(bindingRestrictions).WriteToDebugLog(this);
							}
							type5 = propertyInfo.PropertyType;
							left = Expression.Property(expression3, propertyInfo);
						}
						else
						{
							FieldInfo fieldInfo = (FieldInfo)propertyCacheEntry.member;
							type5 = fieldInfo.FieldType;
							left = Expression.Field(expression3, fieldInfo);
						}
						Type underlyingType = Nullable.GetUnderlyingType(type5);
						if (underlyingType != null)
						{
							if (value.Value == null)
							{
								expression2 = Expression.Block(Expression.Assign(left, this.GetTransformedExpression(customAttributes, Expression.Constant(null, type5))), ExpressionCache.NullConstant);
							}
							else
							{
								ParameterExpression parameterExpression4 = Expression.Variable(underlyingType);
								Expression right;
								if (flag3)
								{
									Expression transformedExpression = this.GetTransformedExpression(customAttributes, value.Expression);
									right = DynamicExpression.Dynamic(PSConvertBinder.Get(underlyingType), underlyingType, transformedExpression);
								}
								else
								{
									right = value.CastOrConvert(underlyingType);
								}
								expression2 = Expression.Block(new ParameterExpression[]
								{
									parameterExpression4
								}, new Expression[]
								{
									Expression.Assign(parameterExpression4, right),
									Expression.Assign(left, Expression.New(type5.GetConstructor(new Type[]
									{
										underlyingType
									}), new Expression[]
									{
										parameterExpression4
									})),
									parameterExpression4.Cast(typeof(object))
								});
							}
						}
						else
						{
							ParameterExpression parameterExpression5 = Expression.Variable(type5);
							Expression right2;
							if (flag3)
							{
								right2 = DynamicExpression.Dynamic(PSConvertBinder.Get(type5), type5, this.GetTransformedExpression(customAttributes, value.Expression));
							}
							else
							{
								right2 = ((type5 == typeof(object) && value.LimitType == typeof(PSObject)) ? Expression.Call(CachedReflectionInfo.PSObject_Base, value.Expression.Cast(typeof(PSObject))) : value.CastOrConvert(type5));
							}
							expression2 = Expression.Block(new ParameterExpression[]
							{
								parameterExpression5
							}, new Expression[]
							{
								Expression.Assign(parameterExpression5, right2),
								Expression.Assign(left, parameterExpression5),
								parameterExpression5.Cast(typeof(object))
							});
						}
						ParameterExpression parameterExpression6 = Expression.Variable(typeof(Exception));
						expression2 = Expression.TryCatch(expression2.Cast(typeof(object)), new CatchBlock[]
						{
							Expression.Catch(parameterExpression6, Expression.Block(Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_ConvertToMethodInvocationException, parameterExpression6, Expression.Constant(typeof(SetValueInvocationException), typeof(Type)), Expression.Constant(base.Name), ExpressionCache.Constant(0), Expression.Constant(null, typeof(MemberInfo))), Expression.Rethrow(typeof(object))))
						});
						return new DynamicMetaObject(expression2, bindingRestrictions).WriteToDebugLog(this);
					}
				}
				PSCodeProperty pscodeProperty = pspropertyInfo as PSCodeProperty;
				if (pscodeProperty != null)
				{
					ParameterExpression parameterExpression7 = Expression.Variable(typeof(object));
					return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
					{
						parameterExpression7
					}, new Expression[]
					{
						Expression.Assign(parameterExpression7, value.CastOrConvert(parameterExpression7.Type)),
						PSInvokeMemberBinder.InvokeMethod(pscodeProperty.SetterCodeReference, null, new DynamicMetaObject[]
						{
							target,
							value
						}, false, PSInvokeMemberBinder.MethodInvocationType.Setter),
						parameterExpression7
					}), bindingRestrictions).WriteToDebugLog(this);
				}
				PSScriptProperty psscriptProperty = pspropertyInfo as PSScriptProperty;
				if (psscriptProperty != null)
				{
					return new DynamicMetaObject(Expression.Call(Expression.Constant(psscriptProperty, typeof(PSScriptProperty)), CachedReflectionInfo.PSScriptProperty_InvokeSetter, PSGetMemberBinder.GetTargetExpr(target, null), value.Expression.Cast(typeof(object))), bindingRestrictions).WriteToDebugLog(this);
				}
			}
			if (errorSuggestion != null)
			{
				return errorSuggestion.WriteToDebugLog(this);
			}
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.PSSetMemberBinder_SetAdaptedValue, PSGetMemberBinder.GetTargetExpr(target, typeof(object)), Expression.Constant(base.Name), value.Expression.Cast(typeof(object))), bindingRestrictions).WriteToDebugLog(this);
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0016B3DC File Offset: 0x001695DC
		private DynamicMetaObject GeneratePropertyAssignmentException(BindingRestrictions restrictions)
		{
			Expression innerException = Expression.New(CachedReflectionInfo.SetValueException_ctor, new Expression[]
			{
				Expression.Constant("PropertyAssignmentException"),
				Expression.Constant(null, typeof(Exception)),
				Expression.Constant(ParserStrings.PropertyIsReadOnly),
				Expression.NewArrayInit(typeof(object), new Expression[]
				{
					Expression.Constant(base.Name)
				})
			});
			Expression expression = Compiler.ThrowRuntimeErrorWithInnerException("PropertyAssignmentException", Expression.Constant(ParserStrings.PropertyIsReadOnly), innerException, this.ReturnType, new Expression[]
			{
				Expression.Constant(base.Name)
			});
			return new DynamicMetaObject(expression, restrictions);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0016B490 File Offset: 0x00169690
		internal static object SetAdaptedValue(object obj, string member, object value)
		{
			try
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				PSMemberInfo psmemberInfo = null;
				if (executionContextFromTLS != null && executionContextFromTLS.TypeTable != null)
				{
					ConsolidatedString typeNames = PSObject.GetTypeNames(obj);
					psmemberInfo = executionContextFromTLS.TypeTable.GetMembers<PSMemberInfo>(typeNames)[member];
					if (psmemberInfo != null)
					{
						psmemberInfo = PSGetMemberBinder.CloneMemberInfo(psmemberInfo, obj);
					}
				}
				PSObject.AdapterSet mappedAdapter = PSObject.GetMappedAdapter(obj, (executionContextFromTLS != null) ? executionContextFromTLS.TypeTable : null);
				if (psmemberInfo == null)
				{
					psmemberInfo = mappedAdapter.OriginalAdapter.BaseGetMember<PSMemberInfo>(obj, member);
				}
				if (psmemberInfo == null && mappedAdapter.DotNetAdapter != null)
				{
					psmemberInfo = mappedAdapter.DotNetAdapter.BaseGetMember<PSMemberInfo>(obj, member);
				}
				if (psmemberInfo == null)
				{
					throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "PropertyAssignmentException", ParserStrings.PropertyNotFound, new object[]
					{
						member
					});
				}
				psmemberInfo.Value = value;
			}
			catch (SetValueException)
			{
				throw;
			}
			catch (Exception exception)
			{
				ExceptionHandlingOps.ConvertToMethodInvocationException(exception, typeof(SetValueInvocationException), member, 0, null);
				throw;
			}
			return value;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0016B588 File Offset: 0x00169788
		internal static void InvalidateCache()
		{
			lock (PSSetMemberBinder._binderCache)
			{
				foreach (PSSetMemberBinder pssetMemberBinder in PSSetMemberBinder._binderCache.Values)
				{
					pssetMemberBinder._getMemberBinder._version++;
				}
			}
		}

		// Token: 0x040021E4 RID: 8676
		private static readonly Dictionary<Tuple<string, Type, bool>, PSSetMemberBinder> _binderCache = new Dictionary<Tuple<string, Type, bool>, PSSetMemberBinder>(new PSSetMemberBinder.KeyComparer());

		// Token: 0x040021E5 RID: 8677
		private readonly bool _static;

		// Token: 0x040021E6 RID: 8678
		private readonly Type _classScope;

		// Token: 0x040021E7 RID: 8679
		private readonly PSGetMemberBinder _getMemberBinder;

		// Token: 0x0200061E RID: 1566
		private class KeyComparer : IEqualityComparer<Tuple<string, Type, bool>>
		{
			// Token: 0x06004432 RID: 17458 RVA: 0x0016B628 File Offset: 0x00169828
			public bool Equals(Tuple<string, Type, bool> x, Tuple<string, Type, bool> y)
			{
				StringComparison comparisonType = x.Item3 ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
				return x.Item1.Equals(y.Item1, comparisonType) && x.Item2 == y.Item2 && x.Item3 == y.Item3;
			}

			// Token: 0x06004433 RID: 17459 RVA: 0x0016B67C File Offset: 0x0016987C
			public int GetHashCode(Tuple<string, Type, bool> obj)
			{
				StringComparer stringComparer = obj.Item3 ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
				return Utils.CombineHashCodes(stringComparer.GetHashCode(obj.Item1), (obj.Item2 == null) ? 0 : obj.Item2.GetHashCode(), obj.Item3.GetHashCode());
			}
		}
	}
}
