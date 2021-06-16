using System;

namespace CommandLine
{
	// Token: 0x02000032 RID: 50
	public sealed class MissingValueOptionError : NamedError
	{
		// Token: 0x0600012C RID: 300 RVA: 0x000052E2 File Offset: 0x000034E2
		internal MissingValueOptionError(NameInfo nameInfo) : base(ErrorType.MissingValueOptionError, nameInfo)
		{
		}
	}
}
