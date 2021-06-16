using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A5 RID: 1701
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> : Instruction
	{
		// Token: 0x06004767 RID: 18279 RVA: 0x0017BD1F File Offset: 0x00179F1F
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>>.Create(binder));
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0017BD2C File Offset: 0x00179F2C
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06004769 RID: 18281 RVA: 0x0017BD3B File Offset: 0x00179F3B
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x0600476A RID: 18282 RVA: 0x0017BD3E File Offset: 0x00179F3E
		public override int ConsumedStack
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0017BD44 File Offset: 0x00179F44
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 9] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 9]), (T1)((object)frame.Data[frame.StackIndex - 8]), (T2)((object)frame.Data[frame.StackIndex - 7]), (T3)((object)frame.Data[frame.StackIndex - 6]), (T4)((object)frame.Data[frame.StackIndex - 5]), (T5)((object)frame.Data[frame.StackIndex - 4]), (T6)((object)frame.Data[frame.StackIndex - 3]), (T7)((object)frame.Data[frame.StackIndex - 2]), (T8)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 8;
			return 1;
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0017BE40 File Offset: 0x0017A040
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022FB RID: 8955
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>> _site;
	}
}
