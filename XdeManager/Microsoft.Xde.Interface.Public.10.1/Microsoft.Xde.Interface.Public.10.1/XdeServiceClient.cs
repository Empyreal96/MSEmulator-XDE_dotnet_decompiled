using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.Xde.Interface
{
	// Token: 0x0200000D RID: 13
	public class XdeServiceClient : IXdeInstance, IXdeServiceCallback, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600002A RID: 42 RVA: 0x00002230 File Offset: 0x00000430
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x00002268 File Offset: 0x00000468
		public event ToolPipeReadyHandler PipeReady;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000022A0 File Offset: 0x000004A0
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x000022D8 File Offset: 0x000004D8
		public event XdeRebootHandler XdeReboot;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002310 File Offset: 0x00000510
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x00002348 File Offset: 0x00000548
		public event XdeExitHandler XdeExit;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000237D File Offset: 0x0000057D
		public VirtualMachineEnabledState VmState
		{
			get
			{
				this.EnsureNotDisposed();
				return this.serverProxy.GetVmState();
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002390 File Offset: 0x00000590
		public void Connect(string virtualMachineName)
		{
			this.Connect(virtualMachineName, TimeSpan.FromSeconds(20.0));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023A7 File Offset: 0x000005A7
		public void Connect(string virtualMachineName, TimeSpan timeout)
		{
			this.serverProxy = Utility.Connect<IXdeInstanceContract>(virtualMachineName, this, EndPointType.Normal, timeout, out this.pipeFactory);
			this.serverProxy.RegisterForEvents(EndPointType.Normal);
			this.eventsSubscribed = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000023D4 File Offset: 0x000005D4
		public void Disconnect()
		{
			object obj = this.disconnectLock;
			lock (obj)
			{
				if (this.serverProxy != null)
				{
					IChannel channel = this.serverProxy as IChannel;
					if (channel != null && channel.State == CommunicationState.Opened)
					{
						if (this.eventsSubscribed)
						{
							try
							{
								this.serverProxy.UnRegisterForEvents(EndPointType.Normal);
								this.eventsSubscribed = false;
							}
							catch (Exception)
							{
							}
						}
						Utility.TryCloseOrAbort(channel);
					}
					this.serverProxy = null;
				}
				if (this.pipeFactory != null)
				{
					Utility.TryCloseOrAbort(this.pipeFactory);
					this.pipeFactory = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002488 File Offset: 0x00000688
		public void GetEndPoint(out string hostIP, out string deviceIP)
		{
			this.EnsureNotDisposed();
			this.serverProxy.GetEndPoint(out hostIP, out deviceIP);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000249D File Offset: 0x0000069D
		public void BringToFront()
		{
			this.EnsureNotDisposed();
			this.serverProxy.BringToFront();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000024B0 File Offset: 0x000006B0
		public void Close()
		{
			this.EnsureNotDisposed();
			this.serverProxy.Close();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000024C3 File Offset: 0x000006C3
		public bool IsToolsPipeReady()
		{
			this.EnsureNotDisposed();
			return this.serverProxy.IsToolsPipeReady();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000024D6 File Offset: 0x000006D6
		public void OnDeviceToolsPipeReady()
		{
			ToolPipeReadyHandler pipeReady = this.PipeReady;
			if (pipeReady == null)
			{
				return;
			}
			pipeReady();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000024E8 File Offset: 0x000006E8
		public void OnXdeReboot()
		{
			XdeRebootHandler xdeReboot = this.XdeReboot;
			if (xdeReboot == null)
			{
				return;
			}
			xdeReboot();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000024FA File Offset: 0x000006FA
		public void OnXdeExit()
		{
			this.eventsSubscribed = false;
			this.Disconnect();
			XdeExitHandler xdeExit = this.XdeExit;
			if (xdeExit == null)
			{
				return;
			}
			xdeExit();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002519 File Offset: 0x00000719
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002528 File Offset: 0x00000728
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.Disconnect();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000253B File Offset: 0x0000073B
		private void EnsureNotDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x04000016 RID: 22
		private IXdeInstanceContract serverProxy;

		// Token: 0x04000017 RID: 23
		private ChannelFactory<IXdeInstanceContract> pipeFactory;

		// Token: 0x04000018 RID: 24
		private bool disposed;

		// Token: 0x04000019 RID: 25
		private bool eventsSubscribed;

		// Token: 0x0400001A RID: 26
		private object disconnectLock = new object();
	}
}
