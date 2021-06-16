using System;
using System.ComponentModel;
using System.IO;
using System.Management.Automation.Internal;
using System.Security.Cryptography.X509Certificates;

namespace System.Management.Automation
{
	// Token: 0x02000804 RID: 2052
	public sealed class Signature
	{
		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06004F42 RID: 20290 RVA: 0x001A45D1 File Offset: 0x001A27D1
		public X509Certificate2 SignerCertificate
		{
			get
			{
				return this.signerCert;
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06004F43 RID: 20291 RVA: 0x001A45D9 File Offset: 0x001A27D9
		public X509Certificate2 TimeStamperCertificate
		{
			get
			{
				return this.timeStamperCert;
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06004F44 RID: 20292 RVA: 0x001A45E1 File Offset: 0x001A27E1
		public SignatureStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06004F45 RID: 20293 RVA: 0x001A45E9 File Offset: 0x001A27E9
		public string StatusMessage
		{
			get
			{
				return this.statusMessage;
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06004F46 RID: 20294 RVA: 0x001A45F1 File Offset: 0x001A27F1
		public string Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06004F47 RID: 20295 RVA: 0x001A45F9 File Offset: 0x001A27F9
		// (set) Token: 0x06004F48 RID: 20296 RVA: 0x001A4601 File Offset: 0x001A2801
		public SignatureType SignatureType { get; internal set; }

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06004F49 RID: 20297 RVA: 0x001A460A File Offset: 0x001A280A
		// (set) Token: 0x06004F4A RID: 20298 RVA: 0x001A4612 File Offset: 0x001A2812
		public bool IsOSBinary { get; internal set; }

		// Token: 0x06004F4B RID: 20299 RVA: 0x001A461C File Offset: 0x001A281C
		internal Signature(string filePath, uint error, X509Certificate2 signer, X509Certificate2 timestamper)
		{
			Utils.CheckArgForNullOrEmpty(filePath, "filePath");
			Utils.CheckArgForNull(signer, "signer");
			Utils.CheckArgForNull(timestamper, "timestamper");
			this.Init(filePath, signer, error, timestamper);
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x001A466E File Offset: 0x001A286E
		internal Signature(string filePath, X509Certificate2 signer)
		{
			Utils.CheckArgForNullOrEmpty(filePath, "filePath");
			Utils.CheckArgForNull(signer, "signer");
			this.Init(filePath, signer, 0U, null);
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x001A46A8 File Offset: 0x001A28A8
		internal Signature(string filePath, uint error, X509Certificate2 signer)
		{
			Utils.CheckArgForNullOrEmpty(filePath, "filePath");
			Utils.CheckArgForNull(signer, "signer");
			this.Init(filePath, signer, error, null);
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x001A46E2 File Offset: 0x001A28E2
		internal Signature(string filePath, uint error)
		{
			Utils.CheckArgForNullOrEmpty(filePath, "filePath");
			this.Init(filePath, null, error, null);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x001A4714 File Offset: 0x001A2914
		private void Init(string filePath, X509Certificate2 signer, uint error, X509Certificate2 timestamper)
		{
			this.path = filePath;
			this.win32Error = error;
			this.signerCert = signer;
			this.timeStamperCert = timestamper;
			this.SignatureType = SignatureType.None;
			SignatureStatus signatureStatusFromWin32Error = Signature.GetSignatureStatusFromWin32Error(error);
			this.status = signatureStatusFromWin32Error;
			this.statusMessage = Signature.GetSignatureStatusMessage(signatureStatusFromWin32Error, error, filePath);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x001A4764 File Offset: 0x001A2964
		private static SignatureStatus GetSignatureStatusFromWin32Error(uint error)
		{
			SignatureStatus result = SignatureStatus.UnknownError;
			if (error <= 2148081677U)
			{
				if (error == 0U)
				{
					return SignatureStatus.Valid;
				}
				if (error == 2148073480U)
				{
					return SignatureStatus.Incompatible;
				}
				if (error != 2148081677U)
				{
					return result;
				}
			}
			else if (error <= 2148204545U)
			{
				if (error != 2148098064U)
				{
					if (error != 2148204545U)
					{
						return result;
					}
					return SignatureStatus.NotSupportedFileFormat;
				}
			}
			else
			{
				if (error == 2148204800U)
				{
					return SignatureStatus.NotSigned;
				}
				if (error != 2148204817U)
				{
					return result;
				}
				return SignatureStatus.NotTrusted;
			}
			result = SignatureStatus.HashMismatch;
			return result;
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x001A47D8 File Offset: 0x001A29D8
		private static string GetSignatureStatusMessage(SignatureStatus status, uint error, string filePath)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			switch (status)
			{
			case SignatureStatus.Valid:
				text2 = MshSignature.MshSignature_Valid;
				break;
			case SignatureStatus.UnknownError:
			{
				int intFromDWORD = SecuritySupport.GetIntFromDWORD(error);
				Win32Exception ex = new Win32Exception(intFromDWORD);
				text = ex.Message;
				break;
			}
			case SignatureStatus.NotSigned:
				text2 = MshSignature.MshSignature_NotSigned;
				text3 = filePath;
				break;
			case SignatureStatus.HashMismatch:
				text2 = MshSignature.MshSignature_HashMismatch;
				text3 = filePath;
				break;
			case SignatureStatus.NotTrusted:
				text2 = MshSignature.MshSignature_NotTrusted;
				text3 = filePath;
				break;
			case SignatureStatus.NotSupportedFileFormat:
				text2 = MshSignature.MshSignature_NotSupportedFileFormat;
				text3 = System.IO.Path.GetExtension(filePath);
				if (string.IsNullOrEmpty(text3))
				{
					text2 = MshSignature.MshSignature_NotSupportedFileFormat_NoExtension;
					text3 = null;
				}
				break;
			case SignatureStatus.Incompatible:
				if (error == 2148073480U)
				{
					text2 = MshSignature.MshSignature_Incompatible_HashAlgorithm;
				}
				else
				{
					text2 = MshSignature.MshSignature_Incompatible;
				}
				text3 = filePath;
				break;
			}
			if (text == null)
			{
				if (text3 == null)
				{
					text = text2;
				}
				else
				{
					text = StringUtil.Format(text2, text3);
				}
			}
			return text;
		}

		// Token: 0x04002871 RID: 10353
		private string path;

		// Token: 0x04002872 RID: 10354
		private SignatureStatus status = SignatureStatus.UnknownError;

		// Token: 0x04002873 RID: 10355
		private uint win32Error;

		// Token: 0x04002874 RID: 10356
		private X509Certificate2 signerCert;

		// Token: 0x04002875 RID: 10357
		private string statusMessage = string.Empty;

		// Token: 0x04002876 RID: 10358
		private X509Certificate2 timeStamperCert;

		// Token: 0x04002877 RID: 10359
		internal static bool? CatalogApiAvailable = null;
	}
}
