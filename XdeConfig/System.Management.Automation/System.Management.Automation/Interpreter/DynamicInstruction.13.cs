using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A8 RID: 1704
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet> : Instruction
	{
		// Token: 0x06004779 RID: 18297 RVA: 0x0017C124 File Offset: 0x0017A324
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>>.Create(binder));
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0017C131 File Offset: 0x0017A331
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x0017C140 File Offset: 0x0017A340
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x0600477C RID: 18300 RVA: 0x0017C143 File Offset: 0x0017A343
		public override int ConsumedStack
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0017C148 File Offset: 0x0017A348
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 12] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 12]), (T1)((object)frame.Data[frame.StackIndex - 11]), (T2)((object)frame.Data[frame.StackIndex - 10]), (T3)((object)frame.Data[frame.StackIndex - 9]), (T4)((object)frame.Data[frame.StackIndex - 8]), (T5)((object)frame.Data[frame.StackIndex - 7]), (T6)((object)frame.Data[frame.StackIndex - 6]), (T7)((object)frame.Data[frame.StackIndex - 5]), (T8)((object)frame.Data[frame.StackIndex - 4]), (T9)((object)frame.Data[frame.StackIndex - 3]), (T10)((object)frame.Data[frame.StackIndex - 2]), (T11)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 11;
			return 1;
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x0017C284 File Offset: 0x0017A484
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022FE RID: 8958
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>> _site;
	}
}
