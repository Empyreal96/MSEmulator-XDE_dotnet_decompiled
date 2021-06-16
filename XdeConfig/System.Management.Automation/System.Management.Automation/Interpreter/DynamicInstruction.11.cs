using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A6 RID: 1702
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> : Instruction
	{
		// Token: 0x0600476D RID: 18285 RVA: 0x0017BE61 File Offset: 0x0017A061
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>>.Create(binder));
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x0017BE6E File Offset: 0x0017A06E
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x0600476F RID: 18287 RVA: 0x0017BE7D File Offset: 0x0017A07D
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06004770 RID: 18288 RVA: 0x0017BE80 File Offset: 0x0017A080
		public override int ConsumedStack
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x0017BE84 File Offset: 0x0017A084
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 10] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 10]), (T1)((object)frame.Data[frame.StackIndex - 9]), (T2)((object)frame.Data[frame.StackIndex - 8]), (T3)((object)frame.Data[frame.StackIndex - 7]), (T4)((object)frame.Data[frame.StackIndex - 6]), (T5)((object)frame.Data[frame.StackIndex - 5]), (T6)((object)frame.Data[frame.StackIndex - 4]), (T7)((object)frame.Data[frame.StackIndex - 3]), (T8)((object)frame.Data[frame.StackIndex - 2]), (T9)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 9;
			return 1;
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0017BF96 File Offset: 0x0017A196
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022FC RID: 8956
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>> _site;
	}
}
