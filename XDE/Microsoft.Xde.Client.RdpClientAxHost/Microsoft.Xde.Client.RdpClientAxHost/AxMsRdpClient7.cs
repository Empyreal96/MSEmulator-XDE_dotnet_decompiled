using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000048 RID: 72
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{a9d7038d-b5ed-472e-9c47-94bea90a5910}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient7 : AxHost
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x00020EE9 File Offset: 0x0001F0E9
		public AxMsRdpClient7() : base("a9d7038d-b5ed-472e-9c47-94bea90a5910")
		{
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00020EF6 File Offset: 0x0001F0F6
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x00020F17 File Offset: 0x0001F117
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

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00020F39 File Offset: 0x0001F139
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00020F5A File Offset: 0x0001F15A
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

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00020F7C File Offset: 0x0001F17C
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x00020F9D File Offset: 0x0001F19D
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00020FBF File Offset: 0x0001F1BF
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x00020FE0 File Offset: 0x0001F1E0
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

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00021002 File Offset: 0x0001F202
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x00021023 File Offset: 0x0001F223
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00021045 File Offset: 0x0001F245
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00021066 File Offset: 0x0001F266
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x00021087 File Offset: 0x0001F287
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

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x000210A9 File Offset: 0x0001F2A9
		// (set) Token: 0x06000C4C RID: 3148 RVA: 0x000210CA File Offset: 0x0001F2CA
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

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x000210EC File Offset: 0x0001F2EC
		// (set) Token: 0x06000C4E RID: 3150 RVA: 0x0002110D File Offset: 0x0001F30D
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

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0002112F File Offset: 0x0001F32F
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

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00021150 File Offset: 0x0001F350
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

		// Token: 0x170001ED RID: 493
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x00021171 File Offset: 0x0001F371
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00021193 File Offset: 0x0001F393
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x000211B4 File Offset: 0x0001F3B4
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

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x000211D5 File Offset: 0x0001F3D5
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

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x000211F6 File Offset: 0x0001F3F6
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00021217 File Offset: 0x0001F417
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00021238 File Offset: 0x0001F438
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

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00021259 File Offset: 0x0001F459
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0002127A File Offset: 0x0001F47A
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

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0002129C File Offset: 0x0001F49C
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x000212BD File Offset: 0x0001F4BD
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

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x000212DE File Offset: 0x0001F4DE
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

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x000212FF File Offset: 0x0001F4FF
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x00021320 File Offset: 0x0001F520
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

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00021342 File Offset: 0x0001F542
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

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00021363 File Offset: 0x0001F563
		// (set) Token: 0x06000C61 RID: 3169 RVA: 0x00021384 File Offset: 0x0001F584
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

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000213A6 File Offset: 0x0001F5A6
		[DispId(300)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x000213C7 File Offset: 0x0001F5C7
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

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x000213E8 File Offset: 0x0001F5E8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00021409 File Offset: 0x0001F609
		[Browsable(false)]
		[DispId(502)]
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

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0002142A File Offset: 0x0001F62A
		[DispId(504)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0002144B File Offset: 0x0001F64B
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

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0002146C File Offset: 0x0001F66C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(507)]
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

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0002148D File Offset: 0x0001F68D
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000214AE File Offset: 0x0001F6AE
		[Browsable(false)]
		[DispId(600)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x000214CF File Offset: 0x0001F6CF
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000214F0 File Offset: 0x0001F6F0
		[DispId(603)]
		[Browsable(false)]
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

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00021511 File Offset: 0x0001F711
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

		// Token: 0x1400021D RID: 541
		// (add) Token: 0x06000C6E RID: 3182 RVA: 0x00021534 File Offset: 0x0001F734
		// (remove) Token: 0x06000C6F RID: 3183 RVA: 0x0002156C File Offset: 0x0001F76C
		public event EventHandler OnConnecting;

		// Token: 0x1400021E RID: 542
		// (add) Token: 0x06000C70 RID: 3184 RVA: 0x000215A4 File Offset: 0x0001F7A4
		// (remove) Token: 0x06000C71 RID: 3185 RVA: 0x000215DC File Offset: 0x0001F7DC
		public event EventHandler OnConnected;

		// Token: 0x1400021F RID: 543
		// (add) Token: 0x06000C72 RID: 3186 RVA: 0x00021614 File Offset: 0x0001F814
		// (remove) Token: 0x06000C73 RID: 3187 RVA: 0x0002164C File Offset: 0x0001F84C
		public event EventHandler OnLoginComplete;

		// Token: 0x14000220 RID: 544
		// (add) Token: 0x06000C74 RID: 3188 RVA: 0x00021684 File Offset: 0x0001F884
		// (remove) Token: 0x06000C75 RID: 3189 RVA: 0x000216BC File Offset: 0x0001F8BC
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000221 RID: 545
		// (add) Token: 0x06000C76 RID: 3190 RVA: 0x000216F4 File Offset: 0x0001F8F4
		// (remove) Token: 0x06000C77 RID: 3191 RVA: 0x0002172C File Offset: 0x0001F92C
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000222 RID: 546
		// (add) Token: 0x06000C78 RID: 3192 RVA: 0x00021764 File Offset: 0x0001F964
		// (remove) Token: 0x06000C79 RID: 3193 RVA: 0x0002179C File Offset: 0x0001F99C
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000223 RID: 547
		// (add) Token: 0x06000C7A RID: 3194 RVA: 0x000217D4 File Offset: 0x0001F9D4
		// (remove) Token: 0x06000C7B RID: 3195 RVA: 0x0002180C File Offset: 0x0001FA0C
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000224 RID: 548
		// (add) Token: 0x06000C7C RID: 3196 RVA: 0x00021844 File Offset: 0x0001FA44
		// (remove) Token: 0x06000C7D RID: 3197 RVA: 0x0002187C File Offset: 0x0001FA7C
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000225 RID: 549
		// (add) Token: 0x06000C7E RID: 3198 RVA: 0x000218B4 File Offset: 0x0001FAB4
		// (remove) Token: 0x06000C7F RID: 3199 RVA: 0x000218EC File Offset: 0x0001FAEC
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000226 RID: 550
		// (add) Token: 0x06000C80 RID: 3200 RVA: 0x00021924 File Offset: 0x0001FB24
		// (remove) Token: 0x06000C81 RID: 3201 RVA: 0x0002195C File Offset: 0x0001FB5C
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000227 RID: 551
		// (add) Token: 0x06000C82 RID: 3202 RVA: 0x00021994 File Offset: 0x0001FB94
		// (remove) Token: 0x06000C83 RID: 3203 RVA: 0x000219CC File Offset: 0x0001FBCC
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000228 RID: 552
		// (add) Token: 0x06000C84 RID: 3204 RVA: 0x00021A04 File Offset: 0x0001FC04
		// (remove) Token: 0x06000C85 RID: 3205 RVA: 0x00021A3C File Offset: 0x0001FC3C
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000229 RID: 553
		// (add) Token: 0x06000C86 RID: 3206 RVA: 0x00021A74 File Offset: 0x0001FC74
		// (remove) Token: 0x06000C87 RID: 3207 RVA: 0x00021AAC File Offset: 0x0001FCAC
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400022A RID: 554
		// (add) Token: 0x06000C88 RID: 3208 RVA: 0x00021AE4 File Offset: 0x0001FCE4
		// (remove) Token: 0x06000C89 RID: 3209 RVA: 0x00021B1C File Offset: 0x0001FD1C
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400022B RID: 555
		// (add) Token: 0x06000C8A RID: 3210 RVA: 0x00021B54 File Offset: 0x0001FD54
		// (remove) Token: 0x06000C8B RID: 3211 RVA: 0x00021B8C File Offset: 0x0001FD8C
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400022C RID: 556
		// (add) Token: 0x06000C8C RID: 3212 RVA: 0x00021BC4 File Offset: 0x0001FDC4
		// (remove) Token: 0x06000C8D RID: 3213 RVA: 0x00021BFC File Offset: 0x0001FDFC
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400022D RID: 557
		// (add) Token: 0x06000C8E RID: 3214 RVA: 0x00021C34 File Offset: 0x0001FE34
		// (remove) Token: 0x06000C8F RID: 3215 RVA: 0x00021C6C File Offset: 0x0001FE6C
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400022E RID: 558
		// (add) Token: 0x06000C90 RID: 3216 RVA: 0x00021CA4 File Offset: 0x0001FEA4
		// (remove) Token: 0x06000C91 RID: 3217 RVA: 0x00021CDC File Offset: 0x0001FEDC
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400022F RID: 559
		// (add) Token: 0x06000C92 RID: 3218 RVA: 0x00021D14 File Offset: 0x0001FF14
		// (remove) Token: 0x06000C93 RID: 3219 RVA: 0x00021D4C File Offset: 0x0001FF4C
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000230 RID: 560
		// (add) Token: 0x06000C94 RID: 3220 RVA: 0x00021D84 File Offset: 0x0001FF84
		// (remove) Token: 0x06000C95 RID: 3221 RVA: 0x00021DBC File Offset: 0x0001FFBC
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000231 RID: 561
		// (add) Token: 0x06000C96 RID: 3222 RVA: 0x00021DF4 File Offset: 0x0001FFF4
		// (remove) Token: 0x06000C97 RID: 3223 RVA: 0x00021E2C File Offset: 0x0002002C
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000232 RID: 562
		// (add) Token: 0x06000C98 RID: 3224 RVA: 0x00021E64 File Offset: 0x00020064
		// (remove) Token: 0x06000C99 RID: 3225 RVA: 0x00021E9C File Offset: 0x0002009C
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000233 RID: 563
		// (add) Token: 0x06000C9A RID: 3226 RVA: 0x00021ED4 File Offset: 0x000200D4
		// (remove) Token: 0x06000C9B RID: 3227 RVA: 0x00021F0C File Offset: 0x0002010C
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000234 RID: 564
		// (add) Token: 0x06000C9C RID: 3228 RVA: 0x00021F44 File Offset: 0x00020144
		// (remove) Token: 0x06000C9D RID: 3229 RVA: 0x00021F7C File Offset: 0x0002017C
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000235 RID: 565
		// (add) Token: 0x06000C9E RID: 3230 RVA: 0x00021FB4 File Offset: 0x000201B4
		// (remove) Token: 0x06000C9F RID: 3231 RVA: 0x00021FEC File Offset: 0x000201EC
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000236 RID: 566
		// (add) Token: 0x06000CA0 RID: 3232 RVA: 0x00022024 File Offset: 0x00020224
		// (remove) Token: 0x06000CA1 RID: 3233 RVA: 0x0002205C File Offset: 0x0002025C
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000237 RID: 567
		// (add) Token: 0x06000CA2 RID: 3234 RVA: 0x00022094 File Offset: 0x00020294
		// (remove) Token: 0x06000CA3 RID: 3235 RVA: 0x000220CC File Offset: 0x000202CC
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000238 RID: 568
		// (add) Token: 0x06000CA4 RID: 3236 RVA: 0x00022104 File Offset: 0x00020304
		// (remove) Token: 0x06000CA5 RID: 3237 RVA: 0x0002213C File Offset: 0x0002033C
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000239 RID: 569
		// (add) Token: 0x06000CA6 RID: 3238 RVA: 0x00022174 File Offset: 0x00020374
		// (remove) Token: 0x06000CA7 RID: 3239 RVA: 0x000221AC File Offset: 0x000203AC
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400023A RID: 570
		// (add) Token: 0x06000CA8 RID: 3240 RVA: 0x000221E4 File Offset: 0x000203E4
		// (remove) Token: 0x06000CA9 RID: 3241 RVA: 0x0002221C File Offset: 0x0002041C
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000CAA RID: 3242 RVA: 0x00022251 File Offset: 0x00020451
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00022272 File Offset: 0x00020472
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00022293 File Offset: 0x00020493
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000222B5 File Offset: 0x000204B5
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x000222D8 File Offset: 0x000204D8
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x000222FC File Offset: 0x000204FC
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0002232C File Offset: 0x0002052C
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0002235C File Offset: 0x0002055C
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0002238C File Offset: 0x0002058C
		public virtual string GetStatusText(uint statusCode)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetStatusText", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetStatusText(statusCode);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000223BC File Offset: 0x000205BC
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient7EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0002240C File Offset: 0x0002060C
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

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0002243C File Offset: 0x0002063C
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient7)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00022470 File Offset: 0x00020670
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00022487 File Offset: 0x00020687
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0002249E File Offset: 0x0002069E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000224B5 File Offset: 0x000206B5
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000224CC File Offset: 0x000206CC
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x000224E3 File Offset: 0x000206E3
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000224FA File Offset: 0x000206FA
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00022511 File Offset: 0x00020711
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00022528 File Offset: 0x00020728
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002253F File Offset: 0x0002073F
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00022556 File Offset: 0x00020756
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002256D File Offset: 0x0002076D
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00022584 File Offset: 0x00020784
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002259B File Offset: 0x0002079B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000225B2 File Offset: 0x000207B2
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000225C9 File Offset: 0x000207C9
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x000225E0 File Offset: 0x000207E0
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x000225F7 File Offset: 0x000207F7
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002260E File Offset: 0x0002080E
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00022625 File Offset: 0x00020825
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0002263C File Offset: 0x0002083C
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00022653 File Offset: 0x00020853
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002266A File Offset: 0x0002086A
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00022681 File Offset: 0x00020881
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00022698 File Offset: 0x00020898
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000226AF File Offset: 0x000208AF
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x000226C6 File Offset: 0x000208C6
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000226DD File Offset: 0x000208DD
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000226F4 File Offset: 0x000208F4
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002270B File Offset: 0x0002090B
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x04000280 RID: 640
		private IMsRdpClient7 ocx;

		// Token: 0x04000281 RID: 641
		private AxMsRdpClient7EventMulticaster eventMulticaster;

		// Token: 0x04000282 RID: 642
		private AxHost.ConnectionPointCookie cookie;
	}
}
