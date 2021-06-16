using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200003A RID: 58
	[AxHost.ClsidAttribute("{4edcb26c-d24c-4e72-af07-b576699ac0de}")]
	[DesignTimeVisible(true)]
	[DefaultEvent("OnConnecting")]
	public class AxMsRdpClient4 : AxHost
	{
		// Token: 0x06000773 RID: 1907 RVA: 0x0001499D File Offset: 0x00012B9D
		public AxMsRdpClient4() : base("4edcb26c-d24c-4e72-af07-b576699ac0de")
		{
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x000149AA File Offset: 0x00012BAA
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x000149CB File Offset: 0x00012BCB
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

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000149ED File Offset: 0x00012BED
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x00014A0E File Offset: 0x00012C0E
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

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00014A30 File Offset: 0x00012C30
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x00014A51 File Offset: 0x00012C51
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00014A73 File Offset: 0x00012C73
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00014A94 File Offset: 0x00012C94
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00014AB6 File Offset: 0x00012CB6
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00014AD7 File Offset: 0x00012CD7
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00014AF9 File Offset: 0x00012CF9
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00014B1A File Offset: 0x00012D1A
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x00014B3B File Offset: 0x00012D3B
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00014B5D File Offset: 0x00012D5D
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x00014B7E File Offset: 0x00012D7E
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00014BA0 File Offset: 0x00012DA0
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x00014BC1 File Offset: 0x00012DC1
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

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x00014BE3 File Offset: 0x00012DE3
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00014C04 File Offset: 0x00012E04
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

		// Token: 0x17000112 RID: 274
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x00014C25 File Offset: 0x00012E25
		[Browsable(false)]
		[DispId(19)]
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

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x00014C47 File Offset: 0x00012E47
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

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00014C68 File Offset: 0x00012E68
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

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00014C89 File Offset: 0x00012E89
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

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00014CAA File Offset: 0x00012EAA
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00014CCB File Offset: 0x00012ECB
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00014CEC File Offset: 0x00012EEC
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x00014D0D File Offset: 0x00012F0D
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x00014D2E File Offset: 0x00012F2E
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

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x00014D50 File Offset: 0x00012F50
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

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00014D71 File Offset: 0x00012F71
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00014D92 File Offset: 0x00012F92
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

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00014DB3 File Offset: 0x00012FB3
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x00014DD4 File Offset: 0x00012FD4
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

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00014DF6 File Offset: 0x00012FF6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(200)]
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

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00014E17 File Offset: 0x00013017
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x00014E38 File Offset: 0x00013038
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

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00014E5A File Offset: 0x0001305A
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

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00014E7B File Offset: 0x0001307B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DispId(400)]
		[Browsable(false)]
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

		// Token: 0x1400014B RID: 331
		// (add) Token: 0x0600079A RID: 1946 RVA: 0x00014E9C File Offset: 0x0001309C
		// (remove) Token: 0x0600079B RID: 1947 RVA: 0x00014ED4 File Offset: 0x000130D4
		public event EventHandler OnConnecting;

		// Token: 0x1400014C RID: 332
		// (add) Token: 0x0600079C RID: 1948 RVA: 0x00014F0C File Offset: 0x0001310C
		// (remove) Token: 0x0600079D RID: 1949 RVA: 0x00014F44 File Offset: 0x00013144
		public event EventHandler OnConnected;

		// Token: 0x1400014D RID: 333
		// (add) Token: 0x0600079E RID: 1950 RVA: 0x00014F7C File Offset: 0x0001317C
		// (remove) Token: 0x0600079F RID: 1951 RVA: 0x00014FB4 File Offset: 0x000131B4
		public event EventHandler OnLoginComplete;

		// Token: 0x1400014E RID: 334
		// (add) Token: 0x060007A0 RID: 1952 RVA: 0x00014FEC File Offset: 0x000131EC
		// (remove) Token: 0x060007A1 RID: 1953 RVA: 0x00015024 File Offset: 0x00013224
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400014F RID: 335
		// (add) Token: 0x060007A2 RID: 1954 RVA: 0x0001505C File Offset: 0x0001325C
		// (remove) Token: 0x060007A3 RID: 1955 RVA: 0x00015094 File Offset: 0x00013294
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x14000150 RID: 336
		// (add) Token: 0x060007A4 RID: 1956 RVA: 0x000150CC File Offset: 0x000132CC
		// (remove) Token: 0x060007A5 RID: 1957 RVA: 0x00015104 File Offset: 0x00013304
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x14000151 RID: 337
		// (add) Token: 0x060007A6 RID: 1958 RVA: 0x0001513C File Offset: 0x0001333C
		// (remove) Token: 0x060007A7 RID: 1959 RVA: 0x00015174 File Offset: 0x00013374
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000152 RID: 338
		// (add) Token: 0x060007A8 RID: 1960 RVA: 0x000151AC File Offset: 0x000133AC
		// (remove) Token: 0x060007A9 RID: 1961 RVA: 0x000151E4 File Offset: 0x000133E4
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x14000153 RID: 339
		// (add) Token: 0x060007AA RID: 1962 RVA: 0x0001521C File Offset: 0x0001341C
		// (remove) Token: 0x060007AB RID: 1963 RVA: 0x00015254 File Offset: 0x00013454
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000154 RID: 340
		// (add) Token: 0x060007AC RID: 1964 RVA: 0x0001528C File Offset: 0x0001348C
		// (remove) Token: 0x060007AD RID: 1965 RVA: 0x000152C4 File Offset: 0x000134C4
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000155 RID: 341
		// (add) Token: 0x060007AE RID: 1966 RVA: 0x000152FC File Offset: 0x000134FC
		// (remove) Token: 0x060007AF RID: 1967 RVA: 0x00015334 File Offset: 0x00013534
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000156 RID: 342
		// (add) Token: 0x060007B0 RID: 1968 RVA: 0x0001536C File Offset: 0x0001356C
		// (remove) Token: 0x060007B1 RID: 1969 RVA: 0x000153A4 File Offset: 0x000135A4
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000157 RID: 343
		// (add) Token: 0x060007B2 RID: 1970 RVA: 0x000153DC File Offset: 0x000135DC
		// (remove) Token: 0x060007B3 RID: 1971 RVA: 0x00015414 File Offset: 0x00013614
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x14000158 RID: 344
		// (add) Token: 0x060007B4 RID: 1972 RVA: 0x0001544C File Offset: 0x0001364C
		// (remove) Token: 0x060007B5 RID: 1973 RVA: 0x00015484 File Offset: 0x00013684
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x14000159 RID: 345
		// (add) Token: 0x060007B6 RID: 1974 RVA: 0x000154BC File Offset: 0x000136BC
		// (remove) Token: 0x060007B7 RID: 1975 RVA: 0x000154F4 File Offset: 0x000136F4
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400015A RID: 346
		// (add) Token: 0x060007B8 RID: 1976 RVA: 0x0001552C File Offset: 0x0001372C
		// (remove) Token: 0x060007B9 RID: 1977 RVA: 0x00015564 File Offset: 0x00013764
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400015B RID: 347
		// (add) Token: 0x060007BA RID: 1978 RVA: 0x0001559C File Offset: 0x0001379C
		// (remove) Token: 0x060007BB RID: 1979 RVA: 0x000155D4 File Offset: 0x000137D4
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400015C RID: 348
		// (add) Token: 0x060007BC RID: 1980 RVA: 0x0001560C File Offset: 0x0001380C
		// (remove) Token: 0x060007BD RID: 1981 RVA: 0x00015644 File Offset: 0x00013844
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400015D RID: 349
		// (add) Token: 0x060007BE RID: 1982 RVA: 0x0001567C File Offset: 0x0001387C
		// (remove) Token: 0x060007BF RID: 1983 RVA: 0x000156B4 File Offset: 0x000138B4
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400015E RID: 350
		// (add) Token: 0x060007C0 RID: 1984 RVA: 0x000156EC File Offset: 0x000138EC
		// (remove) Token: 0x060007C1 RID: 1985 RVA: 0x00015724 File Offset: 0x00013924
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400015F RID: 351
		// (add) Token: 0x060007C2 RID: 1986 RVA: 0x0001575C File Offset: 0x0001395C
		// (remove) Token: 0x060007C3 RID: 1987 RVA: 0x00015794 File Offset: 0x00013994
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000160 RID: 352
		// (add) Token: 0x060007C4 RID: 1988 RVA: 0x000157CC File Offset: 0x000139CC
		// (remove) Token: 0x060007C5 RID: 1989 RVA: 0x00015804 File Offset: 0x00013A04
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000161 RID: 353
		// (add) Token: 0x060007C6 RID: 1990 RVA: 0x0001583C File Offset: 0x00013A3C
		// (remove) Token: 0x060007C7 RID: 1991 RVA: 0x00015874 File Offset: 0x00013A74
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000162 RID: 354
		// (add) Token: 0x060007C8 RID: 1992 RVA: 0x000158AC File Offset: 0x00013AAC
		// (remove) Token: 0x060007C9 RID: 1993 RVA: 0x000158E4 File Offset: 0x00013AE4
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000163 RID: 355
		// (add) Token: 0x060007CA RID: 1994 RVA: 0x0001591C File Offset: 0x00013B1C
		// (remove) Token: 0x060007CB RID: 1995 RVA: 0x00015954 File Offset: 0x00013B54
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000164 RID: 356
		// (add) Token: 0x060007CC RID: 1996 RVA: 0x0001598C File Offset: 0x00013B8C
		// (remove) Token: 0x060007CD RID: 1997 RVA: 0x000159C4 File Offset: 0x00013BC4
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000165 RID: 357
		// (add) Token: 0x060007CE RID: 1998 RVA: 0x000159FC File Offset: 0x00013BFC
		// (remove) Token: 0x060007CF RID: 1999 RVA: 0x00015A34 File Offset: 0x00013C34
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000166 RID: 358
		// (add) Token: 0x060007D0 RID: 2000 RVA: 0x00015A6C File Offset: 0x00013C6C
		// (remove) Token: 0x060007D1 RID: 2001 RVA: 0x00015AA4 File Offset: 0x00013CA4
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x14000167 RID: 359
		// (add) Token: 0x060007D2 RID: 2002 RVA: 0x00015ADC File Offset: 0x00013CDC
		// (remove) Token: 0x060007D3 RID: 2003 RVA: 0x00015B14 File Offset: 0x00013D14
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x14000168 RID: 360
		// (add) Token: 0x060007D4 RID: 2004 RVA: 0x00015B4C File Offset: 0x00013D4C
		// (remove) Token: 0x060007D5 RID: 2005 RVA: 0x00015B84 File Offset: 0x00013D84
		public event EventHandler OnAutoReconnected;

		// Token: 0x060007D6 RID: 2006 RVA: 0x00015BB9 File Offset: 0x00013DB9
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00015BDA File Offset: 0x00013DDA
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00015BFB File Offset: 0x00013DFB
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00015C1D File Offset: 0x00013E1D
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00015C40 File Offset: 0x00013E40
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00015C64 File Offset: 0x00013E64
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00015C94 File Offset: 0x00013E94
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00015CC4 File Offset: 0x00013EC4
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient4EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00015D14 File Offset: 0x00013F14
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

		// Token: 0x060007DF RID: 2015 RVA: 0x00015D44 File Offset: 0x00013F44
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

		// Token: 0x060007E0 RID: 2016 RVA: 0x00015D78 File Offset: 0x00013F78
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00015D8F File Offset: 0x00013F8F
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00015DA6 File Offset: 0x00013FA6
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00015DBD File Offset: 0x00013FBD
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00015DD4 File Offset: 0x00013FD4
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00015DEB File Offset: 0x00013FEB
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00015E02 File Offset: 0x00014002
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00015E19 File Offset: 0x00014019
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00015E30 File Offset: 0x00014030
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00015E47 File Offset: 0x00014047
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00015E5E File Offset: 0x0001405E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00015E75 File Offset: 0x00014075
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00015E8C File Offset: 0x0001408C
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00015EA3 File Offset: 0x000140A3
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00015EBA File Offset: 0x000140BA
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00015ED1 File Offset: 0x000140D1
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00015EE8 File Offset: 0x000140E8
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00015EFF File Offset: 0x000140FF
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00015F16 File Offset: 0x00014116
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00015F2D File Offset: 0x0001412D
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00015F44 File Offset: 0x00014144
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00015F5B File Offset: 0x0001415B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00015F72 File Offset: 0x00014172
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00015F89 File Offset: 0x00014189
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00015FA0 File Offset: 0x000141A0
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00015FB7 File Offset: 0x000141B7
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00015FCE File Offset: 0x000141CE
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00015FE5 File Offset: 0x000141E5
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00015FFC File Offset: 0x000141FC
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00016013 File Offset: 0x00014213
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x04000192 RID: 402
		private IMsRdpClient4 ocx;

		// Token: 0x04000193 RID: 403
		private AxMsRdpClient4EventMulticaster eventMulticaster;

		// Token: 0x04000194 RID: 404
		private AxHost.ConnectionPointCookie cookie;
	}
}
