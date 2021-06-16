using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000002 RID: 2
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{a41a4187-5a86-4e26-b40a-856f9035d9cb}")]
	[DesignTimeVisible(true)]
	public class AxMsTscAxNotSafeForScripting : AxHost
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public AxMsTscAxNotSafeForScripting() : base("a41a4187-5a86-4e26-b40a-856f9035d9cb")
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020DD File Offset: 0x000002DD
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020FE File Offset: 0x000002FE
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002120 File Offset: 0x00000320
		// (set) Token: 0x06000005 RID: 5 RVA: 0x00002141 File Offset: 0x00000341
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002163 File Offset: 0x00000363
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002184 File Offset: 0x00000384
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021A6 File Offset: 0x000003A6
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000021C7 File Offset: 0x000003C7
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021E9 File Offset: 0x000003E9
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000220A File Offset: 0x0000040A
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000222C File Offset: 0x0000042C
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

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000224D File Offset: 0x0000044D
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000226E File Offset: 0x0000046E
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002290 File Offset: 0x00000490
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022B1 File Offset: 0x000004B1
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022D3 File Offset: 0x000004D3
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000022F4 File Offset: 0x000004F4
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002316 File Offset: 0x00000516
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002337 File Offset: 0x00000537
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

		// Token: 0x1700000C RID: 12
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002358 File Offset: 0x00000558
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000237A File Offset: 0x0000057A
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000239B File Offset: 0x0000059B
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000023BC File Offset: 0x000005BC
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000023DD File Offset: 0x000005DD
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000023FE File Offset: 0x000005FE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000241F File Offset: 0x0000061F
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600001C RID: 28 RVA: 0x00002440 File Offset: 0x00000640
		// (remove) Token: 0x0600001D RID: 29 RVA: 0x00002478 File Offset: 0x00000678
		public event EventHandler OnConnecting;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600001E RID: 30 RVA: 0x000024B0 File Offset: 0x000006B0
		// (remove) Token: 0x0600001F RID: 31 RVA: 0x000024E8 File Offset: 0x000006E8
		public event EventHandler OnConnected;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000020 RID: 32 RVA: 0x00002520 File Offset: 0x00000720
		// (remove) Token: 0x06000021 RID: 33 RVA: 0x00002558 File Offset: 0x00000758
		public event EventHandler OnLoginComplete;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000022 RID: 34 RVA: 0x00002590 File Offset: 0x00000790
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x000025C8 File Offset: 0x000007C8
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000024 RID: 36 RVA: 0x00002600 File Offset: 0x00000800
		// (remove) Token: 0x06000025 RID: 37 RVA: 0x00002638 File Offset: 0x00000838
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000026 RID: 38 RVA: 0x00002670 File Offset: 0x00000870
		// (remove) Token: 0x06000027 RID: 39 RVA: 0x000026A8 File Offset: 0x000008A8
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000028 RID: 40 RVA: 0x000026E0 File Offset: 0x000008E0
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x00002718 File Offset: 0x00000918
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600002A RID: 42 RVA: 0x00002750 File Offset: 0x00000950
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x00002788 File Offset: 0x00000988
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000027C0 File Offset: 0x000009C0
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x000027F8 File Offset: 0x000009F8
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002830 File Offset: 0x00000A30
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x00002868 File Offset: 0x00000A68
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000030 RID: 48 RVA: 0x000028A0 File Offset: 0x00000AA0
		// (remove) Token: 0x06000031 RID: 49 RVA: 0x000028D8 File Offset: 0x00000AD8
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000032 RID: 50 RVA: 0x00002910 File Offset: 0x00000B10
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x00002948 File Offset: 0x00000B48
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000034 RID: 52 RVA: 0x00002980 File Offset: 0x00000B80
		// (remove) Token: 0x06000035 RID: 53 RVA: 0x000029B8 File Offset: 0x00000BB8
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000036 RID: 54 RVA: 0x000029F0 File Offset: 0x00000BF0
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x00002A28 File Offset: 0x00000C28
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000038 RID: 56 RVA: 0x00002A60 File Offset: 0x00000C60
		// (remove) Token: 0x06000039 RID: 57 RVA: 0x00002A98 File Offset: 0x00000C98
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600003A RID: 58 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (remove) Token: 0x0600003B RID: 59 RVA: 0x00002B08 File Offset: 0x00000D08
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600003C RID: 60 RVA: 0x00002B40 File Offset: 0x00000D40
		// (remove) Token: 0x0600003D RID: 61 RVA: 0x00002B78 File Offset: 0x00000D78
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600003E RID: 62 RVA: 0x00002BB0 File Offset: 0x00000DB0
		// (remove) Token: 0x0600003F RID: 63 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000040 RID: 64 RVA: 0x00002C20 File Offset: 0x00000E20
		// (remove) Token: 0x06000041 RID: 65 RVA: 0x00002C58 File Offset: 0x00000E58
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000042 RID: 66 RVA: 0x00002C90 File Offset: 0x00000E90
		// (remove) Token: 0x06000043 RID: 67 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000044 RID: 68 RVA: 0x00002D00 File Offset: 0x00000F00
		// (remove) Token: 0x06000045 RID: 69 RVA: 0x00002D38 File Offset: 0x00000F38
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000046 RID: 70 RVA: 0x00002D70 File Offset: 0x00000F70
		// (remove) Token: 0x06000047 RID: 71 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000048 RID: 72 RVA: 0x00002DE0 File Offset: 0x00000FE0
		// (remove) Token: 0x06000049 RID: 73 RVA: 0x00002E18 File Offset: 0x00001018
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600004A RID: 74 RVA: 0x00002E50 File Offset: 0x00001050
		// (remove) Token: 0x0600004B RID: 75 RVA: 0x00002E88 File Offset: 0x00001088
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600004C RID: 76 RVA: 0x00002EC0 File Offset: 0x000010C0
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x00002EF8 File Offset: 0x000010F8
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600004E RID: 78 RVA: 0x00002F30 File Offset: 0x00001130
		// (remove) Token: 0x0600004F RID: 79 RVA: 0x00002F68 File Offset: 0x00001168
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000050 RID: 80 RVA: 0x00002FA0 File Offset: 0x000011A0
		// (remove) Token: 0x06000051 RID: 81 RVA: 0x00002FD8 File Offset: 0x000011D8
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000052 RID: 82 RVA: 0x00003010 File Offset: 0x00001210
		// (remove) Token: 0x06000053 RID: 83 RVA: 0x00003048 File Offset: 0x00001248
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000054 RID: 84 RVA: 0x00003080 File Offset: 0x00001280
		// (remove) Token: 0x06000055 RID: 85 RVA: 0x000030B8 File Offset: 0x000012B8
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000056 RID: 86 RVA: 0x000030F0 File Offset: 0x000012F0
		// (remove) Token: 0x06000057 RID: 87 RVA: 0x00003128 File Offset: 0x00001328
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000058 RID: 88 RVA: 0x0000315D File Offset: 0x0000135D
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000317E File Offset: 0x0000137E
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000319F File Offset: 0x0000139F
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000031C1 File Offset: 0x000013C1
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000031E4 File Offset: 0x000013E4
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsTscAxNotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003234 File Offset: 0x00001434
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

		// Token: 0x0600005E RID: 94 RVA: 0x00003264 File Offset: 0x00001464
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

		// Token: 0x0600005F RID: 95 RVA: 0x00003298 File Offset: 0x00001498
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000032AF File Offset: 0x000014AF
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000032C6 File Offset: 0x000014C6
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000032DD File Offset: 0x000014DD
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000032F4 File Offset: 0x000014F4
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000330B File Offset: 0x0000150B
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003322 File Offset: 0x00001522
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003339 File Offset: 0x00001539
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003350 File Offset: 0x00001550
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003367 File Offset: 0x00001567
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000337E File Offset: 0x0000157E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003395 File Offset: 0x00001595
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000033AC File Offset: 0x000015AC
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000033C3 File Offset: 0x000015C3
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000033DA File Offset: 0x000015DA
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000033F1 File Offset: 0x000015F1
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003408 File Offset: 0x00001608
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000341F File Offset: 0x0000161F
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003436 File Offset: 0x00001636
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000344D File Offset: 0x0000164D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003464 File Offset: 0x00001664
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000347B File Offset: 0x0000167B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003492 File Offset: 0x00001692
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000034A9 File Offset: 0x000016A9
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000034C0 File Offset: 0x000016C0
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000034D7 File Offset: 0x000016D7
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000034EE File Offset: 0x000016EE
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003505 File Offset: 0x00001705
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000351C File Offset: 0x0000171C
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003533 File Offset: 0x00001733
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x04000001 RID: 1
		private IMsTscAx ocx;

		// Token: 0x04000002 RID: 2
		private AxMsTscAxNotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x04000003 RID: 3
		private AxHost.ConnectionPointCookie cookie;
	}
}
