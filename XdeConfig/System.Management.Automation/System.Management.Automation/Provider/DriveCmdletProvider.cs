using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000463 RID: 1123
	public abstract class DriveCmdletProvider : CmdletProvider
	{
		// Token: 0x060031BB RID: 12731 RVA: 0x0010E57C File Offset: 0x0010C77C
		internal PSDriveInfo NewDrive(PSDriveInfo drive, CmdletProviderContext context)
		{
			base.Context = context;
			if (drive.Credential != null && drive.Credential != PSCredential.Empty && !CmdletProviderManagementIntrinsics.CheckProviderCapabilities(ProviderCapabilities.Credentials, base.ProviderInfo))
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.NewDriveCredentials_NotSupported, new object[0]);
			}
			return this.NewDrive(drive);
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x0010E5CC File Offset: 0x0010C7CC
		internal object NewDriveDynamicParameters(CmdletProviderContext context)
		{
			base.Context = context;
			return this.NewDriveDynamicParameters();
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x0010E5DB File Offset: 0x0010C7DB
		internal PSDriveInfo RemoveDrive(PSDriveInfo drive, CmdletProviderContext context)
		{
			base.Context = context;
			return this.RemoveDrive(drive);
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x0010E5EB File Offset: 0x0010C7EB
		internal Collection<PSDriveInfo> InitializeDefaultDrives(CmdletProviderContext context)
		{
			base.Context = context;
			base.Context.Drive = null;
			return this.InitializeDefaultDrives();
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x0010E608 File Offset: 0x0010C808
		protected virtual PSDriveInfo NewDrive(PSDriveInfo drive)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
			return drive;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x0010E63C File Offset: 0x0010C83C
		protected virtual object NewDriveDynamicParameters()
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x0010E670 File Offset: 0x0010C870
		protected virtual PSDriveInfo RemoveDrive(PSDriveInfo drive)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
			return drive;
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x0010E6A4 File Offset: 0x0010C8A4
		protected virtual Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			Collection<PSDriveInfo> result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = new Collection<PSDriveInfo>();
			}
			return result;
		}
	}
}
