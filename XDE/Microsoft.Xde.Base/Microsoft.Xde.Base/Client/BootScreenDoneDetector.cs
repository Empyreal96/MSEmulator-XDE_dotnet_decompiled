using System;
using System.Drawing;
using System.Threading;
using Microsoft.Xde.Common;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000003 RID: 3
	public sealed class BootScreenDoneDetector : IDisposable
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002180 File Offset: 0x00000380
		public BootScreenDoneDetector(IXdeVirtualMachine machine, Size requestedResolution, bool usingSnapshot)
		{
			this.machine = machine;
			this.usingSnapshot = usingSnapshot;
			this.requestedResolution = requestedResolution;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000008 RID: 8 RVA: 0x000021AC File Offset: 0x000003AC
		// (remove) Token: 0x06000009 RID: 9 RVA: 0x000021E4 File Offset: 0x000003E4
		public event EventHandler BootComplete;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000A RID: 10 RVA: 0x0000221C File Offset: 0x0000041C
		// (remove) Token: 0x0600000B RID: 11 RVA: 0x00002254 File Offset: 0x00000454
		public event EventHandler BootFailed;

		// Token: 0x0600000C RID: 12 RVA: 0x00002289 File Offset: 0x00000489
		public bool IsValidResolution(Size resolution)
		{
			return resolution == this.requestedResolution;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002297 File Offset: 0x00000497
		public void Start()
		{
			if (this.watcherThread != null)
			{
				throw new InvalidOperationException();
			}
			this.watcherThread = new Thread(new ThreadStart(this.WatcherProc));
			this.watcherThread.Start();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022CC File Offset: 0x000004CC
		public void Stop()
		{
			if (this.watcherThread != null)
			{
				this.stopEvent.Set();
				if (this.watcherThread.IsAlive)
				{
					try
					{
						this.watcherThread.Join();
					}
					catch (ThreadStateException)
					{
					}
				}
				this.watcherThread = null;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002324 File Offset: 0x00000524
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			this.Stop();
			this.stopEvent.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002350 File Offset: 0x00000550
		private void WatcherProc()
		{
			TimeSpan t = this.usingSnapshot ? BootScreenDoneDetector.checkScreenTimeoutSnapshot : BootScreenDoneDetector.checkScreenTimeoutNonSnapshot;
			TimeSpan timeout = this.usingSnapshot ? BootScreenDoneDetector.checkScreenRetryTimeoutSnapshot : BootScreenDoneDetector.checkScreenRetryTimeoutNonSnapshot;
			DateTime now = DateTime.Now;
			while (!this.GetIsBootComplete())
			{
				if (!this.disposed && !this.stopEvent.WaitOne(timeout))
				{
					DateTime now2 = DateTime.Now;
					if (!(now2 < now) && !(now2 - now > t))
					{
						continue;
					}
					if (this.BootFailed != null)
					{
						this.BootFailed(this, EventArgs.Empty);
					}
				}
				return;
			}
			if (this.BootComplete != null)
			{
				this.BootComplete(this, EventArgs.Empty);
				return;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023FC File Offset: 0x000005FC
		private bool GetIsBootComplete()
		{
			if (this.disposed)
			{
				return false;
			}
			Size currentResolution;
			try
			{
				if (this.machine.EnabledState != VirtualMachineEnabledState.Enabled)
				{
					return false;
				}
				currentResolution = this.machine.GetCurrentResolution();
			}
			catch (Exception)
			{
				return false;
			}
			return this.IsValidResolution(currentResolution) || currentResolution.Width > BootScreenDoneDetector.BootScreenSize.Width;
		}

		// Token: 0x04000003 RID: 3
		private static readonly Size BootScreenSize = new Size(1024, 768);

		// Token: 0x04000004 RID: 4
		private static TimeSpan checkScreenTimeoutNonSnapshot = new TimeSpan(0, 5, 0);

		// Token: 0x04000005 RID: 5
		private static TimeSpan checkScreenTimeoutSnapshot = new TimeSpan(0, 1, 0);

		// Token: 0x04000006 RID: 6
		private static TimeSpan checkScreenRetryTimeoutNonSnapshot = new TimeSpan(0, 0, 5);

		// Token: 0x04000007 RID: 7
		private static TimeSpan checkScreenRetryTimeoutSnapshot = new TimeSpan(0, 0, 0, 0, 500);

		// Token: 0x04000008 RID: 8
		private IXdeVirtualMachine machine;

		// Token: 0x04000009 RID: 9
		private ManualResetEvent stopEvent = new ManualResetEvent(false);

		// Token: 0x0400000A RID: 10
		private bool disposed;

		// Token: 0x0400000B RID: 11
		private Thread watcherThread;

		// Token: 0x0400000C RID: 12
		private bool usingSnapshot;

		// Token: 0x0400000D RID: 13
		private Size requestedResolution;
	}
}
