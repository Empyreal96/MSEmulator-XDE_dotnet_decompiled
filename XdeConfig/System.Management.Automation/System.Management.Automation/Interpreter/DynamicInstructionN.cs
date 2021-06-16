using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200069B RID: 1691
	internal sealed class DynamicInstructionN : Instruction
	{
		// Token: 0x06004729 RID: 18217 RVA: 0x0017B22C File Offset: 0x0017942C
		public DynamicInstructionN(Type delegateType, CallSite site)
		{
			MethodInfo method = delegateType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			this._target = CallInstruction.Create(method, parameters);
			this._site = site;
			this._argumentCount = parameters.Length - 1;
			this._targetDelegate = site.GetType().GetField("Target").GetValue(site);
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0017B28D File Offset: 0x0017948D
		public DynamicInstructionN(Type delegateType, CallSite site, bool isVoid) : this(delegateType, site)
		{
			this._isVoid = isVoid;
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x0600472B RID: 18219 RVA: 0x0017B29E File Offset: 0x0017949E
		public override int ProducedStack
		{
			get
			{
				if (!this._isVoid)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x0600472C RID: 18220 RVA: 0x0017B2AB File Offset: 0x001794AB
		public override int ConsumedStack
		{
			get
			{
				return this._argumentCount;
			}
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0017B2B4 File Offset: 0x001794B4
		public override int Run(InterpretedFrame frame)
		{
			int num = frame.StackIndex - this._argumentCount;
			object[] array = new object[1 + this._argumentCount];
			array[0] = this._site;
			for (int i = 0; i < this._argumentCount; i++)
			{
				array[1 + i] = frame.Data[num + i];
			}
			object obj = this._target.InvokeInstance(this._targetDelegate, array);
			if (this._isVoid)
			{
				frame.StackIndex = num;
			}
			else
			{
				frame.Data[num] = obj;
				frame.StackIndex = num + 1;
			}
			return 1;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0017B33D File Offset: 0x0017953D
		public override string ToString()
		{
			return "DynamicInstructionN(" + this._site + ")";
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0017B354 File Offset: 0x00179554
		internal static Type GetDynamicInstructionType(Type delegateType)
		{
			Type[] genericArguments = delegateType.GetGenericArguments();
			if (genericArguments.Length == 0)
			{
				return null;
			}
			Type[] array = genericArguments.Skip(1).ToArray<Type>();
			Type typeFromHandle;
			switch (array.Length)
			{
			case 1:
				typeFromHandle = typeof(DynamicInstruction<>);
				break;
			case 2:
				typeFromHandle = typeof(DynamicInstruction<, >);
				break;
			case 3:
				typeFromHandle = typeof(DynamicInstruction<, , >);
				break;
			case 4:
				typeFromHandle = typeof(DynamicInstruction<, , , >);
				break;
			case 5:
				typeFromHandle = typeof(DynamicInstruction<, , , , >);
				break;
			case 6:
				typeFromHandle = typeof(DynamicInstruction<, , , , , >);
				break;
			case 7:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , >);
				break;
			case 8:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , >);
				break;
			case 9:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , >);
				break;
			case 10:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , >);
				break;
			case 11:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , , >);
				break;
			case 12:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , , , >);
				break;
			case 13:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , , , , >);
				break;
			case 14:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , , , , , >);
				break;
			case 15:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , , , , , , >);
				break;
			case 16:
				typeFromHandle = typeof(DynamicInstruction<, , , , , , , , , , , , , , , >);
				break;
			default:
				throw Assert.Unreachable;
			}
			return typeFromHandle.MakeGenericType(array);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0017B4BC File Offset: 0x001796BC
		internal static Instruction CreateUntypedInstruction(CallSiteBinder binder, int argCount)
		{
			switch (argCount)
			{
			case 0:
				return DynamicInstruction<object>.Factory(binder);
			case 1:
				return DynamicInstruction<object, object>.Factory(binder);
			case 2:
				return DynamicInstruction<object, object, object>.Factory(binder);
			case 3:
				return DynamicInstruction<object, object, object, object>.Factory(binder);
			case 4:
				return DynamicInstruction<object, object, object, object, object>.Factory(binder);
			case 5:
				return DynamicInstruction<object, object, object, object, object, object>.Factory(binder);
			case 6:
				return DynamicInstruction<object, object, object, object, object, object, object>.Factory(binder);
			case 7:
				return DynamicInstruction<object, object, object, object, object, object, object, object>.Factory(binder);
			case 8:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 9:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 10:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 11:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 12:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 13:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 14:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			case 15:
				return DynamicInstruction<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>.Factory(binder);
			default:
				return null;
			}
		}

		// Token: 0x040022ED RID: 8941
		private readonly CallInstruction _target;

		// Token: 0x040022EE RID: 8942
		private readonly object _targetDelegate;

		// Token: 0x040022EF RID: 8943
		private readonly CallSite _site;

		// Token: 0x040022F0 RID: 8944
		private readonly int _argumentCount;

		// Token: 0x040022F1 RID: 8945
		private readonly bool _isVoid;
	}
}
