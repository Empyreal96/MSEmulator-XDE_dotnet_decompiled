using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000732 RID: 1842
	internal sealed class PopInstruction : Instruction
	{
		// Token: 0x06004A7A RID: 19066 RVA: 0x001875F6 File Offset: 0x001857F6
		private PopInstruction()
		{
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06004A7B RID: 19067 RVA: 0x001875FE File Offset: 0x001857FE
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x00187601 File Offset: 0x00185801
		public override int Run(InterpretedFrame frame)
		{
			frame.Pop();
			return 1;
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x0018760B File Offset: 0x0018580B
		public override string ToString()
		{
			return "Pop()";
		}

		// Token: 0x04002411 RID: 9233
		internal static readonly PopInstruction Instance = new PopInstruction();
	}
}
