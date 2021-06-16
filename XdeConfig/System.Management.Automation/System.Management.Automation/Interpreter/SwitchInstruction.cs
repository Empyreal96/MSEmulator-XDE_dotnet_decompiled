using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200068F RID: 1679
	internal sealed class SwitchInstruction : Instruction
	{
		// Token: 0x06004709 RID: 18185 RVA: 0x0017AC26 File Offset: 0x00178E26
		internal SwitchInstruction(Dictionary<int, int> cases)
		{
			this._cases = cases;
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x0600470A RID: 18186 RVA: 0x0017AC35 File Offset: 0x00178E35
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x0600470B RID: 18187 RVA: 0x0017AC38 File Offset: 0x00178E38
		public override int ProducedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0017AC3C File Offset: 0x00178E3C
		public override int Run(InterpretedFrame frame)
		{
			int result;
			if (!this._cases.TryGetValue((int)frame.Pop(), out result))
			{
				return 1;
			}
			return result;
		}

		// Token: 0x040022DD RID: 8925
		private readonly Dictionary<int, int> _cases;
	}
}
