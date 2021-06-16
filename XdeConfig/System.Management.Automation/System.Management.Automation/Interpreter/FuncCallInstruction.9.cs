using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200067F RID: 1663
	internal sealed class FuncCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7, TRet> : CallInstruction
	{
		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x060046B1 RID: 18097 RVA: 0x00179F83 File Offset: 0x00178183
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x00179F90 File Offset: 0x00178190
		public override int ArgumentCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x00179F93 File Offset: 0x00178193
		public FuncCallInstruction(Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x00179FA2 File Offset: 0x001781A2
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet>));
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x00179FC8 File Offset: 0x001781C8
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5), (arg6 != null) ? ((T6)((object)arg6)) : default(T6), (arg7 != null) ? ((T7)((object)arg7)) : default(T7));
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0017A094 File Offset: 0x00178294
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 8] = this._target((T0)((object)frame.Data[frame.StackIndex - 8]), (T1)((object)frame.Data[frame.StackIndex - 7]), (T2)((object)frame.Data[frame.StackIndex - 6]), (T3)((object)frame.Data[frame.StackIndex - 5]), (T4)((object)frame.Data[frame.StackIndex - 4]), (T5)((object)frame.Data[frame.StackIndex - 3]), (T6)((object)frame.Data[frame.StackIndex - 2]), (T7)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 7;
			return 1;
		}

		// Token: 0x040022BA RID: 8890
		private readonly Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet> _target;
	}
}
