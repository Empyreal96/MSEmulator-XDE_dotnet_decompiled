using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000678 RID: 1656
	internal sealed class FuncCallInstruction<T0, TRet> : CallInstruction
	{
		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x001796B3 File Offset: 0x001778B3
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x001796C0 File Offset: 0x001778C0
		public override int ArgumentCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x001796C3 File Offset: 0x001778C3
		public FuncCallInstruction(Func<T0, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x001796D2 File Offset: 0x001778D2
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, TRet>)target.CreateDelegate(typeof(Func<T0, TRet>));
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x001796F8 File Offset: 0x001778F8
		public override object Invoke(object arg0)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0));
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x0017972C File Offset: 0x0017792C
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 1] = this._target((T0)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex = frame.StackIndex;
			return 1;
		}

		// Token: 0x040022B3 RID: 8883
		private readonly Func<T0, TRet> _target;
	}
}
