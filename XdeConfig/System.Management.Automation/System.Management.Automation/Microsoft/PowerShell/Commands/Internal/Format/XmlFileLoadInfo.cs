using System;
using System.Collections.ObjectModel;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200096D RID: 2413
	internal sealed class XmlFileLoadInfo
	{
		// Token: 0x0600587D RID: 22653 RVA: 0x001CC98C File Offset: 0x001CAB8C
		internal XmlFileLoadInfo()
		{
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x001CC994 File Offset: 0x001CAB94
		internal XmlFileLoadInfo(string dir, string path, Collection<string> errors, string psSnapinName)
		{
			this.fileDirectory = dir;
			this.filePath = path;
			this.errors = errors;
			this.psSnapinName = psSnapinName;
		}

		// Token: 0x04002F6E RID: 12142
		internal string fileDirectory;

		// Token: 0x04002F6F RID: 12143
		internal string filePath;

		// Token: 0x04002F70 RID: 12144
		internal Collection<string> errors;

		// Token: 0x04002F71 RID: 12145
		internal string psSnapinName;
	}
}
