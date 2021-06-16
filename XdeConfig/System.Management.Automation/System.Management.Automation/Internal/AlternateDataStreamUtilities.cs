using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000786 RID: 1926
	public static class AlternateDataStreamUtilities
	{
		// Token: 0x06004C92 RID: 19602 RVA: 0x00194F48 File Offset: 0x00193148
		internal static List<AlternateStreamData> GetStreams(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			List<AlternateStreamData> list = new List<AlternateStreamData>();
			AlternateDataStreamUtilities.AlternateStreamNativeData alternateStreamNativeData = new AlternateDataStreamUtilities.AlternateStreamNativeData();
			AlternateDataStreamUtilities.SafeFindHandle safeFindHandle = AlternateDataStreamUtilities.NativeMethods.FindFirstStreamW(path, AlternateDataStreamUtilities.NativeMethods.StreamInfoLevels.FindStreamInfoStandard, alternateStreamNativeData, 0U);
			if (safeFindHandle.IsInvalid)
			{
				throw new Win32Exception();
			}
			goto IL_32;
			try
			{
				do
				{
					IL_32:
					alternateStreamNativeData.Name = alternateStreamNativeData.Name.Substring(1);
					string text = ":$DATA";
					if (!string.Equals(alternateStreamNativeData.Name, text, StringComparison.OrdinalIgnoreCase))
					{
						alternateStreamNativeData.Name = alternateStreamNativeData.Name.Replace(text, "");
					}
					AlternateStreamData alternateStreamData = new AlternateStreamData();
					alternateStreamData.Stream = alternateStreamNativeData.Name;
					alternateStreamData.Length = alternateStreamNativeData.Length;
					alternateStreamData.FileName = path.Replace(alternateStreamData.Stream, "");
					alternateStreamData.FileName = alternateStreamData.FileName.Trim(new char[]
					{
						':'
					});
					list.Add(alternateStreamData);
					alternateStreamNativeData = new AlternateDataStreamUtilities.AlternateStreamNativeData();
				}
				while (AlternateDataStreamUtilities.NativeMethods.FindNextStreamW(safeFindHandle, alternateStreamNativeData));
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != 38)
				{
					throw new Win32Exception(lastWin32Error);
				}
			}
			finally
			{
				safeFindHandle.Dispose();
			}
			return list;
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x0019506C File Offset: 0x0019326C
		internal static FileStream CreateFileStream(string path, string streamName, FileMode mode, FileAccess access, FileShare share)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (streamName == null)
			{
				throw new ArgumentNullException("streamName");
			}
			string str = streamName.Trim();
			str = ":" + str;
			string text = path + str;
			if (mode == FileMode.Append)
			{
				mode = FileMode.OpenOrCreate;
			}
			SafeFileHandle safeFileHandle = AlternateDataStreamUtilities.NativeMethods.CreateFile(text, access, share, IntPtr.Zero, mode, 0, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				string message = StringUtil.Format(FileSystemProviderStrings.AlternateDataStreamNotFound, streamName, path);
				throw new FileNotFoundException(message, text);
			}
			return new FileStream(safeFileHandle, access);
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x001950F4 File Offset: 0x001932F4
		internal static void DeleteFileStream(string path, string streamName)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (streamName == null)
			{
				throw new ArgumentNullException("streamName");
			}
			string text = streamName.Trim();
			if (text.IndexOf(':') != 0)
			{
				text = ":" + text;
			}
			string lpFileName = path + text;
			AlternateDataStreamUtilities.NativeMethods.DeleteFile(lpFileName);
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x0019514C File Offset: 0x0019334C
		internal static void SetZoneOfOrigin(string path, SecurityZone securityZone)
		{
			using (FileStream fileStream = AlternateDataStreamUtilities.CreateFileStream(path, "Zone.Identifier", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.Unicode))
				{
					textWriter.WriteLine("[ZoneTransfer]");
					textWriter.WriteLine("ZoneId={0}", (int)securityZone);
				}
			}
		}

		// Token: 0x02000787 RID: 1927
		private static class NativeMethods
		{
			// Token: 0x06004C96 RID: 19606
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

			// Token: 0x06004C97 RID: 19607
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool DeleteFile(string lpFileName);

			// Token: 0x06004C98 RID: 19608
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern AlternateDataStreamUtilities.SafeFindHandle FindFirstStreamW(string lpFileName, AlternateDataStreamUtilities.NativeMethods.StreamInfoLevels InfoLevel, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] AlternateDataStreamUtilities.AlternateStreamNativeData lpFindStreamData, uint dwFlags);

			// Token: 0x06004C99 RID: 19609
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool FindNextStreamW(AlternateDataStreamUtilities.SafeFindHandle hndFindFile, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] AlternateDataStreamUtilities.AlternateStreamNativeData lpFindStreamData);

			// Token: 0x04002532 RID: 9522
			internal const int ERROR_HANDLE_EOF = 38;

			// Token: 0x02000788 RID: 1928
			internal enum StreamInfoLevels
			{
				// Token: 0x04002534 RID: 9524
				FindStreamInfoStandard
			}
		}

		// Token: 0x02000789 RID: 1929
		internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x06004C9A RID: 19610 RVA: 0x001951C4 File Offset: 0x001933C4
			private SafeFindHandle() : base(true)
			{
			}

			// Token: 0x06004C9B RID: 19611 RVA: 0x001951CD File Offset: 0x001933CD
			protected override bool ReleaseHandle()
			{
				return AlternateDataStreamUtilities.SafeFindHandle.FindClose(this.handle);
			}

			// Token: 0x06004C9C RID: 19612
			[DllImport("kernel32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool FindClose(IntPtr handle);
		}

		// Token: 0x0200078A RID: 1930
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class AlternateStreamNativeData
		{
			// Token: 0x04002535 RID: 9525
			public long Length;

			// Token: 0x04002536 RID: 9526
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 296)]
			public string Name;
		}
	}
}
