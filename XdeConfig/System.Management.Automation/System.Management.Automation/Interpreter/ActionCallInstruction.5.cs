using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000671 RID: 1649
	internal sealed class ActionCallInstruction<T0, T1, T2, T3> : CallInstruction
	{
		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x0600465D RID: 18013 RVA: 0x00178CB7 File Offset: 0x00176EB7
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x0600465E RID: 18014 RVA: 0x00178CC4 File Offset: 0x00176EC4
		public override int ArgumentCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00178CC7 File Offset: 0x00176EC7
		public ActionCallInstruction(Action<T0, T1, T2, T3> target)
		{
			this._target = target;
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00178CD6 File Offset: 0x00176ED6
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action<T0, T1, T2, T3>)target.CreateDelegate(typeof(Action<T0, T1, T2, T3>));
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00178CFC File Offset: 0x00176EFC
		public override object Invoke(object arg0, object arg1, object arg2, object arg3)
		{
			this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3));
			return null;
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00178D68 File Offset: 0x00176F68
		public override int Run(InterpretedFrame frame)
		{
			this._target((T0)((object)frame.Data[frame.StackIndex - 4]), (T1)((object)frame.Data[frame.StackIndex - 3]), (T2)((object)frame.Data[frame.StackIndex - 2]), (T3)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 4;
			return 1;
		}

		// Token: 0x040022AC RID: 8876
		private readonly Action<T0, T1, T2, T3> _target;
	}
}
