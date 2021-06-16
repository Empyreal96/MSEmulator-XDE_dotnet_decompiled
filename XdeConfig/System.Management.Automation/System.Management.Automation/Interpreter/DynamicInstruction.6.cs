using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A1 RID: 1697
	internal class DynamicInstruction<T0, T1, T2, T3, T4, TRet> : Instruction
	{
		// Token: 0x0600474F RID: 18255 RVA: 0x0017B8E7 File Offset: 0x00179AE7
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, TRet>>.Create(binder));
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x0017B8F4 File Offset: 0x00179AF4
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06004751 RID: 18257 RVA: 0x0017B903 File Offset: 0x00179B03
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06004752 RID: 18258 RVA: 0x0017B906 File Offset: 0x00179B06
		public override int ConsumedStack
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x0017B90C File Offset: 0x00179B0C
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 5] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 5]), (T1)((object)frame.Data[frame.StackIndex - 4]), (T2)((object)frame.Data[frame.StackIndex - 3]), (T3)((object)frame.Data[frame.StackIndex - 2]), (T4)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 4;
			return 1;
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x0017B9B6 File Offset: 0x00179BB6
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F7 RID: 8951
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, TRet>> _site;
	}
}
