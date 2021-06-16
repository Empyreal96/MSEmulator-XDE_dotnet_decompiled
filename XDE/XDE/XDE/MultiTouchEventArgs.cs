using System;
using System.Windows;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000020 RID: 32
	public class MultiTouchEventArgs : EventArgs
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x000080F0 File Offset: 0x000062F0
		public MultiTouchEventArgs(Point point1, Point point2, TouchEventType type)
		{
			this.Point1 = point1;
			this.Point2 = point2;
			this.Type = type;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000810D File Offset: 0x0000630D
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00008115 File Offset: 0x00006315
		public TouchEventType Type { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000811E File Offset: 0x0000631E
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00008126 File Offset: 0x00006326
		public Point Point1 { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000812F File Offset: 0x0000632F
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00008137 File Offset: 0x00006337
		public Point Point2 { get; private set; }
	}
}
