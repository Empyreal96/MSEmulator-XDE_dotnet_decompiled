using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004B9 RID: 1209
	internal sealed class ComplexSpecificParameters : ShapeSpecificParameters
	{
		// Token: 0x04001B5A RID: 7002
		internal const int maxDepthAllowable = 5;

		// Token: 0x04001B5B RID: 7003
		internal ComplexSpecificParameters.ClassInfoDisplay classDisplay = ComplexSpecificParameters.ClassInfoDisplay.shortName;

		// Token: 0x04001B5C RID: 7004
		internal int maxDepth = 5;

		// Token: 0x020004BA RID: 1210
		internal enum ClassInfoDisplay
		{
			// Token: 0x04001B5E RID: 7006
			none,
			// Token: 0x04001B5F RID: 7007
			fullName,
			// Token: 0x04001B60 RID: 7008
			shortName
		}
	}
}
