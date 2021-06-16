using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000105 RID: 261
	internal class BsonBinary : BsonValue
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00036A91 File Offset: 0x00034C91
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x00036A99 File Offset: 0x00034C99
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x06000D96 RID: 3478 RVA: 0x00036AA2 File Offset: 0x00034CA2
		public BsonBinary(byte[] value, BsonBinaryType binaryType) : base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
