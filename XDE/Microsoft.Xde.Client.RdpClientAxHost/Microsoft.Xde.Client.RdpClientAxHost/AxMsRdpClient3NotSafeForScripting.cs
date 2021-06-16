using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000032 RID: 50
	[AxHost.ClsidAttribute("{ace575fd-1fcf-4074-9401-ebab990fa9de}")]
	[DesignTimeVisible(true)]
	[DefaultEvent("OnConnecting")]
	public class AxMsRdpClient3NotSafeForScripting : AxHost
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x0000DC1D File Offset: 0x0000BE1D
		public AxMsRdpClient3NotSafeForScripting() : base("ace575fd-1fcf-4074-9401-ebab990fa9de")
		{
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0000DC2A File Offset: 0x0000BE2A
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x0000DC4B File Offset: 0x0000BE4B
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000DC6D File Offset: 0x0000BE6D
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0000DC8E File Offset: 0x0000BE8E
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0000DCD1 File Offset: 0x0000BED1
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000DCF3 File Offset: 0x0000BEF3
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0000DD14 File Offset: 0x0000BF14
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000DD36 File Offset: 0x0000BF36
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0000DD57 File Offset: 0x0000BF57
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000DD79 File Offset: 0x0000BF79
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0000DD9A File Offset: 0x0000BF9A
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x0000DDBB File Offset: 0x0000BFBB
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0000DDDD File Offset: 0x0000BFDD
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x0000DDFE File Offset: 0x0000BFFE
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0000DE20 File Offset: 0x0000C020
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x0000DE41 File Offset: 0x0000C041
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0000DE63 File Offset: 0x0000C063
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0000DE84 File Offset: 0x0000C084
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

		// Token: 0x170000A9 RID: 169
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0000DEA5 File Offset: 0x0000C0A5
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0000DEC7 File Offset: 0x0000C0C7
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0000DEE8 File Offset: 0x0000C0E8
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000DF09 File Offset: 0x0000C109
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0000DF2A File Offset: 0x0000C12A
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000DF4B File Offset: 0x0000C14B
		[DispId(98)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0000DF6C File Offset: 0x0000C16C
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000DF8D File Offset: 0x0000C18D
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0000DFAE File Offset: 0x0000C1AE
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000DFF1 File Offset: 0x0000C1F1
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000E012 File Offset: 0x0000C212
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0000E033 File Offset: 0x0000C233
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x0000E054 File Offset: 0x0000C254
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0000E076 File Offset: 0x0000C276
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000E097 File Offset: 0x0000C297
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0000E0DA File Offset: 0x0000C2DA
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x060004F4 RID: 1268 RVA: 0x0000E0FC File Offset: 0x0000C2FC
		// (remove) Token: 0x060004F5 RID: 1269 RVA: 0x0000E134 File Offset: 0x0000C334
		public event EventHandler OnConnecting;

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x060004F6 RID: 1270 RVA: 0x0000E16C File Offset: 0x0000C36C
		// (remove) Token: 0x060004F7 RID: 1271 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		public event EventHandler OnConnected;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x060004F8 RID: 1272 RVA: 0x0000E1DC File Offset: 0x0000C3DC
		// (remove) Token: 0x060004F9 RID: 1273 RVA: 0x0000E214 File Offset: 0x0000C414
		public event EventHandler OnLoginComplete;

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x060004FA RID: 1274 RVA: 0x0000E24C File Offset: 0x0000C44C
		// (remove) Token: 0x060004FB RID: 1275 RVA: 0x0000E284 File Offset: 0x0000C484
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x060004FC RID: 1276 RVA: 0x0000E2BC File Offset: 0x0000C4BC
		// (remove) Token: 0x060004FD RID: 1277 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x060004FE RID: 1278 RVA: 0x0000E32C File Offset: 0x0000C52C
		// (remove) Token: 0x060004FF RID: 1279 RVA: 0x0000E364 File Offset: 0x0000C564
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06000500 RID: 1280 RVA: 0x0000E39C File Offset: 0x0000C59C
		// (remove) Token: 0x06000501 RID: 1281 RVA: 0x0000E3D4 File Offset: 0x0000C5D4
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06000502 RID: 1282 RVA: 0x0000E40C File Offset: 0x0000C60C
		// (remove) Token: 0x06000503 RID: 1283 RVA: 0x0000E444 File Offset: 0x0000C644
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06000504 RID: 1284 RVA: 0x0000E47C File Offset: 0x0000C67C
		// (remove) Token: 0x06000505 RID: 1285 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06000506 RID: 1286 RVA: 0x0000E4EC File Offset: 0x0000C6EC
		// (remove) Token: 0x06000507 RID: 1287 RVA: 0x0000E524 File Offset: 0x0000C724
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06000508 RID: 1288 RVA: 0x0000E55C File Offset: 0x0000C75C
		// (remove) Token: 0x06000509 RID: 1289 RVA: 0x0000E594 File Offset: 0x0000C794
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x0600050A RID: 1290 RVA: 0x0000E5CC File Offset: 0x0000C7CC
		// (remove) Token: 0x0600050B RID: 1291 RVA: 0x0000E604 File Offset: 0x0000C804
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x0600050C RID: 1292 RVA: 0x0000E63C File Offset: 0x0000C83C
		// (remove) Token: 0x0600050D RID: 1293 RVA: 0x0000E674 File Offset: 0x0000C874
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x0600050E RID: 1294 RVA: 0x0000E6AC File Offset: 0x0000C8AC
		// (remove) Token: 0x0600050F RID: 1295 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06000510 RID: 1296 RVA: 0x0000E71C File Offset: 0x0000C91C
		// (remove) Token: 0x06000511 RID: 1297 RVA: 0x0000E754 File Offset: 0x0000C954
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06000512 RID: 1298 RVA: 0x0000E78C File Offset: 0x0000C98C
		// (remove) Token: 0x06000513 RID: 1299 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06000514 RID: 1300 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		// (remove) Token: 0x06000515 RID: 1301 RVA: 0x0000E834 File Offset: 0x0000CA34
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06000516 RID: 1302 RVA: 0x0000E86C File Offset: 0x0000CA6C
		// (remove) Token: 0x06000517 RID: 1303 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06000518 RID: 1304 RVA: 0x0000E8DC File Offset: 0x0000CADC
		// (remove) Token: 0x06000519 RID: 1305 RVA: 0x0000E914 File Offset: 0x0000CB14
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x0600051A RID: 1306 RVA: 0x0000E94C File Offset: 0x0000CB4C
		// (remove) Token: 0x0600051B RID: 1307 RVA: 0x0000E984 File Offset: 0x0000CB84
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x0600051C RID: 1308 RVA: 0x0000E9BC File Offset: 0x0000CBBC
		// (remove) Token: 0x0600051D RID: 1309 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x0600051E RID: 1310 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		// (remove) Token: 0x0600051F RID: 1311 RVA: 0x0000EA64 File Offset: 0x0000CC64
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x06000520 RID: 1312 RVA: 0x0000EA9C File Offset: 0x0000CC9C
		// (remove) Token: 0x06000521 RID: 1313 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06000522 RID: 1314 RVA: 0x0000EB0C File Offset: 0x0000CD0C
		// (remove) Token: 0x06000523 RID: 1315 RVA: 0x0000EB44 File Offset: 0x0000CD44
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06000524 RID: 1316 RVA: 0x0000EB7C File Offset: 0x0000CD7C
		// (remove) Token: 0x06000525 RID: 1317 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06000526 RID: 1318 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		// (remove) Token: 0x06000527 RID: 1319 RVA: 0x0000EC24 File Offset: 0x0000CE24
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06000528 RID: 1320 RVA: 0x0000EC5C File Offset: 0x0000CE5C
		// (remove) Token: 0x06000529 RID: 1321 RVA: 0x0000EC94 File Offset: 0x0000CE94
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x0600052A RID: 1322 RVA: 0x0000ECCC File Offset: 0x0000CECC
		// (remove) Token: 0x0600052B RID: 1323 RVA: 0x0000ED04 File Offset: 0x0000CF04
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x0600052C RID: 1324 RVA: 0x0000ED3C File Offset: 0x0000CF3C
		// (remove) Token: 0x0600052D RID: 1325 RVA: 0x0000ED74 File Offset: 0x0000CF74
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x0600052E RID: 1326 RVA: 0x0000EDAC File Offset: 0x0000CFAC
		// (remove) Token: 0x0600052F RID: 1327 RVA: 0x0000EDE4 File Offset: 0x0000CFE4
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000530 RID: 1328 RVA: 0x0000EE19 File Offset: 0x0000D019
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000EE3A File Offset: 0x0000D03A
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000EE5B File Offset: 0x0000D05B
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000EE7D File Offset: 0x0000D07D
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000EF24 File Offset: 0x0000D124
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient3NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000EF74 File Offset: 0x0000D174
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

		// Token: 0x06000539 RID: 1337 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient3)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000EFEF File Offset: 0x0000D1EF
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000F006 File Offset: 0x0000D206
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000F01D File Offset: 0x0000D21D
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000F034 File Offset: 0x0000D234
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000F04B File Offset: 0x0000D24B
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000F062 File Offset: 0x0000D262
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000F079 File Offset: 0x0000D279
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000F090 File Offset: 0x0000D290
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000F0A7 File Offset: 0x0000D2A7
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000F0BE File Offset: 0x0000D2BE
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000F0D5 File Offset: 0x0000D2D5
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000F0EC File Offset: 0x0000D2EC
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000F103 File Offset: 0x0000D303
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000F11A File Offset: 0x0000D31A
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000F131 File Offset: 0x0000D331
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000F148 File Offset: 0x0000D348
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000F15F File Offset: 0x0000D35F
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000F176 File Offset: 0x0000D376
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000F18D File Offset: 0x0000D38D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000F1A4 File Offset: 0x0000D3A4
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000F1BB File Offset: 0x0000D3BB
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000F1D2 File Offset: 0x0000D3D2
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0000F1E9 File Offset: 0x0000D3E9
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0000F200 File Offset: 0x0000D400
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0000F217 File Offset: 0x0000D417
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000F22E File Offset: 0x0000D42E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000F245 File Offset: 0x0000D445
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000F25C File Offset: 0x0000D45C
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000F273 File Offset: 0x0000D473
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400010A RID: 266
		private IMsRdpClient3 ocx;

		// Token: 0x0400010B RID: 267
		private AxMsRdpClient3NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x0400010C RID: 268
		private AxHost.ConnectionPointCookie cookie;
	}
}
