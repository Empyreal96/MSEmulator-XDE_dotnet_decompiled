using System;

namespace System.Management.Automation
{
	// Token: 0x02000824 RID: 2084
	internal class DriveScopeItemSearcher : ScopedItemSearcher<PSDriveInfo>
	{
		// Token: 0x06004FF1 RID: 20465 RVA: 0x001A7B40 File Offset: 0x001A5D40
		public DriveScopeItemSearcher(SessionStateInternal sessionState, VariablePath lookupPath) : base(sessionState, lookupPath)
		{
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001A7B4C File Offset: 0x001A5D4C
		protected override bool GetScopeItem(SessionStateScope scope, VariablePath name, out PSDriveInfo drive)
		{
			bool result = true;
			drive = scope.GetDrive(name.DriveName);
			if (drive == null)
			{
				result = false;
			}
			return result;
		}
	}
}
