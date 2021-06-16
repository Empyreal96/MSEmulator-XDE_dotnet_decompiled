using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000102 RID: 258
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000D8B RID: 3467 RVA: 0x00036A1A File Offset: 0x00034C1A
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00036A30 File Offset: 0x00034C30
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00036A38 File Offset: 0x00034C38
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400041C RID: 1052
		private readonly object _value;

		// Token: 0x0400041D RID: 1053
		private readonly BsonType _type;
	}
}
