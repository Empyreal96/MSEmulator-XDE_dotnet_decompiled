using System;

namespace DiscUtils
{
	// Token: 0x02000032 RID: 50
	public sealed class VirtualDiskTypeInfo
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00004EED File Offset: 0x000030ED
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00004EF5 File Offset: 0x000030F5
		public GeometryCalculation CalcGeometry { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00004EFE File Offset: 0x000030FE
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00004F06 File Offset: 0x00003106
		public bool CanBeHardDisk { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00004F0F File Offset: 0x0000310F
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00004F17 File Offset: 0x00003117
		public bool DeterministicGeometry { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00004F20 File Offset: 0x00003120
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00004F28 File Offset: 0x00003128
		public string Name { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00004F31 File Offset: 0x00003131
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00004F39 File Offset: 0x00003139
		public bool PreservesBiosGeometry { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00004F42 File Offset: 0x00003142
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00004F4A File Offset: 0x0000314A
		public string Variant { get; set; }
	}
}
