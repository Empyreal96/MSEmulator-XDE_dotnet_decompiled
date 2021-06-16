using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000733 RID: 1843
	internal sealed class DupInstruction : Instruction
	{
		// Token: 0x06004A7F RID: 19071 RVA: 0x0018761E File Offset: 0x0018581E
		private DupInstruction()
		{
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06004A80 RID: 19072 RVA: 0x00187626 File Offset: 0x00185826
		public override int ConsumedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06004A81 RID: 19073 RVA: 0x00187629 File Offset: 0x00185829
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x0018762C File Offset: 0x0018582C
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex++] = frame.Peek();
			return 1;
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x00187658 File Offset: 0x00185858
		public override string ToString()
		{
			return "Dup()";
		}

		// Token: 0x04002412 RID: 9234
		internal static readonly DupInstruction Instance = new DupInstruction();
	}
}
