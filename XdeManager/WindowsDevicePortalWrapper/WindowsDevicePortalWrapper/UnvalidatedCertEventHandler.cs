using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000008 RID: 8
	// (Invoke) Token: 0x06000124 RID: 292
	public delegate bool UnvalidatedCertEventHandler(DevicePortal sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);
}
