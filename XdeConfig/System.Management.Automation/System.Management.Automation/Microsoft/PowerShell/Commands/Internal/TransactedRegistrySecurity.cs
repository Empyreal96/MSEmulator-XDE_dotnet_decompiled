using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007B1 RID: 1969
	public sealed class TransactedRegistrySecurity : NativeObjectSecurity
	{
		// Token: 0x06004D4C RID: 19788 RVA: 0x001972ED File Offset: 0x001954ED
		public TransactedRegistrySecurity() : base(true, ResourceType.RegistryKey)
		{
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x001972F7 File Offset: 0x001954F7
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal TransactedRegistrySecurity(SafeRegistryHandle hKey, string name, AccessControlSections includeSections) : base(true, ResourceType.RegistryKey, hKey, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(TransactedRegistrySecurity._HandleErrorCode), null)
		{
			new RegistryPermission(RegistryPermissionAccess.NoAccess, AccessControlActions.View, name).Demand();
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x00197320 File Offset: 0x00195520
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception result = null;
			if (errorCode != 2)
			{
				if (errorCode != 6)
				{
					if (errorCode == 123)
					{
						result = new ArgumentException(RegistryProviderStrings.Arg_RegInvalidKeyName);
					}
				}
				else
				{
					result = new ArgumentException(RegistryProviderStrings.AccessControl_InvalidHandle);
				}
			}
			else
			{
				result = new IOException(RegistryProviderStrings.Arg_RegKeyNotFound);
			}
			return result;
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x00197366 File Offset: 0x00195566
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new TransactedRegistryAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x00197376 File Offset: 0x00195576
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new TransactedRegistryAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x00197388 File Offset: 0x00195588
		internal AccessControlSections GetAccessControlSectionsFromChanges()
		{
			AccessControlSections accessControlSections = AccessControlSections.None;
			if (base.AccessRulesModified)
			{
				accessControlSections = AccessControlSections.Access;
			}
			if (base.AuditRulesModified)
			{
				accessControlSections |= AccessControlSections.Audit;
			}
			if (base.OwnerModified)
			{
				accessControlSections |= AccessControlSections.Owner;
			}
			if (base.GroupModified)
			{
				accessControlSections |= AccessControlSections.Group;
			}
			return accessControlSections;
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x001973C8 File Offset: 0x001955C8
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal void Persist(SafeRegistryHandle hKey, string keyName)
		{
			new RegistryPermission(RegistryPermissionAccess.NoAccess, AccessControlActions.Change, keyName).Demand();
			base.WriteLock();
			try
			{
				AccessControlSections accessControlSectionsFromChanges = this.GetAccessControlSectionsFromChanges();
				if (accessControlSectionsFromChanges != AccessControlSections.None)
				{
					base.Persist(hKey, accessControlSectionsFromChanges);
					base.OwnerModified = (base.GroupModified = (base.AuditRulesModified = (base.AccessRulesModified = false)));
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x00197438 File Offset: 0x00195638
		public void AddAccessRule(TransactedRegistryAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x00197441 File Offset: 0x00195641
		public void SetAccessRule(TransactedRegistryAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0019744A File Offset: 0x0019564A
		public void ResetAccessRule(TransactedRegistryAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x00197453 File Offset: 0x00195653
		public bool RemoveAccessRule(TransactedRegistryAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0019745C File Offset: 0x0019565C
		public void RemoveAccessRuleAll(TransactedRegistryAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x00197465 File Offset: 0x00195665
		public void RemoveAccessRuleSpecific(TransactedRegistryAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0019746E File Offset: 0x0019566E
		public void AddAuditRule(TransactedRegistryAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x00197477 File Offset: 0x00195677
		public void SetAuditRule(TransactedRegistryAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x00197480 File Offset: 0x00195680
		public bool RemoveAuditRule(TransactedRegistryAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x00197489 File Offset: 0x00195689
		public void RemoveAuditRuleAll(TransactedRegistryAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x00197492 File Offset: 0x00195692
		public void RemoveAuditRuleSpecific(TransactedRegistryAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06004D5E RID: 19806 RVA: 0x0019749B File Offset: 0x0019569B
		public override Type AccessRightType
		{
			get
			{
				return typeof(RegistryRights);
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06004D5F RID: 19807 RVA: 0x001974A7 File Offset: 0x001956A7
		public override Type AccessRuleType
		{
			get
			{
				return typeof(TransactedRegistryAccessRule);
			}
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06004D60 RID: 19808 RVA: 0x001974B3 File Offset: 0x001956B3
		public override Type AuditRuleType
		{
			get
			{
				return typeof(TransactedRegistryAuditRule);
			}
		}

		// Token: 0x04002670 RID: 9840
		private const string resBaseName = "RegistryProviderStrings";
	}
}
