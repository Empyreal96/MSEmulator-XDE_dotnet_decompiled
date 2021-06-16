using System;

namespace CommandLine
{
	// Token: 0x02000038 RID: 56
	public sealed class RepeatedOptionError : NamedError
	{
		// Token: 0x06000133 RID: 307 RVA: 0x0000532D File Offset: 0x0000352D
		internal RepeatedOptionError(NameInfo nameInfo) : base(ErrorType.RepeatedOptionError, nameInfo)
		{
		}
	}
}
