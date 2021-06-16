using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000920 RID: 2336
	internal sealed class DatabaseLoadingInfo
	{
		// Token: 0x04002EA1 RID: 11937
		internal string fileDirectory;

		// Token: 0x04002EA2 RID: 11938
		internal string filePath;

		// Token: 0x04002EA3 RID: 11939
		internal bool isFullyTrusted;

		// Token: 0x04002EA4 RID: 11940
		internal string xPath;

		// Token: 0x04002EA5 RID: 11941
		internal DateTime loadTime = DateTime.Now;
	}
}
