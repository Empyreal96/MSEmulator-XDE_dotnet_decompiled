using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200067D RID: 1661
	internal sealed class FuncCallInstruction<T0, T1, T2, T3, T4, T5, TRet> : CallInstruction
	{
		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x00179C2B File Offset: 0x00177E2B
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x060046A6 RID: 18086 RVA: 0x00179C38 File Offset: 0x00177E38
		public override int ArgumentCount
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x00179C3B File Offset: 0x00177E3B
		public FuncCallInstruction(Func<T0, T1, T2, T3, T4, T5, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x00179C4A File Offset: 0x00177E4A
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, T3, T4, T5, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, T3, T4, T5, TRet>));
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x00179C70 File Offset: 0x00177E70
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5));
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x00179D10 File Offset: 0x00177F10
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 6] = this._target((T0)((object)frame.Data[frame.StackIndex - 6]), (T1)((object)frame.Data[frame.StackIndex - 5]), (T2)((object)frame.Data[frame.StackIndex - 4]), (T3)((object)frame.Data[frame.StackIndex - 3]), (T4)((object)frame.Data[frame.StackIndex - 2]), (T5)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 5;
			return 1;
		}

		// Token: 0x040022B8 RID: 8888
		private readonly Func<T0, T1, T2, T3, T4, T5, TRet> _target;
	}
}
