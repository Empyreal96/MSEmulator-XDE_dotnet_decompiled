using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000106 RID: 262
	internal class BsonRegex : BsonToken
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00036AB3 File Offset: 0x00034CB3
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00036ABB File Offset: 0x00034CBB
		public BsonString Pattern { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00036AC4 File Offset: 0x00034CC4
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00036ACC File Offset: 0x00034CCC
		public BsonString Options { get; set; }

		// Token: 0x06000D9B RID: 3483 RVA: 0x00036AD5 File Offset: 0x00034CD5
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00036AF7 File Offset: 0x00034CF7
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
