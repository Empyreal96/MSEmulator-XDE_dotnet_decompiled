using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A8 RID: 168
	[Flags]
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum JsonSchemaType
	{
		// Token: 0x04000342 RID: 834
		None = 0,
		// Token: 0x04000343 RID: 835
		String = 1,
		// Token: 0x04000344 RID: 836
		Float = 2,
		// Token: 0x04000345 RID: 837
		Integer = 4,
		// Token: 0x04000346 RID: 838
		Boolean = 8,
		// Token: 0x04000347 RID: 839
		Object = 16,
		// Token: 0x04000348 RID: 840
		Array = 32,
		// Token: 0x04000349 RID: 841
		Null = 64,
		// Token: 0x0400034A RID: 842
		Any = 127
	}
}
