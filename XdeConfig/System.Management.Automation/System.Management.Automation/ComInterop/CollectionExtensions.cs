using System;
using System.Collections.Generic;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A51 RID: 2641
	internal static class CollectionExtensions
	{
		// Token: 0x060069A1 RID: 27041 RVA: 0x00214128 File Offset: 0x00212328
		internal static T[] RemoveFirst<T>(this T[] array)
		{
			T[] array2 = new T[array.Length - 1];
			Array.Copy(array, 1, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x00214150 File Offset: 0x00212350
		internal static T[] AddFirst<T>(this IList<T> list, T item)
		{
			T[] array = new T[list.Count + 1];
			array[0] = item;
			list.CopyTo(array, 1);
			return array;
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x0021417C File Offset: 0x0021237C
		internal static T[] ToArray<T>(this IList<T> list)
		{
			T[] array = new T[list.Count];
			list.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x002141A0 File Offset: 0x002123A0
		internal static T[] AddLast<T>(this IList<T> list, T item)
		{
			T[] array = new T[list.Count + 1];
			list.CopyTo(array, 0);
			array[list.Count] = item;
			return array;
		}
	}
}
