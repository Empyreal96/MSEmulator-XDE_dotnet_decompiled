using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x0200078F RID: 1935
	[SuppressUnmanagedCodeSecurity]
	internal static class Win32Native
	{
		// Token: 0x06004CBB RID: 19643
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LookupAccountSid(string lpSystemName, IntPtr sid, StringBuilder lpName, ref int cchName, StringBuilder referencedDomainName, ref int cchReferencedDomainName, out Win32Native.SID_NAME_USE peUse);

		// Token: 0x06004CBC RID: 19644
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06004CBD RID: 19645
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenProcessToken(SafeHandle processHandle, uint desiredAccess, out IntPtr tokenHandle);

		// Token: 0x06004CBE RID: 19646
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetTokenInformation(IntPtr tokenHandle, Win32Native.TOKEN_INFORMATION_CLASS tokenInformationClass, IntPtr tokenInformation, int tokenInformationLength, out int returnLength);

		// Token: 0x06004CBF RID: 19647
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint GetSecurityDescriptorLength(IntPtr byteArray);

		// Token: 0x06004CC0 RID: 19648
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetSecurityInfo", SetLastError = true)]
		internal static extern uint GetSecurityInfoByHandle(SafeHandle handle, uint objectType, uint securityInformation, out IntPtr sidOwner, out IntPtr sidGroup, out IntPtr dacl, out IntPtr sacl, out IntPtr securityDescriptor);

		// Token: 0x06004CC1 RID: 19649
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetSecurityInfo", SetLastError = true)]
		internal static extern uint SetSecurityInfoByHandle(SafeHandle handle, uint objectType, uint securityInformation, byte[] owner, byte[] group, byte[] dacl, byte[] sacl);

		// Token: 0x06004CC2 RID: 19650
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x06004CC3 RID: 19651
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegConnectRegistry(string machineName, SafeRegistryHandle key, out SafeRegistryHandle result);

		// Token: 0x06004CC4 RID: 19652
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegCreateKeyEx(SafeRegistryHandle hKey, string lpSubKey, int Reserved, string lpClass, int dwOptions, int samDesigner, Win32Native.SECURITY_ATTRIBUTES lpSecurityAttributes, out SafeRegistryHandle hkResult, out int lpdwDisposition);

		// Token: 0x06004CC5 RID: 19653
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegDeleteKey(SafeRegistryHandle hKey, string lpSubKey);

		// Token: 0x06004CC6 RID: 19654
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegDeleteKeyTransacted(SafeRegistryHandle hKey, string lpSubKey, int samDesired, uint reserved, SafeTransactionHandle hTransaction, IntPtr pExtendedParameter);

		// Token: 0x06004CC7 RID: 19655
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegDeleteValue(SafeRegistryHandle hKey, string lpValueName);

		// Token: 0x06004CC8 RID: 19656
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegEnumKeyEx(SafeRegistryHandle hKey, int dwIndex, StringBuilder lpName, out int lpcbName, int[] lpReserved, StringBuilder lpClass, int[] lpcbClass, long[] lpftLastWriteTime);

		// Token: 0x06004CC9 RID: 19657
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegEnumValue(SafeRegistryHandle hKey, int dwIndex, StringBuilder lpValueName, ref int lpcbValueName, IntPtr lpReserved_MustBeZero, int[] lpType, byte[] lpData, int[] lpcbData);

		// Token: 0x06004CCA RID: 19658
		[DllImport("advapi32.dll")]
		internal static extern int RegFlushKey(SafeRegistryHandle hKey);

		// Token: 0x06004CCB RID: 19659
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegOpenKeyEx(SafeRegistryHandle hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle hkResult);

		// Token: 0x06004CCC RID: 19660
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegOpenKeyTransacted(SafeRegistryHandle hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle hkResult, SafeTransactionHandle hTransaction, IntPtr pExtendedParameter);

		// Token: 0x06004CCD RID: 19661
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryInfoKey(SafeRegistryHandle hKey, StringBuilder lpClass, int[] lpcbClass, IntPtr lpReserved_MustBeZero, ref int lpcSubKeys, int[] lpcbMaxSubKeyLen, int[] lpcbMaxClassLen, ref int lpcValues, int[] lpcbMaxValueNameLen, int[] lpcbMaxValueLen, int[] lpcbSecurityDescriptor, int[] lpftLastWriteTime);

		// Token: 0x06004CCE RID: 19662
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] byte[] lpData, ref int lpcbData);

		// Token: 0x06004CCF RID: 19663
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, ref int lpData, ref int lpcbData);

		// Token: 0x06004CD0 RID: 19664
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, ref long lpData, ref int lpcbData);

		// Token: 0x06004CD1 RID: 19665
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] char[] lpData, ref int lpcbData);

		// Token: 0x06004CD2 RID: 19666
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, StringBuilder lpData, ref int lpcbData);

		// Token: 0x06004CD3 RID: 19667
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, byte[] lpData, int cbData);

		// Token: 0x06004CD4 RID: 19668
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, ref int lpData, int cbData);

		// Token: 0x06004CD5 RID: 19669
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, ref long lpData, int cbData);

		// Token: 0x06004CD6 RID: 19670
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, string lpData, int cbData);

		// Token: 0x06004CD7 RID: 19671
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegCreateKeyTransacted(SafeRegistryHandle hKey, string lpSubKey, int Reserved, string lpClass, int dwOptions, int samDesigner, Win32Native.SECURITY_ATTRIBUTES lpSecurityAttributes, out SafeRegistryHandle hkResult, out int lpdwDisposition, SafeTransactionHandle hTransaction, IntPtr pExtendedParameter);

		// Token: 0x06004CD8 RID: 19672
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode)]
		internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

		// Token: 0x06004CD9 RID: 19673 RVA: 0x001957CC File Offset: 0x001939CC
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = Win32Native.FormatMessage(12800, Win32Native.NULL, errorCode, 0, stringBuilder, stringBuilder.Capacity, Win32Native.NULL);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			string unknownError_Num = RegistryProviderStrings.UnknownError_Num;
			return string.Format(CultureInfo.CurrentCulture, unknownError_Num, new object[]
			{
				errorCode.ToString(CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x06004CDA RID: 19674
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int lstrlen(sbyte[] ptr);

		// Token: 0x06004CDB RID: 19675
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int lstrlen(IntPtr ptr);

		// Token: 0x06004CDC RID: 19676
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		internal static extern int lstrlenA(IntPtr ptr);

		// Token: 0x06004CDD RID: 19677
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int lstrlenW(IntPtr ptr);

		// Token: 0x06004CDE RID: 19678
		[DllImport("kernel32.dll")]
		internal static extern UIntPtr VirtualQuery(UIntPtr lpAddress, ref Win32Native.MEMORY_BASIC_INFORMATION lpBuffer, UIntPtr dwLength);

		// Token: 0x06004CDF RID: 19679
		[DllImport("kernel32.dll")]
		internal static extern void GetSystemInfo(out Win32Native.SYSTEM_INFO lpSystemInfo);

		// Token: 0x06004CE0 RID: 19680 RVA: 0x0019583C File Offset: 0x00193A3C
		static Win32Native()
		{
			Win32Native.SYSTEM_INFO system_INFO = default(Win32Native.SYSTEM_INFO);
			Win32Native.GetSystemInfo(out system_INFO);
			Win32Native.PAGE_SIZE = (uint)system_INFO.dwPageSize;
		}

		// Token: 0x0400253E RID: 9534
		internal const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x0400253F RID: 9535
		internal const int SECURITY_ANONYMOUS = 0;

		// Token: 0x04002540 RID: 9536
		internal const int SECURITY_SQOS_PRESENT = 1048576;

		// Token: 0x04002541 RID: 9537
		private const string resBaseName = "RegistryProviderStrings";

		// Token: 0x04002542 RID: 9538
		internal const int KEY_QUERY_VALUE = 1;

		// Token: 0x04002543 RID: 9539
		internal const int KEY_SET_VALUE = 2;

		// Token: 0x04002544 RID: 9540
		internal const int KEY_CREATE_SUB_KEY = 4;

		// Token: 0x04002545 RID: 9541
		internal const int KEY_ENUMERATE_SUB_KEYS = 8;

		// Token: 0x04002546 RID: 9542
		internal const int KEY_NOTIFY = 16;

		// Token: 0x04002547 RID: 9543
		internal const int KEY_CREATE_LINK = 32;

		// Token: 0x04002548 RID: 9544
		internal const int KEY_READ = 131097;

		// Token: 0x04002549 RID: 9545
		internal const int KEY_WRITE = 131078;

		// Token: 0x0400254A RID: 9546
		internal const int KEY_WOW64_64KEY = 256;

		// Token: 0x0400254B RID: 9547
		internal const int KEY_WOW64_32KEY = 512;

		// Token: 0x0400254C RID: 9548
		internal const int REG_NONE = 0;

		// Token: 0x0400254D RID: 9549
		internal const int REG_SZ = 1;

		// Token: 0x0400254E RID: 9550
		internal const int REG_EXPAND_SZ = 2;

		// Token: 0x0400254F RID: 9551
		internal const int REG_BINARY = 3;

		// Token: 0x04002550 RID: 9552
		internal const int REG_DWORD = 4;

		// Token: 0x04002551 RID: 9553
		internal const int REG_DWORD_LITTLE_ENDIAN = 4;

		// Token: 0x04002552 RID: 9554
		internal const int REG_DWORD_BIG_ENDIAN = 5;

		// Token: 0x04002553 RID: 9555
		internal const int REG_LINK = 6;

		// Token: 0x04002554 RID: 9556
		internal const int REG_MULTI_SZ = 7;

		// Token: 0x04002555 RID: 9557
		internal const int REG_RESOURCE_LIST = 8;

		// Token: 0x04002556 RID: 9558
		internal const int REG_FULL_RESOURCE_DESCRIPTOR = 9;

		// Token: 0x04002557 RID: 9559
		internal const int REG_RESOURCE_REQUIREMENTS_LIST = 10;

		// Token: 0x04002558 RID: 9560
		internal const int REG_QWORD = 11;

		// Token: 0x04002559 RID: 9561
		internal const int HWND_BROADCAST = 65535;

		// Token: 0x0400255A RID: 9562
		internal const int WM_SETTINGCHANGE = 26;

		// Token: 0x0400255B RID: 9563
		internal const uint CRYPTPROTECTMEMORY_BLOCK_SIZE = 16U;

		// Token: 0x0400255C RID: 9564
		internal const uint CRYPTPROTECTMEMORY_SAME_PROCESS = 0U;

		// Token: 0x0400255D RID: 9565
		internal const uint CRYPTPROTECTMEMORY_CROSS_PROCESS = 1U;

		// Token: 0x0400255E RID: 9566
		internal const uint CRYPTPROTECTMEMORY_SAME_LOGON = 2U;

		// Token: 0x0400255F RID: 9567
		internal const string MICROSOFT_KERBEROS_NAME = "Kerberos";

		// Token: 0x04002560 RID: 9568
		internal const uint ANONYMOUS_LOGON_LUID = 998U;

		// Token: 0x04002561 RID: 9569
		internal const int SECURITY_ANONYMOUS_LOGON_RID = 7;

		// Token: 0x04002562 RID: 9570
		internal const int SECURITY_AUTHENTICATED_USER_RID = 11;

		// Token: 0x04002563 RID: 9571
		internal const int SECURITY_LOCAL_SYSTEM_RID = 18;

		// Token: 0x04002564 RID: 9572
		internal const int SECURITY_BUILTIN_DOMAIN_RID = 32;

		// Token: 0x04002565 RID: 9573
		internal const int DOMAIN_USER_RID_GUEST = 501;

		// Token: 0x04002566 RID: 9574
		internal const uint SE_GROUP_MANDATORY = 1U;

		// Token: 0x04002567 RID: 9575
		internal const uint SE_GROUP_ENABLED_BY_DEFAULT = 2U;

		// Token: 0x04002568 RID: 9576
		internal const uint SE_GROUP_ENABLED = 4U;

		// Token: 0x04002569 RID: 9577
		internal const uint SE_GROUP_OWNER = 8U;

		// Token: 0x0400256A RID: 9578
		internal const uint SE_GROUP_USE_FOR_DENY_ONLY = 16U;

		// Token: 0x0400256B RID: 9579
		internal const uint SE_GROUP_LOGON_ID = 3221225472U;

		// Token: 0x0400256C RID: 9580
		internal const uint SE_GROUP_RESOURCE = 536870912U;

		// Token: 0x0400256D RID: 9581
		internal const uint DUPLICATE_CLOSE_SOURCE = 1U;

		// Token: 0x0400256E RID: 9582
		internal const uint DUPLICATE_SAME_ACCESS = 2U;

		// Token: 0x0400256F RID: 9583
		internal const uint DUPLICATE_SAME_ATTRIBUTES = 4U;

		// Token: 0x04002570 RID: 9584
		internal const int READ_CONTROL = 131072;

		// Token: 0x04002571 RID: 9585
		internal const int SYNCHRONIZE = 1048576;

		// Token: 0x04002572 RID: 9586
		internal const int STANDARD_RIGHTS_READ = 131072;

		// Token: 0x04002573 RID: 9587
		internal const int STANDARD_RIGHTS_WRITE = 131072;

		// Token: 0x04002574 RID: 9588
		internal const int SEMAPHORE_MODIFY_STATE = 2;

		// Token: 0x04002575 RID: 9589
		internal const int EVENT_MODIFY_STATE = 2;

		// Token: 0x04002576 RID: 9590
		internal const int MUTEX_MODIFY_STATE = 1;

		// Token: 0x04002577 RID: 9591
		internal const int MUTEX_ALL_ACCESS = 2031617;

		// Token: 0x04002578 RID: 9592
		internal const int LMEM_FIXED = 0;

		// Token: 0x04002579 RID: 9593
		internal const int LMEM_ZEROINIT = 64;

		// Token: 0x0400257A RID: 9594
		internal const int LPTR = 64;

		// Token: 0x0400257B RID: 9595
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x0400257C RID: 9596
		internal const string USER32 = "user32.dll";

		// Token: 0x0400257D RID: 9597
		internal const string ADVAPI32 = "advapi32.dll";

		// Token: 0x0400257E RID: 9598
		internal const string OLE32 = "ole32.dll";

		// Token: 0x0400257F RID: 9599
		internal const string OLEAUT32 = "oleaut32.dll";

		// Token: 0x04002580 RID: 9600
		internal const string SHFOLDER = "shfolder.dll";

		// Token: 0x04002581 RID: 9601
		internal const string SHIM = "mscoree.dll";

		// Token: 0x04002582 RID: 9602
		internal const string CRYPT32 = "crypt32.dll";

		// Token: 0x04002583 RID: 9603
		internal const string SECUR32 = "secur32.dll";

		// Token: 0x04002584 RID: 9604
		internal const string MSCORWKS = "mscorwks.dll";

		// Token: 0x04002585 RID: 9605
		internal const string LSTRCPY = "lstrcpy";

		// Token: 0x04002586 RID: 9606
		internal const string LSTRCPYN = "lstrcpyn";

		// Token: 0x04002587 RID: 9607
		internal const string LSTRLEN = "lstrlen";

		// Token: 0x04002588 RID: 9608
		internal const string LSTRLENA = "lstrlenA";

		// Token: 0x04002589 RID: 9609
		internal const string LSTRLENW = "lstrlenW";

		// Token: 0x0400258A RID: 9610
		internal const string MOVEMEMORY = "RtlMoveMemory";

		// Token: 0x0400258B RID: 9611
		internal const int SEM_FAILCRITICALERRORS = 1;

		// Token: 0x0400258C RID: 9612
		internal const int ERROR_SUCCESS = 0;

		// Token: 0x0400258D RID: 9613
		internal const int ERROR_INVALID_FUNCTION = 1;

		// Token: 0x0400258E RID: 9614
		internal const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x0400258F RID: 9615
		internal const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x04002590 RID: 9616
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04002591 RID: 9617
		internal const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x04002592 RID: 9618
		internal const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x04002593 RID: 9619
		internal const int ERROR_INVALID_DATA = 13;

		// Token: 0x04002594 RID: 9620
		internal const int ERROR_INVALID_DRIVE = 15;

		// Token: 0x04002595 RID: 9621
		internal const int ERROR_NO_MORE_FILES = 18;

		// Token: 0x04002596 RID: 9622
		internal const int ERROR_NOT_READY = 21;

		// Token: 0x04002597 RID: 9623
		internal const int ERROR_BAD_LENGTH = 24;

		// Token: 0x04002598 RID: 9624
		internal const int ERROR_SHARING_VIOLATION = 32;

		// Token: 0x04002599 RID: 9625
		internal const int ERROR_NOT_SUPPORTED = 50;

		// Token: 0x0400259A RID: 9626
		internal const int ERROR_FILE_EXISTS = 80;

		// Token: 0x0400259B RID: 9627
		internal const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x0400259C RID: 9628
		internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;

		// Token: 0x0400259D RID: 9629
		internal const int ERROR_INVALID_NAME = 123;

		// Token: 0x0400259E RID: 9630
		internal const int ERROR_BAD_PATHNAME = 161;

		// Token: 0x0400259F RID: 9631
		internal const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x040025A0 RID: 9632
		internal const int ERROR_ENVVAR_NOT_FOUND = 203;

		// Token: 0x040025A1 RID: 9633
		internal const int ERROR_FILENAME_EXCED_RANGE = 206;

		// Token: 0x040025A2 RID: 9634
		internal const int ERROR_NO_DATA = 232;

		// Token: 0x040025A3 RID: 9635
		internal const int ERROR_PIPE_NOT_CONNECTED = 233;

		// Token: 0x040025A4 RID: 9636
		internal const int ERROR_MORE_DATA = 234;

		// Token: 0x040025A5 RID: 9637
		internal const int ERROR_OPERATION_ABORTED = 995;

		// Token: 0x040025A6 RID: 9638
		internal const int ERROR_NO_TOKEN = 1008;

		// Token: 0x040025A7 RID: 9639
		internal const int ERROR_DLL_INIT_FAILED = 1114;

		// Token: 0x040025A8 RID: 9640
		internal const int ERROR_NON_ACCOUNT_SID = 1257;

		// Token: 0x040025A9 RID: 9641
		internal const int ERROR_NOT_ALL_ASSIGNED = 1300;

		// Token: 0x040025AA RID: 9642
		internal const int ERROR_UNKNOWN_REVISION = 1305;

		// Token: 0x040025AB RID: 9643
		internal const int ERROR_INVALID_OWNER = 1307;

		// Token: 0x040025AC RID: 9644
		internal const int ERROR_INVALID_PRIMARY_GROUP = 1308;

		// Token: 0x040025AD RID: 9645
		internal const int ERROR_NO_SUCH_PRIVILEGE = 1313;

		// Token: 0x040025AE RID: 9646
		internal const int ERROR_PRIVILEGE_NOT_HELD = 1314;

		// Token: 0x040025AF RID: 9647
		internal const int ERROR_NONE_MAPPED = 1332;

		// Token: 0x040025B0 RID: 9648
		internal const int ERROR_INVALID_ACL = 1336;

		// Token: 0x040025B1 RID: 9649
		internal const int ERROR_INVALID_SID = 1337;

		// Token: 0x040025B2 RID: 9650
		internal const int ERROR_INVALID_SECURITY_DESCR = 1338;

		// Token: 0x040025B3 RID: 9651
		internal const int ERROR_BAD_IMPERSONATION_LEVEL = 1346;

		// Token: 0x040025B4 RID: 9652
		internal const int ERROR_CANT_OPEN_ANONYMOUS = 1347;

		// Token: 0x040025B5 RID: 9653
		internal const int ERROR_NO_SECURITY_ON_OBJECT = 1350;

		// Token: 0x040025B6 RID: 9654
		internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 1789;

		// Token: 0x040025B7 RID: 9655
		internal const int ERROR_MIN_KTM_CODE = 6700;

		// Token: 0x040025B8 RID: 9656
		internal const int ERROR_INVALID_TRANSACTION = 6700;

		// Token: 0x040025B9 RID: 9657
		internal const int ERROR_MAX_KTM_CODE = 6799;

		// Token: 0x040025BA RID: 9658
		internal const uint STATUS_SUCCESS = 0U;

		// Token: 0x040025BB RID: 9659
		internal const uint STATUS_SOME_NOT_MAPPED = 263U;

		// Token: 0x040025BC RID: 9660
		internal const uint STATUS_NO_MEMORY = 3221225495U;

		// Token: 0x040025BD RID: 9661
		internal const uint STATUS_OBJECT_NAME_NOT_FOUND = 3221225524U;

		// Token: 0x040025BE RID: 9662
		internal const uint STATUS_NONE_MAPPED = 3221225587U;

		// Token: 0x040025BF RID: 9663
		internal const uint STATUS_INSUFFICIENT_RESOURCES = 3221225626U;

		// Token: 0x040025C0 RID: 9664
		internal const uint STATUS_ACCESS_DENIED = 3221225506U;

		// Token: 0x040025C1 RID: 9665
		internal const int INVALID_FILE_SIZE = -1;

		// Token: 0x040025C2 RID: 9666
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040025C3 RID: 9667
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040025C4 RID: 9668
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x040025C5 RID: 9669
		internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		// Token: 0x040025C6 RID: 9670
		internal static readonly IntPtr NULL = IntPtr.Zero;

		// Token: 0x040025C7 RID: 9671
		internal static readonly uint PAGE_SIZE;

		// Token: 0x02000790 RID: 1936
		internal enum TOKEN_INFORMATION_CLASS
		{
			// Token: 0x040025C9 RID: 9673
			TokenUser = 1,
			// Token: 0x040025CA RID: 9674
			TokenGroups,
			// Token: 0x040025CB RID: 9675
			TokenPrivileges,
			// Token: 0x040025CC RID: 9676
			TokenOwner,
			// Token: 0x040025CD RID: 9677
			TokenPrimaryGroup,
			// Token: 0x040025CE RID: 9678
			TokenDefaultDacl,
			// Token: 0x040025CF RID: 9679
			TokenSource,
			// Token: 0x040025D0 RID: 9680
			TokenType,
			// Token: 0x040025D1 RID: 9681
			TokenImpersonationLevel,
			// Token: 0x040025D2 RID: 9682
			TokenStatistics,
			// Token: 0x040025D3 RID: 9683
			TokenRestrictedSids,
			// Token: 0x040025D4 RID: 9684
			TokenSessionId,
			// Token: 0x040025D5 RID: 9685
			TokenGroupsAndPrivileges,
			// Token: 0x040025D6 RID: 9686
			TokenSessionReference,
			// Token: 0x040025D7 RID: 9687
			TokenSandBoxInert,
			// Token: 0x040025D8 RID: 9688
			TokenAuditPolicy,
			// Token: 0x040025D9 RID: 9689
			TokenOrigin
		}

		// Token: 0x02000791 RID: 1937
		internal enum SID_NAME_USE
		{
			// Token: 0x040025DB RID: 9691
			SidTypeUser = 1,
			// Token: 0x040025DC RID: 9692
			SidTypeGroup,
			// Token: 0x040025DD RID: 9693
			SidTypeDomain,
			// Token: 0x040025DE RID: 9694
			SidTypeAlias,
			// Token: 0x040025DF RID: 9695
			SidTypeWellKnownGroup,
			// Token: 0x040025E0 RID: 9696
			SidTypeDeletedAccount,
			// Token: 0x040025E1 RID: 9697
			SidTypeInvalid,
			// Token: 0x040025E2 RID: 9698
			SidTypeUnknown,
			// Token: 0x040025E3 RID: 9699
			SidTypeComputer
		}

		// Token: 0x02000792 RID: 1938
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct SID_AND_ATTRIBUTES
		{
			// Token: 0x040025E4 RID: 9700
			internal IntPtr Sid;

			// Token: 0x040025E5 RID: 9701
			internal uint Attributes;
		}

		// Token: 0x02000793 RID: 1939
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_USER
		{
			// Token: 0x040025E6 RID: 9702
			internal Win32Native.SID_AND_ATTRIBUTES User;
		}

		// Token: 0x02000794 RID: 1940
		internal enum SECURITY_IMPERSONATION_LEVEL : short
		{
			// Token: 0x040025E8 RID: 9704
			Anonymous,
			// Token: 0x040025E9 RID: 9705
			Identification,
			// Token: 0x040025EA RID: 9706
			Impersonation,
			// Token: 0x040025EB RID: 9707
			Delegation = 4
		}

		// Token: 0x02000795 RID: 1941
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class OSVERSIONINFO
		{
			// Token: 0x06004CE1 RID: 19681 RVA: 0x00195879 File Offset: 0x00193A79
			internal OSVERSIONINFO()
			{
				this.OSVersionInfoSize = Marshal.SizeOf(this);
			}

			// Token: 0x040025EC RID: 9708
			internal int OSVersionInfoSize;

			// Token: 0x040025ED RID: 9709
			internal int MajorVersion;

			// Token: 0x040025EE RID: 9710
			internal int MinorVersion;

			// Token: 0x040025EF RID: 9711
			internal int BuildNumber;

			// Token: 0x040025F0 RID: 9712
			internal int PlatformId;

			// Token: 0x040025F1 RID: 9713
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string CSDVersion;
		}

		// Token: 0x02000796 RID: 1942
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class OSVERSIONINFOEX
		{
			// Token: 0x06004CE2 RID: 19682 RVA: 0x0019588D File Offset: 0x00193A8D
			public OSVERSIONINFOEX()
			{
				this.OSVersionInfoSize = Marshal.SizeOf(this);
			}

			// Token: 0x040025F2 RID: 9714
			internal int OSVersionInfoSize;

			// Token: 0x040025F3 RID: 9715
			internal int MajorVersion;

			// Token: 0x040025F4 RID: 9716
			internal int MinorVersion;

			// Token: 0x040025F5 RID: 9717
			internal int BuildNumber;

			// Token: 0x040025F6 RID: 9718
			internal int PlatformId;

			// Token: 0x040025F7 RID: 9719
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string CSDVersion;

			// Token: 0x040025F8 RID: 9720
			internal ushort ServicePackMajor;

			// Token: 0x040025F9 RID: 9721
			internal ushort ServicePackMinor;

			// Token: 0x040025FA RID: 9722
			internal short SuiteMask;

			// Token: 0x040025FB RID: 9723
			internal byte ProductType;

			// Token: 0x040025FC RID: 9724
			internal byte Reserved;
		}

		// Token: 0x02000797 RID: 1943
		internal struct SYSTEM_INFO
		{
			// Token: 0x040025FD RID: 9725
			internal int dwOemId;

			// Token: 0x040025FE RID: 9726
			internal int dwPageSize;

			// Token: 0x040025FF RID: 9727
			internal IntPtr lpMinimumApplicationAddress;

			// Token: 0x04002600 RID: 9728
			internal IntPtr lpMaximumApplicationAddress;

			// Token: 0x04002601 RID: 9729
			internal IntPtr dwActiveProcessorMask;

			// Token: 0x04002602 RID: 9730
			internal int dwNumberOfProcessors;

			// Token: 0x04002603 RID: 9731
			internal int dwProcessorType;

			// Token: 0x04002604 RID: 9732
			internal int dwAllocationGranularity;

			// Token: 0x04002605 RID: 9733
			internal short wProcessorLevel;

			// Token: 0x04002606 RID: 9734
			internal short wProcessorRevision;
		}

		// Token: 0x02000798 RID: 1944
		[StructLayout(LayoutKind.Sequential)]
		internal class SECURITY_ATTRIBUTES
		{
			// Token: 0x04002607 RID: 9735
			internal int nLength;

			// Token: 0x04002608 RID: 9736
			internal unsafe byte* pSecurityDescriptor = null;

			// Token: 0x04002609 RID: 9737
			internal int bInheritHandle;
		}

		// Token: 0x02000799 RID: 1945
		[Serializable]
		internal struct WIN32_FILE_ATTRIBUTE_DATA
		{
			// Token: 0x0400260A RID: 9738
			internal int fileAttributes;

			// Token: 0x0400260B RID: 9739
			internal uint ftCreationTimeLow;

			// Token: 0x0400260C RID: 9740
			internal uint ftCreationTimeHigh;

			// Token: 0x0400260D RID: 9741
			internal uint ftLastAccessTimeLow;

			// Token: 0x0400260E RID: 9742
			internal uint ftLastAccessTimeHigh;

			// Token: 0x0400260F RID: 9743
			internal uint ftLastWriteTimeLow;

			// Token: 0x04002610 RID: 9744
			internal uint ftLastWriteTimeHigh;

			// Token: 0x04002611 RID: 9745
			internal int fileSizeHigh;

			// Token: 0x04002612 RID: 9746
			internal int fileSizeLow;
		}

		// Token: 0x0200079A RID: 1946
		internal struct FILE_TIME
		{
			// Token: 0x06004CE4 RID: 19684 RVA: 0x001958B1 File Offset: 0x00193AB1
			public FILE_TIME(long fileTime)
			{
				this.ftTimeLow = (uint)fileTime;
				this.ftTimeHigh = (uint)(fileTime >> 32);
			}

			// Token: 0x06004CE5 RID: 19685 RVA: 0x001958C6 File Offset: 0x00193AC6
			public long ToTicks()
			{
				return (long)(((ulong)this.ftTimeHigh << 32) + (ulong)this.ftTimeLow);
			}

			// Token: 0x04002613 RID: 9747
			internal uint ftTimeLow;

			// Token: 0x04002614 RID: 9748
			internal uint ftTimeHigh;
		}

		// Token: 0x0200079B RID: 1947
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct KERB_S4U_LOGON
		{
			// Token: 0x04002615 RID: 9749
			internal uint MessageType;

			// Token: 0x04002616 RID: 9750
			internal uint Flags;

			// Token: 0x04002617 RID: 9751
			internal Win32Native.UNICODE_INTPTR_STRING ClientUpn;

			// Token: 0x04002618 RID: 9752
			internal Win32Native.UNICODE_INTPTR_STRING ClientRealm;
		}

		// Token: 0x0200079C RID: 1948
		internal struct LSA_OBJECT_ATTRIBUTES
		{
			// Token: 0x04002619 RID: 9753
			internal int Length;

			// Token: 0x0400261A RID: 9754
			internal IntPtr RootDirectory;

			// Token: 0x0400261B RID: 9755
			internal IntPtr ObjectName;

			// Token: 0x0400261C RID: 9756
			internal int Attributes;

			// Token: 0x0400261D RID: 9757
			internal IntPtr SecurityDescriptor;

			// Token: 0x0400261E RID: 9758
			internal IntPtr SecurityQualityOfService;
		}

		// Token: 0x0200079D RID: 1949
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct UNICODE_STRING
		{
			// Token: 0x0400261F RID: 9759
			internal ushort Length;

			// Token: 0x04002620 RID: 9760
			internal ushort MaximumLength;

			// Token: 0x04002621 RID: 9761
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string Buffer;
		}

		// Token: 0x0200079E RID: 1950
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct UNICODE_INTPTR_STRING
		{
			// Token: 0x06004CE6 RID: 19686 RVA: 0x001958DA File Offset: 0x00193ADA
			internal UNICODE_INTPTR_STRING(int length, int maximumLength, IntPtr buffer)
			{
				this.Length = (ushort)length;
				this.MaxLength = (ushort)maximumLength;
				this.Buffer = buffer;
			}

			// Token: 0x04002622 RID: 9762
			internal ushort Length;

			// Token: 0x04002623 RID: 9763
			internal ushort MaxLength;

			// Token: 0x04002624 RID: 9764
			internal IntPtr Buffer;
		}

		// Token: 0x0200079F RID: 1951
		internal struct LSA_TRANSLATED_NAME
		{
			// Token: 0x04002625 RID: 9765
			internal int Use;

			// Token: 0x04002626 RID: 9766
			internal Win32Native.UNICODE_INTPTR_STRING Name;

			// Token: 0x04002627 RID: 9767
			internal int DomainIndex;
		}

		// Token: 0x020007A0 RID: 1952
		internal struct LSA_TRANSLATED_SID
		{
			// Token: 0x04002628 RID: 9768
			internal int Use;

			// Token: 0x04002629 RID: 9769
			internal uint Rid;

			// Token: 0x0400262A RID: 9770
			internal int DomainIndex;
		}

		// Token: 0x020007A1 RID: 1953
		internal struct LSA_TRANSLATED_SID2
		{
			// Token: 0x0400262B RID: 9771
			internal int Use;

			// Token: 0x0400262C RID: 9772
			internal IntPtr Sid;

			// Token: 0x0400262D RID: 9773
			internal int DomainIndex;

			// Token: 0x0400262E RID: 9774
			private uint Flags;
		}

		// Token: 0x020007A2 RID: 1954
		internal struct LSA_TRUST_INFORMATION
		{
			// Token: 0x0400262F RID: 9775
			internal Win32Native.UNICODE_INTPTR_STRING Name;

			// Token: 0x04002630 RID: 9776
			internal IntPtr Sid;
		}

		// Token: 0x020007A3 RID: 1955
		internal struct LSA_REFERENCED_DOMAIN_LIST
		{
			// Token: 0x04002631 RID: 9777
			internal int Entries;

			// Token: 0x04002632 RID: 9778
			internal IntPtr Domains;
		}

		// Token: 0x020007A4 RID: 1956
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct QUOTA_LIMITS
		{
			// Token: 0x04002633 RID: 9779
			internal IntPtr PagedPoolLimit;

			// Token: 0x04002634 RID: 9780
			internal IntPtr NonPagedPoolLimit;

			// Token: 0x04002635 RID: 9781
			internal IntPtr MinimumWorkingSetSize;

			// Token: 0x04002636 RID: 9782
			internal IntPtr MaximumWorkingSetSize;

			// Token: 0x04002637 RID: 9783
			internal IntPtr PagefileLimit;

			// Token: 0x04002638 RID: 9784
			internal IntPtr TimeLimit;
		}

		// Token: 0x020007A5 RID: 1957
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_GROUPS
		{
			// Token: 0x04002639 RID: 9785
			internal uint GroupCount;

			// Token: 0x0400263A RID: 9786
			internal Win32Native.SID_AND_ATTRIBUTES Groups;
		}

		// Token: 0x020007A6 RID: 1958
		[StructLayout(LayoutKind.Sequential)]
		internal class MEMORYSTATUSEX
		{
			// Token: 0x06004CE7 RID: 19687 RVA: 0x001958F3 File Offset: 0x00193AF3
			internal MEMORYSTATUSEX()
			{
				this.length = Marshal.SizeOf(this);
			}

			// Token: 0x0400263B RID: 9787
			internal int length;

			// Token: 0x0400263C RID: 9788
			internal int memoryLoad;

			// Token: 0x0400263D RID: 9789
			internal ulong totalPhys;

			// Token: 0x0400263E RID: 9790
			internal ulong availPhys;

			// Token: 0x0400263F RID: 9791
			internal ulong totalPageFile;

			// Token: 0x04002640 RID: 9792
			internal ulong availPageFile;

			// Token: 0x04002641 RID: 9793
			internal ulong totalVirtual;

			// Token: 0x04002642 RID: 9794
			internal ulong availVirtual;

			// Token: 0x04002643 RID: 9795
			internal ulong availExtendedVirtual;
		}

		// Token: 0x020007A7 RID: 1959
		[StructLayout(LayoutKind.Sequential)]
		internal class MEMORYSTATUS
		{
			// Token: 0x06004CE8 RID: 19688 RVA: 0x00195907 File Offset: 0x00193B07
			internal MEMORYSTATUS()
			{
				this.length = Marshal.SizeOf(this);
			}

			// Token: 0x04002644 RID: 9796
			internal int length;

			// Token: 0x04002645 RID: 9797
			internal int memoryLoad;

			// Token: 0x04002646 RID: 9798
			internal uint totalPhys;

			// Token: 0x04002647 RID: 9799
			internal uint availPhys;

			// Token: 0x04002648 RID: 9800
			internal uint totalPageFile;

			// Token: 0x04002649 RID: 9801
			internal uint availPageFile;

			// Token: 0x0400264A RID: 9802
			internal uint totalVirtual;

			// Token: 0x0400264B RID: 9803
			internal uint availVirtual;
		}

		// Token: 0x020007A8 RID: 1960
		internal struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x0400264C RID: 9804
			internal UIntPtr BaseAddress;

			// Token: 0x0400264D RID: 9805
			internal UIntPtr AllocationBase;

			// Token: 0x0400264E RID: 9806
			internal uint AllocationProtect;

			// Token: 0x0400264F RID: 9807
			internal UIntPtr RegionSize;

			// Token: 0x04002650 RID: 9808
			internal uint State;

			// Token: 0x04002651 RID: 9809
			internal uint Protect;

			// Token: 0x04002652 RID: 9810
			internal uint Type;
		}
	}
}
