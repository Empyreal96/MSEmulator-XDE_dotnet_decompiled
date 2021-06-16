using System;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000197 RID: 407
	internal class CabinetExtractorLoader : ICabinetExtractorLoader
	{
		// Token: 0x06001395 RID: 5013 RVA: 0x00079278 File Offset: 0x00077478
		internal static CabinetExtractorLoader GetInstance()
		{
			if (0.0 == Interlocked.CompareExchange(ref CabinetExtractorLoader.created, 1.0, 0.0))
			{
				CabinetExtractorLoader.instance = new CabinetExtractorLoader();
				CabinetExtractorLoader.extractorInstance = new CabinetExtractor();
			}
			return CabinetExtractorLoader.instance;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000792C5 File Offset: 0x000774C5
		internal override ICabinetExtractor GetCabinetExtractor()
		{
			return CabinetExtractorLoader.extractorInstance;
		}

		// Token: 0x04000865 RID: 2149
		private static CabinetExtractor extractorInstance;

		// Token: 0x04000866 RID: 2150
		private static CabinetExtractorLoader instance;

		// Token: 0x04000867 RID: 2151
		private static double created = 0.0;
	}
}
