using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000665 RID: 1637
	internal sealed class NewArrayInstruction<TElement> : Instruction
	{
		// Token: 0x060045E6 RID: 17894 RVA: 0x00177112 File Offset: 0x00175312
		internal NewArrayInstruction()
		{
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x0017711A File Offset: 0x0017531A
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060045E8 RID: 17896 RVA: 0x0017711D File Offset: 0x0017531D
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x00177120 File Offset: 0x00175320
		public override int Run(InterpretedFrame frame)
		{
			int num = (int)frame.Pop();
			frame.Push(new TElement[num]);
			return 1;
		}
	}
}
