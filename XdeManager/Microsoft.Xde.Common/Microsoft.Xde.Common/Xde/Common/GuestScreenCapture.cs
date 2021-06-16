using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200005A RID: 90
	public struct GuestScreenCapture
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00004EEE File Offset: 0x000030EE
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00004EF6 File Offset: 0x000030F6
		public int Width { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00004EFF File Offset: 0x000030FF
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00004F07 File Offset: 0x00003107
		public int Height { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00004F10 File Offset: 0x00003110
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00004F18 File Offset: 0x00003118
		public int Stride { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00004F21 File Offset: 0x00003121
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00004F29 File Offset: 0x00003129
		public int PixelFormat { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00004F32 File Offset: 0x00003132
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00004F3A File Offset: 0x0000313A
		public byte[] RawData { get; set; }
	}
}
