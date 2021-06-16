using System;

namespace CommandLine
{
	// Token: 0x02000034 RID: 52
	public sealed class MissingRequiredOptionError : NamedError
	{
		// Token: 0x0600012E RID: 302 RVA: 0x000052F6 File Offset: 0x000034F6
		internal MissingRequiredOptionError(NameInfo nameInfo) : base(ErrorType.MissingRequiredOptionError, nameInfo)
		{
		}
	}
}
