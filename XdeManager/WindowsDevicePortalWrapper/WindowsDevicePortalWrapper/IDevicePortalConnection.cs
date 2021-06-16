using System;
using System.Net;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000018 RID: 24
	public interface IDevicePortalConnection
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600016F RID: 367
		Uri Connection { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000170 RID: 368
		Uri WebSocketConnection { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000171 RID: 369
		NetworkCredential Credentials { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000172 RID: 370
		// (set) Token: 0x06000173 RID: 371
		string Family { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000174 RID: 372
		// (set) Token: 0x06000175 RID: 373
		DevicePortal.OperatingSystemInformation OsInfo { get; set; }

		// Token: 0x06000176 RID: 374
		void UpdateConnection(bool requiresHttps);

		// Token: 0x06000177 RID: 375
		void UpdateConnection(DevicePortal.IpConfiguration ipConfig, bool requiresHttps, bool preservePort);
	}
}
