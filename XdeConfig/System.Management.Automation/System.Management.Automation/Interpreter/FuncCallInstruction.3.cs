using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000679 RID: 1657
	internal sealed class FuncCallInstruction<T0, T1, TRet> : CallInstruction
	{
		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x0600468D RID: 18061 RVA: 0x00179779 File Offset: 0x00177979
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x0600468E RID: 18062 RVA: 0x00179786 File Offset: 0x00177986
		public override int ArgumentCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x00179789 File Offset: 0x00177989
		public FuncCallInstruction(Func<T0, T1, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00179798 File Offset: 0x00177998
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, TRet>)target.CreateDelegate(typeof(Func<T0, T1, TRet>));
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x001797BC File Offset: 0x001779BC
		public override object Invoke(object arg0, object arg1)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1));
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x00179804 File Offset: 0x00177A04
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 2] = this._target((T0)((object)frame.Data[frame.StackIndex - 2]), (T1)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex--;
			return 1;
		}

		// Token: 0x040022B4 RID: 8884
		private readonly Func<T0, T1, TRet> _target;
	}
}
