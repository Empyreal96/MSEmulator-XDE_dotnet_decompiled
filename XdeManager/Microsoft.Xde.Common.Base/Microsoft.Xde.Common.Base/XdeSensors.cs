using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001C RID: 28
	[Flags]
	public enum XdeSensors
	{
		// Token: 0x040000C6 RID: 198
		None = 0,
		// Token: 0x040000C7 RID: 199
		ALS = 1,
		// Token: 0x040000C8 RID: 200
		Camera_8_1 = 2,
		// Token: 0x040000C9 RID: 201
		FFC = 4,
		// Token: 0x040000CA RID: 202
		Gyro = 8,
		// Token: 0x040000CB RID: 203
		Magnetometer = 16,
		// Token: 0x040000CC RID: 204
		NFC = 32,
		// Token: 0x040000CD RID: 205
		Reserved = 64,
		// Token: 0x040000CE RID: 206
		SoftwareButtons = 128,
		// Token: 0x040000CF RID: 207
		DesktopGPU = 256,
		// Token: 0x040000D0 RID: 208
		Max = 512,
		// Token: 0x040000D1 RID: 209
		All = 511,
		// Token: 0x040000D2 RID: 210
		Default = 383
	}
}
