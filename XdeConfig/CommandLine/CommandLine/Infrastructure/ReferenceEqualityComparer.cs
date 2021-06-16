using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000062 RID: 98
	internal sealed class ReferenceEqualityComparer : IEqualityComparer, IEqualityComparer<object>
	{
		// Token: 0x06000279 RID: 633 RVA: 0x0000A33A File Offset: 0x0000853A
		public bool Equals(object x, object y)
		{
			return x == y;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A340 File Offset: 0x00008540
		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		// Token: 0x040000C2 RID: 194
		public static readonly ReferenceEqualityComparer Default = new ReferenceEqualityComparer();
	}
}
