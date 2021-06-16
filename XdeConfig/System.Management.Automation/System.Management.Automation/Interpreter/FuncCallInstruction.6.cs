using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200067C RID: 1660
	internal sealed class FuncCallInstruction<T0, T1, T2, T3, T4, TRet> : CallInstruction
	{
		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x00179ABF File Offset: 0x00177CBF
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x00179ACC File Offset: 0x00177CCC
		public override int ArgumentCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x00179ACF File Offset: 0x00177CCF
		public FuncCallInstruction(Func<T0, T1, T2, T3, T4, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00179ADE File Offset: 0x00177CDE
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, T3, T4, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, T3, T4, TRet>));
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x00179B04 File Offset: 0x00177D04
		public override object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3), (arg4 != null) ? ((T4)((object)arg4)) : default(T4));
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x00179B8C File Offset: 0x00177D8C
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 5] = this._target((T0)((object)frame.Data[frame.StackIndex - 5]), (T1)((object)frame.Data[frame.StackIndex - 4]), (T2)((object)frame.Data[frame.StackIndex - 3]), (T3)((object)frame.Data[frame.StackIndex - 2]), (T4)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 4;
			return 1;
		}

		// Token: 0x040022B7 RID: 8887
		private readonly Func<T0, T1, T2, T3, T4, TRet> _target;
	}
}
