using System;

namespace System.Management.Automation
{
	// Token: 0x0200098B RID: 2443
	public enum CompletionResultType
	{
		// Token: 0x04003037 RID: 12343
		Text,
		// Token: 0x04003038 RID: 12344
		History,
		// Token: 0x04003039 RID: 12345
		Command,
		// Token: 0x0400303A RID: 12346
		ProviderItem,
		// Token: 0x0400303B RID: 12347
		ProviderContainer,
		// Token: 0x0400303C RID: 12348
		Property,
		// Token: 0x0400303D RID: 12349
		Method,
		// Token: 0x0400303E RID: 12350
		ParameterName,
		// Token: 0x0400303F RID: 12351
		ParameterValue,
		// Token: 0x04003040 RID: 12352
		Variable,
		// Token: 0x04003041 RID: 12353
		Namespace,
		// Token: 0x04003042 RID: 12354
		Type,
		// Token: 0x04003043 RID: 12355
		Keyword,
		// Token: 0x04003044 RID: 12356
		DynamicKeyword
	}
}
