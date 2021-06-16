using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002C RID: 44
	[DesignTimeVisible(true)]
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{3523c2fb-4031-44e4-9a3b-f1e94986ee7f}")]
	public class AxMsRdpClient2NotSafeForScripting : AxHost
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x00008A75 File Offset: 0x00006C75
		public AxMsRdpClient2NotSafeForScripting() : base("3523c2fb-4031-44e4-9a3b-f1e94986ee7f")
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00008A82 File Offset: 0x00006C82
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00008AA3 File Offset: 0x00006CA3
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00008AC5 File Offset: 0x00006CC5
		// (set) Token: 0x060002DA RID: 730 RVA: 0x00008AE6 File Offset: 0x00006CE6
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00008B08 File Offset: 0x00006D08
		// (set) Token: 0x060002DC RID: 732 RVA: 0x00008B29 File Offset: 0x00006D29
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00008B4B File Offset: 0x00006D4B
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00008B6C File Offset: 0x00006D6C
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00008B8E File Offset: 0x00006D8E
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00008BAF File Offset: 0x00006DAF
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

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00008BD1 File Offset: 0x00006DD1
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00008BF2 File Offset: 0x00006DF2
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00008C13 File Offset: 0x00006E13
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00008C35 File Offset: 0x00006E35
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00008C56 File Offset: 0x00006E56
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00008C78 File Offset: 0x00006E78
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00008C99 File Offset: 0x00006E99
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

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00008CBB File Offset: 0x00006EBB
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00008CDC File Offset: 0x00006EDC
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

		// Token: 0x1700005E RID: 94
		// (set) Token: 0x060002EA RID: 746 RVA: 0x00008CFD File Offset: 0x00006EFD
		[DispId(19)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00008D1F File Offset: 0x00006F1F
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00008D40 File Offset: 0x00006F40
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00008D61 File Offset: 0x00006F61
		[DispId(22)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00008D82 File Offset: 0x00006F82
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00008DA3 File Offset: 0x00006FA3
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

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00008DC4 File Offset: 0x00006FC4
		[Browsable(false)]
		[DispId(99)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00008DE5 File Offset: 0x00006FE5
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x00008E06 File Offset: 0x00007006
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

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00008E28 File Offset: 0x00007028
		[DispId(101)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00008E49 File Offset: 0x00007049
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00008E6A File Offset: 0x0000706A
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

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00008E8B File Offset: 0x0000708B
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00008EAC File Offset: 0x000070AC
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00008ECE File Offset: 0x000070CE
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(200)]
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00008EEF File Offset: 0x000070EF
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00008F10 File Offset: 0x00007110
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

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060002FB RID: 763 RVA: 0x00008F34 File Offset: 0x00007134
		// (remove) Token: 0x060002FC RID: 764 RVA: 0x00008F6C File Offset: 0x0000716C
		public event EventHandler OnConnecting;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060002FD RID: 765 RVA: 0x00008FA4 File Offset: 0x000071A4
		// (remove) Token: 0x060002FE RID: 766 RVA: 0x00008FDC File Offset: 0x000071DC
		public event EventHandler OnConnected;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x060002FF RID: 767 RVA: 0x00009014 File Offset: 0x00007214
		// (remove) Token: 0x06000300 RID: 768 RVA: 0x0000904C File Offset: 0x0000724C
		public event EventHandler OnLoginComplete;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06000301 RID: 769 RVA: 0x00009084 File Offset: 0x00007284
		// (remove) Token: 0x06000302 RID: 770 RVA: 0x000090BC File Offset: 0x000072BC
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06000303 RID: 771 RVA: 0x000090F4 File Offset: 0x000072F4
		// (remove) Token: 0x06000304 RID: 772 RVA: 0x0000912C File Offset: 0x0000732C
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06000305 RID: 773 RVA: 0x00009164 File Offset: 0x00007364
		// (remove) Token: 0x06000306 RID: 774 RVA: 0x0000919C File Offset: 0x0000739C
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06000307 RID: 775 RVA: 0x000091D4 File Offset: 0x000073D4
		// (remove) Token: 0x06000308 RID: 776 RVA: 0x0000920C File Offset: 0x0000740C
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06000309 RID: 777 RVA: 0x00009244 File Offset: 0x00007444
		// (remove) Token: 0x0600030A RID: 778 RVA: 0x0000927C File Offset: 0x0000747C
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600030B RID: 779 RVA: 0x000092B4 File Offset: 0x000074B4
		// (remove) Token: 0x0600030C RID: 780 RVA: 0x000092EC File Offset: 0x000074EC
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x0600030D RID: 781 RVA: 0x00009324 File Offset: 0x00007524
		// (remove) Token: 0x0600030E RID: 782 RVA: 0x0000935C File Offset: 0x0000755C
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x0600030F RID: 783 RVA: 0x00009394 File Offset: 0x00007594
		// (remove) Token: 0x06000310 RID: 784 RVA: 0x000093CC File Offset: 0x000075CC
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06000311 RID: 785 RVA: 0x00009404 File Offset: 0x00007604
		// (remove) Token: 0x06000312 RID: 786 RVA: 0x0000943C File Offset: 0x0000763C
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06000313 RID: 787 RVA: 0x00009474 File Offset: 0x00007674
		// (remove) Token: 0x06000314 RID: 788 RVA: 0x000094AC File Offset: 0x000076AC
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06000315 RID: 789 RVA: 0x000094E4 File Offset: 0x000076E4
		// (remove) Token: 0x06000316 RID: 790 RVA: 0x0000951C File Offset: 0x0000771C
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06000317 RID: 791 RVA: 0x00009554 File Offset: 0x00007754
		// (remove) Token: 0x06000318 RID: 792 RVA: 0x0000958C File Offset: 0x0000778C
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06000319 RID: 793 RVA: 0x000095C4 File Offset: 0x000077C4
		// (remove) Token: 0x0600031A RID: 794 RVA: 0x000095FC File Offset: 0x000077FC
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x0600031B RID: 795 RVA: 0x00009634 File Offset: 0x00007834
		// (remove) Token: 0x0600031C RID: 796 RVA: 0x0000966C File Offset: 0x0000786C
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x0600031D RID: 797 RVA: 0x000096A4 File Offset: 0x000078A4
		// (remove) Token: 0x0600031E RID: 798 RVA: 0x000096DC File Offset: 0x000078DC
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x0600031F RID: 799 RVA: 0x00009714 File Offset: 0x00007914
		// (remove) Token: 0x06000320 RID: 800 RVA: 0x0000974C File Offset: 0x0000794C
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06000321 RID: 801 RVA: 0x00009784 File Offset: 0x00007984
		// (remove) Token: 0x06000322 RID: 802 RVA: 0x000097BC File Offset: 0x000079BC
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06000323 RID: 803 RVA: 0x000097F4 File Offset: 0x000079F4
		// (remove) Token: 0x06000324 RID: 804 RVA: 0x0000982C File Offset: 0x00007A2C
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06000325 RID: 805 RVA: 0x00009864 File Offset: 0x00007A64
		// (remove) Token: 0x06000326 RID: 806 RVA: 0x0000989C File Offset: 0x00007A9C
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06000327 RID: 807 RVA: 0x000098D4 File Offset: 0x00007AD4
		// (remove) Token: 0x06000328 RID: 808 RVA: 0x0000990C File Offset: 0x00007B0C
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06000329 RID: 809 RVA: 0x00009944 File Offset: 0x00007B44
		// (remove) Token: 0x0600032A RID: 810 RVA: 0x0000997C File Offset: 0x00007B7C
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x0600032B RID: 811 RVA: 0x000099B4 File Offset: 0x00007BB4
		// (remove) Token: 0x0600032C RID: 812 RVA: 0x000099EC File Offset: 0x00007BEC
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x0600032D RID: 813 RVA: 0x00009A24 File Offset: 0x00007C24
		// (remove) Token: 0x0600032E RID: 814 RVA: 0x00009A5C File Offset: 0x00007C5C
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x0600032F RID: 815 RVA: 0x00009A94 File Offset: 0x00007C94
		// (remove) Token: 0x06000330 RID: 816 RVA: 0x00009ACC File Offset: 0x00007CCC
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06000331 RID: 817 RVA: 0x00009B04 File Offset: 0x00007D04
		// (remove) Token: 0x06000332 RID: 818 RVA: 0x00009B3C File Offset: 0x00007D3C
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06000333 RID: 819 RVA: 0x00009B74 File Offset: 0x00007D74
		// (remove) Token: 0x06000334 RID: 820 RVA: 0x00009BAC File Offset: 0x00007DAC
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06000335 RID: 821 RVA: 0x00009BE4 File Offset: 0x00007DE4
		// (remove) Token: 0x06000336 RID: 822 RVA: 0x00009C1C File Offset: 0x00007E1C
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000337 RID: 823 RVA: 0x00009C51 File Offset: 0x00007E51
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00009C72 File Offset: 0x00007E72
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00009C93 File Offset: 0x00007E93
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00009CB5 File Offset: 0x00007EB5
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00009CD8 File Offset: 0x00007ED8
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00009CFC File Offset: 0x00007EFC
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00009D2C File Offset: 0x00007F2C
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00009D5C File Offset: 0x00007F5C
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient2NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00009DAC File Offset: 0x00007FAC
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

		// Token: 0x06000340 RID: 832 RVA: 0x00009DDC File Offset: 0x00007FDC
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient2)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00009E10 File Offset: 0x00008010
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00009E27 File Offset: 0x00008027
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00009E3E File Offset: 0x0000803E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00009E55 File Offset: 0x00008055
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00009E6C File Offset: 0x0000806C
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00009E83 File Offset: 0x00008083
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00009E9A File Offset: 0x0000809A
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00009EB1 File Offset: 0x000080B1
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00009EC8 File Offset: 0x000080C8
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00009EDF File Offset: 0x000080DF
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00009EF6 File Offset: 0x000080F6
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00009F0D File Offset: 0x0000810D
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00009F24 File Offset: 0x00008124
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00009F3B File Offset: 0x0000813B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00009F52 File Offset: 0x00008152
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00009F69 File Offset: 0x00008169
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00009F80 File Offset: 0x00008180
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00009F97 File Offset: 0x00008197
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00009FAE File Offset: 0x000081AE
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00009FC5 File Offset: 0x000081C5
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00009FDC File Offset: 0x000081DC
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00009FF3 File Offset: 0x000081F3
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000A00A File Offset: 0x0000820A
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000A021 File Offset: 0x00008221
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000A038 File Offset: 0x00008238
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000A04F File Offset: 0x0000824F
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000A066 File Offset: 0x00008266
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000A07D File Offset: 0x0000827D
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000A094 File Offset: 0x00008294
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000A0AB File Offset: 0x000082AB
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040000A4 RID: 164
		private IMsRdpClient2 ocx;

		// Token: 0x040000A5 RID: 165
		private AxMsRdpClient2NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x040000A6 RID: 166
		private AxHost.ConnectionPointCookie cookie;
	}
}
