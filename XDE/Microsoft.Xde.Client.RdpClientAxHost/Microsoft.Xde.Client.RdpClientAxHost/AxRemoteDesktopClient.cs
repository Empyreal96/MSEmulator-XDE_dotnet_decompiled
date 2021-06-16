using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200004E RID: 78
	[DesignTimeVisible(true)]
	[AxHost.ClsidAttribute("{eab16c5d-eed1-4e95-868b-0fba1b42c092}")]
	public class AxRemoteDesktopClient : AxHost
	{
		// Token: 0x06000E63 RID: 3683 RVA: 0x000266DD File Offset: 0x000248DD
		public AxRemoteDesktopClient() : base("eab16c5d-eed1-4e95-868b-0fba1b42c092")
		{
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x000266EA File Offset: 0x000248EA
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(710)]
		public virtual IRemoteDesktopClientSettings Settings
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Settings", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Settings;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0002670B File Offset: 0x0002490B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(711)]
		[Browsable(false)]
		public virtual IRemoteDesktopClientActions Actions
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Actions", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Actions;
			}
		}

		// Token: 0x14000277 RID: 631
		// (add) Token: 0x06000E66 RID: 3686 RVA: 0x0002672C File Offset: 0x0002492C
		// (remove) Token: 0x06000E67 RID: 3687 RVA: 0x00026764 File Offset: 0x00024964
		public event EventHandler OnConnecting;

		// Token: 0x14000278 RID: 632
		// (add) Token: 0x06000E68 RID: 3688 RVA: 0x0002679C File Offset: 0x0002499C
		// (remove) Token: 0x06000E69 RID: 3689 RVA: 0x000267D4 File Offset: 0x000249D4
		public event EventHandler OnConnected;

		// Token: 0x14000279 RID: 633
		// (add) Token: 0x06000E6A RID: 3690 RVA: 0x0002680C File Offset: 0x00024A0C
		// (remove) Token: 0x06000E6B RID: 3691 RVA: 0x00026844 File Offset: 0x00024A44
		public event EventHandler OnLoginCompleted;

		// Token: 0x1400027A RID: 634
		// (add) Token: 0x06000E6C RID: 3692 RVA: 0x0002687C File Offset: 0x00024A7C
		// (remove) Token: 0x06000E6D RID: 3693 RVA: 0x000268B4 File Offset: 0x00024AB4
		public event IRemoteDesktopClientEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400027B RID: 635
		// (add) Token: 0x06000E6E RID: 3694 RVA: 0x000268EC File Offset: 0x00024AEC
		// (remove) Token: 0x06000E6F RID: 3695 RVA: 0x00026924 File Offset: 0x00024B24
		public event IRemoteDesktopClientEvents_OnStatusChangedEventHandler OnStatusChanged;

		// Token: 0x1400027C RID: 636
		// (add) Token: 0x06000E70 RID: 3696 RVA: 0x0002695C File Offset: 0x00024B5C
		// (remove) Token: 0x06000E71 RID: 3697 RVA: 0x00026994 File Offset: 0x00024B94
		public event IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400027D RID: 637
		// (add) Token: 0x06000E72 RID: 3698 RVA: 0x000269CC File Offset: 0x00024BCC
		// (remove) Token: 0x06000E73 RID: 3699 RVA: 0x00026A04 File Offset: 0x00024C04
		public event EventHandler OnAutoReconnected;

		// Token: 0x1400027E RID: 638
		// (add) Token: 0x06000E74 RID: 3700 RVA: 0x00026A3C File Offset: 0x00024C3C
		// (remove) Token: 0x06000E75 RID: 3701 RVA: 0x00026A74 File Offset: 0x00024C74
		public event EventHandler OnDialogDisplaying;

		// Token: 0x1400027F RID: 639
		// (add) Token: 0x06000E76 RID: 3702 RVA: 0x00026AAC File Offset: 0x00024CAC
		// (remove) Token: 0x06000E77 RID: 3703 RVA: 0x00026AE4 File Offset: 0x00024CE4
		public event EventHandler OnDialogDismissed;

		// Token: 0x14000280 RID: 640
		// (add) Token: 0x06000E78 RID: 3704 RVA: 0x00026B1C File Offset: 0x00024D1C
		// (remove) Token: 0x06000E79 RID: 3705 RVA: 0x00026B54 File Offset: 0x00024D54
		public event IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000281 RID: 641
		// (add) Token: 0x06000E7A RID: 3706 RVA: 0x00026B8C File Offset: 0x00024D8C
		// (remove) Token: 0x06000E7B RID: 3707 RVA: 0x00026BC4 File Offset: 0x00024DC4
		public event IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler OnAdminMessageReceived;

		// Token: 0x14000282 RID: 642
		// (add) Token: 0x06000E7C RID: 3708 RVA: 0x00026BFC File Offset: 0x00024DFC
		// (remove) Token: 0x06000E7D RID: 3709 RVA: 0x00026C34 File Offset: 0x00024E34
		public event IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler OnKeyCombinationPressed;

		// Token: 0x14000283 RID: 643
		// (add) Token: 0x06000E7E RID: 3710 RVA: 0x00026C6C File Offset: 0x00024E6C
		// (remove) Token: 0x06000E7F RID: 3711 RVA: 0x00026CA4 File Offset: 0x00024EA4
		public event IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler OnRemoteDesktopSizeChanged;

		// Token: 0x06000E80 RID: 3712 RVA: 0x00026CD9 File Offset: 0x00024ED9
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00026CFA File Offset: 0x00024EFA
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00026D1B File Offset: 0x00024F1B
		public virtual void DeleteSavedCredentials(string serverName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("DeleteSavedCredentials", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.DeleteSavedCredentials(serverName);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00026D40 File Offset: 0x00024F40
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxRemoteDesktopClientEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IRemoteDesktopClientEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00026D90 File Offset: 0x00024F90
		protected override void DetachSink()
		{
			try
			{
				this.cookie.Disconnect();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00026DC0 File Offset: 0x00024FC0
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IRemoteDesktopClient)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00026DF4 File Offset: 0x00024FF4
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00026E0B File Offset: 0x0002500B
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00026E22 File Offset: 0x00025022
		internal void RaiseOnOnLoginCompleted(object sender, EventArgs e)
		{
			if (this.OnLoginCompleted != null)
			{
				this.OnLoginCompleted(sender, e);
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00026E39 File Offset: 0x00025039
		internal void RaiseOnOnDisconnected(object sender, IRemoteDesktopClientEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00026E50 File Offset: 0x00025050
		internal void RaiseOnOnStatusChanged(object sender, IRemoteDesktopClientEvents_OnStatusChangedEvent e)
		{
			if (this.OnStatusChanged != null)
			{
				this.OnStatusChanged(sender, e);
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00026E67 File Offset: 0x00025067
		internal void RaiseOnOnAutoReconnecting(object sender, IRemoteDesktopClientEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00026E7E File Offset: 0x0002507E
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00026E95 File Offset: 0x00025095
		internal void RaiseOnOnDialogDisplaying(object sender, EventArgs e)
		{
			if (this.OnDialogDisplaying != null)
			{
				this.OnDialogDisplaying(sender, e);
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00026EAC File Offset: 0x000250AC
		internal void RaiseOnOnDialogDismissed(object sender, EventArgs e)
		{
			if (this.OnDialogDismissed != null)
			{
				this.OnDialogDismissed(sender, e);
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00026EC3 File Offset: 0x000250C3
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00026EDA File Offset: 0x000250DA
		internal void RaiseOnOnAdminMessageReceived(object sender, IRemoteDesktopClientEvents_OnAdminMessageReceivedEvent e)
		{
			if (this.OnAdminMessageReceived != null)
			{
				this.OnAdminMessageReceived(sender, e);
			}
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00026EF1 File Offset: 0x000250F1
		internal void RaiseOnOnKeyCombinationPressed(object sender, IRemoteDesktopClientEvents_OnKeyCombinationPressedEvent e)
		{
			if (this.OnKeyCombinationPressed != null)
			{
				this.OnKeyCombinationPressed(sender, e);
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00026F08 File Offset: 0x00025108
		internal void RaiseOnOnRemoteDesktopSizeChanged(object sender, IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEvent e)
		{
			if (this.OnRemoteDesktopSizeChanged != null)
			{
				this.OnRemoteDesktopSizeChanged(sender, e);
			}
		}

		// Token: 0x040002E6 RID: 742
		private IRemoteDesktopClient ocx;

		// Token: 0x040002E7 RID: 743
		private AxRemoteDesktopClientEventMulticaster eventMulticaster;

		// Token: 0x040002E8 RID: 744
		private AxHost.ConnectionPointCookie cookie;
	}
}
