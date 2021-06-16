using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000673 RID: 1651
	internal sealed class ActionCallInstruction<T0, T1, T2, T3, T4, T5> : CallInstruction
	{
		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06004669 RID: 18025 RVA: 0x00178F33 File Offset: 0x00177133
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x0600466A RID: 18026 RVA: 0x00178F40 File Offset: 0x00177140
		public override int ArgumentCount
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x00178F43 File Offset: 0x00177143
		public ActionCallInstruction(Action<T0, T1, T2, T3, T4, T5> target)
		{
			this._target = target;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x00178F52 File Offset: 0x00177152
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2, T3, T4, T5>)target.CreateDelegate(typeof(Action<T0, T1, T2, T3, T4, T5>));
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x00178F78 File Offset: 0x00177178
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5));
			return null;
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x00179014 File Offset: 0x00177214
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 6]), (T1)((object)frame.Data[frame.StackIndex - 5]), (T2)((object)frame.Data[frame.StackIndex - 4]), (T3)((object)frame.Data[frame.StackIndex - 3]), (T4)((object)frame.Data[frame.StackIndex - 2]), (T5)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 6;
			return 1;
		}

		// Token: 0x040022AE RID: 8878
		private readonly Action<T0, T1, T2, T3, T4, T5> _target;
	}
}
