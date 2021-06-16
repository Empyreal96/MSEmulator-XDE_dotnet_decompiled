using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000893 RID: 2195
	internal class PSDataCollectionWriter<T> : ObjectWriter
	{
		// Token: 0x06005431 RID: 21553 RVA: 0x001BCDCC File Offset: 0x001BAFCC
		public PSDataCollectionWriter(PSDataCollectionStream<T> stream) : base(stream)
		{
		}
	}
}
