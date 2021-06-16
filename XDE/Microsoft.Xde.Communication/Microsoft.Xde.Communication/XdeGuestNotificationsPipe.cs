using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000007 RID: 7
	public class XdeGuestNotificationsPipe : XdePipe, IXdeGuestNotificationsPipe, IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable, IXdeAutomationGuestNotificationsPipe
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002951 File Offset: 0x00000B51
		protected XdeGuestNotificationsPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeGuestNotificationsPipe.guestNotificationsGuid)
		{
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600004C RID: 76 RVA: 0x00002960 File Offset: 0x00000B60
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x00002998 File Offset: 0x00000B98
		public event EventHandler MicrophoneStarted;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600004E RID: 78 RVA: 0x000029D0 File Offset: 0x00000BD0
		// (remove) Token: 0x0600004F RID: 79 RVA: 0x00002A08 File Offset: 0x00000C08
		public event EventHandler MicrophoneStopped;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000050 RID: 80 RVA: 0x00002A40 File Offset: 0x00000C40
		// (remove) Token: 0x06000051 RID: 81 RVA: 0x00002A78 File Offset: 0x00000C78
		public event EventHandler<GuestUpdatedEventArgs> GuestUpdated;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000052 RID: 82 RVA: 0x00002AB0 File Offset: 0x00000CB0
		// (remove) Token: 0x06000053 RID: 83 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public event EventHandler<ExEventArgs> ErrorEncountered;

		// Token: 0x06000054 RID: 84 RVA: 0x00002B1D File Offset: 0x00000D1D
		public static IXdeGuestNotificationsPipe XdeGuestNotificationsPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeGuestNotificationsPipe(addressInfo);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002B25 File Offset: 0x00000D25
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

		// Token: 0x06000056 RID: 86 RVA: 0x00002B63 File Offset: 0x00000D63
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

		// Token: 0x06000057 RID: 87 RVA: 0x00002B8C File Offset: 0x00000D8C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopListening();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002BA0 File Offset: 0x00000DA0
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
				switch (num)
				{
				case 2:
					this.GenerateNotificationEvent(this.MicrophoneStarted);
					break;
				case 3:
					this.GenerateNotificationEvent(this.MicrophoneStopped);
					break;
				case 4:
					try
					{
						byte[] array = new byte[base.ReceiveIntFromGuest()];
						base.ReceiveFromGuest(array);
						string @string = Encoding.Unicode.GetString(array, 0, array.Length);
						byte[] array2 = new byte[base.ReceiveIntFromGuest()];
						base.ReceiveFromGuest(array2);
						string string2 = Encoding.Unicode.GetString(array2, 0, array2.Length);
						if (!string.IsNullOrEmpty(@string) && !string.IsNullOrEmpty(string2) && this.GuestUpdated != null)
						{
							this.GuestUpdated(this, new GuestUpdatedEventArgs(@string, string2));
						}
						break;
					}
					catch (Exception ex2)
					{
						if (!this.stopListening && this.ErrorEncountered != null && this.ErrorEncountered != null)
						{
							this.ErrorEncountered(this, new ExEventArgs(ex2));
						}
						break;
					}
					return;
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002CF8 File Offset: 0x00000EF8
		private void GenerateNotificationEvent(EventHandler eventHandler)
		{
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x0400000D RID: 13
		private static readonly Guid guestNotificationsGuid = new Guid("{e34f8a1d-166c-44ae-9085-c68462b66099}");

		// Token: 0x0400000E RID: 14
		private Thread listeningThread;

		// Token: 0x0400000F RID: 15
		private bool stopListening;

		// Token: 0x02000012 RID: 18
		private enum GuestNotificationType
		{
			// Token: 0x04000048 RID: 72
			MicrophoneStart = 2,
			// Token: 0x04000049 RID: 73
			MicrophoneStop,
			// Token: 0x0400004A RID: 74
			GenericEvent
		}
	}
}
