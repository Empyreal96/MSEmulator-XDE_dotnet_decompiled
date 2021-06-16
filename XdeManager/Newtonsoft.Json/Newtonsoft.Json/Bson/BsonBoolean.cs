using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000103 RID: 259
	internal class BsonBoolean : BsonValue
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x00036A40 File Offset: 0x00034C40
		private BsonBoolean(bool value) : base(value, BsonType.Boolean)
		{
		}

		// Token: 0x0400041E RID: 1054
		public static readonly BsonBoolean False = new BsonBoolean(false);

		// Token: 0x0400041F RID: 1055
		public static readonly BsonBoolean True = new BsonBoolean(true);
	}
}
