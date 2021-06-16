using System;
using System.Collections.Generic;

namespace DiscUtils
{
	// Token: 0x02000031 RID: 49
	public sealed class VirtualDiskParameters
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00004E7D File Offset: 0x0000307D
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00004E85 File Offset: 0x00003085
		public GenericDiskAdapterType AdapterType { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00004E8E File Offset: 0x0000308E
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x00004E96 File Offset: 0x00003096
		public Geometry BiosGeometry { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00004E9F File Offset: 0x0000309F
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00004EA7 File Offset: 0x000030A7
		public long Capacity { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00004EB0 File Offset: 0x000030B0
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00004EB8 File Offset: 0x000030B8
		public VirtualDiskClass DiskType { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00004EC1 File Offset: 0x000030C1
		public Dictionary<string, string> ExtendedParameters { get; } = new Dictionary<string, string>();

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00004EC9 File Offset: 0x000030C9
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00004ED1 File Offset: 0x000030D1
		public Geometry Geometry { get; set; }
	}
}
