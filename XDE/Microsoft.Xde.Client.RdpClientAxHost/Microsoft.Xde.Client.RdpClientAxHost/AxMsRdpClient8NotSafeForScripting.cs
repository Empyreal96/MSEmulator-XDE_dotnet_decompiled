using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200004A RID: 74
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{a3bc03a0-041d-42e3-ad22-882b7865c9c5}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient8NotSafeForScripting : AxHost
	{
		// Token: 0x06000CF3 RID: 3315 RVA: 0x00022C0D File Offset: 0x00020E0D
		public AxMsRdpClient8NotSafeForScripting() : base("a3bc03a0-041d-42e3-ad22-882b7865c9c5")
		{
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00022C1A File Offset: 0x00020E1A
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x00022C3B File Offset: 0x00020E3B
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00022C5D File Offset: 0x00020E5D
		// (set) Token: 0x06000CF7 RID: 3319 RVA: 0x00022C7E File Offset: 0x00020E7E
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00022CA0 File Offset: 0x00020EA0
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x00022CC1 File Offset: 0x00020EC1
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

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00022CE3 File Offset: 0x00020EE3
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x00022D04 File Offset: 0x00020F04
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00022D26 File Offset: 0x00020F26
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x00022D47 File Offset: 0x00020F47
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

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00022D69 File Offset: 0x00020F69
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00022D8A File Offset: 0x00020F8A
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x00022DAB File Offset: 0x00020FAB
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00022DCD File Offset: 0x00020FCD
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x00022DEE File Offset: 0x00020FEE
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00022E10 File Offset: 0x00021010
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x00022E31 File Offset: 0x00021031
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

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00022E53 File Offset: 0x00021053
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00022E74 File Offset: 0x00021074
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

		// Token: 0x17000212 RID: 530
		// (set) Token: 0x06000D07 RID: 3335 RVA: 0x00022E95 File Offset: 0x00021095
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00022EB7 File Offset: 0x000210B7
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00022ED8 File Offset: 0x000210D8
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

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00022EF9 File Offset: 0x000210F9
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

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00022F1A File Offset: 0x0002111A
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

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00022F3B File Offset: 0x0002113B
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

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00022F5C File Offset: 0x0002115C
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00022F7D File Offset: 0x0002117D
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x00022F9E File Offset: 0x0002119E
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00022FC0 File Offset: 0x000211C0
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00022FE1 File Offset: 0x000211E1
		[Browsable(false)]
		[DispId(102)]
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

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00023002 File Offset: 0x00021202
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

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x00023023 File Offset: 0x00021223
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x00023044 File Offset: 0x00021244
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

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00023066 File Offset: 0x00021266
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

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00023087 File Offset: 0x00021287
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x000230A8 File Offset: 0x000212A8
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

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x000230CA File Offset: 0x000212CA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000230EB File Offset: 0x000212EB
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

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0002310C File Offset: 0x0002130C
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

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0002312D File Offset: 0x0002132D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0002314E File Offset: 0x0002134E
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

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0002316F File Offset: 0x0002136F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x00023190 File Offset: 0x00021390
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

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x000231B1 File Offset: 0x000213B1
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

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x000231D2 File Offset: 0x000213D2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(600)]
		[Browsable(false)]
		public virtual IMsRdpClientAdvancedSettings7 AdvancedSettings8
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings8", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings8;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x000231F3 File Offset: 0x000213F3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(601)]
		public virtual IMsRdpClientTransportSettings3 TransportSettings3
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("TransportSettings3", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.TransportSettings3;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x00023214 File Offset: 0x00021414
		[DispId(603)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual IMsRdpClientSecuredSettings2 SecuredSettings3
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("SecuredSettings3", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.SecuredSettings3;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00023235 File Offset: 0x00021435
		[Browsable(false)]
		[DispId(604)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual ITSRemoteProgram2 RemoteProgram2
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("RemoteProgram2", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.RemoteProgram2;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00023256 File Offset: 0x00021456
		[DispId(701)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IMsRdpClientAdvancedSettings8 AdvancedSettings9
		{
			get
			{
				if (this.ocx == null)
				{
					throw new AxHost.InvalidActiveXStateException("AdvancedSettings9", AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AdvancedSettings9;
			}
		}

		// Token: 0x1400023B RID: 571
		// (add) Token: 0x06000D25 RID: 3365 RVA: 0x00023278 File Offset: 0x00021478
		// (remove) Token: 0x06000D26 RID: 3366 RVA: 0x000232B0 File Offset: 0x000214B0
		public event EventHandler OnConnecting;

		// Token: 0x1400023C RID: 572
		// (add) Token: 0x06000D27 RID: 3367 RVA: 0x000232E8 File Offset: 0x000214E8
		// (remove) Token: 0x06000D28 RID: 3368 RVA: 0x00023320 File Offset: 0x00021520
		public event EventHandler OnConnected;

		// Token: 0x1400023D RID: 573
		// (add) Token: 0x06000D29 RID: 3369 RVA: 0x00023358 File Offset: 0x00021558
		// (remove) Token: 0x06000D2A RID: 3370 RVA: 0x00023390 File Offset: 0x00021590
		public event EventHandler OnLoginComplete;

		// Token: 0x1400023E RID: 574
		// (add) Token: 0x06000D2B RID: 3371 RVA: 0x000233C8 File Offset: 0x000215C8
		// (remove) Token: 0x06000D2C RID: 3372 RVA: 0x00023400 File Offset: 0x00021600
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400023F RID: 575
		// (add) Token: 0x06000D2D RID: 3373 RVA: 0x00023438 File Offset: 0x00021638
		// (remove) Token: 0x06000D2E RID: 3374 RVA: 0x00023470 File Offset: 0x00021670
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000240 RID: 576
		// (add) Token: 0x06000D2F RID: 3375 RVA: 0x000234A8 File Offset: 0x000216A8
		// (remove) Token: 0x06000D30 RID: 3376 RVA: 0x000234E0 File Offset: 0x000216E0
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000241 RID: 577
		// (add) Token: 0x06000D31 RID: 3377 RVA: 0x00023518 File Offset: 0x00021718
		// (remove) Token: 0x06000D32 RID: 3378 RVA: 0x00023550 File Offset: 0x00021750
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000242 RID: 578
		// (add) Token: 0x06000D33 RID: 3379 RVA: 0x00023588 File Offset: 0x00021788
		// (remove) Token: 0x06000D34 RID: 3380 RVA: 0x000235C0 File Offset: 0x000217C0
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000243 RID: 579
		// (add) Token: 0x06000D35 RID: 3381 RVA: 0x000235F8 File Offset: 0x000217F8
		// (remove) Token: 0x06000D36 RID: 3382 RVA: 0x00023630 File Offset: 0x00021830
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000244 RID: 580
		// (add) Token: 0x06000D37 RID: 3383 RVA: 0x00023668 File Offset: 0x00021868
		// (remove) Token: 0x06000D38 RID: 3384 RVA: 0x000236A0 File Offset: 0x000218A0
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000245 RID: 581
		// (add) Token: 0x06000D39 RID: 3385 RVA: 0x000236D8 File Offset: 0x000218D8
		// (remove) Token: 0x06000D3A RID: 3386 RVA: 0x00023710 File Offset: 0x00021910
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000246 RID: 582
		// (add) Token: 0x06000D3B RID: 3387 RVA: 0x00023748 File Offset: 0x00021948
		// (remove) Token: 0x06000D3C RID: 3388 RVA: 0x00023780 File Offset: 0x00021980
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000247 RID: 583
		// (add) Token: 0x06000D3D RID: 3389 RVA: 0x000237B8 File Offset: 0x000219B8
		// (remove) Token: 0x06000D3E RID: 3390 RVA: 0x000237F0 File Offset: 0x000219F0
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000248 RID: 584
		// (add) Token: 0x06000D3F RID: 3391 RVA: 0x00023828 File Offset: 0x00021A28
		// (remove) Token: 0x06000D40 RID: 3392 RVA: 0x00023860 File Offset: 0x00021A60
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000249 RID: 585
		// (add) Token: 0x06000D41 RID: 3393 RVA: 0x00023898 File Offset: 0x00021A98
		// (remove) Token: 0x06000D42 RID: 3394 RVA: 0x000238D0 File Offset: 0x00021AD0
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400024A RID: 586
		// (add) Token: 0x06000D43 RID: 3395 RVA: 0x00023908 File Offset: 0x00021B08
		// (remove) Token: 0x06000D44 RID: 3396 RVA: 0x00023940 File Offset: 0x00021B40
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400024B RID: 587
		// (add) Token: 0x06000D45 RID: 3397 RVA: 0x00023978 File Offset: 0x00021B78
		// (remove) Token: 0x06000D46 RID: 3398 RVA: 0x000239B0 File Offset: 0x00021BB0
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400024C RID: 588
		// (add) Token: 0x06000D47 RID: 3399 RVA: 0x000239E8 File Offset: 0x00021BE8
		// (remove) Token: 0x06000D48 RID: 3400 RVA: 0x00023A20 File Offset: 0x00021C20
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400024D RID: 589
		// (add) Token: 0x06000D49 RID: 3401 RVA: 0x00023A58 File Offset: 0x00021C58
		// (remove) Token: 0x06000D4A RID: 3402 RVA: 0x00023A90 File Offset: 0x00021C90
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400024E RID: 590
		// (add) Token: 0x06000D4B RID: 3403 RVA: 0x00023AC8 File Offset: 0x00021CC8
		// (remove) Token: 0x06000D4C RID: 3404 RVA: 0x00023B00 File Offset: 0x00021D00
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400024F RID: 591
		// (add) Token: 0x06000D4D RID: 3405 RVA: 0x00023B38 File Offset: 0x00021D38
		// (remove) Token: 0x06000D4E RID: 3406 RVA: 0x00023B70 File Offset: 0x00021D70
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000250 RID: 592
		// (add) Token: 0x06000D4F RID: 3407 RVA: 0x00023BA8 File Offset: 0x00021DA8
		// (remove) Token: 0x06000D50 RID: 3408 RVA: 0x00023BE0 File Offset: 0x00021DE0
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000251 RID: 593
		// (add) Token: 0x06000D51 RID: 3409 RVA: 0x00023C18 File Offset: 0x00021E18
		// (remove) Token: 0x06000D52 RID: 3410 RVA: 0x00023C50 File Offset: 0x00021E50
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000252 RID: 594
		// (add) Token: 0x06000D53 RID: 3411 RVA: 0x00023C88 File Offset: 0x00021E88
		// (remove) Token: 0x06000D54 RID: 3412 RVA: 0x00023CC0 File Offset: 0x00021EC0
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000253 RID: 595
		// (add) Token: 0x06000D55 RID: 3413 RVA: 0x00023CF8 File Offset: 0x00021EF8
		// (remove) Token: 0x06000D56 RID: 3414 RVA: 0x00023D30 File Offset: 0x00021F30
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000254 RID: 596
		// (add) Token: 0x06000D57 RID: 3415 RVA: 0x00023D68 File Offset: 0x00021F68
		// (remove) Token: 0x06000D58 RID: 3416 RVA: 0x00023DA0 File Offset: 0x00021FA0
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000255 RID: 597
		// (add) Token: 0x06000D59 RID: 3417 RVA: 0x00023DD8 File Offset: 0x00021FD8
		// (remove) Token: 0x06000D5A RID: 3418 RVA: 0x00023E10 File Offset: 0x00022010
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000256 RID: 598
		// (add) Token: 0x06000D5B RID: 3419 RVA: 0x00023E48 File Offset: 0x00022048
		// (remove) Token: 0x06000D5C RID: 3420 RVA: 0x00023E80 File Offset: 0x00022080
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000257 RID: 599
		// (add) Token: 0x06000D5D RID: 3421 RVA: 0x00023EB8 File Offset: 0x000220B8
		// (remove) Token: 0x06000D5E RID: 3422 RVA: 0x00023EF0 File Offset: 0x000220F0
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000258 RID: 600
		// (add) Token: 0x06000D5F RID: 3423 RVA: 0x00023F28 File Offset: 0x00022128
		// (remove) Token: 0x06000D60 RID: 3424 RVA: 0x00023F60 File Offset: 0x00022160
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000D61 RID: 3425 RVA: 0x00023F95 File Offset: 0x00022195
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00023FB6 File Offset: 0x000221B6
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00023FD7 File Offset: 0x000221D7
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00023FF9 File Offset: 0x000221F9
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0002401C File Offset: 0x0002221C
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00024040 File Offset: 0x00022240
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00024070 File Offset: 0x00022270
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x000240A0 File Offset: 0x000222A0
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x000240D0 File Offset: 0x000222D0
		public virtual string GetStatusText(uint statusCode)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetStatusText", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetStatusText(statusCode);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x000240FF File Offset: 0x000222FF
		public virtual void SendRemoteAction(RemoteSessionActionType actionType)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendRemoteAction", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendRemoteAction(actionType);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00024124 File Offset: 0x00022324
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient8NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00024174 File Offset: 0x00022374
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

		// Token: 0x06000D6D RID: 3437 RVA: 0x000241A4 File Offset: 0x000223A4
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient8)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000241D8 File Offset: 0x000223D8
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000241EF File Offset: 0x000223EF
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00024206 File Offset: 0x00022406
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0002421D File Offset: 0x0002241D
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00024234 File Offset: 0x00022434
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0002424B File Offset: 0x0002244B
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00024262 File Offset: 0x00022462
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00024279 File Offset: 0x00022479
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00024290 File Offset: 0x00022490
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000242A7 File Offset: 0x000224A7
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000242BE File Offset: 0x000224BE
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x000242D5 File Offset: 0x000224D5
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x000242EC File Offset: 0x000224EC
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00024303 File Offset: 0x00022503
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002431A File Offset: 0x0002251A
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00024331 File Offset: 0x00022531
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00024348 File Offset: 0x00022548
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002435F File Offset: 0x0002255F
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00024376 File Offset: 0x00022576
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002438D File Offset: 0x0002258D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000243A4 File Offset: 0x000225A4
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000243BB File Offset: 0x000225BB
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000243D2 File Offset: 0x000225D2
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000243E9 File Offset: 0x000225E9
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00024400 File Offset: 0x00022600
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00024417 File Offset: 0x00022617
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002442E File Offset: 0x0002262E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00024445 File Offset: 0x00022645
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0002445C File Offset: 0x0002265C
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00024473 File Offset: 0x00022673
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040002A2 RID: 674
		private IMsRdpClient8 ocx;

		// Token: 0x040002A3 RID: 675
		private AxMsRdpClient8NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x040002A4 RID: 676
		private AxHost.ConnectionPointCookie cookie;
	}
}
