using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000026 RID: 38
	[AxHost.ClsidAttribute("{1fb464c8-09bb-4017-a2f5-eb742f04392f}")]
	[DefaultEvent("OnConnecting")]
	[DesignTimeVisible(true)]
	public class AxMsTscAx : AxHost
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00003B65 File Offset: 0x00001D65
		public AxMsTscAx() : base("1fb464c8-09bb-4017-a2f5-eb742f04392f")
		{
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00003B72 File Offset: 0x00001D72
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00003B93 File Offset: 0x00001D93
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00003BB5 File Offset: 0x00001DB5
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003BD6 File Offset: 0x00001DD6
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003BF8 File Offset: 0x00001DF8
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00003C19 File Offset: 0x00001E19
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

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003C3B File Offset: 0x00001E3B
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00003C5C File Offset: 0x00001E5C
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003C7E File Offset: 0x00001E7E
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00003C9F File Offset: 0x00001E9F
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00003CC1 File Offset: 0x00001EC1
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

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00003CE2 File Offset: 0x00001EE2
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00003D03 File Offset: 0x00001F03
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00003D25 File Offset: 0x00001F25
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00003D46 File Offset: 0x00001F46
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(13)]
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003D68 File Offset: 0x00001F68
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003D89 File Offset: 0x00001F89
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(16)]
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003DAB File Offset: 0x00001FAB
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003DCC File Offset: 0x00001FCC
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

		// Token: 0x1700001E RID: 30
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00003DED File Offset: 0x00001FED
		[Browsable(false)]
		[DispId(19)]
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00003E0F File Offset: 0x0000200F
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

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00003E30 File Offset: 0x00002030
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003E51 File Offset: 0x00002051
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00003E72 File Offset: 0x00002072
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(97)]
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003E93 File Offset: 0x00002093
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(98)]
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

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003EB4 File Offset: 0x000020B4
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

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600010C RID: 268 RVA: 0x00003ED8 File Offset: 0x000020D8
		// (remove) Token: 0x0600010D RID: 269 RVA: 0x00003F10 File Offset: 0x00002110
		public event EventHandler OnConnecting;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x0600010E RID: 270 RVA: 0x00003F48 File Offset: 0x00002148
		// (remove) Token: 0x0600010F RID: 271 RVA: 0x00003F80 File Offset: 0x00002180
		public event EventHandler OnConnected;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000110 RID: 272 RVA: 0x00003FB8 File Offset: 0x000021B8
		// (remove) Token: 0x06000111 RID: 273 RVA: 0x00003FF0 File Offset: 0x000021F0
		public event EventHandler OnLoginComplete;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000112 RID: 274 RVA: 0x00004028 File Offset: 0x00002228
		// (remove) Token: 0x06000113 RID: 275 RVA: 0x00004060 File Offset: 0x00002260
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000114 RID: 276 RVA: 0x00004098 File Offset: 0x00002298
		// (remove) Token: 0x06000115 RID: 277 RVA: 0x000040D0 File Offset: 0x000022D0
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000116 RID: 278 RVA: 0x00004108 File Offset: 0x00002308
		// (remove) Token: 0x06000117 RID: 279 RVA: 0x00004140 File Offset: 0x00002340
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000118 RID: 280 RVA: 0x00004178 File Offset: 0x00002378
		// (remove) Token: 0x06000119 RID: 281 RVA: 0x000041B0 File Offset: 0x000023B0
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x0600011A RID: 282 RVA: 0x000041E8 File Offset: 0x000023E8
		// (remove) Token: 0x0600011B RID: 283 RVA: 0x00004220 File Offset: 0x00002420
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600011C RID: 284 RVA: 0x00004258 File Offset: 0x00002458
		// (remove) Token: 0x0600011D RID: 285 RVA: 0x00004290 File Offset: 0x00002490
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x0600011E RID: 286 RVA: 0x000042C8 File Offset: 0x000024C8
		// (remove) Token: 0x0600011F RID: 287 RVA: 0x00004300 File Offset: 0x00002500
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000120 RID: 288 RVA: 0x00004338 File Offset: 0x00002538
		// (remove) Token: 0x06000121 RID: 289 RVA: 0x00004370 File Offset: 0x00002570
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000122 RID: 290 RVA: 0x000043A8 File Offset: 0x000025A8
		// (remove) Token: 0x06000123 RID: 291 RVA: 0x000043E0 File Offset: 0x000025E0
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000124 RID: 292 RVA: 0x00004418 File Offset: 0x00002618
		// (remove) Token: 0x06000125 RID: 293 RVA: 0x00004450 File Offset: 0x00002650
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000126 RID: 294 RVA: 0x00004488 File Offset: 0x00002688
		// (remove) Token: 0x06000127 RID: 295 RVA: 0x000044C0 File Offset: 0x000026C0
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000128 RID: 296 RVA: 0x000044F8 File Offset: 0x000026F8
		// (remove) Token: 0x06000129 RID: 297 RVA: 0x00004530 File Offset: 0x00002730
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600012A RID: 298 RVA: 0x00004568 File Offset: 0x00002768
		// (remove) Token: 0x0600012B RID: 299 RVA: 0x000045A0 File Offset: 0x000027A0
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x0600012C RID: 300 RVA: 0x000045D8 File Offset: 0x000027D8
		// (remove) Token: 0x0600012D RID: 301 RVA: 0x00004610 File Offset: 0x00002810
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x0600012E RID: 302 RVA: 0x00004648 File Offset: 0x00002848
		// (remove) Token: 0x0600012F RID: 303 RVA: 0x00004680 File Offset: 0x00002880
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000130 RID: 304 RVA: 0x000046B8 File Offset: 0x000028B8
		// (remove) Token: 0x06000131 RID: 305 RVA: 0x000046F0 File Offset: 0x000028F0
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000132 RID: 306 RVA: 0x00004728 File Offset: 0x00002928
		// (remove) Token: 0x06000133 RID: 307 RVA: 0x00004760 File Offset: 0x00002960
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000134 RID: 308 RVA: 0x00004798 File Offset: 0x00002998
		// (remove) Token: 0x06000135 RID: 309 RVA: 0x000047D0 File Offset: 0x000029D0
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000136 RID: 310 RVA: 0x00004808 File Offset: 0x00002A08
		// (remove) Token: 0x06000137 RID: 311 RVA: 0x00004840 File Offset: 0x00002A40
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000138 RID: 312 RVA: 0x00004878 File Offset: 0x00002A78
		// (remove) Token: 0x06000139 RID: 313 RVA: 0x000048B0 File Offset: 0x00002AB0
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600013A RID: 314 RVA: 0x000048E8 File Offset: 0x00002AE8
		// (remove) Token: 0x0600013B RID: 315 RVA: 0x00004920 File Offset: 0x00002B20
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600013C RID: 316 RVA: 0x00004958 File Offset: 0x00002B58
		// (remove) Token: 0x0600013D RID: 317 RVA: 0x00004990 File Offset: 0x00002B90
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x0600013E RID: 318 RVA: 0x000049C8 File Offset: 0x00002BC8
		// (remove) Token: 0x0600013F RID: 319 RVA: 0x00004A00 File Offset: 0x00002C00
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000140 RID: 320 RVA: 0x00004A38 File Offset: 0x00002C38
		// (remove) Token: 0x06000141 RID: 321 RVA: 0x00004A70 File Offset: 0x00002C70
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000142 RID: 322 RVA: 0x00004AA8 File Offset: 0x00002CA8
		// (remove) Token: 0x06000143 RID: 323 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000144 RID: 324 RVA: 0x00004B18 File Offset: 0x00002D18
		// (remove) Token: 0x06000145 RID: 325 RVA: 0x00004B50 File Offset: 0x00002D50
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000146 RID: 326 RVA: 0x00004B88 File Offset: 0x00002D88
		// (remove) Token: 0x06000147 RID: 327 RVA: 0x00004BC0 File Offset: 0x00002DC0
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000148 RID: 328 RVA: 0x00004BF5 File Offset: 0x00002DF5
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004C16 File Offset: 0x00002E16
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00004C37 File Offset: 0x00002E37
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00004C59 File Offset: 0x00002E59
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00004C7C File Offset: 0x00002E7C
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsTscAxEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004CCC File Offset: 0x00002ECC
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

		// Token: 0x0600014E RID: 334 RVA: 0x00004CFC File Offset: 0x00002EFC
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsTscAx)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004D30 File Offset: 0x00002F30
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00004D47 File Offset: 0x00002F47
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004D5E File Offset: 0x00002F5E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004D75 File Offset: 0x00002F75
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00004D8C File Offset: 0x00002F8C
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00004DA3 File Offset: 0x00002FA3
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004DBA File Offset: 0x00002FBA
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004DD1 File Offset: 0x00002FD1
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00004DE8 File Offset: 0x00002FE8
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004DFF File Offset: 0x00002FFF
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004E16 File Offset: 0x00003016
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004E2D File Offset: 0x0000302D
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004E44 File Offset: 0x00003044
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004E5B File Offset: 0x0000305B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004E72 File Offset: 0x00003072
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004E89 File Offset: 0x00003089
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004EA0 File Offset: 0x000030A0
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004EB7 File Offset: 0x000030B7
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004ECE File Offset: 0x000030CE
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004EE5 File Offset: 0x000030E5
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00004EFC File Offset: 0x000030FC
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004F13 File Offset: 0x00003113
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00004F2A File Offset: 0x0000312A
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00004F41 File Offset: 0x00003141
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004F58 File Offset: 0x00003158
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004F6F File Offset: 0x0000316F
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004F86 File Offset: 0x00003186
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004F9D File Offset: 0x0000319D
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004FB4 File Offset: 0x000031B4
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004FCB File Offset: 0x000031CB
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400003E RID: 62
		private IMsTscAx ocx;

		// Token: 0x0400003F RID: 63
		private AxMsTscAxEventMulticaster eventMulticaster;

		// Token: 0x04000040 RID: 64
		private AxHost.ConnectionPointCookie cookie;
	}
}
