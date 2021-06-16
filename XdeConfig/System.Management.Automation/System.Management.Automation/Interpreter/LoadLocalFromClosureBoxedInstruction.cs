using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F6 RID: 1782
	internal sealed class LoadLocalFromClosureBoxedInstruction : LocalAccessInstruction
	{
		// Token: 0x060049AB RID: 18859 RVA: 0x0018507E File Offset: 0x0018327E
		internal LoadLocalFromClosureBoxedInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x060049AC RID: 18860 RVA: 0x00185087 File Offset: 0x00183287
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x0018508C File Offset: 0x0018328C
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object> strongBox = frame.Closure[this._index];
			frame.Data[frame.StackIndex++] = strongBox;
			return 1;
		}
	}
}
