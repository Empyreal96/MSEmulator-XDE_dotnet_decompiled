using System;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020008D5 RID: 2261
	public interface IBackgroundDispatcher
	{
		// Token: 0x06005569 RID: 21865
		bool QueueUserWorkItem(WaitCallback callback);

		// Token: 0x0600556A RID: 21866
		bool QueueUserWorkItem(WaitCallback callback, object state);

		// Token: 0x0600556B RID: 21867
		IAsyncResult BeginInvoke(WaitCallback callback, object state, AsyncCallback completionCallback, object asyncState);

		// Token: 0x0600556C RID: 21868
		void EndInvoke(IAsyncResult asyncResult);
	}
}
