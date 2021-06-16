using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000046 RID: 70
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{54d38bf7-b1ef-4479-9674-1bd6ea465258}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient7NotSafeForScripting : AxHost
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x0001F1C5 File Offset: 0x0001D3C5
		public AxMsRdpClient7NotSafeForScripting() : base("54d38bf7-b1ef-4479-9674-1bd6ea465258")
		{
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0001F1D2 File Offset: 0x0001D3D2
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x0001F1F3 File Offset: 0x0001D3F3
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0001F215 File Offset: 0x0001D415
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x0001F236 File Offset: 0x0001D436
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0001F258 File Offset: 0x0001D458
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x0001F279 File Offset: 0x0001D479
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

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0001F29B File Offset: 0x0001D49B
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x0001F2BC File Offset: 0x0001D4BC
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0001F2DE File Offset: 0x0001D4DE
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x0001F2FF File Offset: 0x0001D4FF
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

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0001F321 File Offset: 0x0001D521
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

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0001F342 File Offset: 0x0001D542
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x0001F363 File Offset: 0x0001D563
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0001F385 File Offset: 0x0001D585
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x0001F3A6 File Offset: 0x0001D5A6
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x0001F3E9 File Offset: 0x0001D5E9
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

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0001F40B File Offset: 0x0001D60B
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

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0001F42C File Offset: 0x0001D62C
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

		// Token: 0x170001C8 RID: 456
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0001F44D File Offset: 0x0001D64D
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

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0001F46F File Offset: 0x0001D66F
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x0001F490 File Offset: 0x0001D690
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0001F4B1 File Offset: 0x0001D6B1
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

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0001F4D2 File Offset: 0x0001D6D2
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0001F4F3 File Offset: 0x0001D6F3
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x0001F514 File Offset: 0x0001D714
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0001F535 File Offset: 0x0001D735
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x0001F556 File Offset: 0x0001D756
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

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0001F578 File Offset: 0x0001D778
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

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0001F599 File Offset: 0x0001D799
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0001F5BA File Offset: 0x0001D7BA
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0001F5DB File Offset: 0x0001D7DB
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0001F5FC File Offset: 0x0001D7FC
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

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0001F61E File Offset: 0x0001D81E
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0001F63F File Offset: 0x0001D83F
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x0001F660 File Offset: 0x0001D860
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0001F682 File Offset: 0x0001D882
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

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0001F6A3 File Offset: 0x0001D8A3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0001F6C4 File Offset: 0x0001D8C4
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

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0001F6E5 File Offset: 0x0001D8E5
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

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0001F706 File Offset: 0x0001D906
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

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0001F727 File Offset: 0x0001D927
		[Browsable(false)]
		[DispId(505)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0001F748 File Offset: 0x0001D948
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0001F769 File Offset: 0x0001D969
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0001F78A File Offset: 0x0001D98A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DispId(600)]
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

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0001F7AB File Offset: 0x0001D9AB
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

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0001F7CC File Offset: 0x0001D9CC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(603)]
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

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0001F7ED File Offset: 0x0001D9ED
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x140001FF RID: 511
		// (add) Token: 0x06000BB8 RID: 3000 RVA: 0x0001F810 File Offset: 0x0001DA10
		// (remove) Token: 0x06000BB9 RID: 3001 RVA: 0x0001F848 File Offset: 0x0001DA48
		public event EventHandler OnConnecting;

		// Token: 0x14000200 RID: 512
		// (add) Token: 0x06000BBA RID: 3002 RVA: 0x0001F880 File Offset: 0x0001DA80
		// (remove) Token: 0x06000BBB RID: 3003 RVA: 0x0001F8B8 File Offset: 0x0001DAB8
		public event EventHandler OnConnected;

		// Token: 0x14000201 RID: 513
		// (add) Token: 0x06000BBC RID: 3004 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		// (remove) Token: 0x06000BBD RID: 3005 RVA: 0x0001F928 File Offset: 0x0001DB28
		public event EventHandler OnLoginComplete;

		// Token: 0x14000202 RID: 514
		// (add) Token: 0x06000BBE RID: 3006 RVA: 0x0001F960 File Offset: 0x0001DB60
		// (remove) Token: 0x06000BBF RID: 3007 RVA: 0x0001F998 File Offset: 0x0001DB98
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000203 RID: 515
		// (add) Token: 0x06000BC0 RID: 3008 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
		// (remove) Token: 0x06000BC1 RID: 3009 RVA: 0x0001FA08 File Offset: 0x0001DC08
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000204 RID: 516
		// (add) Token: 0x06000BC2 RID: 3010 RVA: 0x0001FA40 File Offset: 0x0001DC40
		// (remove) Token: 0x06000BC3 RID: 3011 RVA: 0x0001FA78 File Offset: 0x0001DC78
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000205 RID: 517
		// (add) Token: 0x06000BC4 RID: 3012 RVA: 0x0001FAB0 File Offset: 0x0001DCB0
		// (remove) Token: 0x06000BC5 RID: 3013 RVA: 0x0001FAE8 File Offset: 0x0001DCE8
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000206 RID: 518
		// (add) Token: 0x06000BC6 RID: 3014 RVA: 0x0001FB20 File Offset: 0x0001DD20
		// (remove) Token: 0x06000BC7 RID: 3015 RVA: 0x0001FB58 File Offset: 0x0001DD58
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000207 RID: 519
		// (add) Token: 0x06000BC8 RID: 3016 RVA: 0x0001FB90 File Offset: 0x0001DD90
		// (remove) Token: 0x06000BC9 RID: 3017 RVA: 0x0001FBC8 File Offset: 0x0001DDC8
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000208 RID: 520
		// (add) Token: 0x06000BCA RID: 3018 RVA: 0x0001FC00 File Offset: 0x0001DE00
		// (remove) Token: 0x06000BCB RID: 3019 RVA: 0x0001FC38 File Offset: 0x0001DE38
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000209 RID: 521
		// (add) Token: 0x06000BCC RID: 3020 RVA: 0x0001FC70 File Offset: 0x0001DE70
		// (remove) Token: 0x06000BCD RID: 3021 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x1400020A RID: 522
		// (add) Token: 0x06000BCE RID: 3022 RVA: 0x0001FCE0 File Offset: 0x0001DEE0
		// (remove) Token: 0x06000BCF RID: 3023 RVA: 0x0001FD18 File Offset: 0x0001DF18
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x1400020B RID: 523
		// (add) Token: 0x06000BD0 RID: 3024 RVA: 0x0001FD50 File Offset: 0x0001DF50
		// (remove) Token: 0x06000BD1 RID: 3025 RVA: 0x0001FD88 File Offset: 0x0001DF88
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400020C RID: 524
		// (add) Token: 0x06000BD2 RID: 3026 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
		// (remove) Token: 0x06000BD3 RID: 3027 RVA: 0x0001FDF8 File Offset: 0x0001DFF8
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400020D RID: 525
		// (add) Token: 0x06000BD4 RID: 3028 RVA: 0x0001FE30 File Offset: 0x0001E030
		// (remove) Token: 0x06000BD5 RID: 3029 RVA: 0x0001FE68 File Offset: 0x0001E068
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400020E RID: 526
		// (add) Token: 0x06000BD6 RID: 3030 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
		// (remove) Token: 0x06000BD7 RID: 3031 RVA: 0x0001FED8 File Offset: 0x0001E0D8
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400020F RID: 527
		// (add) Token: 0x06000BD8 RID: 3032 RVA: 0x0001FF10 File Offset: 0x0001E110
		// (remove) Token: 0x06000BD9 RID: 3033 RVA: 0x0001FF48 File Offset: 0x0001E148
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000210 RID: 528
		// (add) Token: 0x06000BDA RID: 3034 RVA: 0x0001FF80 File Offset: 0x0001E180
		// (remove) Token: 0x06000BDB RID: 3035 RVA: 0x0001FFB8 File Offset: 0x0001E1B8
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000211 RID: 529
		// (add) Token: 0x06000BDC RID: 3036 RVA: 0x0001FFF0 File Offset: 0x0001E1F0
		// (remove) Token: 0x06000BDD RID: 3037 RVA: 0x00020028 File Offset: 0x0001E228
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000212 RID: 530
		// (add) Token: 0x06000BDE RID: 3038 RVA: 0x00020060 File Offset: 0x0001E260
		// (remove) Token: 0x06000BDF RID: 3039 RVA: 0x00020098 File Offset: 0x0001E298
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000213 RID: 531
		// (add) Token: 0x06000BE0 RID: 3040 RVA: 0x000200D0 File Offset: 0x0001E2D0
		// (remove) Token: 0x06000BE1 RID: 3041 RVA: 0x00020108 File Offset: 0x0001E308
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000214 RID: 532
		// (add) Token: 0x06000BE2 RID: 3042 RVA: 0x00020140 File Offset: 0x0001E340
		// (remove) Token: 0x06000BE3 RID: 3043 RVA: 0x00020178 File Offset: 0x0001E378
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000215 RID: 533
		// (add) Token: 0x06000BE4 RID: 3044 RVA: 0x000201B0 File Offset: 0x0001E3B0
		// (remove) Token: 0x06000BE5 RID: 3045 RVA: 0x000201E8 File Offset: 0x0001E3E8
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000216 RID: 534
		// (add) Token: 0x06000BE6 RID: 3046 RVA: 0x00020220 File Offset: 0x0001E420
		// (remove) Token: 0x06000BE7 RID: 3047 RVA: 0x00020258 File Offset: 0x0001E458
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000217 RID: 535
		// (add) Token: 0x06000BE8 RID: 3048 RVA: 0x00020290 File Offset: 0x0001E490
		// (remove) Token: 0x06000BE9 RID: 3049 RVA: 0x000202C8 File Offset: 0x0001E4C8
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000218 RID: 536
		// (add) Token: 0x06000BEA RID: 3050 RVA: 0x00020300 File Offset: 0x0001E500
		// (remove) Token: 0x06000BEB RID: 3051 RVA: 0x00020338 File Offset: 0x0001E538
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000219 RID: 537
		// (add) Token: 0x06000BEC RID: 3052 RVA: 0x00020370 File Offset: 0x0001E570
		// (remove) Token: 0x06000BED RID: 3053 RVA: 0x000203A8 File Offset: 0x0001E5A8
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400021A RID: 538
		// (add) Token: 0x06000BEE RID: 3054 RVA: 0x000203E0 File Offset: 0x0001E5E0
		// (remove) Token: 0x06000BEF RID: 3055 RVA: 0x00020418 File Offset: 0x0001E618
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x1400021B RID: 539
		// (add) Token: 0x06000BF0 RID: 3056 RVA: 0x00020450 File Offset: 0x0001E650
		// (remove) Token: 0x06000BF1 RID: 3057 RVA: 0x00020488 File Offset: 0x0001E688
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400021C RID: 540
		// (add) Token: 0x06000BF2 RID: 3058 RVA: 0x000204C0 File Offset: 0x0001E6C0
		// (remove) Token: 0x06000BF3 RID: 3059 RVA: 0x000204F8 File Offset: 0x0001E6F8
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002052D File Offset: 0x0001E72D
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002054E File Offset: 0x0001E74E
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002056F File Offset: 0x0001E76F
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00020591 File Offset: 0x0001E791
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x000205B4 File Offset: 0x0001E7B4
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000205D8 File Offset: 0x0001E7D8
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00020608 File Offset: 0x0001E808
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00020638 File Offset: 0x0001E838
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00020668 File Offset: 0x0001E868
		public virtual string GetStatusText(uint statusCode)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetStatusText", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetStatusText(statusCode);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00020698 File Offset: 0x0001E898
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient7NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000206E8 File Offset: 0x0001E8E8
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

		// Token: 0x06000BFF RID: 3071 RVA: 0x00020718 File Offset: 0x0001E918
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

		// Token: 0x06000C00 RID: 3072 RVA: 0x0002074C File Offset: 0x0001E94C
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00020763 File Offset: 0x0001E963
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0002077A File Offset: 0x0001E97A
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00020791 File Offset: 0x0001E991
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x000207A8 File Offset: 0x0001E9A8
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000207BF File Offset: 0x0001E9BF
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000207D6 File Offset: 0x0001E9D6
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000207ED File Offset: 0x0001E9ED
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00020804 File Offset: 0x0001EA04
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002081B File Offset: 0x0001EA1B
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00020832 File Offset: 0x0001EA32
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00020849 File Offset: 0x0001EA49
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00020860 File Offset: 0x0001EA60
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00020877 File Offset: 0x0001EA77
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0002088E File Offset: 0x0001EA8E
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000208A5 File Offset: 0x0001EAA5
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000208BC File Offset: 0x0001EABC
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000208D3 File Offset: 0x0001EAD3
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000208EA File Offset: 0x0001EAEA
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00020901 File Offset: 0x0001EB01
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00020918 File Offset: 0x0001EB18
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002092F File Offset: 0x0001EB2F
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00020946 File Offset: 0x0001EB46
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0002095D File Offset: 0x0001EB5D
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00020974 File Offset: 0x0001EB74
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002098B File Offset: 0x0001EB8B
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000209A2 File Offset: 0x0001EBA2
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000209B9 File Offset: 0x0001EBB9
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000209D0 File Offset: 0x0001EBD0
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000209E7 File Offset: 0x0001EBE7
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400025E RID: 606
		private IMsRdpClient7 ocx;

		// Token: 0x0400025F RID: 607
		private AxMsRdpClient7NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x04000260 RID: 608
		private AxHost.ConnectionPointCookie cookie;
	}
}
