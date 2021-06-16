using System;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000063 RID: 99
	internal sealed class BatchedJoinBlockTargetSharedResources
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		internal BatchedJoinBlockTargetSharedResources(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions, Action batchSizeReachedAction, Action allTargetsDecliningAction, Action<Exception> exceptionAction, Action completionAction)
		{
			BatchedJoinBlockTargetSharedResources <>4__this = this;
			this._incomingLock = new object();
			this._batchSize = batchSize;
			this._remainingAliveTargets = 0;
			this._remainingItemsInBatch = batchSize;
			this._allTargetsDecliningPermanentlyAction = delegate()
			{
				allTargetsDecliningAction();
				<>4__this._decliningPermanently = true;
			};
			this._batchSizeReachedAction = delegate()
			{
				batchSizeReachedAction();
				<>4__this._batchesCreated += 1L;
				if (<>4__this._batchesCreated >= dataflowBlockOptions.ActualMaxNumberOfGroups)
				{
					<>4__this._allTargetsDecliningPermanentlyAction();
					return;
				}
				<>4__this._remainingItemsInBatch = <>4__this._batchSize;
			};
			this._exceptionAction = exceptionAction;
			this._completionAction = completionAction;
		}

		// Token: 0x0400013E RID: 318
		internal readonly object _incomingLock;

		// Token: 0x0400013F RID: 319
		internal readonly int _batchSize;

		// Token: 0x04000140 RID: 320
		internal readonly Action _batchSizeReachedAction;

		// Token: 0x04000141 RID: 321
		internal readonly Action _allTargetsDecliningPermanentlyAction;

		// Token: 0x04000142 RID: 322
		internal readonly Action<Exception> _exceptionAction;

		// Token: 0x04000143 RID: 323
		internal readonly Action _completionAction;

		// Token: 0x04000144 RID: 324
		internal int _remainingItemsInBatch;

		// Token: 0x04000145 RID: 325
		internal int _remainingAliveTargets;

		// Token: 0x04000146 RID: 326
		internal bool _decliningPermanently;

		// Token: 0x04000147 RID: 327
		internal long _batchesCreated;
	}
}
