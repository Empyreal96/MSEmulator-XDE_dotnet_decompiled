using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A7 RID: 1703
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet> : Instruction
	{
		// Token: 0x06004773 RID: 18291 RVA: 0x0017BFB7 File Offset: 0x0017A1B7
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>>.Create(binder));
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0017BFC4 File Offset: 0x0017A1C4
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06004775 RID: 18293 RVA: 0x0017BFD3 File Offset: 0x0017A1D3
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06004776 RID: 18294 RVA: 0x0017BFD6 File Offset: 0x0017A1D6
		public override int ConsumedStack
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0017BFDC File Offset: 0x0017A1DC
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 11] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 11]), (T1)((object)frame.Data[frame.StackIndex - 10]), (T2)((object)frame.Data[frame.StackIndex - 9]), (T3)((object)frame.Data[frame.StackIndex - 8]), (T4)((object)frame.Data[frame.StackIndex - 7]), (T5)((object)frame.Data[frame.StackIndex - 6]), (T6)((object)frame.Data[frame.StackIndex - 5]), (T7)((object)frame.Data[frame.StackIndex - 4]), (T8)((object)frame.Data[frame.StackIndex - 3]), (T9)((object)frame.Data[frame.StackIndex - 2]), (T10)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 10;
			return 1;
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0017C103 File Offset: 0x0017A303
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022FD RID: 8957
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>> _site;
	}
}
