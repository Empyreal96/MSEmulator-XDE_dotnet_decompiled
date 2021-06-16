using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000CD RID: 205
	internal abstract class QueryExpression
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0003061D File Offset: 0x0002E81D
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x00030625 File Offset: 0x0002E825
		public QueryOperator Operator { get; set; }

		// Token: 0x06000BF3 RID: 3059
		public abstract bool IsMatch(JToken root, JToken t);
	}
}
