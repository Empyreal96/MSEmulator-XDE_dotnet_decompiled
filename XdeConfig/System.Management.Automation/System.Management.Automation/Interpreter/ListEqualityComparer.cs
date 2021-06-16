using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200075B RID: 1883
	internal sealed class ListEqualityComparer<T> : EqualityComparer<ICollection<T>>
	{
		// Token: 0x06004B16 RID: 19222 RVA: 0x0018943D File Offset: 0x0018763D
		private ListEqualityComparer()
		{
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x00189445 File Offset: 0x00187645
		public override bool Equals(ICollection<T> x, ICollection<T> y)
		{
			return x.ListEquals(y);
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x0018944E File Offset: 0x0018764E
		public override int GetHashCode(ICollection<T> obj)
		{
			return obj.ListHashCode<T>();
		}

		// Token: 0x04002446 RID: 9286
		internal static readonly ListEqualityComparer<T> Instance = new ListEqualityComparer<T>();
	}
}
