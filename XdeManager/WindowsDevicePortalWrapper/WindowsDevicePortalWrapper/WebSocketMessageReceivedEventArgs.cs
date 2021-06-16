using System;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000013 RID: 19
	public class WebSocketMessageReceivedEventArgs<T> : EventArgs
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00007825 File Offset: 0x00005A25
		internal WebSocketMessageReceivedEventArgs(T message)
		{
			this.Message = message;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00007834 File Offset: 0x00005A34
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000783C File Offset: 0x00005A3C
		public T Message { get; private set; }
	}
}
