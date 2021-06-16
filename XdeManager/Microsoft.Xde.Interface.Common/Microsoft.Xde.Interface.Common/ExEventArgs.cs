using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000006 RID: 6
	public class ExEventArgs : EventArgs
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002048 File Offset: 0x00000248
		public ExEventArgs(Exception ex)
		{
			this.ExceptionData = ex;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002057 File Offset: 0x00000257
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000205F File Offset: 0x0000025F
		public Exception ExceptionData { get; private set; }
	}
}
