using System;
using System.Security.Principal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200035A RID: 858
	public sealed class PSIdentity : IIdentity
	{
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000EB3E0 File Offset: 0x000E95E0
		public string AuthenticationType
		{
			get
			{
				return this.authenticationType;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000EB3E8 File Offset: 0x000E95E8
		public bool IsAuthenticated
		{
			get
			{
				return this.isAuthenticated;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x000EB3F0 File Offset: 0x000E95F0
		public string Name
		{
			get
			{
				return this.userName;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x000EB3F8 File Offset: 0x000E95F8
		public PSCertificateDetails CertificateDetails
		{
			get
			{
				return this.certDetails;
			}
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x000EB400 File Offset: 0x000E9600
		public PSIdentity(string authType, bool isAuthenticated, string userName, PSCertificateDetails cert)
		{
			this.authenticationType = authType;
			this.isAuthenticated = isAuthenticated;
			this.userName = userName;
			this.certDetails = cert;
		}

		// Token: 0x04001510 RID: 5392
		private string authenticationType;

		// Token: 0x04001511 RID: 5393
		private bool isAuthenticated;

		// Token: 0x04001512 RID: 5394
		private string userName;

		// Token: 0x04001513 RID: 5395
		private PSCertificateDetails certDetails;
	}
}
