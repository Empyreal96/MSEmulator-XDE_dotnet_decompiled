using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200004C RID: 76
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{5f681803-2900-4c43-a1cc-cf405404a676}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient8 : AxHost
	{
		// Token: 0x06000DAB RID: 3499 RVA: 0x00024975 File Offset: 0x00022B75
		public AxMsRdpClient8() : base("5f681803-2900-4c43-a1cc-cf405404a676")
		{
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00024982 File Offset: 0x00022B82
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x000249A3 File Offset: 0x00022BA3
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

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x000249C5 File Offset: 0x00022BC5
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x000249E6 File Offset: 0x00022BE6
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

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00024A08 File Offset: 0x00022C08
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x00024A29 File Offset: 0x00022C29
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

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x00024A4B File Offset: 0x00022C4B
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00024A6C File Offset: 0x00022C6C
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x00024A8E File Offset: 0x00022C8E
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x00024AAF File Offset: 0x00022CAF
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

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00024AD1 File Offset: 0x00022CD1
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00024AF2 File Offset: 0x00022CF2
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x00024B13 File Offset: 0x00022D13
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

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00024B35 File Offset: 0x00022D35
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x00024B56 File Offset: 0x00022D56
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00024B78 File Offset: 0x00022D78
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x00024B99 File Offset: 0x00022D99
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

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00024BBB File Offset: 0x00022DBB
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

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00024BDC File Offset: 0x00022DDC
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

		// Token: 0x17000238 RID: 568
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x00024BFD File Offset: 0x00022DFD
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00024C1F File Offset: 0x00022E1F
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

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00024C40 File Offset: 0x00022E40
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

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00024C61 File Offset: 0x00022E61
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

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00024C82 File Offset: 0x00022E82
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

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00024CA3 File Offset: 0x00022EA3
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

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00024CC4 File Offset: 0x00022EC4
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

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00024CE5 File Offset: 0x00022EE5
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x00024D06 File Offset: 0x00022F06
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

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00024D28 File Offset: 0x00022F28
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

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00024D49 File Offset: 0x00022F49
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

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00024D6A File Offset: 0x00022F6A
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

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00024D8B File Offset: 0x00022F8B
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x00024DAC File Offset: 0x00022FAC
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

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00024DCE File Offset: 0x00022FCE
		[DispId(200)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00024DEF File Offset: 0x00022FEF
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x00024E10 File Offset: 0x00023010
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

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00024E32 File Offset: 0x00023032
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

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x00024E53 File Offset: 0x00023053
		[DispId(400)]
		[Browsable(false)]
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

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00024E74 File Offset: 0x00023074
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(500)]
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

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00024E95 File Offset: 0x00023095
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

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00024EB6 File Offset: 0x000230B6
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

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x00024ED7 File Offset: 0x000230D7
		[DispId(505)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00024EF8 File Offset: 0x000230F8
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

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00024F19 File Offset: 0x00023119
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(506)]
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

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00024F3A File Offset: 0x0002313A
		[DispId(600)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00024F5B File Offset: 0x0002315B
		[DispId(601)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00024F7C File Offset: 0x0002317C
		[Browsable(false)]
		[DispId(603)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00024F9D File Offset: 0x0002319D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(604)]
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

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00024FBE File Offset: 0x000231BE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(701)]
		[Browsable(false)]
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

		// Token: 0x14000259 RID: 601
		// (add) Token: 0x06000DDD RID: 3549 RVA: 0x00024FE0 File Offset: 0x000231E0
		// (remove) Token: 0x06000DDE RID: 3550 RVA: 0x00025018 File Offset: 0x00023218
		public event EventHandler OnConnecting;

		// Token: 0x1400025A RID: 602
		// (add) Token: 0x06000DDF RID: 3551 RVA: 0x00025050 File Offset: 0x00023250
		// (remove) Token: 0x06000DE0 RID: 3552 RVA: 0x00025088 File Offset: 0x00023288
		public event EventHandler OnConnected;

		// Token: 0x1400025B RID: 603
		// (add) Token: 0x06000DE1 RID: 3553 RVA: 0x000250C0 File Offset: 0x000232C0
		// (remove) Token: 0x06000DE2 RID: 3554 RVA: 0x000250F8 File Offset: 0x000232F8
		public event EventHandler OnLoginComplete;

		// Token: 0x1400025C RID: 604
		// (add) Token: 0x06000DE3 RID: 3555 RVA: 0x00025130 File Offset: 0x00023330
		// (remove) Token: 0x06000DE4 RID: 3556 RVA: 0x00025168 File Offset: 0x00023368
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400025D RID: 605
		// (add) Token: 0x06000DE5 RID: 3557 RVA: 0x000251A0 File Offset: 0x000233A0
		// (remove) Token: 0x06000DE6 RID: 3558 RVA: 0x000251D8 File Offset: 0x000233D8
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x1400025E RID: 606
		// (add) Token: 0x06000DE7 RID: 3559 RVA: 0x00025210 File Offset: 0x00023410
		// (remove) Token: 0x06000DE8 RID: 3560 RVA: 0x00025248 File Offset: 0x00023448
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x1400025F RID: 607
		// (add) Token: 0x06000DE9 RID: 3561 RVA: 0x00025280 File Offset: 0x00023480
		// (remove) Token: 0x06000DEA RID: 3562 RVA: 0x000252B8 File Offset: 0x000234B8
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000260 RID: 608
		// (add) Token: 0x06000DEB RID: 3563 RVA: 0x000252F0 File Offset: 0x000234F0
		// (remove) Token: 0x06000DEC RID: 3564 RVA: 0x00025328 File Offset: 0x00023528
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000261 RID: 609
		// (add) Token: 0x06000DED RID: 3565 RVA: 0x00025360 File Offset: 0x00023560
		// (remove) Token: 0x06000DEE RID: 3566 RVA: 0x00025398 File Offset: 0x00023598
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000262 RID: 610
		// (add) Token: 0x06000DEF RID: 3567 RVA: 0x000253D0 File Offset: 0x000235D0
		// (remove) Token: 0x06000DF0 RID: 3568 RVA: 0x00025408 File Offset: 0x00023608
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000263 RID: 611
		// (add) Token: 0x06000DF1 RID: 3569 RVA: 0x00025440 File Offset: 0x00023640
		// (remove) Token: 0x06000DF2 RID: 3570 RVA: 0x00025478 File Offset: 0x00023678
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000264 RID: 612
		// (add) Token: 0x06000DF3 RID: 3571 RVA: 0x000254B0 File Offset: 0x000236B0
		// (remove) Token: 0x06000DF4 RID: 3572 RVA: 0x000254E8 File Offset: 0x000236E8
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000265 RID: 613
		// (add) Token: 0x06000DF5 RID: 3573 RVA: 0x00025520 File Offset: 0x00023720
		// (remove) Token: 0x06000DF6 RID: 3574 RVA: 0x00025558 File Offset: 0x00023758
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000266 RID: 614
		// (add) Token: 0x06000DF7 RID: 3575 RVA: 0x00025590 File Offset: 0x00023790
		// (remove) Token: 0x06000DF8 RID: 3576 RVA: 0x000255C8 File Offset: 0x000237C8
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000267 RID: 615
		// (add) Token: 0x06000DF9 RID: 3577 RVA: 0x00025600 File Offset: 0x00023800
		// (remove) Token: 0x06000DFA RID: 3578 RVA: 0x00025638 File Offset: 0x00023838
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000268 RID: 616
		// (add) Token: 0x06000DFB RID: 3579 RVA: 0x00025670 File Offset: 0x00023870
		// (remove) Token: 0x06000DFC RID: 3580 RVA: 0x000256A8 File Offset: 0x000238A8
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000269 RID: 617
		// (add) Token: 0x06000DFD RID: 3581 RVA: 0x000256E0 File Offset: 0x000238E0
		// (remove) Token: 0x06000DFE RID: 3582 RVA: 0x00025718 File Offset: 0x00023918
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400026A RID: 618
		// (add) Token: 0x06000DFF RID: 3583 RVA: 0x00025750 File Offset: 0x00023950
		// (remove) Token: 0x06000E00 RID: 3584 RVA: 0x00025788 File Offset: 0x00023988
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400026B RID: 619
		// (add) Token: 0x06000E01 RID: 3585 RVA: 0x000257C0 File Offset: 0x000239C0
		// (remove) Token: 0x06000E02 RID: 3586 RVA: 0x000257F8 File Offset: 0x000239F8
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400026C RID: 620
		// (add) Token: 0x06000E03 RID: 3587 RVA: 0x00025830 File Offset: 0x00023A30
		// (remove) Token: 0x06000E04 RID: 3588 RVA: 0x00025868 File Offset: 0x00023A68
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400026D RID: 621
		// (add) Token: 0x06000E05 RID: 3589 RVA: 0x000258A0 File Offset: 0x00023AA0
		// (remove) Token: 0x06000E06 RID: 3590 RVA: 0x000258D8 File Offset: 0x00023AD8
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x1400026E RID: 622
		// (add) Token: 0x06000E07 RID: 3591 RVA: 0x00025910 File Offset: 0x00023B10
		// (remove) Token: 0x06000E08 RID: 3592 RVA: 0x00025948 File Offset: 0x00023B48
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x1400026F RID: 623
		// (add) Token: 0x06000E09 RID: 3593 RVA: 0x00025980 File Offset: 0x00023B80
		// (remove) Token: 0x06000E0A RID: 3594 RVA: 0x000259B8 File Offset: 0x00023BB8
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000270 RID: 624
		// (add) Token: 0x06000E0B RID: 3595 RVA: 0x000259F0 File Offset: 0x00023BF0
		// (remove) Token: 0x06000E0C RID: 3596 RVA: 0x00025A28 File Offset: 0x00023C28
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000271 RID: 625
		// (add) Token: 0x06000E0D RID: 3597 RVA: 0x00025A60 File Offset: 0x00023C60
		// (remove) Token: 0x06000E0E RID: 3598 RVA: 0x00025A98 File Offset: 0x00023C98
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000272 RID: 626
		// (add) Token: 0x06000E0F RID: 3599 RVA: 0x00025AD0 File Offset: 0x00023CD0
		// (remove) Token: 0x06000E10 RID: 3600 RVA: 0x00025B08 File Offset: 0x00023D08
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000273 RID: 627
		// (add) Token: 0x06000E11 RID: 3601 RVA: 0x00025B40 File Offset: 0x00023D40
		// (remove) Token: 0x06000E12 RID: 3602 RVA: 0x00025B78 File Offset: 0x00023D78
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000274 RID: 628
		// (add) Token: 0x06000E13 RID: 3603 RVA: 0x00025BB0 File Offset: 0x00023DB0
		// (remove) Token: 0x06000E14 RID: 3604 RVA: 0x00025BE8 File Offset: 0x00023DE8
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000275 RID: 629
		// (add) Token: 0x06000E15 RID: 3605 RVA: 0x00025C20 File Offset: 0x00023E20
		// (remove) Token: 0x06000E16 RID: 3606 RVA: 0x00025C58 File Offset: 0x00023E58
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000276 RID: 630
		// (add) Token: 0x06000E17 RID: 3607 RVA: 0x00025C90 File Offset: 0x00023E90
		// (remove) Token: 0x06000E18 RID: 3608 RVA: 0x00025CC8 File Offset: 0x00023EC8
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000E19 RID: 3609 RVA: 0x00025CFD File Offset: 0x00023EFD
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00025D1E File Offset: 0x00023F1E
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00025D3F File Offset: 0x00023F3F
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00025D61 File Offset: 0x00023F61
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00025D84 File Offset: 0x00023F84
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00025DA8 File Offset: 0x00023FA8
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00025DD8 File Offset: 0x00023FD8
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00025E08 File Offset: 0x00024008
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00025E38 File Offset: 0x00024038
		public virtual string GetStatusText(uint statusCode)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetStatusText", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetStatusText(statusCode);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00025E67 File Offset: 0x00024067
		public virtual void SendRemoteAction(RemoteSessionActionType actionType)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendRemoteAction", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendRemoteAction(actionType);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00025E8C File Offset: 0x0002408C
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient8EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00025EDC File Offset: 0x000240DC
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

		// Token: 0x06000E25 RID: 3621 RVA: 0x00025F0C File Offset: 0x0002410C
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

		// Token: 0x06000E26 RID: 3622 RVA: 0x00025F40 File Offset: 0x00024140
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00025F57 File Offset: 0x00024157
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00025F6E File Offset: 0x0002416E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00025F85 File Offset: 0x00024185
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00025F9C File Offset: 0x0002419C
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00025FB3 File Offset: 0x000241B3
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00025FCA File Offset: 0x000241CA
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00025FE1 File Offset: 0x000241E1
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00025FF8 File Offset: 0x000241F8
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0002600F File Offset: 0x0002420F
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00026026 File Offset: 0x00024226
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0002603D File Offset: 0x0002423D
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00026054 File Offset: 0x00024254
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0002606B File Offset: 0x0002426B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00026082 File Offset: 0x00024282
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00026099 File Offset: 0x00024299
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000260B0 File Offset: 0x000242B0
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x000260C7 File Offset: 0x000242C7
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000260DE File Offset: 0x000242DE
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000260F5 File Offset: 0x000242F5
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0002610C File Offset: 0x0002430C
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00026123 File Offset: 0x00024323
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0002613A File Offset: 0x0002433A
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00026151 File Offset: 0x00024351
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00026168 File Offset: 0x00024368
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0002617F File Offset: 0x0002437F
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00026196 File Offset: 0x00024396
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x000261AD File Offset: 0x000243AD
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000261C4 File Offset: 0x000243C4
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x000261DB File Offset: 0x000243DB
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040002C4 RID: 708
		private IMsRdpClient8 ocx;

		// Token: 0x040002C5 RID: 709
		private AxMsRdpClient8EventMulticaster eventMulticaster;

		// Token: 0x040002C6 RID: 710
		private AxHost.ConnectionPointCookie cookie;
	}
}
