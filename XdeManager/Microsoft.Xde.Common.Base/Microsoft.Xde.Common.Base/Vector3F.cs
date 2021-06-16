using System;
using System.Windows.Media.Media3D;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000027 RID: 39
	public struct Vector3F
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000410C File Offset: 0x0000230C
		public Vector3F(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00004123 File Offset: 0x00002323
		public static Vector3F FromVector3D(Vector3D v)
		{
			return new Vector3F((float)v.X, (float)v.Y, (float)v.Z);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00004142 File Offset: 0x00002342
		public override string ToString()
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", this.X, this.Y, this.Z);
		}

		// Token: 0x04000101 RID: 257
		public float X;

		// Token: 0x04000102 RID: 258
		public float Y;

		// Token: 0x04000103 RID: 259
		public float Z;
	}
}
