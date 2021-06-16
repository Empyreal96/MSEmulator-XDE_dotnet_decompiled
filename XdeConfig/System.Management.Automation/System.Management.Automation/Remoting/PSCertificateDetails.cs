using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200035B RID: 859
	public sealed class PSCertificateDetails
	{
		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x000EB425 File Offset: 0x000E9625
		public string Subject
		{
			get
			{
				return this.subject;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x000EB42D File Offset: 0x000E962D
		public string IssuerName
		{
			get
			{
				return this.issuerName;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002AA7 RID: 10919 RVA: 0x000EB435 File Offset: 0x000E9635
		public string IssuerThumbprint
		{
			get
			{
				return this.issuerThumbprint;
			}
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000EB43D File Offset: 0x000E963D
		public PSCertificateDetails(string subject, string issuerName, string issuerThumbprint)
		{
			this.subject = subject;
			this.issuerName = issuerName;
			this.issuerThumbprint = issuerThumbprint;
		}

		// Token: 0x04001514 RID: 5396
		private string subject;

		// Token: 0x04001515 RID: 5397
		private string issuerName;

		// Token: 0x04001516 RID: 5398
		private string issuerThumbprint;
	}
}
