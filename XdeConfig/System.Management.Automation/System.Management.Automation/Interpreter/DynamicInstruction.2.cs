using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200069D RID: 1693
	internal class DynamicInstruction<T0, TRet> : Instruction
	{
		// Token: 0x06004737 RID: 18231 RVA: 0x0017B600 File Offset: 0x00179800
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, TRet>(CallSite<Func<CallSite, T0, TRet>>.Create(binder));
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0017B60D File Offset: 0x0017980D
		private DynamicInstruction(CallSite<Func<CallSite, T0, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06004739 RID: 18233 RVA: 0x0017B61C File Offset: 0x0017981C
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x0600473A RID: 18234 RVA: 0x0017B61F File Offset: 0x0017981F
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0017B624 File Offset: 0x00179824
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 1] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 1]));
			return 1;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0017B670 File Offset: 0x00179870
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022F3 RID: 8947
		private CallSite<Func<CallSite, T0, TRet>> _site;
	}
}
