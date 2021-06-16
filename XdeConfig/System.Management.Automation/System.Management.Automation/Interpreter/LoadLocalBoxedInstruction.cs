using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F4 RID: 1780
	internal sealed class LoadLocalBoxedInstruction : LocalAccessInstruction
	{
		// Token: 0x060049A5 RID: 18853 RVA: 0x00184FEA File Offset: 0x001831EA
		internal LoadLocalBoxedInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x00184FF3 File Offset: 0x001831F3
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x00184FF8 File Offset: 0x001831F8
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object> strongBox = (StrongBox<object>)frame.Data[this._index];
			frame.Data[frame.StackIndex++] = strongBox.Value;
			return 1;
		}
	}
}
