using System;
using System.Text;

namespace DiscUtils
{
	// Token: 0x02000012 RID: 18
	public sealed class FileSystemParameters
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00002DA4 File Offset: 0x00000FA4
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00002DAC File Offset: 0x00000FAC
		public Encoding FileNameEncoding { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00002DB5 File Offset: 0x00000FB5
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00002DBD File Offset: 0x00000FBD
		public TimeConverter TimeConverter { get; set; }
	}
}
