using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000C6 RID: 198
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0002F23C File Offset: 0x0002D43C
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x0002F244 File Offset: 0x0002D444
		public List<int> Indexes { get; set; }

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002F24D File Offset: 0x0002D44D
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				foreach (int index in this.Indexes)
				{
					JToken tokenIndex = PathFilter.GetTokenIndex(t, errorWhenNoMatch, index);
					if (tokenIndex != null)
					{
						yield return tokenIndex;
					}
				}
				List<int>.Enumerator enumerator2 = default(List<int>.Enumerator);
				t = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}
	}
}
