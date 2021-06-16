using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006A4 RID: 1700
	internal class DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, TRet> : Instruction
	{
		// Token: 0x06004761 RID: 18273 RVA: 0x0017BBF3 File Offset: 0x00179DF3
		public static Instruction Factory(CallSiteBinder binder)
		{
			return new DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>>.Create(binder));
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x0017BC00 File Offset: 0x00179E00
		private DynamicInstruction(CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>> site)
		{
			this._site = site;
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06004763 RID: 18275 RVA: 0x0017BC0F File Offset: 0x00179E0F
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06004764 RID: 18276 RVA: 0x0017BC12 File Offset: 0x00179E12
		public override int ConsumedStack
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0017BC18 File Offset: 0x00179E18
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex - 8] = this._site.Target(this._site, (T0)((object)frame.Data[frame.StackIndex - 8]), (T1)((object)frame.Data[frame.StackIndex - 7]), (T2)((object)frame.Data[frame.StackIndex - 6]), (T3)((object)frame.Data[frame.StackIndex - 5]), (T4)((object)frame.Data[frame.StackIndex - 4]), (T5)((object)frame.Data[frame.StackIndex - 3]), (T6)((object)frame.Data[frame.StackIndex - 2]), (T7)((object)frame.Data[frame.StackIndex - 1]));
			frame.StackIndex -= 7;
			return 1;
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0017BCFE File Offset: 0x00179EFE
		public override string ToString()
		{
			return "Dynamic(" + this._site.Binder.ToString() + ")";
		}

		// Token: 0x040022FA RID: 8954
		private CallSite<Func<CallSite, T0, T1, T2, T3, T4, T5, T6, T7, TRet>> _site;
	}
}
