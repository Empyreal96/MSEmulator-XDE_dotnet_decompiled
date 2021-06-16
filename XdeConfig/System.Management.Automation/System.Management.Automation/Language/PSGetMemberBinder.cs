using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.ComInterop;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Threading;

namespace System.Management.Automation.Language
{
	// Token: 0x0200061A RID: 1562
	internal class PSGetMemberBinder : GetMemberBinder
	{
		// Token: 0x06004400 RID: 17408 RVA: 0x00168FF4 File Offset: 0x001671F4
		static PSGetMemberBinder()
		{
			PSGetMemberBinder._binderCache.Add(Tuple.Create<string, Type, bool, bool>("psadapted", null, false, false), new PSGetMemberBinder.ReservedMemberBinder("psadapted", true, false));
			PSGetMemberBinder._binderCache.Add(Tuple.Create<string, Type, bool, bool>("psextended", null, false, false), new PSGetMemberBinder.ReservedMemberBinder("psextended", true, false));
			PSGetMemberBinder._binderCache.Add(Tuple.Create<string, Type, bool, bool>("psbase", null, false, false), new PSGetMemberBinder.ReservedMemberBinder("psbase", true, false));
			PSGetMemberBinder._binderCache.Add(Tuple.Create<string, Type, bool, bool>("psobject", null, false, false), new PSGetMemberBinder.ReservedMemberBinder("psobject", true, false));
			PSGetMemberBinder._binderCache.Add(Tuple.Create<string, Type, bool, bool>("pstypenames", null, false, false), new PSGetMemberBinder.ReservedMemberBinder("pstypenames", true, false));
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06004401 RID: 17409 RVA: 0x001690CE File Offset: 0x001672CE
		internal bool HasInstanceMember
		{
			get
			{
				return this._hasInstanceMember;
			}
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x001690E0 File Offset: 0x001672E0
		internal static void SetHasInstanceMember(string memberName)
		{
			List<PSGetMemberBinder> orAdd = PSGetMemberBinder._binderCacheIgnoringCase.GetOrAdd(memberName, (string _) => new List<PSGetMemberBinder>());
			lock (orAdd)
			{
				if (!orAdd.Any<PSGetMemberBinder>())
				{
					PSGetMemberBinder.Get(memberName, null, false);
				}
				foreach (PSGetMemberBinder psgetMemberBinder in orAdd)
				{
					if (!psgetMemberBinder._hasInstanceMember)
					{
						lock (psgetMemberBinder)
						{
							if (!psgetMemberBinder._hasInstanceMember)
							{
								psgetMemberBinder._version++;
								psgetMemberBinder._hasInstanceMember = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x001691DC File Offset: 0x001673DC
		internal static void TypeTableMemberAdded(string memberName)
		{
			List<PSGetMemberBinder> orAdd = PSGetMemberBinder._binderCacheIgnoringCase.GetOrAdd(memberName, (string _) => new List<PSGetMemberBinder>());
			lock (orAdd)
			{
				if (orAdd.Count == 0)
				{
					PSGetMemberBinder.Get(memberName, null, false);
				}
				foreach (PSGetMemberBinder psgetMemberBinder in orAdd)
				{
					lock (psgetMemberBinder)
					{
						psgetMemberBinder._version++;
						psgetMemberBinder._hasTypeTableMember = true;
					}
				}
			}
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x001692C8 File Offset: 0x001674C8
		internal static void TypeTableMemberPossiblyUpdated(string memberName)
		{
			List<PSGetMemberBinder> orAdd = PSGetMemberBinder._binderCacheIgnoringCase.GetOrAdd(memberName, (string _) => new List<PSGetMemberBinder>());
			lock (orAdd)
			{
				foreach (PSGetMemberBinder psgetMemberBinder in orAdd)
				{
					Interlocked.Increment(ref psgetMemberBinder._version);
				}
			}
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0016936C File Offset: 0x0016756C
		public static PSGetMemberBinder Get(string memberName, TypeDefinitionAst classScope, bool @static)
		{
			return PSGetMemberBinder.Get(memberName, (classScope != null) ? classScope.Type : null, @static, false);
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x00169382 File Offset: 0x00167582
		public static PSGetMemberBinder Get(string memberName, Type classScope, bool @static)
		{
			return PSGetMemberBinder.Get(memberName, classScope, @static, false);
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x0016938D File Offset: 0x0016758D
		private PSGetMemberBinder GetNonEnumeratingBinder()
		{
			return PSGetMemberBinder.Get(base.Name, this._classScope, false, true);
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x001693AC File Offset: 0x001675AC
		private static PSGetMemberBinder Get(string memberName, Type classScope, bool @static, bool nonEnumerating)
		{
			PSGetMemberBinder psgetMemberBinder;
			lock (PSGetMemberBinder._binderCache)
			{
				Tuple<string, Type, bool, bool> key = Tuple.Create<string, Type, bool, bool>(memberName, classScope, @static, nonEnumerating);
				if (!PSGetMemberBinder._binderCache.TryGetValue(key, out psgetMemberBinder))
				{
					if (PSMemberInfoCollection<PSMemberInfo>.IsReservedName(memberName))
					{
						Tuple<string, Type, bool, bool> key2 = Tuple.Create<string, Type, bool, bool>(memberName.ToLowerInvariant(), null, @static, nonEnumerating);
						psgetMemberBinder = PSGetMemberBinder._binderCache[key2];
					}
					else
					{
						psgetMemberBinder = new PSGetMemberBinder(memberName, classScope, true, @static, nonEnumerating);
						if (!@static)
						{
							List<PSGetMemberBinder> orAdd = PSGetMemberBinder._binderCacheIgnoringCase.GetOrAdd(memberName, (string _) => new List<PSGetMemberBinder>());
							lock (orAdd)
							{
								if (orAdd.Any<PSGetMemberBinder>())
								{
									psgetMemberBinder._hasInstanceMember = orAdd[0]._hasInstanceMember;
									psgetMemberBinder._hasTypeTableMember = orAdd[0]._hasTypeTableMember;
								}
								orAdd.Add(psgetMemberBinder);
							}
						}
					}
					PSGetMemberBinder._binderCache.Add(key, psgetMemberBinder);
				}
			}
			return psgetMemberBinder;
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x001694CC File Offset: 0x001676CC
		private PSGetMemberBinder(string name, Type classScope, bool ignoreCase, bool @static, bool nonEnumerating) : base(name, ignoreCase)
		{
			this._static = @static;
			this._classScope = classScope;
			this._version = 0;
			this._nonEnumerating = nonEnumerating;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x001694F4 File Offset: 0x001676F4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "GetMember: {0}{1}{2} ver:{3}", new object[]
			{
				base.Name,
				this._static ? " static" : "",
				this._nonEnumerating ? " nonEnumerating" : "",
				this._version
			});
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00169560 File Offset: 0x00167760
		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			if (!target.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[0]);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				object obj = PSObject.Base(target.Value);
				if (obj != null && obj.GetType().FullName.Equals("System.__ComObject"))
				{
					return this.DeferForPSObject(new DynamicMetaObject[]
					{
						target
					}).WriteToDebugLog(this);
				}
			}
			DynamicMetaObject dynamicMetaObject;
			if (ComBinder.TryBindGetMember(this, target, out dynamicMetaObject))
			{
				dynamicMetaObject = new DynamicMetaObject(PSGetMemberBinder.WrapGetMemberInTry(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
				return dynamicMetaObject.WriteToDebugLog(this);
			}
			object obj2 = PSObject.Base(target.Value);
			if (obj2 == null)
			{
				return this.PropertyDoesntExist(target, target.PSGetTypeRestriction()).WriteToDebugLog(this);
			}
			Expression expression = null;
			PSMemberInfo psmemberInfo;
			if (this._hasInstanceMember && PSGetMemberBinder.TryGetInstanceMember(target.Value, base.Name, out psmemberInfo))
			{
				ParameterExpression parameterExpression = Expression.Variable(typeof(PSMemberInfo));
				expression = Expression.Condition(Expression.Call(CachedReflectionInfo.PSGetMemberBinder_TryGetInstanceMember, target.Expression.Cast(typeof(object)), Expression.Constant(base.Name), parameterExpression), Expression.Property(parameterExpression, "Value"), base.GetUpdateExpression(typeof(object)));
				expression = PSGetMemberBinder.WrapGetMemberInTry(expression);
				return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
				{
					parameterExpression
				}, new Expression[]
				{
					expression
				}), BinderUtils.GetVersionCheck(this, this._version)).WriteToDebugLog(this);
			}
			BindingRestrictions restrictions;
			bool flag;
			Type type;
			psmemberInfo = this.GetPSMemberInfo(target, out restrictions, out flag, out type, null, null);
			if (!flag)
			{
				return new DynamicMetaObject(PSGetMemberBinder.WrapGetMemberInTry(Expression.Call(CachedReflectionInfo.PSGetMemberBinder_GetAdaptedValue, PSGetMemberBinder.GetTargetExpr(target, typeof(object)), Expression.Constant(base.Name))), restrictions).WriteToDebugLog(this);
			}
			if (psmemberInfo != null)
			{
				PSPropertyInfo pspropertyInfo = psmemberInfo as PSPropertyInfo;
				if (pspropertyInfo != null)
				{
					if (!pspropertyInfo.IsGettable)
					{
						return this.GenerateGetPropertyException(restrictions).WriteToDebugLog(this);
					}
					PSProperty psproperty = pspropertyInfo as PSProperty;
					if (psproperty != null)
					{
						DotNetAdapter.PropertyCacheEntry propertyCacheEntry = psproperty.adapterData as DotNetAdapter.PropertyCacheEntry;
						if (!propertyCacheEntry.member.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
						{
							Expression expression2 = this._static ? null : PSGetMemberBinder.GetTargetExpr(target, propertyCacheEntry.member.DeclaringType);
							PropertyInfo propertyInfo = propertyCacheEntry.member as PropertyInfo;
							if (propertyInfo != null)
							{
								if (propertyInfo.GetMethod.IsFamily && (this._classScope == null || !this._classScope.IsSubclassOf(propertyInfo.DeclaringType)))
								{
									return this.GenerateGetPropertyException(restrictions).WriteToDebugLog(this);
								}
								expression = Expression.Property(expression2, propertyInfo);
							}
							else
							{
								expression = Expression.Field(expression2, (FieldInfo)propertyCacheEntry.member);
							}
						}
						else
						{
							expression = ExpressionCache.NullConstant;
						}
					}
					PSScriptProperty psscriptProperty = pspropertyInfo as PSScriptProperty;
					if (psscriptProperty != null)
					{
						expression = Expression.Call(Expression.Constant(psscriptProperty, typeof(PSScriptProperty)), CachedReflectionInfo.PSScriptProperty_InvokeGetter, new Expression[]
						{
							target.Expression.Cast(typeof(object))
						});
					}
					PSCodeProperty pscodeProperty = pspropertyInfo as PSCodeProperty;
					if (pscodeProperty != null)
					{
						expression = PSInvokeMemberBinder.InvokeMethod(pscodeProperty.GetterCodeReference, null, new DynamicMetaObject[]
						{
							target
						}, false, PSInvokeMemberBinder.MethodInvocationType.Getter);
					}
					PSNoteProperty psnoteProperty = pspropertyInfo as PSNoteProperty;
					if (psnoteProperty != null)
					{
						expression = Expression.Property(Expression.Constant(pspropertyInfo, typeof(PSNoteProperty)), CachedReflectionInfo.PSNoteProperty_Value);
					}
					if (type != null)
					{
						expression = expression.Convert(type);
					}
				}
				else
				{
					expression = Expression.Call(CachedReflectionInfo.PSGetMemberBinder_CloneMemberInfo, Expression.Constant(psmemberInfo, typeof(PSMemberInfo)), target.Expression.Cast(typeof(object)));
				}
			}
			if (obj2 is IDictionary)
			{
				Type type2 = null;
				bool flag2 = PSGetMemberBinder.IsGenericDictionary(obj2, ref type2);
				if (!flag2 || type2 != null)
				{
					ParameterExpression parameterExpression2 = Expression.Variable(typeof(object));
					if (expression == null)
					{
						expression = (errorSuggestion ?? this.PropertyDoesntExist(target, restrictions)).Expression;
					}
					MethodInfo methodInfo = flag2 ? CachedReflectionInfo.PSGetMemberBinder_TryGetGenericDictionaryValue.MakeGenericMethod(new Type[]
					{
						type2
					}) : CachedReflectionInfo.PSGetMemberBinder_TryGetIDictionaryValue;
					expression = Expression.Block(new ParameterExpression[]
					{
						parameterExpression2
					}, new Expression[]
					{
						Expression.Condition(Expression.Call(methodInfo, PSGetMemberBinder.GetTargetExpr(target, methodInfo.GetParameters()[0].ParameterType), Expression.Constant(base.Name), parameterExpression2), parameterExpression2, expression.Cast(typeof(object)))
					});
				}
			}
			if (expression == null)
			{
				return (errorSuggestion ?? this.PropertyDoesntExist(target, restrictions)).WriteToDebugLog(this);
			}
			return new DynamicMetaObject(PSGetMemberBinder.WrapGetMemberInTry(expression), restrictions).WriteToDebugLog(this);
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x00169A48 File Offset: 0x00167C48
		private DynamicMetaObject GenerateGetPropertyException(BindingRestrictions restrictions)
		{
			return new DynamicMetaObject(Compiler.ThrowRuntimeError("WriteOnlyProperty", ExtendedTypeSystem.WriteOnlyProperty, this.ReturnType, new Expression[]
			{
				Expression.Constant(base.Name)
			}), restrictions);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x00169A88 File Offset: 0x00167C88
		internal static bool IsGenericDictionary(object value, ref Type genericTypeArg)
		{
			bool result = false;
			foreach (Type type in value.GetType().GetInterfaces())
			{
				if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<, >))
				{
					result = true;
					Type[] genericArguments = type.GetGenericArguments();
					if (genericArguments[0] == typeof(string))
					{
						genericTypeArg = genericArguments[1];
					}
				}
			}
			return result;
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x00169B04 File Offset: 0x00167D04
		internal static Expression GetTargetExpr(DynamicMetaObject target, Type castToType = null)
		{
			Expression expression = target.Expression;
			object obj = target.Value;
			PSObject psobject = obj as PSObject;
			if (psobject != null && psobject != AutomationNull.Value && !psobject.isDeserialized)
			{
				expression = Expression.Call(CachedReflectionInfo.PSObject_Base, expression);
				obj = PSObject.Base(obj);
			}
			Type type = castToType ?? ((obj != null) ? (DotNetAdapter.IsRuntimeTypeInstance(obj) ? typeof(Type) : obj.GetType()) : typeof(object));
			if (expression.Type != type)
			{
				expression = (type.GetTypeInfo().IsValueType ? ((Nullable.GetUnderlyingType(expression.Type) != null) ? Expression.Property(expression, "Value") : Expression.Unbox(expression, type)) : expression.Cast(type));
			}
			return expression;
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x00169BC8 File Offset: 0x00167DC8
		private DynamicMetaObject PropertyDoesntExist(DynamicMetaObject target, BindingRestrictions restrictions)
		{
			if (!this._nonEnumerating && target.Value != AutomationNull.Value)
			{
				DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
				if (dynamicMetaObject != null)
				{
					return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_PropertyGetter, Expression.Constant(this.GetNonEnumeratingBinder()), dynamicMetaObject.Expression), restrictions);
				}
			}
			if (base.Name.Equals("Length", StringComparison.OrdinalIgnoreCase) || base.Name.Equals("Count", StringComparison.OrdinalIgnoreCase))
			{
				int i = (PSObject.Base(target.Value) == null) ? 0 : 1;
				return new DynamicMetaObject(Expression.Condition(Compiler.IsStrictMode(2, null), this.ThrowPropertyNotFoundStrict(), ExpressionCache.Constant(i).Cast(typeof(object))), restrictions);
			}
			ConditionalExpression expression = Expression.Condition(Compiler.IsStrictMode(2, null), this.ThrowPropertyNotFoundStrict(), this._nonEnumerating ? ExpressionCache.AutomationNullConstant : ExpressionCache.NullConstant);
			return new DynamicMetaObject(expression, restrictions);
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x00169CAC File Offset: 0x00167EAC
		private Expression ThrowPropertyNotFoundStrict()
		{
			return Compiler.CreateThrow(typeof(object), typeof(PropertyNotFoundException), new Type[]
			{
				typeof(string),
				typeof(Exception),
				typeof(string),
				typeof(object[])
			}, new object[]
			{
				"PropertyNotFoundStrict",
				null,
				ParserStrings.PropertyNotFoundStrict,
				new object[]
				{
					base.Name
				}
			});
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x00169D3A File Offset: 0x00167F3A
		internal static DynamicMetaObject EnsureAllowedInLanguageMode(PSLanguageMode languageMode, DynamicMetaObject target, object targetValue, string name, bool isStatic, DynamicMetaObject[] args, BindingRestrictions moreTests, string errorID, string resourceString)
		{
			if (languageMode == PSLanguageMode.ConstrainedLanguage && !PSGetMemberBinder.IsAllowedInConstrainedLanguage(targetValue, name, isStatic))
			{
				return target.ThrowRuntimeError(args, moreTests, errorID, resourceString, new Expression[0]);
			}
			return null;
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x00169D64 File Offset: 0x00167F64
		internal static bool IsAllowedInConstrainedLanguage(object targetValue, string name, bool isStatic)
		{
			if (string.Equals(name, "ToString", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			Type type = targetValue as Type;
			if (!isStatic || type == null)
			{
				type = targetValue.GetType();
			}
			return CoreTypes.Contains(type);
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x00169DA4 File Offset: 0x00167FA4
		internal BindingRestrictions NotInstanceMember(DynamicMetaObject target)
		{
			ParameterExpression parameterExpression = Expression.Variable(typeof(PSMemberInfo));
			MethodCallExpression expression = Expression.Call(CachedReflectionInfo.PSGetMemberBinder_TryGetInstanceMember, target.Expression.Cast(typeof(object)), Expression.Constant(base.Name), parameterExpression);
			return BindingRestrictions.GetExpressionRestriction(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Not(expression)
			}));
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x00169E14 File Offset: 0x00168014
		private static Expression WrapGetMemberInTry(Expression expr)
		{
			ParameterExpression parameterExpression = Expression.Variable(typeof(Exception));
			return Expression.TryCatch(expr.Cast(typeof(object)), new CatchBlock[]
			{
				Expression.Catch(typeof(TerminateException), Expression.Rethrow(typeof(object))),
				Expression.Catch(typeof(MethodException), Expression.Rethrow(typeof(object))),
				Expression.Catch(typeof(PropertyNotFoundException), Expression.Rethrow(typeof(object))),
				Expression.Catch(parameterExpression, Expression.Block(Expression.Call(CachedReflectionInfo.CommandProcessorBase_CheckForSevereException, parameterExpression), ExpressionCache.NullConstant))
			});
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x00169ED0 File Offset: 0x001680D0
		private PSMemberInfo ResolveAlias(PSAliasProperty alias, DynamicMetaObject target, HashSet<string> aliases, List<BindingRestrictions> aliasRestrictions)
		{
			if (aliases == null)
			{
				aliases = new HashSet<string>
				{
					alias.Name
				};
			}
			else
			{
				if (aliases.Contains(alias.Name))
				{
					throw new ExtendedTypeSystemException("CycleInAliasLookup", null, ExtendedTypeSystem.CycleInAlias, new object[]
					{
						alias.Name
					});
				}
				aliases.Add(alias.Name);
			}
			PSGetMemberBinder psgetMemberBinder = PSGetMemberBinder.Get(alias.ReferencedMemberName, this._classScope, false);
			if (psgetMemberBinder.HasInstanceMember)
			{
				return null;
			}
			BindingRestrictions bindingRestrictions;
			bool flag;
			Type type;
			return psgetMemberBinder.GetPSMemberInfo(target, out bindingRestrictions, out flag, out type, aliases, aliasRestrictions);
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x00169F74 File Offset: 0x00168174
		internal PSMemberInfo GetPSMemberInfo(DynamicMetaObject target, out BindingRestrictions restrictions, out bool canOptimize, out Type aliasConversionType, HashSet<string> aliases = null, List<BindingRestrictions> aliasRestrictions = null)
		{
			aliasConversionType = null;
			BindingRestrictions versionCheck;
			bool hasTypeTableMember;
			bool hasInstanceMember;
			lock (this)
			{
				versionCheck = BinderUtils.GetVersionCheck(this, this._version);
				hasTypeTableMember = this._hasTypeTableMember;
				hasInstanceMember = this._hasInstanceMember;
			}
			if (this._static)
			{
				restrictions = target.PSGetStaticMemberRestriction();
				restrictions = restrictions.Merge(versionCheck);
				canOptimize = true;
				return PSObject.GetStaticCLRMember(target.Value, base.Name);
			}
			canOptimize = false;
			PSMemberInfo psmemberInfo = null;
			ConsolidatedString consolidatedString = null;
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			TypeTable typeTable = (executionContextFromTLS != null) ? executionContextFromTLS.TypeTable : null;
			if (hasTypeTableMember)
			{
				consolidatedString = PSObject.GetTypeNames(target.Value);
				if (typeTable != null)
				{
					psmemberInfo = typeTable.GetMembers<PSMemberInfo>(consolidatedString)[base.Name];
					if (psmemberInfo != null)
					{
						canOptimize = true;
					}
				}
			}
			PSObject psobject = target.Value as PSObject;
			bool flag2 = psobject != null && psobject.isDeserialized;
			object obj = flag2 ? target.Value : PSObject.Base(target.Value);
			PSObject.AdapterSet mappedAdapter = PSObject.GetMappedAdapter(obj, typeTable);
			if (psmemberInfo == null)
			{
				canOptimize = mappedAdapter.OriginalAdapter.SiteBinderCanOptimize;
				if (canOptimize)
				{
					psmemberInfo = mappedAdapter.OriginalAdapter.BaseGetMember<PSMemberInfo>(obj, base.Name);
				}
			}
			if (psmemberInfo == null && canOptimize && mappedAdapter.DotNetAdapter != null)
			{
				psmemberInfo = mappedAdapter.DotNetAdapter.BaseGetMember<PSMemberInfo>(obj, base.Name);
			}
			restrictions = versionCheck;
			if (aliasRestrictions != null)
			{
				aliasRestrictions.Add(versionCheck);
			}
			PSAliasProperty psaliasProperty = psmemberInfo as PSAliasProperty;
			if (psaliasProperty != null)
			{
				aliasConversionType = psaliasProperty.ConversionType;
				if (aliasRestrictions == null)
				{
					aliasRestrictions = new List<BindingRestrictions>();
				}
				psmemberInfo = this.ResolveAlias(psaliasProperty, target, aliases, aliasRestrictions);
				if (psmemberInfo == null)
				{
					canOptimize = false;
				}
				foreach (BindingRestrictions restrictions2 in aliasRestrictions)
				{
					restrictions = restrictions.Merge(restrictions2);
				}
			}
			if (this._classScope != null && (target.LimitType == this._classScope || target.LimitType.IsSubclassOf(this._classScope)) && mappedAdapter.OriginalAdapter == PSObject.dotNetInstanceAdapter)
			{
				List<MethodBase> list = null;
				foreach (MemberInfo memberInfo in this._classScope.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
				{
					if (base.Name.Equals(memberInfo.Name, StringComparison.OrdinalIgnoreCase))
					{
						PropertyInfo propertyInfo = memberInfo as PropertyInfo;
						if (propertyInfo != null)
						{
							MethodInfo getMethod = propertyInfo.GetGetMethod(true);
							MethodInfo setMethod = propertyInfo.GetSetMethod(true);
							if ((getMethod == null || getMethod.IsFamily || getMethod.IsPublic) && (setMethod == null || setMethod.IsFamily || setMethod.IsPublic))
							{
								psmemberInfo = new PSProperty(base.Name, PSObject.dotNetInstanceAdapter, target.Value, new DotNetAdapter.PropertyCacheEntry(propertyInfo));
							}
						}
						else
						{
							FieldInfo fieldInfo = memberInfo as FieldInfo;
							if (fieldInfo != null)
							{
								if (fieldInfo.IsFamily)
								{
									psmemberInfo = new PSProperty(base.Name, PSObject.dotNetInstanceAdapter, target.Value, new DotNetAdapter.PropertyCacheEntry(fieldInfo));
								}
							}
							else
							{
								MethodInfo methodInfo = memberInfo as MethodInfo;
								if (methodInfo != null && (methodInfo.IsPublic || methodInfo.IsFamily))
								{
									if (list == null)
									{
										list = new List<MethodBase>();
									}
									list.Add(methodInfo);
								}
							}
						}
					}
				}
				if (list != null && list.Count > 0)
				{
					PSMethod psmethod = psmemberInfo as PSMethod;
					if (psmethod != null)
					{
						DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)psmethod.adapterData;
						list.AddRange(from e in methodCacheEntry.methodInformationStructures
						select e.method);
						psmemberInfo = null;
					}
					if (psmemberInfo != null)
					{
						psmemberInfo = null;
					}
					else
					{
						DotNetAdapter.MethodCacheEntry adapterData = new DotNetAdapter.MethodCacheEntry(list.ToArray());
						psmemberInfo = new PSMethod(base.Name, PSObject.dotNetInstanceAdapter, null, adapterData);
					}
				}
			}
			if (hasInstanceMember)
			{
				restrictions = restrictions.Merge(this.NotInstanceMember(target));
			}
			restrictions = restrictions.Merge(target.PSGetTypeRestriction());
			if (flag2)
			{
				restrictions = restrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Field(target.Expression.Cast(typeof(PSObject)), CachedReflectionInfo.PSObject_isDeserialized)));
			}
			if (hasTypeTableMember)
			{
				restrictions = restrictions.Merge(BindingRestrictions.GetInstanceRestriction(Expression.Call(CachedReflectionInfo.PSGetMemberBinder_GetTypeTableFromTLS, new Expression[0]), typeTable));
				restrictions = restrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Call(CachedReflectionInfo.PSGetMemberBinder_IsTypeNameSame, target.Expression.Cast(typeof(object)), Expression.Constant(consolidatedString.Key))));
			}
			return psmemberInfo;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x0016A424 File Offset: 0x00168624
		internal static PSMemberInfo CloneMemberInfo(PSMemberInfo memberInfo, object obj)
		{
			memberInfo = memberInfo.Copy();
			memberInfo.ReplicateInstance(obj);
			return memberInfo;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0016A438 File Offset: 0x00168638
		internal static object GetAdaptedValue(object obj, string member)
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
			if (psmemberInfo != null)
			{
				return psmemberInfo.Value;
			}
			if (executionContextFromTLS != null && executionContextFromTLS.IsStrictVersion(2))
			{
				throw new PropertyNotFoundException("PropertyNotFoundStrict", null, ParserStrings.PropertyNotFoundStrict, new object[]
				{
					LanguagePrimitives.ConvertTo<string>(member)
				});
			}
			return null;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0016A4F6 File Offset: 0x001686F6
		internal static bool IsTypeNameSame(object value, string typeName)
		{
			return value != null && string.Equals(PSObject.GetTypeNames(value).Key, typeName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x0016A510 File Offset: 0x00168710
		internal static TypeTable GetTypeTableFromTLS()
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				return null;
			}
			return executionContextFromTLS.TypeTable;
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x0016A530 File Offset: 0x00168730
		internal static bool TryGetInstanceMember(object value, string memberName, out PSMemberInfo memberInfo)
		{
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
			memberInfo = (PSObject.HasInstanceMembers(value, out psmemberInfoInternalCollection) ? psmemberInfoInternalCollection[memberName] : null);
			return memberInfo != null;
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x0016A55C File Offset: 0x0016875C
		internal static bool TryGetIDictionaryValue(IDictionary hash, string memberName, out object value)
		{
			try
			{
				if (hash.Contains(memberName))
				{
					value = hash[memberName];
					return true;
				}
			}
			catch (InvalidOperationException)
			{
			}
			value = null;
			return false;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0016A59C File Offset: 0x0016879C
		internal static bool TryGetGenericDictionaryValue<T>(IDictionary<string, T> hash, string memberName, out object value)
		{
			T t;
			if (hash.TryGetValue(memberName, out t))
			{
				value = t;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x040021D7 RID: 8663
		private static readonly Dictionary<Tuple<string, Type, bool, bool>, PSGetMemberBinder> _binderCache = new Dictionary<Tuple<string, Type, bool, bool>, PSGetMemberBinder>(new PSGetMemberBinder.KeyComparer());

		// Token: 0x040021D8 RID: 8664
		private static readonly ConcurrentDictionary<string, List<PSGetMemberBinder>> _binderCacheIgnoringCase = new ConcurrentDictionary<string, List<PSGetMemberBinder>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040021D9 RID: 8665
		private readonly bool _static;

		// Token: 0x040021DA RID: 8666
		private readonly bool _nonEnumerating;

		// Token: 0x040021DB RID: 8667
		private readonly Type _classScope;

		// Token: 0x040021DC RID: 8668
		internal int _version;

		// Token: 0x040021DD RID: 8669
		private bool _hasInstanceMember;

		// Token: 0x040021DE RID: 8670
		private bool _hasTypeTableMember;

		// Token: 0x0200061B RID: 1563
		private class KeyComparer : IEqualityComparer<Tuple<string, Type, bool, bool>>
		{
			// Token: 0x06004423 RID: 17443 RVA: 0x0016A5C4 File Offset: 0x001687C4
			public bool Equals(Tuple<string, Type, bool, bool> x, Tuple<string, Type, bool, bool> y)
			{
				StringComparison comparisonType = x.Item3 ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
				return x.Item1.Equals(y.Item1, comparisonType) && x.Item2 == y.Item2 && x.Item3 == y.Item3 && x.Item4 == y.Item4;
			}

			// Token: 0x06004424 RID: 17444 RVA: 0x0016A624 File Offset: 0x00168824
			public int GetHashCode(Tuple<string, Type, bool, bool> obj)
			{
				StringComparer stringComparer = obj.Item3 ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
				return Utils.CombineHashCodes(stringComparer.GetHashCode(obj.Item1), (obj.Item2 == null) ? 0 : obj.Item2.GetHashCode(), obj.Item3.GetHashCode(), obj.Item4.GetHashCode());
			}
		}

		// Token: 0x0200061C RID: 1564
		private class ReservedMemberBinder : PSGetMemberBinder
		{
			// Token: 0x06004426 RID: 17446 RVA: 0x0016A697 File Offset: 0x00168897
			internal ReservedMemberBinder(string name, bool ignoreCase, bool @static) : base(name, null, ignoreCase, @static, false)
			{
			}

			// Token: 0x06004427 RID: 17447 RVA: 0x0016A6A4 File Offset: 0x001688A4
			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				MethodInfo method = null;
				Expression arg = null;
				string name;
				if ((name = base.Name) != null)
				{
					if (!(name == "psadapted"))
					{
						if (!(name == "psbase"))
						{
							if (!(name == "psextended"))
							{
								if (!(name == "psobject"))
								{
									if (name == "pstypenames")
									{
										method = CachedReflectionInfo.ReservedNameMembers_PSTypeNames;
										arg = target.Expression.Convert(typeof(PSObject));
									}
								}
								else
								{
									method = CachedReflectionInfo.ReservedNameMembers_GeneratePSObjectMemberSet;
									arg = target.Expression.Cast(typeof(object));
								}
							}
							else
							{
								method = CachedReflectionInfo.ReservedNameMembers_GeneratePSExtendedMemberSet;
								arg = target.Expression.Cast(typeof(object));
							}
						}
						else
						{
							method = CachedReflectionInfo.ReservedNameMembers_GeneratePSBaseMemberSet;
							arg = target.Expression.Cast(typeof(object));
						}
					}
					else
					{
						method = CachedReflectionInfo.ReservedNameMembers_GeneratePSAdaptedMemberSet;
						arg = target.Expression.Cast(typeof(object));
					}
				}
				return new DynamicMetaObject(PSGetMemberBinder.WrapGetMemberInTry(Expression.Call(method, arg)), target.PSGetTypeRestriction());
			}
		}
	}
}
