using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000674 RID: 1652
	internal sealed class ActionCallInstruction<T0, T1, T2, T3, T4, T5, T6> : CallInstruction
	{
		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x0600466F RID: 18031 RVA: 0x001790B3 File Offset: 0x001772B3
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06004670 RID: 18032 RVA: 0x001790C0 File Offset: 0x001772C0
		public override int ArgumentCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x001790C3 File Offset: 0x001772C3
		public ActionCallInstruction(Action<T0, T1, T2, T3, T4, T5, T6> target)
		{
			this._target = target;
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x001790D2 File Offset: 0x001772D2
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2, T3, T4, T5, T6>)target.CreateDelegate(typeof(Action<T0, T1, T2, T3, T4, T5, T6>));
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x001790F8 File Offset: 0x001772F8
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5), (arg6 != null) ? ((T6)((object)arg6)) : default(T6));
			return null;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x001791A8 File Offset: 0x001773A8
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 7]), (T1)((object)frame.Data[frame.StackIndex - 6]), (T2)((object)frame.Data[frame.StackIndex - 5]), (T3)((object)frame.Data[frame.StackIndex - 4]), (T4)((object)frame.Data[frame.StackIndex - 3]), (T5)((object)frame.Data[frame.StackIndex - 2]), (T6)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 7;
			return 1;
		}

		// Token: 0x040022AF RID: 8879
		private readonly Action<T0, T1, T2, T3, T4, T5, T6> _target;
	}
}
