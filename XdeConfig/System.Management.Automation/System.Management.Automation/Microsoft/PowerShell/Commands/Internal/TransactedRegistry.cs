using System;
using System.Runtime.InteropServices;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007AA RID: 1962
	[ComVisible(true)]
	internal static class TransactedRegistry
	{
		// Token: 0x04002653 RID: 9811
		private const string resBaseName = "RegistryProviderStrings";

		// Token: 0x04002654 RID: 9812
		internal static readonly TransactedRegistryKey CurrentUser = TransactedRegistryKey.GetBaseKey(BaseRegistryKeys.HKEY_CURRENT_USER);

		// Token: 0x04002655 RID: 9813
		internal static readonly TransactedRegistryKey LocalMachine = TransactedRegistryKey.GetBaseKey(BaseRegistryKeys.HKEY_LOCAL_MACHINE);

		// Token: 0x04002656 RID: 9814
		internal static readonly TransactedRegistryKey ClassesRoot = TransactedRegistryKey.GetBaseKey(BaseRegistryKeys.HKEY_CLASSES_ROOT);

		// Token: 0x04002657 RID: 9815
		internal static readonly TransactedRegistryKey Users = TransactedRegistryKey.GetBaseKey(BaseRegistryKeys.HKEY_USERS);

		// Token: 0x04002658 RID: 9816
		internal static readonly TransactedRegistryKey CurrentConfig = TransactedRegistryKey.GetBaseKey(BaseRegistryKeys.HKEY_CURRENT_CONFIG);
	}
}
