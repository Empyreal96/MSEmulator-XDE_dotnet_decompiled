using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200001A RID: 26
	public sealed class DriveManagementIntrinsics
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00005FD0 File Offset: 0x000041D0
		private DriveManagementIntrinsics()
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005FD8 File Offset: 0x000041D8
		internal DriveManagementIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005FF5 File Offset: 0x000041F5
		public PSDriveInfo Current
		{
			get
			{
				return this.sessionState.CurrentDrive;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006002 File Offset: 0x00004202
		public PSDriveInfo New(PSDriveInfo drive, string scope)
		{
			return this.sessionState.NewDrive(drive, scope);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006011 File Offset: 0x00004211
		internal void New(PSDriveInfo drive, string scope, CmdletProviderContext context)
		{
			this.sessionState.NewDrive(drive, scope, context);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006021 File Offset: 0x00004221
		internal object NewDriveDynamicParameters(string providerId, CmdletProviderContext context)
		{
			return this.sessionState.NewDriveDynamicParameters(providerId, context);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006030 File Offset: 0x00004230
		public void Remove(string driveName, bool force, string scope)
		{
			this.sessionState.RemoveDrive(driveName, force, scope);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006040 File Offset: 0x00004240
		internal void Remove(string driveName, bool force, string scope, CmdletProviderContext context)
		{
			this.sessionState.RemoveDrive(driveName, force, scope, context);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006052 File Offset: 0x00004252
		public PSDriveInfo Get(string driveName)
		{
			return this.sessionState.GetDrive(driveName);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006060 File Offset: 0x00004260
		public PSDriveInfo GetAtScope(string driveName, string scope)
		{
			return this.sessionState.GetDrive(driveName, scope);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000606F File Offset: 0x0000426F
		public Collection<PSDriveInfo> GetAll()
		{
			return this.sessionState.Drives(null);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000607D File Offset: 0x0000427D
		public Collection<PSDriveInfo> GetAllAtScope(string scope)
		{
			return this.sessionState.Drives(scope);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000608B File Offset: 0x0000428B
		public Collection<PSDriveInfo> GetAllForProvider(string providerName)
		{
			return this.sessionState.GetDrivesForProvider(providerName);
		}

		// Token: 0x04000061 RID: 97
		private SessionStateInternal sessionState;
	}
}
