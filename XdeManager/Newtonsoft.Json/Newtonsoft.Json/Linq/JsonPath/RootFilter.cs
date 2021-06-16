using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D2 RID: 210
	internal class RootFilter : PathFilter
	{
		// Token: 0x06000C0C RID: 3084 RVA: 0x00030BB9 File Offset: 0x0002EDB9
		private RootFilter()
		{
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00030BC1 File Offset: 0x0002EDC1
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new JToken[]
			{
				root
			};
		}

		// Token: 0x040003C6 RID: 966
		public static readonly RootFilter Instance = new RootFilter();
	}
}
