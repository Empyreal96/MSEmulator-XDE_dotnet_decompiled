using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000258 RID: 600
	internal enum PSKeyword : ulong
	{
		// Token: 0x04000BBB RID: 3003
		Runspace = 1UL,
		// Token: 0x04000BBC RID: 3004
		Pipeline,
		// Token: 0x04000BBD RID: 3005
		Protocol = 4UL,
		// Token: 0x04000BBE RID: 3006
		Transport = 8UL,
		// Token: 0x04000BBF RID: 3007
		Host = 16UL,
		// Token: 0x04000BC0 RID: 3008
		Cmdlets = 32UL,
		// Token: 0x04000BC1 RID: 3009
		Serializer = 64UL,
		// Token: 0x04000BC2 RID: 3010
		Session = 128UL,
		// Token: 0x04000BC3 RID: 3011
		ManagedPlugin = 256UL,
		// Token: 0x04000BC4 RID: 3012
		UseAlwaysOperational = 9223372036854775808UL,
		// Token: 0x04000BC5 RID: 3013
		UseAlwaysAnalytic = 4611686018427387904UL
	}
}
