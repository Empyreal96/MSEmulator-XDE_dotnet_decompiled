using System;

namespace CommandLine
{
	// Token: 0x0200002D RID: 45
	public enum ErrorType
	{
		// Token: 0x0400004C RID: 76
		BadFormatTokenError,
		// Token: 0x0400004D RID: 77
		MissingValueOptionError,
		// Token: 0x0400004E RID: 78
		UnknownOptionError,
		// Token: 0x0400004F RID: 79
		MissingRequiredOptionError,
		// Token: 0x04000050 RID: 80
		MutuallyExclusiveSetError,
		// Token: 0x04000051 RID: 81
		BadFormatConversionError,
		// Token: 0x04000052 RID: 82
		SequenceOutOfRangeError,
		// Token: 0x04000053 RID: 83
		RepeatedOptionError,
		// Token: 0x04000054 RID: 84
		NoVerbSelectedError,
		// Token: 0x04000055 RID: 85
		BadVerbSelectedError,
		// Token: 0x04000056 RID: 86
		HelpRequestedError,
		// Token: 0x04000057 RID: 87
		HelpVerbRequestedError,
		// Token: 0x04000058 RID: 88
		VersionRequestedError,
		// Token: 0x04000059 RID: 89
		SetValueExceptionError,
		// Token: 0x0400005A RID: 90
		InvalidAttributeConfigurationError
	}
}
