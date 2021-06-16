using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000075 RID: 117
	[DebuggerDisplay("BoundedCapacity={BoundedCapacity}, PostponedMessages={PostponedMessagesCountForDebugger}")]
	internal class BoundingStateWithPostponed<TInput> : BoundingState
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x0000D78A File Offset: 0x0000B98A
		internal BoundingStateWithPostponed(int boundedCapacity) : base(boundedCapacity)
		{
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000D79E File Offset: 0x0000B99E
		private int PostponedMessagesCountForDebugger
		{
			get
			{
				return this.PostponedMessages.Count;
			}
		}

		// Token: 0x0400017B RID: 379
		internal readonly QueuedMap<ISourceBlock<TInput>, DataflowMessageHeader> PostponedMessages = new QueuedMap<ISourceBlock<TInput>, DataflowMessageHeader>();

		// Token: 0x0400017C RID: 380
		internal int OutstandingTransfers;
	}
}
