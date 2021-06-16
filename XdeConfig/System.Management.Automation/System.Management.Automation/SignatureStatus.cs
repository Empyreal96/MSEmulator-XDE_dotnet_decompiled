using System;

namespace System.Management.Automation
{
	// Token: 0x02000802 RID: 2050
	public enum SignatureStatus
	{
		// Token: 0x04002866 RID: 10342
		Valid,
		// Token: 0x04002867 RID: 10343
		UnknownError,
		// Token: 0x04002868 RID: 10344
		NotSigned,
		// Token: 0x04002869 RID: 10345
		HashMismatch,
		// Token: 0x0400286A RID: 10346
		NotTrusted,
		// Token: 0x0400286B RID: 10347
		NotSupportedFileFormat,
		// Token: 0x0400286C RID: 10348
		Incompatible
	}
}
