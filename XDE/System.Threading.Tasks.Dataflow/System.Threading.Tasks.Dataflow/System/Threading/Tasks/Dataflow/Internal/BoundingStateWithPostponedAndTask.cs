using System;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000076 RID: 118
	internal class BoundingStateWithPostponedAndTask<TInput> : BoundingStateWithPostponed<TInput>
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x0000D7AB File Offset: 0x0000B9AB
		internal BoundingStateWithPostponedAndTask(int boundedCapacity) : base(boundedCapacity)
		{
		}

		// Token: 0x0400017D RID: 381
		internal Task TaskForInputProcessing;
	}
}
