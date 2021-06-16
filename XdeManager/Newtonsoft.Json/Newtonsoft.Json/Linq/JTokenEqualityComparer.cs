using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BD RID: 189
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x0002D4D2 File Offset: 0x0002B6D2
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002D4DB File Offset: 0x0002B6DB
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
