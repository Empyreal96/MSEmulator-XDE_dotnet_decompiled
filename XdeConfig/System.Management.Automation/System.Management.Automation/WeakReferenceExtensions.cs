using System;

namespace System.Management.Automation
{
	// Token: 0x02000878 RID: 2168
	internal static class WeakReferenceExtensions
	{
		// Token: 0x06005304 RID: 21252 RVA: 0x001B9E2C File Offset: 0x001B802C
		internal static bool TryGetTarget<T>(this WeakReference weakReference, out T target) where T : class
		{
			object target2 = weakReference.Target;
			target = (target2 as T);
			return target != null;
		}
	}
}
