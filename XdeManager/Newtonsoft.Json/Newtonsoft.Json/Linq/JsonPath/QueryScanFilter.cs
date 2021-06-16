using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D1 RID: 209
	internal class QueryScanFilter : PathFilter
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00030B82 File Offset: 0x0002ED82
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x00030B8A File Offset: 0x0002ED8A
		public QueryExpression Expression { get; set; }

		// Token: 0x06000C0A RID: 3082 RVA: 0x00030B93 File Offset: 0x0002ED93
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken jtoken in current)
			{
				JContainer jcontainer;
				if ((jcontainer = (jtoken as JContainer)) != null)
				{
					foreach (JToken jtoken2 in jcontainer.DescendantsAndSelf())
					{
						if (this.Expression.IsMatch(root, jtoken2))
						{
							yield return jtoken2;
						}
					}
					IEnumerator<JToken> enumerator2 = null;
				}
				else if (this.Expression.IsMatch(root, jtoken))
				{
					yield return jtoken;
				}
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}
	}
}
