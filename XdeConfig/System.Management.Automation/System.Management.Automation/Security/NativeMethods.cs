using System;
using System.Management.Automation.Internal;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.Management.Automation.Security
{
	// Token: 0x020007B8 RID: 1976
	internal static class NativeMethods
	{
		// Token: 0x06004D7B RID: 19835
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertEnumSystemStore(NativeMethods.CertStoreFlags Flags, IntPtr notUsed1, IntPtr notUsed2, NativeMethods.CertEnumSystemStoreCallBackProto fn);

		// Token: 0x06004D7C RID: 19836
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CertEnumCertificatesInStore(IntPtr storeHandle, IntPtr certContext);

		// Token: 0x06004D7D RID: 19837
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CertFindCertificateInStore(IntPtr hCertStore, NativeMethods.CertOpenStoreEncodingType dwEncodingType, uint dwFindFlags, NativeMethods.CertFindType dwFindType, [MarshalAs(UnmanagedType.LPWStr)] string pvFindPara, IntPtr notUsed1);

		// Token: 0x06004D7E RID: 19838
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertCloseStore(IntPtr hCertStore, int dwFlags);

		// Token: 0x06004D7F RID: 19839
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertGetEnhancedKeyUsage(IntPtr pCertContext, uint dwFlags, IntPtr pUsage, out int pcbUsage);

		// Token: 0x06004D80 RID: 19840
		[DllImport("Crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CertOpenStore(NativeMethods.CertOpenStoreProvider storeProvider, NativeMethods.CertOpenStoreEncodingType dwEncodingType, IntPtr notUsed1, NativeMethods.CertOpenStoreFlags dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string storeName);

		// Token: 0x06004D81 RID: 19841
		[DllImport("Crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertControlStore(IntPtr hCertStore, uint dwFlags, NativeMethods.CertControlStoreType dwCtrlType, IntPtr pvCtrlPara);

		// Token: 0x06004D82 RID: 19842
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertDeleteCertificateFromStore(IntPtr pCertContext);

		// Token: 0x06004D83 RID: 19843
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

		// Token: 0x06004D84 RID: 19844
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertAddCertificateContextToStore(IntPtr hCertStore, IntPtr pCertContext, uint dwAddDisposition, ref IntPtr ppStoreContext);

		// Token: 0x06004D85 RID: 19845
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertFreeCertificateContext(IntPtr certContext);

		// Token: 0x06004D86 RID: 19846
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertGetCertificateContextProperty(IntPtr pCertContext, NativeMethods.CertPropertyId dwPropId, IntPtr pvData, ref int pcbData);

		// Token: 0x06004D87 RID: 19847
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CertSetCertificateContextProperty(IntPtr pCertContext, NativeMethods.CertPropertyId dwPropId, uint dwFlags, IntPtr pvData);

		// Token: 0x06004D88 RID: 19848
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern string CryptFindLocalizedName(string pwszCryptName);

		// Token: 0x06004D89 RID: 19849
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptAcquireContext(ref IntPtr hProv, string strContainerName, string strProviderName, int nProviderType, uint uiProviderFlags);

		// Token: 0x06004D8A RID: 19850
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptReleaseContext(IntPtr hProv, int dwFlags);

		// Token: 0x06004D8B RID: 19851
		[DllImport("advapi32.dll", SetLastError = true)]
		internal unsafe static extern bool CryptSetProvParam(IntPtr hProv, NativeMethods.ProviderParam dwParam, void* pbData, int dwFlags);

		// Token: 0x06004D8C RID: 19852
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
		internal static extern int NCryptOpenStorageProvider(ref IntPtr hProv, string strProviderName, uint dwFlags);

		// Token: 0x06004D8D RID: 19853
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
		internal static extern int NCryptOpenKey(IntPtr hProv, ref IntPtr hKey, string strKeyName, uint dwLegacySpec, uint dwFlags);

		// Token: 0x06004D8E RID: 19854
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
		internal unsafe static extern int NCryptSetProperty(IntPtr hProv, string pszProperty, void* pbInput, int cbInput, int dwFlags);

		// Token: 0x06004D8F RID: 19855
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
		internal static extern int NCryptDeleteKey(IntPtr hKey, uint dwFlags);

		// Token: 0x06004D90 RID: 19856
		[DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
		internal static extern int NCryptFreeObject(IntPtr hObject);

		// Token: 0x06004D91 RID: 19857
		[DllImport("cryptUI.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptUIWizDigitalSign(uint dwFlags, IntPtr hwndParentNotUsed, IntPtr pwszWizardTitleNotUsed, IntPtr pDigitalSignInfo, IntPtr ppSignContextNotUsed);

		// Token: 0x06004D92 RID: 19858 RVA: 0x00197984 File Offset: 0x00195B84
		[ArchitectureSensitive]
		internal static NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_EXTENDED_INFO InitSignInfoExtendedStruct(string description, string moreInfoUrl, string hashAlgorithm)
		{
			NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_EXTENDED_INFO cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO = default(NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_EXTENDED_INFO);
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.dwSize = (uint)Marshal.SizeOf(cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO);
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.dwAttrFlagsNotUsed = 0U;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.pwszDescription = description;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.pwszMoreInfoLocation = moreInfoUrl;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.pszHashAlg = null;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.pwszSigningCertDisplayStringNotUsed = IntPtr.Zero;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.hAdditionalCertStoreNotUsed = IntPtr.Zero;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.psAuthenticatedNotUsed = IntPtr.Zero;
			cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.psUnauthenticatedNotUsed = IntPtr.Zero;
			if (hashAlgorithm != null)
			{
				cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO.pszHashAlg = hashAlgorithm;
			}
			return cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO;
		}

		// Token: 0x06004D93 RID: 19859
		[DllImport("crypt32.dll")]
		internal static extern IntPtr CryptFindOIDInfo(uint dwKeyType, IntPtr pvKey, uint dwGroupId);

		// Token: 0x06004D94 RID: 19860 RVA: 0x00197A08 File Offset: 0x00195C08
		[ArchitectureSensitive]
		internal static uint GetCertChoiceFromSigningOption(SigningOption option)
		{
			uint result;
			switch (option)
			{
			case SigningOption.AddOnlyCertificate:
				result = 0U;
				break;
			case SigningOption.AddFullCertificateChain:
				result = 1U;
				break;
			case SigningOption.AddFullCertificateChainExceptRoot:
				result = 2U;
				break;
			default:
				result = 2U;
				break;
			}
			return result;
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x00197A3C File Offset: 0x00195C3C
		[ArchitectureSensitive]
		internal static NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_INFO InitSignInfoStruct(string fileName, X509Certificate2 signingCert, string timeStampServerUrl, string hashAlgorithm, SigningOption option)
		{
			NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_INFO cryptui_WIZ_DIGITAL_SIGN_INFO = default(NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_INFO);
			cryptui_WIZ_DIGITAL_SIGN_INFO.dwSize = (uint)Marshal.SizeOf(cryptui_WIZ_DIGITAL_SIGN_INFO);
			cryptui_WIZ_DIGITAL_SIGN_INFO.dwSubjectChoice = 1U;
			cryptui_WIZ_DIGITAL_SIGN_INFO.pwszFileName = fileName;
			cryptui_WIZ_DIGITAL_SIGN_INFO.dwSigningCertChoice = 1U;
			cryptui_WIZ_DIGITAL_SIGN_INFO.pSigningCertContext = signingCert.Handle;
			cryptui_WIZ_DIGITAL_SIGN_INFO.pwszTimestampURL = timeStampServerUrl;
			cryptui_WIZ_DIGITAL_SIGN_INFO.dwAdditionalCertChoice = NativeMethods.GetCertChoiceFromSigningOption(option);
			NativeMethods.CRYPTUI_WIZ_DIGITAL_SIGN_EXTENDED_INFO cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO = NativeMethods.InitSignInfoExtendedStruct("", "", hashAlgorithm);
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO));
			Marshal.StructureToPtr(cryptui_WIZ_DIGITAL_SIGN_EXTENDED_INFO, intPtr, false);
			cryptui_WIZ_DIGITAL_SIGN_INFO.pSignExtInfo = intPtr;
			return cryptui_WIZ_DIGITAL_SIGN_INFO;
		}

		// Token: 0x06004D96 RID: 19862
		[DllImport("wintrust.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint WinVerifyTrust(IntPtr hWndNotUsed, IntPtr pgActionID, IntPtr pWinTrustData);

		// Token: 0x06004D97 RID: 19863 RVA: 0x00197AD8 File Offset: 0x00195CD8
		[ArchitectureSensitive]
		internal static NativeMethods.WINTRUST_FILE_INFO InitWintrustFileInfoStruct(string fileName)
		{
			NativeMethods.WINTRUST_FILE_INFO wintrust_FILE_INFO = default(NativeMethods.WINTRUST_FILE_INFO);
			wintrust_FILE_INFO.cbStruct = (uint)Marshal.SizeOf(wintrust_FILE_INFO);
			wintrust_FILE_INFO.pcwszFilePath = fileName;
			wintrust_FILE_INFO.hFileNotUsed = IntPtr.Zero;
			wintrust_FILE_INFO.pgKnownSubjectNotUsed = IntPtr.Zero;
			return wintrust_FILE_INFO;
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x00197B28 File Offset: 0x00195D28
		[ArchitectureSensitive]
		internal static NativeMethods.WINTRUST_BLOB_INFO InitWintrustBlobInfoStruct(string fileName, string content)
		{
			NativeMethods.WINTRUST_BLOB_INFO wintrust_BLOB_INFO = default(NativeMethods.WINTRUST_BLOB_INFO);
			byte[] bytes = Encoding.Unicode.GetBytes(content);
			wintrust_BLOB_INFO.gSubject.Data1 = 1614531615U;
			wintrust_BLOB_INFO.gSubject.Data2 = 19289;
			wintrust_BLOB_INFO.gSubject.Data3 = 19976;
			wintrust_BLOB_INFO.gSubject.Data4 = new byte[]
			{
				183,
				36,
				210,
				198,
				41,
				126,
				243,
				81
			};
			wintrust_BLOB_INFO.cbStruct = (uint)Marshal.SizeOf(wintrust_BLOB_INFO);
			wintrust_BLOB_INFO.pcwszDisplayName = fileName;
			wintrust_BLOB_INFO.cbMemObject = (uint)bytes.Length;
			wintrust_BLOB_INFO.pbMemObject = Marshal.AllocCoTaskMem(bytes.Length);
			Marshal.Copy(bytes, 0, wintrust_BLOB_INFO.pbMemObject, bytes.Length);
			return wintrust_BLOB_INFO;
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x00197BE0 File Offset: 0x00195DE0
		[ArchitectureSensitive]
		internal static NativeMethods.WINTRUST_DATA InitWintrustDataStructFromFile(NativeMethods.WINTRUST_FILE_INFO wfi)
		{
			NativeMethods.WINTRUST_DATA wintrust_DATA = default(NativeMethods.WINTRUST_DATA);
			wintrust_DATA.cbStruct = (uint)Marshal.SizeOf(wintrust_DATA);
			wintrust_DATA.pPolicyCallbackData = IntPtr.Zero;
			wintrust_DATA.pSIPClientData = IntPtr.Zero;
			wintrust_DATA.dwUIChoice = 2U;
			wintrust_DATA.fdwRevocationChecks = 0U;
			wintrust_DATA.dwUnionChoice = 1U;
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(wfi));
			Marshal.StructureToPtr(wfi, intPtr, false);
			wintrust_DATA.Choice.pFile = intPtr;
			wintrust_DATA.dwStateAction = 1U;
			wintrust_DATA.hWVTStateData = IntPtr.Zero;
			wintrust_DATA.pwszURLReference = null;
			wintrust_DATA.dwProvFlags = 0U;
			return wintrust_DATA;
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x00197C88 File Offset: 0x00195E88
		[ArchitectureSensitive]
		internal static NativeMethods.WINTRUST_DATA InitWintrustDataStructFromBlob(NativeMethods.WINTRUST_BLOB_INFO wbi)
		{
			NativeMethods.WINTRUST_DATA result = default(NativeMethods.WINTRUST_DATA);
			result.cbStruct = (uint)Marshal.SizeOf(wbi);
			result.pPolicyCallbackData = IntPtr.Zero;
			result.pSIPClientData = IntPtr.Zero;
			result.dwUIChoice = 2U;
			result.fdwRevocationChecks = 0U;
			result.dwUnionChoice = 3U;
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(wbi));
			Marshal.StructureToPtr(wbi, intPtr, false);
			result.Choice.pBlob = intPtr;
			result.dwStateAction = 1U;
			result.hWVTStateData = IntPtr.Zero;
			result.pwszURLReference = null;
			result.dwProvFlags = 0U;
			return result;
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x00197D30 File Offset: 0x00195F30
		[ArchitectureSensitive]
		internal static uint DestroyWintrustDataStruct(NativeMethods.WINTRUST_DATA wtd)
		{
			uint result = 2147500037U;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Guid guid = new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");
			try
			{
				intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(guid));
				Marshal.StructureToPtr(guid, intPtr, false);
				wtd.dwStateAction = 2U;
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(wtd));
				Marshal.StructureToPtr(wtd, intPtr2, false);
				result = NativeMethods.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
				wtd = ClrFacade.PtrToStructure<NativeMethods.WINTRUST_DATA>(intPtr2);
			}
			finally
			{
				ClrFacade.DestroyStructure<NativeMethods.WINTRUST_DATA>(intPtr2);
				Marshal.FreeCoTaskMem(intPtr2);
				ClrFacade.DestroyStructure<Guid>(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
			}
			if (wtd.dwUnionChoice == 3U)
			{
				Marshal.FreeCoTaskMem(ClrFacade.PtrToStructure<NativeMethods.WINTRUST_BLOB_INFO>(wtd.Choice.pBlob).pbMemObject);
				ClrFacade.DestroyStructure<NativeMethods.WINTRUST_BLOB_INFO>(wtd.Choice.pBlob);
				Marshal.FreeCoTaskMem(wtd.Choice.pBlob);
			}
			else
			{
				ClrFacade.DestroyStructure<NativeMethods.WINTRUST_FILE_INFO>(wtd.Choice.pFile);
				Marshal.FreeCoTaskMem(wtd.Choice.pFile);
			}
			return result;
		}

		// Token: 0x06004D9C RID: 19868
		[DllImport("wintrust.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr WTHelperProvDataFromStateData(IntPtr hStateData);

		// Token: 0x06004D9D RID: 19869
		[DllImport("wintrust.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr WTHelperGetProvSignerFromChain(IntPtr pProvData, uint idxSigner, uint fCounterSigner, uint idxCounterSigner);

		// Token: 0x06004D9E RID: 19870
		[DllImport("wintrust.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr WTHelperGetProvCertFromChain(IntPtr pSgnr, uint idxCert);

		// Token: 0x06004D9F RID: 19871
		[DllImport("wintrust.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int WTGetSignatureInfo([MarshalAs(UnmanagedType.LPWStr)] [In] string pszFile, [In] IntPtr hFile, NativeMethods.SIGNATURE_INFO_FLAGS sigInfoFlags, ref NativeMethods.SIGNATURE_INFO psiginfo, ref IntPtr ppCertContext, ref IntPtr phWVTStateData);

		// Token: 0x06004DA0 RID: 19872 RVA: 0x00197E50 File Offset: 0x00196050
		internal static void FreeWVTStateData(IntPtr phWVTStateData)
		{
			NativeMethods.WINTRUST_DATA wintrust_DATA = default(NativeMethods.WINTRUST_DATA);
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Guid guid = new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");
			try
			{
				intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(guid));
				Marshal.StructureToPtr(guid, intPtr, false);
				wintrust_DATA.cbStruct = (uint)Marshal.SizeOf(wintrust_DATA);
				wintrust_DATA.dwUIChoice = 2U;
				wintrust_DATA.fdwRevocationChecks = 0U;
				wintrust_DATA.dwUnionChoice = 3U;
				wintrust_DATA.dwStateAction = 2U;
				wintrust_DATA.hWVTStateData = phWVTStateData;
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(wintrust_DATA));
				Marshal.StructureToPtr(wintrust_DATA, intPtr2, false);
				NativeMethods.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
			}
			finally
			{
				ClrFacade.DestroyStructure<NativeMethods.WINTRUST_DATA>(intPtr2);
				Marshal.FreeCoTaskMem(intPtr2);
				ClrFacade.DestroyStructure<Guid>(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
			}
		}

		// Token: 0x06004DA1 RID: 19873
		[DllImport("wintrust.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WTHelperIsChainedToMicrosoft([In] ref NativeMethods.CERT_CONTEXT pSignerCert, [In] IntPtr hCertBag, [MarshalAs(UnmanagedType.Bool)] bool fTrustTestCert);

		// Token: 0x06004DA2 RID: 19874
		[DllImport("wintrust.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WTHelperIsChainedToMicrosoftFromStateData([In] IntPtr hWVTStateData, [MarshalAs(UnmanagedType.Bool)] bool fTrustTestCert);

		// Token: 0x06004DA3 RID: 19875
		[DllImport("certca.dll")]
		internal static extern int CCFindCertificateBuildFilter([MarshalAs(UnmanagedType.LPWStr)] string filter, ref IntPtr certFilter);

		// Token: 0x06004DA4 RID: 19876
		[DllImport("certca.dll")]
		internal static extern void CCFindCertificateFreeFilter(IntPtr certFilter);

		// Token: 0x06004DA5 RID: 19877
		[DllImport("certca.dll")]
		internal static extern IntPtr CCFindCertificateFromFilter(IntPtr storeHandle, IntPtr certFilter, IntPtr prevCertContext);

		// Token: 0x06004DA6 RID: 19878
		[DllImport("certca.dll")]
		internal static extern int CCGetCertNameList(IntPtr certContext, NativeMethods.AltNameType dwAltNameChoice, NativeMethods.CryptDecodeFlags dwFlags, out uint cName, out IntPtr papwszName);

		// Token: 0x06004DA7 RID: 19879
		[DllImport("certca.dll")]
		internal static extern void CCFreeStringArray(IntPtr papwsz);

		// Token: 0x06004DA8 RID: 19880
		[DllImport("certenroll.dll")]
		internal static extern int LogCertDelete(bool fMachine, IntPtr pCertContext);

		// Token: 0x06004DA9 RID: 19881
		[DllImport("certenroll.dll")]
		internal static extern int LogCertCopy(bool fMachine, IntPtr pCertContext);

		// Token: 0x06004DAA RID: 19882
		[DllImport("kernel32.dll")]
		internal static extern bool ProcessIdToSessionId(uint dwProcessId, out uint pSessionId);

		// Token: 0x06004DAB RID: 19883
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetConsoleWindow();

		// Token: 0x06004DAC RID: 19884
		[DllImport("user32.dll")]
		internal static extern IntPtr GetDesktopWindow();

		// Token: 0x06004DAD RID: 19885
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SaferIdentifyLevel(uint dwNumProperties, [In] ref SAFER_CODE_PROPERTIES pCodeProperties, out IntPtr pLevelHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string bucket);

		// Token: 0x06004DAE RID: 19886
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SaferComputeTokenFromLevel([In] IntPtr LevelHandle, [In] IntPtr InAccessToken, ref IntPtr OutAccessToken, uint dwFlags, IntPtr lpReserved);

		// Token: 0x06004DAF RID: 19887
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SaferCloseLevel([In] IntPtr hLevelHandle);

		// Token: 0x06004DB0 RID: 19888
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle([In] IntPtr hObject);

		// Token: 0x06004DB1 RID: 19889
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint GetNamedSecurityInfo(string pObjectName, NativeMethods.SeObjectType ObjectType, NativeMethods.SecurityInformation SecurityInfo, out IntPtr ppsidOwner, out IntPtr ppsidGroup, out IntPtr ppDacl, out IntPtr ppSacl, out IntPtr ppSecurityDescriptor);

		// Token: 0x06004DB2 RID: 19890
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint SetNamedSecurityInfo(string pObjectName, NativeMethods.SeObjectType ObjectType, NativeMethods.SecurityInformation SecurityInfo, IntPtr psidOwner, IntPtr psidGroup, IntPtr pDacl, IntPtr pSacl);

		// Token: 0x06004DB3 RID: 19891
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool ConvertStringSidToSid(string StringSid, out IntPtr Sid);

		// Token: 0x06004DB4 RID: 19892
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool IsValidSid(IntPtr pSid);

		// Token: 0x06004DB5 RID: 19893
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint GetLengthSid(IntPtr pSid);

		// Token: 0x06004DB6 RID: 19894
		[DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint LsaQueryCAPs(IntPtr[] CAPIDs, uint CAPIDCount, out IntPtr CAPs, out uint CAPCount);

		// Token: 0x06004DB7 RID: 19895
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint LsaFreeMemory(IntPtr Buffer);

		// Token: 0x06004DB8 RID: 19896
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool InitializeAcl(IntPtr pAcl, uint nAclLength, uint dwAclRevision);

		// Token: 0x06004DB9 RID: 19897
		[DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
		internal static extern uint RtlAddScopedPolicyIDAce(IntPtr Acl, uint AceRevision, uint AceFlags, uint AccessMask, IntPtr Sid);

		// Token: 0x06004DBA RID: 19898
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr GetCurrentProcess();

		// Token: 0x06004DBB RID: 19899
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr GetCurrentThread();

		// Token: 0x06004DBC RID: 19900
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

		// Token: 0x06004DBD RID: 19901
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool OpenThreadToken(IntPtr ThreadHandle, uint DesiredAccess, bool OpenAsSelf, out IntPtr TokenHandle);

		// Token: 0x06004DBE RID: 19902
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref NativeMethods.LUID lpLuid);

		// Token: 0x06004DBF RID: 19903
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref NativeMethods.TOKEN_PRIVILEGE NewState, uint BufferLength, ref NativeMethods.TOKEN_PRIVILEGE PreviousState, ref uint ReturnLength);

		// Token: 0x06004DC0 RID: 19904
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr hMem);

		// Token: 0x0400269A RID: 9882
		internal const string NCRYPT_WINDOW_HANDLE_PROPERTY = "HWND Handle";

		// Token: 0x0400269B RID: 9883
		internal const int CRYPT_E_NOT_FOUND = -2146885628;

		// Token: 0x0400269C RID: 9884
		internal const int E_INVALID_DATA = -2147024883;

		// Token: 0x0400269D RID: 9885
		internal const int NTE_NOT_SUPPORTED = -2146893783;

		// Token: 0x0400269E RID: 9886
		internal const uint ERROR_SUCCESS = 0U;

		// Token: 0x0400269F RID: 9887
		internal const uint ERROR_NO_TOKEN = 1008U;

		// Token: 0x040026A0 RID: 9888
		internal const uint STATUS_SUCCESS = 0U;

		// Token: 0x040026A1 RID: 9889
		internal const uint STATUS_INVALID_PARAMETER = 3221225485U;

		// Token: 0x040026A2 RID: 9890
		internal const uint ACL_REVISION = 2U;

		// Token: 0x040026A3 RID: 9891
		internal const uint SYSTEM_SCOPED_POLICY_ID_ACE_TYPE = 19U;

		// Token: 0x040026A4 RID: 9892
		internal const uint SUB_CONTAINERS_AND_OBJECTS_INHERIT = 3U;

		// Token: 0x040026A5 RID: 9893
		internal const uint INHERIT_ONLY_ACE = 8U;

		// Token: 0x040026A6 RID: 9894
		internal const uint TOKEN_ASSIGN_PRIMARY = 1U;

		// Token: 0x040026A7 RID: 9895
		internal const uint TOKEN_DUPLICATE = 2U;

		// Token: 0x040026A8 RID: 9896
		internal const uint TOKEN_IMPERSONATE = 4U;

		// Token: 0x040026A9 RID: 9897
		internal const uint TOKEN_QUERY = 8U;

		// Token: 0x040026AA RID: 9898
		internal const uint TOKEN_QUERY_SOURCE = 16U;

		// Token: 0x040026AB RID: 9899
		internal const uint TOKEN_ADJUST_PRIVILEGES = 32U;

		// Token: 0x040026AC RID: 9900
		internal const uint TOKEN_ADJUST_GROUPS = 64U;

		// Token: 0x040026AD RID: 9901
		internal const uint TOKEN_ADJUST_DEFAULT = 128U;

		// Token: 0x040026AE RID: 9902
		internal const uint TOKEN_ADJUST_SESSIONID = 256U;

		// Token: 0x040026AF RID: 9903
		internal const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1U;

		// Token: 0x040026B0 RID: 9904
		internal const uint SE_PRIVILEGE_ENABLED = 2U;

		// Token: 0x040026B1 RID: 9905
		internal const uint SE_PRIVILEGE_REMOVED = 4U;

		// Token: 0x040026B2 RID: 9906
		internal const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648U;

		// Token: 0x020007B9 RID: 1977
		// (Invoke) Token: 0x06004DC2 RID: 19906
		internal delegate bool CertEnumSystemStoreCallBackProto([MarshalAs(UnmanagedType.LPWStr)] string storeName, uint dwFlagsNotUsed, IntPtr notUsed1, IntPtr notUsed2, IntPtr notUsed3);

		// Token: 0x020007BA RID: 1978
		[Flags]
		internal enum CertFindType
		{
			// Token: 0x040026B4 RID: 9908
			CERT_COMPARE_ANY = 0,
			// Token: 0x040026B5 RID: 9909
			CERT_FIND_ISSUER_STR = 524292,
			// Token: 0x040026B6 RID: 9910
			CERT_FIND_SUBJECT_STR = 524295,
			// Token: 0x040026B7 RID: 9911
			CERT_FIND_CROSS_CERT_DIST_POINTS = 1114112,
			// Token: 0x040026B8 RID: 9912
			CERT_FIND_SUBJECT_INFO_ACCESS = 1245184,
			// Token: 0x040026B9 RID: 9913
			CERT_FIND_HASH_STR = 1310720
		}

		// Token: 0x020007BB RID: 1979
		[Flags]
		internal enum CertStoreFlags
		{
			// Token: 0x040026BB RID: 9915
			CERT_SYSTEM_STORE_CURRENT_USER = 65536,
			// Token: 0x040026BC RID: 9916
			CERT_SYSTEM_STORE_LOCAL_MACHINE = 131072,
			// Token: 0x040026BD RID: 9917
			CERT_SYSTEM_STORE_CURRENT_SERVICE = 262144,
			// Token: 0x040026BE RID: 9918
			CERT_SYSTEM_STORE_SERVICES = 327680,
			// Token: 0x040026BF RID: 9919
			CERT_SYSTEM_STORE_USERS = 393216,
			// Token: 0x040026C0 RID: 9920
			CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY = 458752,
			// Token: 0x040026C1 RID: 9921
			CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY = 524288,
			// Token: 0x040026C2 RID: 9922
			CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE = 589824
		}

		// Token: 0x020007BC RID: 1980
		[Flags]
		internal enum CertOpenStoreFlags
		{
			// Token: 0x040026C4 RID: 9924
			CERT_STORE_NO_CRYPT_RELEASE_FLAG = 1,
			// Token: 0x040026C5 RID: 9925
			CERT_STORE_SET_LOCALIZED_NAME_FLAG = 2,
			// Token: 0x040026C6 RID: 9926
			CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG = 4,
			// Token: 0x040026C7 RID: 9927
			CERT_STORE_DELETE_FLAG = 16,
			// Token: 0x040026C8 RID: 9928
			CERT_STORE_UNSAFE_PHYSICAL_FLAG = 32,
			// Token: 0x040026C9 RID: 9929
			CERT_STORE_SHARE_STORE_FLAG = 64,
			// Token: 0x040026CA RID: 9930
			CERT_STORE_SHARE_CONTEXT_FLAG = 128,
			// Token: 0x040026CB RID: 9931
			CERT_STORE_MANIFOLD_FLAG = 256,
			// Token: 0x040026CC RID: 9932
			CERT_STORE_ENUM_ARCHIVED_FLAG = 512,
			// Token: 0x040026CD RID: 9933
			CERT_STORE_UPDATE_KEYID_FLAG = 1024,
			// Token: 0x040026CE RID: 9934
			CERT_STORE_BACKUP_RESTORE_FLAG = 2048,
			// Token: 0x040026CF RID: 9935
			CERT_STORE_READONLY_FLAG = 32768,
			// Token: 0x040026D0 RID: 9936
			CERT_STORE_OPEN_EXISTING_FLAG = 16384,
			// Token: 0x040026D1 RID: 9937
			CERT_STORE_CREATE_NEW_FLAG = 8192,
			// Token: 0x040026D2 RID: 9938
			CERT_STORE_MAXIMUM_ALLOWED_FLAG = 4096,
			// Token: 0x040026D3 RID: 9939
			CERT_SYSTEM_STORE_CURRENT_USER = 65536,
			// Token: 0x040026D4 RID: 9940
			CERT_SYSTEM_STORE_LOCAL_MACHINE = 131072,
			// Token: 0x040026D5 RID: 9941
			CERT_SYSTEM_STORE_CURRENT_SERVICE = 262144,
			// Token: 0x040026D6 RID: 9942
			CERT_SYSTEM_STORE_SERVICES = 327680,
			// Token: 0x040026D7 RID: 9943
			CERT_SYSTEM_STORE_USERS = 393216,
			// Token: 0x040026D8 RID: 9944
			CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY = 458752,
			// Token: 0x040026D9 RID: 9945
			CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY = 524288,
			// Token: 0x040026DA RID: 9946
			CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE = 589824
		}

		// Token: 0x020007BD RID: 1981
		[Flags]
		internal enum CertOpenStoreProvider
		{
			// Token: 0x040026DC RID: 9948
			CERT_STORE_PROV_MEMORY = 2,
			// Token: 0x040026DD RID: 9949
			CERT_STORE_PROV_SYSTEM = 10,
			// Token: 0x040026DE RID: 9950
			CERT_STORE_PROV_SYSTEM_REGISTRY = 13
		}

		// Token: 0x020007BE RID: 1982
		[Flags]
		internal enum CertOpenStoreEncodingType
		{
			// Token: 0x040026E0 RID: 9952
			X509_ASN_ENCODING = 1
		}

		// Token: 0x020007BF RID: 1983
		[Flags]
		internal enum CertControlStoreType : uint
		{
			// Token: 0x040026E2 RID: 9954
			CERT_STORE_CTRL_RESYNC = 1U,
			// Token: 0x040026E3 RID: 9955
			CERT_STORE_CTRL_COMMIT = 3U,
			// Token: 0x040026E4 RID: 9956
			CERT_STORE_CTRL_AUTO_RESYNC = 4U
		}

		// Token: 0x020007C0 RID: 1984
		[Flags]
		internal enum AddCertificateContext : uint
		{
			// Token: 0x040026E6 RID: 9958
			CERT_STORE_ADD_NEW = 1U,
			// Token: 0x040026E7 RID: 9959
			CERT_STORE_ADD_USE_EXISTING = 2U,
			// Token: 0x040026E8 RID: 9960
			CERT_STORE_ADD_REPLACE_EXISTING = 3U,
			// Token: 0x040026E9 RID: 9961
			CERT_STORE_ADD_ALWAYS = 4U,
			// Token: 0x040026EA RID: 9962
			CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES = 5U,
			// Token: 0x040026EB RID: 9963
			CERT_STORE_ADD_NEWER = 6U,
			// Token: 0x040026EC RID: 9964
			CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES = 7U
		}

		// Token: 0x020007C1 RID: 1985
		[Flags]
		internal enum CertPropertyId
		{
			// Token: 0x040026EE RID: 9966
			CERT_KEY_PROV_HANDLE_PROP_ID = 1,
			// Token: 0x040026EF RID: 9967
			CERT_KEY_PROV_INFO_PROP_ID = 2,
			// Token: 0x040026F0 RID: 9968
			CERT_SHA1_HASH_PROP_ID = 3,
			// Token: 0x040026F1 RID: 9969
			CERT_MD5_HASH_PROP_ID = 4,
			// Token: 0x040026F2 RID: 9970
			CERT_SEND_AS_TRUSTED_ISSUER_PROP_ID = 102
		}

		// Token: 0x020007C2 RID: 1986
		[Flags]
		internal enum NCryptDeletKeyFlag
		{
			// Token: 0x040026F4 RID: 9972
			NCRYPT_MACHINE_KEY_FLAG = 32,
			// Token: 0x040026F5 RID: 9973
			NCRYPT_SILENT_FLAG = 64
		}

		// Token: 0x020007C3 RID: 1987
		[Flags]
		internal enum ProviderFlagsEnum : uint
		{
			// Token: 0x040026F7 RID: 9975
			CRYPT_VERIFYCONTEXT = 4026531840U,
			// Token: 0x040026F8 RID: 9976
			CRYPT_NEWKEYSET = 8U,
			// Token: 0x040026F9 RID: 9977
			CRYPT_DELETEKEYSET = 16U,
			// Token: 0x040026FA RID: 9978
			CRYPT_MACHINE_KEYSET = 32U,
			// Token: 0x040026FB RID: 9979
			CRYPT_SILENT = 64U
		}

		// Token: 0x020007C4 RID: 1988
		internal enum ProviderParam
		{
			// Token: 0x040026FD RID: 9981
			PP_CLIENT_HWND = 1
		}

		// Token: 0x020007C5 RID: 1989
		internal enum PROV : uint
		{
			// Token: 0x040026FF RID: 9983
			RSA_FULL = 1U,
			// Token: 0x04002700 RID: 9984
			RSA_SIG,
			// Token: 0x04002701 RID: 9985
			DSS,
			// Token: 0x04002702 RID: 9986
			FORTEZZA,
			// Token: 0x04002703 RID: 9987
			MS_EXCHANGE,
			// Token: 0x04002704 RID: 9988
			SSL,
			// Token: 0x04002705 RID: 9989
			RSA_SCHANNEL = 12U,
			// Token: 0x04002706 RID: 9990
			DSS_DH,
			// Token: 0x04002707 RID: 9991
			EC_ECDSA_SIG,
			// Token: 0x04002708 RID: 9992
			EC_ECNRA_SIG,
			// Token: 0x04002709 RID: 9993
			EC_ECDSA_FULL,
			// Token: 0x0400270A RID: 9994
			EC_ECNRA_FULL,
			// Token: 0x0400270B RID: 9995
			DH_SCHANNEL,
			// Token: 0x0400270C RID: 9996
			SPYRUS_LYNKS = 20U,
			// Token: 0x0400270D RID: 9997
			RNG,
			// Token: 0x0400270E RID: 9998
			INTEL_SEC
		}

		// Token: 0x020007C6 RID: 1990
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_KEY_PROV_INFO
		{
			// Token: 0x0400270F RID: 9999
			public string pwszContainerName;

			// Token: 0x04002710 RID: 10000
			public string pwszProvName;

			// Token: 0x04002711 RID: 10001
			public NativeMethods.PROV dwProvType;

			// Token: 0x04002712 RID: 10002
			public uint dwFlags;

			// Token: 0x04002713 RID: 10003
			public uint cProvParam;

			// Token: 0x04002714 RID: 10004
			public IntPtr rgProvParam;

			// Token: 0x04002715 RID: 10005
			public uint dwKeySpec;
		}

		// Token: 0x020007C7 RID: 1991
		[Flags]
		internal enum CryptUIFlags
		{
			// Token: 0x04002717 RID: 10007
			CRYPTUI_WIZ_NO_UI = 1
		}

		// Token: 0x020007C8 RID: 1992
		internal struct CRYPTUI_WIZ_DIGITAL_SIGN_INFO
		{
			// Token: 0x04002718 RID: 10008
			internal uint dwSize;

			// Token: 0x04002719 RID: 10009
			internal uint dwSubjectChoice;

			// Token: 0x0400271A RID: 10010
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pwszFileName;

			// Token: 0x0400271B RID: 10011
			internal uint dwSigningCertChoice;

			// Token: 0x0400271C RID: 10012
			internal IntPtr pSigningCertContext;

			// Token: 0x0400271D RID: 10013
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pwszTimestampURL;

			// Token: 0x0400271E RID: 10014
			internal uint dwAdditionalCertChoice;

			// Token: 0x0400271F RID: 10015
			internal IntPtr pSignExtInfo;
		}

		// Token: 0x020007C9 RID: 1993
		[Flags]
		internal enum SignInfoSubjectChoice
		{
			// Token: 0x04002721 RID: 10017
			CRYPTUI_WIZ_DIGITAL_SIGN_SUBJECT_FILE = 1
		}

		// Token: 0x020007CA RID: 1994
		[Flags]
		internal enum SignInfoCertChoice
		{
			// Token: 0x04002723 RID: 10019
			CRYPTUI_WIZ_DIGITAL_SIGN_CERT = 1
		}

		// Token: 0x020007CB RID: 1995
		[Flags]
		internal enum SignInfoAdditionalCertChoice
		{
			// Token: 0x04002725 RID: 10021
			CRYPTUI_WIZ_DIGITAL_SIGN_ADD_CHAIN = 1,
			// Token: 0x04002726 RID: 10022
			CRYPTUI_WIZ_DIGITAL_SIGN_ADD_CHAIN_NO_ROOT = 2
		}

		// Token: 0x020007CC RID: 1996
		internal struct CRYPTUI_WIZ_DIGITAL_SIGN_EXTENDED_INFO
		{
			// Token: 0x04002727 RID: 10023
			internal uint dwSize;

			// Token: 0x04002728 RID: 10024
			internal uint dwAttrFlagsNotUsed;

			// Token: 0x04002729 RID: 10025
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pwszDescription;

			// Token: 0x0400272A RID: 10026
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pwszMoreInfoLocation;

			// Token: 0x0400272B RID: 10027
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszHashAlg;

			// Token: 0x0400272C RID: 10028
			internal IntPtr pwszSigningCertDisplayStringNotUsed;

			// Token: 0x0400272D RID: 10029
			internal IntPtr hAdditionalCertStoreNotUsed;

			// Token: 0x0400272E RID: 10030
			internal IntPtr psAuthenticatedNotUsed;

			// Token: 0x0400272F RID: 10031
			internal IntPtr psUnauthenticatedNotUsed;
		}

		// Token: 0x020007CD RID: 1997
		internal struct CRYPT_OID_INFO
		{
			// Token: 0x04002730 RID: 10032
			public uint cbSize;

			// Token: 0x04002731 RID: 10033
			[MarshalAs(UnmanagedType.LPStr)]
			public string pszOID;

			// Token: 0x04002732 RID: 10034
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwszName;

			// Token: 0x04002733 RID: 10035
			public uint dwGroupId;

			// Token: 0x04002734 RID: 10036
			public NativeMethods.Anonymous_a3ae7823_8a1d_432c_bc07_a72b6fc6c7d8 Union1;

			// Token: 0x04002735 RID: 10037
			public NativeMethods.CRYPT_ATTR_BLOB ExtraInfo;
		}

		// Token: 0x020007CE RID: 1998
		[StructLayout(LayoutKind.Explicit)]
		internal struct Anonymous_a3ae7823_8a1d_432c_bc07_a72b6fc6c7d8
		{
			// Token: 0x04002736 RID: 10038
			[FieldOffset(0)]
			public uint dwValue;

			// Token: 0x04002737 RID: 10039
			[FieldOffset(0)]
			public uint Algid;

			// Token: 0x04002738 RID: 10040
			[FieldOffset(0)]
			public uint dwLength;
		}

		// Token: 0x020007CF RID: 1999
		internal struct CRYPT_ATTR_BLOB
		{
			// Token: 0x04002739 RID: 10041
			public uint cbData;

			// Token: 0x0400273A RID: 10042
			public IntPtr pbData;
		}

		// Token: 0x020007D0 RID: 2000
		internal struct CRYPT_DATA_BLOB
		{
			// Token: 0x0400273B RID: 10043
			public uint cbData;

			// Token: 0x0400273C RID: 10044
			public IntPtr pbData;
		}

		// Token: 0x020007D1 RID: 2001
		internal struct CERT_CONTEXT
		{
			// Token: 0x0400273D RID: 10045
			public int dwCertEncodingType;

			// Token: 0x0400273E RID: 10046
			public IntPtr pbCertEncoded;

			// Token: 0x0400273F RID: 10047
			public int cbCertEncoded;

			// Token: 0x04002740 RID: 10048
			public IntPtr pCertInfo;

			// Token: 0x04002741 RID: 10049
			public IntPtr hCertStore;
		}

		// Token: 0x020007D2 RID: 2002
		internal struct WINTRUST_FILE_INFO
		{
			// Token: 0x04002742 RID: 10050
			internal uint cbStruct;

			// Token: 0x04002743 RID: 10051
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pcwszFilePath;

			// Token: 0x04002744 RID: 10052
			internal IntPtr hFileNotUsed;

			// Token: 0x04002745 RID: 10053
			internal IntPtr pgKnownSubjectNotUsed;
		}

		// Token: 0x020007D3 RID: 2003
		internal struct WINTRUST_BLOB_INFO
		{
			// Token: 0x04002746 RID: 10054
			internal uint cbStruct;

			// Token: 0x04002747 RID: 10055
			internal NativeMethods.GUID gSubject;

			// Token: 0x04002748 RID: 10056
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pcwszDisplayName;

			// Token: 0x04002749 RID: 10057
			internal uint cbMemObject;

			// Token: 0x0400274A RID: 10058
			internal IntPtr pbMemObject;

			// Token: 0x0400274B RID: 10059
			internal uint cbMemSignedMsg;

			// Token: 0x0400274C RID: 10060
			internal IntPtr pbMemSignedMsg;
		}

		// Token: 0x020007D4 RID: 2004
		internal struct GUID
		{
			// Token: 0x0400274D RID: 10061
			internal uint Data1;

			// Token: 0x0400274E RID: 10062
			internal ushort Data2;

			// Token: 0x0400274F RID: 10063
			internal ushort Data3;

			// Token: 0x04002750 RID: 10064
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			internal byte[] Data4;
		}

		// Token: 0x020007D5 RID: 2005
		[Flags]
		internal enum WintrustUIChoice
		{
			// Token: 0x04002752 RID: 10066
			WTD_UI_ALL = 1,
			// Token: 0x04002753 RID: 10067
			WTD_UI_NONE = 2,
			// Token: 0x04002754 RID: 10068
			WTD_UI_NOBAD = 3,
			// Token: 0x04002755 RID: 10069
			WTD_UI_NOGOOD = 4
		}

		// Token: 0x020007D6 RID: 2006
		[Flags]
		internal enum WintrustUnionChoice
		{
			// Token: 0x04002757 RID: 10071
			WTD_CHOICE_FILE = 1,
			// Token: 0x04002758 RID: 10072
			WTD_CHOICE_BLOB = 3
		}

		// Token: 0x020007D7 RID: 2007
		[Flags]
		internal enum WintrustProviderFlags
		{
			// Token: 0x0400275A RID: 10074
			WTD_PROV_FLAGS_MASK = 65535,
			// Token: 0x0400275B RID: 10075
			WTD_USE_IE4_TRUST_FLAG = 1,
			// Token: 0x0400275C RID: 10076
			WTD_NO_IE4_CHAIN_FLAG = 2,
			// Token: 0x0400275D RID: 10077
			WTD_NO_POLICY_USAGE_FLAG = 4,
			// Token: 0x0400275E RID: 10078
			WTD_REVOCATION_CHECK_NONE = 16,
			// Token: 0x0400275F RID: 10079
			WTD_REVOCATION_CHECK_END_CERT = 32,
			// Token: 0x04002760 RID: 10080
			WTD_REVOCATION_CHECK_CHAIN = 64,
			// Token: 0x04002761 RID: 10081
			WTD_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 128,
			// Token: 0x04002762 RID: 10082
			WTD_SAFER_FLAG = 256,
			// Token: 0x04002763 RID: 10083
			WTD_HASH_ONLY_FLAG = 512,
			// Token: 0x04002764 RID: 10084
			WTD_USE_DEFAULT_OSVER_CHECK = 1024,
			// Token: 0x04002765 RID: 10085
			WTD_LIFETIME_SIGNING_FLAG = 2048,
			// Token: 0x04002766 RID: 10086
			WTD_CACHE_ONLY_URL_RETRIEVAL = 4096
		}

		// Token: 0x020007D8 RID: 2008
		[Flags]
		internal enum WintrustAction
		{
			// Token: 0x04002768 RID: 10088
			WTD_STATEACTION_IGNORE = 0,
			// Token: 0x04002769 RID: 10089
			WTD_STATEACTION_VERIFY = 1,
			// Token: 0x0400276A RID: 10090
			WTD_STATEACTION_CLOSE = 2,
			// Token: 0x0400276B RID: 10091
			WTD_STATEACTION_AUTO_CACHE = 3,
			// Token: 0x0400276C RID: 10092
			WTD_STATEACTION_AUTO_CACHE_FLUSH = 4
		}

		// Token: 0x020007D9 RID: 2009
		[StructLayout(LayoutKind.Explicit)]
		internal struct WinTrust_Choice
		{
			// Token: 0x0400276D RID: 10093
			[FieldOffset(0)]
			internal IntPtr pFile;

			// Token: 0x0400276E RID: 10094
			[FieldOffset(0)]
			internal IntPtr pCatalog;

			// Token: 0x0400276F RID: 10095
			[FieldOffset(0)]
			internal IntPtr pBlob;

			// Token: 0x04002770 RID: 10096
			[FieldOffset(0)]
			internal IntPtr pSgnr;

			// Token: 0x04002771 RID: 10097
			[FieldOffset(0)]
			internal IntPtr pCert;
		}

		// Token: 0x020007DA RID: 2010
		internal struct WINTRUST_DATA
		{
			// Token: 0x04002772 RID: 10098
			internal uint cbStruct;

			// Token: 0x04002773 RID: 10099
			internal IntPtr pPolicyCallbackData;

			// Token: 0x04002774 RID: 10100
			internal IntPtr pSIPClientData;

			// Token: 0x04002775 RID: 10101
			internal uint dwUIChoice;

			// Token: 0x04002776 RID: 10102
			internal uint fdwRevocationChecks;

			// Token: 0x04002777 RID: 10103
			internal uint dwUnionChoice;

			// Token: 0x04002778 RID: 10104
			internal NativeMethods.WinTrust_Choice Choice;

			// Token: 0x04002779 RID: 10105
			internal uint dwStateAction;

			// Token: 0x0400277A RID: 10106
			internal IntPtr hWVTStateData;

			// Token: 0x0400277B RID: 10107
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pwszURLReference;

			// Token: 0x0400277C RID: 10108
			internal uint dwProvFlags;

			// Token: 0x0400277D RID: 10109
			internal uint dwUIContext;
		}

		// Token: 0x020007DB RID: 2011
		internal struct CRYPT_PROVIDER_CERT
		{
			// Token: 0x0400277E RID: 10110
			private uint cbStruct;

			// Token: 0x0400277F RID: 10111
			internal IntPtr pCert;

			// Token: 0x04002780 RID: 10112
			private uint fCommercial;

			// Token: 0x04002781 RID: 10113
			private uint fTrustedRoot;

			// Token: 0x04002782 RID: 10114
			private uint fSelfSigned;

			// Token: 0x04002783 RID: 10115
			private uint fTestCert;

			// Token: 0x04002784 RID: 10116
			private uint dwRevokedReason;

			// Token: 0x04002785 RID: 10117
			private uint dwConfidence;

			// Token: 0x04002786 RID: 10118
			private uint dwError;

			// Token: 0x04002787 RID: 10119
			private IntPtr pTrustListContext;

			// Token: 0x04002788 RID: 10120
			private uint fTrustListSignerCert;

			// Token: 0x04002789 RID: 10121
			private IntPtr pCtlContext;

			// Token: 0x0400278A RID: 10122
			private uint dwCtlError;

			// Token: 0x0400278B RID: 10123
			private uint fIsCyclic;

			// Token: 0x0400278C RID: 10124
			private IntPtr pChainElement;
		}

		// Token: 0x020007DC RID: 2012
		internal struct CRYPT_PROVIDER_SGNR
		{
			// Token: 0x0400278D RID: 10125
			private uint cbStruct;

			// Token: 0x0400278E RID: 10126
			private NativeMethods.FILETIME sftVerifyAsOf;

			// Token: 0x0400278F RID: 10127
			private uint csCertChain;

			// Token: 0x04002790 RID: 10128
			private IntPtr pasCertChain;

			// Token: 0x04002791 RID: 10129
			private uint dwSignerType;

			// Token: 0x04002792 RID: 10130
			private IntPtr psSigner;

			// Token: 0x04002793 RID: 10131
			private uint dwError;

			// Token: 0x04002794 RID: 10132
			internal uint csCounterSigners;

			// Token: 0x04002795 RID: 10133
			internal IntPtr pasCounterSigners;

			// Token: 0x04002796 RID: 10134
			private IntPtr pChainContext;
		}

		// Token: 0x020007DD RID: 2013
		internal struct CERT_ENHKEY_USAGE
		{
			// Token: 0x04002797 RID: 10135
			internal uint cUsageIdentifier;

			// Token: 0x04002798 RID: 10136
			internal IntPtr rgpszUsageIdentifier;
		}

		// Token: 0x020007DE RID: 2014
		internal enum SIGNATURE_STATE
		{
			// Token: 0x0400279A RID: 10138
			SIGNATURE_STATE_UNSIGNED_MISSING,
			// Token: 0x0400279B RID: 10139
			SIGNATURE_STATE_UNSIGNED_UNSUPPORTED,
			// Token: 0x0400279C RID: 10140
			SIGNATURE_STATE_UNSIGNED_POLICY,
			// Token: 0x0400279D RID: 10141
			SIGNATURE_STATE_INVALID_CORRUPT,
			// Token: 0x0400279E RID: 10142
			SIGNATURE_STATE_INVALID_POLICY,
			// Token: 0x0400279F RID: 10143
			SIGNATURE_STATE_VALID,
			// Token: 0x040027A0 RID: 10144
			SIGNATURE_STATE_TRUSTED,
			// Token: 0x040027A1 RID: 10145
			SIGNATURE_STATE_UNTRUSTED
		}

		// Token: 0x020007DF RID: 2015
		internal enum SIGNATURE_INFO_FLAGS
		{
			// Token: 0x040027A3 RID: 10147
			SIF_NONE,
			// Token: 0x040027A4 RID: 10148
			SIF_AUTHENTICODE_SIGNED,
			// Token: 0x040027A5 RID: 10149
			SIF_CATALOG_SIGNED,
			// Token: 0x040027A6 RID: 10150
			SIF_VERSION_INFO = 4,
			// Token: 0x040027A7 RID: 10151
			SIF_CHECK_OS_BINARY = 2048,
			// Token: 0x040027A8 RID: 10152
			SIF_BASE_VERIFICATION = 4096,
			// Token: 0x040027A9 RID: 10153
			SIF_CATALOG_FIRST = 8192,
			// Token: 0x040027AA RID: 10154
			SIF_MOTW = 16384
		}

		// Token: 0x020007E0 RID: 2016
		internal enum SIGNATURE_INFO_AVAILABILITY
		{
			// Token: 0x040027AC RID: 10156
			SIA_DISPLAYNAME = 1,
			// Token: 0x040027AD RID: 10157
			SIA_PUBLISHERNAME,
			// Token: 0x040027AE RID: 10158
			SIA_MOREINFOURL = 4,
			// Token: 0x040027AF RID: 10159
			SIA_HASH = 8
		}

		// Token: 0x020007E1 RID: 2017
		internal enum SIGNATURE_INFO_TYPE
		{
			// Token: 0x040027B1 RID: 10161
			SIT_UNKNOWN,
			// Token: 0x040027B2 RID: 10162
			SIT_AUTHENTICODE,
			// Token: 0x040027B3 RID: 10163
			SIT_CATALOG
		}

		// Token: 0x020007E2 RID: 2018
		internal struct SIGNATURE_INFO
		{
			// Token: 0x040027B4 RID: 10164
			internal uint cbSize;

			// Token: 0x040027B5 RID: 10165
			internal NativeMethods.SIGNATURE_STATE nSignatureState;

			// Token: 0x040027B6 RID: 10166
			internal NativeMethods.SIGNATURE_INFO_TYPE nSignatureType;

			// Token: 0x040027B7 RID: 10167
			internal uint dwSignatureInfoAvailability;

			// Token: 0x040027B8 RID: 10168
			internal uint dwInfoAvailability;

			// Token: 0x040027B9 RID: 10169
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszDisplayName;

			// Token: 0x040027BA RID: 10170
			internal uint cchDisplayName;

			// Token: 0x040027BB RID: 10171
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszPublisherName;

			// Token: 0x040027BC RID: 10172
			internal uint cchPublisherName;

			// Token: 0x040027BD RID: 10173
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszMoreInfoURL;

			// Token: 0x040027BE RID: 10174
			internal uint cchMoreInfoURL;

			// Token: 0x040027BF RID: 10175
			internal IntPtr prgbHash;

			// Token: 0x040027C0 RID: 10176
			internal uint cbHash;

			// Token: 0x040027C1 RID: 10177
			internal int fOSBinary;
		}

		// Token: 0x020007E3 RID: 2019
		internal struct CERT_INFO
		{
			// Token: 0x040027C2 RID: 10178
			internal uint dwVersion;

			// Token: 0x040027C3 RID: 10179
			internal NativeMethods.CRYPT_ATTR_BLOB SerialNumber;

			// Token: 0x040027C4 RID: 10180
			internal NativeMethods.CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;

			// Token: 0x040027C5 RID: 10181
			internal NativeMethods.CRYPT_ATTR_BLOB Issuer;

			// Token: 0x040027C6 RID: 10182
			internal NativeMethods.FILETIME NotBefore;

			// Token: 0x040027C7 RID: 10183
			internal NativeMethods.FILETIME NotAfter;

			// Token: 0x040027C8 RID: 10184
			internal NativeMethods.CRYPT_ATTR_BLOB Subject;

			// Token: 0x040027C9 RID: 10185
			internal NativeMethods.CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;

			// Token: 0x040027CA RID: 10186
			internal NativeMethods.CRYPT_BIT_BLOB IssuerUniqueId;

			// Token: 0x040027CB RID: 10187
			internal NativeMethods.CRYPT_BIT_BLOB SubjectUniqueId;

			// Token: 0x040027CC RID: 10188
			internal uint cExtension;

			// Token: 0x040027CD RID: 10189
			internal IntPtr rgExtension;
		}

		// Token: 0x020007E4 RID: 2020
		internal struct CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x040027CE RID: 10190
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040027CF RID: 10191
			internal NativeMethods.CRYPT_ATTR_BLOB Parameters;
		}

		// Token: 0x020007E5 RID: 2021
		internal struct FILETIME
		{
			// Token: 0x040027D0 RID: 10192
			internal uint dwLowDateTime;

			// Token: 0x040027D1 RID: 10193
			internal uint dwHighDateTime;
		}

		// Token: 0x020007E6 RID: 2022
		internal struct CERT_PUBLIC_KEY_INFO
		{
			// Token: 0x040027D2 RID: 10194
			internal NativeMethods.CRYPT_ALGORITHM_IDENTIFIER Algorithm;

			// Token: 0x040027D3 RID: 10195
			internal NativeMethods.CRYPT_BIT_BLOB PublicKey;
		}

		// Token: 0x020007E7 RID: 2023
		internal struct CRYPT_BIT_BLOB
		{
			// Token: 0x040027D4 RID: 10196
			internal uint cbData;

			// Token: 0x040027D5 RID: 10197
			internal IntPtr pbData;

			// Token: 0x040027D6 RID: 10198
			internal uint cUnusedBits;
		}

		// Token: 0x020007E8 RID: 2024
		internal struct CERT_EXTENSION
		{
			// Token: 0x040027D7 RID: 10199
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszObjId;

			// Token: 0x040027D8 RID: 10200
			internal int fCritical;

			// Token: 0x040027D9 RID: 10201
			internal NativeMethods.CRYPT_ATTR_BLOB Value;
		}

		// Token: 0x020007E9 RID: 2025
		internal enum AltNameType : uint
		{
			// Token: 0x040027DB RID: 10203
			CERT_ALT_NAME_OTHER_NAME = 1U,
			// Token: 0x040027DC RID: 10204
			CERT_ALT_NAME_RFC822_NAME,
			// Token: 0x040027DD RID: 10205
			CERT_ALT_NAME_DNS_NAME,
			// Token: 0x040027DE RID: 10206
			CERT_ALT_NAME_X400_ADDRESS,
			// Token: 0x040027DF RID: 10207
			CERT_ALT_NAME_DIRECTORY_NAME,
			// Token: 0x040027E0 RID: 10208
			CERT_ALT_NAME_EDI_PARTY_NAME,
			// Token: 0x040027E1 RID: 10209
			CERT_ALT_NAME_URL,
			// Token: 0x040027E2 RID: 10210
			CERT_ALT_NAME_IP_ADDRESS,
			// Token: 0x040027E3 RID: 10211
			CERT_ALT_NAME_REGISTERED_ID
		}

		// Token: 0x020007EA RID: 2026
		internal enum CryptDecodeFlags : uint
		{
			// Token: 0x040027E5 RID: 10213
			CRYPT_DECODE_ENABLE_PUNYCODE_FLAG = 33554432U,
			// Token: 0x040027E6 RID: 10214
			CRYPT_DECODE_ENABLE_UTF8PERCENT_FLAG = 67108864U,
			// Token: 0x040027E7 RID: 10215
			CRYPT_DECODE_ENABLE_IA5CONVERSION_FLAG = 100663296U
		}

		// Token: 0x020007EB RID: 2027
		internal enum SeObjectType : uint
		{
			// Token: 0x040027E9 RID: 10217
			SE_UNKNOWN_OBJECT_TYPE,
			// Token: 0x040027EA RID: 10218
			SE_FILE_OBJECT,
			// Token: 0x040027EB RID: 10219
			SE_SERVICE,
			// Token: 0x040027EC RID: 10220
			SE_PRINTER,
			// Token: 0x040027ED RID: 10221
			SE_REGISTRY_KEY,
			// Token: 0x040027EE RID: 10222
			SE_LMSHARE,
			// Token: 0x040027EF RID: 10223
			SE_KERNEL_OBJECT,
			// Token: 0x040027F0 RID: 10224
			SE_WINDOW_OBJECT,
			// Token: 0x040027F1 RID: 10225
			SE_DS_OBJECT,
			// Token: 0x040027F2 RID: 10226
			SE_DS_OBJECT_ALL,
			// Token: 0x040027F3 RID: 10227
			SE_PROVIDER_DEFINED_OBJECT,
			// Token: 0x040027F4 RID: 10228
			SE_WMIGUID_OBJECT,
			// Token: 0x040027F5 RID: 10229
			SE_REGISTRY_WOW64_32KEY
		}

		// Token: 0x020007EC RID: 2028
		internal enum SecurityInformation : uint
		{
			// Token: 0x040027F7 RID: 10231
			OWNER_SECURITY_INFORMATION = 1U,
			// Token: 0x040027F8 RID: 10232
			GROUP_SECURITY_INFORMATION,
			// Token: 0x040027F9 RID: 10233
			DACL_SECURITY_INFORMATION = 4U,
			// Token: 0x040027FA RID: 10234
			SACL_SECURITY_INFORMATION = 8U,
			// Token: 0x040027FB RID: 10235
			LABEL_SECURITY_INFORMATION = 16U,
			// Token: 0x040027FC RID: 10236
			ATTRIBUTE_SECURITY_INFORMATION = 32U,
			// Token: 0x040027FD RID: 10237
			SCOPE_SECURITY_INFORMATION = 64U,
			// Token: 0x040027FE RID: 10238
			BACKUP_SECURITY_INFORMATION = 65536U,
			// Token: 0x040027FF RID: 10239
			PROTECTED_DACL_SECURITY_INFORMATION = 2147483648U,
			// Token: 0x04002800 RID: 10240
			PROTECTED_SACL_SECURITY_INFORMATION = 1073741824U,
			// Token: 0x04002801 RID: 10241
			UNPROTECTED_DACL_SECURITY_INFORMATION = 536870912U,
			// Token: 0x04002802 RID: 10242
			UNPROTECTED_SACL_SECURITY_INFORMATION = 268435456U
		}

		// Token: 0x020007ED RID: 2029
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID
		{
			// Token: 0x04002803 RID: 10243
			internal uint LowPart;

			// Token: 0x04002804 RID: 10244
			internal uint HighPart;
		}

		// Token: 0x020007EE RID: 2030
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID_AND_ATTRIBUTES
		{
			// Token: 0x04002805 RID: 10245
			internal NativeMethods.LUID Luid;

			// Token: 0x04002806 RID: 10246
			internal uint Attributes;
		}

		// Token: 0x020007EF RID: 2031
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_PRIVILEGE
		{
			// Token: 0x04002807 RID: 10247
			internal uint PrivilegeCount;

			// Token: 0x04002808 RID: 10248
			internal NativeMethods.LUID_AND_ATTRIBUTES Privilege;
		}

		// Token: 0x020007F0 RID: 2032
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct ACL
		{
			// Token: 0x04002809 RID: 10249
			internal byte AclRevision;

			// Token: 0x0400280A RID: 10250
			internal byte Sbz1;

			// Token: 0x0400280B RID: 10251
			internal ushort AclSize;

			// Token: 0x0400280C RID: 10252
			internal ushort AceCount;

			// Token: 0x0400280D RID: 10253
			internal ushort Sbz2;
		}

		// Token: 0x020007F1 RID: 2033
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct ACE_HEADER
		{
			// Token: 0x0400280E RID: 10254
			internal byte AceType;

			// Token: 0x0400280F RID: 10255
			internal byte AceFlags;

			// Token: 0x04002810 RID: 10256
			internal ushort AceSize;
		}

		// Token: 0x020007F2 RID: 2034
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct SYSTEM_AUDIT_ACE
		{
			// Token: 0x04002811 RID: 10257
			internal NativeMethods.ACE_HEADER Header;

			// Token: 0x04002812 RID: 10258
			internal uint Mask;

			// Token: 0x04002813 RID: 10259
			internal uint SidStart;
		}

		// Token: 0x020007F3 RID: 2035
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LSA_UNICODE_STRING
		{
			// Token: 0x04002814 RID: 10260
			internal ushort Length;

			// Token: 0x04002815 RID: 10261
			internal ushort MaximumLength;

			// Token: 0x04002816 RID: 10262
			internal IntPtr Buffer;
		}

		// Token: 0x020007F4 RID: 2036
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CENTRAL_ACCESS_POLICY
		{
			// Token: 0x04002817 RID: 10263
			internal IntPtr CAPID;

			// Token: 0x04002818 RID: 10264
			internal NativeMethods.LSA_UNICODE_STRING Name;

			// Token: 0x04002819 RID: 10265
			internal NativeMethods.LSA_UNICODE_STRING Description;

			// Token: 0x0400281A RID: 10266
			internal NativeMethods.LSA_UNICODE_STRING ChangeId;

			// Token: 0x0400281B RID: 10267
			internal uint Flags;

			// Token: 0x0400281C RID: 10268
			internal uint CAPECount;

			// Token: 0x0400281D RID: 10269
			internal IntPtr CAPEs;
		}
	}
}
