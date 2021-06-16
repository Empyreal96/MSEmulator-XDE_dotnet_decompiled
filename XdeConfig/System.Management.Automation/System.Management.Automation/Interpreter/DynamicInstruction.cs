using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200069C RID: 1692
	internal class DynamicInstruction<TRet> : Instruction
	{
		// Token: 0x06004731 RID: 18225 RVA: 0x0017B584 File Offset: 0x00179784
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<TRet>(CallSite<Func<CallSite, TRet>>.Create(binder));
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0017B591 File Offset: 0x00179791
		private DynamicInstruction(CallSite<Func<CallSite, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06004733 RID: 18227 RVA: 0x0017B5A0 File Offset: 0x001797A0
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06004734 RID: 18228 RVA: 0x0017B5A3 File Offset: 0x001797A3
		public override int ConsumedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0017B5A6 File Offset: 0x001797A6
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex] = this._site.Target(this._site);
			frame.StackIndex -= -1;
			return 1;
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0017B5DF File Offset: 0x001797DF
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F2 RID: 8946
		private CallSite<Func<CallSite, TRet>> _site;
	}
}
