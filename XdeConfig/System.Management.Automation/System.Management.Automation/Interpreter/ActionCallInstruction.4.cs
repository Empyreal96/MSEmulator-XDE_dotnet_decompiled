using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000670 RID: 1648
	internal sealed class ActionCallInstruction<T0, T1, T2> : CallInstruction
	{
		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x00178BB7 File Offset: 0x00176DB7
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x00178BC4 File Offset: 0x00176DC4
		public override int ArgumentCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00178BC7 File Offset: 0x00176DC7
		public ActionCallInstruction(Action<T0, T1, T2> target)
		{
			this._target = target;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00178BD6 File Offset: 0x00176DD6
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2>)target.CreateDelegate(typeof(Action<T0, T1, T2>));
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00178BFC File Offset: 0x00176DFC
		public override object Invoke(object arg0, object arg1, object arg2)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2));
			return null;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00178C54 File Offset: 0x00176E54
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 3]), (T1)((object)frame.Data[frame.StackIndex - 2]), (T2)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 3;
			return 1;
		}

		// Token: 0x040022AB RID: 8875
		private readonly Action<T0, T1, T2> _target;
	}
}
