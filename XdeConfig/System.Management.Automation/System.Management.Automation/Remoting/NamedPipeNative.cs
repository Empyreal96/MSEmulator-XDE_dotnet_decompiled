using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002EA RID: 746
	internal static class NamedPipeNative
	{
		// Token: 0x0600238A RID: 9098
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafePipeHandle CreateNamedPipe(string lpName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, NamedPipeNative.SECURITY_ATTRIBUTES securityAttributes);

		// Token: 0x0600238B RID: 9099 RVA: 0x000C7980 File Offset: 0x000C5B80
		internal static NamedPipeNative.SECURITY_ATTRIBUTES GetSecurityAttributes(GCHandle securityDescriptorPinnedHandle)
		{
			NamedPipeNative.SECURITY_ATTRIBUTES security_ATTRIBUTES = new NamedPipeNative.SECURITY_ATTRIBUTES();
			security_ATTRIBUTES.InheritHandle = false;
			security_ATTRIBUTES.NLength = Marshal.SizeOf(security_ATTRIBUTES);
			security_ATTRIBUTES.LPSecurityDescriptor = securityDescriptorPinnedHandle.AddrOfPinnedObject();
			return security_ATTRIBUTES;
		}

		// Token: 0x0600238C RID: 9100
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafePipeHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x0600238D RID: 9101
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WaitNamedPipe(string lpNamedPipeName, uint nTimeOut);

		// Token: 0x0600238E RID: 9102
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ImpersonateNamedPipeClient(IntPtr hNamedPipe);

		// Token: 0x0600238F RID: 9103
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool RevertToSelf();

		// Token: 0x0400114C RID: 4428
		internal const uint PIPE_ACCESS_DUPLEX = 3U;

		// Token: 0x0400114D RID: 4429
		internal const uint PIPE_ACCESS_OUTBOUND = 2U;

		// Token: 0x0400114E RID: 4430
		internal const uint PIPE_ACCESS_INBOUND = 1U;

		// Token: 0x0400114F RID: 4431
		internal const uint PIPE_TYPE_BYTE = 0U;

		// Token: 0x04001150 RID: 4432
		internal const uint PIPE_TYPE_MESSAGE = 4U;

		// Token: 0x04001151 RID: 4433
		internal const uint FILE_FLAG_OVERLAPPED = 1073741824U;

		// Token: 0x04001152 RID: 4434
		internal const uint FILE_FLAG_FIRST_PIPE_INSTANCE = 524288U;

		// Token: 0x04001153 RID: 4435
		internal const uint PIPE_WAIT = 0U;

		// Token: 0x04001154 RID: 4436
		internal const uint PIPE_NOWAIT = 1U;

		// Token: 0x04001155 RID: 4437
		internal const uint PIPE_READMODE_BYTE = 0U;

		// Token: 0x04001156 RID: 4438
		internal const uint PIPE_READMODE_MESSAGE = 2U;

		// Token: 0x04001157 RID: 4439
		internal const uint PIPE_ACCEPT_REMOTE_CLIENTS = 0U;

		// Token: 0x04001158 RID: 4440
		internal const uint PIPE_REJECT_REMOTE_CLIENTS = 8U;

		// Token: 0x04001159 RID: 4441
		internal const uint ERROR_FILE_NOT_FOUND = 2U;

		// Token: 0x0400115A RID: 4442
		internal const uint ERROR_BROKEN_PIPE = 109U;

		// Token: 0x0400115B RID: 4443
		internal const uint ERROR_PIPE_BUSY = 231U;

		// Token: 0x0400115C RID: 4444
		internal const uint ERROR_NO_DATA = 232U;

		// Token: 0x0400115D RID: 4445
		internal const uint ERROR_MORE_DATA = 234U;

		// Token: 0x0400115E RID: 4446
		internal const uint ERROR_PIPE_CONNECTED = 535U;

		// Token: 0x0400115F RID: 4447
		internal const uint ERROR_IO_INCOMPLETE = 996U;

		// Token: 0x04001160 RID: 4448
		internal const uint ERROR_IO_PENDING = 997U;

		// Token: 0x04001161 RID: 4449
		internal const uint GENERIC_READ = 2147483648U;

		// Token: 0x04001162 RID: 4450
		internal const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x04001163 RID: 4451
		internal const uint GENERIC_EXECUTE = 536870912U;

		// Token: 0x04001164 RID: 4452
		internal const uint GENERIC_ALL = 268435456U;

		// Token: 0x04001165 RID: 4453
		internal const uint CREATE_NEW = 1U;

		// Token: 0x04001166 RID: 4454
		internal const uint CREATE_ALWAYS = 2U;

		// Token: 0x04001167 RID: 4455
		internal const uint OPEN_EXISTING = 3U;

		// Token: 0x04001168 RID: 4456
		internal const uint OPEN_ALWAYS = 4U;

		// Token: 0x04001169 RID: 4457
		internal const uint TRUNCATE_EXISTING = 5U;

		// Token: 0x0400116A RID: 4458
		internal const uint SECURITY_IMPERSONATIONLEVEL_ANONYMOUS = 0U;

		// Token: 0x0400116B RID: 4459
		internal const uint SECURITY_IMPERSONATIONLEVEL_IDENTIFCATION = 1U;

		// Token: 0x0400116C RID: 4460
		internal const uint SECURITY_IMPERSONATIONLEVEL_IMPERSONATION = 2U;

		// Token: 0x0400116D RID: 4461
		internal const uint SECURITY_IMPERSONATIONLEVEL_DELEGATION = 3U;

		// Token: 0x0400116E RID: 4462
		internal const uint INFINITE = 4294967295U;

		// Token: 0x020002EB RID: 747
		[StructLayout(LayoutKind.Sequential)]
		internal class SECURITY_ATTRIBUTES
		{
			// Token: 0x06002390 RID: 9104 RVA: 0x000C79B4 File Offset: 0x000C5BB4
			public SECURITY_ATTRIBUTES()
			{
				this.NLength = 12;
			}

			// Token: 0x0400116F RID: 4463
			public int NLength;

			// Token: 0x04001170 RID: 4464
			public IntPtr LPSecurityDescriptor = IntPtr.Zero;

			// Token: 0x04001171 RID: 4465
			public bool InheritHandle;
		}
	}
}
