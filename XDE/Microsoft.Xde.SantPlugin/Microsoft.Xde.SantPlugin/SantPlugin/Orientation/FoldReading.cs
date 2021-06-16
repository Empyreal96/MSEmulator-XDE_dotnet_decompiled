using System;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000025 RID: 37
	public struct FoldReading
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00006920 File Offset: 0x00004B20
		public override string ToString()
		{
			return string.Format("ContributingPanelId: {0}, Angle: {1}", this.ContributingPanelId, this.Angle);
		}

		// Token: 0x040000D0 RID: 208
		public PanelId ContributingPanelId;

		// Token: 0x040000D1 RID: 209
		public float Angle;
	}
}
