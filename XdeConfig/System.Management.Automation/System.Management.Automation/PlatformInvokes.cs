using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation
{
	// Token: 0x020008C3 RID: 2243
	internal class PlatformInvokes
	{
		// Token: 0x0600551C RID: 21788
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int GetFileAttributes(string lpFileName);

		// Token: 0x0600551D RID: 21789
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr FindFirstFile(string lpFileName, out PlatformInvokes.WIN32_FIND_DATA lpFindFileData);

		// Token: 0x0600551E RID: 21790
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool FindNextFile(IntPtr hFindFile, out PlatformInvokes.WIN32_FIND_DATA lpFindFileData);

		// Token: 0x0600551F RID: 21791
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool FindClose(IntPtr hFindFile);

		// Token: 0x06005520 RID: 21792
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CreateFile(string lpFileName, PlatformInvokes.FileDesiredAccess dwDesiredAccess, PlatformInvokes.FileShareMode dwShareMode, IntPtr lpSecurityAttributes, PlatformInvokes.FileCreationDisposition dwCreationDisposition, PlatformInvokes.FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06005521 RID: 21793
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06005522 RID: 21794
		[DllImport("kernel32.dll")]
		internal static extern bool DosDateTimeToFileTime(short wFatDate, short wFatTime, PlatformInvokes.FILETIME lpFileTime);

		// Token: 0x06005523 RID: 21795
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool LocalFileTimeToFileTime(PlatformInvokes.FILETIME lpLocalFileTime, PlatformInvokes.FILETIME lpFileTime);

		// Token: 0x06005524 RID: 21796
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool SetFileTime(IntPtr hFile, PlatformInvokes.FILETIME lpCreationTime, PlatformInvokes.FILETIME lpLastAccessTime, PlatformInvokes.FILETIME lpLastWriteTime);

		// Token: 0x06005525 RID: 21797
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool SetFileAttributesW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, PlatformInvokes.FileAttributes dwFileAttributes);

		// Token: 0x06005526 RID: 21798 RVA: 0x001C12C8 File Offset: 0x001BF4C8
		internal static bool EnableTokenPrivilege(string privilegeName, ref PlatformInvokes.TOKEN_PRIVILEGE oldPrivilegeState)
		{
			bool result = false;
			PlatformInvokes.TOKEN_PRIVILEGE token_PRIVILEGE = default(PlatformInvokes.TOKEN_PRIVILEGE);
			if (PlatformInvokes.LookupPrivilegeValue(null, privilegeName, ref token_PRIVILEGE.Privilege.Luid))
			{
				IntPtr currentProcess = PlatformInvokes.GetCurrentProcess();
				if (currentProcess != IntPtr.Zero)
				{
					IntPtr zero = IntPtr.Zero;
					if (PlatformInvokes.OpenProcessToken(currentProcess, 40U, out zero))
					{
						PlatformInvokes.PRIVILEGE_SET privilege_SET = default(PlatformInvokes.PRIVILEGE_SET);
						privilege_SET.Privilege.Luid = token_PRIVILEGE.Privilege.Luid;
						privilege_SET.PrivilegeCount = 1U;
						privilege_SET.Control = 1U;
						bool flag = false;
						if (PlatformInvokes.PrivilegeCheck(zero, ref privilege_SET, out flag) && flag)
						{
							oldPrivilegeState.PrivilegeCount = 0U;
							result = true;
						}
						else
						{
							token_PRIVILEGE.PrivilegeCount = 1U;
							token_PRIVILEGE.Privilege.Attributes = 2U;
							int bufferLength = ClrFacade.SizeOf<PlatformInvokes.TOKEN_PRIVILEGE>();
							int num = 0;
							if (PlatformInvokes.AdjustTokenPrivileges(zero, false, ref token_PRIVILEGE, bufferLength, out oldPrivilegeState, ref num))
							{
								int lastWin32Error = Marshal.GetLastWin32Error();
								if (lastWin32Error == 0)
								{
									result = true;
								}
								else if (lastWin32Error == 1300)
								{
									oldPrivilegeState.PrivilegeCount = 0U;
									result = true;
								}
							}
						}
					}
					if (zero != IntPtr.Zero)
					{
						PlatformInvokes.CloseHandle(zero);
					}
					PlatformInvokes.CloseHandle(currentProcess);
				}
			}
			return result;
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x001C13DC File Offset: 0x001BF5DC
		internal static bool RestoreTokenPrivilege(string privilegeName, ref PlatformInvokes.TOKEN_PRIVILEGE previousPrivilegeState)
		{
			if (previousPrivilegeState.PrivilegeCount == 0U)
			{
				return true;
			}
			bool result = false;
			PlatformInvokes.TOKEN_PRIVILEGE token_PRIVILEGE = default(PlatformInvokes.TOKEN_PRIVILEGE);
			if (PlatformInvokes.LookupPrivilegeValue(null, privilegeName, ref token_PRIVILEGE.Privilege.Luid) && token_PRIVILEGE.Privilege.Luid.HighPart == previousPrivilegeState.Privilege.Luid.HighPart && token_PRIVILEGE.Privilege.Luid.LowPart == previousPrivilegeState.Privilege.Luid.LowPart)
			{
				IntPtr currentProcess = PlatformInvokes.GetCurrentProcess();
				if (currentProcess != IntPtr.Zero)
				{
					IntPtr zero = IntPtr.Zero;
					if (PlatformInvokes.OpenProcessToken(currentProcess, 40U, out zero))
					{
						int bufferLength = ClrFacade.SizeOf<PlatformInvokes.TOKEN_PRIVILEGE>();
						int num = 0;
						if (PlatformInvokes.AdjustTokenPrivileges(zero, false, ref previousPrivilegeState, bufferLength, out token_PRIVILEGE, ref num) && Marshal.GetLastWin32Error() == 0)
						{
							result = true;
						}
					}
					if (zero != IntPtr.Zero)
					{
						PlatformInvokes.CloseHandle(zero);
					}
					PlatformInvokes.CloseHandle(currentProcess);
				}
			}
			return result;
		}

		// Token: 0x06005528 RID: 21800
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref PlatformInvokes.LUID lpLuid);

		// Token: 0x06005529 RID: 21801
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool PrivilegeCheck(IntPtr tokenHandler, ref PlatformInvokes.PRIVILEGE_SET requiredPrivileges, out bool pfResult);

		// Token: 0x0600552A RID: 21802
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool AdjustTokenPrivileges(IntPtr tokenHandler, bool disableAllPrivilege, ref PlatformInvokes.TOKEN_PRIVILEGE newPrivilegeState, int bufferLength, out PlatformInvokes.TOKEN_PRIVILEGE previousPrivilegeState, ref int returnLength);

		// Token: 0x0600552B RID: 21803
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetCurrentProcess();

		// Token: 0x0600552C RID: 21804
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

		// Token: 0x04002C70 RID: 11376
		internal const int MAX_PATH = 260;

		// Token: 0x04002C71 RID: 11377
		internal const int MAX_ALTERNATE = 14;

		// Token: 0x04002C72 RID: 11378
		internal const int TOKEN_ADJUST_PRIVILEGES = 32;

		// Token: 0x04002C73 RID: 11379
		internal const int TOKEN_QUERY = 8;

		// Token: 0x04002C74 RID: 11380
		internal const int TOKEN_ALL_ACCESS = 2032127;

		// Token: 0x04002C75 RID: 11381
		internal const uint SE_PRIVILEGE_DISABLED = 0U;

		// Token: 0x04002C76 RID: 11382
		internal const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1U;

		// Token: 0x04002C77 RID: 11383
		internal const uint SE_PRIVILEGE_ENABLED = 2U;

		// Token: 0x04002C78 RID: 11384
		internal const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648U;

		// Token: 0x04002C79 RID: 11385
		internal const int ERROR_SUCCESS = 0;

		// Token: 0x020008C4 RID: 2244
		[StructLayout(LayoutKind.Sequential)]
		internal class FILETIME
		{
			// Token: 0x0600552E RID: 21806 RVA: 0x001C14CA File Offset: 0x001BF6CA
			internal FILETIME()
			{
				this.dwLowDateTime = 0U;
				this.dwHighDateTime = 0U;
			}

			// Token: 0x0600552F RID: 21807 RVA: 0x001C14E0 File Offset: 0x001BF6E0
			internal FILETIME(long fileTime)
			{
				this.dwLowDateTime = (uint)fileTime;
				this.dwHighDateTime = (uint)(fileTime >> 32);
			}

			// Token: 0x06005530 RID: 21808 RVA: 0x001C14FB File Offset: 0x001BF6FB
			public long ToTicks()
			{
				return (long)(((ulong)this.dwHighDateTime << 32) + (ulong)this.dwLowDateTime);
			}

			// Token: 0x04002C7A RID: 11386
			internal uint dwLowDateTime;

			// Token: 0x04002C7B RID: 11387
			internal uint dwHighDateTime;
		}

		// Token: 0x020008C5 RID: 2245
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WIN32_FIND_DATA
		{
			// Token: 0x04002C7C RID: 11388
			internal PlatformInvokes.FileAttributes dwFileAttributes;

			// Token: 0x04002C7D RID: 11389
			internal PlatformInvokes.FILETIME ftCreationTime;

			// Token: 0x04002C7E RID: 11390
			internal PlatformInvokes.FILETIME ftLastAccessTime;

			// Token: 0x04002C7F RID: 11391
			internal PlatformInvokes.FILETIME ftLastWriteTime;

			// Token: 0x04002C80 RID: 11392
			internal uint nFileSizeHigh;

			// Token: 0x04002C81 RID: 11393
			internal uint nFileSizeLow;

			// Token: 0x04002C82 RID: 11394
			internal uint dwReserved0;

			// Token: 0x04002C83 RID: 11395
			internal uint dwReserved1;

			// Token: 0x04002C84 RID: 11396
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string cFileName;

			// Token: 0x04002C85 RID: 11397
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			internal string cAlternate;
		}

		// Token: 0x020008C6 RID: 2246
		[Flags]
		internal enum FileDesiredAccess : uint
		{
			// Token: 0x04002C87 RID: 11399
			GenericRead = 2147483648U,
			// Token: 0x04002C88 RID: 11400
			GenericWrite = 1073741824U,
			// Token: 0x04002C89 RID: 11401
			GenericExecute = 536870912U,
			// Token: 0x04002C8A RID: 11402
			GenericAll = 268435456U
		}

		// Token: 0x020008C7 RID: 2247
		[Flags]
		internal enum FileShareMode : uint
		{
			// Token: 0x04002C8C RID: 11404
			None = 0U,
			// Token: 0x04002C8D RID: 11405
			Read = 1U,
			// Token: 0x04002C8E RID: 11406
			Write = 2U,
			// Token: 0x04002C8F RID: 11407
			Delete = 4U
		}

		// Token: 0x020008C8 RID: 2248
		internal enum FileCreationDisposition : uint
		{
			// Token: 0x04002C91 RID: 11409
			New = 1U,
			// Token: 0x04002C92 RID: 11410
			CreateAlways,
			// Token: 0x04002C93 RID: 11411
			OpenExisting,
			// Token: 0x04002C94 RID: 11412
			OpenAlways,
			// Token: 0x04002C95 RID: 11413
			TruncateExisting
		}

		// Token: 0x020008C9 RID: 2249
		[Flags]
		internal enum FileAttributes : uint
		{
			// Token: 0x04002C97 RID: 11415
			ReadOnly = 1U,
			// Token: 0x04002C98 RID: 11416
			Hidden = 2U,
			// Token: 0x04002C99 RID: 11417
			System = 4U,
			// Token: 0x04002C9A RID: 11418
			Directory = 16U,
			// Token: 0x04002C9B RID: 11419
			Archive = 32U,
			// Token: 0x04002C9C RID: 11420
			Normal = 128U,
			// Token: 0x04002C9D RID: 11421
			Temporary = 256U,
			// Token: 0x04002C9E RID: 11422
			Offline = 4096U,
			// Token: 0x04002C9F RID: 11423
			NotContentIndexed = 8192U,
			// Token: 0x04002CA0 RID: 11424
			Encrypted = 16384U,
			// Token: 0x04002CA1 RID: 11425
			Write_Through = 2147483648U,
			// Token: 0x04002CA2 RID: 11426
			Overlapped = 1073741824U,
			// Token: 0x04002CA3 RID: 11427
			NoBuffering = 536870912U,
			// Token: 0x04002CA4 RID: 11428
			RandomAccess = 268435456U,
			// Token: 0x04002CA5 RID: 11429
			SequentialScan = 134217728U,
			// Token: 0x04002CA6 RID: 11430
			DeleteOnClose = 67108864U,
			// Token: 0x04002CA7 RID: 11431
			BackupSemantics = 33554432U,
			// Token: 0x04002CA8 RID: 11432
			PosixSemantics = 16777216U,
			// Token: 0x04002CA9 RID: 11433
			OpenReparsePoint = 2097152U,
			// Token: 0x04002CAA RID: 11434
			OpenNoRecall = 1048576U,
			// Token: 0x04002CAB RID: 11435
			SessionAware = 8388608U
		}

		// Token: 0x020008CA RID: 2250
		[StructLayout(LayoutKind.Sequential)]
		internal class SecurityAttributes
		{
			// Token: 0x06005531 RID: 21809 RVA: 0x001C150F File Offset: 0x001BF70F
			internal SecurityAttributes()
			{
				this.nLength = 12;
				this.bInheritHandle = true;
				this.lpSecurityDescriptor = new PlatformInvokes.SafeLocalMemHandle(IntPtr.Zero, true);
			}

			// Token: 0x04002CAC RID: 11436
			internal int nLength;

			// Token: 0x04002CAD RID: 11437
			internal PlatformInvokes.SafeLocalMemHandle lpSecurityDescriptor;

			// Token: 0x04002CAE RID: 11438
			internal bool bInheritHandle;
		}

		// Token: 0x020008CB RID: 2251
		internal sealed class SafeLocalMemHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x06005532 RID: 21810 RVA: 0x001C1537 File Offset: 0x001BF737
			internal SafeLocalMemHandle() : base(true)
			{
			}

			// Token: 0x06005533 RID: 21811 RVA: 0x001C1540 File Offset: 0x001BF740
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			internal SafeLocalMemHandle(IntPtr existingHandle, bool ownsHandle) : base(ownsHandle)
			{
				base.SetHandle(existingHandle);
			}

			// Token: 0x06005534 RID: 21812
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[DllImport("kernel32.dll")]
			private static extern IntPtr LocalFree(IntPtr hMem);

			// Token: 0x06005535 RID: 21813 RVA: 0x001C1550 File Offset: 0x001BF750
			protected override bool ReleaseHandle()
			{
				return PlatformInvokes.SafeLocalMemHandle.LocalFree(this.handle) == IntPtr.Zero;
			}
		}

		// Token: 0x020008CC RID: 2252
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_PRIVILEGE
		{
			// Token: 0x04002CAF RID: 11439
			internal uint PrivilegeCount;

			// Token: 0x04002CB0 RID: 11440
			internal PlatformInvokes.LUID_AND_ATTRIBUTES Privilege;
		}

		// Token: 0x020008CD RID: 2253
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID
		{
			// Token: 0x04002CB1 RID: 11441
			internal uint LowPart;

			// Token: 0x04002CB2 RID: 11442
			internal uint HighPart;
		}

		// Token: 0x020008CE RID: 2254
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID_AND_ATTRIBUTES
		{
			// Token: 0x04002CB3 RID: 11443
			internal PlatformInvokes.LUID Luid;

			// Token: 0x04002CB4 RID: 11444
			internal uint Attributes;
		}

		// Token: 0x020008CF RID: 2255
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct PRIVILEGE_SET
		{
			// Token: 0x04002CB5 RID: 11445
			internal uint PrivilegeCount;

			// Token: 0x04002CB6 RID: 11446
			internal uint Control;

			// Token: 0x04002CB7 RID: 11447
			internal PlatformInvokes.LUID_AND_ATTRIBUTES Privilege;
		}
	}
}
