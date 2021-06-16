using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x0200080B RID: 2059
	internal static class CmsUtils
	{
		// Token: 0x06004F71 RID: 20337 RVA: 0x001A53BC File Offset: 0x001A35BC
		internal static string Encrypt(byte[] contentBytes, CmsMessageRecipient[] recipients, SessionState sessionState, out ErrorRecord error)
		{
			error = null;
			if (contentBytes == null || contentBytes.Length == 0)
			{
				return string.Empty;
			}
			ContentInfo contentInfo = new ContentInfo(contentBytes);
			EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo, new AlgorithmIdentifier(Oid.FromOidValue("2.16.840.1.101.3.4.1.42", OidGroup.EncryptionAlgorithm)));
			CmsRecipientCollection cmsRecipientCollection = new CmsRecipientCollection();
			foreach (CmsMessageRecipient cmsMessageRecipient in recipients)
			{
				if (cmsMessageRecipient.Certificates != null && cmsMessageRecipient.Certificates.Count == 0)
				{
					cmsMessageRecipient.Resolve(sessionState, ResolutionPurpose.Encryption, out error);
				}
				if (error != null)
				{
					return null;
				}
				foreach (X509Certificate2 certificate in cmsMessageRecipient.Certificates)
				{
					cmsRecipientCollection.Add(new CmsRecipient(certificate));
				}
			}
			envelopedCms.Encrypt(cmsRecipientCollection);
			byte[] bytes = envelopedCms.Encode();
			return CmsUtils.GetAsciiArmor(bytes);
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x001A5490 File Offset: 0x001A3690
		internal static string GetAsciiArmor(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(CmsUtils.BEGIN_CMS_SIGIL);
			string value = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
			stringBuilder.AppendLine(value);
			stringBuilder.Append(CmsUtils.END_CMS_SIGIL);
			return stringBuilder.ToString();
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001A54D4 File Offset: 0x001A36D4
		internal static byte[] RemoveAsciiArmor(string actualContent, string beginMarker, string endMarker, out int startIndex, out int endIndex)
		{
			startIndex = -1;
			endIndex = -1;
			startIndex = actualContent.IndexOf(beginMarker, StringComparison.OrdinalIgnoreCase);
			if (startIndex < 0)
			{
				return null;
			}
			endIndex = actualContent.IndexOf(endMarker, startIndex, StringComparison.OrdinalIgnoreCase) + endMarker.Length;
			if (endIndex < endMarker.Length)
			{
				return null;
			}
			int num = startIndex + beginMarker.Length;
			int num2 = endIndex - endMarker.Length;
			string text = actualContent.Substring(num, num2 - num);
			text = Regex.Replace(text, "\\s", "");
			return Convert.FromBase64String(text);
		}

		// Token: 0x04002899 RID: 10393
		internal static string BEGIN_CMS_SIGIL = "-----BEGIN CMS-----";

		// Token: 0x0400289A RID: 10394
		internal static string END_CMS_SIGIL = "-----END CMS-----";

		// Token: 0x0400289B RID: 10395
		internal static string BEGIN_CERTIFICATE_SIGIL = "-----BEGIN CERTIFICATE-----";

		// Token: 0x0400289C RID: 10396
		internal static string END_CERTIFICATE_SIGIL = "-----END CERTIFICATE-----";
	}
}
