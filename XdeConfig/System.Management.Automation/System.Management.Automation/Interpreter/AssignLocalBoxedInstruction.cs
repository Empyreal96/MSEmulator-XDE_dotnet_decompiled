using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F9 RID: 1785
	internal sealed class AssignLocalBoxedInstruction : LocalAccessInstruction
	{
		// Token: 0x060049B7 RID: 18871 RVA: 0x0018514E File Offset: 0x0018334E
		internal AssignLocalBoxedInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x00185157 File Offset: 0x00183357
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x0018515A File Offset: 0x0018335A
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x00185160 File Offset: 0x00183360
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object> strongBox = (StrongBox<object>)frame.Data[this._index];
			strongBox.Value = frame.Peek();
			return 1;
		}
	}
}
