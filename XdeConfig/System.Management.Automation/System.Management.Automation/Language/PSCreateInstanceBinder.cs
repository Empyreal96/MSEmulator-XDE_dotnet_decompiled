using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000623 RID: 1571
	internal class PSCreateInstanceBinder : CreateInstanceBinder
	{
		// Token: 0x0600445F RID: 17503 RVA: 0x0016D3A8 File Offset: 0x0016B5A8
		public static PSCreateInstanceBinder Get(CallInfo callInfo, PSMethodInvocationConstraints constraints, bool publicTypeOnly = false)
		{
			PSCreateInstanceBinder pscreateInstanceBinder;
			lock (PSCreateInstanceBinder._binderCache)
			{
				Tuple<CallInfo, PSMethodInvocationConstraints, bool> key = Tuple.Create<CallInfo, PSMethodInvocationConstraints, bool>(callInfo, constraints, publicTypeOnly);
				if (!PSCreateInstanceBinder._binderCache.TryGetValue(key, out pscreateInstanceBinder))
				{
					pscreateInstanceBinder = new PSCreateInstanceBinder(callInfo, constraints, publicTypeOnly);
					PSCreateInstanceBinder._binderCache.Add(key, pscreateInstanceBinder);
				}
			}
			return pscreateInstanceBinder;
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0016D410 File Offset: 0x0016B610
		internal static void InvalidateCache()
		{
			lock (PSCreateInstanceBinder._binderCache)
			{
				foreach (PSCreateInstanceBinder pscreateInstanceBinder in PSCreateInstanceBinder._binderCache.Values)
				{
					pscreateInstanceBinder._version++;
				}
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0016D498 File Offset: 0x0016B698
		internal PSCreateInstanceBinder(CallInfo callInfo, PSMethodInvocationConstraints constraints, bool publicTypeOnly) : base(callInfo)
		{
			this._publicTypeOnly = publicTypeOnly;
			this._callInfo = callInfo;
			this._constraints = constraints;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0016D4B8 File Offset: 0x0016B6B8
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PSCreateInstanceBinder: ver:{0} args:{1} constraints:<{2}>", new object[]
			{
				this._version,
				this._callInfo.ArgumentCount,
				(this._constraints != null) ? this._constraints.ToString() : ""
			});
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0016D534 File Offset: 0x0016B734
		public override DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			if (target.HasValue)
			{
				if (!args.Any((DynamicMetaObject arg) => !arg.HasValue))
				{
					object obj = PSObject.Base(target.Value);
					if (obj == null)
					{
						return target.ThrowRuntimeError(args, BindingRestrictions.Empty, "InvokeMethodOnNull", ParserStrings.InvokeMethodOnNull, new Expression[0]).WriteToDebugLog(this);
					}
					Type type = (obj as Type) ?? obj.GetType();
					TypeInfo typeInfo = type.GetTypeInfo();
					BindingRestrictions bindingRestrictions;
					if (this._publicTypeOnly && !TypeResolver.IsPublic(typeInfo))
					{
						bindingRestrictions = BindingRestrictions.GetExpressionRestriction(Expression.Call(CachedReflectionInfo.PSCreateInstanceBinder_IsTargetTypeNonPublic, target.Expression));
						return target.ThrowRuntimeError(bindingRestrictions, "MethodNotFound", ParserStrings.MethodNotFound, new Expression[]
						{
							Expression.Call(CachedReflectionInfo.PSCreateInstanceBinder_GetTargetTypeName, target.Expression),
							Expression.Constant("new")
						}).WriteToDebugLog(this);
					}
					ConstructorInfo[] constructors = type.GetConstructors();
					bindingRestrictions = (object.ReferenceEquals(type, obj) ? ((target.Value is PSObject) ? BindingRestrictions.GetInstanceRestriction(Expression.Call(CachedReflectionInfo.PSObject_Base, target.Expression), type) : BindingRestrictions.GetInstanceRestriction(target.Expression, type)) : target.PSGetTypeRestriction());
					bindingRestrictions = bindingRestrictions.Merge(BinderUtils.GetOptionalVersionAndLanguageCheckForType(this, type, this._version));
					if (constructors.Length == 0 && this._callInfo.ArgumentCount == 0 && typeInfo.IsValueType)
					{
						return new DynamicMetaObject(Expression.New(type).Cast(this.ReturnType), bindingRestrictions).WriteToDebugLog(this);
					}
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					if (executionContextFromTLS != null && executionContextFromTLS.LanguageMode == PSLanguageMode.ConstrainedLanguage && !CoreTypes.Contains(type))
					{
						return target.ThrowRuntimeError(bindingRestrictions, "CannotCreateTypeConstrainedLanguage", ParserStrings.CannotCreateTypeConstrainedLanguage, new Expression[0]).WriteToDebugLog(this);
					}
					bindingRestrictions = args.Aggregate(bindingRestrictions, (BindingRestrictions current, DynamicMetaObject arg) => current.Merge(arg.PSGetMethodArgumentRestriction()));
					MethodInformation[] methodInformationArray = DotNetAdapter.GetMethodInformationArray(constructors);
					return PSInvokeMemberBinder.InvokeDotNetMethod(this._callInfo, "new", this._constraints, PSInvokeMemberBinder.MethodInvocationType.Ordinary, target, args, bindingRestrictions, methodInformationArray, typeof(MethodException)).WriteToDebugLog(this);
				}
			}
			return base.Defer(args.Prepend(target).ToArray<DynamicMetaObject>());
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0016D764 File Offset: 0x0016B964
		internal static bool IsTargetTypeNonPublic(object target)
		{
			object obj = PSObject.Base(target);
			if (obj == null)
			{
				return false;
			}
			Type type = (obj as Type) ?? obj.GetType();
			return !TypeResolver.IsPublic(type);
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0016D798 File Offset: 0x0016B998
		internal static string GetTargetTypeName(object target)
		{
			object obj = PSObject.Base(target);
			Type type = (obj as Type) ?? obj.GetType();
			return type.FullName;
		}

		// Token: 0x04002204 RID: 8708
		private readonly CallInfo _callInfo;

		// Token: 0x04002205 RID: 8709
		private readonly PSMethodInvocationConstraints _constraints;

		// Token: 0x04002206 RID: 8710
		private readonly bool _publicTypeOnly;

		// Token: 0x04002207 RID: 8711
		private int _version;

		// Token: 0x04002208 RID: 8712
		private static readonly Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints, bool>, PSCreateInstanceBinder> _binderCache = new Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints, bool>, PSCreateInstanceBinder>(new PSCreateInstanceBinder.KeyComparer());

		// Token: 0x02000624 RID: 1572
		private class KeyComparer : IEqualityComparer<Tuple<CallInfo, PSMethodInvocationConstraints, bool>>
		{
			// Token: 0x06004469 RID: 17513 RVA: 0x0016D7D4 File Offset: 0x0016B9D4
			public bool Equals(Tuple<CallInfo, PSMethodInvocationConstraints, bool> x, Tuple<CallInfo, PSMethodInvocationConstraints, bool> y)
			{
				return x.Item1.Equals(y.Item1) && ((x.Item2 == null) ? (y.Item2 == null) : x.Item2.Equals(y.Item2)) && x.Item3 == y.Item3;
			}

			// Token: 0x0600446A RID: 17514 RVA: 0x0016D82A File Offset: 0x0016BA2A
			public int GetHashCode(Tuple<CallInfo, PSMethodInvocationConstraints, bool> obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
