using System;
using System.Drawing;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class VmUserSettings
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000B87F File Offset: 0x00009A7F
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000B887 File Offset: 0x00009A87
		public string Name { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000B890 File Offset: 0x00009A90
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000B898 File Offset: 0x00009A98
		public Point ScreenLocation { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000B8A1 File Offset: 0x00009AA1
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000B8A9 File Offset: 0x00009AA9
		public int Zoom
		{
			get
			{
				return this.zoom;
			}
			set
			{
				this.zoom = MathUtils.Clamp<int>(value, 10, 100);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000B8BB File Offset: 0x00009ABB
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000B8C3 File Offset: 0x00009AC3
		public string DefaultSnapshot { get; set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000B8CC File Offset: 0x00009ACC
		// (set) Token: 0x0600030D RID: 781 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		public DisplayOrientation DisplayOrientation { get; set; }

		// Token: 0x04000107 RID: 263
		private const int MaxZoom = 100;

		// Token: 0x04000108 RID: 264
		private const int MinZoom = 10;

		// Token: 0x04000109 RID: 265
		private int zoom = 60;
	}
}
