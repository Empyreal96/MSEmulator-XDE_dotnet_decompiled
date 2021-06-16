using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x0200047B RID: 1147
	internal sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600332F RID: 13103 RVA: 0x001180D9 File Offset: 0x001162D9
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeRegistryHandle() : base(true)
		{
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x001180E2 File Offset: 0x001162E2
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x06003331 RID: 13105
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll")]
		internal static extern int RegCloseKey(IntPtr hKey);

		// Token: 0x06003332 RID: 13106 RVA: 0x001180F4 File Offset: 0x001162F4
		protected override bool ReleaseHandle()
		{
			int num = SafeRegistryHandle.RegCloseKey(this.handle);
			return num == 0;
		}
	}
}
