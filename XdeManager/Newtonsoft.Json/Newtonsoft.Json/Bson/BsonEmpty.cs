using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000101 RID: 257
	internal class BsonEmpty : BsonToken
	{
		// Token: 0x06000D88 RID: 3464 RVA: 0x000369EA File Offset: 0x00034BEA
		private BsonEmpty(BsonType type)
		{
			this.Type = type;
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x000369F9 File Offset: 0x00034BF9
		public override BsonType Type { get; }

		// Token: 0x04000419 RID: 1049
		public static readonly BsonToken Null = new BsonEmpty(BsonType.Null);

		// Token: 0x0400041A RID: 1050
		public static readonly BsonToken Undefined = new BsonEmpty(BsonType.Undefined);
	}
}
