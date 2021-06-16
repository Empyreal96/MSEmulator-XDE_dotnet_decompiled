using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F3 RID: 1779
	internal sealed class LoadLocalInstruction : LocalAccessInstruction, IBoxableInstruction
	{
		// Token: 0x060049A1 RID: 18849 RVA: 0x00184F95 File Offset: 0x00183195
		internal LoadLocalInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x00184F9E File Offset: 0x0018319E
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00184FA4 File Offset: 0x001831A4
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex++] = frame.Data[this._index];
			return 1;
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x00184FD7 File Offset: 0x001831D7
		public Instruction BoxIfIndexMatches(int index)
		{
			if (index != this._index)
			{
				return null;
			}
			return InstructionList.LoadLocalBoxed(index);
		}
	}
}
