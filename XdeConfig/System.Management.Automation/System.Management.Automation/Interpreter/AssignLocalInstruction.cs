using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F7 RID: 1783
	internal sealed class AssignLocalInstruction : LocalAccessInstruction, IBoxableInstruction
	{
		// Token: 0x060049AE RID: 18862 RVA: 0x001850C1 File Offset: 0x001832C1
		internal AssignLocalInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x060049AF RID: 18863 RVA: 0x001850CA File Offset: 0x001832CA
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x060049B0 RID: 18864 RVA: 0x001850CD File Offset: 0x001832CD
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x001850D0 File Offset: 0x001832D0
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[this._index] = frame.Peek();
			return 1;
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x001850E6 File Offset: 0x001832E6
		public Instruction BoxIfIndexMatches(int index)
		{
			if (index != this._index)
			{
				return null;
			}
			return InstructionList.AssignLocalBoxed(index);
		}
	}
}
