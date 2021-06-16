using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000686 RID: 1670
	internal abstract class IndexedBranchInstruction : Instruction
	{
		// Token: 0x060046D6 RID: 18134 RVA: 0x0017A5AD File Offset: 0x001787AD
		public IndexedBranchInstruction(int labelIndex)
		{
			this._labelIndex = labelIndex;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x0017A5BC File Offset: 0x001787BC
		public RuntimeLabel GetLabel(InterpretedFrame frame)
		{
			return frame.Interpreter._labels[this._labelIndex];
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x0017A5DC File Offset: 0x001787DC
		public override string ToDebugString(int instructionIndex, object cookie, Func<int, int> labelIndexer, IList<object> objects)
		{
			int num = labelIndexer(this._labelIndex);
			return this.ToString() + ((num != int.MinValue) ? (" -> " + num) : "");
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x0017A620 File Offset: 0x00178820
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.InstructionName,
				"[",
				this._labelIndex,
				"]"
			});
		}

		// Token: 0x040022C5 RID: 8901
		protected const int CacheSize = 32;

		// Token: 0x040022C6 RID: 8902
		internal readonly int _labelIndex;
	}
}
