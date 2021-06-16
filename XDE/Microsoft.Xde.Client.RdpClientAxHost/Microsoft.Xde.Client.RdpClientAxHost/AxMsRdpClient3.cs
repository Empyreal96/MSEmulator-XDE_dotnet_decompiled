using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000034 RID: 52
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{7584c670-2274-4efb-b00b-d6aaba6d3850}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient3 : AxHost
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x0000F775 File Offset: 0x0000D975
		public AxMsRdpClient3() : base("7584c670-2274-4efb-b00b-d6aaba6d3850")
		{
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0000F782 File Offset: 0x0000D982
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0000F7A3 File Offset: 0x0000D9A3
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0000F7C5 File Offset: 0x0000D9C5
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0000F7E6 File Offset: 0x0000D9E6
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0000F808 File Offset: 0x0000DA08
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0000F829 File Offset: 0x0000DA29
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0000F84B File Offset: 0x0000DA4B
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0000F86C File Offset: 0x0000DA6C
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000F88E File Offset: 0x0000DA8E
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x0000F8AF File Offset: 0x0000DAAF
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000F8D1 File Offset: 0x0000DAD1
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000F8F2 File Offset: 0x0000DAF2
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0000F913 File Offset: 0x0000DB13
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000F935 File Offset: 0x0000DB35
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0000F956 File Offset: 0x0000DB56
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0000F978 File Offset: 0x0000DB78
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0000F999 File Offset: 0x0000DB99
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0000F9BB File Offset: 0x0000DBBB
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0000F9DC File Offset: 0x0000DBDC
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

		// Token: 0x170000C3 RID: 195
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x0000F9FD File Offset: 0x0000DBFD
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0000FA1F File Offset: 0x0000DC1F
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

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0000FA40 File Offset: 0x0000DC40
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0000FA61 File Offset: 0x0000DC61
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0000FA82 File Offset: 0x0000DC82
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x0000FAA3 File Offset: 0x0000DCA3
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0000FAC4 File Offset: 0x0000DCC4
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0000FAE5 File Offset: 0x0000DCE5
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x0000FB06 File Offset: 0x0000DD06
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000FB28 File Offset: 0x0000DD28
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0000FB49 File Offset: 0x0000DD49
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0000FB6A File Offset: 0x0000DD6A
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0000FB8B File Offset: 0x0000DD8B
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0000FBAC File Offset: 0x0000DDAC
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0000FBCE File Offset: 0x0000DDCE
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0000FBEF File Offset: 0x0000DDEF
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x0000FC10 File Offset: 0x0000DE10
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0000FC32 File Offset: 0x0000DE32
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(300)]
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

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x0600059D RID: 1437 RVA: 0x0000FC54 File Offset: 0x0000DE54
		// (remove) Token: 0x0600059E RID: 1438 RVA: 0x0000FC8C File Offset: 0x0000DE8C
		public event EventHandler OnConnecting;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x0600059F RID: 1439 RVA: 0x0000FCC4 File Offset: 0x0000DEC4
		// (remove) Token: 0x060005A0 RID: 1440 RVA: 0x0000FCFC File Offset: 0x0000DEFC
		public event EventHandler OnConnected;

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x060005A1 RID: 1441 RVA: 0x0000FD34 File Offset: 0x0000DF34
		// (remove) Token: 0x060005A2 RID: 1442 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		public event EventHandler OnLoginComplete;

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x060005A3 RID: 1443 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		// (remove) Token: 0x060005A4 RID: 1444 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x060005A5 RID: 1445 RVA: 0x0000FE14 File Offset: 0x0000E014
		// (remove) Token: 0x060005A6 RID: 1446 RVA: 0x0000FE4C File Offset: 0x0000E04C
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x060005A7 RID: 1447 RVA: 0x0000FE84 File Offset: 0x0000E084
		// (remove) Token: 0x060005A8 RID: 1448 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x060005A9 RID: 1449 RVA: 0x0000FEF4 File Offset: 0x0000E0F4
		// (remove) Token: 0x060005AA RID: 1450 RVA: 0x0000FF2C File Offset: 0x0000E12C
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x060005AB RID: 1451 RVA: 0x0000FF64 File Offset: 0x0000E164
		// (remove) Token: 0x060005AC RID: 1452 RVA: 0x0000FF9C File Offset: 0x0000E19C
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x060005AD RID: 1453 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		// (remove) Token: 0x060005AE RID: 1454 RVA: 0x0001000C File Offset: 0x0000E20C
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x060005AF RID: 1455 RVA: 0x00010044 File Offset: 0x0000E244
		// (remove) Token: 0x060005B0 RID: 1456 RVA: 0x0001007C File Offset: 0x0000E27C
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x060005B1 RID: 1457 RVA: 0x000100B4 File Offset: 0x0000E2B4
		// (remove) Token: 0x060005B2 RID: 1458 RVA: 0x000100EC File Offset: 0x0000E2EC
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x060005B3 RID: 1459 RVA: 0x00010124 File Offset: 0x0000E324
		// (remove) Token: 0x060005B4 RID: 1460 RVA: 0x0001015C File Offset: 0x0000E35C
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x060005B5 RID: 1461 RVA: 0x00010194 File Offset: 0x0000E394
		// (remove) Token: 0x060005B6 RID: 1462 RVA: 0x000101CC File Offset: 0x0000E3CC
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x060005B7 RID: 1463 RVA: 0x00010204 File Offset: 0x0000E404
		// (remove) Token: 0x060005B8 RID: 1464 RVA: 0x0001023C File Offset: 0x0000E43C
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x060005B9 RID: 1465 RVA: 0x00010274 File Offset: 0x0000E474
		// (remove) Token: 0x060005BA RID: 1466 RVA: 0x000102AC File Offset: 0x0000E4AC
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x060005BB RID: 1467 RVA: 0x000102E4 File Offset: 0x0000E4E4
		// (remove) Token: 0x060005BC RID: 1468 RVA: 0x0001031C File Offset: 0x0000E51C
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x060005BD RID: 1469 RVA: 0x00010354 File Offset: 0x0000E554
		// (remove) Token: 0x060005BE RID: 1470 RVA: 0x0001038C File Offset: 0x0000E58C
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x060005BF RID: 1471 RVA: 0x000103C4 File Offset: 0x0000E5C4
		// (remove) Token: 0x060005C0 RID: 1472 RVA: 0x000103FC File Offset: 0x0000E5FC
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000103 RID: 259
		// (add) Token: 0x060005C1 RID: 1473 RVA: 0x00010434 File Offset: 0x0000E634
		// (remove) Token: 0x060005C2 RID: 1474 RVA: 0x0001046C File Offset: 0x0000E66C
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000104 RID: 260
		// (add) Token: 0x060005C3 RID: 1475 RVA: 0x000104A4 File Offset: 0x0000E6A4
		// (remove) Token: 0x060005C4 RID: 1476 RVA: 0x000104DC File Offset: 0x0000E6DC
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x060005C5 RID: 1477 RVA: 0x00010514 File Offset: 0x0000E714
		// (remove) Token: 0x060005C6 RID: 1478 RVA: 0x0001054C File Offset: 0x0000E74C
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000106 RID: 262
		// (add) Token: 0x060005C7 RID: 1479 RVA: 0x00010584 File Offset: 0x0000E784
		// (remove) Token: 0x060005C8 RID: 1480 RVA: 0x000105BC File Offset: 0x0000E7BC
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000107 RID: 263
		// (add) Token: 0x060005C9 RID: 1481 RVA: 0x000105F4 File Offset: 0x0000E7F4
		// (remove) Token: 0x060005CA RID: 1482 RVA: 0x0001062C File Offset: 0x0000E82C
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x060005CB RID: 1483 RVA: 0x00010664 File Offset: 0x0000E864
		// (remove) Token: 0x060005CC RID: 1484 RVA: 0x0001069C File Offset: 0x0000E89C
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x060005CD RID: 1485 RVA: 0x000106D4 File Offset: 0x0000E8D4
		// (remove) Token: 0x060005CE RID: 1486 RVA: 0x0001070C File Offset: 0x0000E90C
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x1400010A RID: 266
		// (add) Token: 0x060005CF RID: 1487 RVA: 0x00010744 File Offset: 0x0000E944
		// (remove) Token: 0x060005D0 RID: 1488 RVA: 0x0001077C File Offset: 0x0000E97C
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x060005D1 RID: 1489 RVA: 0x000107B4 File Offset: 0x0000E9B4
		// (remove) Token: 0x060005D2 RID: 1490 RVA: 0x000107EC File Offset: 0x0000E9EC
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x060005D3 RID: 1491 RVA: 0x00010824 File Offset: 0x0000EA24
		// (remove) Token: 0x060005D4 RID: 1492 RVA: 0x0001085C File Offset: 0x0000EA5C
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x060005D5 RID: 1493 RVA: 0x00010894 File Offset: 0x0000EA94
		// (remove) Token: 0x060005D6 RID: 1494 RVA: 0x000108CC File Offset: 0x0000EACC
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x060005D7 RID: 1495 RVA: 0x00010904 File Offset: 0x0000EB04
		// (remove) Token: 0x060005D8 RID: 1496 RVA: 0x0001093C File Offset: 0x0000EB3C
		public event EventHandler OnAutoReconnected;

		// Token: 0x060005D9 RID: 1497 RVA: 0x00010971 File Offset: 0x0000EB71
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00010992 File Offset: 0x0000EB92
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000109B3 File Offset: 0x0000EBB3
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000109D5 File Offset: 0x0000EBD5
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000109F8 File Offset: 0x0000EBF8
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00010A1C File Offset: 0x0000EC1C
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00010A4C File Offset: 0x0000EC4C
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00010A7C File Offset: 0x0000EC7C
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient3EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00010ACC File Offset: 0x0000ECCC
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

		// Token: 0x060005E2 RID: 1506 RVA: 0x00010AFC File Offset: 0x0000ECFC
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

		// Token: 0x060005E3 RID: 1507 RVA: 0x00010B30 File Offset: 0x0000ED30
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00010B47 File Offset: 0x0000ED47
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00010B5E File Offset: 0x0000ED5E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00010B75 File Offset: 0x0000ED75
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00010B8C File Offset: 0x0000ED8C
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00010BA3 File Offset: 0x0000EDA3
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00010BBA File Offset: 0x0000EDBA
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00010BD1 File Offset: 0x0000EDD1
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00010BE8 File Offset: 0x0000EDE8
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00010BFF File Offset: 0x0000EDFF
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00010C16 File Offset: 0x0000EE16
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00010C2D File Offset: 0x0000EE2D
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00010C44 File Offset: 0x0000EE44
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00010C5B File Offset: 0x0000EE5B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00010C72 File Offset: 0x0000EE72
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00010C89 File Offset: 0x0000EE89
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00010CA0 File Offset: 0x0000EEA0
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00010CB7 File Offset: 0x0000EEB7
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00010CCE File Offset: 0x0000EECE
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00010CE5 File Offset: 0x0000EEE5
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00010CFC File Offset: 0x0000EEFC
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00010D13 File Offset: 0x0000EF13
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00010D2A File Offset: 0x0000EF2A
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00010D41 File Offset: 0x0000EF41
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00010D58 File Offset: 0x0000EF58
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00010D6F File Offset: 0x0000EF6F
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00010D86 File Offset: 0x0000EF86
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00010D9D File Offset: 0x0000EF9D
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00010DB4 File Offset: 0x0000EFB4
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00010DCB File Offset: 0x0000EFCB
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400012C RID: 300
		private IMsRdpClient3 ocx;

		// Token: 0x0400012D RID: 301
		private AxMsRdpClient3EventMulticaster eventMulticaster;

		// Token: 0x0400012E RID: 302
		private AxHost.ConnectionPointCookie cookie;
	}
}
