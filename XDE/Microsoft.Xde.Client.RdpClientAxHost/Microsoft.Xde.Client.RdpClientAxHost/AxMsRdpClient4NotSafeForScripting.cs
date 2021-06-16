using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000038 RID: 56
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{6ae29350-321b-42be-bbe5-12fb5270c0de}")]
	[DesignTimeVisible(true)]
	public class AxMsRdpClient4NotSafeForScripting : AxHost
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x00012E25 File Offset: 0x00011025
		public AxMsRdpClient4NotSafeForScripting() : base("6ae29350-321b-42be-bbe5-12fb5270c0de")
		{
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00012E32 File Offset: 0x00011032
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x00012E53 File Offset: 0x00011053
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00012E75 File Offset: 0x00011075
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x00012E96 File Offset: 0x00011096
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00012EB8 File Offset: 0x000110B8
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x00012ED9 File Offset: 0x000110D9
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00012EFB File Offset: 0x000110FB
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x00012F1C File Offset: 0x0001111C
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00012F3E File Offset: 0x0001113E
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x00012F5F File Offset: 0x0001115F
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00012F81 File Offset: 0x00011181
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00012FA2 File Offset: 0x000111A2
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00012FC3 File Offset: 0x000111C3
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00012FE5 File Offset: 0x000111E5
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00013006 File Offset: 0x00011206
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

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00013028 File Offset: 0x00011228
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00013049 File Offset: 0x00011249
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001306B File Offset: 0x0001126B
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

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001308C File Offset: 0x0001128C
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

		// Token: 0x170000F7 RID: 247
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x000130AD File Offset: 0x000112AD
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

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000130CF File Offset: 0x000112CF
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

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x000130F0 File Offset: 0x000112F0
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

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00013111 File Offset: 0x00011311
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00013132 File Offset: 0x00011332
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00013153 File Offset: 0x00011353
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00013174 File Offset: 0x00011374
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00013195 File Offset: 0x00011395
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x000131B6 File Offset: 0x000113B6
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x000131D8 File Offset: 0x000113D8
		[DispId(101)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000131F9 File Offset: 0x000113F9
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001321A File Offset: 0x0001141A
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

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001323B File Offset: 0x0001143B
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0001325C File Offset: 0x0001145C
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

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001327E File Offset: 0x0001147E
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

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001329F File Offset: 0x0001149F
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x000132C0 File Offset: 0x000114C0
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

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000132E2 File Offset: 0x000114E2
		[Browsable(false)]
		[DispId(300)]
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00013303 File Offset: 0x00011503
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

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x060006F0 RID: 1776 RVA: 0x00013324 File Offset: 0x00011524
		// (remove) Token: 0x060006F1 RID: 1777 RVA: 0x0001335C File Offset: 0x0001155C
		public event EventHandler OnConnecting;

		// Token: 0x1400012E RID: 302
		// (add) Token: 0x060006F2 RID: 1778 RVA: 0x00013394 File Offset: 0x00011594
		// (remove) Token: 0x060006F3 RID: 1779 RVA: 0x000133CC File Offset: 0x000115CC
		public event EventHandler OnConnected;

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x060006F4 RID: 1780 RVA: 0x00013404 File Offset: 0x00011604
		// (remove) Token: 0x060006F5 RID: 1781 RVA: 0x0001343C File Offset: 0x0001163C
		public event EventHandler OnLoginComplete;

		// Token: 0x14000130 RID: 304
		// (add) Token: 0x060006F6 RID: 1782 RVA: 0x00013474 File Offset: 0x00011674
		// (remove) Token: 0x060006F7 RID: 1783 RVA: 0x000134AC File Offset: 0x000116AC
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000131 RID: 305
		// (add) Token: 0x060006F8 RID: 1784 RVA: 0x000134E4 File Offset: 0x000116E4
		// (remove) Token: 0x060006F9 RID: 1785 RVA: 0x0001351C File Offset: 0x0001171C
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000132 RID: 306
		// (add) Token: 0x060006FA RID: 1786 RVA: 0x00013554 File Offset: 0x00011754
		// (remove) Token: 0x060006FB RID: 1787 RVA: 0x0001358C File Offset: 0x0001178C
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x060006FC RID: 1788 RVA: 0x000135C4 File Offset: 0x000117C4
		// (remove) Token: 0x060006FD RID: 1789 RVA: 0x000135FC File Offset: 0x000117FC
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x060006FE RID: 1790 RVA: 0x00013634 File Offset: 0x00011834
		// (remove) Token: 0x060006FF RID: 1791 RVA: 0x0001366C File Offset: 0x0001186C
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000135 RID: 309
		// (add) Token: 0x06000700 RID: 1792 RVA: 0x000136A4 File Offset: 0x000118A4
		// (remove) Token: 0x06000701 RID: 1793 RVA: 0x000136DC File Offset: 0x000118DC
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x06000702 RID: 1794 RVA: 0x00013714 File Offset: 0x00011914
		// (remove) Token: 0x06000703 RID: 1795 RVA: 0x0001374C File Offset: 0x0001194C
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x06000704 RID: 1796 RVA: 0x00013784 File Offset: 0x00011984
		// (remove) Token: 0x06000705 RID: 1797 RVA: 0x000137BC File Offset: 0x000119BC
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x06000706 RID: 1798 RVA: 0x000137F4 File Offset: 0x000119F4
		// (remove) Token: 0x06000707 RID: 1799 RVA: 0x0001382C File Offset: 0x00011A2C
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x06000708 RID: 1800 RVA: 0x00013864 File Offset: 0x00011A64
		// (remove) Token: 0x06000709 RID: 1801 RVA: 0x0001389C File Offset: 0x00011A9C
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x0600070A RID: 1802 RVA: 0x000138D4 File Offset: 0x00011AD4
		// (remove) Token: 0x0600070B RID: 1803 RVA: 0x0001390C File Offset: 0x00011B0C
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x0600070C RID: 1804 RVA: 0x00013944 File Offset: 0x00011B44
		// (remove) Token: 0x0600070D RID: 1805 RVA: 0x0001397C File Offset: 0x00011B7C
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x0600070E RID: 1806 RVA: 0x000139B4 File Offset: 0x00011BB4
		// (remove) Token: 0x0600070F RID: 1807 RVA: 0x000139EC File Offset: 0x00011BEC
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x06000710 RID: 1808 RVA: 0x00013A24 File Offset: 0x00011C24
		// (remove) Token: 0x06000711 RID: 1809 RVA: 0x00013A5C File Offset: 0x00011C5C
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x06000712 RID: 1810 RVA: 0x00013A94 File Offset: 0x00011C94
		// (remove) Token: 0x06000713 RID: 1811 RVA: 0x00013ACC File Offset: 0x00011CCC
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x06000714 RID: 1812 RVA: 0x00013B04 File Offset: 0x00011D04
		// (remove) Token: 0x06000715 RID: 1813 RVA: 0x00013B3C File Offset: 0x00011D3C
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x06000716 RID: 1814 RVA: 0x00013B74 File Offset: 0x00011D74
		// (remove) Token: 0x06000717 RID: 1815 RVA: 0x00013BAC File Offset: 0x00011DAC
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x06000718 RID: 1816 RVA: 0x00013BE4 File Offset: 0x00011DE4
		// (remove) Token: 0x06000719 RID: 1817 RVA: 0x00013C1C File Offset: 0x00011E1C
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x0600071A RID: 1818 RVA: 0x00013C54 File Offset: 0x00011E54
		// (remove) Token: 0x0600071B RID: 1819 RVA: 0x00013C8C File Offset: 0x00011E8C
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x0600071C RID: 1820 RVA: 0x00013CC4 File Offset: 0x00011EC4
		// (remove) Token: 0x0600071D RID: 1821 RVA: 0x00013CFC File Offset: 0x00011EFC
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x0600071E RID: 1822 RVA: 0x00013D34 File Offset: 0x00011F34
		// (remove) Token: 0x0600071F RID: 1823 RVA: 0x00013D6C File Offset: 0x00011F6C
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x06000720 RID: 1824 RVA: 0x00013DA4 File Offset: 0x00011FA4
		// (remove) Token: 0x06000721 RID: 1825 RVA: 0x00013DDC File Offset: 0x00011FDC
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x06000722 RID: 1826 RVA: 0x00013E14 File Offset: 0x00012014
		// (remove) Token: 0x06000723 RID: 1827 RVA: 0x00013E4C File Offset: 0x0001204C
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000147 RID: 327
		// (add) Token: 0x06000724 RID: 1828 RVA: 0x00013E84 File Offset: 0x00012084
		// (remove) Token: 0x06000725 RID: 1829 RVA: 0x00013EBC File Offset: 0x000120BC
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000148 RID: 328
		// (add) Token: 0x06000726 RID: 1830 RVA: 0x00013EF4 File Offset: 0x000120F4
		// (remove) Token: 0x06000727 RID: 1831 RVA: 0x00013F2C File Offset: 0x0001212C
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000149 RID: 329
		// (add) Token: 0x06000728 RID: 1832 RVA: 0x00013F64 File Offset: 0x00012164
		// (remove) Token: 0x06000729 RID: 1833 RVA: 0x00013F9C File Offset: 0x0001219C
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400014A RID: 330
		// (add) Token: 0x0600072A RID: 1834 RVA: 0x00013FD4 File Offset: 0x000121D4
		// (remove) Token: 0x0600072B RID: 1835 RVA: 0x0001400C File Offset: 0x0001220C
		public event EventHandler OnAutoReconnected;

		// Token: 0x0600072C RID: 1836 RVA: 0x00014041 File Offset: 0x00012241
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00014062 File Offset: 0x00012262
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00014083 File Offset: 0x00012283
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000140A5 File Offset: 0x000122A5
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000140C8 File Offset: 0x000122C8
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000140EC File Offset: 0x000122EC
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001411C File Offset: 0x0001231C
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001414C File Offset: 0x0001234C
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient4NotSafeForScriptingEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001419C File Offset: 0x0001239C
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

		// Token: 0x06000735 RID: 1845 RVA: 0x000141CC File Offset: 0x000123CC
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

		// Token: 0x06000736 RID: 1846 RVA: 0x00014200 File Offset: 0x00012400
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00014217 File Offset: 0x00012417
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001422E File Offset: 0x0001242E
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00014245 File Offset: 0x00012445
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001425C File Offset: 0x0001245C
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00014273 File Offset: 0x00012473
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001428A File Offset: 0x0001248A
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000142A1 File Offset: 0x000124A1
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000142B8 File Offset: 0x000124B8
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000142CF File Offset: 0x000124CF
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000142E6 File Offset: 0x000124E6
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000142FD File Offset: 0x000124FD
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00014314 File Offset: 0x00012514
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001432B File Offset: 0x0001252B
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00014342 File Offset: 0x00012542
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00014359 File Offset: 0x00012559
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00014370 File Offset: 0x00012570
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00014387 File Offset: 0x00012587
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001439E File Offset: 0x0001259E
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000143B5 File Offset: 0x000125B5
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000143CC File Offset: 0x000125CC
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000143E3 File Offset: 0x000125E3
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000143FA File Offset: 0x000125FA
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00014411 File Offset: 0x00012611
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00014428 File Offset: 0x00012628
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001443F File Offset: 0x0001263F
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00014456 File Offset: 0x00012656
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001446D File Offset: 0x0001266D
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00014484 File Offset: 0x00012684
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001449B File Offset: 0x0001269B
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x04000170 RID: 368
		private IMsRdpClient4 ocx;

		// Token: 0x04000171 RID: 369
		private AxMsRdpClient4NotSafeForScriptingEventMulticaster eventMulticaster;

		// Token: 0x04000172 RID: 370
		private AxHost.ConnectionPointCookie cookie;
	}
}
