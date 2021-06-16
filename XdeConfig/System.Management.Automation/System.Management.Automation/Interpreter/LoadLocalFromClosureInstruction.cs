using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F5 RID: 1781
	internal sealed class LoadLocalFromClosureInstruction : LocalAccessInstruction
	{
		// Token: 0x060049A8 RID: 18856 RVA: 0x00185037 File Offset: 0x00183237
		internal LoadLocalFromClosureInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x00185040 File Offset: 0x00183240
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00185044 File Offset: 0x00183244
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object> strongBox = frame.Closure[this._index];
			frame.Data[frame.StackIndex++] = strongBox.Value;
			return 1;
		}
	}
}
