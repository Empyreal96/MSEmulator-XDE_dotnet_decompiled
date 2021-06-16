using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000044 RID: 68
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{7390f3d8-0439-4c05-91e3-cf5cb290c3d0}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient6 : AxHost
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x0001D555 File Offset: 0x0001B755
		public AxMsRdpClient6() : base("7390f3d8-0439-4c05-91e3-cf5cb290c3d0")
		{
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0001D562 File Offset: 0x0001B762
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x0001D583 File Offset: 0x0001B783
		[DispId(1)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string Server
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Server", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Server;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Server", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Server = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0001D5A5 File Offset: 0x0001B7A5
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x0001D5C6 File Offset: 0x0001B7C6
		[DispId(2)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string Domain
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Domain", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Domain;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Domain", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Domain = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0001D5E8 File Offset: 0x0001B7E8
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x0001D609 File Offset: 0x0001B809
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(3)]
		public virtual string UserName
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("UserName", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.UserName;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("UserName", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.UserName = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0001D62B File Offset: 0x0001B82B
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x0001D64C File Offset: 0x0001B84C
		[DispId(4)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string DisconnectedText
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("DisconnectedText", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.DisconnectedText;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("DisconnectedText", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.DisconnectedText = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0001D66E File Offset: 0x0001B86E
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x0001D68F File Offset: 0x0001B88F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(5)]
		public virtual string ConnectingText
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ConnectingText", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.ConnectingText;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ConnectingText", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.ConnectingText = value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0001D6B1 File Offset: 0x0001B8B1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(6)]
		public virtual short Connected
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Connected", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Connected;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0001D6D2 File Offset: 0x0001B8D2
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x0001D6F3 File Offset: 0x0001B8F3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(12)]
		public virtual int DesktopWidth
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("DesktopWidth", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.DesktopWidth;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("DesktopWidth", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.DesktopWidth = value;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0001D715 File Offset: 0x0001B915
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x0001D736 File Offset: 0x0001B936
		[DispId(13)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int DesktopHeight
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("DesktopHeight", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.DesktopHeight;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("DesktopHeight", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.DesktopHeight = value;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0001D758 File Offset: 0x0001B958
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x0001D779 File Offset: 0x0001B979
		[DispId(16)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int StartConnected
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("StartConnected", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.StartConnected;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("StartConnected", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.StartConnected = value;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x0001D79B File Offset: 0x0001B99B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(17)]
		public virtual int HorizontalScrollBarVisible
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("HorizontalScrollBarVisible", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.HorizontalScrollBarVisible;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x0001D7BC File Offset: 0x0001B9BC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(18)]
		public virtual int VerticalScrollBarVisible
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("VerticalScrollBarVisible", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.VerticalScrollBarVisible;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x0001D7DD File Offset: 0x0001B9DD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(19)]
		public virtual string FullScreenTitle
		{
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("FullScreenTitle", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.FullScreenTitle = value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0001D7FF File Offset: 0x0001B9FF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(20)]
		public virtual int CipherStrength
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("CipherStrength", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.CipherStrength;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0001D820 File Offset: 0x0001BA20
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(21)]
		public virtual string Version
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Version", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Version;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0001D841 File Offset: 0x0001BA41
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(22)]
		public virtual int SecuredSettingsEnabled
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("SecuredSettingsEnabled", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.SecuredSettingsEnabled;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0001D862 File Offset: 0x0001BA62
		[Browsable(false)]
		[DispId(97)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMsTscSecuredSettings SecuredSettings
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("SecuredSettings", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.SecuredSettings;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0001D883 File Offset: 0x0001BA83
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(98)]
		public virtual IMsTscAdvancedSettings AdvancedSettings
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0001D8A4 File Offset: 0x0001BAA4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(99)]
		[Browsable(false)]
		public virtual IMsTscDebug Debugger
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("Debugger", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Debugger;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0001D8C5 File Offset: 0x0001BAC5
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x0001D8E6 File Offset: 0x0001BAE6
		[DispId(100)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int ColorDepth
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ColorDepth", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.ColorDepth;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ColorDepth", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.ColorDepth = value;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0001D908 File Offset: 0x0001BB08
		[DispId(101)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMsRdpClientAdvancedSettings AdvancedSettings2
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings2", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings2;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0001D929 File Offset: 0x0001BB29
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(102)]
		public virtual IMsRdpClientSecuredSettings SecuredSettings2
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("SecuredSettings2", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.SecuredSettings2;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0001D94A File Offset: 0x0001BB4A
		[DispId(103)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual ExtendedDisconnectReasonCode ExtendedDisconnectReason
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ExtendedDisconnectReason", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.ExtendedDisconnectReason;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0001D96B File Offset: 0x0001BB6B
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0001D98C File Offset: 0x0001BB8C
		[DispId(104)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool FullScreen
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("FullScreen", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.FullScreen;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("FullScreen", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.FullScreen = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0001D9AE File Offset: 0x0001BBAE
		[DispId(200)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMsRdpClientAdvancedSettings2 AdvancedSettings3
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings3", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings3;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0001D9CF File Offset: 0x0001BBCF
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x0001D9F0 File Offset: 0x0001BBF0
		[DispId(201)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string ConnectedStatusText
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ConnectedStatusText", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.ConnectedStatusText;
			}
			set
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("ConnectedStatusText", AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.ConnectedStatusText = value;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0001DA12 File Offset: 0x0001BC12
		[DispId(300)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual IMsRdpClientAdvancedSettings3 AdvancedSettings4
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings4", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings4;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0001DA33 File Offset: 0x0001BC33
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(400)]
		public virtual IMsRdpClientAdvancedSettings4 AdvancedSettings5
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings5", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings5;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0001DA54 File Offset: 0x0001BC54
		[DispId(500)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMsRdpClientTransportSettings TransportSettings
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("TransportSettings", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.TransportSettings;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0001DA75 File Offset: 0x0001BC75
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(502)]
		public virtual IMsRdpClientAdvancedSettings5 AdvancedSettings6
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings6", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings6;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0001DA96 File Offset: 0x0001BC96
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(504)]
		public virtual ITSRemoteProgram RemoteProgram
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("RemoteProgram", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.RemoteProgram;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0001DAB7 File Offset: 0x0001BCB7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(505)]
		public virtual IMsRdpClientShell MsRdpClientShell
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("MsRdpClientShell", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.MsRdpClientShell;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(507)]
		public virtual IMsRdpClientAdvancedSettings6 AdvancedSettings7
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings7", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings7;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0001DAF9 File Offset: 0x0001BCF9
		[DispId(506)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMsRdpClientTransportSettings2 TransportSettings2
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("TransportSettings2", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.TransportSettings2;
			}
		}

		// Token: 0x140001E1 RID: 481
		// (add) Token: 0x06000B03 RID: 2819 RVA: 0x0001DB1C File Offset: 0x0001BD1C
		// (remove) Token: 0x06000B04 RID: 2820 RVA: 0x0001DB54 File Offset: 0x0001BD54
		public event EventHandler OnConnecting;

		// Token: 0x140001E2 RID: 482
		// (add) Token: 0x06000B05 RID: 2821 RVA: 0x0001DB8C File Offset: 0x0001BD8C
		// (remove) Token: 0x06000B06 RID: 2822 RVA: 0x0001DBC4 File Offset: 0x0001BDC4
		public event EventHandler OnConnected;

		// Token: 0x140001E3 RID: 483
		// (add) Token: 0x06000B07 RID: 2823 RVA: 0x0001DBFC File Offset: 0x0001BDFC
		// (remove) Token: 0x06000B08 RID: 2824 RVA: 0x0001DC34 File Offset: 0x0001BE34
		public event EventHandler OnLoginComplete;

		// Token: 0x140001E4 RID: 484
		// (add) Token: 0x06000B09 RID: 2825 RVA: 0x0001DC6C File Offset: 0x0001BE6C
		// (remove) Token: 0x06000B0A RID: 2826 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140001E5 RID: 485
		// (add) Token: 0x06000B0B RID: 2827 RVA: 0x0001DCDC File Offset: 0x0001BEDC
		// (remove) Token: 0x06000B0C RID: 2828 RVA: 0x0001DD14 File Offset: 0x0001BF14
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x140001E6 RID: 486
		// (add) Token: 0x06000B0D RID: 2829 RVA: 0x0001DD4C File Offset: 0x0001BF4C
		// (remove) Token: 0x06000B0E RID: 2830 RVA: 0x0001DD84 File Offset: 0x0001BF84
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x140001E7 RID: 487
		// (add) Token: 0x06000B0F RID: 2831 RVA: 0x0001DDBC File Offset: 0x0001BFBC
		// (remove) Token: 0x06000B10 RID: 2832 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140001E8 RID: 488
		// (add) Token: 0x06000B11 RID: 2833 RVA: 0x0001DE2C File Offset: 0x0001C02C
		// (remove) Token: 0x06000B12 RID: 2834 RVA: 0x0001DE64 File Offset: 0x0001C064
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x140001E9 RID: 489
		// (add) Token: 0x06000B13 RID: 2835 RVA: 0x0001DE9C File Offset: 0x0001C09C
		// (remove) Token: 0x06000B14 RID: 2836 RVA: 0x0001DED4 File Offset: 0x0001C0D4
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140001EA RID: 490
		// (add) Token: 0x06000B15 RID: 2837 RVA: 0x0001DF0C File Offset: 0x0001C10C
		// (remove) Token: 0x06000B16 RID: 2838 RVA: 0x0001DF44 File Offset: 0x0001C144
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140001EB RID: 491
		// (add) Token: 0x06000B17 RID: 2839 RVA: 0x0001DF7C File Offset: 0x0001C17C
		// (remove) Token: 0x06000B18 RID: 2840 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140001EC RID: 492
		// (add) Token: 0x06000B19 RID: 2841 RVA: 0x0001DFEC File Offset: 0x0001C1EC
		// (remove) Token: 0x06000B1A RID: 2842 RVA: 0x0001E024 File Offset: 0x0001C224
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140001ED RID: 493
		// (add) Token: 0x06000B1B RID: 2843 RVA: 0x0001E05C File Offset: 0x0001C25C
		// (remove) Token: 0x06000B1C RID: 2844 RVA: 0x0001E094 File Offset: 0x0001C294
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140001EE RID: 494
		// (add) Token: 0x06000B1D RID: 2845 RVA: 0x0001E0CC File Offset: 0x0001C2CC
		// (remove) Token: 0x06000B1E RID: 2846 RVA: 0x0001E104 File Offset: 0x0001C304
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140001EF RID: 495
		// (add) Token: 0x06000B1F RID: 2847 RVA: 0x0001E13C File Offset: 0x0001C33C
		// (remove) Token: 0x06000B20 RID: 2848 RVA: 0x0001E174 File Offset: 0x0001C374
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140001F0 RID: 496
		// (add) Token: 0x06000B21 RID: 2849 RVA: 0x0001E1AC File Offset: 0x0001C3AC
		// (remove) Token: 0x06000B22 RID: 2850 RVA: 0x0001E1E4 File Offset: 0x0001C3E4
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140001F1 RID: 497
		// (add) Token: 0x06000B23 RID: 2851 RVA: 0x0001E21C File Offset: 0x0001C41C
		// (remove) Token: 0x06000B24 RID: 2852 RVA: 0x0001E254 File Offset: 0x0001C454
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140001F2 RID: 498
		// (add) Token: 0x06000B25 RID: 2853 RVA: 0x0001E28C File Offset: 0x0001C48C
		// (remove) Token: 0x06000B26 RID: 2854 RVA: 0x0001E2C4 File Offset: 0x0001C4C4
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140001F3 RID: 499
		// (add) Token: 0x06000B27 RID: 2855 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		// (remove) Token: 0x06000B28 RID: 2856 RVA: 0x0001E334 File Offset: 0x0001C534
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140001F4 RID: 500
		// (add) Token: 0x06000B29 RID: 2857 RVA: 0x0001E36C File Offset: 0x0001C56C
		// (remove) Token: 0x06000B2A RID: 2858 RVA: 0x0001E3A4 File Offset: 0x0001C5A4
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140001F5 RID: 501
		// (add) Token: 0x06000B2B RID: 2859 RVA: 0x0001E3DC File Offset: 0x0001C5DC
		// (remove) Token: 0x06000B2C RID: 2860 RVA: 0x0001E414 File Offset: 0x0001C614
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140001F6 RID: 502
		// (add) Token: 0x06000B2D RID: 2861 RVA: 0x0001E44C File Offset: 0x0001C64C
		// (remove) Token: 0x06000B2E RID: 2862 RVA: 0x0001E484 File Offset: 0x0001C684
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140001F7 RID: 503
		// (add) Token: 0x06000B2F RID: 2863 RVA: 0x0001E4BC File Offset: 0x0001C6BC
		// (remove) Token: 0x06000B30 RID: 2864 RVA: 0x0001E4F4 File Offset: 0x0001C6F4
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140001F8 RID: 504
		// (add) Token: 0x06000B31 RID: 2865 RVA: 0x0001E52C File Offset: 0x0001C72C
		// (remove) Token: 0x06000B32 RID: 2866 RVA: 0x0001E564 File Offset: 0x0001C764
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140001F9 RID: 505
		// (add) Token: 0x06000B33 RID: 2867 RVA: 0x0001E59C File Offset: 0x0001C79C
		// (remove) Token: 0x06000B34 RID: 2868 RVA: 0x0001E5D4 File Offset: 0x0001C7D4
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140001FA RID: 506
		// (add) Token: 0x06000B35 RID: 2869 RVA: 0x0001E60C File Offset: 0x0001C80C
		// (remove) Token: 0x06000B36 RID: 2870 RVA: 0x0001E644 File Offset: 0x0001C844
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140001FB RID: 507
		// (add) Token: 0x06000B37 RID: 2871 RVA: 0x0001E67C File Offset: 0x0001C87C
		// (remove) Token: 0x06000B38 RID: 2872 RVA: 0x0001E6B4 File Offset: 0x0001C8B4
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140001FC RID: 508
		// (add) Token: 0x06000B39 RID: 2873 RVA: 0x0001E6EC File Offset: 0x0001C8EC
		// (remove) Token: 0x06000B3A RID: 2874 RVA: 0x0001E724 File Offset: 0x0001C924
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140001FD RID: 509
		// (add) Token: 0x06000B3B RID: 2875 RVA: 0x0001E75C File Offset: 0x0001C95C
		// (remove) Token: 0x06000B3C RID: 2876 RVA: 0x0001E794 File Offset: 0x0001C994
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140001FE RID: 510
		// (add) Token: 0x06000B3D RID: 2877 RVA: 0x0001E7CC File Offset: 0x0001C9CC
		// (remove) Token: 0x06000B3E RID: 2878 RVA: 0x0001E804 File Offset: 0x0001CA04
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000B3F RID: 2879 RVA: 0x0001E839 File Offset: 0x0001CA39
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0001E85A File Offset: 0x0001CA5A
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0001E87B File Offset: 0x0001CA7B
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0001E89D File Offset: 0x0001CA9D
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0001E8C0 File Offset: 0x0001CAC0
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0001E8E4 File Offset: 0x0001CAE4
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0001E914 File Offset: 0x0001CB14
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0001E944 File Offset: 0x0001CB44
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0001E974 File Offset: 0x0001CB74
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient6EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
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

		// Token: 0x06000B49 RID: 2889 RVA: 0x0001E9F4 File Offset: 0x0001CBF4
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient6)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0001EA28 File Offset: 0x0001CC28
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0001EA3F File Offset: 0x0001CC3F
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0001EA56 File Offset: 0x0001CC56
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0001EA6D File Offset: 0x0001CC6D
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0001EA84 File Offset: 0x0001CC84
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0001EA9B File Offset: 0x0001CC9B
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0001EAB2 File Offset: 0x0001CCB2
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0001EAC9 File Offset: 0x0001CCC9
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0001EAE0 File Offset: 0x0001CCE0
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0001EAF7 File Offset: 0x0001CCF7
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0001EB0E File Offset: 0x0001CD0E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0001EB25 File Offset: 0x0001CD25
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0001EB3C File Offset: 0x0001CD3C
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0001EB53 File Offset: 0x0001CD53
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0001EB6A File Offset: 0x0001CD6A
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0001EB81 File Offset: 0x0001CD81
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0001EB98 File Offset: 0x0001CD98
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0001EBAF File Offset: 0x0001CDAF
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0001EBC6 File Offset: 0x0001CDC6
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0001EBDD File Offset: 0x0001CDDD
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0001EC0B File Offset: 0x0001CE0B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0001EC22 File Offset: 0x0001CE22
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0001EC39 File Offset: 0x0001CE39
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0001EC50 File Offset: 0x0001CE50
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0001EC67 File Offset: 0x0001CE67
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0001EC7E File Offset: 0x0001CE7E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0001EC95 File Offset: 0x0001CE95
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0001ECAC File Offset: 0x0001CEAC
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0001ECC3 File Offset: 0x0001CEC3
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400023C RID: 572
		private IMsRdpClient6 ocx;

		// Token: 0x0400023D RID: 573
		private AxMsRdpClient6EventMulticaster eventMulticaster;

		// Token: 0x0400023E RID: 574
		private AxHost.ConnectionPointCookie cookie;
	}
}
