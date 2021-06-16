using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000010 RID: 16
	public class XdeSimpleCommandsPipe : XdePipe, IXdeSimpleCommandsPipe, IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable, IXdeAutomationSimpleCommandsPipe
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x000057DD File Offset: 0x000039DD
		protected XdeSimpleCommandsPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeSimpleCommandsPipe.simpleCommandsGuid)
		{
			this.pipeLock = new object();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000057F8 File Offset: 0x000039F8
		private string DnsServerList
		{
			get
			{
				string text = "";
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				List<IPAddress> list = new List<IPAddress>();
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					if (networkInterface.OperationalStatus == OperationalStatus.Up)
					{
						IPAddressCollection dnsAddresses = networkInterface.GetIPProperties().DnsAddresses;
						if (dnsAddresses.Count > 0)
						{
							foreach (IPAddress item in dnsAddresses)
							{
								if (!list.Contains(item))
								{
									list.Add(item);
								}
							}
						}
					}
				}
				foreach (IPAddress ipaddress in list)
				{
					if (string.IsNullOrEmpty(text))
					{
						text = ipaddress.ToString();
					}
					else
					{
						text = text + ";" + ipaddress.ToString();
					}
				}
				return text;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000058F8 File Offset: 0x00003AF8
		public static IXdeSimpleCommandsPipe XdeSimpleCommandsPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeSimpleCommandsPipe(addressInfo);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005900 File Offset: 0x00003B00
		public Size GetScreenSize()
		{
			int[] array = new int[2];
			int[] array2 = new int[2];
			array[0] = 0;
			array[1] = 0;
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(array);
				base.ReceiveFromGuest(array2);
			}
			return new Size(array2[0], array2[1]);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000596C File Offset: 0x00003B6C
		public void SetupGuestProxyAndDNSServers()
		{
			this.SetupHttpProxyServer();
			this.SetupSocksProxyServer();
			this.SetupDnsServers();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005980 File Offset: 0x00003B80
		public void SetupHttpProxyServer()
		{
			this.SetupProxyServer(true);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005989 File Offset: 0x00003B89
		public void SetupSocksProxyServer()
		{
			this.SetupProxyServer(false);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005994 File Offset: 0x00003B94
		public void InitiateSystemShutdown()
		{
			int[] data = new int[]
			{
				4,
				0
			};
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000059E4 File Offset: 0x00003BE4
		public void InitiateSystemReboot()
		{
			int[] data = new int[]
			{
				10,
				0
			};
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005A38 File Offset: 0x00003C38
		public void IndicateInternetConnectivityReprobe()
		{
			int[] data = new int[]
			{
				13,
				0
			};
			int num = 0;
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.GetAdapterAddressesFailed);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public void SetGuestSystemTimeAndZone()
		{
			int num = Marshal.SizeOf(typeof(XdeSimpleCommandsPipe.SYSTEMTIME)) + Marshal.SizeOf(typeof(XdeSimpleCommandsPipe.DynamicTimeZoneInformation));
			XdeSimpleCommandsPipe.SYSTEMTIME time = this.GetTime();
			XdeSimpleCommandsPipe.DynamicTimeZoneInformation timeZone = this.GetTimeZone();
			int[] array = new int[2];
			int num2 = 0;
			array[0] = 5;
			array[1] = num;
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(array);
				base.SendStructToGuest(time);
				base.SendStructToGuest(timeZone);
				num2 = base.ReceiveIntFromGuest();
			}
			if (num2 != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.SetSystemTimeAndZoneFailed);
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005B5C File Offset: 0x00003D5C
		public void SetGuestNATSubnet(IPSubnet subnet, IPAddress gateway)
		{
			int[] array = new int[2];
			int num = 0;
			array[0] = 14;
			array[1] = 0;
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(array);
				base.SendToGuest(BitConverter.ToInt32(subnet.IPPrefix.GetAddressBytes(), 0));
				base.SendToGuest(BitConverter.ToInt32(subnet.IPMask.GetAddressBytes(), 0));
				base.SendToGuest(BitConverter.ToInt32(gateway.GetAddressBytes(), 0));
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.SetGuestNATSubnetFailed);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005C08 File Offset: 0x00003E08
		public void SetNetworkThrottlingParams(ThrottlerParams throttlingParams)
		{
			int[] array = new int[2];
			int num = 0;
			array[0] = 15;
			array[1] = Marshal.SizeOf(typeof(ThrottlerParams));
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(array);
				base.SendStructToGuest(throttlingParams);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.SetNetworkThrottlingParamsFailed);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005C8C File Offset: 0x00003E8C
		public NetworkAdapterInformation[] GetGuestAdapterInformation()
		{
			int[] data = new int[]
			{
				6,
				0
			};
			object obj = this.pipeLock;
			NetworkAdapterInformation[] result;
			lock (obj)
			{
				base.SendToGuest(data);
				int num = base.ReceiveIntFromGuest();
				if (num < 0)
				{
					base.ThrowXdePipeException(PipeExceptions.GetAdapterAddressesFailed);
				}
				NetworkAdapterInformation[] array = new NetworkAdapterInformation[num];
				for (int i = 0; i < num; i++)
				{
					int num2 = base.ReceiveIntFromGuest();
					if (num2 <= 0)
					{
						base.ThrowXdePipeException(PipeExceptions.GetAdapterAddressesFailed);
					}
					byte[] array2 = new byte[num2];
					base.ReceiveFromGuest(array2);
					array[i].MacAddress = XdeSimpleCommandsPipe.GetMacAddress(array2);
					array[i].IPAddresses = this.ReceiveIPAddresses();
				}
				result = array;
			}
			return result;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005D60 File Offset: 0x00003F60
		public void InitiateIPRenew()
		{
			int[] array = new int[2];
			int num = 0;
			array[0] = 7;
			array[1] = 0;
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(array);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.IpRenewalFailed);
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public void SetupDnsServers()
		{
			int[] array = new int[2];
			int num = 0;
			array[0] = 9;
			array[1] = 0;
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(array);
				base.SendToGuest(this.DnsServerList);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.DnsServerSetupFailed);
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005E40 File Offset: 0x00004040
		public void SetupProxyServer(bool isHttpProxy)
		{
			int num = 0;
			string stringValue;
			int num2;
			if (isHttpProxy)
			{
				stringValue = this.DetectHttpProxyServer();
				num2 = 1;
			}
			else
			{
				stringValue = this.DetectSocksProxyServer();
				num2 = 8;
			}
			int[] data = new int[]
			{
				num2,
				0
			};
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
				base.SendToGuest(stringValue);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				string message;
				if (isHttpProxy)
				{
					message = PipeExceptions.HttpProxySetupFailed;
				}
				else
				{
					message = PipeExceptions.SocksProxySetupFailed;
				}
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005EDC File Offset: 0x000040DC
		public XdeSensors GetSensorsEnabledStates()
		{
			int result = 0;
			int num = 0;
			int[] data = new int[]
			{
				11,
				0
			};
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
				result = base.ReceiveIntFromGuest();
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.GetSensorsEnabledStatesFailed);
			}
			return (XdeSensors)result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005F54 File Offset: 0x00004154
		public void SetSensorsEnabledStates(XdeSensors sensorsStatusBV)
		{
			int num = 0;
			int[] data = new int[]
			{
				12,
				4
			};
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
				base.SendToGuest((int)sensorsStatusBV);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 1)
			{
				base.ThrowXdePipeException(PipeExceptions.SetSensorsEnabledStatesFailed);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005FC4 File Offset: 0x000041C4
		public void SetEnabledMonitors(int monitorsToEnable)
		{
			int num = 0;
			int[] data = new int[]
			{
				16,
				4
			};
			object obj = this.pipeLock;
			lock (obj)
			{
				base.SendToGuest(data);
				base.SendToGuest(monitorsToEnable);
				num = base.ReceiveIntFromGuest();
			}
			if (num != 0)
			{
				string message = StringUtilities.CurrentCultureFormat(PipeExceptions.SetEnabledMonitorsFormat, new object[]
				{
					num
				});
				base.ThrowXdePipeException(message);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000604C File Offset: 0x0000424C
		protected string DetectHttpProxyServer()
		{
			Uri uri = new Uri("http://www.microsoft.com");
			Uri proxy = WebRequest.Create(uri).Proxy.GetProxy(uri);
			string text;
			if (proxy == uri)
			{
				text = "";
			}
			else
			{
				text = proxy.Host;
				text += ":";
				text += proxy.Port.ToString();
			}
			return text;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000060B0 File Offset: 0x000042B0
		protected string DetectSocksProxyServer()
		{
			NativeMethods.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG winhttp_CURRENT_USER_IE_PROXY_CONFIG = default(NativeMethods.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG);
			string result = "";
			try
			{
				if (NativeMethods.WinHttpGetIEProxyConfigForCurrentUser(ref winhttp_CURRENT_USER_IE_PROXY_CONFIG) && winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoConfigUrl == null && !winhttp_CURRENT_USER_IE_PROXY_CONFIG.fAutoDetect && winhttp_CURRENT_USER_IE_PROXY_CONFIG.Proxy != null)
				{
					result = this.ExtractProxyString(winhttp_CURRENT_USER_IE_PROXY_CONFIG.Proxy, "socks", 1080);
				}
			}
			finally
			{
				winhttp_CURRENT_USER_IE_PROXY_CONFIG.Free();
			}
			return result;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006124 File Offset: 0x00004324
		protected string ExtractProxyString(string settingsString, string protocolPrefix, int defaultProxyPort)
		{
			string text = settingsString;
			int num;
			if (settingsString.Contains("="))
			{
				num = text.IndexOf(protocolPrefix + "=", StringComparison.OrdinalIgnoreCase);
				if (num == -1)
				{
					return "";
				}
				text = text.Remove(0, num + protocolPrefix.Length + 1);
			}
			num = text.IndexOf(';');
			if (num != -1)
			{
				text = text.Remove(num);
			}
			while (text.Length > 0 && text[text.Length - 1] == '/')
			{
				text = text.Remove(text.Length - 1);
			}
			if (text.Length > 0)
			{
				num = text.IndexOf(':');
				if (num == -1)
				{
					text += ":";
					text += defaultProxyPort.ToString();
				}
			}
			return text;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000061E4 File Offset: 0x000043E4
		protected NetworkIPAddress[] ReceiveIPAddresses()
		{
			int num = base.ReceiveIntFromGuest();
			if (num < 0)
			{
				base.ThrowXdePipeException(PipeExceptions.GetAdapterAddressesFailed);
			}
			NetworkIPAddress[] array = new NetworkIPAddress[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = base.ReceiveIntFromGuest();
				if (num2 <= 0)
				{
					base.ThrowXdePipeException(PipeExceptions.GetAdapterAddressesFailed);
				}
				byte[] array2 = new byte[num2];
				base.ReceiveFromGuest(array2);
				array[i].IPAddress = new IPAddress(array2);
				array[i].DadState = (IpDadState)base.ReceiveIntFromGuest();
			}
			return array;
		}

		// Token: 0x0600010D RID: 269
		[DllImport("kernel32.dll")]
		private static extern void GetSystemTime(ref XdeSimpleCommandsPipe.SYSTEMTIME systemTime);

		// Token: 0x0600010E RID: 270
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetDynamicTimeZoneInformation(out XdeSimpleCommandsPipe.DynamicTimeZoneInformation timeZoneInformation);

		// Token: 0x0600010F RID: 271 RVA: 0x00006264 File Offset: 0x00004464
		private static string GetMacAddress(byte[] macAddressBytes)
		{
			string text = string.Empty;
			foreach (byte b in macAddressBytes)
			{
				text += b.ToString("X2");
			}
			return text;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000062A0 File Offset: 0x000044A0
		private XdeSimpleCommandsPipe.SYSTEMTIME GetTime()
		{
			XdeSimpleCommandsPipe.SYSTEMTIME result = default(XdeSimpleCommandsPipe.SYSTEMTIME);
			XdeSimpleCommandsPipe.GetSystemTime(ref result);
			return result;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000062C0 File Offset: 0x000044C0
		private XdeSimpleCommandsPipe.DynamicTimeZoneInformation GetTimeZone()
		{
			XdeSimpleCommandsPipe.DynamicTimeZoneInformation result;
			if (XdeSimpleCommandsPipe.GetDynamicTimeZoneInformation(out result) == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				Logger.Instance.LogError("GetDynamicTimeZoneInformationFailed", new
				{
					PartA_iKey = "A-MSTelDefault",
					err = lastWin32Error
				});
			}
			return result;
		}

		// Token: 0x04000045 RID: 69
		private static Guid simpleCommandsGuid = new Guid("{12e13c32-4705-4a98-9295-9d78e77c8164}");

		// Token: 0x04000046 RID: 70
		private object pipeLock;

		// Token: 0x02000028 RID: 40
		private enum XdeSimpleCommands
		{
			// Token: 0x040000A3 RID: 163
			GetVideoResolution,
			// Token: 0x040000A4 RID: 164
			SetHttpProxy,
			// Token: 0x040000A5 RID: 165
			CaptureScreen,
			// Token: 0x040000A6 RID: 166
			InitiateSystemShutdown = 4,
			// Token: 0x040000A7 RID: 167
			SetSystemTimeAndZone,
			// Token: 0x040000A8 RID: 168
			GetNetworkAdapters,
			// Token: 0x040000A9 RID: 169
			AsyncRenewExternalIp,
			// Token: 0x040000AA RID: 170
			SetSocksProxy,
			// Token: 0x040000AB RID: 171
			SetDnsServers,
			// Token: 0x040000AC RID: 172
			InitiateSystemReboot,
			// Token: 0x040000AD RID: 173
			GetSensorsEnabledStates,
			// Token: 0x040000AE RID: 174
			SetSensorsEnabledStates,
			// Token: 0x040000AF RID: 175
			IndicateInternetConnectivityReprobe,
			// Token: 0x040000B0 RID: 176
			SetGuestNATSubnet,
			// Token: 0x040000B1 RID: 177
			SetNetworkThrottlingParams,
			// Token: 0x040000B2 RID: 178
			EnableMonitors
		}

		// Token: 0x02000029 RID: 41
		private struct SYSTEMTIME
		{
			// Token: 0x040000B3 RID: 179
			[MarshalAs(UnmanagedType.U2)]
			public ushort Year;

			// Token: 0x040000B4 RID: 180
			[MarshalAs(UnmanagedType.U2)]
			public ushort Month;

			// Token: 0x040000B5 RID: 181
			[MarshalAs(UnmanagedType.U2)]
			public ushort DayOfWeek;

			// Token: 0x040000B6 RID: 182
			[MarshalAs(UnmanagedType.U2)]
			public ushort Day;

			// Token: 0x040000B7 RID: 183
			[MarshalAs(UnmanagedType.U2)]
			public ushort Hour;

			// Token: 0x040000B8 RID: 184
			[MarshalAs(UnmanagedType.U2)]
			public ushort Minute;

			// Token: 0x040000B9 RID: 185
			[MarshalAs(UnmanagedType.U2)]
			public ushort Second;

			// Token: 0x040000BA RID: 186
			[MarshalAs(UnmanagedType.U2)]
			public ushort Milliseconds;
		}

		// Token: 0x0200002A RID: 42
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct DynamicTimeZoneInformation
		{
			// Token: 0x040000BB RID: 187
			public int Bias;

			// Token: 0x040000BC RID: 188
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string StandardName;

			// Token: 0x040000BD RID: 189
			public XdeSimpleCommandsPipe.SYSTEMTIME StandardDate;

			// Token: 0x040000BE RID: 190
			public int StandardBias;

			// Token: 0x040000BF RID: 191
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DaylightName;

			// Token: 0x040000C0 RID: 192
			public XdeSimpleCommandsPipe.SYSTEMTIME DaylightDate;

			// Token: 0x040000C1 RID: 193
			public int DaylightBias;

			// Token: 0x040000C2 RID: 194
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string TimeZoneKeyName;

			// Token: 0x040000C3 RID: 195
			[MarshalAs(UnmanagedType.U1)]
			public bool DynamicDaylightTimeDisabled;
		}
	}
}
