using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000689 RID: 1673
	internal sealed class EnterFinallyInstruction : IndexedBranchInstruction
	{
		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x060046EA RID: 18154 RVA: 0x0017A9DF File Offset: 0x00178BDF
		public override int ProducedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x060046EB RID: 18155 RVA: 0x0017A9E2 File Offset: 0x00178BE2
		public override int ConsumedContinuations
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x0017A9E5 File Offset: 0x00178BE5
		private EnterFinallyInstruction(int labelIndex) : base(labelIndex)
		{
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x0017A9F0 File Offset: 0x00178BF0
		internal static EnterFinallyInstruction Create(int labelIndex)
		{
			if (labelIndex < 32)
			{
				EnterFinallyInstruction result;
				if ((result = EnterFinallyInstruction.Cache[labelIndex]) == null)
				{
					result = (EnterFinallyInstruction.Cache[labelIndex] = new EnterFinallyInstruction(labelIndex));
				}
				return result;
			}
			return new EnterFinallyInstruction(labelIndex);
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x0017AA24 File Offset: 0x00178C24
		public override int Run(InterpretedFrame frame)
		{
			if (!frame.IsJumpHappened())
			{
				frame.SetStackDepth(base.GetLabel(frame).StackDepth);
			}
			frame.PushPendingContinuation();
			frame.RemoveContinuation();
			return 1;
		}

		// Token: 0x040022CD RID: 8909
		private static readonly EnterFinallyInstruction[] Cache = new EnterFinallyInstruction[32];
	}
}
