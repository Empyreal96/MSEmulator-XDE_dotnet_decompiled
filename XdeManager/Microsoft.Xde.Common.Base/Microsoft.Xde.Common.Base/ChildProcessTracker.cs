using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000003 RID: 3
	public static class ChildProcessTracker
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002354 File Offset: 0x00000554
		static ChildProcessTracker()
		{
			string name = "Microsoft.Xde.Common.ChildProcessTracker" + Process.GetCurrentProcess().Id;
			ChildProcessTracker.JobHandle = ChildProcessTracker.CreateJobObject(IntPtr.Zero, name);
			if (ChildProcessTracker.JobHandle == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			ChildProcessTracker.JOBOBJECT_BASIC_LIMIT_INFORMATION basicLimitInformation = default(ChildProcessTracker.JOBOBJECT_BASIC_LIMIT_INFORMATION);
			basicLimitInformation.LimitFlags = ChildProcessTracker.JOBOBJECTLIMIT.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE;
			ChildProcessTracker.JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure = default(ChildProcessTracker.JOBOBJECT_EXTENDED_LIMIT_INFORMATION);
			structure.BasicLimitInformation = basicLimitInformation;
			int num = Marshal.SizeOf(typeof(ChildProcessTracker.JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.StructureToPtr<ChildProcessTracker.JOBOBJECT_EXTENDED_LIMIT_INFORMATION>(structure, intPtr, false);
				if (!ChildProcessTracker.SetInformationJobObject(ChildProcessTracker.JobHandle, ChildProcessTracker.JobObjectInfoType.ExtendedLimitInformation, intPtr, (uint)num))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002424 File Offset: 0x00000624
		public static void AddProcess(Process process)
		{
			if (ChildProcessTracker.JobHandle != IntPtr.Zero && !ChildProcessTracker.AssignProcessToJobObject(ChildProcessTracker.JobHandle, process.Handle) && !process.HasExited)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x0600000E RID: 14
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string name);

		// Token: 0x0600000F RID: 15
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetInformationJobObject(IntPtr job, ChildProcessTracker.JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

		// Token: 0x06000010 RID: 16
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

		// Token: 0x0400000B RID: 11
		private static readonly IntPtr JobHandle;

		// Token: 0x0200002E RID: 46
		private enum JobObjectInfoType
		{
			// Token: 0x0400010E RID: 270
			AssociateCompletionPortInformation = 7,
			// Token: 0x0400010F RID: 271
			BasicLimitInformation = 2,
			// Token: 0x04000110 RID: 272
			BasicUIRestrictions = 4,
			// Token: 0x04000111 RID: 273
			EndOfJobTimeInformation = 6,
			// Token: 0x04000112 RID: 274
			ExtendedLimitInformation = 9,
			// Token: 0x04000113 RID: 275
			SecurityLimitInformation = 5,
			// Token: 0x04000114 RID: 276
			GroupInformation = 11
		}

		// Token: 0x0200002F RID: 47
		private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
		{
			// Token: 0x04000115 RID: 277
			public long PerProcessUserTimeLimit;

			// Token: 0x04000116 RID: 278
			public long PerJobUserTimeLimit;

			// Token: 0x04000117 RID: 279
			public ChildProcessTracker.JOBOBJECTLIMIT LimitFlags;

			// Token: 0x04000118 RID: 280
			public UIntPtr MinimumWorkingSetSize;

			// Token: 0x04000119 RID: 281
			public UIntPtr MaximumWorkingSetSize;

			// Token: 0x0400011A RID: 282
			public uint ActiveProcessLimit;

			// Token: 0x0400011B RID: 283
			public long Affinity;

			// Token: 0x0400011C RID: 284
			public uint PriorityClass;

			// Token: 0x0400011D RID: 285
			public uint SchedulingClass;
		}

		// Token: 0x02000030 RID: 48
		[Flags]
		private enum JOBOBJECTLIMIT : uint
		{
			// Token: 0x0400011F RID: 287
			JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 8192U
		}

		// Token: 0x02000031 RID: 49
		private struct IO_COUNTERS
		{
			// Token: 0x04000120 RID: 288
			public ulong ReadOperationCount;

			// Token: 0x04000121 RID: 289
			public ulong WriteOperationCount;

			// Token: 0x04000122 RID: 290
			public ulong OtherOperationCount;

			// Token: 0x04000123 RID: 291
			public ulong ReadTransferCount;

			// Token: 0x04000124 RID: 292
			public ulong WriteTransferCount;

			// Token: 0x04000125 RID: 293
			public ulong OtherTransferCount;
		}

		// Token: 0x02000032 RID: 50
		private struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
		{
			// Token: 0x04000126 RID: 294
			public ChildProcessTracker.JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;

			// Token: 0x04000127 RID: 295
			public ChildProcessTracker.IO_COUNTERS IoInfo;

			// Token: 0x04000128 RID: 296
			public UIntPtr ProcessMemoryLimit;

			// Token: 0x04000129 RID: 297
			public UIntPtr JobMemoryLimit;

			// Token: 0x0400012A RID: 298
			public UIntPtr PeakProcessMemoryUsed;

			// Token: 0x0400012B RID: 299
			public UIntPtr PeakJobMemoryUsed;
		}
	}
}
