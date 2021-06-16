using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F8 RID: 1784
	internal sealed class StoreLocalInstruction : LocalAccessInstruction, IBoxableInstruction
	{
		// Token: 0x060049B3 RID: 18867 RVA: 0x001850F9 File Offset: 0x001832F9
		internal StoreLocalInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x00185102 File Offset: 0x00183302
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x00185108 File Offset: 0x00183308
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[this._index] = frame.Data[--frame.StackIndex];
			return 1;
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x0018513B File Offset: 0x0018333B
		public Instruction BoxIfIndexMatches(int index)
		{
			if (index != this._index)
			{
				return null;
			}
			return InstructionList.StoreLocalBoxed(index);
		}
	}
}
