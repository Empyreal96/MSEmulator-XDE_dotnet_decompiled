using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006AA RID: 1706
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet> : Instruction
	{
		// Token: 0x06004785 RID: 18309 RVA: 0x0017C43A File Offset: 0x0017A63A
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>>.Create(binder));
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0017C447 File Offset: 0x0017A647
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06004787 RID: 18311 RVA: 0x0017C456 File Offset: 0x0017A656
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06004788 RID: 18312 RVA: 0x0017C459 File Offset: 0x0017A659
		public override int ConsumedStack
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0017C460 File Offset: 0x0017A660
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 14] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 14]), (T1)((object)frame.Data[frame.StackIndex - 13]), (T2)((object)frame.Data[frame.StackIndex - 12]), (T3)((object)frame.Data[frame.StackIndex - 11]), (T4)((object)frame.Data[frame.StackIndex - 10]), (T5)((object)frame.Data[frame.StackIndex - 9]), (T6)((object)frame.Data[frame.StackIndex - 8]), (T7)((object)frame.Data[frame.StackIndex - 7]), (T8)((object)frame.Data[frame.StackIndex - 6]), (T9)((object)frame.Data[frame.StackIndex - 5]), (T10)((object)frame.Data[frame.StackIndex - 4]), (T11)((object)frame.Data[frame.StackIndex - 3]), (T12)((object)frame.Data[frame.StackIndex - 2]), (T13)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 13;
			return 1;
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0017C5C6 File Offset: 0x0017A7C6
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x04002300 RID: 8960
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>> _site;
	}
}
