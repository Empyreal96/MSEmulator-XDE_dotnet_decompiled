using System;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000027 RID: 39
	public struct OcclusionReading
	{
		// Token: 0x06000169 RID: 361 RVA: 0x00006944 File Offset: 0x00004B44
		public override string ToString()
		{
			return string.Format("Type: {0} DockingState: {1} X: {2} Y: {3} Width: {4} Height: {5}", new object[]
			{
				this.DeviceType,
				this.DockingState,
				this.X,
				this.Y,
				this.Width,
				this.Height
			});
		}

		// Token: 0x040000D6 RID: 214
		public OcclusionDeviceType DeviceType;

		// Token: 0x040000D7 RID: 215
		public byte DockingState;

		// Token: 0x040000D8 RID: 216
		public ushort X;

		// Token: 0x040000D9 RID: 217
		public ushort Y;

		// Token: 0x040000DA RID: 218
		public ushort Width;

		// Token: 0x040000DB RID: 219
		public ushort Height;
	}
}
