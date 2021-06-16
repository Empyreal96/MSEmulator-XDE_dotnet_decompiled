using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000108 RID: 264
	internal enum BsonType : sbyte
	{
		// Token: 0x04000428 RID: 1064
		Number = 1,
		// Token: 0x04000429 RID: 1065
		String,
		// Token: 0x0400042A RID: 1066
		Object,
		// Token: 0x0400042B RID: 1067
		Array,
		// Token: 0x0400042C RID: 1068
		Binary,
		// Token: 0x0400042D RID: 1069
		Undefined,
		// Token: 0x0400042E RID: 1070
		Oid,
		// Token: 0x0400042F RID: 1071
		Boolean,
		// Token: 0x04000430 RID: 1072
		Date,
		// Token: 0x04000431 RID: 1073
		Null,
		// Token: 0x04000432 RID: 1074
		Regex,
		// Token: 0x04000433 RID: 1075
		Reference,
		// Token: 0x04000434 RID: 1076
		Code,
		// Token: 0x04000435 RID: 1077
		Symbol,
		// Token: 0x04000436 RID: 1078
		CodeWScope,
		// Token: 0x04000437 RID: 1079
		Integer,
		// Token: 0x04000438 RID: 1080
		TimeStamp,
		// Token: 0x04000439 RID: 1081
		Long,
		// Token: 0x0400043A RID: 1082
		MinKey = -1,
		// Token: 0x0400043B RID: 1083
		MaxKey = 127
	}
}
