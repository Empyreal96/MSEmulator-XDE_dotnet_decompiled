using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200067B RID: 1659
	internal sealed class FuncCallInstruction<T0, T1, T2, T3, TRet> : CallInstruction
	{
		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06004699 RID: 18073 RVA: 0x0017997F File Offset: 0x00177B7F
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x0600469A RID: 18074 RVA: 0x0017998C File Offset: 0x00177B8C
		public override int ArgumentCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x0017998F File Offset: 0x00177B8F
		public FuncCallInstruction(Func<T0, T1, T2, T3, TRet> target)
		{
			this._target = target;
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x0017999E File Offset: 0x00177B9E
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<T0, T1, T2, T3, TRet>)target.CreateDelegate(typeof(Func<T0, T1, T2, T3, TRet>));
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x001799C4 File Offset: 0x00177BC4
		public override object Invoke(object arg0, object arg1, object arg2, object arg3)
		{
			return this._target((arg0 != null) ? ((T0)((object)arg0)) : default(T0), (arg1 != null) ? ((T1)((object)arg1)) : default(T1), (arg2 != null) ? ((T2)((object)arg2)) : default(T2), (arg3 != null) ? ((T3)((object)arg3)) : default(T3));
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00179A34 File Offset: 0x00177C34
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 4] = this._target((T0)((object)frame.Data[frame.StackIndex - 4]), (T1)((object)frame.Data[frame.StackIndex - 3]), (T2)((object)frame.Data[frame.StackIndex - 2]), (T3)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 3;
			return 1;
		}

		// Token: 0x040022B6 RID: 8886
		private readonly Func<T0, T1, T2, T3, TRet> _target;
	}
}
