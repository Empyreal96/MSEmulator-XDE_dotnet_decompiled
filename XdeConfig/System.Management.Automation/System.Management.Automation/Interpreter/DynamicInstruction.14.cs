using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A9 RID: 1705
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet> : Instruction
	{
		// Token: 0x0600477F RID: 18303 RVA: 0x0017C2A5 File Offset: 0x0017A4A5
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>>.Create(binder));
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0017C2B2 File Offset: 0x0017A4B2
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06004781 RID: 18305 RVA: 0x0017C2C1 File Offset: 0x0017A4C1
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06004782 RID: 18306 RVA: 0x0017C2C4 File Offset: 0x0017A4C4
		public override int ConsumedStack
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0017C2C8 File Offset: 0x0017A4C8
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 13] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 13]), (T1)((object)frame.Data[frame.StackIndex - 12]), (T2)((object)frame.Data[frame.StackIndex - 11]), (T3)((object)frame.Data[frame.StackIndex - 10]), (T4)((object)frame.Data[frame.StackIndex - 9]), (T5)((object)frame.Data[frame.StackIndex - 8]), (T6)((object)frame.Data[frame.StackIndex - 7]), (T7)((object)frame.Data[frame.StackIndex - 6]), (T8)((object)frame.Data[frame.StackIndex - 5]), (T9)((object)frame.Data[frame.StackIndex - 4]), (T10)((object)frame.Data[frame.StackIndex - 3]), (T11)((object)frame.Data[frame.StackIndex - 2]), (T12)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 12;
			return 1;
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0017C419 File Offset: 0x0017A619
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022FF RID: 8959
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>> _site;
	}
}
