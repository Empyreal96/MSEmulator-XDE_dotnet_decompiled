using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200066F RID: 1647
	internal sealed class ActionCallInstruction<T0, T1> : CallInstruction
	{
		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x00178AE1 File Offset: 0x00176CE1
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x00178AEE File Offset: 0x00176CEE
		public override int ArgumentCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x00178AF1 File Offset: 0x00176CF1
		public ActionCallInstruction(Action<T0, T1> target)
		{
			this._target = target;
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x00178B00 File Offset: 0x00176D00
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1>)target.CreateDelegate(typeof(Action<T0, T1>));
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x00178B24 File Offset: 0x00176D24
		public override object Invoke(object arg0, object arg1)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1));
			return null;
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00178B68 File Offset: 0x00176D68
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 2]), (T1)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 2;
			return 1;
		}

		// Token: 0x040022AA RID: 8874
		private readonly Action<T0, T1> _target;
	}
}
