using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000809 RID: 2057
	internal sealed class CertificateFilterInfo
	{
		// Token: 0x06004F67 RID: 20327 RVA: 0x001A51B4 File Offset: 0x001A33B4
		internal CertificateFilterInfo()
		{
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x001A51C3 File Offset: 0x001A33C3
		// (set) Token: 0x06004F69 RID: 20329 RVA: 0x001A51CB File Offset: 0x001A33CB
		internal CertificatePurpose Purpose
		{
			get
			{
				return this.purpose;
			}
			set
			{
				this.purpose = value;
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06004F6A RID: 20330 RVA: 0x001A51D4 File Offset: 0x001A33D4
		// (set) Token: 0x06004F6B RID: 20331 RVA: 0x001A51DC File Offset: 0x001A33DC
		internal bool SSLServerAuthentication
		{
			get
			{
				return this.sslServerAuthentication;
			}
			set
			{
				this.sslServerAuthentication = value;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (set) Token: 0x06004F6C RID: 20332 RVA: 0x001A51E5 File Offset: 0x001A33E5
		internal string DnsName
		{
			set
			{
				this.dnsName = value;
			}
		}

		// Token: 0x17001024 RID: 4132
		// (set) Token: 0x06004F6D RID: 20333 RVA: 0x001A51EE File Offset: 0x001A33EE
		internal string[] Eku
		{
			set
			{
				this.eku = value;
			}
		}

		// Token: 0x17001025 RID: 4133
		// (set) Token: 0x06004F6E RID: 20334 RVA: 0x001A51F7 File Offset: 0x001A33F7
		internal int ExpiringInDays
		{
			set
			{
				this.expiringInDays = value;
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06004F6F RID: 20335 RVA: 0x001A5200 File Offset: 0x001A3400
		internal string FilterString
		{
			get
			{
				string text = "";
				if (this.dnsName != null)
				{
					text = this.AppendFilter(text, "dns", this.dnsName);
				}
				string text2 = "";
				if (this.eku != null)
				{
					for (int i = 0; i < this.eku.Length; i++)
					{
						if (text2.Length != 0)
						{
							text2 += ",";
						}
						text2 += this.eku[i];
					}
				}
				if (this.purpose == CertificatePurpose.CodeSigning)
				{
					if (text2.Length != 0)
					{
						text2 += ",";
					}
					text2 += "1.3.6.1.5.5.7.3.3";
				}
				if (this.purpose == CertificatePurpose.DocumentEncryption)
				{
					if (text2.Length != 0)
					{
						text2 += ",";
					}
					text2 += "1.3.6.1.4.1.311.80.1";
				}
				if (this.sslServerAuthentication)
				{
					if (text2.Length != 0)
					{
						text2 += ",";
					}
					text2 += "1.3.6.1.5.5.7.3.1";
				}
				if (text2.Length != 0)
				{
					text = this.AppendFilter(text, "eku", text2);
					if (this.purpose == CertificatePurpose.CodeSigning || this.sslServerAuthentication)
					{
						text = this.AppendFilter(text, "key", "*");
					}
				}
				if (this.expiringInDays >= 0)
				{
					text = this.AppendFilter(text, "ExpiringInDays", this.expiringInDays.ToString(CultureInfo.InvariantCulture));
				}
				if (text.Length == 0)
				{
					text = null;
				}
				return text;
			}
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x001A5358 File Offset: 0x001A3558
		private string AppendFilter(string filterString, string name, string value)
		{
			string text = value;
			if (text.Length != 0)
			{
				if (text.Contains("=") || text.Contains("&"))
				{
					throw Marshal.GetExceptionForHR(-2147024883);
				}
				text = name + "=" + text;
				if (filterString.Length != 0)
				{
					text = "&" + text;
				}
			}
			return filterString + text;
		}

		// Token: 0x0400288C RID: 10380
		internal const string CodeSigningOid = "1.3.6.1.5.5.7.3.3";

		// Token: 0x0400288D RID: 10381
		internal const string szOID_PKIX_KP_SERVER_AUTH = "1.3.6.1.5.5.7.3.1";

		// Token: 0x0400288E RID: 10382
		internal const string DocumentEncryptionOid = "1.3.6.1.4.1.311.80.1";

		// Token: 0x0400288F RID: 10383
		private CertificatePurpose purpose;

		// Token: 0x04002890 RID: 10384
		private bool sslServerAuthentication;

		// Token: 0x04002891 RID: 10385
		private string dnsName;

		// Token: 0x04002892 RID: 10386
		private string[] eku;

		// Token: 0x04002893 RID: 10387
		private int expiringInDays = -1;
	}
}
