using System;
using System.Diagnostics;
using System.Linq;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000069 RID: 105
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	internal sealed class JoinBlockTargetSharedResources
	{
		// Token: 0x06000386 RID: 902 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		internal JoinBlockTargetSharedResources(IDataflowBlock ownerJoin, JoinBlockTargetBase[] targets, Action joinFilledAction, Action<Exception> exceptionAction, GroupingDataflowBlockOptions dataflowBlockOptions)
		{
			this._ownerJoin = ownerJoin;
			this._targets = targets;
			this._joinFilledAction = joinFilledAction;
			this._exceptionAction = exceptionAction;
			this._dataflowBlockOptions = dataflowBlockOptions;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				this._boundingState = new BoundingState(dataflowBlockOptions.BoundedCapacity);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0000C934 File Offset: 0x0000AB34
		internal object IncomingLock
		{
			get
			{
				return this._targets;
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000C93C File Offset: 0x0000AB3C
		internal void CompleteEachTarget()
		{
			foreach (JoinBlockTargetBase joinBlockTargetBase in this._targets)
			{
				joinBlockTargetBase.CompleteCore(null, true, false);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000C96C File Offset: 0x0000AB6C
		internal bool AllTargetsHaveAtLeastOneMessage
		{
			get
			{
				foreach (JoinBlockTargetBase joinBlockTargetBase in this._targets)
				{
					if (!joinBlockTargetBase.HasAtLeastOneMessageAvailable)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
		private bool TargetsHaveAtLeastOneMessageQueuedOrPostponed
		{
			get
			{
				if (this._boundingState == null)
				{
					foreach (JoinBlockTargetBase joinBlockTargetBase in this._targets)
					{
						if (!joinBlockTargetBase.HasAtLeastOneMessageAvailable && (this._decliningPermanently || joinBlockTargetBase.IsDecliningPermanently || !joinBlockTargetBase.HasAtLeastOnePostponedMessage))
						{
							return false;
						}
					}
					return true;
				}
				bool countIsLessThanBound = this._boundingState.CountIsLessThanBound;
				bool flag = true;
				bool flag2 = false;
				foreach (JoinBlockTargetBase joinBlockTargetBase2 in this._targets)
				{
					bool flag3 = !this._decliningPermanently && !joinBlockTargetBase2.IsDecliningPermanently && joinBlockTargetBase2.HasAtLeastOnePostponedMessage;
					if (this._dataflowBlockOptions.Greedy && flag3 && (countIsLessThanBound || !joinBlockTargetBase2.HasTheHighestNumberOfMessagesAvailable))
					{
						return true;
					}
					bool hasAtLeastOneMessageAvailable = joinBlockTargetBase2.HasAtLeastOneMessageAvailable;
					flag &= (hasAtLeastOneMessageAvailable || flag3);
					if (hasAtLeastOneMessageAvailable)
					{
						flag2 = true;
					}
				}
				return flag && (flag2 || countIsLessThanBound);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000CA88 File Offset: 0x0000AC88
		private bool RetrievePostponedItemsNonGreedy()
		{
			object incomingLock = this.IncomingLock;
			lock (incomingLock)
			{
				if (!this.TargetsHaveAtLeastOneMessageQueuedOrPostponed)
				{
					return false;
				}
			}
			bool flag2 = true;
			foreach (JoinBlockTargetBase joinBlockTargetBase in this._targets)
			{
				if (!joinBlockTargetBase.ReserveOneMessage())
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				foreach (JoinBlockTargetBase joinBlockTargetBase2 in this._targets)
				{
					if (!joinBlockTargetBase2.ConsumeReservedMessage())
					{
						flag2 = false;
						break;
					}
				}
			}
			if (!flag2)
			{
				foreach (JoinBlockTargetBase joinBlockTargetBase3 in this._targets)
				{
					joinBlockTargetBase3.ReleaseReservedMessage();
				}
			}
			return flag2;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000CB64 File Offset: 0x0000AD64
		private bool RetrievePostponedItemsGreedyBounded()
		{
			bool flag = false;
			foreach (JoinBlockTargetBase joinBlockTargetBase in this._targets)
			{
				flag |= joinBlockTargetBase.ConsumeOnePostponedMessage();
			}
			return flag;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000CB98 File Offset: 0x0000AD98
		private bool CanceledOrFaulted
		{
			get
			{
				return this._dataflowBlockOptions.CancellationToken.IsCancellationRequested || this._hasExceptions;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000CBC2 File Offset: 0x0000ADC2
		internal bool JoinNeedsProcessing
		{
			get
			{
				return this._taskForInputProcessing == null && !this.CanceledOrFaulted && this.TargetsHaveAtLeastOneMessageQueuedOrPostponed;
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000CBDC File Offset: 0x0000ADDC
		internal void ProcessAsyncIfNecessary(bool isReplacementReplica = false)
		{
			if (this.JoinNeedsProcessing)
			{
				this.ProcessAsyncIfNecessary_Slow(isReplacementReplica);
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000CBF0 File Offset: 0x0000ADF0
		private void ProcessAsyncIfNecessary_Slow(bool isReplacementReplica)
		{
			this._taskForInputProcessing = new Task(delegate(object thisSharedResources)
			{
				((JoinBlockTargetSharedResources)thisSharedResources).ProcessMessagesLoopCore();
			}, this, Common.GetCreationOptionsForTask(isReplacementReplica));
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.TaskLaunchedForMessageHandling(this._ownerJoin, this._taskForInputProcessing, DataflowEtwProvider.TaskLaunchedReason.ProcessingInputMessages, this._targets.Max((JoinBlockTargetBase t) => t.NumberOfMessagesAvailableOrPostponed));
			}
			Exception ex = Common.StartTaskSafe(this._taskForInputProcessing, this._dataflowBlockOptions.TaskScheduler);
			if (ex != null)
			{
				this._exceptionAction(ex);
				this._taskForInputProcessing = null;
				this.CompleteBlockIfPossible();
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000CCAC File Offset: 0x0000AEAC
		internal void CompleteBlockIfPossible()
		{
			if (!this._completionReserved)
			{
				bool flag = this._decliningPermanently && !this.AllTargetsHaveAtLeastOneMessage;
				if (!flag)
				{
					foreach (JoinBlockTargetBase joinBlockTargetBase in this._targets)
					{
						if (joinBlockTargetBase.IsDecliningPermanently && !joinBlockTargetBase.HasAtLeastOneMessageAvailable)
						{
							flag = true;
							break;
						}
					}
				}
				bool flag2 = this._taskForInputProcessing == null && (flag || this.CanceledOrFaulted);
				if (flag2)
				{
					this._completionReserved = true;
					this._decliningPermanently = true;
					Task.Factory.StartNew(delegate(object state)
					{
						JoinBlockTargetSharedResources joinBlockTargetSharedResources = (JoinBlockTargetSharedResources)state;
						foreach (JoinBlockTargetBase joinBlockTargetBase2 in joinBlockTargetSharedResources._targets)
						{
							joinBlockTargetBase2.CompleteOncePossible();
						}
					}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
				}
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		private void ProcessMessagesLoopCore()
		{
			try
			{
				int num = 0;
				int actualMaxMessagesPerTask = this._dataflowBlockOptions.ActualMaxMessagesPerTask;
				bool flag;
				do
				{
					flag = ((!this._dataflowBlockOptions.Greedy) ? this.RetrievePostponedItemsNonGreedy() : this.RetrievePostponedItemsGreedyBounded());
					if (flag)
					{
						object incomingLock = this.IncomingLock;
						lock (incomingLock)
						{
							if (this.AllTargetsHaveAtLeastOneMessage)
							{
								this._joinFilledAction();
								this._joinsCreated += 1L;
								if (!this._dataflowBlockOptions.Greedy && this._boundingState != null)
								{
									this._boundingState.CurrentCount++;
								}
							}
						}
					}
					num++;
				}
				while (flag && num < actualMaxMessagesPerTask);
			}
			catch (Exception exception)
			{
				this._targets[0].CompleteCore(exception, true, true);
			}
			finally
			{
				object incomingLock2 = this.IncomingLock;
				lock (incomingLock2)
				{
					this._taskForInputProcessing = null;
					this.ProcessAsyncIfNecessary(true);
					this.CompleteBlockIfPossible();
				}
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
		internal void OnItemsRemoved(int numItemsRemoved)
		{
			if (this._boundingState != null)
			{
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					this._boundingState.CurrentCount -= numItemsRemoved;
					this.ProcessAsyncIfNecessary(false);
					this.CompleteBlockIfPossible();
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000CF14 File Offset: 0x0000B114
		private object DebuggerDisplayContent
		{
			get
			{
				IDebuggerDisplay debuggerDisplay = this._ownerJoin as IDebuggerDisplay;
				return string.Format("Block=\"{0}\"", (debuggerDisplay != null) ? debuggerDisplay.Content : this._ownerJoin);
			}
		}

		// Token: 0x04000155 RID: 341
		internal readonly IDataflowBlock _ownerJoin;

		// Token: 0x04000156 RID: 342
		internal readonly JoinBlockTargetBase[] _targets;

		// Token: 0x04000157 RID: 343
		internal readonly Action<Exception> _exceptionAction;

		// Token: 0x04000158 RID: 344
		internal readonly Action _joinFilledAction;

		// Token: 0x04000159 RID: 345
		internal readonly GroupingDataflowBlockOptions _dataflowBlockOptions;

		// Token: 0x0400015A RID: 346
		internal readonly BoundingState _boundingState;

		// Token: 0x0400015B RID: 347
		internal bool _decliningPermanently;

		// Token: 0x0400015C RID: 348
		internal Task _taskForInputProcessing;

		// Token: 0x0400015D RID: 349
		internal bool _hasExceptions;

		// Token: 0x0400015E RID: 350
		internal long _joinsCreated;

		// Token: 0x0400015F RID: 351
		private bool _completionReserved;
	}
}
