using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200067A RID: 1658
	internal sealed class FuncCallInstruction<T0, T1, T2, TRet> : CallInstruction
	{
		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06004693 RID: 18067 RVA: 0x00179867 File Offset: 0x00177A67
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06004694 RID: 18068 RVA: 0x00179874 File Offset: 0x00177A74
		public override int ArgumentCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x00179877 File Offset: 0x00177A77
		public FuncCallInstruction(Func<T0, T1, T2, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00179886 File Offset: 0x00177A86
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, TRet>));
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x001798AC File Offset: 0x00177AAC
		public override object Invoke(object arg0, object arg1, object arg2)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2));
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x00179908 File Offset: 0x00177B08
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 3] = this._target((T0)((object)frame.Data[frame.StackIndex - 3]), (T1)((object)frame.Data[frame.StackIndex - 2]), (T2)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 2;
			return 1;
		}

		// Token: 0x040022B5 RID: 8885
		private readonly Func<T0, T1, T2, TRet> _target;
	}
}
