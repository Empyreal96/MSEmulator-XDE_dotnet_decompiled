using System;
using System.ComponentModel;
using System.Threading;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x0200000F RID: 15
	public class XdeShellReadyPipe : XdePipe, IXdeShellReadyPipe, IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x000055D1 File Offset: 0x000037D1
		protected XdeShellReadyPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeShellReadyPipe.shellReadyNotificationsGuid, XdeShellReadyPipe.PipeName)
		{
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060000EA RID: 234 RVA: 0x000055E4 File Offset: 0x000037E4
		// (remove) Token: 0x060000EB RID: 235 RVA: 0x0000561C File Offset: 0x0000381C
		public event EventHandler ShellReadyEvent;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060000EC RID: 236 RVA: 0x00005654 File Offset: 0x00003854
		// (remove) Token: 0x060000ED RID: 237 RVA: 0x0000568C File Offset: 0x0000388C
		public event EventHandler<ExEventArgs> ErrorEncountered;

		// Token: 0x060000EE RID: 238 RVA: 0x000056C1 File Offset: 0x000038C1
		public static IXdeShellReadyPipe XdeShellReadyPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeShellReadyPipe(addressInfo);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000056C9 File Offset: 0x000038C9
		public void StartListening()
		{
			if (this.listeningThread != null)
			{
				throw new InvalidOperationException("StartListening was already called.");
			}
			this.listeningThread = new Thread(new ThreadStart(this.ListenForNotifications));
			this.stopListening = false;
			this.listeningThread.Start();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005707 File Offset: 0x00003907
		public void StopListening()
		{
			this.stopListening = true;
			if (this.listeningThread != null)
			{
				base.DisconnectFromGuest();
				this.listeningThread.Join();
				this.listeningThread = null;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005730 File Offset: 0x00003930
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopListening();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005744 File Offset: 0x00003944
		private void ListenForNotifications()
		{
			for (;;)
			{
				int num = 0;
				try
				{
					num = base.ReceiveIntFromGuest();
				}
				catch (Exception ex)
				{
					if (!this.stopListening && this.ErrorEncountered != null && this.ErrorEncountered != null)
					{
						this.ErrorEncountered(this, new ExEventArgs(ex));
					}
					break;
				}
				if (this.stopListening)
				{
					break;
				}
				XdeShellReadyPipe.ShellReadyNotificationType shellReadyNotificationType = (XdeShellReadyPipe.ShellReadyNotificationType)num;
				if (shellReadyNotificationType == XdeShellReadyPipe.ShellReadyNotificationType.ShellReadyNotificationEvent)
				{
					this.GenerateNotificationEvent(this.ShellReadyEvent);
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00002CF8 File Offset: 0x00000EF8
		private void GenerateNotificationEvent(EventHandler eventHandler)
		{
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x0400003F RID: 63
		public static readonly string PipeName = typeof(XdeShellReadyPipe).Name;

		// Token: 0x04000040 RID: 64
		private static readonly Guid shellReadyNotificationsGuid = new Guid("{b38ed84d-a05f-4a87-a1d7-4c626f36e3C6}");

		// Token: 0x04000041 RID: 65
		private Thread listeningThread;

		// Token: 0x04000042 RID: 66
		private bool stopListening;

		// Token: 0x02000027 RID: 39
		private enum ShellReadyNotificationType
		{
			// Token: 0x040000A1 RID: 161
			ShellReadyNotificationEvent = 1
		}
	}
}
