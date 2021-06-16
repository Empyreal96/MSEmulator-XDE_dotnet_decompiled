using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000028 RID: 40
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{7cacbd7b-0d99-468f-ac33-22e495c0afe5}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClientNotSafeForScripting : AxHost
	{
		// Token: 0x0600018C RID: 396 RVA: 0x000054CD File Offset: 0x000036CD
		public AxMsRdpClientNotSafeForScripting() : base("7cacbd7b-0d99-468f-ac33-22e495c0afe5")
		{
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000054DA File Offset: 0x000036DA
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000054FB File Offset: 0x000036FB
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000551D File Offset: 0x0000371D
		// (set) Token: 0x06000190 RID: 400 RVA: 0x0000553E File Offset: 0x0000373E
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00005560 File Offset: 0x00003760
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00005581 File Offset: 0x00003781
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000055A3 File Offset: 0x000037A3
		// (set) Token: 0x06000194 RID: 404 RVA: 0x000055C4 File Offset: 0x000037C4
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000055E6 File Offset: 0x000037E6
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00005607 File Offset: 0x00003807
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00005629 File Offset: 0x00003829
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000564A File Offset: 0x0000384A
		// (set) Token: 0x06000199 RID: 409 RVA: 0x0000566B File Offset: 0x0000386B
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000568D File Offset: 0x0000388D
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000056AE File Offset: 0x000038AE
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000056D0 File Offset: 0x000038D0
		// (set) Token: 0x0600019D RID: 413 RVA: 0x000056F1 File Offset: 0x000038F1
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00005713 File Offset: 0x00003913
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00005734 File Offset: 0x00003934
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

		// Token: 0x17000030 RID: 48
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00005755 File Offset: 0x00003955
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00005777 File Offset: 0x00003977
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00005798 File Offset: 0x00003998
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000057B9 File Offset: 0x000039B9
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x000057DA File Offset: 0x000039DA
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000057FB File Offset: 0x000039FB
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000581C File Offset: 0x00003A1C
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000583D File Offset: 0x00003A3D
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000585E File Offset: 0x00003A5E
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00005880 File Offset: 0x00003A80
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(101)]
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000058A1 File Offset: 0x00003AA1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000058C2 File Offset: 0x00003AC2
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000058E3 File Offset: 0x00003AE3
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00005904 File Offset: 0x00003B04
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

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x060001AE RID: 430 RVA: 0x00005928 File Offset: 0x00003B28
		// (remove) Token: 0x060001AF RID: 431 RVA: 0x00005960 File Offset: 0x00003B60
		public event EventHandler OnConnecting;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x060001B0 RID: 432 RVA: 0x00005998 File Offset: 0x00003B98
		// (remove) Token: 0x060001B1 RID: 433 RVA: 0x000059D0 File Offset: 0x00003BD0
		public event EventHandler OnConnected;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x060001B2 RID: 434 RVA: 0x00005A08 File Offset: 0x00003C08
		// (remove) Token: 0x060001B3 RID: 435 RVA: 0x00005A40 File Offset: 0x00003C40
		public event EventHandler OnLoginComplete;

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x060001B4 RID: 436 RVA: 0x00005A78 File Offset: 0x00003C78
		// (remove) Token: 0x060001B5 RID: 437 RVA: 0x00005AB0 File Offset: 0x00003CB0
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x060001B6 RID: 438 RVA: 0x00005AE8 File Offset: 0x00003CE8
		// (remove) Token: 0x060001B7 RID: 439 RVA: 0x00005B20 File Offset: 0x00003D20
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x060001B8 RID: 440 RVA: 0x00005B58 File Offset: 0x00003D58
		// (remove) Token: 0x060001B9 RID: 441 RVA: 0x00005B90 File Offset: 0x00003D90
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x060001BA RID: 442 RVA: 0x00005BC8 File Offset: 0x00003DC8
		// (remove) Token: 0x060001BB RID: 443 RVA: 0x00005C00 File Offset: 0x00003E00
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060001BC RID: 444 RVA: 0x00005C38 File Offset: 0x00003E38
		// (remove) Token: 0x060001BD RID: 445 RVA: 0x00005C70 File Offset: 0x00003E70
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060001BE RID: 446 RVA: 0x00005CA8 File Offset: 0x00003EA8
		// (remove) Token: 0x060001BF RID: 447 RVA: 0x00005CE0 File Offset: 0x00003EE0
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060001C0 RID: 448 RVA: 0x00005D18 File Offset: 0x00003F18
		// (remove) Token: 0x060001C1 RID: 449 RVA: 0x00005D50 File Offset: 0x00003F50
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060001C2 RID: 450 RVA: 0x00005D88 File Offset: 0x00003F88
		// (remove) Token: 0x060001C3 RID: 451 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x060001C4 RID: 452 RVA: 0x00005DF8 File Offset: 0x00003FF8
		// (remove) Token: 0x060001C5 RID: 453 RVA: 0x00005E30 File Offset: 0x00004030
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x060001C6 RID: 454 RVA: 0x00005E68 File Offset: 0x00004068
		// (remove) Token: 0x060001C7 RID: 455 RVA: 0x00005EA0 File Offset: 0x000040A0
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x060001C8 RID: 456 RVA: 0x00005ED8 File Offset: 0x000040D8
		// (remove) Token: 0x060001C9 RID: 457 RVA: 0x00005F10 File Offset: 0x00004110
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x060001CA RID: 458 RVA: 0x00005F48 File Offset: 0x00004148
		// (remove) Token: 0x060001CB RID: 459 RVA: 0x00005F80 File Offset: 0x00004180
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x060001CC RID: 460 RVA: 0x00005FB8 File Offset: 0x000041B8
		// (remove) Token: 0x060001CD RID: 461 RVA: 0x00005FF0 File Offset: 0x000041F0
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x060001CE RID: 462 RVA: 0x00006028 File Offset: 0x00004228
		// (remove) Token: 0x060001CF RID: 463 RVA: 0x00006060 File Offset: 0x00004260
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060001D0 RID: 464 RVA: 0x00006098 File Offset: 0x00004298
		// (remove) Token: 0x060001D1 RID: 465 RVA: 0x000060D0 File Offset: 0x000042D0
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060001D2 RID: 466 RVA: 0x00006108 File Offset: 0x00004308
		// (remove) Token: 0x060001D3 RID: 467 RVA: 0x00006140 File Offset: 0x00004340
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060001D4 RID: 468 RVA: 0x00006178 File Offset: 0x00004378
		// (remove) Token: 0x060001D5 RID: 469 RVA: 0x000061B0 File Offset: 0x000043B0
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060001D6 RID: 470 RVA: 0x000061E8 File Offset: 0x000043E8
		// (remove) Token: 0x060001D7 RID: 471 RVA: 0x00006220 File Offset: 0x00004420
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060001D8 RID: 472 RVA: 0x00006258 File Offset: 0x00004458
		// (remove) Token: 0x060001D9 RID: 473 RVA: 0x00006290 File Offset: 0x00004490
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060001DA RID: 474 RVA: 0x000062C8 File Offset: 0x000044C8
		// (remove) Token: 0x060001DB RID: 475 RVA: 0x00006300 File Offset: 0x00004500
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060001DC RID: 476 RVA: 0x00006338 File Offset: 0x00004538
		// (remove) Token: 0x060001DD RID: 477 RVA: 0x00006370 File Offset: 0x00004570
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060001DE RID: 478 RVA: 0x000063A8 File Offset: 0x000045A8
		// (remove) Token: 0x060001DF RID: 479 RVA: 0x000063E0 File Offset: 0x000045E0
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060001E0 RID: 480 RVA: 0x00006418 File Offset: 0x00004618
		// (remove) Token: 0x060001E1 RID: 481 RVA: 0x00006450 File Offset: 0x00004650
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060001E2 RID: 482 RVA: 0x00006488 File Offset: 0x00004688
		// (remove) Token: 0x060001E3 RID: 483 RVA: 0x000064C0 File Offset: 0x000046C0
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060001E4 RID: 484 RVA: 0x000064F8 File Offset: 0x000046F8
		// (remove) Token: 0x060001E5 RID: 485 RVA: 0x00006530 File Offset: 0x00004730
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x060001E6 RID: 486 RVA: 0x00006568 File Offset: 0x00004768
		// (remove) Token: 0x060001E7 RID: 487 RVA: 0x000065A0 File Offset: 0x000047A0
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060001E8 RID: 488 RVA: 0x000065D8 File Offset: 0x000047D8
		// (remove) Token: 0x060001E9 RID: 489 RVA: 0x00006610 File Offset: 0x00004810
		public event EventHandler OnAutoReconnected;

		// Token: 0x060001EA RID: 490 RVA: 0x00006645 File Offset: 0x00004845
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006666 File Offset: 0x00004866
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006687 File Offset: 0x00004887
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000066A9 File Offset: 0x000048A9
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000066CC File Offset: 0x000048CC
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000066F0 File Offset: 0x000048F0
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00006720 File Offset: 0x00004920
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006750 File Offset: 0x00004950
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClientNotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000067A0 File Offset: 0x000049A0
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

		// Token: 0x060001F3 RID: 499 RVA: 0x000067D0 File Offset: 0x000049D0
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

		// Token: 0x060001F4 RID: 500 RVA: 0x00006804 File Offset: 0x00004A04
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000681B File Offset: 0x00004A1B
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00006832 File Offset: 0x00004A32
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00006849 File Offset: 0x00004A49
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00006860 File Offset: 0x00004A60
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00006877 File Offset: 0x00004A77
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000688E File Offset: 0x00004A8E
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000068A5 File Offset: 0x00004AA5
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000068BC File Offset: 0x00004ABC
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000068D3 File Offset: 0x00004AD3
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000068EA File Offset: 0x00004AEA
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00006901 File Offset: 0x00004B01
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00006918 File Offset: 0x00004B18
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000692F File Offset: 0x00004B2F
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00006946 File Offset: 0x00004B46
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000695D File Offset: 0x00004B5D
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00006974 File Offset: 0x00004B74
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000698B File Offset: 0x00004B8B
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000069A2 File Offset: 0x00004BA2
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000069B9 File Offset: 0x00004BB9
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000069D0 File Offset: 0x00004BD0
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000069E7 File Offset: 0x00004BE7
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000069FE File Offset: 0x00004BFE
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00006A15 File Offset: 0x00004C15
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00006A2C File Offset: 0x00004C2C
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00006A43 File Offset: 0x00004C43
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00006A5A File Offset: 0x00004C5A
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00006A71 File Offset: 0x00004C71
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00006A88 File Offset: 0x00004C88
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00006A9F File Offset: 0x00004C9F
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x04000060 RID: 96
		private IMsRdpClient ocx;

		// Token: 0x04000061 RID: 97
		private AxMsRdpClientNotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x04000062 RID: 98
		private AxHost.ConnectionPointCookie cookie;
	}
}
