using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D3 RID: 211
	internal class ScanFilter : PathFilter
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C0F RID: 3087 RVA: 0x00030BD9 File Offset: 0x0002EDD9
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x00030BE1 File Offset: 0x0002EDE1
		public string Name { get; set; }

		// Token: 0x06000C11 RID: 3089 RVA: 0x00030BEA File Offset: 0x0002EDEA
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken c in current)
			{
				if (this.Name == null)
				{
					yield return c;
				}
				JToken value = c;
				for (;;)
				{
					JContainer container = value as JContainer;
					value = PathFilter.GetNextScanValue(c, container, value);
					if (value == null)
					{
						break;
					}
					JProperty jproperty;
					if ((jproperty = (value as JProperty)) != null)
					{
						if (jproperty.Name == this.Name)
						{
							yield return jproperty.Value;
						}
					}
					else if (this.Name == null)
					{
						yield return value;
					}
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
