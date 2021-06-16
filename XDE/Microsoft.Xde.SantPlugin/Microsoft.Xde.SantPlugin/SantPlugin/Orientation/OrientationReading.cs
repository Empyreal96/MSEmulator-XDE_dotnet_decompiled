using System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000028 RID: 40
	public struct OrientationReading
	{
		// Token: 0x0600016A RID: 362 RVA: 0x000069B5 File Offset: 0x00004BB5
		public OrientationReading(float angle, Vector3F c3, Vector3F r2)
		{
			this.Angle = angle;
			this.C3 = c3;
			this.R2 = r2;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000069CC File Offset: 0x00004BCC
		public override string ToString()
		{
			return string.Format("Angle: {0}, C3: {1},{2},{3}, R2: {4},{5},{6}", new object[]
			{
				this.Angle,
				this.C3.X,
				this.C3.Y,
				this.C3.Z,
				this.R2.X,
				this.R2.Y,
				this.R2.Z
			});
		}

		// Token: 0x040000DC RID: 220
		public float Angle;

		// Token: 0x040000DD RID: 221
		public Vector3F C3;

		// Token: 0x040000DE RID: 222
		public Vector3F R2;
	}
}
