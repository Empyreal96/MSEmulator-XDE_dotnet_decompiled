using System;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Management.Automation
{
	// Token: 0x020007FB RID: 2043
	internal static class SignatureHelper
	{
		// Token: 0x06004DC5 RID: 19909 RVA: 0x00197F2C File Offset: 0x0019612C
		[ArchitectureSensitive]
		internal static Signature SignFile(SigningOption option, string fileName, X509Certificate2 certificate, string timeStampServerUrl, string hashAlgorithm)
		{
			Signature result = null;
			IntPtr intPtr = IntPtr.Zero;
			uint num = 0U;
			string hashAlgorithm2 = null;
			Utils.CheckArgForNullOrEmpty(fileName, "fileName");
			Utils.CheckArgForNull(certificate, "certificate");
			if (!string.IsNullOrEmpty(timeStampServerUrl) && (timeStampServerUrl.Length <= 7 || timeStampServerUrl.IndexOf("http://", StringComparison.OrdinalIgnoreCase) != 0))
			{
				throw PSTraceSource.NewArgumentException("certificate", Authenticode.TimeStampUrlRequired, new object[0]);
			}
			if (!string.IsNullOrEmpty(hashAlgorithm))
			{
				IntPtr pvKey = Marshal.StringToHGlobalUni(hashAlgorithm);
				IntPtr intPtr2 = NativeMethods.CryptFindOIDInfo(2U, pvKey, 0U);
				if (intPtr2 == IntPtr.Zero)
				{
					throw PSTraceSource.NewArgumentException("certificate", Authenticode.InvalidHashAlgorithm, new object[0]);
				}
				hashAlgorithm2 = ClrFacade.PtrToStructure<NativeMethods.CRYPT_OID_INFO>(intPtr2).pszOID;
			}
			if (!SecuritySupport.CertIsGoodForSigning(certificate))
			{
				throw PSTraceSource.NewArgumentException("certificate", Authenticode.CertNotGoodForSigning, new object[0]);
			}
			SecuritySupport.CheckIfFileExists(fileName);
			try
			{
				string timeStampServerUrl2 = null;
				if (!string.IsNullOrEmpty(timeStampServerUrl))
				{
					timeStampServerUrl2 = timeStampServerUrl;
				}
				NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_INFO cryptui_WIZ_DIGITAL_SIGN_INFO = NativeMethods.InitSignInfoStruct(fileName, certificate, timeStampServerUrl2, hashAlgorithm2, option);
				intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cryptui_WIZ_DIGITAL_SIGN_INFO));
				Marshal.StructureToPtr(cryptui_WIZ_DIGITAL_SIGN_INFO, intPtr, false);
				bool flag = NativeMethods.CryptUIWizDigitalSign(1U, IntPtr.Zero, IntPtr.Zero, intPtr, IntPtr.Zero);
				ClrFacade.DestroyStructure<NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_EXTENDED_INFO>(cryptui_WIZ_DIGITAL_SIGN_INFO.pSignExtInfo);
				Marshal.FreeCoTaskMem(cryptui_WIZ_DIGITAL_SIGN_INFO.pSignExtInfo);
				if (!flag)
				{
					num = SignatureHelper.GetLastWin32Error();
					if (num == 2147500037U || num == 2147942401U || num == 2147954407U)
					{
						flag = true;
					}
					else
					{
						if (num == 2148073480U)
						{
							throw PSTraceSource.NewArgumentException("certificate", Authenticode.InvalidHashAlgorithm, new object[0]);
						}
						SignatureHelper.tracer.TraceError("CryptUIWizDigitalSign: failed: {0:x}", new object[]
						{
							num
						});
					}
				}
				if (flag)
				{
					result = SignatureHelper.GetSignature(fileName, null);
				}
				else
				{
					result = new Signature(fileName, num);
				}
			}
			finally
			{
				ClrFacade.DestroyStructure<NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_INFO>(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x00198110 File Offset: 0x00196310
		[ArchitectureSensitive]
		internal static Signature GetSignature(string fileName, string fileContent)
		{
			Signature signature = SignatureHelper.GetSignatureFromCatalog(fileName);
			if (signature == null || signature.Status != SignatureStatus.Valid)
			{
				signature = SignatureHelper.GetSignatureFromWinVerifyTrust(fileName, fileContent);
			}
			return signature;
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x0019813C File Offset: 0x0019633C
		private static Signature GetSignatureFromCatalog(string filename)
		{
			if (Signature.CatalogApiAvailable != null && !Signature.CatalogApiAvailable.Value)
			{
				return null;
			}
			Signature signature = null;
			Utils.CheckArgForNullOrEmpty(filename, "fileName");
			SecuritySupport.CheckIfFileExists(filename);
			try
			{
				using (FileStream fileStream = File.OpenRead(filename))
				{
					NativeMethods.SIGNATURE_INFO signature_INFO = default(NativeMethods.SIGNATURE_INFO);
					signature_INFO.cbSize = (uint)Marshal.SizeOf(signature_INFO);
					IntPtr zero = IntPtr.Zero;
					IntPtr zero2 = IntPtr.Zero;
					try
					{
						int hresult = NativeMethods.WTGetSignatureInfo(filename, fileStream.SafeFileHandle.DangerousGetHandle(), (NativeMethods.SIGNATURE_INFO_FLAGS)14339, ref signature_INFO, ref zero, ref zero2);
						if (Utils.Succeeded(hresult))
						{
							uint errorFromSignatureState = SignatureHelper.GetErrorFromSignatureState(signature_INFO.nSignatureState);
							if (zero != IntPtr.Zero)
							{
								X509Certificate2 signer = new X509Certificate2(zero);
								signature = new Signature(filename, errorFromSignatureState, signer);
								switch (signature_INFO.nSignatureType)
								{
								case NativeMethods.SIGNATURE_INFO_TYPE.SIT_AUTHENTICODE:
									signature.SignatureType = SignatureType.Authenticode;
									break;
								case NativeMethods.SIGNATURE_INFO_TYPE.SIT_CATALOG:
									signature.SignatureType = SignatureType.Catalog;
									break;
								}
								if (signature_INFO.fOSBinary == 1)
								{
									signature.IsOSBinary = true;
								}
							}
							else
							{
								signature = new Signature(filename, errorFromSignatureState);
							}
							if (Signature.CatalogApiAvailable == null)
							{
								string text = Path.Combine(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID), "Modules\\Microsoft.PowerShell.Utility\\Microsoft.PowerShell.Utility.psm1");
								if (signature.Status != SignatureStatus.Valid)
								{
									if (string.Equals(filename, text, StringComparison.OrdinalIgnoreCase))
									{
										Signature.CatalogApiAvailable = new bool?(false);
									}
									else
									{
										Signature signatureFromCatalog = SignatureHelper.GetSignatureFromCatalog(text);
										Signature.CatalogApiAvailable = new bool?(signatureFromCatalog != null && signatureFromCatalog.Status == SignatureStatus.Valid);
									}
								}
							}
						}
					}
					finally
					{
						if (zero2 != IntPtr.Zero)
						{
							NativeMethods.FreeWVTStateData(zero2);
						}
						if (zero != IntPtr.Zero)
						{
							NativeMethods.CertFreeCertificateContext(zero);
						}
					}
				}
			}
			catch (TypeLoadException)
			{
				Signature.CatalogApiAvailable = new bool?(false);
				return null;
			}
			return signature;
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x00198348 File Offset: 0x00196548
		private static uint GetErrorFromSignatureState(NativeMethods.SIGNATURE_STATE state)
		{
			switch (state)
			{
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_UNSIGNED_MISSING:
				return 2148204800U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_UNSIGNED_UNSUPPORTED:
				return 2148204800U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_UNSIGNED_POLICY:
				return 2148204800U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_INVALID_CORRUPT:
				return 2148098064U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_INVALID_POLICY:
				return 2148081677U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_VALID:
				return 0U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_TRUSTED:
				return 0U;
			case NativeMethods.SIGNATURE_STATE.SIGNATURE_STATE_UNTRUSTED:
				return 2148204817U;
			default:
				return 2148204800U;
			}
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x001983AC File Offset: 0x001965AC
		private static Signature GetSignatureFromWinVerifyTrust(string fileName, string fileContent)
		{
			Signature result = null;
			Utils.CheckArgForNullOrEmpty(fileName, "fileName");
			SecuritySupport.CheckIfFileExists(fileName);
			try
			{
				NativeMethods.WINTRUST_DATA wtd;
				uint num = SignatureHelper.GetWinTrustData(fileName, fileContent, out wtd);
				if (num != 0U)
				{
					SignatureHelper.tracer.WriteLine("GetWinTrustData failed: {0:x}", new object[]
					{
						num
					});
				}
				result = SignatureHelper.GetSignatureFromWintrustData(fileName, num, wtd);
				num = NativeMethods.DestroyWintrustDataStruct(wtd);
				if (num != 0U)
				{
					SignatureHelper.tracer.WriteLine("DestroyWinTrustDataStruct failed: {0:x}", new object[]
					{
						num
					});
				}
			}
			catch (AccessViolationException)
			{
				result = new Signature(fileName, 2148204800U);
			}
			return result;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x00198458 File Offset: 0x00196658
		[ArchitectureSensitive]
		private static uint GetWinTrustData(string fileName, string fileContent, out NativeMethods.WINTRUST_DATA wtData)
		{
			uint result = 2147500037U;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Guid guid = new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");
			try
			{
				intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(guid));
				Marshal.StructureToPtr(guid, intPtr, false);
				NativeMethods.WINTRUST_DATA wintrust_DATA;
				if (fileContent == null)
				{
					NativeMethods.WINTRUST_FILE_INFO wfi = NativeMethods.InitWintrustFileInfoStruct(fileName);
					wintrust_DATA = NativeMethods.InitWintrustDataStructFromFile(wfi);
				}
				else
				{
					NativeMethods.WINTRUST_BLOB_INFO wbi = NativeMethods.InitWintrustBlobInfoStruct(fileName, fileContent);
					wintrust_DATA = NativeMethods.InitWintrustDataStructFromBlob(wbi);
				}
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(wintrust_DATA));
				Marshal.StructureToPtr(wintrust_DATA, intPtr2, false);
				result = NativeMethods.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
				wtData = ClrFacade.PtrToStructure<NativeMethods.WINTRUST_DATA>(intPtr2);
			}
			finally
			{
				ClrFacade.DestroyStructure<Guid>(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
				ClrFacade.DestroyStructure<NativeMethods.WINTRUST_DATA>(intPtr2);
				Marshal.FreeCoTaskMem(intPtr2);
			}
			return result;
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x00198530 File Offset: 0x00196730
		[ArchitectureSensitive]
		private static X509Certificate2 GetCertFromChain(IntPtr pSigner)
		{
			X509Certificate2 result = null;
			IntPtr intPtr = NativeMethods.WTHelperGetProvCertFromChain(pSigner, 0U);
			if (intPtr != IntPtr.Zero)
			{
				result = new X509Certificate2(ClrFacade.PtrToStructure<NativeMethods.CRYPT_PROVIDER_CERT>(intPtr).pCert);
			}
			return result;
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x0019856C File Offset: 0x0019676C
		[ArchitectureSensitive]
		private static Signature GetSignatureFromWintrustData(string filePath, uint error, NativeMethods.WINTRUST_DATA wtd)
		{
			Signature signature = null;
			X509Certificate2 x509Certificate = null;
			SignatureHelper.tracer.WriteLine("GetSignatureFromWintrustData: error: {0}", new object[]
			{
				error
			});
			IntPtr intPtr = NativeMethods.WTHelperProvDataFromStateData(wtd.hWVTStateData);
			if (intPtr != IntPtr.Zero)
			{
				IntPtr intPtr2 = NativeMethods.WTHelperGetProvSignerFromChain(intPtr, 0U, 0U, 0U);
				if (intPtr2 != IntPtr.Zero)
				{
					X509Certificate2 certFromChain = SignatureHelper.GetCertFromChain(intPtr2);
					if (certFromChain != null)
					{
						NativeMethods.CRYPT_PROVIDER_SGNR crypt_PROVIDER_SGNR = ClrFacade.PtrToStructure<NativeMethods.CRYPT_PROVIDER_SGNR>(intPtr2);
						if (crypt_PROVIDER_SGNR.csCounterSigners == 1U)
						{
							x509Certificate = SignatureHelper.GetCertFromChain(crypt_PROVIDER_SGNR.pasCounterSigners);
						}
						if (x509Certificate != null)
						{
							signature = new Signature(filePath, error, certFromChain, x509Certificate);
						}
						else
						{
							signature = new Signature(filePath, error, certFromChain);
						}
						signature.SignatureType = SignatureType.Authenticode;
					}
				}
			}
			if (signature == null && error != 0U)
			{
				signature = new Signature(filePath, error);
			}
			return signature;
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x00198630 File Offset: 0x00196830
		[ArchitectureSensitive]
		private static uint GetLastWin32Error()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			return SecuritySupport.GetDWORDFromInt(lastWin32Error);
		}

		// Token: 0x04002837 RID: 10295
		[TraceSource("SignatureHelper", "tracer for SignatureHelper")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("SignatureHelper", "tracer for SignatureHelper");
	}
}
