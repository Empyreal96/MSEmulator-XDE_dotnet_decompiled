using System;

namespace CommandLine
{
	// Token: 0x0200003D RID: 61
	public sealed class VersionRequestedError : Error
	{
		// Token: 0x0600013B RID: 315 RVA: 0x0000538E File Offset: 0x0000358E
		internal VersionRequestedError() : base(ErrorType.VersionRequestedError, true)
		{
		}
	}
}
