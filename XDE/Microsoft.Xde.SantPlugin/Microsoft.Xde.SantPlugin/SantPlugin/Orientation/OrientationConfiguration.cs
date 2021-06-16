using System;
using System.Windows.Media.Media3D;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200001F RID: 31
	public struct OrientationConfiguration
	{
		// Token: 0x0600012C RID: 300 RVA: 0x000057A3 File Offset: 0x000039A3
		public bool Equals(OrientationConfiguration other)
		{
			return this.LeftAngle == other.LeftAngle && this.RightAngle == other.RightAngle && this.YRotation == other.YRotation && this.ZRotation == other.ZRotation;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000057E0 File Offset: 0x000039E0
		public OrientationReading2 GetReading()
		{
			float angle = (float)(this.RightAngle - this.LeftAngle);
			Matrix3D identity = Matrix3D.Identity;
			Quaternion quaternion = new Quaternion(OrientationConfiguration.Zaxis, this.ZRotation);
			identity.Rotate(new Quaternion(OrientationConfiguration.Yaxis, 180.0 + this.YRotation + this.LeftAngle));
			identity.Rotate(quaternion);
			identity.Invert();
			Vector3D v = OrientationConfiguration.Gravity * identity;
			Matrix3D identity2 = Matrix3D.Identity;
			identity2.Rotate(new Quaternion(OrientationConfiguration.Yaxis, 0.0 + this.YRotation + this.RightAngle));
			identity2.Rotate(quaternion);
			identity2.Invert();
			Vector3D v2 = OrientationConfiguration.Gravity * identity2;
			return new OrientationReading2(angle, Vector3F.FromVector3D(v), Vector3F.FromVector3D(v2), this.PanelId);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000058B8 File Offset: 0x00003AB8
		public override string ToString()
		{
			return string.Format("LeftAngle: {0}, RightAngle: {1}, ZRot: {2}, YRot: {3}", new object[]
			{
				this.LeftAngle,
				this.RightAngle,
				this.ZRotation,
				this.YRotation
			});
		}

		// Token: 0x040000A7 RID: 167
		public double LeftAngle;

		// Token: 0x040000A8 RID: 168
		public double RightAngle;

		// Token: 0x040000A9 RID: 169
		public double ZRotation;

		// Token: 0x040000AA RID: 170
		public double YRotation;

		// Token: 0x040000AB RID: 171
		public PanelId PanelId;

		// Token: 0x040000AC RID: 172
		private static readonly Vector3D Yaxis = new Vector3D(0.0, 1.0, 0.0);

		// Token: 0x040000AD RID: 173
		private static readonly Vector3D Zaxis = new Vector3D(0.0, 0.0, 1.0);

		// Token: 0x040000AE RID: 174
		private static readonly Vector3D Gravity = new Vector3D(0.0, -1.0, 0.0);
	}
}
