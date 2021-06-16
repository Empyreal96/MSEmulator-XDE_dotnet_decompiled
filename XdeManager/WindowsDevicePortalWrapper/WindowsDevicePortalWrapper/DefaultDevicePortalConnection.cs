using System;
using System.Net;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000007 RID: 7
	public class DefaultDevicePortalConnection : IDevicePortalConnection
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00006FFC File Offset: 0x000051FC
		public DefaultDevicePortalConnection(string address, string userName, string password)
		{
			this.Connection = new Uri(address);
			if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
			{
				this.Credentials = new NetworkCredential(string.Format("auto-{0}", userName), password);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007037 File Offset: 0x00005237
		public DefaultDevicePortalConnection(string address, string userName, SecureString password)
		{
			this.Connection = new Uri(address);
			if (!string.IsNullOrEmpty(userName) && password != null && password.Length > 0)
			{
				this.Credentials = new NetworkCredential(string.Format("auto-{0}", userName), password);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00007076 File Offset: 0x00005276
		// (set) Token: 0x06000117 RID: 279 RVA: 0x0000707E File Offset: 0x0000527E
		public Uri Connection { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00007088 File Offset: 0x00005288
		public Uri WebSocketConnection
		{
			get
			{
				if (this.Connection == null)
				{
					return null;
				}
				string arg = this.Connection.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ? "wss" : "ws";
				return new Uri(string.Format("{0}://{1}", arg, this.Connection.Authority));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000070E5 File Offset: 0x000052E5
		// (set) Token: 0x0600011A RID: 282 RVA: 0x000070ED File Offset: 0x000052ED
		public NetworkCredential Credentials { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000070F6 File Offset: 0x000052F6
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000070FE File Offset: 0x000052FE
		public string Family { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00007107 File Offset: 0x00005307
		// (set) Token: 0x0600011E RID: 286 RVA: 0x0000710F File Offset: 0x0000530F
		public DevicePortal.OperatingSystemInformation OsInfo { get; set; }

		// Token: 0x0600011F RID: 287 RVA: 0x00007118 File Offset: 0x00005318
		public X509Certificate2 GetDeviceCertificate()
		{
			return this.deviceCertificate;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007120 File Offset: 0x00005320
		public void SetDeviceCertificate(X509Certificate2 certificate)
		{
			this.deviceCertificate = certificate;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007129 File Offset: 0x00005329
		public void UpdateConnection(bool requiresHttps)
		{
			this.Connection = new Uri(string.Format("{0}://{1}", requiresHttps ? "https" : "http", this.Connection.Authority));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000715C File Offset: 0x0000535C
		public void UpdateConnection(DevicePortal.IpConfiguration ipConfig, bool requiresHttps, bool preservePort)
		{
			Uri uri = null;
			foreach (DevicePortal.NetworkAdapterInfo networkAdapterInfo in ipConfig.Adapters)
			{
				foreach (DevicePortal.IpAddressInfo ipAddressInfo in networkAdapterInfo.IpAddresses)
				{
					if (ipAddressInfo.Address != "0.0.0.0" && !ipAddressInfo.Address.StartsWith("169.", StringComparison.OrdinalIgnoreCase))
					{
						string text = ipAddressInfo.Address;
						if (preservePort)
						{
							text = string.Format("{0}:{1}", text, this.Connection.Port);
						}
						uri = new Uri(string.Format("{0}://{1}", requiresHttps ? "https" : "http", text));
						break;
					}
				}
				if (uri != null)
				{
					this.Connection = uri;
					break;
				}
			}
		}

		// Token: 0x040000F1 RID: 241
		private X509Certificate2 deviceCertificate;
	}
}
