using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000676 RID: 1654
	internal sealed class ActionCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8> : CallInstruction
	{
		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x0017942F File Offset: 0x0017762F
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x0600467C RID: 18044 RVA: 0x0017943C File Offset: 0x0017763C
		public override int ArgumentCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00179440 File Offset: 0x00177640
		public ActionCallInstruction(Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> target)
		{
			this._target = target;
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0017944F File Offset: 0x0017764F
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>)target.CreateDelegate(typeof(Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>));
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x00179474 File Offset: 0x00177674
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4), (arg5 != null) ? ((T5)((object)arg5)) : default(T5), (arg6 != null) ? ((T6)((object)arg6)) : default(T6), (arg7 != null) ? ((T7)((object)arg7)) : default(T7), (arg8 != null) ? ((T8)((object)arg8)) : default(T8));
			return null;
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x00179554 File Offset: 0x00177754
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 9]), (T1)((object)frame.Data[frame.StackIndex - 8]), (T2)((object)frame.Data[frame.StackIndex - 7]), (T3)((object)frame.Data[frame.StackIndex - 6]), (T4)((object)frame.Data[frame.StackIndex - 5]), (T5)((object)frame.Data[frame.StackIndex - 4]), (T6)((object)frame.Data[frame.StackIndex - 3]), (T7)((object)frame.Data[frame.StackIndex - 2]), (T8)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 9;
			return 1;
		}

		// Token: 0x040022B1 RID: 8881
		private readonly Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> _target;
	}
}
