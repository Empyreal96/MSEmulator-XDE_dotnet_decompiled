using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200007C RID: 124
	internal sealed class EnumerableDebugView<TKey, TValue>
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x0000D97D File Offset: 0x0000BB7D
		public EnumerableDebugView(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
		{
			this._enumerable = enumerable;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000D98C File Offset: 0x0000BB8C
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<TKey, TValue>[] Items
		{
			get
			{
				return this._enumerable.ToArray<KeyValuePair<TKey, TValue>>();
			}
		}

		// Token: 0x04000188 RID: 392
		private readonly IEnumerable<KeyValuePair<TKey, TValue>> _enumerable;
	}
}
