using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000102 RID: 258
	[DebuggerDisplay("MethodInformation: {methodDefinition}")]
	internal class MethodInformation
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0004DD4C File Offset: 0x0004BF4C
		internal string methodDefinition
		{
			get
			{
				if (this._cachedMethodDefinition == null)
				{
					string memberName = (this.method is ConstructorInfo) ? "new" : this.method.Name;
					string methodInfoOverloadDefinition = DotNetAdapter.GetMethodInfoOverloadDefinition(memberName, this.method, this.method.GetParameters().Length - this.parameters.Length);
					Interlocked.CompareExchange<string>(ref this._cachedMethodDefinition, methodInfoOverloadDefinition, null);
				}
				return this._cachedMethodDefinition;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0004DDB8 File Offset: 0x0004BFB8
		internal MethodInformation(MethodBase method, int parametersToIgnore)
		{
			this.method = method;
			this.isGeneric = method.IsGenericMethod;
			ParameterInfo[] array = method.GetParameters();
			int num = array.Length - parametersToIgnore;
			this.parameters = new ParameterInformation[num];
			for (int i = 0; i < num; i++)
			{
				this.parameters[i] = new ParameterInformation(array[i]);
				if (array[i].IsOptional)
				{
					this.hasOptional = true;
				}
			}
			this.hasVarArgs = false;
			if (num > 0)
			{
				ParameterInfo parameterInfo = array[num - 1];
				if (!this.hasOptional && parameterInfo.ParameterType.IsArray)
				{
					object[] customAttributes = parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false);
					if (customAttributes != null && customAttributes.Any<object>())
					{
						this.hasVarArgs = true;
						this.parameters[num - 1].isParamArray = true;
					}
				}
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0004DE81 File Offset: 0x0004C081
		internal MethodInformation(bool hasvarargs, bool hasoptional, ParameterInformation[] arguments)
		{
			this.hasVarArgs = hasvarargs;
			this.hasOptional = hasoptional;
			this.parameters = arguments;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0004DEA0 File Offset: 0x0004C0A0
		internal object Invoke(object target, object[] arguments)
		{
			if (target is PSObject && !this.method.DeclaringType.IsAssignableFrom(target.GetType()))
			{
				target = PSObject.Base(target);
			}
			if (!this.useReflection)
			{
				if (this.methodInvoker == null)
				{
					if (!(this.method is MethodInfo))
					{
						this.useReflection = true;
					}
					else
					{
						this.methodInvoker = this.GetMethodInvoker(this.method as MethodInfo);
					}
				}
				if (this.methodInvoker != null)
				{
					return this.methodInvoker(target, arguments);
				}
			}
			return this.method.Invoke(target, arguments);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0004DF35 File Offset: 0x0004C135
		private static void EmitLdc(ILGenerator emitter, int c)
		{
			if (c < MethodInformation._ldc.Length)
			{
				emitter.Emit(MethodInformation._ldc[c]);
				return;
			}
			emitter.Emit(OpCodes.Ldc_I4, c);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0004DF64 File Offset: 0x0004C164
		private static bool CompareMethodParameters(MethodBase method1, MethodBase method2)
		{
			ParameterInfo[] array = method1.GetParameters();
			ParameterInfo[] array2 = method2.GetParameters();
			if (array.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ParameterType != array2[i].ParameterType)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0004DFB4 File Offset: 0x0004C1B4
		private static Type FindInterfaceForMethod(MethodInfo method, out MethodInfo methodToCall)
		{
			methodToCall = null;
			Type declaringType = method.DeclaringType;
			foreach (Type type in declaringType.GetInterfaces())
			{
				MethodInfo methodInfo = type.GetMethod(method.Name, BindingFlags.Instance);
				if (methodInfo != null && MethodInformation.CompareMethodParameters(methodInfo, method))
				{
					methodToCall = methodInfo;
					return type;
				}
			}
			return null;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0004E010 File Offset: 0x0004C210
		private MethodInformation.MethodInvoker GetMethodInvoker(MethodInfo method)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			MethodInfo meth = method;
			DynamicMethod dynamicMethod = new DynamicMethod(method.Name, typeof(object), new Type[]
			{
				typeof(object),
				typeof(object[])
			}, typeof(Adapter).GetTypeInfo().Module, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ParameterInfo[] array = method.GetParameters();
			int num = 0;
			if (!method.IsStatic && method.DeclaringType.GetTypeInfo().IsValueType && !method.IsVirtual)
			{
				flag = true;
				num++;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsOut || array[i].ParameterType.IsByRef)
				{
					flag2 = true;
					num++;
				}
			}
			LocalBuilder[] array2 = null;
			Type returnType = method.ReturnType;
			int num2;
			if (num > 0)
			{
				if (flag2 && returnType != typeof(void))
				{
					num++;
					flag3 = true;
				}
				array2 = new LocalBuilder[num];
				num2 = 0;
				if (flag)
				{
					Type type = method.DeclaringType;
					array2[num2] = ilgenerator.DeclareLocal(type);
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Unbox_Any, type);
					ilgenerator.Emit(OpCodes.Stloc, array2[num2]);
					num2++;
				}
				for (int j = 0; j < array.Length; j++)
				{
					Type type = array[j].ParameterType;
					if (array[j].IsOut || type.IsByRef)
					{
						if (type.IsByRef)
						{
							type = type.GetElementType();
						}
						array2[num2] = ilgenerator.DeclareLocal(type);
						ilgenerator.Emit(OpCodes.Ldarg_1);
						MethodInformation.EmitLdc(ilgenerator, j);
						ilgenerator.Emit(OpCodes.Ldelem_Ref);
						if (type.GetTypeInfo().IsValueType)
						{
							ilgenerator.Emit(OpCodes.Unbox_Any, type);
						}
						else if (type != typeof(object))
						{
							ilgenerator.Emit(OpCodes.Castclass, type);
						}
						ilgenerator.Emit(OpCodes.Stloc, array2[num2]);
						num2++;
					}
				}
				if (flag3)
				{
					array2[num2] = ilgenerator.DeclareLocal(returnType);
				}
			}
			num2 = 0;
			if (!method.IsStatic)
			{
				if (method.DeclaringType.GetTypeInfo().IsValueType)
				{
					if (method.IsVirtual)
					{
						Type type = MethodInformation.FindInterfaceForMethod(method, out meth);
						if (type == null)
						{
							this.useReflection = true;
							return null;
						}
						ilgenerator.Emit(OpCodes.Ldarg_0);
						ilgenerator.Emit(OpCodes.Castclass, type);
					}
					else
					{
						ilgenerator.Emit(OpCodes.Ldloca, array2[num2]);
						num2++;
					}
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldarg_0);
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				Type type = array[j].ParameterType;
				if (type.IsByRef)
				{
					ilgenerator.Emit(OpCodes.Ldloca, array2[num2]);
					num2++;
				}
				else if (array[j].IsOut)
				{
					ilgenerator.Emit(OpCodes.Ldloc, array2[num2]);
					num2++;
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldarg_1);
					MethodInformation.EmitLdc(ilgenerator, j);
					ilgenerator.Emit(OpCodes.Ldelem_Ref);
					if (type.GetTypeInfo().IsValueType)
					{
						ilgenerator.Emit(OpCodes.Unbox_Any, type);
					}
					else if (type != typeof(object))
					{
						ilgenerator.Emit(OpCodes.Castclass, type);
					}
				}
			}
			ilgenerator.Emit(method.IsStatic ? OpCodes.Call : OpCodes.Callvirt, meth);
			if (flag3)
			{
				ilgenerator.Emit(OpCodes.Stloc, array2[array2.Length - 1]);
			}
			if (flag2)
			{
				num2 = (flag ? 1 : 0);
				for (int j = 0; j < array.Length; j++)
				{
					Type type = array[j].ParameterType;
					if (array[j].IsOut || type.IsByRef)
					{
						if (type.IsByRef)
						{
							type = type.GetElementType();
						}
						ilgenerator.Emit(OpCodes.Ldarg_1);
						MethodInformation.EmitLdc(ilgenerator, j);
						ilgenerator.Emit(OpCodes.Ldloc, array2[num2]);
						if (type.GetTypeInfo().IsValueType)
						{
							ilgenerator.Emit(OpCodes.Box, type);
						}
						ilgenerator.Emit(OpCodes.Stelem_Ref);
						num2++;
					}
				}
			}
			if (returnType == typeof(void))
			{
				ilgenerator.Emit(OpCodes.Ldnull);
			}
			else
			{
				if (flag3)
				{
					ilgenerator.Emit(OpCodes.Ldloc, array2[array2.Length - 1]);
				}
				Adapter.DoBoxingIfNecessary(ilgenerator, returnType);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (MethodInformation.MethodInvoker)dynamicMethod.CreateDelegate(typeof(MethodInformation.MethodInvoker));
		}

		// Token: 0x0400064C RID: 1612
		internal MethodBase method;

		// Token: 0x0400064D RID: 1613
		private string _cachedMethodDefinition;

		// Token: 0x0400064E RID: 1614
		internal ParameterInformation[] parameters;

		// Token: 0x0400064F RID: 1615
		internal bool hasVarArgs;

		// Token: 0x04000650 RID: 1616
		internal bool hasOptional;

		// Token: 0x04000651 RID: 1617
		internal bool isGeneric;

		// Token: 0x04000652 RID: 1618
		private bool useReflection;

		// Token: 0x04000653 RID: 1619
		private MethodInformation.MethodInvoker methodInvoker;

		// Token: 0x04000654 RID: 1620
		private static OpCode[] _ldc = new OpCode[]
		{
			OpCodes.Ldc_I4_0,
			OpCodes.Ldc_I4_1,
			OpCodes.Ldc_I4_2,
			OpCodes.Ldc_I4_3,
			OpCodes.Ldc_I4_4,
			OpCodes.Ldc_I4_5,
			OpCodes.Ldc_I4_6,
			OpCodes.Ldc_I4_7,
			OpCodes.Ldc_I4_8
		};

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x06000E53 RID: 3667
		private delegate object MethodInvoker(object target, object[] arguments);
	}
}
