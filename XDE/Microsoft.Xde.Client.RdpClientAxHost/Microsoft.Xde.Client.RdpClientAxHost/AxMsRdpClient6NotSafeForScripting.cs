using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000042 RID: 66
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{d2ea46a7-c2bf-426b-af24-e19c44456399}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient6NotSafeForScripting : AxHost
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x0001B8E5 File Offset: 0x00019AE5
		public AxMsRdpClient6NotSafeForScripting() : base("d2ea46a7-c2bf-426b-af24-e19c44456399")
		{
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0001B8F2 File Offset: 0x00019AF2
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x0001B913 File Offset: 0x00019B13
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(1)]
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

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0001B935 File Offset: 0x00019B35
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x0001B956 File Offset: 0x00019B56
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

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0001B978 File Offset: 0x00019B78
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x0001B999 File Offset: 0x00019B99
		[DispId(3)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0001B9BB File Offset: 0x00019BBB
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x0001B9DC File Offset: 0x00019BDC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(4)]
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

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0001B9FE File Offset: 0x00019BFE
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x0001BA1F File Offset: 0x00019C1F
		[DispId(5)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0001BA41 File Offset: 0x00019C41
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

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0001BA62 File Offset: 0x00019C62
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x0001BA83 File Offset: 0x00019C83
		[DispId(12)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0001BAA5 File Offset: 0x00019CA5
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0001BAC6 File Offset: 0x00019CC6
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

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0001BB09 File Offset: 0x00019D09
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

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001BB2B File Offset: 0x00019D2B
		[DispId(17)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0001BB4C File Offset: 0x00019D4C
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

		// Token: 0x17000186 RID: 390
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x0001BB6D File Offset: 0x00019D6D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(19)]
		[Browsable(false)]
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

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0001BB8F File Offset: 0x00019D8F
		[DispId(20)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001BBB0 File Offset: 0x00019DB0
		[DispId(21)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0001BBD1 File Offset: 0x00019DD1
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0001BBF2 File Offset: 0x00019DF2
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0001BC13 File Offset: 0x00019E13
		[DispId(98)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0001BC34 File Offset: 0x00019E34
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(99)]
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

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0001BC55 File Offset: 0x00019E55
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0001BC76 File Offset: 0x00019E76
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(100)]
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

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0001BC98 File Offset: 0x00019E98
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

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0001BCB9 File Offset: 0x00019EB9
		[DispId(102)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0001BCDA File Offset: 0x00019EDA
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

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0001BCFB File Offset: 0x00019EFB
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0001BD1C File Offset: 0x00019F1C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(104)]
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

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0001BD3E File Offset: 0x00019F3E
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

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0001BD5F File Offset: 0x00019F5F
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x0001BD80 File Offset: 0x00019F80
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

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0001BDA2 File Offset: 0x00019FA2
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(300)]
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

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0001BDC3 File Offset: 0x00019FC3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0001BDE4 File Offset: 0x00019FE4
		[Browsable(false)]
		[DispId(500)]
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

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0001BE05 File Offset: 0x0001A005
		[DispId(502)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0001BE26 File Offset: 0x0001A026
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0001BE47 File Offset: 0x0001A047
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

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0001BE68 File Offset: 0x0001A068
		[Browsable(false)]
		[DispId(507)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0001BE89 File Offset: 0x0001A089
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

		// Token: 0x140001C3 RID: 451
		// (add) Token: 0x06000A52 RID: 2642 RVA: 0x0001BEAC File Offset: 0x0001A0AC
		// (remove) Token: 0x06000A53 RID: 2643 RVA: 0x0001BEE4 File Offset: 0x0001A0E4
		public event EventHandler OnConnecting;

		// Token: 0x140001C4 RID: 452
		// (add) Token: 0x06000A54 RID: 2644 RVA: 0x0001BF1C File Offset: 0x0001A11C
		// (remove) Token: 0x06000A55 RID: 2645 RVA: 0x0001BF54 File Offset: 0x0001A154
		public event EventHandler OnConnected;

		// Token: 0x140001C5 RID: 453
		// (add) Token: 0x06000A56 RID: 2646 RVA: 0x0001BF8C File Offset: 0x0001A18C
		// (remove) Token: 0x06000A57 RID: 2647 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
		public event EventHandler OnLoginComplete;

		// Token: 0x140001C6 RID: 454
		// (add) Token: 0x06000A58 RID: 2648 RVA: 0x0001BFFC File Offset: 0x0001A1FC
		// (remove) Token: 0x06000A59 RID: 2649 RVA: 0x0001C034 File Offset: 0x0001A234
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140001C7 RID: 455
		// (add) Token: 0x06000A5A RID: 2650 RVA: 0x0001C06C File Offset: 0x0001A26C
		// (remove) Token: 0x06000A5B RID: 2651 RVA: 0x0001C0A4 File Offset: 0x0001A2A4
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x140001C8 RID: 456
		// (add) Token: 0x06000A5C RID: 2652 RVA: 0x0001C0DC File Offset: 0x0001A2DC
		// (remove) Token: 0x06000A5D RID: 2653 RVA: 0x0001C114 File Offset: 0x0001A314
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x140001C9 RID: 457
		// (add) Token: 0x06000A5E RID: 2654 RVA: 0x0001C14C File Offset: 0x0001A34C
		// (remove) Token: 0x06000A5F RID: 2655 RVA: 0x0001C184 File Offset: 0x0001A384
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140001CA RID: 458
		// (add) Token: 0x06000A60 RID: 2656 RVA: 0x0001C1BC File Offset: 0x0001A3BC
		// (remove) Token: 0x06000A61 RID: 2657 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x140001CB RID: 459
		// (add) Token: 0x06000A62 RID: 2658 RVA: 0x0001C22C File Offset: 0x0001A42C
		// (remove) Token: 0x06000A63 RID: 2659 RVA: 0x0001C264 File Offset: 0x0001A464
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140001CC RID: 460
		// (add) Token: 0x06000A64 RID: 2660 RVA: 0x0001C29C File Offset: 0x0001A49C
		// (remove) Token: 0x06000A65 RID: 2661 RVA: 0x0001C2D4 File Offset: 0x0001A4D4
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140001CD RID: 461
		// (add) Token: 0x06000A66 RID: 2662 RVA: 0x0001C30C File Offset: 0x0001A50C
		// (remove) Token: 0x06000A67 RID: 2663 RVA: 0x0001C344 File Offset: 0x0001A544
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140001CE RID: 462
		// (add) Token: 0x06000A68 RID: 2664 RVA: 0x0001C37C File Offset: 0x0001A57C
		// (remove) Token: 0x06000A69 RID: 2665 RVA: 0x0001C3B4 File Offset: 0x0001A5B4
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140001CF RID: 463
		// (add) Token: 0x06000A6A RID: 2666 RVA: 0x0001C3EC File Offset: 0x0001A5EC
		// (remove) Token: 0x06000A6B RID: 2667 RVA: 0x0001C424 File Offset: 0x0001A624
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140001D0 RID: 464
		// (add) Token: 0x06000A6C RID: 2668 RVA: 0x0001C45C File Offset: 0x0001A65C
		// (remove) Token: 0x06000A6D RID: 2669 RVA: 0x0001C494 File Offset: 0x0001A694
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140001D1 RID: 465
		// (add) Token: 0x06000A6E RID: 2670 RVA: 0x0001C4CC File Offset: 0x0001A6CC
		// (remove) Token: 0x06000A6F RID: 2671 RVA: 0x0001C504 File Offset: 0x0001A704
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140001D2 RID: 466
		// (add) Token: 0x06000A70 RID: 2672 RVA: 0x0001C53C File Offset: 0x0001A73C
		// (remove) Token: 0x06000A71 RID: 2673 RVA: 0x0001C574 File Offset: 0x0001A774
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140001D3 RID: 467
		// (add) Token: 0x06000A72 RID: 2674 RVA: 0x0001C5AC File Offset: 0x0001A7AC
		// (remove) Token: 0x06000A73 RID: 2675 RVA: 0x0001C5E4 File Offset: 0x0001A7E4
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140001D4 RID: 468
		// (add) Token: 0x06000A74 RID: 2676 RVA: 0x0001C61C File Offset: 0x0001A81C
		// (remove) Token: 0x06000A75 RID: 2677 RVA: 0x0001C654 File Offset: 0x0001A854
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140001D5 RID: 469
		// (add) Token: 0x06000A76 RID: 2678 RVA: 0x0001C68C File Offset: 0x0001A88C
		// (remove) Token: 0x06000A77 RID: 2679 RVA: 0x0001C6C4 File Offset: 0x0001A8C4
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140001D6 RID: 470
		// (add) Token: 0x06000A78 RID: 2680 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		// (remove) Token: 0x06000A79 RID: 2681 RVA: 0x0001C734 File Offset: 0x0001A934
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140001D7 RID: 471
		// (add) Token: 0x06000A7A RID: 2682 RVA: 0x0001C76C File Offset: 0x0001A96C
		// (remove) Token: 0x06000A7B RID: 2683 RVA: 0x0001C7A4 File Offset: 0x0001A9A4
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140001D8 RID: 472
		// (add) Token: 0x06000A7C RID: 2684 RVA: 0x0001C7DC File Offset: 0x0001A9DC
		// (remove) Token: 0x06000A7D RID: 2685 RVA: 0x0001C814 File Offset: 0x0001AA14
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140001D9 RID: 473
		// (add) Token: 0x06000A7E RID: 2686 RVA: 0x0001C84C File Offset: 0x0001AA4C
		// (remove) Token: 0x06000A7F RID: 2687 RVA: 0x0001C884 File Offset: 0x0001AA84
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140001DA RID: 474
		// (add) Token: 0x06000A80 RID: 2688 RVA: 0x0001C8BC File Offset: 0x0001AABC
		// (remove) Token: 0x06000A81 RID: 2689 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140001DB RID: 475
		// (add) Token: 0x06000A82 RID: 2690 RVA: 0x0001C92C File Offset: 0x0001AB2C
		// (remove) Token: 0x06000A83 RID: 2691 RVA: 0x0001C964 File Offset: 0x0001AB64
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140001DC RID: 476
		// (add) Token: 0x06000A84 RID: 2692 RVA: 0x0001C99C File Offset: 0x0001AB9C
		// (remove) Token: 0x06000A85 RID: 2693 RVA: 0x0001C9D4 File Offset: 0x0001ABD4
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140001DD RID: 477
		// (add) Token: 0x06000A86 RID: 2694 RVA: 0x0001CA0C File Offset: 0x0001AC0C
		// (remove) Token: 0x06000A87 RID: 2695 RVA: 0x0001CA44 File Offset: 0x0001AC44
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140001DE RID: 478
		// (add) Token: 0x06000A88 RID: 2696 RVA: 0x0001CA7C File Offset: 0x0001AC7C
		// (remove) Token: 0x06000A89 RID: 2697 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140001DF RID: 479
		// (add) Token: 0x06000A8A RID: 2698 RVA: 0x0001CAEC File Offset: 0x0001ACEC
		// (remove) Token: 0x06000A8B RID: 2699 RVA: 0x0001CB24 File Offset: 0x0001AD24
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140001E0 RID: 480
		// (add) Token: 0x06000A8C RID: 2700 RVA: 0x0001CB5C File Offset: 0x0001AD5C
		// (remove) Token: 0x06000A8D RID: 2701 RVA: 0x0001CB94 File Offset: 0x0001AD94
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000A8E RID: 2702 RVA: 0x0001CBC9 File Offset: 0x0001ADC9
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0001CBEA File Offset: 0x0001ADEA
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0001CC0B File Offset: 0x0001AE0B
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0001CC2D File Offset: 0x0001AE2D
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0001CC50 File Offset: 0x0001AE50
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001CC74 File Offset: 0x0001AE74
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001CCA4 File Offset: 0x0001AEA4
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0001CCD4 File Offset: 0x0001AED4
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001CD04 File Offset: 0x0001AF04
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient6NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001CD54 File Offset: 0x0001AF54
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

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001CD84 File Offset: 0x0001AF84
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

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001CDB8 File Offset: 0x0001AFB8
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001CDCF File Offset: 0x0001AFCF
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001CDE6 File Offset: 0x0001AFE6
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001CDFD File Offset: 0x0001AFFD
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0001CE14 File Offset: 0x0001B014
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0001CE2B File Offset: 0x0001B02B
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0001CE42 File Offset: 0x0001B042
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001CE59 File Offset: 0x0001B059
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0001CE70 File Offset: 0x0001B070
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001CE87 File Offset: 0x0001B087
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001CE9E File Offset: 0x0001B09E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001CEB5 File Offset: 0x0001B0B5
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001CECC File Offset: 0x0001B0CC
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001CEE3 File Offset: 0x0001B0E3
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001CEFA File Offset: 0x0001B0FA
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0001CF11 File Offset: 0x0001B111
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0001CF28 File Offset: 0x0001B128
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0001CF3F File Offset: 0x0001B13F
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0001CF56 File Offset: 0x0001B156
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001CF6D File Offset: 0x0001B16D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0001CF84 File Offset: 0x0001B184
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0001CF9B File Offset: 0x0001B19B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0001CFB2 File Offset: 0x0001B1B2
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0001CFC9 File Offset: 0x0001B1C9
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0001CFE0 File Offset: 0x0001B1E0
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0001CFF7 File Offset: 0x0001B1F7
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0001D00E File Offset: 0x0001B20E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0001D025 File Offset: 0x0001B225
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001D03C File Offset: 0x0001B23C
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0001D053 File Offset: 0x0001B253
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400021A RID: 538
		private IMsRdpClient6 ocx;

		// Token: 0x0400021B RID: 539
		private AxMsRdpClient6NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x0400021C RID: 540
		private AxHost.ConnectionPointCookie cookie;
	}
}
