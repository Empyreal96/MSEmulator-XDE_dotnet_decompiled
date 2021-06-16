using System;

namespace CommandLine
{
	// Token: 0x02000033 RID: 51
	public sealed class UnknownOptionError : TokenError
	{
		// Token: 0x0600012D RID: 301 RVA: 0x000052EC File Offset: 0x000034EC
		internal UnknownOptionError(string token) : base(ErrorType.UnknownOptionError, token)
		{
		}
	}
}
