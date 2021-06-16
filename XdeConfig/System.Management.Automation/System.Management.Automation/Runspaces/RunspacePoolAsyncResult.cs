using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000249 RID: 585
	internal sealed class RunspacePoolAsyncResult : AsyncResult
	{
		// Token: 0x06001B8A RID: 7050 RVA: 0x000A1939 File Offset: 0x0009FB39
		internal RunspacePoolAsyncResult(Guid ownerId, AsyncCallback callback, object state, bool isCalledFromOpenAsync) : base(ownerId, callback, state)
		{
			this.isAssociatedWithAsyncOpen = isCalledFromOpenAsync;
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001B8B RID: 7051 RVA: 0x000A194C File Offset: 0x0009FB4C
		internal bool IsAssociatedWithAsyncOpen
		{
			get
			{
				return this.isAssociatedWithAsyncOpen;
			}
		}

		// Token: 0x04000B55 RID: 2901
		private bool isAssociatedWithAsyncOpen;
	}
}
