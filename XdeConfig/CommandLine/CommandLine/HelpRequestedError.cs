using System;

namespace CommandLine
{
	// Token: 0x0200003A RID: 58
	public sealed class HelpRequestedError : Error
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00005342 File Offset: 0x00003542
		internal HelpRequestedError() : base(ErrorType.HelpRequestedError, true)
		{
		}
	}
}
