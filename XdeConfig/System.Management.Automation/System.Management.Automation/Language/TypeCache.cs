using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005E6 RID: 1510
	internal class TypeCache
	{
		// Token: 0x060040C0 RID: 16576 RVA: 0x00157838 File Offset: 0x00155A38
		internal static Type Lookup(ITypeName typeName, TypeResolutionState typeResolutionState)
		{
			Type result;
			TypeCache._cache.TryGetValue(Tuple.Create<ITypeName, TypeResolutionState>(typeName, typeResolutionState), out result);
			return result;
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x0015785A File Offset: 0x00155A5A
		internal static void Add(ITypeName typeName, TypeResolutionState typeResolutionState, Type type)
		{
			TypeCache._cache.GetOrAdd(Tuple.Create<ITypeName, TypeResolutionState>(typeName, typeResolutionState), type);
		}

		// Token: 0x04002089 RID: 8329
		private static readonly ConcurrentDictionary<Tuple<ITypeName, TypeResolutionState>, Type> _cache = new ConcurrentDictionary<Tuple<ITypeName, TypeResolutionState>, Type>(new TypeCache.KeyComparer());

		// Token: 0x020005E7 RID: 1511
		private class KeyComparer : IEqualityComparer<Tuple<ITypeName, TypeResolutionState>>
		{
			// Token: 0x060040C4 RID: 16580 RVA: 0x00157888 File Offset: 0x00155A88
			public bool Equals(Tuple<ITypeName, TypeResolutionState> x, Tuple<ITypeName, TypeResolutionState> y)
			{
				return x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2);
			}

			// Token: 0x060040C5 RID: 16581 RVA: 0x001578B0 File Offset: 0x00155AB0
			public int GetHashCode(Tuple<ITypeName, TypeResolutionState> obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
