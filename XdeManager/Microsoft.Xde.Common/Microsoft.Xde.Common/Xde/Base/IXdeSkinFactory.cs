using System;
using System.Collections.Generic;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000019 RID: 25
	public interface IXdeSkinFactory
	{
		// Token: 0x060000A0 RID: 160
		IXdeSkin LoadSkinFromResolution(string resolutionName, XdeSensors sensors);

		// Token: 0x060000A1 RID: 161
		bool IsValidResolutionName(string resolutionName);

		// Token: 0x060000A2 RID: 162
		SkinDisplay GetSkinDisplayInformation(string resolutionName);

		// Token: 0x060000A3 RID: 163
		IEnumerable<SkinDisplay> GetAllSkinDisplayInformation();
	}
}
