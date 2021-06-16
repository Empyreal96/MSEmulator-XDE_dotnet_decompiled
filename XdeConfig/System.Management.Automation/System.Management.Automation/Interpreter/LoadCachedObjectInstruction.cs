using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000731 RID: 1841
	internal sealed class LoadCachedObjectInstruction : Instruction
	{
		// Token: 0x06004A75 RID: 19061 RVA: 0x00187548 File Offset: 0x00185748
		internal LoadCachedObjectInstruction(uint index)
		{
			this._index = index;
		}

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06004A76 RID: 19062 RVA: 0x00187557 File Offset: 0x00185757
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0018755C File Offset: 0x0018575C
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex++] = frame.Interpreter._objects[(int)((UIntPtr)this._index)];
			return 1;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x00187598 File Offset: 0x00185798
		public override string ToDebugString(int instructionIndex, object cookie, Func<int, int> labelIndexer, IList<object> objects)
		{
			return string.Format(CultureInfo.InvariantCulture, "LoadCached({0}: {1})", new object[]
			{
				this._index,
				objects[(int)this._index]
			});
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x001875DA File Offset: 0x001857DA
		public override string ToString()
		{
			return "LoadCached(" + this._index + ")";
		}

		// Token: 0x04002410 RID: 9232
		private readonly uint _index;
	}
}
