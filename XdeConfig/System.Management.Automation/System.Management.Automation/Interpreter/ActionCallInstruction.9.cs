using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000675 RID: 1653
	internal sealed class ActionCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7> : CallInstruction
	{
		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06004675 RID: 18037 RVA: 0x0017925B File Offset: 0x0017745B
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x00179268 File Offset: 0x00177468
		public override int ArgumentCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x0017926B File Offset: 0x0017746B
		public ActionCallInstruction(Action<T0, T1, T2, T3, T4, T5, T6, T7> target)
		{
			this._target = target;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0017927A File Offset: 0x0017747A
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2, T3, T4, T5, T6, T7>)target.CreateDelegate(typeof(Action<T0, T1, T2, T3, T4, T5, T6, T7>));
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x001792A0 File Offset: 0x001774A0
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5), (arg6 != null) ? ((T6)((object)arg6)) : default(T6), (arg7 != null) ? ((T7)((object)arg7)) : default(T7));
			return null;
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x00179368 File Offset: 0x00177568
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 8]), (T1)((object)frame.Data[frame.StackIndex - 7]), (T2)((object)frame.Data[frame.StackIndex - 6]), (T3)((object)frame.Data[frame.StackIndex - 5]), (T4)((object)frame.Data[frame.StackIndex - 4]), (T5)((object)frame.Data[frame.StackIndex - 3]), (T6)((object)frame.Data[frame.StackIndex - 2]), (T7)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 8;
			return 1;
		}

		// Token: 0x040022B0 RID: 8880
		private readonly Action<T0, T1, T2, T3, T4, T5, T6, T7> _target;
	}
}
