using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000198 RID: 408
	internal static class CabinetNativeApi
	{
		// Token: 0x06001399 RID: 5017 RVA: 0x000792E4 File Offset: 0x000774E4
		internal static IntPtr FdiAlloc(int size)
		{
			IntPtr result;
			try
			{
				result = Marshal.AllocHGlobal(size);
			}
			catch (OutOfMemoryException)
			{
				result = IntPtr.Zero;
			}
			return result;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00079314 File Offset: 0x00077514
		internal static void FdiFree(IntPtr memblock)
		{
			Marshal.FreeHGlobal(memblock);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0007931C File Offset: 0x0007751C
		internal static IntPtr FdiOpen(string filename, int oflag, int pmode)
		{
			FileMode mode = CabinetNativeApi.ConvertOpflagToFileMode(oflag);
			FileAccess access = CabinetNativeApi.ConvertPermissionModeToFileAccess(pmode);
			FileShare share = CabinetNativeApi.ConvertPermissionModeToFileShare(pmode);
			IntPtr result;
			try
			{
				FileStream fileStream = new FileStream(filename, mode, access, share);
				if (fileStream == null)
				{
					result = new IntPtr(-1);
				}
				else
				{
					result = GCHandle.ToIntPtr(GCHandle.Alloc(fileStream));
				}
			}
			catch (IOException)
			{
				result = new IntPtr(-1);
			}
			return result;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00079384 File Offset: 0x00077584
		internal static int FdiRead(IntPtr fp, byte[] buffer, int count)
		{
			FileStream fileStream = (FileStream)GCHandle.FromIntPtr(fp).Target;
			int result = 0;
			try
			{
				result = fileStream.Read(buffer, 0, count);
			}
			catch (ArgumentNullException)
			{
				result = -1;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = -1;
			}
			catch (NotSupportedException)
			{
				result = -1;
			}
			catch (IOException)
			{
				result = -1;
			}
			catch (ArgumentException)
			{
				result = -1;
			}
			catch (ObjectDisposedException)
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00079420 File Offset: 0x00077620
		internal static int FdiWrite(IntPtr fp, byte[] buffer, int count)
		{
			FileStream fileStream = (FileStream)GCHandle.FromIntPtr(fp).Target;
			int result = 0;
			try
			{
				fileStream.Write(buffer, 0, count);
				result = count;
			}
			catch (ArgumentNullException)
			{
				result = -1;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = -1;
			}
			catch (NotSupportedException)
			{
				result = -1;
			}
			catch (IOException)
			{
				result = -1;
			}
			catch (ArgumentException)
			{
				result = -1;
			}
			catch (ObjectDisposedException)
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x000794BC File Offset: 0x000776BC
		internal static int FdiClose(IntPtr fp)
		{
			FileStream fileStream = (FileStream)GCHandle.FromIntPtr(fp).Target;
			if (fileStream == null)
			{
				return -1;
			}
			fileStream.Dispose();
			return 0;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x000794EC File Offset: 0x000776EC
		internal static int FdiSeek(IntPtr fp, int offset, int origin)
		{
			FileStream fileStream = (FileStream)GCHandle.FromIntPtr(fp).Target;
			SeekOrigin origin2 = CabinetNativeApi.ConvertOriginToSeekOrigin(origin);
			long num = 0L;
			try
			{
				num = fileStream.Seek((long)offset, origin2);
			}
			catch (NotSupportedException)
			{
				num = -1L;
			}
			catch (IOException)
			{
				num = -1L;
			}
			catch (ArgumentException)
			{
				num = -1L;
			}
			catch (ObjectDisposedException)
			{
				num = -1L;
			}
			return (int)num;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00079574 File Offset: 0x00077774
		internal static IntPtr FdiNotify(CabinetNativeApi.FdiNotificationType fdint, CabinetNativeApi.FdiNotification fdin)
		{
			switch (fdint)
			{
			case CabinetNativeApi.FdiNotificationType.FdintCOPY_FILE:
			{
				string text = Marshal.PtrToStringAnsi(fdin.pv);
				string fileName = Path.GetFileName(fdin.psz1);
				string directoryName = Path.GetDirectoryName(fdin.psz1);
				text = Path.Combine(text, directoryName);
				Directory.CreateDirectory(text);
				string filename = Path.Combine(text, fileName);
				return CabinetNativeApi.FdiOpen(filename, 256, 384);
			}
			case CabinetNativeApi.FdiNotificationType.FdintCLOSE_FILE_INFO:
			{
				CabinetNativeApi.FdiClose(fdin.hf);
				string path = Marshal.PtrToStringAnsi(fdin.pv);
				string lpFileName = Path.Combine(path, fdin.psz1);
				IntPtr intPtr = PlatformInvokes.CreateFile(lpFileName, (PlatformInvokes.FileDesiredAccess)3221225472U, PlatformInvokes.FileShareMode.Read, IntPtr.Zero, PlatformInvokes.FileCreationDisposition.OpenExisting, PlatformInvokes.FileAttributes.Normal, IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					PlatformInvokes.FILETIME filetime = new PlatformInvokes.FILETIME();
					if (PlatformInvokes.DosDateTimeToFileTime(fdin.date, fdin.time, filetime))
					{
						PlatformInvokes.FILETIME filetime2 = new PlatformInvokes.FILETIME();
						if (PlatformInvokes.LocalFileTimeToFileTime(filetime, filetime2))
						{
							PlatformInvokes.SetFileTime(intPtr, filetime2, null, filetime2);
						}
					}
					PlatformInvokes.CloseHandle(intPtr);
				}
				PlatformInvokes.SetFileAttributesW(lpFileName, (PlatformInvokes.FileAttributes)(fdin.attribs & 39));
				return new IntPtr(1);
			}
			default:
				return new IntPtr(0);
			}
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0007969C File Offset: 0x0007789C
		internal static SeekOrigin ConvertOriginToSeekOrigin(int origin)
		{
			switch (origin)
			{
			case 0:
				return SeekOrigin.Begin;
			case 1:
				return SeekOrigin.Current;
			case 2:
				return SeekOrigin.End;
			default:
				return SeekOrigin.Current;
			}
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x000796C8 File Offset: 0x000778C8
		internal static FileMode ConvertOpflagToFileMode(int oflag)
		{
			if (1280 == (oflag & 1280))
			{
				return FileMode.CreateNew;
			}
			if (768 == (oflag & 768))
			{
				return FileMode.OpenOrCreate;
			}
			if ((oflag & 8) != 0)
			{
				return FileMode.Append;
			}
			if ((oflag & 256) != 0)
			{
				return FileMode.Create;
			}
			if ((oflag & 2) != 0)
			{
				return FileMode.Open;
			}
			if ((oflag & 512) != 0)
			{
				return FileMode.Truncate;
			}
			return FileMode.OpenOrCreate;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0007971A File Offset: 0x0007791A
		internal static FileAccess ConvertPermissionModeToFileAccess(int pmode)
		{
			if (384 == (pmode & 384))
			{
				return FileAccess.ReadWrite;
			}
			if ((pmode & 256) != 0)
			{
				return FileAccess.Read;
			}
			if ((pmode & 128) != 0)
			{
				return FileAccess.Write;
			}
			return FileAccess.Read;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00079743 File Offset: 0x00077943
		internal static FileShare ConvertPermissionModeToFileShare(int pmode)
		{
			if (384 == (pmode & 384))
			{
				return FileShare.ReadWrite;
			}
			if ((pmode & 256) != 0)
			{
				return FileShare.Read;
			}
			if ((pmode & 128) != 0)
			{
				return FileShare.Write;
			}
			return FileShare.Read;
		}

		// Token: 0x060013A5 RID: 5029
		[DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = true)]
		internal static extern CabinetNativeApi.FdiContextHandle FDICreate(IntPtr pfnalloc, IntPtr pfnfree, IntPtr pfnopen, IntPtr pfnread, IntPtr pfnwrite, IntPtr pfnclose, IntPtr pfnseek, CabinetNativeApi.FdiCreateCpuType cpuType, CabinetNativeApi.FdiERF erf);

		// Token: 0x060013A6 RID: 5030
		[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = true)]
		internal static extern bool FDICopy(CabinetNativeApi.FdiContextHandle hfdi, [MarshalAs(UnmanagedType.LPStr)] string pszCabinet, [MarshalAs(UnmanagedType.LPStr)] string pszCabPath, int flags, IntPtr pfnfdin, IntPtr pfnfdid, IntPtr pvUser);

		// Token: 0x060013A7 RID: 5031
		[DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = true)]
		internal static extern bool FDIDestroy(IntPtr hfdi);

		// Token: 0x02000199 RID: 409
		// (Invoke) Token: 0x060013A9 RID: 5033
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate IntPtr FdiAllocDelegate(int size);

		// Token: 0x0200019A RID: 410
		// (Invoke) Token: 0x060013AD RID: 5037
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate void FdiFreeDelegate(IntPtr memblock);

		// Token: 0x0200019B RID: 411
		// (Invoke) Token: 0x060013B1 RID: 5041
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate IntPtr FdiOpenDelegate([MarshalAs(UnmanagedType.LPStr)] string filename, int oflag, int pmode);

		// Token: 0x0200019C RID: 412
		// (Invoke) Token: 0x060013B5 RID: 5045
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate int FdiReadDelegate(IntPtr fp, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] [In] [Out] byte[] buffer, int count);

		// Token: 0x0200019D RID: 413
		// (Invoke) Token: 0x060013B9 RID: 5049
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate int FdiWriteDelegate(IntPtr fp, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] [In] byte[] buffer, int count);

		// Token: 0x0200019E RID: 414
		// (Invoke) Token: 0x060013BD RID: 5053
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate int FdiCloseDelegate(IntPtr fp);

		// Token: 0x0200019F RID: 415
		// (Invoke) Token: 0x060013C1 RID: 5057
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate int FdiSeekDelegate(IntPtr fp, int offset, int origin);

		// Token: 0x020001A0 RID: 416
		// (Invoke) Token: 0x060013C5 RID: 5061
		[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal delegate IntPtr FdiNotifyDelegate(CabinetNativeApi.FdiNotificationType fdint, CabinetNativeApi.FdiNotification fdin);

		// Token: 0x020001A1 RID: 417
		[Flags]
		internal enum PermissionMode
		{
			// Token: 0x04000869 RID: 2153
			None = 0,
			// Token: 0x0400086A RID: 2154
			Write = 128,
			// Token: 0x0400086B RID: 2155
			Read = 256
		}

		// Token: 0x020001A2 RID: 418
		[Flags]
		internal enum OpFlags
		{
			// Token: 0x0400086D RID: 2157
			RdOnly = 0,
			// Token: 0x0400086E RID: 2158
			WrOnly = 1,
			// Token: 0x0400086F RID: 2159
			RdWr = 2,
			// Token: 0x04000870 RID: 2160
			Append = 8,
			// Token: 0x04000871 RID: 2161
			Create = 256,
			// Token: 0x04000872 RID: 2162
			Truncate = 512,
			// Token: 0x04000873 RID: 2163
			Excl = 1024
		}

		// Token: 0x020001A3 RID: 419
		internal enum FdiCreateCpuType
		{
			// Token: 0x04000875 RID: 2165
			CpuUnknown = -1,
			// Token: 0x04000876 RID: 2166
			Cpu80286,
			// Token: 0x04000877 RID: 2167
			Cpu80386
		}

		// Token: 0x020001A4 RID: 420
		[StructLayout(LayoutKind.Sequential)]
		internal class FdiNotification
		{
			// Token: 0x04000878 RID: 2168
			internal int cb;

			// Token: 0x04000879 RID: 2169
			internal string psz1;

			// Token: 0x0400087A RID: 2170
			internal string psz2;

			// Token: 0x0400087B RID: 2171
			internal string psz3;

			// Token: 0x0400087C RID: 2172
			internal IntPtr pv;

			// Token: 0x0400087D RID: 2173
			internal IntPtr hf;

			// Token: 0x0400087E RID: 2174
			internal short date;

			// Token: 0x0400087F RID: 2175
			internal short time;

			// Token: 0x04000880 RID: 2176
			internal short attribs;

			// Token: 0x04000881 RID: 2177
			internal short setID;

			// Token: 0x04000882 RID: 2178
			internal short iCabinet;

			// Token: 0x04000883 RID: 2179
			internal short iFolder;

			// Token: 0x04000884 RID: 2180
			internal int fdie;
		}

		// Token: 0x020001A5 RID: 421
		internal enum FdiNotificationType
		{
			// Token: 0x04000886 RID: 2182
			FdintCABINET_INFO,
			// Token: 0x04000887 RID: 2183
			FdintPARTIAL_FILE,
			// Token: 0x04000888 RID: 2184
			FdintCOPY_FILE,
			// Token: 0x04000889 RID: 2185
			FdintCLOSE_FILE_INFO,
			// Token: 0x0400088A RID: 2186
			FdintNEXT_CABINET,
			// Token: 0x0400088B RID: 2187
			FdintENUMERATE
		}

		// Token: 0x020001A6 RID: 422
		[StructLayout(LayoutKind.Sequential)]
		internal class FdiERF
		{
			// Token: 0x0400088C RID: 2188
			internal int erfOper;

			// Token: 0x0400088D RID: 2189
			internal int erfType;

			// Token: 0x0400088E RID: 2190
			internal bool fError;
		}

		// Token: 0x020001A7 RID: 423
		internal sealed class FdiContextHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x060013CA RID: 5066 RVA: 0x0007977C File Offset: 0x0007797C
			private FdiContextHandle() : base(true)
			{
			}

			// Token: 0x060013CB RID: 5067 RVA: 0x00079785 File Offset: 0x00077985
			protected override bool ReleaseHandle()
			{
				return CabinetNativeApi.FDIDestroy(this.handle);
			}
		}
	}
}
