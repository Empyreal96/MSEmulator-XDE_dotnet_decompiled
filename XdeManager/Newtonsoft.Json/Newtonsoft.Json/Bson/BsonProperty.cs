using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000107 RID: 263
	internal class BsonProperty
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00036AFB File Offset: 0x00034CFB
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00036B03 File Offset: 0x00034D03
		public BsonString Name { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00036B0C File Offset: 0x00034D0C
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00036B14 File Offset: 0x00034D14
		public BsonToken Value { get; set; }
	}
}
