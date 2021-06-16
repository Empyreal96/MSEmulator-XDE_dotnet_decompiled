using System;

namespace CommandLine
{
	// Token: 0x02000036 RID: 54
	public sealed class BadFormatConversionError : NamedError
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00005319 File Offset: 0x00003519
		internal BadFormatConversionError(NameInfo nameInfo) : base(ErrorType.BadFormatConversionError, nameInfo)
		{
		}
	}
}
