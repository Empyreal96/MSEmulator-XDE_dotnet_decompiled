using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000104 RID: 260
	internal class BsonString : BsonValue
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00036A67 File Offset: 0x00034C67
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x00036A6F File Offset: 0x00034C6F
		public int ByteCount { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00036A78 File Offset: 0x00034C78
		public bool IncludeLength { get; }

		// Token: 0x06000D93 RID: 3475 RVA: 0x00036A80 File Offset: 0x00034C80
		public BsonString(object value, bool includeLength) : base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
