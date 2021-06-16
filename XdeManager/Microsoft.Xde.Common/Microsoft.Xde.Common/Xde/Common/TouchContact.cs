using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000034 RID: 52
	public struct TouchContact
	{
		// Token: 0x04000106 RID: 262
		public short ContactId;

		// Token: 0x04000107 RID: 263
		public InputEventFlag Flags;

		// Token: 0x04000108 RID: 264
		public short ScreenX;

		// Token: 0x04000109 RID: 265
		public short ScreenY;

		// Token: 0x0400010A RID: 266
		public short WindowX;

		// Token: 0x0400010B RID: 267
		public short WindowY;

		// Token: 0x0400010C RID: 268
		public short AreaX;

		// Token: 0x0400010D RID: 269
		public short AreaY;

		// Token: 0x0400010E RID: 270
		public short DistanceZ;

		// Token: 0x0400010F RID: 271
		public short Reserved1;

		// Token: 0x04000110 RID: 272
		public short Reserved2;

		// Token: 0x04000111 RID: 273
		public short Reserved3;

		// Token: 0x04000112 RID: 274
		public ulong InputSink;

		// Token: 0x04000113 RID: 275
		public Matrix3X2 InputTransform;
	}
}
