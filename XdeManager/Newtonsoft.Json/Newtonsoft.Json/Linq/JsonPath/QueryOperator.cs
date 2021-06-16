using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000CC RID: 204
	internal enum QueryOperator
	{
		// Token: 0x040003B3 RID: 947
		None,
		// Token: 0x040003B4 RID: 948
		Equals,
		// Token: 0x040003B5 RID: 949
		NotEquals,
		// Token: 0x040003B6 RID: 950
		Exists,
		// Token: 0x040003B7 RID: 951
		LessThan,
		// Token: 0x040003B8 RID: 952
		LessThanOrEquals,
		// Token: 0x040003B9 RID: 953
		GreaterThan,
		// Token: 0x040003BA RID: 954
		GreaterThanOrEquals,
		// Token: 0x040003BB RID: 955
		And,
		// Token: 0x040003BC RID: 956
		Or,
		// Token: 0x040003BD RID: 957
		RegexEquals,
		// Token: 0x040003BE RID: 958
		StrictEquals,
		// Token: 0x040003BF RID: 959
		StrictNotEquals
	}
}
