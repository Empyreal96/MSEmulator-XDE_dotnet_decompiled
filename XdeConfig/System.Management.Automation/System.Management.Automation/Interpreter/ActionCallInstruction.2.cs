using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200066E RID: 1646
	internal sealed class ActionCallInstruction<T0> : CallInstruction
	{
		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x00178A41 File Offset: 0x00176C41
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x00178A4E File Offset: 0x00176C4E
		public override int ArgumentCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x00178A51 File Offset: 0x00176C51
		public ActionCallInstruction(Action<T0> target)
		{
			this._target = target;
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x00178A60 File Offset: 0x00176C60
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0>)target.CreateDelegate(typeof(Action<T0>));
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x00178A84 File Offset: 0x00176C84
		public override object Invoke(object arg0)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0));
			return null;
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x00178AB1 File Offset: 0x00176CB1
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex--;
			return 1;
		}

		// Token: 0x040022A9 RID: 8873
		private readonly Action<T0> _target;
	}
}
