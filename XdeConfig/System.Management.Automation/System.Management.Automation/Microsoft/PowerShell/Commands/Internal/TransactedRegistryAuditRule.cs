using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007B0 RID: 1968
	public sealed class TransactedRegistryAuditRule : AuditRule
	{
		// Token: 0x06004D48 RID: 19784 RVA: 0x001972AF File Offset: 0x001954AF
		internal TransactedRegistryAuditRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x001972BF File Offset: 0x001954BF
		internal TransactedRegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x001972D4 File Offset: 0x001954D4
		internal TransactedRegistryAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06004D4B RID: 19787 RVA: 0x001972E5 File Offset: 0x001954E5
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
