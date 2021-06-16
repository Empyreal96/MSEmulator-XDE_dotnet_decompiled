using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000065 RID: 101
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(JoinBlockTarget<>.DebugView))]
	internal sealed class JoinBlockTarget<T> : JoinBlockTargetBase, ITargetBlock<!0>, IDataflowBlock, IDebuggerDisplay
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000BEBC File Offset: 0x0000A0BC
		internal JoinBlockTarget(JoinBlockTargetSharedResources sharedResources)
		{
			GroupingDataflowBlockOptions dataflowBlockOptions = sharedResources._dataflowBlockOptions;
			this._sharedResources = sharedResources;
			if (!dataflowBlockOptions.Greedy || dataflowBlockOptions.BoundedCapacity > 0)
			{
				this._nonGreedy = new JoinBlockTarget<T>.NonGreedyState();
			}
			if (dataflowBlockOptions.Greedy)
			{
				this._messages = new Queue<T>();
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000BF18 File Offset: 0x0000A118
		internal T GetOneMessage()
		{
			if (this._sharedResources._dataflowBlockOptions.Greedy)
			{
				return this._messages.Dequeue();
			}
			T value = this._nonGreedy.ConsumedMessage.Value;
			this._nonGreedy.ConsumedMessage = new KeyValuePair<bool, T>(false, default(T));
			return value;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000BF6F File Offset: 0x0000A16F
		internal override bool IsDecliningPermanently
		{
			get
			{
				return this._decliningPermanently;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000BF77 File Offset: 0x0000A177
		internal override bool HasAtLeastOneMessageAvailable
		{
			get
			{
				if (this._sharedResources._dataflowBlockOptions.Greedy)
				{
					return this._messages.Count > 0;
				}
				return this._nonGreedy.ConsumedMessage.Key;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000BFAA File Offset: 0x0000A1AA
		internal override bool HasAtLeastOnePostponedMessage
		{
			get
			{
				return this._nonGreedy != null && this._nonGreedy.PostponedMessages.Count > 0;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000BFC9 File Offset: 0x0000A1C9
		internal override int NumberOfMessagesAvailableOrPostponed
		{
			get
			{
				if (this._sharedResources._dataflowBlockOptions.Greedy)
				{
					return this._messages.Count;
				}
				return this._nonGreedy.PostponedMessages.Count;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000BFFC File Offset: 0x0000A1FC
		internal override bool HasTheHighestNumberOfMessagesAvailable
		{
			get
			{
				int count = this._messages.Count;
				foreach (JoinBlockTargetBase joinBlockTargetBase in this._sharedResources._targets)
				{
					if (joinBlockTargetBase != this && joinBlockTargetBase.NumberOfMessagesAvailableOrPostponed > count)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C044 File Offset: 0x0000A244
		internal override bool ReserveOneMessage()
		{
			object incomingLock = this._sharedResources.IncomingLock;
			KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> reservedMessage;
			lock (incomingLock)
			{
				if (!this._nonGreedy.PostponedMessages.TryPop(out reservedMessage))
				{
					return false;
				}
			}
			while (!reservedMessage.Key.ReserveMessage(reservedMessage.Value, this))
			{
				object incomingLock2 = this._sharedResources.IncomingLock;
				bool result;
				lock (incomingLock2)
				{
					if (this._nonGreedy.PostponedMessages.TryPop(out reservedMessage))
					{
						continue;
					}
					result = false;
				}
				return result;
			}
			this._nonGreedy.ReservedMessage = reservedMessage;
			return true;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000C10C File Offset: 0x0000A30C
		internal override bool ConsumeReservedMessage()
		{
			bool flag;
			T value = this._nonGreedy.ReservedMessage.Key.ConsumeMessage(this._nonGreedy.ReservedMessage.Value, this, out flag);
			this._nonGreedy.ReservedMessage = default(KeyValuePair<ISourceBlock<T>, DataflowMessageHeader>);
			if (!flag)
			{
				this._sharedResources._exceptionAction(new InvalidOperationException(SR.InvalidOperation_FailedToConsumeReservedMessage));
				this.CompleteOncePossible();
				return false;
			}
			object incomingLock = this._sharedResources.IncomingLock;
			lock (incomingLock)
			{
				this._nonGreedy.ConsumedMessage = new KeyValuePair<bool, T>(true, value);
				this.CompleteIfLastJoinIsFeasible();
			}
			return true;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		internal override bool ConsumeOnePostponedMessage()
		{
			bool hasTheHighestNumberOfMessagesAvailable;
			bool flag3;
			T item;
			do
			{
				object incomingLock = this._sharedResources.IncomingLock;
				KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair;
				lock (incomingLock)
				{
					hasTheHighestNumberOfMessagesAvailable = this.HasTheHighestNumberOfMessagesAvailable;
					bool flag2 = this._sharedResources._boundingState.CountIsLessThanBound || !hasTheHighestNumberOfMessagesAvailable;
					if (this._decliningPermanently || this._sharedResources._decliningPermanently || !flag2 || !this._nonGreedy.PostponedMessages.TryPop(out keyValuePair))
					{
						return false;
					}
				}
				item = keyValuePair.Key.ConsumeMessage(keyValuePair.Value, this, out flag3);
			}
			while (!flag3);
			object incomingLock2 = this._sharedResources.IncomingLock;
			bool result;
			lock (incomingLock2)
			{
				if (hasTheHighestNumberOfMessagesAvailable)
				{
					this._sharedResources._boundingState.CurrentCount++;
				}
				this._messages.Enqueue(item);
				this.CompleteIfLastJoinIsFeasible();
				result = true;
			}
			return result;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		private void CompleteIfLastJoinIsFeasible()
		{
			int num = this._sharedResources._dataflowBlockOptions.Greedy ? this._messages.Count : (this._nonGreedy.ConsumedMessage.Key ? 1 : 0);
			if (this._sharedResources._joinsCreated + (long)num >= this._sharedResources._dataflowBlockOptions.ActualMaxNumberOfGroups)
			{
				this._decliningPermanently = true;
				bool flag = true;
				foreach (JoinBlockTargetBase joinBlockTargetBase in this._sharedResources._targets)
				{
					if (!joinBlockTargetBase.IsDecliningPermanently)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this._sharedResources._decliningPermanently = true;
				}
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C38C File Offset: 0x0000A58C
		internal override void ReleaseReservedMessage()
		{
			if (this._nonGreedy != null && this._nonGreedy.ReservedMessage.Key != null)
			{
				try
				{
					this._nonGreedy.ReservedMessage.Key.ReleaseReservation(this._nonGreedy.ReservedMessage.Value, this);
				}
				finally
				{
					this.ClearReservation();
				}
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		internal override void ClearReservation()
		{
			this._nonGreedy.ReservedMessage = default(KeyValuePair<ISourceBlock<T>, DataflowMessageHeader>);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000C408 File Offset: 0x0000A608
		internal override void CompleteOncePossible()
		{
			object incomingLock = this._sharedResources.IncomingLock;
			lock (incomingLock)
			{
				this._decliningPermanently = true;
				if (this._messages != null)
				{
					this._messages.Clear();
				}
			}
			List<Exception> list = null;
			if (this._nonGreedy != null)
			{
				Common.ReleaseAllPostponedMessages<T>(this, this._nonGreedy.PostponedMessages, ref list);
			}
			if (list != null)
			{
				foreach (Exception obj in list)
				{
					this._sharedResources._exceptionAction(obj);
				}
			}
			this._completionTask.TrySetResult(default(VoidResult));
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
		{
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (source == null && consumeToAccept)
			{
				throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
			}
			object incomingLock = this._sharedResources.IncomingLock;
			DataflowMessageStatus result;
			lock (incomingLock)
			{
				if (this._decliningPermanently || this._sharedResources._decliningPermanently)
				{
					this._sharedResources.CompleteBlockIfPossible();
					result = DataflowMessageStatus.DecliningPermanently;
				}
				else if (this._sharedResources._dataflowBlockOptions.Greedy && (this._sharedResources._boundingState == null || ((this._sharedResources._boundingState.CountIsLessThanBound || !this.HasTheHighestNumberOfMessagesAvailable) && this._nonGreedy.PostponedMessages.Count == 0 && this._sharedResources._taskForInputProcessing == null)))
				{
					if (consumeToAccept)
					{
						bool flag2;
						messageValue = source.ConsumeMessage(messageHeader, this, out flag2);
						if (!flag2)
						{
							return DataflowMessageStatus.NotAvailable;
						}
					}
					if (this._sharedResources._boundingState != null && this.HasTheHighestNumberOfMessagesAvailable)
					{
						this._sharedResources._boundingState.CurrentCount++;
					}
					this._messages.Enqueue(messageValue);
					this.CompleteIfLastJoinIsFeasible();
					if (this._sharedResources.AllTargetsHaveAtLeastOneMessage)
					{
						this._sharedResources._joinFilledAction();
						this._sharedResources._joinsCreated += 1L;
					}
					this._sharedResources.CompleteBlockIfPossible();
					result = DataflowMessageStatus.Accepted;
				}
				else if (source != null)
				{
					this._nonGreedy.PostponedMessages.Push(source, messageHeader);
					this._sharedResources.ProcessAsyncIfNecessary(false);
					result = DataflowMessageStatus.Postponed;
				}
				else
				{
					result = DataflowMessageStatus.Declined;
				}
			}
			return result;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		internal override void CompleteCore(Exception exception, bool dropPendingMessages, bool releaseReservedMessages)
		{
			bool greedy = this._sharedResources._dataflowBlockOptions.Greedy;
			object incomingLock = this._sharedResources.IncomingLock;
			lock (incomingLock)
			{
				if (exception != null && ((!this._decliningPermanently && !this._sharedResources._decliningPermanently) || releaseReservedMessages))
				{
					this._sharedResources._exceptionAction(exception);
				}
				if (dropPendingMessages && greedy)
				{
					this._messages.Clear();
				}
			}
			if (releaseReservedMessages && !greedy)
			{
				foreach (JoinBlockTargetBase joinBlockTargetBase in this._sharedResources._targets)
				{
					try
					{
						joinBlockTargetBase.ReleaseReservedMessage();
					}
					catch (Exception obj)
					{
						this._sharedResources._exceptionAction(obj);
					}
				}
			}
			object incomingLock2 = this._sharedResources.IncomingLock;
			lock (incomingLock2)
			{
				this._decliningPermanently = true;
				this._sharedResources.CompleteBlockIfPossible();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this.CompleteCore(exception, true, false);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600036F RID: 879 RVA: 0x000033CB File Offset: 0x000015CB
		public Task Completion
		{
			get
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000C7F1 File Offset: 0x0000A9F1
		internal Task CompletionTaskInternal
		{
			get
			{
				return this._completionTask.Task;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000C7FE File Offset: 0x0000A9FE
		private int InputCountForDebugger
		{
			get
			{
				if (this._messages != null)
				{
					return this._messages.Count;
				}
				if (!this._nonGreedy.ConsumedMessage.Key)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000C82C File Offset: 0x0000AA2C
		private object DebuggerDisplayContent
		{
			get
			{
				IDebuggerDisplay debuggerDisplay = this._sharedResources._ownerJoin as IDebuggerDisplay;
				return string.Format("{0} InputCount={1}, Join=\"{2}\"", Common.GetNameForDebugger(this, null), this.InputCountForDebugger, (debuggerDisplay != null) ? debuggerDisplay.Content : this._sharedResources._ownerJoin);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000C87C File Offset: 0x0000AA7C
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x0400014C RID: 332
		private readonly JoinBlockTargetSharedResources _sharedResources;

		// Token: 0x0400014D RID: 333
		private readonly TaskCompletionSource<VoidResult> _completionTask = new TaskCompletionSource<VoidResult>();

		// Token: 0x0400014E RID: 334
		private readonly Queue<T> _messages;

		// Token: 0x0400014F RID: 335
		private readonly JoinBlockTarget<T>.NonGreedyState _nonGreedy;

		// Token: 0x04000150 RID: 336
		private bool _decliningPermanently;

		// Token: 0x02000066 RID: 102
		private sealed class NonGreedyState
		{
			// Token: 0x04000151 RID: 337
			internal readonly QueuedMap<ISourceBlock<T>, DataflowMessageHeader> PostponedMessages = new QueuedMap<ISourceBlock<T>, DataflowMessageHeader>();

			// Token: 0x04000152 RID: 338
			internal KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> ReservedMessage;

			// Token: 0x04000153 RID: 339
			internal KeyValuePair<bool, T> ConsumedMessage;
		}

		// Token: 0x02000067 RID: 103
		private sealed class DebugView
		{
			// Token: 0x06000375 RID: 885 RVA: 0x0000C897 File Offset: 0x0000AA97
			public DebugView(JoinBlockTarget<T> joinBlockTarget)
			{
				this._joinBlockTarget = joinBlockTarget;
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x06000376 RID: 886 RVA: 0x0000C8A6 File Offset: 0x0000AAA6
			public IEnumerable<T> InputQueue
			{
				get
				{
					return this._joinBlockTarget._messages;
				}
			}

			// Token: 0x1700013B RID: 315
			// (get) Token: 0x06000377 RID: 887 RVA: 0x0000C8B3 File Offset: 0x0000AAB3
			public bool IsDecliningPermanently
			{
				get
				{
					return this._joinBlockTarget._decliningPermanently || this._joinBlockTarget._sharedResources._decliningPermanently;
				}
			}

			// Token: 0x04000154 RID: 340
			private readonly JoinBlockTarget<T> _joinBlockTarget;
		}
	}
}
