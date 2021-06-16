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
	// Token: 0x02000620 RID: 1568
	internal class PSInvokeMemberBinder : InvokeMemberBinder
	{
		// Token: 0x06004437 RID: 17463 RVA: 0x0016B70D File Offset: 0x0016990D
		private PSInvokeMemberBinder GetNonEnumeratingBinder()
		{
			return PSInvokeMemberBinder.Get(base.Name, this._classScope, base.CallInfo, false, this._propertySetter, true, this._invocationConstraints);
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0016B734 File Offset: 0x00169934
		public static PSInvokeMemberBinder Get(string memberName, CallInfo callInfo, bool @static, bool propertySetter, PSMethodInvocationConstraints constraints, Type classScope)
		{
			return PSInvokeMemberBinder.Get(memberName, classScope, callInfo, @static, propertySetter, false, constraints);
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0016B744 File Offset: 0x00169944
		private static PSInvokeMemberBinder Get(string memberName, Type classScope, CallInfo callInfo, bool @static, bool propertySetter, bool nonEnumerating, PSMethodInvocationConstraints constraints)
		{
			PSInvokeMemberBinder psinvokeMemberBinder;
			lock (PSInvokeMemberBinder._binderCache)
			{
				Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type> key = Tuple.Create<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type>(memberName, callInfo, propertySetter, nonEnumerating, constraints, @static, classScope);
				if (!PSInvokeMemberBinder._binderCache.TryGetValue(key, out psinvokeMemberBinder))
				{
					psinvokeMemberBinder = new PSInvokeMemberBinder(memberName, true, @static, propertySetter, nonEnumerating, callInfo, constraints, classScope);
					PSInvokeMemberBinder._binderCache.Add(key, psinvokeMemberBinder);
				}
			}
			return psinvokeMemberBinder;
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0016B7BC File Offset: 0x001699BC
		private PSInvokeMemberBinder(string name, bool ignoreCase, bool @static, bool propertySetter, bool nonEnumerating, CallInfo callInfo, PSMethodInvocationConstraints invocationConstraints, Type classScope) : base(name, ignoreCase, callInfo)
		{
			this._static = @static;
			this._propertySetter = propertySetter;
			this._nonEnumerating = nonEnumerating;
			this._invocationConstraints = invocationConstraints;
			this._classScope = classScope;
			this._getMemberBinder = PSGetMemberBinder.Get(name, classScope, @static);
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0016B80C File Offset: 0x00169A0C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSInvokeMember: {0}{1}{2} ver:{3} args:{4} constraints:<{5}>", new object[]
			{
				this._static ? "static " : "",
				this._propertySetter ? "propset " : "",
				base.Name,
				this._getMemberBinder._version,
				base.CallInfo.ArgumentCount,
				(this._invocationConstraints != null) ? this._invocationConstraints.ToString() : ""
			});
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0016B954 File Offset: 0x00169B54
		public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			if (target.HasValue)
			{
				if (!args.Any((DynamicMetaObject arg) => !arg.HasValue))
				{
					if (!(target.Value is PSObject) || PSObject.Base(target.Value) == target.Value)
					{
						if (!args.Any((DynamicMetaObject mo) => mo.Value is PSObject && PSObject.Base(mo.Value) != mo.Value))
						{
							goto IL_C4;
						}
					}
					object obj = PSObject.Base(target.Value);
					if (obj != null && obj.GetType().FullName.Equals("System.__ComObject"))
					{
						return this.DeferForPSObject(args.Prepend(target).ToArray<DynamicMetaObject>()).WriteToDebugLog(this);
					}
					IL_C4:
					DynamicMetaObject binder;
					if (ComBinder.TryBindInvokeMember(this, this._propertySetter, target, args, out binder))
					{
						return binder.UpdateComRestrictionsForPsObject(args).WriteToDebugLog(this);
					}
					object obj2 = PSObject.Base(target.Value);
					if (obj2 == null)
					{
						if (!this._static && !this._nonEnumerating)
						{
							DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(Expression.Call(Expression.NewArrayInit(typeof(object), new Expression[0]), CachedReflectionInfo.IEnumerable_GetEnumerator), BindingRestrictions.GetInstanceRestriction(Expression.Call(CachedReflectionInfo.PSObject_Base, target.Expression), null)).WriteToDebugLog(this);
							BindingRestrictions bindingRestrictions = args.Aggregate(BindingRestrictions.Empty, (BindingRestrictions current, DynamicMetaObject arg) => current.Merge(arg.PSGetMethodArgumentRestriction()));
							if (string.Equals(base.Name, "Where", StringComparison.OrdinalIgnoreCase))
							{
								return this.InvokeWhereOnCollection(dynamicMetaObject, args, bindingRestrictions).WriteToDebugLog(this);
							}
							if (string.Equals(base.Name, "ForEach", StringComparison.OrdinalIgnoreCase))
							{
								return this.InvokeForEachOnCollection(dynamicMetaObject, args, bindingRestrictions).WriteToDebugLog(this);
							}
						}
						return target.ThrowRuntimeError(args, BindingRestrictions.Empty, "InvokeMethodOnNull", ParserStrings.InvokeMethodOnNull, new Expression[0]).WriteToDebugLog(this);
					}
					PSMemberInfo psmemberInfo;
					if (this._getMemberBinder.HasInstanceMember && PSGetMemberBinder.TryGetInstanceMember(target.Value, base.Name, out psmemberInfo))
					{
						ParameterExpression parameterExpression = Expression.Variable(typeof(PSMethodInfo));
						Expression test = Expression.Call(CachedReflectionInfo.PSInvokeMemberBinder_TryGetInstanceMethod, target.Expression.Cast(typeof(object)), Expression.Constant(base.Name), parameterExpression);
						Expression instance = parameterExpression;
						MethodInfo psmethodInfo_Invoke = CachedReflectionInfo.PSMethodInfo_Invoke;
						Expression[] array = new Expression[1];
						array[0] = Expression.NewArrayInit(typeof(object), from dmo in args
						select dmo.Expression.Cast(typeof(object)));
						ConditionalExpression conditionalExpression = Expression.Condition(test, Expression.Call(instance, psmethodInfo_Invoke, array), base.GetUpdateExpression(typeof(object)));
						return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
						{
							parameterExpression
						}, new Expression[]
						{
							conditionalExpression
						}), BinderUtils.GetVersionCheck(this._getMemberBinder, this._getMemberBinder._version)).WriteToDebugLog(this);
					}
					BindingRestrictions bindingRestrictions2;
					bool flag;
					Type type;
					PSMethodInfo psmethodInfo = this._getMemberBinder.GetPSMemberInfo(target, out bindingRestrictions2, out flag, out type, null, null) as PSMethodInfo;
					bindingRestrictions2 = args.Aggregate(bindingRestrictions2, (BindingRestrictions current, DynamicMetaObject arg) => current.Merge(arg.PSGetMethodArgumentRestriction()));
					if (ExecutionContext.HasEverUsedConstrainedLanguage)
					{
						bindingRestrictions2 = bindingRestrictions2.Merge(BinderUtils.GetLanguageModeCheckIfHasEverUsedConstrainedLanguage());
						ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
						DynamicMetaObject dynamicMetaObject2 = PSGetMemberBinder.EnsureAllowedInLanguageMode(executionContextFromTLS.LanguageMode, target, obj2, base.Name, this._static, args, bindingRestrictions2, "MethodInvocationNotSupportedInConstrainedLanguage", ParserStrings.InvokeMethodConstrainedLanguage);
						if (dynamicMetaObject2 != null)
						{
							return dynamicMetaObject2.WriteToDebugLog(this);
						}
					}
					if (!flag)
					{
						Expression expression;
						if (this._propertySetter)
						{
							expression = Expression.Call(CachedReflectionInfo.PSInvokeMemberBinder_InvokeAdaptedSetMember, PSGetMemberBinder.GetTargetExpr(target, typeof(object)), Expression.Constant(base.Name), Expression.NewArrayInit(typeof(object), from arg in args.Take(args.Length - 1)
							select arg.Expression.Cast(typeof(object))), args.Last<DynamicMetaObject>().Expression.Cast(typeof(object)));
						}
						else
						{
							expression = Expression.Call(CachedReflectionInfo.PSInvokeMemberBinder_InvokeAdaptedMember, PSGetMemberBinder.GetTargetExpr(target, typeof(object)), Expression.Constant(base.Name), Expression.NewArrayInit(typeof(object), from arg in args
							select arg.Expression.Cast(typeof(object))));
						}
						return new DynamicMetaObject(expression, bindingRestrictions2).WriteToDebugLog(this);
					}
					if (psmethodInfo is PSMethod || psmethodInfo is PSParameterizedProperty)
					{
						PSObject psobject = target.Value as PSObject;
						if (psobject != null && (obj2.GetType() == typeof(Hashtable) || obj2.GetType() == typeof(ArrayList)))
						{
							bindingRestrictions2 = bindingRestrictions2.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Not(Expression.Field(target.Expression.Cast(typeof(PSObject)), CachedReflectionInfo.PSObject_isDeserialized))));
						}
					}
					PSMethod psmethod = psmethodInfo as PSMethod;
					if (psmethod != null)
					{
						DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)psmethod.adapterData;
						return PSInvokeMemberBinder.InvokeDotNetMethod(base.CallInfo, base.Name, this._invocationConstraints, this._propertySetter ? PSInvokeMemberBinder.MethodInvocationType.Setter : PSInvokeMemberBinder.MethodInvocationType.Ordinary, target, args, bindingRestrictions2, methodCacheEntry.methodInformationStructures, typeof(MethodException)).WriteToDebugLog(this);
					}
					PSScriptMethod psscriptMethod = psmethodInfo as PSScriptMethod;
					if (psscriptMethod != null)
					{
						return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.PSScriptMethod_InvokeScript, Expression.Constant(base.Name), Expression.Constant(psscriptMethod.Script), target.Expression.Cast(typeof(object)), Expression.NewArrayInit(typeof(object), from e in args
						select e.Expression.Cast(typeof(object)))), bindingRestrictions2).WriteToDebugLog(this);
					}
					PSCodeMethod pscodeMethod = psmethodInfo as PSCodeMethod;
					if (pscodeMethod != null)
					{
						return new DynamicMetaObject(PSInvokeMemberBinder.InvokeMethod(pscodeMethod.CodeReference, null, args.Prepend(target).ToArray<DynamicMetaObject>(), false, PSInvokeMemberBinder.MethodInvocationType.Ordinary).Cast(typeof(object)), bindingRestrictions2).WriteToDebugLog(this);
					}
					PSParameterizedProperty psparameterizedProperty = psmethodInfo as PSParameterizedProperty;
					if (psparameterizedProperty != null)
					{
						DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = (DotNetAdapter.ParameterizedPropertyCacheEntry)psparameterizedProperty.adapterData;
						return PSInvokeMemberBinder.InvokeDotNetMethod(base.CallInfo, base.Name, this._invocationConstraints, this._propertySetter ? PSInvokeMemberBinder.MethodInvocationType.Setter : PSInvokeMemberBinder.MethodInvocationType.Ordinary, target, args, bindingRestrictions2, this._propertySetter ? parameterizedPropertyCacheEntry.setterInformation : parameterizedPropertyCacheEntry.getterInformation, this._propertySetter ? typeof(SetValueInvocationException) : typeof(GetValueInvocationException)).WriteToDebugLog(this);
					}
					if (errorSuggestion != null)
					{
						return errorSuggestion.WriteToDebugLog(this);
					}
					if (!this._static && !this._nonEnumerating && target.Value != AutomationNull.Value)
					{
						if (string.Equals(base.Name, "Where", StringComparison.OrdinalIgnoreCase))
						{
							return this.InvokeWhereOnCollection(target, args, bindingRestrictions2).WriteToDebugLog(this);
						}
						if (string.Equals(base.Name, "ForEach", StringComparison.OrdinalIgnoreCase))
						{
							return this.InvokeForEachOnCollection(target, args, bindingRestrictions2).WriteToDebugLog(this);
						}
						DynamicMetaObject dynamicMetaObject3 = PSEnumerableBinder.IsEnumerable(target);
						if (dynamicMetaObject3 != null)
						{
							return this.InvokeMemberOnCollection(dynamicMetaObject3, args, obj2.GetType(), bindingRestrictions2).WriteToDebugLog(this);
						}
					}
					Type type2 = (this._static && obj2 is Type) ? ((Type)obj2) : obj2.GetType();
					return new DynamicMetaObject(Compiler.ThrowRuntimeError("MethodNotFound", ParserStrings.MethodNotFound, new Expression[]
					{
						Expression.Constant(type2.FullName),
						Expression.Constant(base.Name)
					}), bindingRestrictions2).WriteToDebugLog(this);
				}
			}
			return base.Defer(args.Prepend(target).ToArray<DynamicMetaObject>());
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0016C100 File Offset: 0x0016A300
		internal static DynamicMetaObject InvokeDotNetMethod(CallInfo callInfo, string name, PSMethodInvocationConstraints psMethodInvocationConstraints, PSInvokeMemberBinder.MethodInvocationType methodInvocationType, DynamicMetaObject target, DynamicMetaObject[] args, BindingRestrictions restrictions, MethodInformation[] mi, Type errorExceptionType)
		{
			string text = null;
			string text2 = null;
			int num = args.Length;
			if (methodInvocationType == PSInvokeMemberBinder.MethodInvocationType.Setter)
			{
				num--;
			}
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				object value = args[i].Value;
				array[i] = ((value == AutomationNull.Value) ? null : value);
			}
			if (ClrFacade.IsTransparentProxy(target.Value) && (psMethodInvocationConstraints == null || psMethodInvocationConstraints.MethodTargetType == null))
			{
				Type[] parameterTypes = (psMethodInvocationConstraints == null) ? new Type[num] : psMethodInvocationConstraints.ParameterTypes.ToArray<Type>();
				psMethodInvocationConstraints = new PSMethodInvocationConstraints(target.Value.GetType(), parameterTypes);
			}
			bool expandParameters;
			bool flag;
			MethodInformation methodInformation = Adapter.FindBestMethod(mi, psMethodInvocationConstraints, array, ref text, ref text2, out expandParameters, out flag);
			if (flag && methodInvocationType != PSInvokeMemberBinder.MethodInvocationType.BaseCtor)
			{
				methodInvocationType = PSInvokeMemberBinder.MethodInvocationType.NonVirtual;
			}
			if (methodInformation == null)
			{
				return new DynamicMetaObject(Compiler.CreateThrow(typeof(object), errorExceptionType, new Type[]
				{
					typeof(string),
					typeof(Exception),
					typeof(string),
					typeof(object[])
				}, new object[]
				{
					text,
					null,
					text2,
					new object[]
					{
						name,
						callInfo.ArgumentCount
					}
				}), restrictions);
			}
			MethodBase method = methodInformation.method;
			Expression expression = PSInvokeMemberBinder.InvokeMethod(method, target, args, expandParameters, methodInvocationType);
			if (expression.Type == typeof(void))
			{
				expression = Expression.Block(expression, ExpressionCache.AutomationNullConstant);
			}
			if ((method.DeclaringType == typeof(SteppablePipeline) && method.Name.Equals("Begin", StringComparison.Ordinal)) || method.Name.Equals("Process", StringComparison.Ordinal) || method.Name.Equals("End", StringComparison.Ordinal))
			{
				return new DynamicMetaObject(expression, restrictions);
			}
			expression = expression.Cast(typeof(object));
			if (method.DeclaringType.GetTypeInfo().Assembly.GetCustomAttributes(typeof(DynamicClassImplementationAssemblyAttribute)).Any<Attribute>())
			{
				return new DynamicMetaObject(expression, restrictions);
			}
			ParameterExpression parameterExpression = Expression.Variable(typeof(Exception));
			expression = Expression.TryCatch(expression, new CatchBlock[]
			{
				Expression.Catch(parameterExpression, Expression.Block(expression.Type, new Expression[]
				{
					Expression.Call(CachedReflectionInfo.ExceptionHandlingOps_ConvertToMethodInvocationException, parameterExpression, Expression.Constant(errorExceptionType, typeof(Type)), Expression.Constant(method.Name), ExpressionCache.Constant(args.Length), Expression.Constant(method, typeof(MethodBase))),
					Expression.Rethrow(expression.Type)
				}))
			});
			return new DynamicMetaObject(expression, restrictions);
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0016C3EC File Offset: 0x0016A5EC
		public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject result = errorSuggestion;
			if (errorSuggestion == null)
			{
				result = new DynamicMetaObject(DynamicExpression.Dynamic(new PSInvokeBinder(base.CallInfo), typeof(object), from dmo in args.Prepend(target)
				select dmo.Expression), target.Restrictions.Merge(BindingRestrictions.Combine(args)));
			}
			return result;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0016C470 File Offset: 0x0016A670
		internal static MethodInfo FindBestMethod(DynamicMetaObject target, IEnumerable<DynamicMetaObject> args, string methodName, bool @static, PSMethodInvocationConstraints invocationConstraints)
		{
			MethodInfo result = null;
			PSMethod dotNetMethod = PSObject.dotNetInstanceAdapter.GetDotNetMethod<PSMethod>(PSObject.Base(target.Value), methodName);
			if (dotNetMethod != null)
			{
				DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)dotNetMethod.adapterData;
				string text = null;
				string text2 = null;
				bool flag;
				bool flag2;
				MethodInformation methodInformation = Adapter.FindBestMethod(methodCacheEntry.methodInformationStructures, invocationConstraints, args.Select(delegate(DynamicMetaObject arg)
				{
					if (arg.Value != AutomationNull.Value)
					{
						return arg.Value;
					}
					return null;
				}).ToArray<object>(), ref text, ref text2, out flag, out flag2);
				if (methodInformation != null)
				{
					result = (MethodInfo)methodInformation.method;
				}
			}
			return result;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0016C578 File Offset: 0x0016A778
		internal static Expression InvokeMethod(MethodBase mi, DynamicMetaObject target, DynamicMetaObject[] args, bool expandParameters, PSInvokeMemberBinder.MethodInvocationType invocationInvocationType)
		{
			List<ParameterExpression> temps = new List<ParameterExpression>();
			List<Expression> initTemps = new List<Expression>();
			List<Expression> list = new List<Expression>();
			ParameterInfo[] parameters = mi.GetParameters();
			Expression[] array = new Expression[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				string paramName = parameters[i].Name;
				if (string.IsNullOrWhiteSpace(parameters[i].Name))
				{
					paramName = i.ToString(CultureInfo.InvariantCulture);
				}
				object[] customAttributes = parameters[i].GetCustomAttributes(typeof(ParamArrayAttribute), false);
				if (customAttributes != null && customAttributes.Any<object>())
				{
					Type paramElementType = parameters[i].ParameterType.GetElementType();
					if (expandParameters)
					{
						array[i] = Expression.NewArrayInit(paramElementType, from a in args.Skip(i)
						select a.CastOrConvertMethodArgument(paramElementType, paramName, mi.Name, temps, initTemps));
					}
					else
					{
						Expression expression = args[i].CastOrConvertMethodArgument(parameters[i].ParameterType, paramName, mi.Name, temps, initTemps);
						array[i] = expression;
					}
				}
				else if (i >= args.Length)
				{
					object defaultValue = parameters[i].DefaultValue;
					if (defaultValue == null)
					{
						array[i] = Expression.Default(parameters[i].ParameterType);
					}
					else
					{
						array[i] = Expression.Constant(defaultValue);
					}
				}
				else if (parameters[i].ParameterType.IsByRef)
				{
					if (!(args[i].Value is PSReference))
					{
						return Compiler.CreateThrow(typeof(object), typeof(MethodException), new Type[]
						{
							typeof(string),
							typeof(Exception),
							typeof(string),
							typeof(object[])
						}, new object[]
						{
							"NonRefArgumentToRefParameterMsg",
							null,
							ExtendedTypeSystem.NonRefArgumentToRefParameter,
							new object[]
							{
								i + 1,
								typeof(PSReference).FullName,
								"[ref]"
							}
						});
					}
					ParameterExpression parameterExpression = Expression.Variable(parameters[i].ParameterType.GetElementType());
					temps.Add(parameterExpression);
					MemberExpression memberExpression = Expression.Property(args[i].Expression.Cast(typeof(PSReference)), CachedReflectionInfo.PSReference_Value);
					initTemps.Add(Expression.Assign(parameterExpression, memberExpression.Convert(parameterExpression.Type)));
					list.Add(Expression.Assign(memberExpression, parameterExpression.Cast(typeof(object))));
					array[i] = parameterExpression;
				}
				else
				{
					array[i] = args[i].CastOrConvertMethodArgument(parameters[i].ParameterType, paramName, mi.Name, temps, initTemps);
				}
			}
			ConstructorInfo constructorInfo = null;
			MethodInfo methodInfo = mi as MethodInfo;
			if (methodInfo == null)
			{
				constructorInfo = (ConstructorInfo)mi;
			}
			Expression expression2;
			if (constructorInfo != null)
			{
				if (invocationInvocationType == PSInvokeMemberBinder.MethodInvocationType.BaseCtor)
				{
					Expression arg = (target.Value is PSObject) ? target.Expression.Cast(constructorInfo.DeclaringType) : PSGetMemberBinder.GetTargetExpr(target, constructorInfo.DeclaringType);
					expression2 = Expression.Call(CachedReflectionInfo.ClassOps_CallBaseCtor, arg, Expression.Constant(constructorInfo, typeof(ConstructorInfo)), Expression.NewArrayInit(typeof(object), from x in array
					select x.Cast(typeof(object))));
				}
				else
				{
					expression2 = Expression.New(constructorInfo, array);
				}
			}
			else if (invocationInvocationType == PSInvokeMemberBinder.MethodInvocationType.NonVirtual && !methodInfo.IsStatic)
			{
				expression2 = Expression.Call((methodInfo.ReturnType == typeof(void)) ? CachedReflectionInfo.ClassOps_CallVoidMethodNonVirtually : CachedReflectionInfo.ClassOps_CallMethodNonVirtually, PSGetMemberBinder.GetTargetExpr(target, methodInfo.DeclaringType), Expression.Constant(methodInfo, typeof(MethodInfo)), Expression.NewArrayInit(typeof(object), from x in array
				select x.Cast(typeof(object))));
			}
			else
			{
				expression2 = (methodInfo.IsStatic ? Expression.Call(methodInfo, array) : Expression.Call(PSGetMemberBinder.GetTargetExpr(target, methodInfo.DeclaringType), methodInfo, array));
			}
			if (temps.Any<ParameterExpression>())
			{
				if (expression2.Type != typeof(void) && list.Any<Expression>())
				{
					ParameterExpression parameterExpression2 = Expression.Variable(expression2.Type);
					temps.Add(parameterExpression2);
					expression2 = Expression.Assign(parameterExpression2, expression2);
					list.Add(parameterExpression2);
				}
				expression2 = Expression.Block(expression2.Type, temps, initTemps.Append(expression2).Concat(list));
			}
			return expression2;
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0016CADC File Offset: 0x0016ACDC
		private DynamicMetaObject InvokeMemberOnCollection(DynamicMetaObject targetEnumerator, DynamicMetaObject[] args, Type typeForMessage, BindingRestrictions restrictions)
		{
			DynamicExpression dynamicExpression = DynamicExpression.Dynamic(this, this.ReturnType, (from a in args
			select a.Expression).Prepend(ExpressionCache.NullConstant));
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_MethodInvoker, Expression.Constant(this.GetNonEnumeratingBinder()), Expression.Constant(dynamicExpression.DelegateType, typeof(Type)), targetEnumerator.Expression, Expression.NewArrayInit(typeof(object), from a in args
			select a.Expression.Cast(typeof(object))), Expression.Constant(typeForMessage, typeof(Type))), targetEnumerator.Restrictions.Merge(restrictions));
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0016CBA8 File Offset: 0x0016ADA8
		private static DynamicMetaObject GetTargetAsEnumerable(DynamicMetaObject target)
		{
			DynamicMetaObject dynamicMetaObject = PSEnumerableBinder.IsEnumerable(target);
			if (dynamicMetaObject == null)
			{
				dynamicMetaObject = PSEnumerableBinder.IsEnumerable(new DynamicMetaObject(Expression.NewArrayInit(typeof(object), new Expression[]
				{
					target.Expression.Cast(typeof(object))
				}), target.GetSimpleTypeRestriction()));
			}
			return dynamicMetaObject;
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0016CC00 File Offset: 0x0016AE00
		private DynamicMetaObject InvokeWhereOnCollection(DynamicMetaObject target, DynamicMetaObject[] args, BindingRestrictions argRestrictions)
		{
			DynamicMetaObject targetAsEnumerable = PSInvokeMemberBinder.GetTargetAsEnumerable(target);
			switch (args.Length)
			{
			case 1:
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_Where, targetAsEnumerable.Expression, PSGetMemberBinder.GetTargetExpr(args[0], null).Convert(typeof(ScriptBlock)), Expression.Constant(WhereOperatorSelectionMode.Default), Expression.Constant(0)), targetAsEnumerable.Restrictions.Merge(argRestrictions));
			case 2:
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_Where, targetAsEnumerable.Expression, PSGetMemberBinder.GetTargetExpr(args[0], null).Convert(typeof(ScriptBlock)), PSGetMemberBinder.GetTargetExpr(args[1], null).Convert(typeof(WhereOperatorSelectionMode)), Expression.Constant(0)), targetAsEnumerable.Restrictions.Merge(argRestrictions));
			case 3:
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_Where, targetAsEnumerable.Expression, PSGetMemberBinder.GetTargetExpr(args[0], null).Convert(typeof(ScriptBlock)), PSGetMemberBinder.GetTargetExpr(args[1], null).Convert(typeof(WhereOperatorSelectionMode)), PSGetMemberBinder.GetTargetExpr(args[2], null).Convert(typeof(int))), targetAsEnumerable.Restrictions.Merge(argRestrictions));
			default:
				return new DynamicMetaObject(Expression.Throw(Expression.New(CachedReflectionInfo.MethodException_ctor, new Expression[]
				{
					Expression.Constant("MethodCountCouldNotFindBest"),
					Expression.Constant(null, typeof(Exception)),
					Expression.Constant(ExtendedTypeSystem.MethodArgumentCountException),
					Expression.NewArrayInit(typeof(object), new Expression[]
					{
						Expression.Constant(".Where({ expression } [, mode [, numberToReturn]])").Cast(typeof(object)),
						ExpressionCache.Constant(args.Length).Cast(typeof(object))
					})
				}), this.ReturnType), targetAsEnumerable.Restrictions.Merge(argRestrictions));
			}
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0016CE0C File Offset: 0x0016B00C
		private DynamicMetaObject InvokeForEachOnCollection(DynamicMetaObject targetEnumerator, DynamicMetaObject[] args, BindingRestrictions restrictions)
		{
			targetEnumerator = PSInvokeMemberBinder.GetTargetAsEnumerable(targetEnumerator);
			if (args.Length < 1)
			{
				return new DynamicMetaObject(Expression.Throw(Expression.New(CachedReflectionInfo.MethodException_ctor, new Expression[]
				{
					Expression.Constant("MethodCountCouldNotFindBest"),
					Expression.Constant(null, typeof(Exception)),
					Expression.Constant(ExtendedTypeSystem.MethodArgumentCountException),
					Expression.NewArrayInit(typeof(object), new Expression[]
					{
						Expression.Constant(".ForEach(expression [, arguments...])").Cast(typeof(object)),
						ExpressionCache.Constant(args.Length).Cast(typeof(object))
					})
				}), this.ReturnType), targetEnumerator.Restrictions.Merge(restrictions));
			}
			Expression expression = PSEnumerableBinder.IsEnumerable(targetEnumerator).Expression;
			Expression arg;
			if (args.Length > 1)
			{
				arg = Expression.NewArrayInit(typeof(object), from a in args.Skip(1)
				select a.Expression.Cast(typeof(object)));
			}
			else
			{
				arg = Expression.NewArrayInit(typeof(object), new Expression[0]);
			}
			return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.EnumerableOps_ForEach, expression, PSGetMemberBinder.GetTargetExpr(args[0], typeof(object)), arg), targetEnumerator.Restrictions.Merge(restrictions));
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0016CF96 File Offset: 0x0016B196
		internal static bool IsHomogenousArray<T>(object[] args)
		{
			return args.Length != 0 && args.All(delegate(object element)
			{
				object obj = PSObject.Base(element);
				return obj != null && obj.GetType().Equals(typeof(T));
			});
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x0016CFEC File Offset: 0x0016B1EC
		internal static bool IsHeterogeneousArray(object[] args)
		{
			if (args.Length == 0)
			{
				return true;
			}
			object obj = PSObject.Base(args[0]);
			if (obj == null)
			{
				return true;
			}
			Type firstType = obj.GetType();
			return firstType.Equals(typeof(object)) || args.Skip(1).Any(delegate(object element)
			{
				object obj2 = PSObject.Base(element);
				return obj2 == null || !firstType.Equals(obj2.GetType());
			});
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0016D054 File Offset: 0x0016B254
		internal static object InvokeAdaptedMember(object obj, string methodName, object[] args)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			PSObject.AdapterSet mappedAdapter = PSObject.GetMappedAdapter(obj, (executionContextFromTLS != null) ? executionContextFromTLS.TypeTable : null);
			PSMethodInfo psmethodInfo = mappedAdapter.OriginalAdapter.BaseGetMember<PSMemberInfo>(obj, methodName) as PSMethodInfo;
			if (psmethodInfo == null && mappedAdapter.DotNetAdapter != null)
			{
				psmethodInfo = (mappedAdapter.DotNetAdapter.BaseGetMember<PSMemberInfo>(obj, methodName) as PSMethodInfo);
			}
			if (psmethodInfo != null)
			{
				return psmethodInfo.Invoke(args);
			}
			throw InterpreterError.NewInterpreterException(methodName, typeof(RuntimeException), null, "MethodNotFound", ParserStrings.MethodNotFound, new object[]
			{
				ParserOps.GetTypeFullName(obj),
				methodName
			});
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0016D0E8 File Offset: 0x0016B2E8
		internal static object InvokeAdaptedSetMember(object obj, string methodName, object[] args, object valueToSet)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			PSObject.AdapterSet mappedAdapter = PSObject.GetMappedAdapter(obj, (executionContextFromTLS != null) ? executionContextFromTLS.TypeTable : null);
			PSParameterizedProperty psparameterizedProperty = mappedAdapter.OriginalAdapter.BaseGetMember<PSParameterizedProperty>(obj, methodName);
			if (psparameterizedProperty == null && mappedAdapter.DotNetAdapter != null)
			{
				psparameterizedProperty = mappedAdapter.DotNetAdapter.BaseGetMember<PSParameterizedProperty>(obj, methodName);
			}
			if (psparameterizedProperty != null)
			{
				psparameterizedProperty.InvokeSet(valueToSet, args);
				return valueToSet;
			}
			throw InterpreterError.NewInterpreterException(methodName, typeof(RuntimeException), null, "MethodNotFound", ParserStrings.MethodNotFound, new object[]
			{
				ParserOps.GetTypeFullName(obj),
				methodName
			});
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0016D174 File Offset: 0x0016B374
		internal static bool TryGetInstanceMethod(object value, string memberName, out PSMethodInfo methodInfo)
		{
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
			PSMemberInfo psmemberInfo = PSObject.HasInstanceMembers(value, out psmemberInfoInternalCollection) ? psmemberInfoInternalCollection[memberName] : null;
			methodInfo = (psmemberInfo as PSMethodInfo);
			if (psmemberInfo == null)
			{
				return false;
			}
			if (methodInfo == null)
			{
				throw InterpreterError.NewInterpreterException(memberName, typeof(RuntimeException), null, "MethodNotFound", ParserStrings.MethodNotFound, new object[]
				{
					ParserOps.GetTypeFullName(value),
					memberName
				});
			}
			return true;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0016D1DC File Offset: 0x0016B3DC
		internal static void InvalidateCache()
		{
			lock (PSInvokeMemberBinder._binderCache)
			{
				foreach (PSInvokeMemberBinder psinvokeMemberBinder in PSInvokeMemberBinder._binderCache.Values)
				{
					psinvokeMemberBinder._getMemberBinder._version++;
				}
			}
		}

		// Token: 0x040021E8 RID: 8680
		private static readonly Dictionary<Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type>, PSInvokeMemberBinder> _binderCache = new Dictionary<Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type>, PSInvokeMemberBinder>(new PSInvokeMemberBinder.KeyComparer());

		// Token: 0x040021E9 RID: 8681
		internal readonly PSMethodInvocationConstraints _invocationConstraints;

		// Token: 0x040021EA RID: 8682
		internal readonly PSGetMemberBinder _getMemberBinder;

		// Token: 0x040021EB RID: 8683
		private readonly bool _static;

		// Token: 0x040021EC RID: 8684
		private readonly bool _propertySetter;

		// Token: 0x040021ED RID: 8685
		private readonly bool _nonEnumerating;

		// Token: 0x040021EE RID: 8686
		private readonly Type _classScope;

		// Token: 0x02000621 RID: 1569
		internal enum MethodInvocationType
		{
			// Token: 0x040021FF RID: 8703
			Ordinary,
			// Token: 0x04002200 RID: 8704
			Setter,
			// Token: 0x04002201 RID: 8705
			Getter,
			// Token: 0x04002202 RID: 8706
			BaseCtor,
			// Token: 0x04002203 RID: 8707
			NonVirtual
		}

		// Token: 0x02000622 RID: 1570
		private class KeyComparer : IEqualityComparer<Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type>>
		{
			// Token: 0x0600445C RID: 17500 RVA: 0x0016D27C File Offset: 0x0016B47C
			public bool Equals(Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type> x, Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type> y)
			{
				return x.Item1.Equals(y.Item1, StringComparison.OrdinalIgnoreCase) && x.Item2.Equals(y.Item2) && x.Item3 == y.Item3 && x.Item4 == y.Item4 && ((x.Item5 == null) ? (y.Item5 == null) : x.Item5.Equals(y.Item5)) && x.Item6 == y.Item6 && x.Item7 == y.Item7;
			}

			// Token: 0x0600445D RID: 17501 RVA: 0x0016D314 File Offset: 0x0016B514
			public int GetHashCode(Tuple<string, CallInfo, bool, bool, PSMethodInvocationConstraints, bool, Type> obj)
			{
				return Utils.CombineHashCodes(StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item1), obj.Item2.GetHashCode(), obj.Item3.GetHashCode(), obj.Item4.GetHashCode(), (obj.Item5 == null) ? 0 : obj.Item5.GetHashCode(), obj.Item6.GetHashCode(), (obj.Item7 == null) ? 0 : obj.Item7.GetHashCode());
			}
		}
	}
}
