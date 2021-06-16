using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A3 RID: 1699
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, TRet> : Instruction
	{
		// Token: 0x0600475B RID: 18267 RVA: 0x0017BADB File Offset: 0x00179CDB
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>>.Create(binder));
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x0017BAE8 File Offset: 0x00179CE8
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x0600475D RID: 18269 RVA: 0x0017BAF7 File Offset: 0x00179CF7
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x0600475E RID: 18270 RVA: 0x0017BAFA File Offset: 0x00179CFA
		public override int ConsumedStack
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x0017BB00 File Offset: 0x00179D00
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 7] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 7]), (T1)((object)frame.Data[frame.StackIndex - 6]), (T2)((object)frame.Data[frame.StackIndex - 5]), (T3)((object)frame.Data[frame.StackIndex - 4]), (T4)((object)frame.Data[frame.StackIndex - 3]), (T5)((object)frame.Data[frame.StackIndex - 2]), (T6)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 6;
			return 1;
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x0017BBD2 File Offset: 0x00179DD2
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F9 RID: 8953
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, TRet>> _site;
	}
}
