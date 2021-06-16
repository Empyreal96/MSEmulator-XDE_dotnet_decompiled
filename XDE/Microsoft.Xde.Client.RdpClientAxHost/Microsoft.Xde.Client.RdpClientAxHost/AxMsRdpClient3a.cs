using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000036 RID: 54
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{6a6f4b83-45c5-4ca9-bdd9-0d81c12295e4}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient3a : AxHost
	{
		// Token: 0x06000620 RID: 1568 RVA: 0x000112CD File Offset: 0x0000F4CD
		public AxMsRdpClient3a() : base("6a6f4b83-45c5-4ca9-bdd9-0d81c12295e4")
		{
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x000112DA File Offset: 0x0000F4DA
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x000112FB File Offset: 0x0000F4FB
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0001131D File Offset: 0x0000F51D
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x0001133E File Offset: 0x0000F53E
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00011360 File Offset: 0x0000F560
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00011381 File Offset: 0x0000F581
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x000113A3 File Offset: 0x0000F5A3
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x000113C4 File Offset: 0x0000F5C4
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000113E6 File Offset: 0x0000F5E6
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x00011407 File Offset: 0x0000F607
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00011429 File Offset: 0x0000F629
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001144A File Offset: 0x0000F64A
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001146B File Offset: 0x0000F66B
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

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001148D File Offset: 0x0000F68D
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x000114AE File Offset: 0x0000F6AE
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

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000114D0 File Offset: 0x0000F6D0
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x000114F1 File Offset: 0x0000F6F1
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00011513 File Offset: 0x0000F713
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00011534 File Offset: 0x0000F734
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

		// Token: 0x170000DD RID: 221
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x00011555 File Offset: 0x0000F755
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00011577 File Offset: 0x0000F777
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00011598 File Offset: 0x0000F798
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x000115B9 File Offset: 0x0000F7B9
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x000115DA File Offset: 0x0000F7DA
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000115FB File Offset: 0x0000F7FB
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001161C File Offset: 0x0000F81C
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0001163D File Offset: 0x0000F83D
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0001165E File Offset: 0x0000F85E
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00011680 File Offset: 0x0000F880
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x000116A1 File Offset: 0x0000F8A1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(102)]
		[Browsable(false)]
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x000116C2 File Offset: 0x0000F8C2
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x000116E3 File Offset: 0x0000F8E3
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00011704 File Offset: 0x0000F904
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00011726 File Offset: 0x0000F926
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00011747 File Offset: 0x0000F947
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x00011768 File Offset: 0x0000F968
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001178A File Offset: 0x0000F98A
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

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x06000646 RID: 1606 RVA: 0x000117AC File Offset: 0x0000F9AC
		// (remove) Token: 0x06000647 RID: 1607 RVA: 0x000117E4 File Offset: 0x0000F9E4
		public event EventHandler OnConnecting;

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x06000648 RID: 1608 RVA: 0x0001181C File Offset: 0x0000FA1C
		// (remove) Token: 0x06000649 RID: 1609 RVA: 0x00011854 File Offset: 0x0000FA54
		public event EventHandler OnConnected;

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x0600064A RID: 1610 RVA: 0x0001188C File Offset: 0x0000FA8C
		// (remove) Token: 0x0600064B RID: 1611 RVA: 0x000118C4 File Offset: 0x0000FAC4
		public event EventHandler OnLoginComplete;

		// Token: 0x14000112 RID: 274
		// (add) Token: 0x0600064C RID: 1612 RVA: 0x000118FC File Offset: 0x0000FAFC
		// (remove) Token: 0x0600064D RID: 1613 RVA: 0x00011934 File Offset: 0x0000FB34
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x0600064E RID: 1614 RVA: 0x0001196C File Offset: 0x0000FB6C
		// (remove) Token: 0x0600064F RID: 1615 RVA: 0x000119A4 File Offset: 0x0000FBA4
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x06000650 RID: 1616 RVA: 0x000119DC File Offset: 0x0000FBDC
		// (remove) Token: 0x06000651 RID: 1617 RVA: 0x00011A14 File Offset: 0x0000FC14
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x06000652 RID: 1618 RVA: 0x00011A4C File Offset: 0x0000FC4C
		// (remove) Token: 0x06000653 RID: 1619 RVA: 0x00011A84 File Offset: 0x0000FC84
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x06000654 RID: 1620 RVA: 0x00011ABC File Offset: 0x0000FCBC
		// (remove) Token: 0x06000655 RID: 1621 RVA: 0x00011AF4 File Offset: 0x0000FCF4
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x06000656 RID: 1622 RVA: 0x00011B2C File Offset: 0x0000FD2C
		// (remove) Token: 0x06000657 RID: 1623 RVA: 0x00011B64 File Offset: 0x0000FD64
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000118 RID: 280
		// (add) Token: 0x06000658 RID: 1624 RVA: 0x00011B9C File Offset: 0x0000FD9C
		// (remove) Token: 0x06000659 RID: 1625 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000119 RID: 281
		// (add) Token: 0x0600065A RID: 1626 RVA: 0x00011C0C File Offset: 0x0000FE0C
		// (remove) Token: 0x0600065B RID: 1627 RVA: 0x00011C44 File Offset: 0x0000FE44
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x1400011A RID: 282
		// (add) Token: 0x0600065C RID: 1628 RVA: 0x00011C7C File Offset: 0x0000FE7C
		// (remove) Token: 0x0600065D RID: 1629 RVA: 0x00011CB4 File Offset: 0x0000FEB4
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x1400011B RID: 283
		// (add) Token: 0x0600065E RID: 1630 RVA: 0x00011CEC File Offset: 0x0000FEEC
		// (remove) Token: 0x0600065F RID: 1631 RVA: 0x00011D24 File Offset: 0x0000FF24
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x06000660 RID: 1632 RVA: 0x00011D5C File Offset: 0x0000FF5C
		// (remove) Token: 0x06000661 RID: 1633 RVA: 0x00011D94 File Offset: 0x0000FF94
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400011D RID: 285
		// (add) Token: 0x06000662 RID: 1634 RVA: 0x00011DCC File Offset: 0x0000FFCC
		// (remove) Token: 0x06000663 RID: 1635 RVA: 0x00011E04 File Offset: 0x00010004
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x06000664 RID: 1636 RVA: 0x00011E3C File Offset: 0x0001003C
		// (remove) Token: 0x06000665 RID: 1637 RVA: 0x00011E74 File Offset: 0x00010074
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x06000666 RID: 1638 RVA: 0x00011EAC File Offset: 0x000100AC
		// (remove) Token: 0x06000667 RID: 1639 RVA: 0x00011EE4 File Offset: 0x000100E4
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x06000668 RID: 1640 RVA: 0x00011F1C File Offset: 0x0001011C
		// (remove) Token: 0x06000669 RID: 1641 RVA: 0x00011F54 File Offset: 0x00010154
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600066A RID: 1642 RVA: 0x00011F8C File Offset: 0x0001018C
		// (remove) Token: 0x0600066B RID: 1643 RVA: 0x00011FC4 File Offset: 0x000101C4
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000122 RID: 290
		// (add) Token: 0x0600066C RID: 1644 RVA: 0x00011FFC File Offset: 0x000101FC
		// (remove) Token: 0x0600066D RID: 1645 RVA: 0x00012034 File Offset: 0x00010234
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x0600066E RID: 1646 RVA: 0x0001206C File Offset: 0x0001026C
		// (remove) Token: 0x0600066F RID: 1647 RVA: 0x000120A4 File Offset: 0x000102A4
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x06000670 RID: 1648 RVA: 0x000120DC File Offset: 0x000102DC
		// (remove) Token: 0x06000671 RID: 1649 RVA: 0x00012114 File Offset: 0x00010314
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x06000672 RID: 1650 RVA: 0x0001214C File Offset: 0x0001034C
		// (remove) Token: 0x06000673 RID: 1651 RVA: 0x00012184 File Offset: 0x00010384
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x06000674 RID: 1652 RVA: 0x000121BC File Offset: 0x000103BC
		// (remove) Token: 0x06000675 RID: 1653 RVA: 0x000121F4 File Offset: 0x000103F4
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x06000676 RID: 1654 RVA: 0x0001222C File Offset: 0x0001042C
		// (remove) Token: 0x06000677 RID: 1655 RVA: 0x00012264 File Offset: 0x00010464
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x06000678 RID: 1656 RVA: 0x0001229C File Offset: 0x0001049C
		// (remove) Token: 0x06000679 RID: 1657 RVA: 0x000122D4 File Offset: 0x000104D4
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000129 RID: 297
		// (add) Token: 0x0600067A RID: 1658 RVA: 0x0001230C File Offset: 0x0001050C
		// (remove) Token: 0x0600067B RID: 1659 RVA: 0x00012344 File Offset: 0x00010544
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x0600067C RID: 1660 RVA: 0x0001237C File Offset: 0x0001057C
		// (remove) Token: 0x0600067D RID: 1661 RVA: 0x000123B4 File Offset: 0x000105B4
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x0600067E RID: 1662 RVA: 0x000123EC File Offset: 0x000105EC
		// (remove) Token: 0x0600067F RID: 1663 RVA: 0x00012424 File Offset: 0x00010624
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400012C RID: 300
		// (add) Token: 0x06000680 RID: 1664 RVA: 0x0001245C File Offset: 0x0001065C
		// (remove) Token: 0x06000681 RID: 1665 RVA: 0x00012494 File Offset: 0x00010694
		public event EventHandler OnAutoReconnected;

		// Token: 0x06000682 RID: 1666 RVA: 0x000124C9 File Offset: 0x000106C9
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000124EA File Offset: 0x000106EA
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001250B File Offset: 0x0001070B
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001252D File Offset: 0x0001072D
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00012550 File Offset: 0x00010750
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00012574 File Offset: 0x00010774
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000125A4 File Offset: 0x000107A4
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000125D4 File Offset: 0x000107D4
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient3aEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00012624 File Offset: 0x00010824
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

		// Token: 0x0600068B RID: 1675 RVA: 0x00012654 File Offset: 0x00010854
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

		// Token: 0x0600068C RID: 1676 RVA: 0x00012688 File Offset: 0x00010888
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001269F File Offset: 0x0001089F
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000126B6 File Offset: 0x000108B6
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000126CD File Offset: 0x000108CD
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000126E4 File Offset: 0x000108E4
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000126FB File Offset: 0x000108FB
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00012712 File Offset: 0x00010912
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00012729 File Offset: 0x00010929
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00012740 File Offset: 0x00010940
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00012757 File Offset: 0x00010957
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001276E File Offset: 0x0001096E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00012785 File Offset: 0x00010985
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001279C File Offset: 0x0001099C
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000127B3 File Offset: 0x000109B3
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000127CA File Offset: 0x000109CA
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000127E1 File Offset: 0x000109E1
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000127F8 File Offset: 0x000109F8
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001280F File Offset: 0x00010A0F
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00012826 File Offset: 0x00010A26
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001283D File Offset: 0x00010A3D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00012854 File Offset: 0x00010A54
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001286B File Offset: 0x00010A6B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00012882 File Offset: 0x00010A82
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00012899 File Offset: 0x00010A99
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000128B0 File Offset: 0x00010AB0
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000128C7 File Offset: 0x00010AC7
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000128DE File Offset: 0x00010ADE
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000128F5 File Offset: 0x00010AF5
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001290C File Offset: 0x00010B0C
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00012923 File Offset: 0x00010B23
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x0400014E RID: 334
		private IMsRdpClient3 ocx;

		// Token: 0x0400014F RID: 335
		private AxMsRdpClient3aEventMulticaster eventMulticaster;

		// Token: 0x04000150 RID: 336
		private AxHost.ConnectionPointCookie cookie;
	}
}
