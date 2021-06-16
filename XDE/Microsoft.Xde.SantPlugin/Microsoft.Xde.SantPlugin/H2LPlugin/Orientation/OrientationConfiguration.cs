using System;
using System.Windows.Media.Media3D;
using Microsoft.Xde.Common;
using Microsoft.Xde.SantPlugin.Orientation;

namespace Microsoft.Xde.H2LPlugin.Orientation
{
	// Token: 0x0200000E RID: 14
	public struct OrientationConfiguration
	{
		// Token: 0x06000050 RID: 80 RVA: 0x0000312A File Offset: 0x0000132A
		public bool Equals(OrientationConfiguration other)
		{
			return this.Angle1 == other.Angle1 && this.Angle2 == other.Angle2 && this.XRotation == other.XRotation;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003158 File Offset: 0x00001358
		public OrientationReading2 GetReading()
		{
			Matrix3D identity = Matrix3D.Identity;
			Quaternion quaternion = new Quaternion(OrientationConfiguration.Zaxis, this.ZRotation);
			identity.Rotate(new Quaternion(OrientationConfiguration.Xaxis, 180.0 - this.Angle1 - this.Angle2 + this.XRotation));
			identity.Rotate(quaternion);
			identity.Invert();
			Vector3D v = OrientationConfiguration.Gravity * identity;
			Matrix3D identity2 = Matrix3D.Identity;
			identity2.Rotate(new Quaternion(OrientationConfiguration.Xaxis, 0.0 + this.XRotation));
			identity2.Rotate(quaternion);
			identity2.Invert();
			Vector3D v2 = OrientationConfiguration.Gravity * identity2;
			return new OrientationReading2((float)this.Angle1, Vector3F.FromVector3D(v), Vector3F.FromVector3D(v2), PanelId.Left);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003224 File Offset: 0x00001424
		public override string ToString()
		{
			return string.Format("Angle1: {0}, Angle2: {1}, ZRot: {2}, XRot: {3}", new object[]
			{
				this.Angle1,
				this.Angle2,
				this.ZRotation,
				this.XRotation
			});
		}

		// Token: 0x0400002B RID: 43
		public double Angle1;

		// Token: 0x0400002C RID: 44
		public double Angle2;

		// Token: 0x0400002D RID: 45
		public double ZRotation;

		// Token: 0x0400002E RID: 46
		public double XRotation;

		// Token: 0x0400002F RID: 47
		public ushort OcclusionPercent;

		// Token: 0x04000030 RID: 48
		private static readonly Vector3D Xaxis = new Vector3D(1.0, 0.0, 0.0);

		// Token: 0x04000031 RID: 49
		private static readonly Vector3D Yaxis = new Vector3D(0.0, 1.0, 0.0);

		// Token: 0x04000032 RID: 50
		private static readonly Vector3D Zaxis = new Vector3D(0.0, 0.0, 1.0);

		// Token: 0x04000033 RID: 51
		private static readonly Vector3D Gravity = new Vector3D(0.0, -1.0, 0.0);
	}
}
