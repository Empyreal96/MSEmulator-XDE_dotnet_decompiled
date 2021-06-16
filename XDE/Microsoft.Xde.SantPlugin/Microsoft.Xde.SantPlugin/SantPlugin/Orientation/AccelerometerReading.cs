using System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000024 RID: 36
	public struct AccelerometerReading
	{
		// Token: 0x06000167 RID: 359 RVA: 0x000068FE File Offset: 0x00004AFE
		public override string ToString()
		{
			return string.Format("PanelGroup: {0}, Vector: {1}", this.PanelGroup, this.Vector);
		}

		// Token: 0x040000CE RID: 206
		public uint PanelGroup;

		// Token: 0x040000CF RID: 207
		public Vector3F Vector;
	}
}
