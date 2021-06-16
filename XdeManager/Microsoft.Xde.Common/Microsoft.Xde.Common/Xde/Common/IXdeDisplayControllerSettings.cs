using System;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200002B RID: 43
	public interface IXdeDisplayControllerSettings
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600011D RID: 285
		// (set) Token: 0x0600011E RID: 286
		ResolutionType ResolutionType { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600011F RID: 287
		// (set) Token: 0x06000120 RID: 288
		Size Resolution { get; set; }
	}
}
