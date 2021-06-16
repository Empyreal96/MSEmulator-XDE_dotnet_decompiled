using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000074 RID: 116
	[DebuggerDisplay("BoundedCapacity={BoundedCapacity}}")]
	internal class BoundingState
	{
		// Token: 0x060003CE RID: 974 RVA: 0x0000D76B File Offset: 0x0000B96B
		internal BoundingState(int boundedCapacity)
		{
			this.BoundedCapacity = boundedCapacity;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000D77A File Offset: 0x0000B97A
		internal bool CountIsLessThanBound
		{
			get
			{
				return this.CurrentCount < this.BoundedCapacity;
			}
		}

		// Token: 0x04000179 RID: 377
		internal readonly int BoundedCapacity;

		// Token: 0x0400017A RID: 378
		internal int CurrentCount;
	}
}
