using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200008C RID: 140
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	internal sealed class TargetCore<TInput>
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
		private object IncomingLock
		{
			get
			{
				return this._messages;
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		internal TargetCore(ITargetBlock<TInput> owningTarget, Action<KeyValuePair<TInput, long>> callAction, IReorderingBuffer reorderingBuffer, ExecutionDataflowBlockOptions dataflowBlockOptions, TargetCoreOptions targetCoreOptions)
		{
			this._owningTarget = owningTarget;
			this._callAction = callAction;
			this._reorderingBuffer = reorderingBuffer;
			this._dataflowBlockOptions = dataflowBlockOptions;
			this._targetCoreOptions = targetCoreOptions;
			IProducerConsumerQueue<KeyValuePair<TInput, long>> messages;
			if (dataflowBlockOptions.MaxDegreeOfParallelism != 1)
			{
				IProducerConsumerQueue<KeyValuePair<TInput, long>> producerConsumerQueue = new MultiProducerMultiConsumerQueue<KeyValuePair<TInput, long>>();
				messages = producerConsumerQueue;
			}
			else
			{
				IProducerConsumerQueue<KeyValuePair<TInput, long>> producerConsumerQueue = new SingleProducerSingleConsumerQueue<KeyValuePair<TInput, long>>();
				messages = producerConsumerQueue;
			}
			this._messages = messages;
			if (this._dataflowBlockOptions.BoundedCapacity != -1)
			{
				this._boundingState = new BoundingStateWithPostponed<TInput>(this._dataflowBlockOptions.BoundedCapacity);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000F754 File Offset: 0x0000D954
		internal void Complete(Exception exception, bool dropPendingMessages, bool storeExceptionEvenIfAlreadyCompleting = false, bool unwrapInnerExceptions = false, bool revertProcessingState = false)
		{
			object incomingLock = this.IncomingLock;
			lock (incomingLock)
			{
				if (exception != null && (!this._decliningPermanently || storeExceptionEvenIfAlreadyCompleting))
				{
					Common.AddException(ref this._exceptions, exception, unwrapInnerExceptions);
				}
				if (dropPendingMessages)
				{
					KeyValuePair<TInput, long> keyValuePair;
					while (this._messages.TryDequeue(out keyValuePair))
					{
					}
				}
				if (revertProcessingState)
				{
					this._numberOfOutstandingOperations--;
					if (this.UsesAsyncCompletion)
					{
						this._numberOfOutstandingServiceTasks--;
					}
				}
				this._decliningPermanently = true;
				this.CompleteBlockIfPossible();
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000F7F4 File Offset: 0x0000D9F4
		internal DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
		{
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (source == null && consumeToAccept)
			{
				throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
			}
			object incomingLock = this.IncomingLock;
			DataflowMessageStatus result;
			lock (incomingLock)
			{
				if (this._decliningPermanently)
				{
					this.CompleteBlockIfPossible();
					result = DataflowMessageStatus.DecliningPermanently;
				}
				else if (this._boundingState == null || (this._boundingState.OutstandingTransfers == 0 && this._boundingState.CountIsLessThanBound && this._boundingState.PostponedMessages.Count == 0))
				{
					if (consumeToAccept)
					{
						bool flag2;
						messageValue = source.ConsumeMessage(messageHeader, this._owningTarget, out flag2);
						if (!flag2)
						{
							return DataflowMessageStatus.NotAvailable;
						}
					}
					long value = this._nextAvailableInputMessageId.Value;
					this._nextAvailableInputMessageId.Value = value + 1L;
					long value2 = value;
					if (this._boundingState != null)
					{
						this._boundingState.CurrentCount++;
					}
					this._messages.Enqueue(new KeyValuePair<TInput, long>(messageValue, value2));
					this.ProcessAsyncIfNecessary(false);
					result = DataflowMessageStatus.Accepted;
				}
				else if (source != null)
				{
					this._boundingState.PostponedMessages.Push(source, messageHeader);
					this.ProcessAsyncIfNecessary(false);
					result = DataflowMessageStatus.Postponed;
				}
				else
				{
					result = DataflowMessageStatus.Declined;
				}
			}
			return result;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000F93C File Offset: 0x0000DB3C
		internal Task Completion
		{
			get
			{
				return this._completionSource.Task;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0000F949 File Offset: 0x0000DB49
		internal int InputCount
		{
			get
			{
				return this._messages.GetCountSafe(this.IncomingLock);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000F95C File Offset: 0x0000DB5C
		internal void SignalOneAsyncMessageCompleted()
		{
			this.SignalOneAsyncMessageCompleted(0);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000F968 File Offset: 0x0000DB68
		internal void SignalOneAsyncMessageCompleted(int boundingCountChange)
		{
			object incomingLock = this.IncomingLock;
			lock (incomingLock)
			{
				if (this._numberOfOutstandingOperations > 0)
				{
					this._numberOfOutstandingOperations--;
				}
				if (this._boundingState != null && boundingCountChange != 0)
				{
					this._boundingState.CurrentCount += boundingCountChange;
				}
				this.ProcessAsyncIfNecessary(true);
				this.CompleteBlockIfPossible();
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
		private bool UsesAsyncCompletion
		{
			get
			{
				return (this._targetCoreOptions & TargetCoreOptions.UsesAsyncCompletion) > TargetCoreOptions.None;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000F9F1 File Offset: 0x0000DBF1
		private bool HasRoomForMoreOperations
		{
			get
			{
				return this._numberOfOutstandingOperations - this._numberOfOutstandingServiceTasks < this._dataflowBlockOptions.ActualMaxDegreeOfParallelism;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000FA0D File Offset: 0x0000DC0D
		private bool HasRoomForMoreServiceTasks
		{
			get
			{
				if (!this.UsesAsyncCompletion)
				{
					return this.HasRoomForMoreOperations;
				}
				return this.HasRoomForMoreOperations && this._numberOfOutstandingServiceTasks < this._dataflowBlockOptions.ActualMaxDegreeOfParallelism;
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000FA3B File Offset: 0x0000DC3B
		private void ProcessAsyncIfNecessary(bool repeat = false)
		{
			if (this.HasRoomForMoreServiceTasks)
			{
				this.ProcessAsyncIfNecessary_Slow(repeat);
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		private void ProcessAsyncIfNecessary_Slow(bool repeat)
		{
			bool flag = !this._messages.IsEmpty || (!this._decliningPermanently && this._boundingState != null && this._boundingState.CountIsLessThanBound && this._boundingState.PostponedMessages.Count > 0);
			if (flag && !this.CanceledOrFaulted)
			{
				this._numberOfOutstandingOperations++;
				if (this.UsesAsyncCompletion)
				{
					this._numberOfOutstandingServiceTasks++;
				}
				Task task = new Task(delegate(object thisTargetCore)
				{
					((TargetCore<TInput>)thisTargetCore).ProcessMessagesLoopCore();
				}, this, Common.GetCreationOptionsForTask(repeat));
				DataflowEtwProvider log = DataflowEtwProvider.Log;
				if (log.IsEnabled())
				{
					log.TaskLaunchedForMessageHandling(this._owningTarget, task, DataflowEtwProvider.TaskLaunchedReason.ProcessingInputMessages, this._messages.Count + ((this._boundingState != null) ? this._boundingState.PostponedMessages.Count : 0));
				}
				Exception ex = Common.StartTaskSafe(task, this._dataflowBlockOptions.TaskScheduler);
				if (ex != null)
				{
					Task.Factory.StartNew(delegate(object exc)
					{
						this.Complete((Exception)exc, true, true, false, true);
					}, ex, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000FB80 File Offset: 0x0000DD80
		private void ProcessMessagesLoopCore()
		{
			KeyValuePair<TInput, long> obj = default(KeyValuePair<TInput, long>);
			try
			{
				bool usesAsyncCompletion = this.UsesAsyncCompletion;
				bool flag = this._boundingState != null && this._boundingState.BoundedCapacity > 1;
				int num = 0;
				int num2 = 0;
				int actualMaxMessagesPerTask = this._dataflowBlockOptions.ActualMaxMessagesPerTask;
				while (num < actualMaxMessagesPerTask && !this.CanceledOrFaulted)
				{
					KeyValuePair<TInput, long> item;
					if (flag && this.TryConsumePostponedMessage(true, out item))
					{
						object incomingLock = this.IncomingLock;
						lock (incomingLock)
						{
							this._boundingState.OutstandingTransfers--;
							this._messages.Enqueue(item);
							this.ProcessAsyncIfNecessary(false);
						}
					}
					if (usesAsyncCompletion)
					{
						if (!this.TryGetNextMessageForNewAsyncOperation(out obj))
						{
							break;
						}
					}
					else if (!this.TryGetNextAvailableOrPostponedMessage(out obj))
					{
						if (this._dataflowBlockOptions.MaxDegreeOfParallelism != 1 || num2 > 1)
						{
							break;
						}
						if (this._keepAliveBanCounter > 0)
						{
							this._keepAliveBanCounter--;
							break;
						}
						num2 = 0;
						if (!Common.TryKeepAliveUntil<TargetCore<TInput>, KeyValuePair<TInput, long>>(TargetCore<TInput>._keepAlivePredicate, this, out obj))
						{
							this._keepAliveBanCounter = 1000;
							break;
						}
					}
					num++;
					num2++;
					this._callAction(obj);
				}
			}
			catch (Exception ex)
			{
				Common.StoreDataflowMessageValueIntoExceptionData<TInput>(ex, obj.Key, false);
				this.Complete(ex, true, true, false, false);
			}
			finally
			{
				object incomingLock2 = this.IncomingLock;
				lock (incomingLock2)
				{
					this._numberOfOutstandingOperations--;
					if (this.UsesAsyncCompletion)
					{
						this._numberOfOutstandingServiceTasks--;
					}
					this.ProcessAsyncIfNecessary(true);
					this.CompleteBlockIfPossible();
				}
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000FD88 File Offset: 0x0000DF88
		private bool TryGetNextMessageForNewAsyncOperation(out KeyValuePair<TInput, long> messageWithId)
		{
			object incomingLock = this.IncomingLock;
			bool hasRoomForMoreOperations;
			lock (incomingLock)
			{
				hasRoomForMoreOperations = this.HasRoomForMoreOperations;
				if (hasRoomForMoreOperations)
				{
					this._numberOfOutstandingOperations++;
				}
			}
			messageWithId = default(KeyValuePair<TInput, long>);
			if (hasRoomForMoreOperations)
			{
				bool flag2 = false;
				try
				{
					flag2 = this.TryGetNextAvailableOrPostponedMessage(out messageWithId);
				}
				catch
				{
					this.SignalOneAsyncMessageCompleted();
					throw;
				}
				if (!flag2)
				{
					this.SignalOneAsyncMessageCompleted();
				}
				return flag2;
			}
			return false;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000FE14 File Offset: 0x0000E014
		private bool TryGetNextAvailableOrPostponedMessage(out KeyValuePair<TInput, long> messageWithId)
		{
			if (this._messages.TryDequeue(out messageWithId))
			{
				return true;
			}
			if (this._boundingState != null && this.TryConsumePostponedMessage(false, out messageWithId))
			{
				return true;
			}
			messageWithId = default(KeyValuePair<TInput, long>);
			return false;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000FE44 File Offset: 0x0000E044
		private bool TryConsumePostponedMessage(bool forPostponementTransfer, out KeyValuePair<TInput, long> result)
		{
			bool flag = false;
			long num = -1L;
			TInput key;
			for (;;)
			{
				object incomingLock = this.IncomingLock;
				KeyValuePair<ISourceBlock<TInput>, DataflowMessageHeader> keyValuePair;
				lock (incomingLock)
				{
					if (this._decliningPermanently)
					{
						goto IL_11F;
					}
					if (!forPostponementTransfer && this._messages.TryDequeue(out result))
					{
						return true;
					}
					if (!this._boundingState.CountIsLessThanBound || !this._boundingState.PostponedMessages.TryPop(out keyValuePair))
					{
						if (flag)
						{
							flag = false;
							this._boundingState.CurrentCount--;
						}
						goto IL_11F;
					}
					if (!flag)
					{
						flag = true;
						long value = this._nextAvailableInputMessageId.Value;
						this._nextAvailableInputMessageId.Value = value + 1L;
						num = value;
						this._boundingState.CurrentCount++;
						if (forPostponementTransfer)
						{
							this._boundingState.OutstandingTransfers++;
						}
					}
				}
				bool flag3;
				key = keyValuePair.Key.ConsumeMessage(keyValuePair.Value, this._owningTarget, out flag3);
				if (flag3)
				{
					break;
				}
				if (forPostponementTransfer)
				{
					this._boundingState.OutstandingTransfers--;
				}
			}
			result = new KeyValuePair<TInput, long>(key, num);
			return true;
			IL_11F:
			if (this._reorderingBuffer != null && num != -1L)
			{
				this._reorderingBuffer.IgnoreItem(num);
			}
			if (flag)
			{
				this.ChangeBoundingCount(-1);
			}
			result = default(KeyValuePair<TInput, long>);
			return false;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000FFB0 File Offset: 0x0000E1B0
		private bool CanceledOrFaulted
		{
			get
			{
				return this._dataflowBlockOptions.CancellationToken.IsCancellationRequested || Volatile.Read<List<Exception>>(ref this._exceptions) != null;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		private void CompleteBlockIfPossible()
		{
			if ((this._decliningPermanently && this._messages.IsEmpty) || this.CanceledOrFaulted)
			{
				this.CompleteBlockIfPossible_Slow();
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001001C File Offset: 0x0000E21C
		private void CompleteBlockIfPossible_Slow()
		{
			bool flag = this._numberOfOutstandingOperations == 0;
			if (flag && !this._completionReserved)
			{
				this._completionReserved = true;
				this._decliningPermanently = true;
				Task.Factory.StartNew(delegate(object state)
				{
					((TargetCore<TInput>)state).CompleteBlockOncePossible();
				}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00010088 File Offset: 0x0000E288
		private void CompleteBlockOncePossible()
		{
			if (this._boundingState != null)
			{
				Common.ReleaseAllPostponedMessages<TInput>(this._owningTarget, this._boundingState.PostponedMessages, ref this._exceptions);
			}
			IProducerConsumerQueue<KeyValuePair<TInput, long>> messages = this._messages;
			KeyValuePair<TInput, long> keyValuePair;
			while (messages.TryDequeue(out keyValuePair))
			{
			}
			if (Volatile.Read<List<Exception>>(ref this._exceptions) != null)
			{
				this._completionSource.TrySetException(Volatile.Read<List<Exception>>(ref this._exceptions));
			}
			else if (this._dataflowBlockOptions.CancellationToken.IsCancellationRequested)
			{
				this._completionSource.TrySetCanceled();
			}
			else
			{
				this._completionSource.TrySetResult(default(VoidResult));
			}
			DataflowEtwProvider log;
			if ((this._targetCoreOptions & TargetCoreOptions.RepresentsBlockCompletion) != TargetCoreOptions.None && (log = DataflowEtwProvider.Log).IsEnabled())
			{
				log.DataflowBlockCompleted(this._owningTarget);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0001014D File Offset: 0x0000E34D
		internal bool IsBounded
		{
			get
			{
				return this._boundingState != null;
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00010158 File Offset: 0x0000E358
		internal void ChangeBoundingCount(int count)
		{
			if (this._boundingState != null)
			{
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					this._boundingState.CurrentCount += count;
					this.ProcessAsyncIfNecessary(false);
					this.CompleteBlockIfPossible();
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000101BC File Offset: 0x0000E3BC
		private object DebuggerDisplayContent
		{
			get
			{
				IDebuggerDisplay debuggerDisplay = this._owningTarget as IDebuggerDisplay;
				return string.Format("Block=\"{0}\"", (debuggerDisplay != null) ? debuggerDisplay.Content : this._owningTarget);
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000101F0 File Offset: 0x0000E3F0
		internal ExecutionDataflowBlockOptions DataflowBlockOptions
		{
			get
			{
				return this._dataflowBlockOptions;
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000101F8 File Offset: 0x0000E3F8
		internal TargetCore<TInput>.DebuggingInformation GetDebuggingInformation()
		{
			return new TargetCore<TInput>.DebuggingInformation(this);
		}

		// Token: 0x040001BE RID: 446
		private static readonly Common.KeepAlivePredicate<TargetCore<TInput>, KeyValuePair<TInput, long>> _keepAlivePredicate = delegate(TargetCore<TInput> thisTargetCore, out KeyValuePair<TInput, long> messageWithId)
		{
			return thisTargetCore.TryGetNextAvailableOrPostponedMessage(out messageWithId);
		};

		// Token: 0x040001BF RID: 447
		private readonly TaskCompletionSource<VoidResult> _completionSource = new TaskCompletionSource<VoidResult>();

		// Token: 0x040001C0 RID: 448
		private readonly ITargetBlock<TInput> _owningTarget;

		// Token: 0x040001C1 RID: 449
		private readonly IProducerConsumerQueue<KeyValuePair<TInput, long>> _messages;

		// Token: 0x040001C2 RID: 450
		private readonly ExecutionDataflowBlockOptions _dataflowBlockOptions;

		// Token: 0x040001C3 RID: 451
		private readonly Action<KeyValuePair<TInput, long>> _callAction;

		// Token: 0x040001C4 RID: 452
		private readonly TargetCoreOptions _targetCoreOptions;

		// Token: 0x040001C5 RID: 453
		private readonly BoundingStateWithPostponed<TInput> _boundingState;

		// Token: 0x040001C6 RID: 454
		private readonly IReorderingBuffer _reorderingBuffer;

		// Token: 0x040001C7 RID: 455
		private List<Exception> _exceptions;

		// Token: 0x040001C8 RID: 456
		private bool _decliningPermanently;

		// Token: 0x040001C9 RID: 457
		private int _numberOfOutstandingOperations;

		// Token: 0x040001CA RID: 458
		private int _numberOfOutstandingServiceTasks;

		// Token: 0x040001CB RID: 459
		private PaddedInt64 _nextAvailableInputMessageId;

		// Token: 0x040001CC RID: 460
		private bool _completionReserved;

		// Token: 0x040001CD RID: 461
		private int _keepAliveBanCounter;

		// Token: 0x0200008D RID: 141
		internal sealed class DebuggingInformation
		{
			// Token: 0x06000468 RID: 1128 RVA: 0x00010229 File Offset: 0x0000E429
			internal DebuggingInformation(TargetCore<TInput> target)
			{
				this._target = target;
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000469 RID: 1129 RVA: 0x00010238 File Offset: 0x0000E438
			internal int InputCount
			{
				get
				{
					return this._target._messages.Count;
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001024A File Offset: 0x0000E44A
			internal IEnumerable<TInput> InputQueue
			{
				get
				{
					return (from kvp in this._target._messages
					select kvp.Key).ToList<TInput>();
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x0600046B RID: 1131 RVA: 0x00010280 File Offset: 0x0000E480
			internal QueuedMap<ISourceBlock<TInput>, DataflowMessageHeader> PostponedMessages
			{
				get
				{
					if (this._target._boundingState == null)
					{
						return null;
					}
					return this._target._boundingState.PostponedMessages;
				}
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x0600046C RID: 1132 RVA: 0x000102A1 File Offset: 0x0000E4A1
			internal int CurrentDegreeOfParallelism
			{
				get
				{
					return this._target._numberOfOutstandingOperations - this._target._numberOfOutstandingServiceTasks;
				}
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x0600046D RID: 1133 RVA: 0x000102BA File Offset: 0x0000E4BA
			internal ExecutionDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._target._dataflowBlockOptions;
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x0600046E RID: 1134 RVA: 0x000102C7 File Offset: 0x0000E4C7
			internal bool IsDecliningPermanently
			{
				get
				{
					return this._target._decliningPermanently;
				}
			}

			// Token: 0x1700017E RID: 382
			// (get) Token: 0x0600046F RID: 1135 RVA: 0x000102D4 File Offset: 0x0000E4D4
			internal bool IsCompleted
			{
				get
				{
					return this._target.Completion.IsCompleted;
				}
			}

			// Token: 0x040001CE RID: 462
			private readonly TargetCore<TInput> _target;
		}
	}
}
