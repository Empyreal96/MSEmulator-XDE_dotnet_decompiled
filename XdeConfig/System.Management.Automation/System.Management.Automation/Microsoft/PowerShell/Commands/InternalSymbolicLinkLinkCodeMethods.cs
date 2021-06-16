using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200077C RID: 1916
	public static class InternalSymbolicLinkLinkCodeMethods
	{
		// Token: 0x06004C83 RID: 19587
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr InBuffer, int nInBufferSize, IntPtr OutBuffer, int nOutBufferSize, out int pBytesReturned, IntPtr lpOverlapped);

		// Token: 0x06004C84 RID: 19588
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr FindFirstFileName(string lpFileName, uint flags, ref uint StringLength, StringBuilder LinkName);

		// Token: 0x06004C85 RID: 19589
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool FindNextFileName(IntPtr hFindStream, ref uint StringLength, StringBuilder LinkName);

		// Token: 0x06004C86 RID: 19590
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool FindClose(IntPtr hFindFile);

		// Token: 0x06004C87 RID: 19591
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool GetFileInformationByHandle(IntPtr hFile, out InternalSymbolicLinkLinkCodeMethods.BY_HANDLE_FILE_INFORMATION lpFileInformation);

		// Token: 0x06004C88 RID: 19592
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CreateFile(string lpFileName, InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess dwDesiredAccess, InternalSymbolicLinkLinkCodeMethods.FileShareMode dwShareMode, IntPtr lpSecurityAttributes, InternalSymbolicLinkLinkCodeMethods.FileCreationDisposition dwCreationDisposition, InternalSymbolicLinkLinkCodeMethods.FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06004C89 RID: 19593 RVA: 0x00194804 File Offset: 0x00192A04
		public static IEnumerable<string> GetTarget(PSObject instance)
		{
			FileSystemInfo fileSystemInfo = instance.BaseObject as FileSystemInfo;
			if (fileSystemInfo != null)
			{
				using (SafeFileHandle safeFileHandle = InternalSymbolicLinkLinkCodeMethods.OpenReparsePoint(fileSystemInfo.FullName, (InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess)2147483648U))
				{
					string text = InternalSymbolicLinkLinkCodeMethods.InternalGetTarget(safeFileHandle);
					if (text != null)
					{
						return new string[]
						{
							text
						};
					}
				}
				return InternalSymbolicLinkLinkCodeMethods.InternalGetTarget(fileSystemInfo.FullName);
			}
			return null;
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x00194878 File Offset: 0x00192A78
		public static string GetLinkType(PSObject instance)
		{
			FileSystemInfo fileSystemInfo = instance.BaseObject as FileSystemInfo;
			if (fileSystemInfo != null)
			{
				return InternalSymbolicLinkLinkCodeMethods.InternalGetLinkType(fileSystemInfo.FullName);
			}
			return null;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x001948A4 File Offset: 0x00192AA4
		private static List<string> InternalGetTarget(string filePath)
		{
			List<string> list = new List<string>();
			uint capacity = 0U;
			StringBuilder stringBuilder = new StringBuilder();
			IntPtr intPtr = InternalSymbolicLinkLinkCodeMethods.FindFirstFileName(filePath, 0U, ref capacity, stringBuilder);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (intPtr == (IntPtr)(-1) && lastWin32Error == 234)
			{
				stringBuilder = new StringBuilder((int)capacity);
				intPtr = InternalSymbolicLinkLinkCodeMethods.FindFirstFileName(filePath, 0U, ref capacity, stringBuilder);
				lastWin32Error = Marshal.GetLastWin32Error();
			}
			if (intPtr == (IntPtr)(-1))
			{
				throw new Win32Exception(lastWin32Error);
			}
			try
			{
				for (;;)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append(Path.GetPathRoot(filePath));
					stringBuilder2.Append(stringBuilder.ToString());
					FileInfo fileInfo = new FileInfo(stringBuilder2.ToString());
					if (string.Compare(fileInfo.FullName, filePath, StringComparison.OrdinalIgnoreCase) != 0)
					{
						list.Add(fileInfo.FullName);
					}
					bool flag = InternalSymbolicLinkLinkCodeMethods.FindNextFileName(intPtr, ref capacity, stringBuilder);
					lastWin32Error = Marshal.GetLastWin32Error();
					if (!flag && lastWin32Error == 234)
					{
						stringBuilder = new StringBuilder((int)capacity);
						flag = InternalSymbolicLinkLinkCodeMethods.FindNextFileName(intPtr, ref capacity, stringBuilder);
					}
					if (!flag && lastWin32Error != 38)
					{
						break;
					}
					if (!flag)
					{
						goto Block_10;
					}
				}
				throw new Win32Exception(lastWin32Error);
				Block_10:;
			}
			finally
			{
				InternalSymbolicLinkLinkCodeMethods.FindClose(intPtr);
			}
			return list;
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x001949D0 File Offset: 0x00192BD0
		private static string InternalGetLinkType(string filePath)
		{
			string result;
			using (SafeFileHandle safeFileHandle = InternalSymbolicLinkLinkCodeMethods.OpenReparsePoint(filePath, (InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess)2147483648U))
			{
				int num = ClrFacade.SizeOf<InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_SYMBOLICLINK>();
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				bool flag = false;
				try
				{
					safeFileHandle.DangerousAddRef(ref flag);
					IntPtr hDevice = safeFileHandle.DangerousGetHandle();
					int num2;
					if (!InternalSymbolicLinkLinkCodeMethods.DeviceIoControl(hDevice, 589992U, IntPtr.Zero, 0, intPtr, num, out num2, IntPtr.Zero))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error != 4390)
						{
							throw new Win32Exception(lastWin32Error);
						}
					}
					InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_SYMBOLICLINK reparse_DATA_BUFFER_SYMBOLICLINK = ClrFacade.PtrToStructure<InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_SYMBOLICLINK>(intPtr);
					string text;
					if (reparse_DATA_BUFFER_SYMBOLICLINK.ReparseTag == 2684354572U)
					{
						text = "SymbolicLink";
					}
					else if (reparse_DATA_BUFFER_SYMBOLICLINK.ReparseTag == 2684354563U)
					{
						text = "Junction";
					}
					else
					{
						text = (InternalSymbolicLinkLinkCodeMethods.IsHardLink(ref hDevice) ? "HardLink" : null);
					}
					result = text;
				}
				finally
				{
					if (flag)
					{
						safeFileHandle.DangerousRelease();
					}
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return result;
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x00194AD0 File Offset: 0x00192CD0
		internal static bool IsHardLink(ref IntPtr handle)
		{
			InternalSymbolicLinkLinkCodeMethods.BY_HANDLE_FILE_INFORMATION by_HANDLE_FILE_INFORMATION = default(InternalSymbolicLinkLinkCodeMethods.BY_HANDLE_FILE_INFORMATION);
			if (!InternalSymbolicLinkLinkCodeMethods.GetFileInformationByHandle(handle, out by_HANDLE_FILE_INFORMATION))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error);
			}
			return by_HANDLE_FILE_INFORMATION.NumberOfLinks > 1U;
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x00194B10 File Offset: 0x00192D10
		private static string InternalGetTarget(SafeFileHandle handle)
		{
			int num = ClrFacade.SizeOf<InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_SYMBOLICLINK>();
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			bool flag = false;
			string result;
			try
			{
				handle.DangerousAddRef(ref flag);
				int num2;
				if (!InternalSymbolicLinkLinkCodeMethods.DeviceIoControl(handle.DangerousGetHandle(), 589992U, IntPtr.Zero, 0, intPtr, num, out num2, IntPtr.Zero))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 4390)
					{
						throw new Win32Exception(lastWin32Error);
					}
					result = null;
				}
				else
				{
					InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_SYMBOLICLINK reparse_DATA_BUFFER_SYMBOLICLINK = ClrFacade.PtrToStructure<InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_SYMBOLICLINK>(intPtr);
					if (reparse_DATA_BUFFER_SYMBOLICLINK.ReparseTag != 2684354572U && reparse_DATA_BUFFER_SYMBOLICLINK.ReparseTag != 2684354563U)
					{
						result = null;
					}
					else
					{
						string text = null;
						if (reparse_DATA_BUFFER_SYMBOLICLINK.ReparseTag == 2684354572U)
						{
							text = Encoding.Unicode.GetString(reparse_DATA_BUFFER_SYMBOLICLINK.PathBuffer, (int)reparse_DATA_BUFFER_SYMBOLICLINK.SubstituteNameOffset, (int)reparse_DATA_BUFFER_SYMBOLICLINK.SubstituteNameLength);
						}
						if (reparse_DATA_BUFFER_SYMBOLICLINK.ReparseTag == 2684354563U)
						{
							InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_MOUNTPOINT reparse_DATA_BUFFER_MOUNTPOINT = ClrFacade.PtrToStructure<InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_MOUNTPOINT>(intPtr);
							text = Encoding.Unicode.GetString(reparse_DATA_BUFFER_MOUNTPOINT.PathBuffer, (int)reparse_DATA_BUFFER_MOUNTPOINT.SubstituteNameOffset, (int)reparse_DATA_BUFFER_MOUNTPOINT.SubstituteNameLength);
						}
						if (text.StartsWith("\\??\\", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring("\\??\\".Length);
						}
						result = text;
					}
				}
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x00194C64 File Offset: 0x00192E64
		internal static bool CreateJunction(string path, string target)
		{
			if (!string.IsNullOrEmpty(path))
			{
				if (!string.IsNullOrEmpty(target))
				{
					using (SafeHandle safeHandle = InternalSymbolicLinkLinkCodeMethods.OpenReparsePoint(path, InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess.GenericWrite))
					{
						byte[] bytes = Encoding.Unicode.GetBytes("\\??\\" + Path.GetFullPath(target));
						InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_MOUNTPOINT reparse_DATA_BUFFER_MOUNTPOINT = default(InternalSymbolicLinkLinkCodeMethods.REPARSE_DATA_BUFFER_MOUNTPOINT);
						reparse_DATA_BUFFER_MOUNTPOINT.ReparseTag = 2684354563U;
						reparse_DATA_BUFFER_MOUNTPOINT.ReparseDataLength = (ushort)(bytes.Length + 12);
						reparse_DATA_BUFFER_MOUNTPOINT.SubstituteNameOffset = 0;
						reparse_DATA_BUFFER_MOUNTPOINT.SubstituteNameLength = (ushort)bytes.Length;
						reparse_DATA_BUFFER_MOUNTPOINT.PrintNameOffset = (ushort)(bytes.Length + 2);
						reparse_DATA_BUFFER_MOUNTPOINT.PrintNameLength = 0;
						reparse_DATA_BUFFER_MOUNTPOINT.PathBuffer = new byte[16368];
						Array.Copy(bytes, reparse_DATA_BUFFER_MOUNTPOINT.PathBuffer, bytes.Length);
						int cb = Marshal.SizeOf(reparse_DATA_BUFFER_MOUNTPOINT);
						IntPtr intPtr = Marshal.AllocHGlobal(cb);
						bool flag = false;
						try
						{
							Marshal.StructureToPtr(reparse_DATA_BUFFER_MOUNTPOINT, intPtr, false);
							int num = 0;
							safeHandle.DangerousAddRef(ref flag);
							bool flag2 = InternalSymbolicLinkLinkCodeMethods.DeviceIoControl(safeHandle.DangerousGetHandle(), 589988U, intPtr, bytes.Length + 20, IntPtr.Zero, 0, out num, IntPtr.Zero);
							if (!flag2)
							{
								throw new Win32Exception(Marshal.GetLastWin32Error());
							}
							return flag2;
						}
						finally
						{
							Marshal.FreeHGlobal(intPtr);
							if (flag)
							{
								safeHandle.DangerousRelease();
							}
						}
					}
				}
				throw new ArgumentNullException("target");
			}
			throw new ArgumentNullException("path");
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x00194DF0 File Offset: 0x00192FF0
		internal static bool DeleteJunction(string junctionPath)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(junctionPath))
			{
				using (SafeHandle safeHandle = InternalSymbolicLinkLinkCodeMethods.OpenReparsePoint(junctionPath, InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess.GenericWrite))
				{
					bool flag2 = false;
					int num = ClrFacade.SizeOf<InternalSymbolicLinkLinkCodeMethods.REPARSE_GUID_DATA_BUFFER>();
					IntPtr intPtr = Marshal.AllocHGlobal(num);
					IntPtr intPtr2 = Marshal.AllocHGlobal(num);
					try
					{
						safeHandle.DangerousAddRef(ref flag2);
						IntPtr hDevice = safeHandle.DangerousGetHandle();
						int num2;
						flag = InternalSymbolicLinkLinkCodeMethods.DeviceIoControl(hDevice, 589992U, IntPtr.Zero, 0, intPtr, num, out num2, IntPtr.Zero);
						if (!flag)
						{
							int lastWin32Error = Marshal.GetLastWin32Error();
							throw new Win32Exception(lastWin32Error);
						}
						InternalSymbolicLinkLinkCodeMethods.REPARSE_GUID_DATA_BUFFER structure = ClrFacade.PtrToStructure<InternalSymbolicLinkLinkCodeMethods.REPARSE_GUID_DATA_BUFFER>(intPtr);
						structure.ReparseDataLength = 0;
						ClrFacade.StructureToPtr<InternalSymbolicLinkLinkCodeMethods.REPARSE_GUID_DATA_BUFFER>(structure, intPtr2, false);
						flag = InternalSymbolicLinkLinkCodeMethods.DeviceIoControl(hDevice, 589996U, intPtr2, 24, IntPtr.Zero, 0, out num2, IntPtr.Zero);
						if (!flag)
						{
							int lastWin32Error2 = Marshal.GetLastWin32Error();
							throw new Win32Exception(lastWin32Error2);
						}
					}
					finally
					{
						if (flag2)
						{
							safeHandle.DangerousRelease();
						}
						Marshal.FreeHGlobal(intPtr);
						Marshal.FreeHGlobal(intPtr2);
					}
					return flag;
				}
			}
			throw new ArgumentNullException("junctionPath");
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x00194F08 File Offset: 0x00193108
		private static SafeFileHandle OpenReparsePoint(string reparsePoint, InternalSymbolicLinkLinkCodeMethods.FileDesiredAccess accessMode)
		{
			IntPtr preexistingHandle = InternalSymbolicLinkLinkCodeMethods.CreateFile(reparsePoint, accessMode, InternalSymbolicLinkLinkCodeMethods.FileShareMode.Read | InternalSymbolicLinkLinkCodeMethods.FileShareMode.Write | InternalSymbolicLinkLinkCodeMethods.FileShareMode.Delete, IntPtr.Zero, InternalSymbolicLinkLinkCodeMethods.FileCreationDisposition.OpenExisting, InternalSymbolicLinkLinkCodeMethods.FileAttributes.BackupSemantics | InternalSymbolicLinkLinkCodeMethods.FileAttributes.OpenReparsePoint, IntPtr.Zero);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error != 0)
			{
				throw new Win32Exception(lastWin32Error);
			}
			return new SafeFileHandle(preexistingHandle, true);
		}

		// Token: 0x040024E3 RID: 9443
		private const int REPARSE_GUID_DATA_BUFFER_HEADER_SIZE = 24;

		// Token: 0x040024E4 RID: 9444
		private const int MAX_REPARSE_SIZE = 16408;

		// Token: 0x040024E5 RID: 9445
		private const int ERROR_NOT_A_REPARSE_POINT = 4390;

		// Token: 0x040024E6 RID: 9446
		private const int FSCTL_GET_REPARSE_POINT = 589992;

		// Token: 0x040024E7 RID: 9447
		private const int FSCTL_SET_REPARSE_POINT = 589988;

		// Token: 0x040024E8 RID: 9448
		private const int FSCTL_DELETE_REPARSE_POINT = 589996;

		// Token: 0x040024E9 RID: 9449
		private const uint IO_REPARSE_TAG_SYMLINK = 2684354572U;

		// Token: 0x040024EA RID: 9450
		private const uint IO_REPARSE_TAG_MOUNT_POINT = 2684354563U;

		// Token: 0x040024EB RID: 9451
		private const string NonInterpretedPathPrefix = "\\??\\";

		// Token: 0x0200077D RID: 1917
		[Flags]
		internal enum FileDesiredAccess : uint
		{
			// Token: 0x040024ED RID: 9453
			GenericRead = 2147483648U,
			// Token: 0x040024EE RID: 9454
			GenericWrite = 1073741824U,
			// Token: 0x040024EF RID: 9455
			GenericExecute = 536870912U,
			// Token: 0x040024F0 RID: 9456
			GenericAll = 268435456U
		}

		// Token: 0x0200077E RID: 1918
		[Flags]
		internal enum FileShareMode : uint
		{
			// Token: 0x040024F2 RID: 9458
			None = 0U,
			// Token: 0x040024F3 RID: 9459
			Read = 1U,
			// Token: 0x040024F4 RID: 9460
			Write = 2U,
			// Token: 0x040024F5 RID: 9461
			Delete = 4U
		}

		// Token: 0x0200077F RID: 1919
		internal enum FileCreationDisposition : uint
		{
			// Token: 0x040024F7 RID: 9463
			New = 1U,
			// Token: 0x040024F8 RID: 9464
			CreateAlways,
			// Token: 0x040024F9 RID: 9465
			OpenExisting,
			// Token: 0x040024FA RID: 9466
			OpenAlways,
			// Token: 0x040024FB RID: 9467
			TruncateExisting
		}

		// Token: 0x02000780 RID: 1920
		[Flags]
		internal enum FileAttributes : uint
		{
			// Token: 0x040024FD RID: 9469
			Readonly = 1U,
			// Token: 0x040024FE RID: 9470
			Hidden = 2U,
			// Token: 0x040024FF RID: 9471
			System = 4U,
			// Token: 0x04002500 RID: 9472
			Archive = 32U,
			// Token: 0x04002501 RID: 9473
			Encrypted = 16384U,
			// Token: 0x04002502 RID: 9474
			Write_Through = 2147483648U,
			// Token: 0x04002503 RID: 9475
			Overlapped = 1073741824U,
			// Token: 0x04002504 RID: 9476
			NoBuffering = 536870912U,
			// Token: 0x04002505 RID: 9477
			RandomAccess = 268435456U,
			// Token: 0x04002506 RID: 9478
			SequentialScan = 134217728U,
			// Token: 0x04002507 RID: 9479
			DeleteOnClose = 67108864U,
			// Token: 0x04002508 RID: 9480
			BackupSemantics = 33554432U,
			// Token: 0x04002509 RID: 9481
			PosixSemantics = 16777216U,
			// Token: 0x0400250A RID: 9482
			OpenReparsePoint = 2097152U,
			// Token: 0x0400250B RID: 9483
			OpenNoRecall = 1048576U,
			// Token: 0x0400250C RID: 9484
			SessionAware = 8388608U,
			// Token: 0x0400250D RID: 9485
			Normal = 128U
		}

		// Token: 0x02000781 RID: 1921
		private struct REPARSE_DATA_BUFFER_SYMBOLICLINK
		{
			// Token: 0x0400250E RID: 9486
			public uint ReparseTag;

			// Token: 0x0400250F RID: 9487
			public ushort ReparseDataLength;

			// Token: 0x04002510 RID: 9488
			public ushort Reserved;

			// Token: 0x04002511 RID: 9489
			public ushort SubstituteNameOffset;

			// Token: 0x04002512 RID: 9490
			public ushort SubstituteNameLength;

			// Token: 0x04002513 RID: 9491
			public ushort PrintNameOffset;

			// Token: 0x04002514 RID: 9492
			public ushort PrintNameLength;

			// Token: 0x04002515 RID: 9493
			public uint Flags;

			// Token: 0x04002516 RID: 9494
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16368)]
			public byte[] PathBuffer;
		}

		// Token: 0x02000782 RID: 1922
		private struct REPARSE_DATA_BUFFER_MOUNTPOINT
		{
			// Token: 0x04002517 RID: 9495
			public uint ReparseTag;

			// Token: 0x04002518 RID: 9496
			public ushort ReparseDataLength;

			// Token: 0x04002519 RID: 9497
			public ushort Reserved;

			// Token: 0x0400251A RID: 9498
			public ushort SubstituteNameOffset;

			// Token: 0x0400251B RID: 9499
			public ushort SubstituteNameLength;

			// Token: 0x0400251C RID: 9500
			public ushort PrintNameOffset;

			// Token: 0x0400251D RID: 9501
			public ushort PrintNameLength;

			// Token: 0x0400251E RID: 9502
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16368)]
			public byte[] PathBuffer;
		}

		// Token: 0x02000783 RID: 1923
		private struct BY_HANDLE_FILE_INFORMATION
		{
			// Token: 0x0400251F RID: 9503
			public uint FileAttributes;

			// Token: 0x04002520 RID: 9504
			public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;

			// Token: 0x04002521 RID: 9505
			public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

			// Token: 0x04002522 RID: 9506
			public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;

			// Token: 0x04002523 RID: 9507
			public uint VolumeSerialNumber;

			// Token: 0x04002524 RID: 9508
			public uint FileSizeHigh;

			// Token: 0x04002525 RID: 9509
			public uint FileSizeLow;

			// Token: 0x04002526 RID: 9510
			public uint NumberOfLinks;

			// Token: 0x04002527 RID: 9511
			public uint FileIndexHigh;

			// Token: 0x04002528 RID: 9512
			public uint FileIndexLow;
		}

		// Token: 0x02000784 RID: 1924
		private struct GUID
		{
			// Token: 0x04002529 RID: 9513
			public uint Data1;

			// Token: 0x0400252A RID: 9514
			public ushort Data2;

			// Token: 0x0400252B RID: 9515
			public ushort Data3;

			// Token: 0x0400252C RID: 9516
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] Data4;
		}

		// Token: 0x02000785 RID: 1925
		private struct REPARSE_GUID_DATA_BUFFER
		{
			// Token: 0x0400252D RID: 9517
			public uint ReparseTag;

			// Token: 0x0400252E RID: 9518
			public ushort ReparseDataLength;

			// Token: 0x0400252F RID: 9519
			public ushort Reserved;

			// Token: 0x04002530 RID: 9520
			public InternalSymbolicLinkLinkCodeMethods.GUID ReparseGuid;

			// Token: 0x04002531 RID: 9521
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16408)]
			public char[] DataBuffer;
		}
	}
}
