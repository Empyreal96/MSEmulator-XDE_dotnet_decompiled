using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F2 RID: 1778
	internal abstract class LocalAccessInstruction : Instruction
	{
		// Token: 0x0600499F RID: 18847 RVA: 0x00184EFF File Offset: 0x001830FF
		protected LocalAccessInstruction(int index)
		{
			this._index = index;
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00184F10 File Offset: 0x00183110
		public override string ToDebugString(int instructionIndex, object cookie, Func<int, int> labelIndexer, IList<object> objects)
		{
			if (cookie != null)
			{
				return string.Concat(new object[]
				{
					this.InstructionName,
					"(",
					cookie,
					": ",
					this._index,
					")"
				});
			}
			return string.Concat(new object[]
			{
				this.InstructionName,
				"(",
				this._index,
				")"
			});
		}

		// Token: 0x040023C9 RID: 9161
		internal readonly int _index;
	}
}
