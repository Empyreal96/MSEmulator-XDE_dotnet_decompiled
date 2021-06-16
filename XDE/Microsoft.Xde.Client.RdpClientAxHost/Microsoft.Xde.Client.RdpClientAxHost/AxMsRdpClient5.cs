using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000040 RID: 64
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{4eb89ff4-7f78-4a0f-8b8d-2bf02e94e4b2}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient5 : AxHost
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x00019CB9 File Offset: 0x00017EB9
		public AxMsRdpClient5() : base("4eb89ff4-7f78-4a0f-8b8d-2bf02e94e4b2")
		{
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x00019CC6 File Offset: 0x00017EC6
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x00019CE7 File Offset: 0x00017EE7
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00019D09 File Offset: 0x00017F09
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x00019D2A File Offset: 0x00017F2A
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

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00019D4C File Offset: 0x00017F4C
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x00019D6D File Offset: 0x00017F6D
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

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00019D8F File Offset: 0x00017F8F
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x00019DB0 File Offset: 0x00017FB0
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

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00019DD2 File Offset: 0x00017FD2
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x00019DF3 File Offset: 0x00017FF3
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

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x00019E15 File Offset: 0x00018015
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x00019E36 File Offset: 0x00018036
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x00019E57 File Offset: 0x00018057
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

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x00019E79 File Offset: 0x00018079
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x00019E9A File Offset: 0x0001809A
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

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x00019EBC File Offset: 0x000180BC
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x00019EDD File Offset: 0x000180DD
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

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x00019EFF File Offset: 0x000180FF
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

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00019F20 File Offset: 0x00018120
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

		// Token: 0x17000167 RID: 359
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x00019F41 File Offset: 0x00018141
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

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00019F63 File Offset: 0x00018163
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00019F84 File Offset: 0x00018184
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

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00019FA5 File Offset: 0x000181A5
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

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00019FC6 File Offset: 0x000181C6
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x00019FE7 File Offset: 0x000181E7
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

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001A008 File Offset: 0x00018208
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

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001A029 File Offset: 0x00018229
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0001A04A File Offset: 0x0001824A
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

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0001A06C File Offset: 0x0001826C
		[Browsable(false)]
		[DispId(101)]
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

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001A08D File Offset: 0x0001828D
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

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001A0AE File Offset: 0x000182AE
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

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0001A0CF File Offset: 0x000182CF
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0001A0F0 File Offset: 0x000182F0
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0001A112 File Offset: 0x00018312
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0001A133 File Offset: 0x00018333
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0001A154 File Offset: 0x00018354
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

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001A176 File Offset: 0x00018376
		[DispId(300)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0001A197 File Offset: 0x00018397
		[Browsable(false)]
		[DispId(400)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001A1B8 File Offset: 0x000183B8
		[DispId(500)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001A1D9 File Offset: 0x000183D9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(502)]
		[Browsable(false)]
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0001A1FA File Offset: 0x000183FA
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0001A21B File Offset: 0x0001841B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(505)]
		[Browsable(false)]
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

		// Token: 0x140001A5 RID: 421
		// (add) Token: 0x060009A1 RID: 2465 RVA: 0x0001A23C File Offset: 0x0001843C
		// (remove) Token: 0x060009A2 RID: 2466 RVA: 0x0001A274 File Offset: 0x00018474
		public event EventHandler OnConnecting;

		// Token: 0x140001A6 RID: 422
		// (add) Token: 0x060009A3 RID: 2467 RVA: 0x0001A2AC File Offset: 0x000184AC
		// (remove) Token: 0x060009A4 RID: 2468 RVA: 0x0001A2E4 File Offset: 0x000184E4
		public event EventHandler OnConnected;

		// Token: 0x140001A7 RID: 423
		// (add) Token: 0x060009A5 RID: 2469 RVA: 0x0001A31C File Offset: 0x0001851C
		// (remove) Token: 0x060009A6 RID: 2470 RVA: 0x0001A354 File Offset: 0x00018554
		public event EventHandler OnLoginComplete;

		// Token: 0x140001A8 RID: 424
		// (add) Token: 0x060009A7 RID: 2471 RVA: 0x0001A38C File Offset: 0x0001858C
		// (remove) Token: 0x060009A8 RID: 2472 RVA: 0x0001A3C4 File Offset: 0x000185C4
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140001A9 RID: 425
		// (add) Token: 0x060009A9 RID: 2473 RVA: 0x0001A3FC File Offset: 0x000185FC
		// (remove) Token: 0x060009AA RID: 2474 RVA: 0x0001A434 File Offset: 0x00018634
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x140001AA RID: 426
		// (add) Token: 0x060009AB RID: 2475 RVA: 0x0001A46C File Offset: 0x0001866C
		// (remove) Token: 0x060009AC RID: 2476 RVA: 0x0001A4A4 File Offset: 0x000186A4
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x140001AB RID: 427
		// (add) Token: 0x060009AD RID: 2477 RVA: 0x0001A4DC File Offset: 0x000186DC
		// (remove) Token: 0x060009AE RID: 2478 RVA: 0x0001A514 File Offset: 0x00018714
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140001AC RID: 428
		// (add) Token: 0x060009AF RID: 2479 RVA: 0x0001A54C File Offset: 0x0001874C
		// (remove) Token: 0x060009B0 RID: 2480 RVA: 0x0001A584 File Offset: 0x00018784
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x140001AD RID: 429
		// (add) Token: 0x060009B1 RID: 2481 RVA: 0x0001A5BC File Offset: 0x000187BC
		// (remove) Token: 0x060009B2 RID: 2482 RVA: 0x0001A5F4 File Offset: 0x000187F4
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140001AE RID: 430
		// (add) Token: 0x060009B3 RID: 2483 RVA: 0x0001A62C File Offset: 0x0001882C
		// (remove) Token: 0x060009B4 RID: 2484 RVA: 0x0001A664 File Offset: 0x00018864
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140001AF RID: 431
		// (add) Token: 0x060009B5 RID: 2485 RVA: 0x0001A69C File Offset: 0x0001889C
		// (remove) Token: 0x060009B6 RID: 2486 RVA: 0x0001A6D4 File Offset: 0x000188D4
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140001B0 RID: 432
		// (add) Token: 0x060009B7 RID: 2487 RVA: 0x0001A70C File Offset: 0x0001890C
		// (remove) Token: 0x060009B8 RID: 2488 RVA: 0x0001A744 File Offset: 0x00018944
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140001B1 RID: 433
		// (add) Token: 0x060009B9 RID: 2489 RVA: 0x0001A77C File Offset: 0x0001897C
		// (remove) Token: 0x060009BA RID: 2490 RVA: 0x0001A7B4 File Offset: 0x000189B4
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140001B2 RID: 434
		// (add) Token: 0x060009BB RID: 2491 RVA: 0x0001A7EC File Offset: 0x000189EC
		// (remove) Token: 0x060009BC RID: 2492 RVA: 0x0001A824 File Offset: 0x00018A24
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140001B3 RID: 435
		// (add) Token: 0x060009BD RID: 2493 RVA: 0x0001A85C File Offset: 0x00018A5C
		// (remove) Token: 0x060009BE RID: 2494 RVA: 0x0001A894 File Offset: 0x00018A94
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140001B4 RID: 436
		// (add) Token: 0x060009BF RID: 2495 RVA: 0x0001A8CC File Offset: 0x00018ACC
		// (remove) Token: 0x060009C0 RID: 2496 RVA: 0x0001A904 File Offset: 0x00018B04
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140001B5 RID: 437
		// (add) Token: 0x060009C1 RID: 2497 RVA: 0x0001A93C File Offset: 0x00018B3C
		// (remove) Token: 0x060009C2 RID: 2498 RVA: 0x0001A974 File Offset: 0x00018B74
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140001B6 RID: 438
		// (add) Token: 0x060009C3 RID: 2499 RVA: 0x0001A9AC File Offset: 0x00018BAC
		// (remove) Token: 0x060009C4 RID: 2500 RVA: 0x0001A9E4 File Offset: 0x00018BE4
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140001B7 RID: 439
		// (add) Token: 0x060009C5 RID: 2501 RVA: 0x0001AA1C File Offset: 0x00018C1C
		// (remove) Token: 0x060009C6 RID: 2502 RVA: 0x0001AA54 File Offset: 0x00018C54
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140001B8 RID: 440
		// (add) Token: 0x060009C7 RID: 2503 RVA: 0x0001AA8C File Offset: 0x00018C8C
		// (remove) Token: 0x060009C8 RID: 2504 RVA: 0x0001AAC4 File Offset: 0x00018CC4
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140001B9 RID: 441
		// (add) Token: 0x060009C9 RID: 2505 RVA: 0x0001AAFC File Offset: 0x00018CFC
		// (remove) Token: 0x060009CA RID: 2506 RVA: 0x0001AB34 File Offset: 0x00018D34
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140001BA RID: 442
		// (add) Token: 0x060009CB RID: 2507 RVA: 0x0001AB6C File Offset: 0x00018D6C
		// (remove) Token: 0x060009CC RID: 2508 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140001BB RID: 443
		// (add) Token: 0x060009CD RID: 2509 RVA: 0x0001ABDC File Offset: 0x00018DDC
		// (remove) Token: 0x060009CE RID: 2510 RVA: 0x0001AC14 File Offset: 0x00018E14
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140001BC RID: 444
		// (add) Token: 0x060009CF RID: 2511 RVA: 0x0001AC4C File Offset: 0x00018E4C
		// (remove) Token: 0x060009D0 RID: 2512 RVA: 0x0001AC84 File Offset: 0x00018E84
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140001BD RID: 445
		// (add) Token: 0x060009D1 RID: 2513 RVA: 0x0001ACBC File Offset: 0x00018EBC
		// (remove) Token: 0x060009D2 RID: 2514 RVA: 0x0001ACF4 File Offset: 0x00018EF4
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140001BE RID: 446
		// (add) Token: 0x060009D3 RID: 2515 RVA: 0x0001AD2C File Offset: 0x00018F2C
		// (remove) Token: 0x060009D4 RID: 2516 RVA: 0x0001AD64 File Offset: 0x00018F64
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140001BF RID: 447
		// (add) Token: 0x060009D5 RID: 2517 RVA: 0x0001AD9C File Offset: 0x00018F9C
		// (remove) Token: 0x060009D6 RID: 2518 RVA: 0x0001ADD4 File Offset: 0x00018FD4
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140001C0 RID: 448
		// (add) Token: 0x060009D7 RID: 2519 RVA: 0x0001AE0C File Offset: 0x0001900C
		// (remove) Token: 0x060009D8 RID: 2520 RVA: 0x0001AE44 File Offset: 0x00019044
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140001C1 RID: 449
		// (add) Token: 0x060009D9 RID: 2521 RVA: 0x0001AE7C File Offset: 0x0001907C
		// (remove) Token: 0x060009DA RID: 2522 RVA: 0x0001AEB4 File Offset: 0x000190B4
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140001C2 RID: 450
		// (add) Token: 0x060009DB RID: 2523 RVA: 0x0001AEEC File Offset: 0x000190EC
		// (remove) Token: 0x060009DC RID: 2524 RVA: 0x0001AF24 File Offset: 0x00019124
		public event EventHandler OnAutoReconnected;

		// Token: 0x060009DD RID: 2525 RVA: 0x0001AF59 File Offset: 0x00019159
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001AF7A File Offset: 0x0001917A
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001AF9B File Offset: 0x0001919B
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001AFBD File Offset: 0x000191BD
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001AFE0 File Offset: 0x000191E0
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0001B004 File Offset: 0x00019204
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001B034 File Offset: 0x00019234
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001B064 File Offset: 0x00019264
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0001B094 File Offset: 0x00019294
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient5EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0001B0E4 File Offset: 0x000192E4
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

		// Token: 0x060009E7 RID: 2535 RVA: 0x0001B114 File Offset: 0x00019314
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient5)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0001B148 File Offset: 0x00019348
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0001B15F File Offset: 0x0001935F
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0001B176 File Offset: 0x00019376
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0001B18D File Offset: 0x0001938D
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0001B1A4 File Offset: 0x000193A4
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0001B1BB File Offset: 0x000193BB
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0001B1D2 File Offset: 0x000193D2
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0001B1E9 File Offset: 0x000193E9
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0001B200 File Offset: 0x00019400
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0001B217 File Offset: 0x00019417
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0001B22E File Offset: 0x0001942E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001B245 File Offset: 0x00019445
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001B25C File Offset: 0x0001945C
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001B273 File Offset: 0x00019473
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0001B28A File Offset: 0x0001948A
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001B2A1 File Offset: 0x000194A1
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0001B2B8 File Offset: 0x000194B8
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0001B2CF File Offset: 0x000194CF
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0001B2E6 File Offset: 0x000194E6
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0001B2FD File Offset: 0x000194FD
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0001B314 File Offset: 0x00019514
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001B32B File Offset: 0x0001952B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001B342 File Offset: 0x00019542
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001B359 File Offset: 0x00019559
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0001B370 File Offset: 0x00019570
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001B387 File Offset: 0x00019587
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001B39E File Offset: 0x0001959E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001B3B5 File Offset: 0x000195B5
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0001B3CC File Offset: 0x000195CC
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0001B3E3 File Offset: 0x000195E3
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040001F8 RID: 504
		private IMsRdpClient5 ocx;

		// Token: 0x040001F9 RID: 505
		private AxMsRdpClient5EventMulticaster eventMulticaster;

		// Token: 0x040001FA RID: 506
		private AxHost.ConnectionPointCookie cookie;
	}
}
