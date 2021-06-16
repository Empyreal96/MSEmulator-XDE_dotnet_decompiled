using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006FA RID: 1786
	internal sealed class StoreLocalBoxedInstruction : LocalAccessInstruction
	{
		// Token: 0x060049BB RID: 18875 RVA: 0x0018518D File Offset: 0x0018338D
		internal StoreLocalBoxedInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x060049BC RID: 18876 RVA: 0x00185196 File Offset: 0x00183396
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x00185199 File Offset: 0x00183399
		public override int ProducedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x0018519C File Offset: 0x0018339C
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object> strongBox = (StrongBox<object>)frame.Data[this._index];
			strongBox.Value = frame.Data[--frame.StackIndex];
			return 1;
		}
	}
}
