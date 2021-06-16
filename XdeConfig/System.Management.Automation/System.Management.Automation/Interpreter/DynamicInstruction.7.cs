using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A2 RID: 1698
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, TRet> : Instruction
	{
		// Token: 0x06004755 RID: 18261 RVA: 0x0017B9D7 File Offset: 0x00179BD7
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>>.Create(binder));
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x0017B9E4 File Offset: 0x00179BE4
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06004757 RID: 18263 RVA: 0x0017B9F3 File Offset: 0x00179BF3
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x0017B9F6 File Offset: 0x00179BF6
		public override int ConsumedStack
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x0017B9FC File Offset: 0x00179BFC
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 6] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 6]), (T1)((object)frame.Data[frame.StackIndex - 5]), (T2)((object)frame.Data[frame.StackIndex - 4]), (T3)((object)frame.Data[frame.StackIndex - 3]), (T4)((object)frame.Data[frame.StackIndex - 2]), (T5)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 5;
			return 1;
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x0017BABA File Offset: 0x00179CBA
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F8 RID: 8952
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, TRet>> _site;
	}
}
