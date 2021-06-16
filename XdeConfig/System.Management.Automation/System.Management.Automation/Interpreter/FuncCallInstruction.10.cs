using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000680 RID: 1664
	internal sealed class FuncCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> : CallInstruction
	{
		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x0017A16F File Offset: 0x0017836F
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x060046B8 RID: 18104 RVA: 0x0017A17C File Offset: 0x0017837C
		public override int ArgumentCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x0017A180 File Offset: 0x00178380
		public FuncCallInstruction(Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x0017A18F File Offset: 0x0017838F
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>));
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x0017A1B4 File Offset: 0x001783B4
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5), (arg6 != null) ? ((T6)((object)arg6)) : default(T6), (arg7 != null) ? ((T7)((object)arg7)) : default(T7), (arg8 != null) ? ((T8)((object)arg8)) : default(T8));
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x0017A298 File Offset: 0x00178498
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 9] = this._target((T0)((object)frame.Data[frame.StackIndex - 9]), (T1)((object)frame.Data[frame.StackIndex - 8]), (T2)((object)frame.Data[frame.StackIndex - 7]), (T3)((object)frame.Data[frame.StackIndex - 6]), (T4)((object)frame.Data[frame.StackIndex - 5]), (T5)((object)frame.Data[frame.StackIndex - 4]), (T6)((object)frame.Data[frame.StackIndex - 3]), (T7)((object)frame.Data[frame.StackIndex - 2]), (T8)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 8;
			return 1;
		}

		// Token: 0x040022BB RID: 8891
		private readonly Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> _target;
	}
}
