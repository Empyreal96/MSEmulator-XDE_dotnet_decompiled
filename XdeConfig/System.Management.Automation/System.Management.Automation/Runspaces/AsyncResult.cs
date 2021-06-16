using System;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001E8 RID: 488
	internal class AsyncResult : IAsyncResult
	{
		// Token: 0x06001650 RID: 5712 RVA: 0x0008F029 File Offset: 0x0008D229
		internal AsyncResult(Guid ownerId, AsyncCallback callback, object state)
		{
			this.ownerId = ownerId;
			this.callback = callback;
			this.state = state;
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001651 RID: 5713 RVA: 0x0008F051 File Offset: 0x0008D251
		public bool CompletedSynchronously
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x0008F054 File Offset: 0x0008D254
		public bool IsCompleted
		{
			get
			{
				return this.isCompleted;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x0008F05C File Offset: 0x0008D25C
		public object AsyncState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x0008F064 File Offset: 0x0008D264
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this.completedWaitHandle == null)
				{
					lock (this.syncObject)
					{
						if (this.completedWaitHandle == null)
						{
							this.completedWaitHandle = new ManualResetEvent(this.isCompleted);
						}
					}
				}
				return this.completedWaitHandle;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x0008F0C8 File Offset: 0x0008D2C8
		internal Guid OwnerId
		{
			get
			{
				return this.ownerId;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x0008F0D0 File Offset: 0x0008D2D0
		internal Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x0008F0D8 File Offset: 0x0008D2D8
		internal AsyncCallback Callback
		{
			get
			{
				return this.callback;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x0008F0E0 File Offset: 0x0008D2E0
		internal object SyncObject
		{
			get
			{
				return this.syncObject;
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0008F0E8 File Offset: 0x0008D2E8
		internal void SetAsCompleted(Exception exception)
		{
			if (this.isCompleted)
			{
				return;
			}
			lock (this.syncObject)
			{
				if (this.isCompleted)
				{
					return;
				}
				this.exception = exception;
				this.isCompleted = true;
				this.SignalWaitHandle();
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0008F160 File Offset: 0x0008D360
		internal void Release()
		{
			if (!this.isCompleted)
			{
				this.isCompleted = true;
				this.SignalWaitHandle();
			}
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0008F178 File Offset: 0x0008D378
		internal void SignalWaitHandle()
		{
			lock (this.syncObject)
			{
				if (this.completedWaitHandle != null)
				{
					this.completedWaitHandle.Set();
				}
			}
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0008F1C8 File Offset: 0x0008D3C8
		internal void EndInvoke()
		{
			this.invokeOnThreadEvent = new AutoResetEvent(false);
			WaitHandle[] waitHandles = new WaitHandle[]
			{
				this.AsyncWaitHandle,
				this.invokeOnThreadEvent
			};
			bool flag = true;
			while (flag)
			{
				if (WaitHandle.WaitAny(waitHandles) == 0)
				{
					flag = false;
				}
				else
				{
					try
					{
						this.invokeCallback(this.invokeCallbackState);
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
			}
			this.AsyncWaitHandle.Dispose();
			this.completedWaitHandle = null;
			this.invokeOnThreadEvent.Dispose();
			this.invokeOnThreadEvent = null;
			if (this.exception != null)
			{
				throw this.exception;
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0008F274 File Offset: 0x0008D474
		internal bool InvokeCallbackOnThread(WaitCallback callback, object state)
		{
			if (callback == null)
			{
				throw new PSArgumentNullException("callback");
			}
			this.invokeCallback = callback;
			this.invokeCallbackState = state;
			if (this.invokeOnThreadEvent != null)
			{
				this.invokeOnThreadEvent.Set();
				return true;
			}
			return false;
		}

		// Token: 0x0400097C RID: 2428
		private Guid ownerId;

		// Token: 0x0400097D RID: 2429
		private bool isCompleted;

		// Token: 0x0400097E RID: 2430
		private ManualResetEvent completedWaitHandle;

		// Token: 0x0400097F RID: 2431
		private Exception exception;

		// Token: 0x04000980 RID: 2432
		private AsyncCallback callback;

		// Token: 0x04000981 RID: 2433
		private object state;

		// Token: 0x04000982 RID: 2434
		private object syncObject = new object();

		// Token: 0x04000983 RID: 2435
		private AutoResetEvent invokeOnThreadEvent;

		// Token: 0x04000984 RID: 2436
		private WaitCallback invokeCallback;

		// Token: 0x04000985 RID: 2437
		private object invokeCallbackState;
	}
}
