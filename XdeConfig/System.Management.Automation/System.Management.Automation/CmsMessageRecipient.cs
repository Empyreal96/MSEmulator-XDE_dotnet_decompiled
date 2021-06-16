using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x0200080C RID: 2060
	public class CmsMessageRecipient
	{
		// Token: 0x06004F75 RID: 20341 RVA: 0x001A557F File Offset: 0x001A377F
		internal CmsMessageRecipient()
		{
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x001A5587 File Offset: 0x001A3787
		public CmsMessageRecipient(string identifier)
		{
			this.identifier = identifier;
			this.Certificates = new X509Certificate2Collection();
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x001A55A1 File Offset: 0x001A37A1
		public CmsMessageRecipient(X509Certificate2 certificate)
		{
			this.pendingCertificate = certificate;
			this.Certificates = new X509Certificate2Collection();
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06004F78 RID: 20344 RVA: 0x001A55BB File Offset: 0x001A37BB
		// (set) Token: 0x06004F79 RID: 20345 RVA: 0x001A55C3 File Offset: 0x001A37C3
		public X509Certificate2Collection Certificates { get; internal set; }

		// Token: 0x06004F7A RID: 20346 RVA: 0x001A55CC File Offset: 0x001A37CC
		public void Resolve(SessionState sessionState, ResolutionPurpose purpose, out ErrorRecord error)
		{
			error = null;
			if (this.pendingCertificate != null)
			{
				this.ProcessResolvedCertificates(purpose, new List<X509Certificate2>
				{
					this.pendingCertificate
				}, out error);
				if (error != null || this.Certificates.Count != 0)
				{
					return;
				}
			}
			if (this.identifier != null)
			{
				this.ResolveFromBase64Encoding(purpose, out error);
				if (error != null || this.Certificates.Count != 0)
				{
					return;
				}
				this.ResolveFromPath(sessionState, purpose, out error);
				if (error != null || this.Certificates.Count != 0)
				{
					return;
				}
				this.ResolveFromThumbprint(sessionState, purpose, out error);
				if (error != null || this.Certificates.Count != 0)
				{
					return;
				}
				this.ResolveFromSubjectName(sessionState, purpose, out error);
				if (error != null || this.Certificates.Count != 0)
				{
					return;
				}
			}
			if (purpose == ResolutionPurpose.Encryption || !WildcardPattern.ContainsWildcardCharacters(this.identifier))
			{
				error = new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SecuritySupportStrings.NoCertificateFound, new object[]
				{
					this.identifier
				})), "NoCertificateFound", ErrorCategory.ObjectNotFound, this.identifier);
			}
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001A56D0 File Offset: 0x001A38D0
		private void ResolveFromBase64Encoding(ResolutionPurpose purpose, out ErrorRecord error)
		{
			error = null;
			byte[] array = null;
			try
			{
				int num;
				int num2;
				array = CmsUtils.RemoveAsciiArmor(this.identifier, CmsUtils.BEGIN_CERTIFICATE_SIGIL, CmsUtils.END_CERTIFICATE_SIGIL, out num, out num2);
			}
			catch (FormatException)
			{
				return;
			}
			if (array != null)
			{
				List<X509Certificate2> list = new List<X509Certificate2>();
				try
				{
					X509Certificate2 item = new X509Certificate2(array);
					list.Add(item);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					return;
				}
				this.ProcessResolvedCertificates(purpose, list, out error);
				return;
			}
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x001A5750 File Offset: 0x001A3950
		private void ResolveFromPath(SessionState sessionState, ResolutionPurpose purpose, out ErrorRecord error)
		{
			error = null;
			ProviderInfo providerInfo = null;
			Collection<string> collection = null;
			try
			{
				collection = sessionState.Path.GetResolvedProviderPathFromPSPath(this.identifier, out providerInfo);
			}
			catch (SessionStateException)
			{
			}
			if (collection != null && collection.Count != 0)
			{
				if (!string.Equals(providerInfo.Name, "FileSystem", StringComparison.OrdinalIgnoreCase))
				{
					error = new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SecuritySupportStrings.CertificatePathMustBeFileSystemPath, new object[]
					{
						this.identifier
					})), "CertificatePathMustBeFileSystemPath", ErrorCategory.ObjectNotFound, providerInfo);
					return;
				}
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				foreach (string text in collection)
				{
					if (Directory.Exists(text))
					{
						list.AddRange(Directory.GetFiles(text));
						list2.Add(text);
					}
				}
				foreach (string item in list)
				{
					collection.Add(item);
				}
				foreach (string item2 in list2)
				{
					collection.Remove(item2);
				}
				List<X509Certificate2> list3 = new List<X509Certificate2>();
				foreach (string fileName in collection)
				{
					X509Certificate2 item3 = null;
					try
					{
						item3 = new X509Certificate2(fileName);
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
						continue;
					}
					list3.Add(item3);
				}
				this.ProcessResolvedCertificates(purpose, list3, out error);
			}
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x001A5940 File Offset: 0x001A3B40
		private void ResolveFromThumbprint(SessionState sessionState, ResolutionPurpose purpose, out ErrorRecord error)
		{
			if (!Regex.IsMatch(this.identifier, "^[a-f0-9]+$", RegexOptions.IgnoreCase))
			{
				error = null;
				return;
			}
			Collection<PSObject> collection = new Collection<PSObject>();
			try
			{
				string path = sessionState.Path.Combine("Microsoft.PowerShell.Security\\Certificate::CurrentUser\\My", this.identifier);
				if (sessionState.InvokeProvider.Item.Exists(path))
				{
					foreach (PSObject item in sessionState.InvokeProvider.Item.Get(path))
					{
						collection.Add(item);
					}
				}
				path = sessionState.Path.Combine("Microsoft.PowerShell.Security\\Certificate::LocalMachine\\My", this.identifier);
				if (sessionState.InvokeProvider.Item.Exists(path))
				{
					foreach (PSObject item2 in sessionState.InvokeProvider.Item.Get(path))
					{
						collection.Add(item2);
					}
				}
			}
			catch (SessionStateException)
			{
			}
			List<X509Certificate2> list = new List<X509Certificate2>();
			foreach (PSObject psobject in collection)
			{
				X509Certificate2 x509Certificate = psobject.BaseObject as X509Certificate2;
				if (x509Certificate != null)
				{
					list.Add(x509Certificate);
				}
			}
			this.ProcessResolvedCertificates(purpose, list, out error);
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x001A5AD0 File Offset: 0x001A3CD0
		private void ResolveFromSubjectName(SessionState sessionState, ResolutionPurpose purpose, out ErrorRecord error)
		{
			Collection<PSObject> collection = new Collection<PSObject>();
			WildcardPattern wildcardPattern = new WildcardPattern(this.identifier, WildcardOptions.IgnoreCase);
			try
			{
				string[] array = new string[]
				{
					"Microsoft.PowerShell.Security\\Certificate::CurrentUser\\My",
					"Microsoft.PowerShell.Security\\Certificate::LocalMachine\\My"
				};
				foreach (string path in array)
				{
					foreach (PSObject psobject in sessionState.InvokeProvider.ChildItem.Get(path, false))
					{
						if (wildcardPattern.IsMatch(psobject.Properties["Subject"].Value.ToString()))
						{
							collection.Add(psobject);
						}
					}
				}
			}
			catch (SessionStateException)
			{
			}
			List<X509Certificate2> list = new List<X509Certificate2>();
			foreach (PSObject psobject2 in collection)
			{
				X509Certificate2 x509Certificate = psobject2.BaseObject as X509Certificate2;
				if (x509Certificate != null)
				{
					list.Add(x509Certificate);
				}
			}
			this.ProcessResolvedCertificates(purpose, list, out error);
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x001A5C14 File Offset: 0x001A3E14
		private void ProcessResolvedCertificates(ResolutionPurpose purpose, List<X509Certificate2> certificatesToProcess, out ErrorRecord error)
		{
			error = null;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (X509Certificate2 x509Certificate in certificatesToProcess)
			{
				if (!SecuritySupport.CertIsGoodForEncryption(x509Certificate))
				{
					if (!WildcardPattern.ContainsWildcardCharacters(this.identifier))
					{
						error = new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SecuritySupportStrings.CertificateCannotBeUsedForEncryption, new object[]
						{
							x509Certificate.Thumbprint,
							"1.3.6.1.4.1.311.80.1"
						})), "CertificateCannotBeUsedForEncryption", ErrorCategory.InvalidData, x509Certificate);
						break;
					}
				}
				else if ((purpose != ResolutionPurpose.Decryption || x509Certificate.HasPrivateKey) && !hashSet.Contains(x509Certificate.Thumbprint))
				{
					hashSet.Add(x509Certificate.Thumbprint);
					if (purpose == ResolutionPurpose.Encryption && this.Certificates.Count > 0)
					{
						error = new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SecuritySupportStrings.IdentifierMustReferenceSingleCertificate, new object[]
						{
							this.identifier,
							"To"
						})), "IdentifierMustReferenceSingleCertificate", ErrorCategory.LimitsExceeded, certificatesToProcess);
						this.Certificates.Clear();
						break;
					}
					this.Certificates.Add(x509Certificate);
				}
			}
		}

		// Token: 0x0400289D RID: 10397
		private string identifier;

		// Token: 0x0400289E RID: 10398
		private X509Certificate2 pendingCertificate;
	}
}
