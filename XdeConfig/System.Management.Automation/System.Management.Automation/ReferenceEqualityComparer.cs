using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x020008A1 RID: 2209
	internal class ReferenceEqualityComparer : IEqualityComparer
	{
		// Token: 0x06005484 RID: 21636 RVA: 0x001BEC93 File Offset: 0x001BCE93
		bool IEqualityComparer.Equals(object x, object y)
		{
			return object.ReferenceEquals(x, y);
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x001BEC9C File Offset: 0x001BCE9C
		int IEqualityComparer.GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}
	}
}
