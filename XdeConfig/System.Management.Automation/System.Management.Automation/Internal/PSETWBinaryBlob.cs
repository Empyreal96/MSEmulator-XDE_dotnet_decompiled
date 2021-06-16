using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200025F RID: 607
	internal sealed class PSETWBinaryBlob
	{
		// Token: 0x06001C60 RID: 7264 RVA: 0x000A5095 File Offset: 0x000A3295
		public PSETWBinaryBlob(byte[] blob, int offset, int length)
		{
			this.blob = blob;
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x04000C5C RID: 3164
		public readonly byte[] blob;

		// Token: 0x04000C5D RID: 3165
		public readonly int offset;

		// Token: 0x04000C5E RID: 3166
		public readonly int length;
	}
}
