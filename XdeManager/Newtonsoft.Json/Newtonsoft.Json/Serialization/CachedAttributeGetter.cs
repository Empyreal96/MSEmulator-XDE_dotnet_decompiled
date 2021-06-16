using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006A RID: 106
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x00019AE4 File Offset: 0x00017CE4
		public static T GetAttribute(object type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}

		// Token: 0x0400020A RID: 522
		private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));
	}
}
