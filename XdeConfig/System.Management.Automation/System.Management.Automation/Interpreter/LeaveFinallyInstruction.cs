using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200068A RID: 1674
	internal sealed class LeaveFinallyInstruction : Instruction
	{
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x0017AA5B File Offset: 0x00178C5B
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x0017AA5E File Offset: 0x00178C5E
		private LeaveFinallyInstruction()
		{
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x0017AA66 File Offset: 0x00178C66
		public override int Run(InterpretedFrame frame)
		{
			frame.PopPendingContinuation();
			if (!frame.IsJumpHappened())
			{
				return 1;
			}
			return frame.YieldToPendingContinuation();
		}

		// Token: 0x040022CE RID: 8910
		internal static readonly Instruction Instance = new LeaveFinallyInstruction();
	}
}
