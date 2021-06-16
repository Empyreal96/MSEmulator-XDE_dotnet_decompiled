using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008DF RID: 2271
	public sealed class NullWriter : BaseChannelWriter
	{
		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x06005588 RID: 21896 RVA: 0x001C2314 File Offset: 0x001C0514
		public static BaseChannelWriter Instance
		{
			get
			{
				return NullWriter.nullWriter;
			}
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x001C231B File Offset: 0x001C051B
		private NullWriter()
		{
		}

		// Token: 0x04002D4A RID: 11594
		private static readonly BaseChannelWriter nullWriter = new NullWriter();
	}
}
