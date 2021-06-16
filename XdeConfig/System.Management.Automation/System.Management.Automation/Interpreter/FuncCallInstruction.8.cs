using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200067E RID: 1662
	internal sealed class FuncCallInstruction<T0, T1, T2, T3, T4, T5, T6, TRet> : CallInstruction
	{
		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x00179DC3 File Offset: 0x00177FC3
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x00179DD0 File Offset: 0x00177FD0
		public override int ArgumentCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x00179DD3 File Offset: 0x00177FD3
		public FuncCallInstruction(Func<T0, T1, T2, T3, T4, T5, T6, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x00179DE2 File Offset: 0x00177FE2
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, T3, T4, T5, T6, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, T3, T4, T5, T6, TRet>));
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x00179E08 File Offset: 0x00178008
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5), (arg6 != null) ? ((T6)((object)arg6)) : default(T6));
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x00179EBC File Offset: 0x001780BC
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 7] = this._target((T0)((object)frame.Data[frame.StackIndex - 7]), (T1)((object)frame.Data[frame.StackIndex - 6]), (T2)((object)frame.Data[frame.StackIndex - 5]), (T3)((object)frame.Data[frame.StackIndex - 4]), (T4)((object)frame.Data[frame.StackIndex - 3]), (T5)((object)frame.Data[frame.StackIndex - 2]), (T6)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 6;
			return 1;
		}

		// Token: 0x040022B9 RID: 8889
		private readonly Func<T0, T1, T2, T3, T4, T5, T6, TRet> _target;
	}
}
