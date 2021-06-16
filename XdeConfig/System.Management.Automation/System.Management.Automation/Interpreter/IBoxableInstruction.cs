using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006F1 RID: 1777
	internal interface IBoxableInstruction
	{
		// Token: 0x0600499E RID: 18846
		Instruction BoxIfIndexMatches(int index);
	}
}
