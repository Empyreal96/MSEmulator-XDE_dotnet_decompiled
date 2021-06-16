using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x02000628 RID: 1576
	public static class ClassOps
	{
		// Token: 0x06004480 RID: 17536 RVA: 0x0016DF9C File Offset: 0x0016C19C
		public static void ValidateSetProperty(Type type, string propertyName, object value)
		{
			IEnumerable<ValidateArgumentsAttribute> customAttributes = type.GetProperty(propertyName).GetCustomAttributes<ValidateArgumentsAttribute>();
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			EngineIntrinsics engineIntrinsics = (executionContextFromTLS == null) ? null : executionContextFromTLS.EngineIntrinsics;
			foreach (ValidateArgumentsAttribute validateArgumentsAttribute in customAttributes)
			{
				validateArgumentsAttribute.InternalValidate(value, engineIntrinsics);
			}
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0016E00C File Offset: 0x0016C20C
		public static void CallBaseCtor(object target, ConstructorInfo ci, object[] args)
		{
			ci.Invoke(target, args);
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0016E017 File Offset: 0x0016C217
		public static object CallMethodNonVirtually(object target, MethodInfo mi, object[] args)
		{
			return ClassOps.CallMethodNonVirtuallyImpl(target, mi, args);
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0016E021 File Offset: 0x0016C221
		public static void CallVoidMethodNonVirtually(object target, MethodInfo mi, object[] args)
		{
			ClassOps.CallMethodNonVirtuallyImpl(target, mi, args);
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0016E02C File Offset: 0x0016C22C
		private static object CallMethodNonVirtuallyImpl(object target, MethodInfo mi, object[] args)
		{
			DynamicMethod value = ClassOps.NonVirtualCallCache.GetValue(mi, new ConditionalWeakTable<MethodInfo, DynamicMethod>.CreateValueCallback(ClassOps.CreateDynamicMethod));
			List<object> list = new List<object>(args.Length + 1)
			{
				target
			};
			list.AddRange(args);
			return value.Invoke(null, list.ToArray());
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0016E084 File Offset: 0x0016C284
		private static DynamicMethod CreateDynamicMethod(MethodInfo mi)
		{
			List<Type> list = new List<Type>
			{
				mi.DeclaringType
			};
			list.AddRange(from x in mi.GetParameters()
			select x.ParameterType);
			DynamicMethod dynamicMethod = new DynamicMethod("PSNonVirtualCall_" + mi.Name, mi.ReturnType, list.ToArray(), mi.DeclaringType);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			for (int i = 0; i < list.Count; i++)
			{
				ilgenerator.Emit(OpCodes.Ldarg, i);
			}
			ilgenerator.Emit(OpCodes.Tailcall);
			ilgenerator.EmitCall(OpCodes.Call, mi, null);
			ilgenerator.Emit(OpCodes.Ret);
			return dynamicMethod;
		}

		// Token: 0x04002212 RID: 8722
		private static readonly ConditionalWeakTable<MethodInfo, DynamicMethod> NonVirtualCallCache = new ConditionalWeakTable<MethodInfo, DynamicMethod>();
	}
}
