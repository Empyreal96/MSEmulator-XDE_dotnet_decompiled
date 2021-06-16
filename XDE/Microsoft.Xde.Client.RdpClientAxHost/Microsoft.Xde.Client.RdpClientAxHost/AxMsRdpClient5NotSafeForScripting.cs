using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200003E RID: 62
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{4eb2f086-c818-447e-b32c-c51ce2b30d31}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient5NotSafeForScripting : AxHost
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x0001808D File Offset: 0x0001628D
		public AxMsRdpClient5NotSafeForScripting() : base("4eb2f086-c818-447e-b32c-c51ce2b30d31")
		{
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0001809A File Offset: 0x0001629A
		// (set) Token: 0x060008C9 RID: 2249 RVA: 0x000180BB File Offset: 0x000162BB
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

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x000180DD File Offset: 0x000162DD
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x000180FE File Offset: 0x000162FE
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

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00018120 File Offset: 0x00016320
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00018141 File Offset: 0x00016341
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00018163 File Offset: 0x00016363
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x00018184 File Offset: 0x00016384
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x000181A6 File Offset: 0x000163A6
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x000181C7 File Offset: 0x000163C7
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x000181E9 File Offset: 0x000163E9
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

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001820A File Offset: 0x0001640A
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0001822B File Offset: 0x0001642B
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

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001824D File Offset: 0x0001644D
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0001826E File Offset: 0x0001646E
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

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00018290 File Offset: 0x00016490
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x000182B1 File Offset: 0x000164B1
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

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x000182D3 File Offset: 0x000164D3
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

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x000182F4 File Offset: 0x000164F4
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

		// Token: 0x17000148 RID: 328
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00018315 File Offset: 0x00016515
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

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00018337 File Offset: 0x00016537
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

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00018358 File Offset: 0x00016558
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

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00018379 File Offset: 0x00016579
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001839A File Offset: 0x0001659A
		[DispId(97)]
		[Browsable(false)]
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x000183BB File Offset: 0x000165BB
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

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x000183DC File Offset: 0x000165DC
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000183FD File Offset: 0x000165FD
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0001841E File Offset: 0x0001661E
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00018440 File Offset: 0x00016640
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00018461 File Offset: 0x00016661
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00018482 File Offset: 0x00016682
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x000184A3 File Offset: 0x000166A3
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x000184C4 File Offset: 0x000166C4
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x000184E6 File Offset: 0x000166E6
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x00018507 File Offset: 0x00016707
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x00018528 File Offset: 0x00016728
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001854A File Offset: 0x0001674A
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0001856B File Offset: 0x0001676B
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

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0001858C File Offset: 0x0001678C
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

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x000185AD File Offset: 0x000167AD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x000185CE File Offset: 0x000167CE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(504)]
		[Browsable(false)]
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

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x000185EF File Offset: 0x000167EF
		[DispId(505)]
		[Browsable(false)]
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

		// Token: 0x14000187 RID: 391
		// (add) Token: 0x060008F2 RID: 2290 RVA: 0x00018610 File Offset: 0x00016810
		// (remove) Token: 0x060008F3 RID: 2291 RVA: 0x00018648 File Offset: 0x00016848
		public event EventHandler OnConnecting;

		// Token: 0x14000188 RID: 392
		// (add) Token: 0x060008F4 RID: 2292 RVA: 0x00018680 File Offset: 0x00016880
		// (remove) Token: 0x060008F5 RID: 2293 RVA: 0x000186B8 File Offset: 0x000168B8
		public event EventHandler OnConnected;

		// Token: 0x14000189 RID: 393
		// (add) Token: 0x060008F6 RID: 2294 RVA: 0x000186F0 File Offset: 0x000168F0
		// (remove) Token: 0x060008F7 RID: 2295 RVA: 0x00018728 File Offset: 0x00016928
		public event EventHandler OnLoginComplete;

		// Token: 0x1400018A RID: 394
		// (add) Token: 0x060008F8 RID: 2296 RVA: 0x00018760 File Offset: 0x00016960
		// (remove) Token: 0x060008F9 RID: 2297 RVA: 0x00018798 File Offset: 0x00016998
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400018B RID: 395
		// (add) Token: 0x060008FA RID: 2298 RVA: 0x000187D0 File Offset: 0x000169D0
		// (remove) Token: 0x060008FB RID: 2299 RVA: 0x00018808 File Offset: 0x00016A08
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x1400018C RID: 396
		// (add) Token: 0x060008FC RID: 2300 RVA: 0x00018840 File Offset: 0x00016A40
		// (remove) Token: 0x060008FD RID: 2301 RVA: 0x00018878 File Offset: 0x00016A78
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x1400018D RID: 397
		// (add) Token: 0x060008FE RID: 2302 RVA: 0x000188B0 File Offset: 0x00016AB0
		// (remove) Token: 0x060008FF RID: 2303 RVA: 0x000188E8 File Offset: 0x00016AE8
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x1400018E RID: 398
		// (add) Token: 0x06000900 RID: 2304 RVA: 0x00018920 File Offset: 0x00016B20
		// (remove) Token: 0x06000901 RID: 2305 RVA: 0x00018958 File Offset: 0x00016B58
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x1400018F RID: 399
		// (add) Token: 0x06000902 RID: 2306 RVA: 0x00018990 File Offset: 0x00016B90
		// (remove) Token: 0x06000903 RID: 2307 RVA: 0x000189C8 File Offset: 0x00016BC8
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000190 RID: 400
		// (add) Token: 0x06000904 RID: 2308 RVA: 0x00018A00 File Offset: 0x00016C00
		// (remove) Token: 0x06000905 RID: 2309 RVA: 0x00018A38 File Offset: 0x00016C38
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000191 RID: 401
		// (add) Token: 0x06000906 RID: 2310 RVA: 0x00018A70 File Offset: 0x00016C70
		// (remove) Token: 0x06000907 RID: 2311 RVA: 0x00018AA8 File Offset: 0x00016CA8
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000192 RID: 402
		// (add) Token: 0x06000908 RID: 2312 RVA: 0x00018AE0 File Offset: 0x00016CE0
		// (remove) Token: 0x06000909 RID: 2313 RVA: 0x00018B18 File Offset: 0x00016D18
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000193 RID: 403
		// (add) Token: 0x0600090A RID: 2314 RVA: 0x00018B50 File Offset: 0x00016D50
		// (remove) Token: 0x0600090B RID: 2315 RVA: 0x00018B88 File Offset: 0x00016D88
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000194 RID: 404
		// (add) Token: 0x0600090C RID: 2316 RVA: 0x00018BC0 File Offset: 0x00016DC0
		// (remove) Token: 0x0600090D RID: 2317 RVA: 0x00018BF8 File Offset: 0x00016DF8
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000195 RID: 405
		// (add) Token: 0x0600090E RID: 2318 RVA: 0x00018C30 File Offset: 0x00016E30
		// (remove) Token: 0x0600090F RID: 2319 RVA: 0x00018C68 File Offset: 0x00016E68
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000196 RID: 406
		// (add) Token: 0x06000910 RID: 2320 RVA: 0x00018CA0 File Offset: 0x00016EA0
		// (remove) Token: 0x06000911 RID: 2321 RVA: 0x00018CD8 File Offset: 0x00016ED8
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000197 RID: 407
		// (add) Token: 0x06000912 RID: 2322 RVA: 0x00018D10 File Offset: 0x00016F10
		// (remove) Token: 0x06000913 RID: 2323 RVA: 0x00018D48 File Offset: 0x00016F48
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000198 RID: 408
		// (add) Token: 0x06000914 RID: 2324 RVA: 0x00018D80 File Offset: 0x00016F80
		// (remove) Token: 0x06000915 RID: 2325 RVA: 0x00018DB8 File Offset: 0x00016FB8
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000199 RID: 409
		// (add) Token: 0x06000916 RID: 2326 RVA: 0x00018DF0 File Offset: 0x00016FF0
		// (remove) Token: 0x06000917 RID: 2327 RVA: 0x00018E28 File Offset: 0x00017028
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400019A RID: 410
		// (add) Token: 0x06000918 RID: 2328 RVA: 0x00018E60 File Offset: 0x00017060
		// (remove) Token: 0x06000919 RID: 2329 RVA: 0x00018E98 File Offset: 0x00017098
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400019B RID: 411
		// (add) Token: 0x0600091A RID: 2330 RVA: 0x00018ED0 File Offset: 0x000170D0
		// (remove) Token: 0x0600091B RID: 2331 RVA: 0x00018F08 File Offset: 0x00017108
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x1400019C RID: 412
		// (add) Token: 0x0600091C RID: 2332 RVA: 0x00018F40 File Offset: 0x00017140
		// (remove) Token: 0x0600091D RID: 2333 RVA: 0x00018F78 File Offset: 0x00017178
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x1400019D RID: 413
		// (add) Token: 0x0600091E RID: 2334 RVA: 0x00018FB0 File Offset: 0x000171B0
		// (remove) Token: 0x0600091F RID: 2335 RVA: 0x00018FE8 File Offset: 0x000171E8
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x1400019E RID: 414
		// (add) Token: 0x06000920 RID: 2336 RVA: 0x00019020 File Offset: 0x00017220
		// (remove) Token: 0x06000921 RID: 2337 RVA: 0x00019058 File Offset: 0x00017258
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x1400019F RID: 415
		// (add) Token: 0x06000922 RID: 2338 RVA: 0x00019090 File Offset: 0x00017290
		// (remove) Token: 0x06000923 RID: 2339 RVA: 0x000190C8 File Offset: 0x000172C8
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140001A0 RID: 416
		// (add) Token: 0x06000924 RID: 2340 RVA: 0x00019100 File Offset: 0x00017300
		// (remove) Token: 0x06000925 RID: 2341 RVA: 0x00019138 File Offset: 0x00017338
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140001A1 RID: 417
		// (add) Token: 0x06000926 RID: 2342 RVA: 0x00019170 File Offset: 0x00017370
		// (remove) Token: 0x06000927 RID: 2343 RVA: 0x000191A8 File Offset: 0x000173A8
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140001A2 RID: 418
		// (add) Token: 0x06000928 RID: 2344 RVA: 0x000191E0 File Offset: 0x000173E0
		// (remove) Token: 0x06000929 RID: 2345 RVA: 0x00019218 File Offset: 0x00017418
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140001A3 RID: 419
		// (add) Token: 0x0600092A RID: 2346 RVA: 0x00019250 File Offset: 0x00017450
		// (remove) Token: 0x0600092B RID: 2347 RVA: 0x00019288 File Offset: 0x00017488
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140001A4 RID: 420
		// (add) Token: 0x0600092C RID: 2348 RVA: 0x000192C0 File Offset: 0x000174C0
		// (remove) Token: 0x0600092D RID: 2349 RVA: 0x000192F8 File Offset: 0x000174F8
		public event EventHandler OnAutoReconnected;

		// Token: 0x0600092E RID: 2350 RVA: 0x0001932D File Offset: 0x0001752D
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001934E File Offset: 0x0001754E
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001936F File Offset: 0x0001756F
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00019391 File Offset: 0x00017591
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000193B4 File Offset: 0x000175B4
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000193D8 File Offset: 0x000175D8
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00019408 File Offset: 0x00017608
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00019438 File Offset: 0x00017638
		public virtual string GetErrorDescription(uint disconnectReason, uint extendedDisconnectReason)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetErrorDescription", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetErrorDescription(disconnectReason, extendedDisconnectReason);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00019468 File Offset: 0x00017668
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient5NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000194B8 File Offset: 0x000176B8
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

		// Token: 0x06000938 RID: 2360 RVA: 0x000194E8 File Offset: 0x000176E8
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

		// Token: 0x06000939 RID: 2361 RVA: 0x0001951C File Offset: 0x0001771C
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00019533 File Offset: 0x00017733
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001954A File Offset: 0x0001774A
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00019561 File Offset: 0x00017761
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00019578 File Offset: 0x00017778
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001958F File Offset: 0x0001778F
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000195A6 File Offset: 0x000177A6
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x000195BD File Offset: 0x000177BD
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000195D4 File Offset: 0x000177D4
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000195EB File Offset: 0x000177EB
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00019602 File Offset: 0x00017802
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00019619 File Offset: 0x00017819
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00019630 File Offset: 0x00017830
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00019647 File Offset: 0x00017847
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0001965E File Offset: 0x0001785E
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00019675 File Offset: 0x00017875
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001968C File Offset: 0x0001788C
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000196A3 File Offset: 0x000178A3
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000196BA File Offset: 0x000178BA
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000196D1 File Offset: 0x000178D1
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000196E8 File Offset: 0x000178E8
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000196FF File Offset: 0x000178FF
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00019716 File Offset: 0x00017916
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001972D File Offset: 0x0001792D
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00019744 File Offset: 0x00017944
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001975B File Offset: 0x0001795B
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00019772 File Offset: 0x00017972
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00019789 File Offset: 0x00017989
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000197A0 File Offset: 0x000179A0
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000197B7 File Offset: 0x000179B7
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040001D6 RID: 470
		private IMsRdpClient5 ocx;

		// Token: 0x040001D7 RID: 471
		private AxMsRdpClient5NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x040001D8 RID: 472
		private AxHost.ConnectionPointCookie cookie;
	}
}
