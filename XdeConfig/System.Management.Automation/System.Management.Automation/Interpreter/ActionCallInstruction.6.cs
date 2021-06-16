using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000672 RID: 1650
	internal sealed class ActionCallInstruction<T0, T1, T2, T3, T4> : CallInstruction
	{
		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06004663 RID: 18019 RVA: 0x00178DDF File Offset: 0x00176FDF
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06004664 RID: 18020 RVA: 0x00178DEC File Offset: 0x00176FEC
		public override int ArgumentCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x00178DEF File Offset: 0x00176FEF
		public ActionCallInstruction(Action<T0, T1, T2, T3, T4> target)
		{
			this._target = target;
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00178DFE File Offset: 0x00176FFE
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2, T3, T4>)target.CreateDelegate(typeof(Action<T0, T1, T2, T3, T4>));
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x00178E24 File Offset: 0x00177024
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4));
			return null;
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x00178EA8 File Offset: 0x001770A8
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 5]), (T1)((object)frame.Data[frame.StackIndex - 4]), (T2)((object)frame.Data[frame.StackIndex - 3]), (T3)((object)frame.Data[frame.StackIndex - 2]), (T4)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 5;
			return 1;
		}

		// Token: 0x040022AD RID: 8877
		private readonly Action<T0, T1, T2, T3, T4> _target;
	}
}
