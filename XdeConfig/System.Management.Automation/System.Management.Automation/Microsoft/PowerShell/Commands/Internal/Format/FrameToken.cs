using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000930 RID: 2352
	internal sealed class FrameToken : FormatToken
	{
		// Token: 0x04002EC1 RID: 11969
		internal ComplexControlItemDefinition itemDefinition = new ComplexControlItemDefinition();

		// Token: 0x04002EC2 RID: 11970
		internal FrameInfoDefinition frameInfoDefinition = new FrameInfoDefinition();
	}
}
