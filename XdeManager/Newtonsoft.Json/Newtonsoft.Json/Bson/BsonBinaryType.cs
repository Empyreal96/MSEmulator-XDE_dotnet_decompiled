using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x020000FA RID: 250
	internal enum BsonBinaryType : byte
	{
		// Token: 0x040003FA RID: 1018
		Binary,
		// Token: 0x040003FB RID: 1019
		Function,
		// Token: 0x040003FC RID: 1020
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld,
		// Token: 0x040003FD RID: 1021
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld,
		// Token: 0x040003FE RID: 1022
		Uuid,
		// Token: 0x040003FF RID: 1023
		Md5,
		// Token: 0x04000400 RID: 1024
		UserDefined = 128
	}
}
