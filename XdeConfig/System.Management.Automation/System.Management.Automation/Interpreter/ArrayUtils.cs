using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200074D RID: 1869
	internal static class ArrayUtils
	{
		// Token: 0x06004AD4 RID: 19156 RVA: 0x00188340 File Offset: 0x00186540
		internal static T[] AddLast<T>(this IList<T> list, T item)
		{
			T[] array = new T[list.Count + 1];
			list.CopyTo(array, 0);
			array[list.Count] = item;
			return array;
		}
	}
}
