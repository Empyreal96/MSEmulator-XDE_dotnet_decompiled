using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000748 RID: 1864
	internal sealed class DefaultValueInstruction<T> : Instruction
	{
		// Token: 0x06004AB8 RID: 19128 RVA: 0x0018805E File Offset: 0x0018625E
		internal DefaultValueInstruction()
		{
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06004AB9 RID: 19129 RVA: 0x00188066 File Offset: 0x00186266
		public override int ConsumedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06004ABA RID: 19130 RVA: 0x00188069 File Offset: 0x00186269
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0018806C File Offset: 0x0018626C
		public override int Run(InterpretedFrame frame)
		{
			frame.Push(default(T));
			return 1;
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0018808E File Offset: 0x0018628E
		public override string ToString()
		{
			return "New " + typeof(T);
		}
	}
}
