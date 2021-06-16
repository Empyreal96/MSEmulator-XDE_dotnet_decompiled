using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000029 RID: 41
	public enum JsonToken
	{
		// Token: 0x040000E9 RID: 233
		None,
		// Token: 0x040000EA RID: 234
		StartObject,
		// Token: 0x040000EB RID: 235
		StartArray,
		// Token: 0x040000EC RID: 236
		StartConstructor,
		// Token: 0x040000ED RID: 237
		PropertyName,
		// Token: 0x040000EE RID: 238
		Comment,
		// Token: 0x040000EF RID: 239
		Raw,
		// Token: 0x040000F0 RID: 240
		Integer,
		// Token: 0x040000F1 RID: 241
		Float,
		// Token: 0x040000F2 RID: 242
		String,
		// Token: 0x040000F3 RID: 243
		Boolean,
		// Token: 0x040000F4 RID: 244
		Null,
		// Token: 0x040000F5 RID: 245
		Undefined,
		// Token: 0x040000F6 RID: 246
		EndObject,
		// Token: 0x040000F7 RID: 247
		EndArray,
		// Token: 0x040000F8 RID: 248
		EndConstructor,
		// Token: 0x040000F9 RID: 249
		Date,
		// Token: 0x040000FA RID: 250
		Bytes
	}
}
