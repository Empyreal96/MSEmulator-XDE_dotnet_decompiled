using System;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007AB RID: 1963
	internal sealed class BaseRegistryKeys
	{
		// Token: 0x04002659 RID: 9817
		internal static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(int.MinValue);

		// Token: 0x0400265A RID: 9818
		internal static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);

		// Token: 0x0400265B RID: 9819
		internal static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);

		// Token: 0x0400265C RID: 9820
		internal static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);

		// Token: 0x0400265D RID: 9821
		internal static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);
	}
}
