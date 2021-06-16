using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000195 RID: 405
	internal sealed class EmptyCabinetExtractor : ICabinetExtractor
	{
		// Token: 0x0600138C RID: 5004 RVA: 0x00078FC2 File Offset: 0x000771C2
		internal override bool Extract(string cabinetName, string srcPath, string destPath)
		{
			return false;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00078FC5 File Offset: 0x000771C5
		protected override void Dispose(bool disposing)
		{
		}
	}
}
