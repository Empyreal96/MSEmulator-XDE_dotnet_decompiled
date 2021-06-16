using System;

namespace CommandLine
{
	// Token: 0x02000039 RID: 57
	public sealed class BadVerbSelectedError : TokenError
	{
		// Token: 0x06000134 RID: 308 RVA: 0x00005337 File Offset: 0x00003537
		internal BadVerbSelectedError(string token) : base(ErrorType.BadVerbSelectedError, token)
		{
		}
	}
}
