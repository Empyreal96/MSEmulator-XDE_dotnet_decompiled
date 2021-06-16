using System;

namespace System.Management.Automation
{
	// Token: 0x0200046F RID: 1135
	public sealed class PathInfo
	{
		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x0600329A RID: 12954 RVA: 0x00113E48 File Offset: 0x00112048
		public PSDriveInfo Drive
		{
			get
			{
				PSDriveInfo result = null;
				if (this.drive != null && !this.drive.Hidden)
				{
					result = this.drive;
				}
				return result;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x00113E7A File Offset: 0x0011207A
		public ProviderInfo Provider
		{
			get
			{
				return this.provider;
			}
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00113E82 File Offset: 0x00112082
		internal PSDriveInfo GetDrive()
		{
			return this.drive;
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x00113E8C File Offset: 0x0011208C
		public string ProviderPath
		{
			get
			{
				if (this.providerPath == null)
				{
					LocationGlobber locationGlobber = this.sessionState.Internal.ExecutionContext.LocationGlobber;
					this.providerPath = locationGlobber.GetProviderPath(this.Path);
				}
				return this.providerPath;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x00113ECF File Offset: 0x001120CF
		public string Path
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x00113ED8 File Offset: 0x001120D8
		public override string ToString()
		{
			string result = this.path;
			if (this.drive == null || this.drive.Hidden)
			{
				result = LocationGlobber.GetProviderQualifiedPath(this.path, this.provider);
			}
			else
			{
				result = LocationGlobber.GetDriveQualifiedPath(this.path, this.drive);
			}
			return result;
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x00113F30 File Offset: 0x00112130
		internal PathInfo(PSDriveInfo drive, ProviderInfo provider, string path, SessionState sessionState)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.drive = drive;
			this.provider = provider;
			this.path = path;
			this.sessionState = sessionState;
		}

		// Token: 0x04001A74 RID: 6772
		private string providerPath;

		// Token: 0x04001A75 RID: 6773
		private SessionState sessionState;

		// Token: 0x04001A76 RID: 6774
		private PSDriveInfo drive;

		// Token: 0x04001A77 RID: 6775
		private ProviderInfo provider;

		// Token: 0x04001A78 RID: 6776
		private string path = string.Empty;
	}
}
