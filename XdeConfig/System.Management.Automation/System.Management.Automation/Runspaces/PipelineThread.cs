using System;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200021C RID: 540
	internal class PipelineThread : IDisposable
	{
		// Token: 0x0600199A RID: 6554 RVA: 0x0009A2C8 File Offset: 0x000984C8
		internal PipelineThread(ApartmentState apartmentState)
		{
			this.worker = new Thread(new ThreadStart(this.WorkerProc), LocalPipeline.MaxStack);
			this.workItem = null;
			this.workItemReady = new AutoResetEvent(false);
			this.closed = false;
			if (apartmentState != ApartmentState.Unknown)
			{
				this.worker.SetApartmentState(apartmentState);
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600199B RID: 6555 RVA: 0x0009A321 File Offset: 0x00098521
		internal Thread Worker
		{
			get
			{
				return this.worker;
			}
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0009A329 File Offset: 0x00098529
		internal void Start(ThreadStart workItem)
		{
			if (this.closed)
			{
				return;
			}
			this.workItem = workItem;
			this.workItemReady.Set();
			if (this.worker.ThreadState == ThreadState.Unstarted)
			{
				this.worker.Start();
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0009A360 File Offset: 0x00098560
		internal void Close()
		{
			this.Dispose();
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0009A368 File Offset: 0x00098568
		private void WorkerProc()
		{
			while (!this.closed)
			{
				this.workItemReady.WaitOne();
				if (!this.closed)
				{
					this.workItem();
				}
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0009A394 File Offset: 0x00098594
		public void Dispose()
		{
			if (this.closed)
			{
				return;
			}
			this.closed = true;
			this.workItemReady.Set();
			if (this.worker.ThreadState != ThreadState.Unstarted && Thread.CurrentThread != this.worker)
			{
				this.worker.Join();
			}
			this.workItemReady.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0009A3F4 File Offset: 0x000985F4
		~PipelineThread()
		{
			this.Dispose();
		}

		// Token: 0x04000A7C RID: 2684
		private Thread worker;

		// Token: 0x04000A7D RID: 2685
		private ThreadStart workItem;

		// Token: 0x04000A7E RID: 2686
		private AutoResetEvent workItemReady;

		// Token: 0x04000A7F RID: 2687
		private bool closed;
	}
}
