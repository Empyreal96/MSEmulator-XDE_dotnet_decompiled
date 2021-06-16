using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200003C RID: 60
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{54ce37e0-9834-41ae-9896-4dab69dc022b}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient4a : AxHost
	{
		// Token: 0x0600081D RID: 2077 RVA: 0x00016515 File Offset: 0x00014715
		public AxMsRdpClient4a() : base("54ce37e0-9834-41ae-9896-4dab69dc022b")
		{
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00016522 File Offset: 0x00014722
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00016543 File Offset: 0x00014743
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00016565 File Offset: 0x00014765
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x00016586 File Offset: 0x00014786
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

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x000165A8 File Offset: 0x000147A8
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x000165C9 File Offset: 0x000147C9
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x000165EB File Offset: 0x000147EB
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001660C File Offset: 0x0001480C
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001662E File Offset: 0x0001482E
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001664F File Offset: 0x0001484F
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00016671 File Offset: 0x00014871
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00016692 File Offset: 0x00014892
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x000166B3 File Offset: 0x000148B3
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x000166D5 File Offset: 0x000148D5
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x000166F6 File Offset: 0x000148F6
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00016718 File Offset: 0x00014918
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x00016739 File Offset: 0x00014939
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0001675B File Offset: 0x0001495B
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001677C File Offset: 0x0001497C
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

		// Token: 0x1700012D RID: 301
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x0001679D File Offset: 0x0001499D
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

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x000167BF File Offset: 0x000149BF
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x000167E0 File Offset: 0x000149E0
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x00016801 File Offset: 0x00014A01
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00016822 File Offset: 0x00014A22
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

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00016843 File Offset: 0x00014A43
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00016864 File Offset: 0x00014A64
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00016885 File Offset: 0x00014A85
		// (set) Token: 0x06000839 RID: 2105 RVA: 0x000168A6 File Offset: 0x00014AA6
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x000168C8 File Offset: 0x00014AC8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x000168E9 File Offset: 0x00014AE9
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

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001690A File Offset: 0x00014B0A
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

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0001692B File Offset: 0x00014B2B
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x0001694C File Offset: 0x00014B4C
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

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0001696E File Offset: 0x00014B6E
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

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001698F File Offset: 0x00014B8F
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x000169B0 File Offset: 0x00014BB0
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

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x000169D2 File Offset: 0x00014BD2
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

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x000169F3 File Offset: 0x00014BF3
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

		// Token: 0x14000169 RID: 361
		// (add) Token: 0x06000844 RID: 2116 RVA: 0x00016A14 File Offset: 0x00014C14
		// (remove) Token: 0x06000845 RID: 2117 RVA: 0x00016A4C File Offset: 0x00014C4C
		public event EventHandler OnConnecting;

		// Token: 0x1400016A RID: 362
		// (add) Token: 0x06000846 RID: 2118 RVA: 0x00016A84 File Offset: 0x00014C84
		// (remove) Token: 0x06000847 RID: 2119 RVA: 0x00016ABC File Offset: 0x00014CBC
		public event EventHandler OnConnected;

		// Token: 0x1400016B RID: 363
		// (add) Token: 0x06000848 RID: 2120 RVA: 0x00016AF4 File Offset: 0x00014CF4
		// (remove) Token: 0x06000849 RID: 2121 RVA: 0x00016B2C File Offset: 0x00014D2C
		public event EventHandler OnLoginComplete;

		// Token: 0x1400016C RID: 364
		// (add) Token: 0x0600084A RID: 2122 RVA: 0x00016B64 File Offset: 0x00014D64
		// (remove) Token: 0x0600084B RID: 2123 RVA: 0x00016B9C File Offset: 0x00014D9C
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400016D RID: 365
		// (add) Token: 0x0600084C RID: 2124 RVA: 0x00016BD4 File Offset: 0x00014DD4
		// (remove) Token: 0x0600084D RID: 2125 RVA: 0x00016C0C File Offset: 0x00014E0C
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x1400016E RID: 366
		// (add) Token: 0x0600084E RID: 2126 RVA: 0x00016C44 File Offset: 0x00014E44
		// (remove) Token: 0x0600084F RID: 2127 RVA: 0x00016C7C File Offset: 0x00014E7C
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x1400016F RID: 367
		// (add) Token: 0x06000850 RID: 2128 RVA: 0x00016CB4 File Offset: 0x00014EB4
		// (remove) Token: 0x06000851 RID: 2129 RVA: 0x00016CEC File Offset: 0x00014EEC
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000170 RID: 368
		// (add) Token: 0x06000852 RID: 2130 RVA: 0x00016D24 File Offset: 0x00014F24
		// (remove) Token: 0x06000853 RID: 2131 RVA: 0x00016D5C File Offset: 0x00014F5C
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000171 RID: 369
		// (add) Token: 0x06000854 RID: 2132 RVA: 0x00016D94 File Offset: 0x00014F94
		// (remove) Token: 0x06000855 RID: 2133 RVA: 0x00016DCC File Offset: 0x00014FCC
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000172 RID: 370
		// (add) Token: 0x06000856 RID: 2134 RVA: 0x00016E04 File Offset: 0x00015004
		// (remove) Token: 0x06000857 RID: 2135 RVA: 0x00016E3C File Offset: 0x0001503C
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000173 RID: 371
		// (add) Token: 0x06000858 RID: 2136 RVA: 0x00016E74 File Offset: 0x00015074
		// (remove) Token: 0x06000859 RID: 2137 RVA: 0x00016EAC File Offset: 0x000150AC
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000174 RID: 372
		// (add) Token: 0x0600085A RID: 2138 RVA: 0x00016EE4 File Offset: 0x000150E4
		// (remove) Token: 0x0600085B RID: 2139 RVA: 0x00016F1C File Offset: 0x0001511C
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000175 RID: 373
		// (add) Token: 0x0600085C RID: 2140 RVA: 0x00016F54 File Offset: 0x00015154
		// (remove) Token: 0x0600085D RID: 2141 RVA: 0x00016F8C File Offset: 0x0001518C
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000176 RID: 374
		// (add) Token: 0x0600085E RID: 2142 RVA: 0x00016FC4 File Offset: 0x000151C4
		// (remove) Token: 0x0600085F RID: 2143 RVA: 0x00016FFC File Offset: 0x000151FC
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000177 RID: 375
		// (add) Token: 0x06000860 RID: 2144 RVA: 0x00017034 File Offset: 0x00015234
		// (remove) Token: 0x06000861 RID: 2145 RVA: 0x0001706C File Offset: 0x0001526C
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000178 RID: 376
		// (add) Token: 0x06000862 RID: 2146 RVA: 0x000170A4 File Offset: 0x000152A4
		// (remove) Token: 0x06000863 RID: 2147 RVA: 0x000170DC File Offset: 0x000152DC
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000179 RID: 377
		// (add) Token: 0x06000864 RID: 2148 RVA: 0x00017114 File Offset: 0x00015314
		// (remove) Token: 0x06000865 RID: 2149 RVA: 0x0001714C File Offset: 0x0001534C
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400017A RID: 378
		// (add) Token: 0x06000866 RID: 2150 RVA: 0x00017184 File Offset: 0x00015384
		// (remove) Token: 0x06000867 RID: 2151 RVA: 0x000171BC File Offset: 0x000153BC
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400017B RID: 379
		// (add) Token: 0x06000868 RID: 2152 RVA: 0x000171F4 File Offset: 0x000153F4
		// (remove) Token: 0x06000869 RID: 2153 RVA: 0x0001722C File Offset: 0x0001542C
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400017C RID: 380
		// (add) Token: 0x0600086A RID: 2154 RVA: 0x00017264 File Offset: 0x00015464
		// (remove) Token: 0x0600086B RID: 2155 RVA: 0x0001729C File Offset: 0x0001549C
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400017D RID: 381
		// (add) Token: 0x0600086C RID: 2156 RVA: 0x000172D4 File Offset: 0x000154D4
		// (remove) Token: 0x0600086D RID: 2157 RVA: 0x0001730C File Offset: 0x0001550C
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x1400017E RID: 382
		// (add) Token: 0x0600086E RID: 2158 RVA: 0x00017344 File Offset: 0x00015544
		// (remove) Token: 0x0600086F RID: 2159 RVA: 0x0001737C File Offset: 0x0001557C
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x1400017F RID: 383
		// (add) Token: 0x06000870 RID: 2160 RVA: 0x000173B4 File Offset: 0x000155B4
		// (remove) Token: 0x06000871 RID: 2161 RVA: 0x000173EC File Offset: 0x000155EC
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000180 RID: 384
		// (add) Token: 0x06000872 RID: 2162 RVA: 0x00017424 File Offset: 0x00015624
		// (remove) Token: 0x06000873 RID: 2163 RVA: 0x0001745C File Offset: 0x0001565C
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000181 RID: 385
		// (add) Token: 0x06000874 RID: 2164 RVA: 0x00017494 File Offset: 0x00015694
		// (remove) Token: 0x06000875 RID: 2165 RVA: 0x000174CC File Offset: 0x000156CC
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000182 RID: 386
		// (add) Token: 0x06000876 RID: 2166 RVA: 0x00017504 File Offset: 0x00015704
		// (remove) Token: 0x06000877 RID: 2167 RVA: 0x0001753C File Offset: 0x0001573C
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000183 RID: 387
		// (add) Token: 0x06000878 RID: 2168 RVA: 0x00017574 File Offset: 0x00015774
		// (remove) Token: 0x06000879 RID: 2169 RVA: 0x000175AC File Offset: 0x000157AC
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000184 RID: 388
		// (add) Token: 0x0600087A RID: 2170 RVA: 0x000175E4 File Offset: 0x000157E4
		// (remove) Token: 0x0600087B RID: 2171 RVA: 0x0001761C File Offset: 0x0001581C
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000185 RID: 389
		// (add) Token: 0x0600087C RID: 2172 RVA: 0x00017654 File Offset: 0x00015854
		// (remove) Token: 0x0600087D RID: 2173 RVA: 0x0001768C File Offset: 0x0001588C
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000186 RID: 390
		// (add) Token: 0x0600087E RID: 2174 RVA: 0x000176C4 File Offset: 0x000158C4
		// (remove) Token: 0x0600087F RID: 2175 RVA: 0x000176FC File Offset: 0x000158FC
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000880 RID: 2176 RVA: 0x00017731 File Offset: 0x00015931
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00017752 File Offset: 0x00015952
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00017773 File Offset: 0x00015973
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00017795 File Offset: 0x00015995
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000177B8 File Offset: 0x000159B8
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000177DC File Offset: 0x000159DC
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001780C File Offset: 0x00015A0C
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001783C File Offset: 0x00015A3C
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient4aEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001788C File Offset: 0x00015A8C
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

		// Token: 0x06000889 RID: 2185 RVA: 0x000178BC File Offset: 0x00015ABC
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient4)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000178F0 File Offset: 0x00015AF0
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00017907 File Offset: 0x00015B07
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001791E File Offset: 0x00015B1E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00017935 File Offset: 0x00015B35
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001794C File Offset: 0x00015B4C
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00017963 File Offset: 0x00015B63
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001797A File Offset: 0x00015B7A
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00017991 File Offset: 0x00015B91
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000179A8 File Offset: 0x00015BA8
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000179BF File Offset: 0x00015BBF
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000179D6 File Offset: 0x00015BD6
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000179ED File Offset: 0x00015BED
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00017A04 File Offset: 0x00015C04
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00017A1B File Offset: 0x00015C1B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00017A32 File Offset: 0x00015C32
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00017A49 File Offset: 0x00015C49
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00017A60 File Offset: 0x00015C60
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00017A77 File Offset: 0x00015C77
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00017A8E File Offset: 0x00015C8E
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00017AA5 File Offset: 0x00015CA5
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00017ABC File Offset: 0x00015CBC
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00017AD3 File Offset: 0x00015CD3
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00017AEA File Offset: 0x00015CEA
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00017B01 File Offset: 0x00015D01
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00017B18 File Offset: 0x00015D18
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00017B2F File Offset: 0x00015D2F
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00017B46 File Offset: 0x00015D46
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00017B5D File Offset: 0x00015D5D
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00017B74 File Offset: 0x00015D74
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00017B8B File Offset: 0x00015D8B
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040001B4 RID: 436
		private IMsRdpClient4 ocx;

		// Token: 0x040001B5 RID: 437
		private AxMsRdpClient4aEventMulticaster eventMulticaster;

		// Token: 0x040001B6 RID: 438
		private AxHost.ConnectionPointCookie cookie;
	}
}
