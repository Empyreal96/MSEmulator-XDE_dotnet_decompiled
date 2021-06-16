using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000194 RID: 404
	internal class CabinetExtractorFactory
	{
		// Token: 0x06001389 RID: 5001 RVA: 0x00078F8B File Offset: 0x0007718B
		static CabinetExtractorFactory()
		{
			CabinetExtractorFactory.cabinetLoader = CabinetExtractorLoader.GetInstance();
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00078FA1 File Offset: 0x000771A1
		internal static ICabinetExtractor GetCabinetExtractor()
		{
			if (CabinetExtractorFactory.cabinetLoader != null)
			{
				return CabinetExtractorFactory.cabinetLoader.GetCabinetExtractor();
			}
			return CabinetExtractorFactory.EmptyExtractor;
		}

		// Token: 0x04000851 RID: 2129
		private static ICabinetExtractorLoader cabinetLoader;

		// Token: 0x04000852 RID: 2130
		internal static ICabinetExtractor EmptyExtractor = new EmptyCabinetExtractor();
	}
}
