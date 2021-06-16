using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x020000FE RID: 254
	internal abstract class BsonToken
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000D78 RID: 3448
		public abstract BsonType Type { get; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0003691E File Offset: 0x00034B1E
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00036926 File Offset: 0x00034B26
		public BsonToken Parent { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0003692F File Offset: 0x00034B2F
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x00036937 File Offset: 0x00034B37
		public int CalculatedSize { get; set; }
	}
}
