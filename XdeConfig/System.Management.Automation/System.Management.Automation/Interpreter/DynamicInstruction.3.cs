using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200069E RID: 1694
	internal class DynamicInstruction<T0, T1, TRet> : Instruction
	{
		// Token: 0x0600473D RID: 18237 RVA: 0x0017B691 File Offset: 0x00179891
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, TRet>(CallSite<Func<CallSite, T0, T1, TRet>>.Create(binder));
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0017B69E File Offset: 0x0017989E
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x0017B6AD File Offset: 0x001798AD
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06004740 RID: 18240 RVA: 0x0017B6B0 File Offset: 0x001798B0
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0017B6B4 File Offset: 0x001798B4
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 2] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 2]), (T1)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex--;
			return 1;
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0017B722 File Offset: 0x00179922
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F4 RID: 8948
		private CallSite<Func<CallSite, T0, T1, TRet>> _site;
	}
}
