using System;

namespace CommandLine
{
	// Token: 0x02000030 RID: 48
	public sealed class BadFormatTokenError : TokenError
	{
		// Token: 0x06000126 RID: 294 RVA: 0x00005236 File Offset: 0x00003436
		internal BadFormatTokenError(string token) : base(ErrorType.BadFormatTokenError, token)
		{
		}
	}
}
