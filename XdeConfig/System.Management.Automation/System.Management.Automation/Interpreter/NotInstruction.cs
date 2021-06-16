using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006CC RID: 1740
	internal sealed class NotInstruction : Instruction
	{
		// Token: 0x060047E0 RID: 18400 RVA: 0x0017D072 File Offset: 0x0017B272
		private NotInstruction()
		{
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060047E1 RID: 18401 RVA: 0x0017D07A File Offset: 0x0017B27A
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060047E2 RID: 18402 RVA: 0x0017D07D File Offset: 0x0017B27D
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x0017D080 File Offset: 0x0017B280
		public override int Run(InterpretedFrame frame)
		{
			frame.Push(((bool)frame.Pop()) ? ScriptingRuntimeHelpers.False : ScriptingRuntimeHelpers.True);
			return 1;
		}

		// Token: 0x04002320 RID: 8992
		public static readonly Instruction Instance = new NotInstruction();
	}
}
