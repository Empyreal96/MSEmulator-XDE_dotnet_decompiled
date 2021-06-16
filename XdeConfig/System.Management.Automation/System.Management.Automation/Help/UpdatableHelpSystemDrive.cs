using System;
using System.IO;

namespace System.Management.Automation.Help
{
	// Token: 0x020001DF RID: 479
	internal class UpdatableHelpSystemDrive : IDisposable
	{
		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x0008C180 File Offset: 0x0008A380
		internal string DriveName
		{
			get
			{
				return this._driveName + ":\\";
			}
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0008C194 File Offset: 0x0008A394
		internal UpdatableHelpSystemDrive(PSCmdlet cmdlet, string path, PSCredential credential)
		{
			int i = 0;
			while (i < 6)
			{
				this._driveName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
				this._cmdlet = cmdlet;
				if (path.EndsWith("\\", StringComparison.OrdinalIgnoreCase) || path.EndsWith("/", StringComparison.OrdinalIgnoreCase))
				{
					path = path.Remove(path.Length - 1);
				}
				PSDriveInfo psdriveInfo = cmdlet.SessionState.Drive.GetAtScope(this._driveName, "local");
				if (psdriveInfo != null)
				{
					if (psdriveInfo.Root.Equals(path))
					{
						return;
					}
					if (i < 5)
					{
						i++;
						continue;
					}
					cmdlet.SessionState.Drive.Remove(this._driveName, true, "local");
				}
				psdriveInfo = new PSDriveInfo(this._driveName, cmdlet.SessionState.Internal.GetSingleProvider("FileSystem"), path, string.Empty, credential);
				cmdlet.SessionState.Drive.New(psdriveInfo, "local");
				return;
			}
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0008C290 File Offset: 0x0008A490
		public void Dispose()
		{
			PSDriveInfo atScope = this._cmdlet.SessionState.Drive.GetAtScope(this._driveName, "local");
			if (atScope != null)
			{
				this._cmdlet.SessionState.Drive.Remove(this._driveName, true, "local");
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000962 RID: 2402
		private string _driveName;

		// Token: 0x04000963 RID: 2403
		private PSCmdlet _cmdlet;
	}
}
