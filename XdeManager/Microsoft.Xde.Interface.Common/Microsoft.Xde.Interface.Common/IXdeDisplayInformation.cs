using System;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000B RID: 11
	public interface IXdeDisplayInformation
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000024 RID: 36
		float DisplayScale { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000025 RID: 37
		Size DisplaySize { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000026 RID: 38
		Size GuestLogicalResolution { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000027 RID: 39
		Size PhysicalResolution { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000028 RID: 40
		DisplayOrientation Orientation { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000029 RID: 41
		int GapWidth { get; }
	}
}
