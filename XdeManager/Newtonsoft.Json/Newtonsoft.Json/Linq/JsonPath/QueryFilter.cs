using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D0 RID: 208
	internal class QueryFilter : PathFilter
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00030B4B File Offset: 0x0002ED4B
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x00030B53 File Offset: 0x0002ED53
		public QueryExpression Expression { get; set; }

		// Token: 0x06000C06 RID: 3078 RVA: 0x00030B5C File Offset: 0x0002ED5C
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken jtoken in current)
			{
				foreach (JToken jtoken2 in ((IEnumerable<JToken>)jtoken))
				{
					if (this.Expression.IsMatch(root, jtoken2))
					{
						yield return jtoken2;
					}
				}
				IEnumerator<JToken> enumerator2 = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}
	}
}
