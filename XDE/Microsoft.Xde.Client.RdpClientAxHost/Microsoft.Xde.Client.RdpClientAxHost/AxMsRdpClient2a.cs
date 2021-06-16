using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000030 RID: 48
	[DesignTimeVisible(true)]
	[AxHost.ClsidAttribute("{971127bb-259f-48c2-bd75-5f97a3331551}")]
	[DefaultEvent("OnConnecting")]
	public class AxMsRdpClient2a : AxHost
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x0000C0E5 File Offset: 0x0000A2E5
		public AxMsRdpClient2a() : base("971127bb-259f-48c2-bd75-5f97a3331551")
		{
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000C0F2 File Offset: 0x0000A2F2
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0000C113 File Offset: 0x0000A313
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000C135 File Offset: 0x0000A335
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x0000C156 File Offset: 0x0000A356
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(2)]
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0000C178 File Offset: 0x0000A378
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0000C199 File Offset: 0x0000A399
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0000C1BB File Offset: 0x0000A3BB
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x0000C1DC File Offset: 0x0000A3DC
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000C1FE File Offset: 0x0000A3FE
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x0000C21F File Offset: 0x0000A41F
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0000C241 File Offset: 0x0000A441
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000C262 File Offset: 0x0000A462
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000C283 File Offset: 0x0000A483
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000C2A5 File Offset: 0x0000A4A5
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000C2C6 File Offset: 0x0000A4C6
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000C309 File Offset: 0x0000A509
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000C32B File Offset: 0x0000A52B
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0000C34C File Offset: 0x0000A54C
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

		// Token: 0x17000090 RID: 144
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x0000C36D File Offset: 0x0000A56D
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

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0000C38F File Offset: 0x0000A58F
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000C3D1 File Offset: 0x0000A5D1
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

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000C3F2 File Offset: 0x0000A5F2
		[DispId(97)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000C413 File Offset: 0x0000A613
		[DispId(98)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000C434 File Offset: 0x0000A634
		[DispId(99)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000C455 File Offset: 0x0000A655
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0000C476 File Offset: 0x0000A676
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x0000C498 File Offset: 0x0000A698
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

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000C4B9 File Offset: 0x0000A6B9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(102)]
		[Browsable(false)]
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000C4DA File Offset: 0x0000A6DA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(103)]
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000C4FB File Offset: 0x0000A6FB
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0000C51C File Offset: 0x0000A71C
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000C53E File Offset: 0x0000A73E
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000C55F File Offset: 0x0000A75F
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x0000C580 File Offset: 0x0000A780
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(201)]
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

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x0600044B RID: 1099 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
		// (remove) Token: 0x0600044C RID: 1100 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		public event EventHandler OnConnecting;

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x0600044D RID: 1101 RVA: 0x0000C614 File Offset: 0x0000A814
		// (remove) Token: 0x0600044E RID: 1102 RVA: 0x0000C64C File Offset: 0x0000A84C
		public event EventHandler OnConnected;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x0600044F RID: 1103 RVA: 0x0000C684 File Offset: 0x0000A884
		// (remove) Token: 0x06000450 RID: 1104 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		public event EventHandler OnLoginComplete;

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06000451 RID: 1105 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		// (remove) Token: 0x06000452 RID: 1106 RVA: 0x0000C72C File Offset: 0x0000A92C
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06000453 RID: 1107 RVA: 0x0000C764 File Offset: 0x0000A964
		// (remove) Token: 0x06000454 RID: 1108 RVA: 0x0000C79C File Offset: 0x0000A99C
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06000455 RID: 1109 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		// (remove) Token: 0x06000456 RID: 1110 RVA: 0x0000C80C File Offset: 0x0000AA0C
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06000457 RID: 1111 RVA: 0x0000C844 File Offset: 0x0000AA44
		// (remove) Token: 0x06000458 RID: 1112 RVA: 0x0000C87C File Offset: 0x0000AA7C
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06000459 RID: 1113 RVA: 0x0000C8B4 File Offset: 0x0000AAB4
		// (remove) Token: 0x0600045A RID: 1114 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x0600045B RID: 1115 RVA: 0x0000C924 File Offset: 0x0000AB24
		// (remove) Token: 0x0600045C RID: 1116 RVA: 0x0000C95C File Offset: 0x0000AB5C
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x0600045D RID: 1117 RVA: 0x0000C994 File Offset: 0x0000AB94
		// (remove) Token: 0x0600045E RID: 1118 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x0600045F RID: 1119 RVA: 0x0000CA04 File Offset: 0x0000AC04
		// (remove) Token: 0x06000460 RID: 1120 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06000461 RID: 1121 RVA: 0x0000CA74 File Offset: 0x0000AC74
		// (remove) Token: 0x06000462 RID: 1122 RVA: 0x0000CAAC File Offset: 0x0000ACAC
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06000463 RID: 1123 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		// (remove) Token: 0x06000464 RID: 1124 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06000465 RID: 1125 RVA: 0x0000CB54 File Offset: 0x0000AD54
		// (remove) Token: 0x06000466 RID: 1126 RVA: 0x0000CB8C File Offset: 0x0000AD8C
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06000467 RID: 1127 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
		// (remove) Token: 0x06000468 RID: 1128 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06000469 RID: 1129 RVA: 0x0000CC34 File Offset: 0x0000AE34
		// (remove) Token: 0x0600046A RID: 1130 RVA: 0x0000CC6C File Offset: 0x0000AE6C
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x0600046B RID: 1131 RVA: 0x0000CCA4 File Offset: 0x0000AEA4
		// (remove) Token: 0x0600046C RID: 1132 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x0600046D RID: 1133 RVA: 0x0000CD14 File Offset: 0x0000AF14
		// (remove) Token: 0x0600046E RID: 1134 RVA: 0x0000CD4C File Offset: 0x0000AF4C
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x0600046F RID: 1135 RVA: 0x0000CD84 File Offset: 0x0000AF84
		// (remove) Token: 0x06000470 RID: 1136 RVA: 0x0000CDBC File Offset: 0x0000AFBC
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06000471 RID: 1137 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
		// (remove) Token: 0x06000472 RID: 1138 RVA: 0x0000CE2C File Offset: 0x0000B02C
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06000473 RID: 1139 RVA: 0x0000CE64 File Offset: 0x0000B064
		// (remove) Token: 0x06000474 RID: 1140 RVA: 0x0000CE9C File Offset: 0x0000B09C
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06000475 RID: 1141 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		// (remove) Token: 0x06000476 RID: 1142 RVA: 0x0000CF0C File Offset: 0x0000B10C
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06000477 RID: 1143 RVA: 0x0000CF44 File Offset: 0x0000B144
		// (remove) Token: 0x06000478 RID: 1144 RVA: 0x0000CF7C File Offset: 0x0000B17C
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06000479 RID: 1145 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		// (remove) Token: 0x0600047A RID: 1146 RVA: 0x0000CFEC File Offset: 0x0000B1EC
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x0600047B RID: 1147 RVA: 0x0000D024 File Offset: 0x0000B224
		// (remove) Token: 0x0600047C RID: 1148 RVA: 0x0000D05C File Offset: 0x0000B25C
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x0600047D RID: 1149 RVA: 0x0000D094 File Offset: 0x0000B294
		// (remove) Token: 0x0600047E RID: 1150 RVA: 0x0000D0CC File Offset: 0x0000B2CC
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x0600047F RID: 1151 RVA: 0x0000D104 File Offset: 0x0000B304
		// (remove) Token: 0x06000480 RID: 1152 RVA: 0x0000D13C File Offset: 0x0000B33C
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06000481 RID: 1153 RVA: 0x0000D174 File Offset: 0x0000B374
		// (remove) Token: 0x06000482 RID: 1154 RVA: 0x0000D1AC File Offset: 0x0000B3AC
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06000483 RID: 1155 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		// (remove) Token: 0x06000484 RID: 1156 RVA: 0x0000D21C File Offset: 0x0000B41C
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06000485 RID: 1157 RVA: 0x0000D254 File Offset: 0x0000B454
		// (remove) Token: 0x06000486 RID: 1158 RVA: 0x0000D28C File Offset: 0x0000B48C
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000487 RID: 1159 RVA: 0x0000D2C1 File Offset: 0x0000B4C1
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000D2E2 File Offset: 0x0000B4E2
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000D303 File Offset: 0x0000B503
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000D325 File Offset: 0x0000B525
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000D348 File Offset: 0x0000B548
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000D36C File Offset: 0x0000B56C
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000D39C File Offset: 0x0000B59C
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient2aEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000D41C File Offset: 0x0000B61C
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

		// Token: 0x06000490 RID: 1168 RVA: 0x0000D44C File Offset: 0x0000B64C
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

		// Token: 0x06000491 RID: 1169 RVA: 0x0000D480 File Offset: 0x0000B680
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000D497 File Offset: 0x0000B697
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000D4AE File Offset: 0x0000B6AE
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000D4C5 File Offset: 0x0000B6C5
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000D4DC File Offset: 0x0000B6DC
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000D4F3 File Offset: 0x0000B6F3
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000D50A File Offset: 0x0000B70A
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000D521 File Offset: 0x0000B721
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000D538 File Offset: 0x0000B738
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000D54F File Offset: 0x0000B74F
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000D566 File Offset: 0x0000B766
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000D57D File Offset: 0x0000B77D
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000D594 File Offset: 0x0000B794
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000D5AB File Offset: 0x0000B7AB
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000D5C2 File Offset: 0x0000B7C2
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000D5D9 File Offset: 0x0000B7D9
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000D5F0 File Offset: 0x0000B7F0
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000D607 File Offset: 0x0000B807
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000D61E File Offset: 0x0000B81E
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000D635 File Offset: 0x0000B835
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000D64C File Offset: 0x0000B84C
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000D663 File Offset: 0x0000B863
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000D67A File Offset: 0x0000B87A
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000D691 File Offset: 0x0000B891
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000D6BF File Offset: 0x0000B8BF
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000D6D6 File Offset: 0x0000B8D6
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000D6ED File Offset: 0x0000B8ED
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000D704 File Offset: 0x0000B904
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000D71B File Offset: 0x0000B91B
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040000E8 RID: 232
		private IMsRdpClient2 ocx;

		// Token: 0x040000E9 RID: 233
		private AxMsRdpClient2aEventMulticaster eventMulticaster;

		// Token: 0x040000EA RID: 234
		private AxHost.ConnectionPointCookie cookie;
	}
}
