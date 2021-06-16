using System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000029 RID: 41
	public struct OrientationReading2
	{
		// Token: 0x0600016C RID: 364 RVA: 0x00006A69 File Offset: 0x00004C69
		public OrientationReading2(float angle, Vector3F c3, Vector3F r2, PanelId panelId)
		{
			this.Angle = angle;
			this.C3 = c3;
			this.R2 = r2;
			this.PanelId = panelId;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006A88 File Offset: 0x00004C88
		public OrientationReading2(float angle, float x1, float y1, float z1, float x2, float y2, float z2, PanelId panelId)
		{
			this.Angle = angle;
			this.C3 = new Vector3F(x1, y1, z1);
			this.R2 = new Vector3F(x2, y2, z2);
			this.PanelId = panelId;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006ABC File Offset: 0x00004CBC
		public override string ToString()
		{
			return string.Format("Angle: {0}, C3: {1},{2},{3}, R2: {4},{5},{6}, P: {7}", new object[]
			{
				this.Angle,
				this.C3.X,
				this.C3.Y,
				this.C3.Z,
				this.R2.X,
				this.R2.Y,
				this.R2.Z,
				this.PanelId
			});
		}

		// Token: 0x040000DF RID: 223
		public float Angle;

		// Token: 0x040000E0 RID: 224
		public Vector3F C3;

		// Token: 0x040000E1 RID: 225
		public Vector3F R2;

		// Token: 0x040000E2 RID: 226
		public PanelId PanelId;
	}
}
