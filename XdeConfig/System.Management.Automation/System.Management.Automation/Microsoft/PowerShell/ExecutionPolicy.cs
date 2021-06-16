using System;

namespace Microsoft.PowerShell
{
	// Token: 0x02000805 RID: 2053
	public enum ExecutionPolicy
	{
		// Token: 0x0400287B RID: 10363
		Unrestricted,
		// Token: 0x0400287C RID: 10364
		RemoteSigned,
		// Token: 0x0400287D RID: 10365
		AllSigned,
		// Token: 0x0400287E RID: 10366
		Restricted,
		// Token: 0x0400287F RID: 10367
		Bypass,
		// Token: 0x04002880 RID: 10368
		Undefined,
		// Token: 0x04002881 RID: 10369
		Default = 3
	}
}
