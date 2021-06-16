using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002A RID: 42
	[AxHost.ClsidAttribute("{791fa017-2de3-492e-acc5-53c67a2b94d0}")]
	[DefaultEvent("OnConnecting")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient : AxHost
	{
		// Token: 0x06000231 RID: 561 RVA: 0x00006FA1 File Offset: 0x000051A1
		public AxMsRdpClient() : base("791fa017-2de3-492e-acc5-53c67a2b94d0")
		{
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00006FAE File Offset: 0x000051AE
		// (set) Token: 0x06000233 RID: 563 RVA: 0x00006FCF File Offset: 0x000051CF
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00006FF1 File Offset: 0x000051F1
		// (set) Token: 0x06000235 RID: 565 RVA: 0x00007012 File Offset: 0x00005212
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00007034 File Offset: 0x00005234
		// (set) Token: 0x06000237 RID: 567 RVA: 0x00007055 File Offset: 0x00005255
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00007077 File Offset: 0x00005277
		// (set) Token: 0x06000239 RID: 569 RVA: 0x00007098 File Offset: 0x00005298
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000070BA File Offset: 0x000052BA
		// (set) Token: 0x0600023B RID: 571 RVA: 0x000070DB File Offset: 0x000052DB
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600023C RID: 572 RVA: 0x000070FD File Offset: 0x000052FD
		[DispId(6)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000711E File Offset: 0x0000531E
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000713F File Offset: 0x0000533F
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00007161 File Offset: 0x00005361
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00007182 File Offset: 0x00005382
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000071A4 File Offset: 0x000053A4
		// (set) Token: 0x06000242 RID: 578 RVA: 0x000071C5 File Offset: 0x000053C5
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000243 RID: 579 RVA: 0x000071E7 File Offset: 0x000053E7
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00007208 File Offset: 0x00005408
		[DispId(18)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000047 RID: 71
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00007229 File Offset: 0x00005429
		[DispId(19)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000724B File Offset: 0x0000544B
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000726C File Offset: 0x0000546C
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000728D File Offset: 0x0000548D
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000072AE File Offset: 0x000054AE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(97)]
		[Browsable(false)]
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000072CF File Offset: 0x000054CF
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000072F0 File Offset: 0x000054F0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00007311 File Offset: 0x00005511
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00007332 File Offset: 0x00005532
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00007354 File Offset: 0x00005554
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(101)]
		[Browsable(false)]
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00007375 File Offset: 0x00005575
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00007396 File Offset: 0x00005596
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000073B7 File Offset: 0x000055B7
		// (set) Token: 0x06000252 RID: 594 RVA: 0x000073D8 File Offset: 0x000055D8
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

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06000253 RID: 595 RVA: 0x000073FC File Offset: 0x000055FC
		// (remove) Token: 0x06000254 RID: 596 RVA: 0x00007434 File Offset: 0x00005634
		public event EventHandler OnConnecting;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06000255 RID: 597 RVA: 0x0000746C File Offset: 0x0000566C
		// (remove) Token: 0x06000256 RID: 598 RVA: 0x000074A4 File Offset: 0x000056A4
		public event EventHandler OnConnected;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06000257 RID: 599 RVA: 0x000074DC File Offset: 0x000056DC
		// (remove) Token: 0x06000258 RID: 600 RVA: 0x00007514 File Offset: 0x00005714
		public event EventHandler OnLoginComplete;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06000259 RID: 601 RVA: 0x0000754C File Offset: 0x0000574C
		// (remove) Token: 0x0600025A RID: 602 RVA: 0x00007584 File Offset: 0x00005784
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x0600025B RID: 603 RVA: 0x000075BC File Offset: 0x000057BC
		// (remove) Token: 0x0600025C RID: 604 RVA: 0x000075F4 File Offset: 0x000057F4
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x0600025D RID: 605 RVA: 0x0000762C File Offset: 0x0000582C
		// (remove) Token: 0x0600025E RID: 606 RVA: 0x00007664 File Offset: 0x00005864
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x0600025F RID: 607 RVA: 0x0000769C File Offset: 0x0000589C
		// (remove) Token: 0x06000260 RID: 608 RVA: 0x000076D4 File Offset: 0x000058D4
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06000261 RID: 609 RVA: 0x0000770C File Offset: 0x0000590C
		// (remove) Token: 0x06000262 RID: 610 RVA: 0x00007744 File Offset: 0x00005944
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06000263 RID: 611 RVA: 0x0000777C File Offset: 0x0000597C
		// (remove) Token: 0x06000264 RID: 612 RVA: 0x000077B4 File Offset: 0x000059B4
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06000265 RID: 613 RVA: 0x000077EC File Offset: 0x000059EC
		// (remove) Token: 0x06000266 RID: 614 RVA: 0x00007824 File Offset: 0x00005A24
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06000267 RID: 615 RVA: 0x0000785C File Offset: 0x00005A5C
		// (remove) Token: 0x06000268 RID: 616 RVA: 0x00007894 File Offset: 0x00005A94
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06000269 RID: 617 RVA: 0x000078CC File Offset: 0x00005ACC
		// (remove) Token: 0x0600026A RID: 618 RVA: 0x00007904 File Offset: 0x00005B04
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x0600026B RID: 619 RVA: 0x0000793C File Offset: 0x00005B3C
		// (remove) Token: 0x0600026C RID: 620 RVA: 0x00007974 File Offset: 0x00005B74
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x0600026D RID: 621 RVA: 0x000079AC File Offset: 0x00005BAC
		// (remove) Token: 0x0600026E RID: 622 RVA: 0x000079E4 File Offset: 0x00005BE4
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x0600026F RID: 623 RVA: 0x00007A1C File Offset: 0x00005C1C
		// (remove) Token: 0x06000270 RID: 624 RVA: 0x00007A54 File Offset: 0x00005C54
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06000271 RID: 625 RVA: 0x00007A8C File Offset: 0x00005C8C
		// (remove) Token: 0x06000272 RID: 626 RVA: 0x00007AC4 File Offset: 0x00005CC4
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06000273 RID: 627 RVA: 0x00007AFC File Offset: 0x00005CFC
		// (remove) Token: 0x06000274 RID: 628 RVA: 0x00007B34 File Offset: 0x00005D34
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06000275 RID: 629 RVA: 0x00007B6C File Offset: 0x00005D6C
		// (remove) Token: 0x06000276 RID: 630 RVA: 0x00007BA4 File Offset: 0x00005DA4
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06000277 RID: 631 RVA: 0x00007BDC File Offset: 0x00005DDC
		// (remove) Token: 0x06000278 RID: 632 RVA: 0x00007C14 File Offset: 0x00005E14
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06000279 RID: 633 RVA: 0x00007C4C File Offset: 0x00005E4C
		// (remove) Token: 0x0600027A RID: 634 RVA: 0x00007C84 File Offset: 0x00005E84
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x0600027B RID: 635 RVA: 0x00007CBC File Offset: 0x00005EBC
		// (remove) Token: 0x0600027C RID: 636 RVA: 0x00007CF4 File Offset: 0x00005EF4
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x0600027D RID: 637 RVA: 0x00007D2C File Offset: 0x00005F2C
		// (remove) Token: 0x0600027E RID: 638 RVA: 0x00007D64 File Offset: 0x00005F64
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x0600027F RID: 639 RVA: 0x00007D9C File Offset: 0x00005F9C
		// (remove) Token: 0x06000280 RID: 640 RVA: 0x00007DD4 File Offset: 0x00005FD4
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06000281 RID: 641 RVA: 0x00007E0C File Offset: 0x0000600C
		// (remove) Token: 0x06000282 RID: 642 RVA: 0x00007E44 File Offset: 0x00006044
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06000283 RID: 643 RVA: 0x00007E7C File Offset: 0x0000607C
		// (remove) Token: 0x06000284 RID: 644 RVA: 0x00007EB4 File Offset: 0x000060B4
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06000285 RID: 645 RVA: 0x00007EEC File Offset: 0x000060EC
		// (remove) Token: 0x06000286 RID: 646 RVA: 0x00007F24 File Offset: 0x00006124
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06000287 RID: 647 RVA: 0x00007F5C File Offset: 0x0000615C
		// (remove) Token: 0x06000288 RID: 648 RVA: 0x00007F94 File Offset: 0x00006194
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06000289 RID: 649 RVA: 0x00007FCC File Offset: 0x000061CC
		// (remove) Token: 0x0600028A RID: 650 RVA: 0x00008004 File Offset: 0x00006204
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x0600028B RID: 651 RVA: 0x0000803C File Offset: 0x0000623C
		// (remove) Token: 0x0600028C RID: 652 RVA: 0x00008074 File Offset: 0x00006274
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x0600028D RID: 653 RVA: 0x000080AC File Offset: 0x000062AC
		// (remove) Token: 0x0600028E RID: 654 RVA: 0x000080E4 File Offset: 0x000062E4
		public event EventHandler OnAutoReconnected;

		// Token: 0x0600028F RID: 655 RVA: 0x00008119 File Offset: 0x00006319
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000813A File Offset: 0x0000633A
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000815B File Offset: 0x0000635B
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000817D File Offset: 0x0000637D
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000081A0 File Offset: 0x000063A0
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000081C4 File Offset: 0x000063C4
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000081F4 File Offset: 0x000063F4
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00008224 File Offset: 0x00006424
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClientEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00008274 File Offset: 0x00006474
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

		// Token: 0x06000298 RID: 664 RVA: 0x000082A4 File Offset: 0x000064A4
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000082D8 File Offset: 0x000064D8
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000082EF File Offset: 0x000064EF
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00008306 File Offset: 0x00006506
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000831D File Offset: 0x0000651D
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008334 File Offset: 0x00006534
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000834B File Offset: 0x0000654B
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008362 File Offset: 0x00006562
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008379 File Offset: 0x00006579
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008390 File Offset: 0x00006590
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000083A7 File Offset: 0x000065A7
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000083BE File Offset: 0x000065BE
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000083D5 File Offset: 0x000065D5
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000083EC File Offset: 0x000065EC
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00008403 File Offset: 0x00006603
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000841A File Offset: 0x0000661A
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00008431 File Offset: 0x00006631
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00008448 File Offset: 0x00006648
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000845F File Offset: 0x0000665F
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00008476 File Offset: 0x00006676
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000848D File Offset: 0x0000668D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000084A4 File Offset: 0x000066A4
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000084BB File Offset: 0x000066BB
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000084D2 File Offset: 0x000066D2
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000084E9 File Offset: 0x000066E9
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00008500 File Offset: 0x00006700
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00008517 File Offset: 0x00006717
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000852E File Offset: 0x0000672E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008545 File Offset: 0x00006745
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000855C File Offset: 0x0000675C
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00008573 File Offset: 0x00006773
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x04000082 RID: 130
		private IMsRdpClient ocx;

		// Token: 0x04000083 RID: 131
		private AxMsRdpClientEventMulticaster eventMulticaster;

		// Token: 0x04000084 RID: 132
		private AxHost.ConnectionPointCookie cookie;
	}
}
