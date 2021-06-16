using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000193 RID: 403
	internal abstract class ICabinetExtractorLoader
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x00078F80 File Offset: 0x00077180
		internal virtual ICabinetExtractor GetCabinetExtractor()
		{
			return null;
		}
	}
}
