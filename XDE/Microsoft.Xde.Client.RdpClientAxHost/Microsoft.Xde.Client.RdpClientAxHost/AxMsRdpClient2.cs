using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002E RID: 46
	[DesignTimeVisible(true)]
	[DefaultEvent("OnConnecting")]
	[AxHost.ClsidAttribute("{9059f30f-4eb1-4bd2-9fdc-36f43a218f4a}")]
	public class AxMsRdpClient2 : AxHost
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0000A5AD File Offset: 0x000087AD
		public AxMsRdpClient2() : base("9059f30f-4eb1-4bd2-9fdc-36f43a218f4a")
		{
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000A5BA File Offset: 0x000087BA
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0000A5DB File Offset: 0x000087DB
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

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000A5FD File Offset: 0x000087FD
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0000A61E File Offset: 0x0000881E
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

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000A640 File Offset: 0x00008840
		// (set) Token: 0x06000384 RID: 900 RVA: 0x0000A661 File Offset: 0x00008861
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000A683 File Offset: 0x00008883
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000A6A4 File Offset: 0x000088A4
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0000A6C6 File Offset: 0x000088C6
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0000A6E7 File Offset: 0x000088E7
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000A709 File Offset: 0x00008909
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000A72A File Offset: 0x0000892A
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000A74B File Offset: 0x0000894B
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

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000A76D File Offset: 0x0000896D
		// (set) Token: 0x0600038D RID: 909 RVA: 0x0000A78E File Offset: 0x0000898E
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000A7B0 File Offset: 0x000089B0
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000A7D1 File Offset: 0x000089D1
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

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000A7F3 File Offset: 0x000089F3
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000A814 File Offset: 0x00008A14
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

		// Token: 0x17000077 RID: 119
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000A835 File Offset: 0x00008A35
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000A857 File Offset: 0x00008A57
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000A878 File Offset: 0x00008A78
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000A899 File Offset: 0x00008A99
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000A8BA File Offset: 0x00008ABA
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000A8DB File Offset: 0x00008ADB
		[Browsable(false)]
		[DispId(98)]
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000A8FC File Offset: 0x00008AFC
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000A91D File Offset: 0x00008B1D
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000A93E File Offset: 0x00008B3E
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000A960 File Offset: 0x00008B60
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000A981 File Offset: 0x00008B81
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000A9A2 File Offset: 0x00008BA2
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000A9C3 File Offset: 0x00008BC3
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000A9E4 File Offset: 0x00008BE4
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000AA06 File Offset: 0x00008C06
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000AA27 File Offset: 0x00008C27
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000AA48 File Offset: 0x00008C48
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

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x060003A3 RID: 931 RVA: 0x0000AA6C File Offset: 0x00008C6C
		// (remove) Token: 0x060003A4 RID: 932 RVA: 0x0000AAA4 File Offset: 0x00008CA4
		public event EventHandler OnConnecting;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x060003A5 RID: 933 RVA: 0x0000AADC File Offset: 0x00008CDC
		// (remove) Token: 0x060003A6 RID: 934 RVA: 0x0000AB14 File Offset: 0x00008D14
		public event EventHandler OnConnected;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060003A7 RID: 935 RVA: 0x0000AB4C File Offset: 0x00008D4C
		// (remove) Token: 0x060003A8 RID: 936 RVA: 0x0000AB84 File Offset: 0x00008D84
		public event EventHandler OnLoginComplete;

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x060003A9 RID: 937 RVA: 0x0000ABBC File Offset: 0x00008DBC
		// (remove) Token: 0x060003AA RID: 938 RVA: 0x0000ABF4 File Offset: 0x00008DF4
		public event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x060003AB RID: 939 RVA: 0x0000AC2C File Offset: 0x00008E2C
		// (remove) Token: 0x060003AC RID: 940 RVA: 0x0000AC64 File Offset: 0x00008E64
		public event EventHandler OnEnterFullScreenMode;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060003AD RID: 941 RVA: 0x0000AC9C File Offset: 0x00008E9C
		// (remove) Token: 0x060003AE RID: 942 RVA: 0x0000ACD4 File Offset: 0x00008ED4
		public event EventHandler OnLeaveFullScreenMode;

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060003AF RID: 943 RVA: 0x0000AD0C File Offset: 0x00008F0C
		// (remove) Token: 0x060003B0 RID: 944 RVA: 0x0000AD44 File Offset: 0x00008F44
		public event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x060003B1 RID: 945 RVA: 0x0000AD7C File Offset: 0x00008F7C
		// (remove) Token: 0x060003B2 RID: 946 RVA: 0x0000ADB4 File Offset: 0x00008FB4
		public event EventHandler OnRequestGoFullScreen;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x060003B3 RID: 947 RVA: 0x0000ADEC File Offset: 0x00008FEC
		// (remove) Token: 0x060003B4 RID: 948 RVA: 0x0000AE24 File Offset: 0x00009024
		public event EventHandler OnRequestLeaveFullScreen;

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x0000AE5C File Offset: 0x0000905C
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x0000AE94 File Offset: 0x00009094
		public event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x0000AECC File Offset: 0x000090CC
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x0000AF04 File Offset: 0x00009104
		public event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x060003B9 RID: 953 RVA: 0x0000AF3C File Offset: 0x0000913C
		// (remove) Token: 0x060003BA RID: 954 RVA: 0x0000AF74 File Offset: 0x00009174
		public event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060003BB RID: 955 RVA: 0x0000AFAC File Offset: 0x000091AC
		// (remove) Token: 0x060003BC RID: 956 RVA: 0x0000AFE4 File Offset: 0x000091E4
		public event EventHandler OnIdleTimeoutNotification;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x060003BD RID: 957 RVA: 0x0000B01C File Offset: 0x0000921C
		// (remove) Token: 0x060003BE RID: 958 RVA: 0x0000B054 File Offset: 0x00009254
		public event EventHandler OnRequestContainerMinimize;

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x060003BF RID: 959 RVA: 0x0000B08C File Offset: 0x0000928C
		// (remove) Token: 0x060003C0 RID: 960 RVA: 0x0000B0C4 File Offset: 0x000092C4
		public event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x060003C1 RID: 961 RVA: 0x0000B0FC File Offset: 0x000092FC
		// (remove) Token: 0x060003C2 RID: 962 RVA: 0x0000B134 File Offset: 0x00009334
		public event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060003C3 RID: 963 RVA: 0x0000B16C File Offset: 0x0000936C
		// (remove) Token: 0x060003C4 RID: 964 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060003C5 RID: 965 RVA: 0x0000B1DC File Offset: 0x000093DC
		// (remove) Token: 0x060003C6 RID: 966 RVA: 0x0000B214 File Offset: 0x00009414
		public event EventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x060003C7 RID: 967 RVA: 0x0000B24C File Offset: 0x0000944C
		// (remove) Token: 0x060003C8 RID: 968 RVA: 0x0000B284 File Offset: 0x00009484
		public event EventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x060003C9 RID: 969 RVA: 0x0000B2BC File Offset: 0x000094BC
		// (remove) Token: 0x060003CA RID: 970 RVA: 0x0000B2F4 File Offset: 0x000094F4
		public event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060003CB RID: 971 RVA: 0x0000B32C File Offset: 0x0000952C
		// (remove) Token: 0x060003CC RID: 972 RVA: 0x0000B364 File Offset: 0x00009564
		public event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060003CD RID: 973 RVA: 0x0000B39C File Offset: 0x0000959C
		// (remove) Token: 0x060003CE RID: 974 RVA: 0x0000B3D4 File Offset: 0x000095D4
		public event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060003CF RID: 975 RVA: 0x0000B40C File Offset: 0x0000960C
		// (remove) Token: 0x060003D0 RID: 976 RVA: 0x0000B444 File Offset: 0x00009644
		public event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x060003D1 RID: 977 RVA: 0x0000B47C File Offset: 0x0000967C
		// (remove) Token: 0x060003D2 RID: 978 RVA: 0x0000B4B4 File Offset: 0x000096B4
		public event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x060003D3 RID: 979 RVA: 0x0000B4EC File Offset: 0x000096EC
		// (remove) Token: 0x060003D4 RID: 980 RVA: 0x0000B524 File Offset: 0x00009724
		public event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x060003D5 RID: 981 RVA: 0x0000B55C File Offset: 0x0000975C
		// (remove) Token: 0x060003D6 RID: 982 RVA: 0x0000B594 File Offset: 0x00009794
		public event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x060003D7 RID: 983 RVA: 0x0000B5CC File Offset: 0x000097CC
		// (remove) Token: 0x060003D8 RID: 984 RVA: 0x0000B604 File Offset: 0x00009804
		public event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x060003D9 RID: 985 RVA: 0x0000B63C File Offset: 0x0000983C
		// (remove) Token: 0x060003DA RID: 986 RVA: 0x0000B674 File Offset: 0x00009874
		public event EventHandler OnConnectionBarPullDown;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x060003DB RID: 987 RVA: 0x0000B6AC File Offset: 0x000098AC
		// (remove) Token: 0x060003DC RID: 988 RVA: 0x0000B6E4 File Offset: 0x000098E4
		public event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x060003DD RID: 989 RVA: 0x0000B71C File Offset: 0x0000991C
		// (remove) Token: 0x060003DE RID: 990 RVA: 0x0000B754 File Offset: 0x00009954
		public event EventHandler OnAutoReconnected;

		// Token: 0x060003DF RID: 991 RVA: 0x0000B789 File Offset: 0x00009989
		public virtual void Connect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Connect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Connect();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000B7AA File Offset: 0x000099AA
		public virtual void Disconnect()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("Disconnect", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Disconnect();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000B7CB File Offset: 0x000099CB
		public virtual void CreateVirtualChannels(string newVal)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("CreateVirtualChannels", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.CreateVirtualChannels(newVal);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000B7ED File Offset: 0x000099ED
		public virtual void SendOnVirtualChannel(string chanName, string chanData)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SendOnVirtualChannel", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SendOnVirtualChannel(chanName, chanData);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000B810 File Offset: 0x00009A10
		public virtual void SetVirtualChannelOptions(string chanName, int chanOptions)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("SetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.SetVirtualChannelOptions(chanName, chanOptions);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000B834 File Offset: 0x00009A34
		public virtual int GetVirtualChannelOptions(string chanName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("GetVirtualChannelOptions", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetVirtualChannelOptions(chanName);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000B864 File Offset: 0x00009A64
		public virtual ControlCloseStatus RequestClose()
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("RequestClose", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.RequestClose();
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000B894 File Offset: 0x00009A94
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxMsRdpClient2EventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(IMsTscAxEvents));
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000B8E4 File Offset: 0x00009AE4
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

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000B914 File Offset: 0x00009B14
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (IMsRdpClient2)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000B948 File Offset: 0x00009B48
		internal void RaiseOnOnConnecting(object sender, EventArgs e)
		{
			if (this.OnConnecting != null)
			{
				this.OnConnecting(sender, e);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000B95F File Offset: 0x00009B5F
		internal void RaiseOnOnConnected(object sender, EventArgs e)
		{
			if (this.OnConnected != null)
			{
				this.OnConnected(sender, e);
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000B976 File Offset: 0x00009B76
		internal void RaiseOnOnLoginComplete(object sender, EventArgs e)
		{
			if (this.OnLoginComplete != null)
			{
				this.OnLoginComplete(sender, e);
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000B98D File Offset: 0x00009B8D
		internal void RaiseOnOnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
		{
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(sender, e);
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000B9A4 File Offset: 0x00009BA4
		internal void RaiseOnOnEnterFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnEnterFullScreenMode != null)
			{
				this.OnEnterFullScreenMode(sender, e);
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000B9BB File Offset: 0x00009BBB
		internal void RaiseOnOnLeaveFullScreenMode(object sender, EventArgs e)
		{
			if (this.OnLeaveFullScreenMode != null)
			{
				this.OnLeaveFullScreenMode(sender, e);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000B9D2 File Offset: 0x00009BD2
		internal void RaiseOnOnChannelReceivedData(object sender, IMsTscAxEvents_OnChannelReceivedDataEvent e)
		{
			if (this.OnChannelReceivedData != null)
			{
				this.OnChannelReceivedData(sender, e);
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000B9E9 File Offset: 0x00009BE9
		internal void RaiseOnOnRequestGoFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestGoFullScreen != null)
			{
				this.OnRequestGoFullScreen(sender, e);
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000BA00 File Offset: 0x00009C00
		internal void RaiseOnOnRequestLeaveFullScreen(object sender, EventArgs e)
		{
			if (this.OnRequestLeaveFullScreen != null)
			{
				this.OnRequestLeaveFullScreen(sender, e);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000BA17 File Offset: 0x00009C17
		internal void RaiseOnOnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
		{
			if (this.OnFatalError != null)
			{
				this.OnFatalError(sender, e);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000BA2E File Offset: 0x00009C2E
		internal void RaiseOnOnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
		{
			if (this.OnWarning != null)
			{
				this.OnWarning(sender, e);
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000BA45 File Offset: 0x00009C45
		internal void RaiseOnOnRemoteDesktopSizeChange(object sender, IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
		{
			if (this.OnRemoteDesktopSizeChange != null)
			{
				this.OnRemoteDesktopSizeChange(sender, e);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000BA5C File Offset: 0x00009C5C
		internal void RaiseOnOnIdleTimeoutNotification(object sender, EventArgs e)
		{
			if (this.OnIdleTimeoutNotification != null)
			{
				this.OnIdleTimeoutNotification(sender, e);
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000BA73 File Offset: 0x00009C73
		internal void RaiseOnOnRequestContainerMinimize(object sender, EventArgs e)
		{
			if (this.OnRequestContainerMinimize != null)
			{
				this.OnRequestContainerMinimize(sender, e);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000BA8A File Offset: 0x00009C8A
		internal void RaiseOnOnConfirmClose(object sender, IMsTscAxEvents_OnConfirmCloseEvent e)
		{
			if (this.OnConfirmClose != null)
			{
				this.OnConfirmClose(sender, e);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000BAA1 File Offset: 0x00009CA1
		internal void RaiseOnOnReceivedTSPublicKey(object sender, IMsTscAxEvents_OnReceivedTSPublicKeyEvent e)
		{
			if (this.OnReceivedTSPublicKey != null)
			{
				this.OnReceivedTSPublicKey(sender, e);
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000BAB8 File Offset: 0x00009CB8
		internal void RaiseOnOnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
		{
			if (this.OnAutoReconnecting != null)
			{
				this.OnAutoReconnecting(sender, e);
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000BACF File Offset: 0x00009CCF
		internal void RaiseOnOnAuthenticationWarningDisplayed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDisplayed != null)
			{
				this.OnAuthenticationWarningDisplayed(sender, e);
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000BAE6 File Offset: 0x00009CE6
		internal void RaiseOnOnAuthenticationWarningDismissed(object sender, EventArgs e)
		{
			if (this.OnAuthenticationWarningDismissed != null)
			{
				this.OnAuthenticationWarningDismissed(sender, e);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000BAFD File Offset: 0x00009CFD
		internal void RaiseOnOnRemoteProgramResult(object sender, IMsTscAxEvents_OnRemoteProgramResultEvent e)
		{
			if (this.OnRemoteProgramResult != null)
			{
				this.OnRemoteProgramResult(sender, e);
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000BB14 File Offset: 0x00009D14
		internal void RaiseOnOnRemoteProgramDisplayed(object sender, IMsTscAxEvents_OnRemoteProgramDisplayedEvent e)
		{
			if (this.OnRemoteProgramDisplayed != null)
			{
				this.OnRemoteProgramDisplayed(sender, e);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000BB2B File Offset: 0x00009D2B
		internal void RaiseOnOnRemoteWindowDisplayed(object sender, IMsTscAxEvents_OnRemoteWindowDisplayedEvent e)
		{
			if (this.OnRemoteWindowDisplayed != null)
			{
				this.OnRemoteWindowDisplayed(sender, e);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000BB42 File Offset: 0x00009D42
		internal void RaiseOnOnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
		{
			if (this.OnLogonError != null)
			{
				this.OnLogonError(sender, e);
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000BB59 File Offset: 0x00009D59
		internal void RaiseOnOnFocusReleased(object sender, IMsTscAxEvents_OnFocusReleasedEvent e)
		{
			if (this.OnFocusReleased != null)
			{
				this.OnFocusReleased(sender, e);
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000BB70 File Offset: 0x00009D70
		internal void RaiseOnOnUserNameAcquired(object sender, IMsTscAxEvents_OnUserNameAcquiredEvent e)
		{
			if (this.OnUserNameAcquired != null)
			{
				this.OnUserNameAcquired(sender, e);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000BB87 File Offset: 0x00009D87
		internal void RaiseOnOnMouseInputModeChanged(object sender, IMsTscAxEvents_OnMouseInputModeChangedEvent e)
		{
			if (this.OnMouseInputModeChanged != null)
			{
				this.OnMouseInputModeChanged(sender, e);
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000BB9E File Offset: 0x00009D9E
		internal void RaiseOnOnServiceMessageReceived(object sender, IMsTscAxEvents_OnServiceMessageReceivedEvent e)
		{
			if (this.OnServiceMessageReceived != null)
			{
				this.OnServiceMessageReceived(sender, e);
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000BBB5 File Offset: 0x00009DB5
		internal void RaiseOnOnConnectionBarPullDown(object sender, EventArgs e)
		{
			if (this.OnConnectionBarPullDown != null)
			{
				this.OnConnectionBarPullDown(sender, e);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000BBCC File Offset: 0x00009DCC
		internal void RaiseOnOnNetworkBandwidthChanged(object sender, IMsTscAxEvents_OnNetworkBandwidthChangedEvent e)
		{
			if (this.OnNetworkBandwidthChanged != null)
			{
				this.OnNetworkBandwidthChanged(sender, e);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000BBE3 File Offset: 0x00009DE3
		internal void RaiseOnOnAutoReconnected(object sender, EventArgs e)
		{
			if (this.OnAutoReconnected != null)
			{
				this.OnAutoReconnected(sender, e);
			}
		}

		// Token: 0x040000C6 RID: 198
		private IMsRdpClient2 ocx;

		// Token: 0x040000C7 RID: 199
		private AxMsRdpClient2EventMulticaster eventMulticaster;

		// Token: 0x040000C8 RID: 200
		private AxHost.ConnectionPointCookie cookie;
	}
}
