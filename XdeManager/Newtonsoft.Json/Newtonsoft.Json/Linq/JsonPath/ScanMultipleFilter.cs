using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D4 RID: 212
	internal class ScanMultipleFilter : PathFilter
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00030C09 File Offset: 0x0002EE09
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00030C11 File Offset: 0x0002EE11
		public List<string> Names { get; set; }

		// Token: 0x06000C15 RID: 3093 RVA: 0x00030C1A File Offset: 0x0002EE1A
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken c in current)
			{
				JToken value = c;
				for (;;)
				{
					JContainer container = value as JContainer;
					value = PathFilter.GetNextScanValue(c, container, value);
					if (value == null)
					{
						break;
					}
					JProperty property;
					if ((property = (value as JProperty)) != null)
					{
						foreach (string b in this.Names)
						{
							if (property.Name == b)
							{
								yield return property.Value;
							}
						}
						List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					}
					property = null;
				}
				value = null;
				c = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}
	}
}
