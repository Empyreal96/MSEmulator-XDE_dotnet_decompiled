using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001C RID: 28
	public class MessageEventArgs : EventArgs
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00002068 File Offset: 0x00000268
		public MessageEventArgs(string message)
		{
			this.Message = message;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00002077 File Offset: 0x00000277
		// (set) Token: 0x0600009F RID: 159 RVA: 0x0000207F File Offset: 0x0000027F
		public string Message { get; private set; }
	}
}
