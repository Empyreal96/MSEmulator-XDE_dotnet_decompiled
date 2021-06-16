using System;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000038 RID: 56
	public class ResolutionChangedEventArgs : EventArgs
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00004E4A File Offset: 0x0000304A
		public ResolutionChangedEventArgs(Size resolution)
		{
			this.Resolution = resolution;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00004E59 File Offset: 0x00003059
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00004E61 File Offset: 0x00003061
		public Size Resolution { get; private set; }
	}
}
