using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000918 RID: 2328
	internal class XmlLoaderLoggerEntry
	{
		// Token: 0x04002E7C RID: 11900
		internal XmlLoaderLoggerEntry.EntryType entryType;

		// Token: 0x04002E7D RID: 11901
		internal string filePath;

		// Token: 0x04002E7E RID: 11902
		internal string xPath;

		// Token: 0x04002E7F RID: 11903
		internal string message;

		// Token: 0x04002E80 RID: 11904
		internal bool failToLoadFile;

		// Token: 0x02000919 RID: 2329
		internal enum EntryType
		{
			// Token: 0x04002E82 RID: 11906
			Error,
			// Token: 0x04002E83 RID: 11907
			Trace
		}
	}
}
