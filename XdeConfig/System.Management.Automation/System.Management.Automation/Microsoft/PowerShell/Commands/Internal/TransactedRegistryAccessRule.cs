using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007AF RID: 1967
	public sealed class TransactedRegistryAccessRule : AccessRule
	{
		// Token: 0x06004D42 RID: 19778 RVA: 0x00197250 File Offset: 0x00195450
		internal TransactedRegistryAccessRule(IdentityReference identity, RegistryRights registryRights, AccessControlType type) : this(identity, (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0019725E File Offset: 0x0019545E
		internal TransactedRegistryAccessRule(string identity, RegistryRights registryRights, AccessControlType type) : this(new NTAccount(identity), (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x00197271 File Offset: 0x00195471
		public TransactedRegistryAccessRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x00197281 File Offset: 0x00195481
		internal TransactedRegistryAccessRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x00197296 File Offset: 0x00195496
		internal TransactedRegistryAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06004D47 RID: 19783 RVA: 0x001972A7 File Offset: 0x001954A7
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
