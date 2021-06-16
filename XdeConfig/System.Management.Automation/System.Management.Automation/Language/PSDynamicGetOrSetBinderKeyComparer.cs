using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200060D RID: 1549
	internal class PSDynamicGetOrSetBinderKeyComparer : IEqualityComparer<Tuple<Type, bool>>
	{
		// Token: 0x0600436E RID: 17262 RVA: 0x00162B7C File Offset: 0x00160D7C
		public bool Equals(Tuple<Type, bool> x, Tuple<Type, bool> y)
		{
			return x.Item1 == y.Item1 && x.Item2 == y.Item2;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x00162BA1 File Offset: 0x00160DA1
		public int GetHashCode(Tuple<Type, bool> obj)
		{
			return obj.GetHashCode();
		}
	}
}
