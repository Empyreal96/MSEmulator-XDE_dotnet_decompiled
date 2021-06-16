using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006AB RID: 1707
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet> : Instruction
	{
		// Token: 0x0600478B RID: 18315 RVA: 0x0017C5E7 File Offset: 0x0017A7E7
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>>.Create(binder));
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0017C5F4 File Offset: 0x0017A7F4
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x0600478D RID: 18317 RVA: 0x0017C603 File Offset: 0x0017A803
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x0600478E RID: 18318 RVA: 0x0017C606 File Offset: 0x0017A806
		public override int ConsumedStack
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0017C60C File Offset: 0x0017A80C
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 15] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 15]), (T1)((object)frame.Data[frame.StackIndex - 14]), (T2)((object)frame.Data[frame.StackIndex - 13]), (T3)((object)frame.Data[frame.StackIndex - 12]), (T4)((object)frame.Data[frame.StackIndex - 11]), (T5)((object)frame.Data[frame.StackIndex - 10]), (T6)((object)frame.Data[frame.StackIndex - 9]), (T7)((object)frame.Data[frame.StackIndex - 8]), (T8)((object)frame.Data[frame.StackIndex - 7]), (T9)((object)frame.Data[frame.StackIndex - 6]), (T10)((object)frame.Data[frame.StackIndex - 5]), (T11)((object)frame.Data[frame.StackIndex - 4]), (T12)((object)frame.Data[frame.StackIndex - 3]), (T13)((object)frame.Data[frame.StackIndex - 2]), (T14)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 14;
			return 1;
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0017C787 File Offset: 0x0017A987
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x04002301 RID: 8961
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>> _site;
	}
}
