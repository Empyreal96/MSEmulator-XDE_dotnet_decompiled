using System;

namespace CommandLine
{
	// Token: 0x02000037 RID: 55
	public sealed class SequenceOutOfRangeError : NamedError
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00005323 File Offset: 0x00003523
		internal SequenceOutOfRangeError(NameInfo nameInfo) : base(ErrorType.SequenceOutOfRangeError, nameInfo)
		{
		}
	}
}
