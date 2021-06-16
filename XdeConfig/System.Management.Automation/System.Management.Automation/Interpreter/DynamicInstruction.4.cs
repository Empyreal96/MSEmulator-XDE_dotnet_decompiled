using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200069F RID: 1695
	internal class DynamicInstruction<T0, T1, T2, TRet> : Instruction
	{
		// Token: 0x06004743 RID: 18243 RVA: 0x0017B743 File Offset: 0x00179943
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, TRet>(CallSite<Func<CallSite, T0, T1, T2, TRet>>.Create(binder));
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0017B750 File Offset: 0x00179950
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06004745 RID: 18245 RVA: 0x0017B75F File Offset: 0x0017995F
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06004746 RID: 18246 RVA: 0x0017B762 File Offset: 0x00179962
		public override int ConsumedStack
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0017B768 File Offset: 0x00179968
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 3] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 3]), (T1)((object)frame.Data[frame.StackIndex - 2]), (T2)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 2;
			return 1;
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0017B7EA File Offset: 0x001799EA
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F5 RID: 8949
		private CallSite<Func<CallSite, T0, T1, T2, TRet>> _site;
	}
}
