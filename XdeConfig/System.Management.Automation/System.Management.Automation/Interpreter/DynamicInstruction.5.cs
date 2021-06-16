using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A0 RID: 1696
	internal class DynamicInstruction<T0, T1, T2, T3, TRet> : Instruction
	{
		// Token: 0x06004749 RID: 18249 RVA: 0x0017B80B File Offset: 0x00179A0B
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, TRet>>.Create(binder));
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0017B818 File Offset: 0x00179A18
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x0600474B RID: 18251 RVA: 0x0017B827 File Offset: 0x00179A27
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x0600474C RID: 18252 RVA: 0x0017B82A File Offset: 0x00179A2A
		public override int ConsumedStack
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0017B830 File Offset: 0x00179A30
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 4] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 4]), (T1)((object)frame.Data[frame.StackIndex - 3]), (T2)((object)frame.Data[frame.StackIndex - 2]), (T3)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 3;
			return 1;
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x0017B8C6 File Offset: 0x00179AC6
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F6 RID: 8950
		private CallSite<Func<CallSite, T0, T1, T2, T3, TRet>> _site;
	}
}
