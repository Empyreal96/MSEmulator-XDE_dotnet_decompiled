using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a dataflow block that batches inputs into arrays.</summary>
	/// <typeparam name="T">Specifies the type of data put into batches.</typeparam>
	// Token: 0x02000038 RID: 56
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(BatchBlock<>.DebugView))]
	public sealed class BatchBlock<T> : IPropagatorBlock<T, T[]>, ITargetBlock<T>, IDataflowBlock, ISourceBlock<T[]>, IReceivableSourceBlock<T[]>, IDebuggerDisplay
	{
		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.BatchBlock`1" /> with the specified batch size.</summary>
		/// <param name="batchSize">The number of items to group into a batch.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="batchSize" /> must be positive.</exception>
		// Token: 0x0600014B RID: 331 RVA: 0x00005685 File Offset: 0x00003885
		public BatchBlock(int batchSize) : this(batchSize, GroupingDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.BatchBlock`1" /> with the specified batch size, declining option, and block options.</summary>
		/// <param name="batchSize">The number of items to group into a batch.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.BatchBlock`1" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="batchSize" /> must be positive.-or-The <paramref name="batchSize" /> must be smaller than the value of the <see cref="P:System.Threading.Tasks.Dataflow.DataflowBlockOptions.BoundedCapacity" /> option if a non-default value has been set.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600014C RID: 332 RVA: 0x00005694 File Offset: 0x00003894
		public BatchBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions)
		{
			if (batchSize < 1)
			{
				throw new ArgumentOutOfRangeException("batchSize", SR.ArgumentOutOfRange_GenericPositive);
			}
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			if (dataflowBlockOptions.BoundedCapacity > 0 && dataflowBlockOptions.BoundedCapacity < batchSize)
			{
				throw new ArgumentOutOfRangeException("batchSize", SR.ArgumentOutOfRange_BatchSizeMustBeNoGreaterThanBoundedCapacity);
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<ISourceBlock<T[]>, int> itemsRemovedAction = null;
			Func<ISourceBlock<T[]>, T[], IList<T[]>, int> itemCountingFunc = null;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				itemsRemovedAction = delegate(ISourceBlock<T[]> owningSource, int count)
				{
					((BatchBlock<T>)owningSource)._target.OnItemsRemoved(count);
				};
				itemCountingFunc = ((ISourceBlock<T[]> owningSource, T[] singleOutputItem, IList<T[]> multipleOutputItems) => BatchBlock<T>.BatchBlockTargetCore.CountItems(singleOutputItem, multipleOutputItems));
			}
			this._source = new SourceCore<T[]>(this, dataflowBlockOptions, delegate(ISourceBlock<T[]> owningSource)
			{
				((BatchBlock<T>)owningSource)._target.Complete(null, true, false, false);
			}, itemsRemovedAction, itemCountingFunc);
			this._target = new BatchBlock<T>.BatchBlockTargetCore(this, batchSize, delegate(T[] batch)
			{
				this._source.AddMessage(batch);
			}, dataflowBlockOptions);
			this._target.Completion.ContinueWith(delegate(Task <p0>)
			{
				this._source.Complete();
			}, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
			this._source.Completion.ContinueWith(delegate(Task completed, object state)
			{
				IDataflowBlock dataflowBlock = (BatchBlock<T>)state;
				dataflowBlock.Fault(completed.Exception);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
			Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this._source.Completion, delegate(object state)
			{
				((BatchBlock<T>.BatchBlockTargetCore)state).Complete(null, true, false, false);
			}, this._target);
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x0600014D RID: 333 RVA: 0x0000584D File Offset: 0x00003A4D
		public void Complete()
		{
			this._target.Complete(null, false, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x0600014E RID: 334 RVA: 0x0000585E File Offset: 0x00003A5E
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this._target.Complete(exception, true, false, false);
		}

		/// <summary>Triggers the <see cref="T:System.Threading.Tasks.Dataflow.BatchBlock`1" /> to initiate a batching operation even if the number of currently queued or postponed items is less than the <see cref="P:System.Threading.Tasks.Dataflow.BatchBlock`1.BatchSize" />.</summary>
		// Token: 0x0600014F RID: 335 RVA: 0x0000587D File Offset: 0x00003A7D
		public void TriggerBatch()
		{
			this._target.TriggerBatch();
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" /> instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x06000150 RID: 336 RVA: 0x0000588A File Offset: 0x00003A8A
		public IDisposable LinkTo(ITargetBlock<T[]> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate a value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x06000151 RID: 337 RVA: 0x00005899 File Offset: 0x00003A99
		public bool TryReceive(Predicate<T[]> filter, out T[] item)
		{
			return this._source.TryReceive(filter, out item);
		}

		/// <summary>Attempts to synchronously receive all available items from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if one or more items could be received; otherwise, false.</returns>
		/// <param name="items">The items received from the source.</param>
		// Token: 0x06000152 RID: 338 RVA: 0x000058A8 File Offset: 0x00003AA8
		public bool TryReceiveAll(out IList<T[]> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets the number of output items available to be received from this block.</summary>
		/// <returns>The number of output items.</returns>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000058B6 File Offset: 0x00003AB6
		public int OutputCount
		{
			get
			{
				return this._source.OutputCount;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000058C3 File Offset: 0x00003AC3
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		/// <summary>Gets the size of the batches generated by this <see cref="T:System.Threading.Tasks.Dataflow.BatchBlock`1" />.</summary>
		/// <returns>The batch size.</returns>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000058D0 File Offset: 0x00003AD0
		public int BatchSize
		{
			get
			{
				return this._target.BatchSize;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000058DD File Offset: 0x00003ADD
		DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
		{
			return this._target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000058EF File Offset: 0x00003AEF
		T[] ISourceBlock<!0[]>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000058FF File Offset: 0x00003AFF
		bool ISourceBlock<!0[]>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000590E File Offset: 0x00003B0E
		void ISourceBlock<!0[]>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600015A RID: 346 RVA: 0x0000591D File Offset: 0x00003B1D
		private int OutputCountForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().OutputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x0600015B RID: 347 RVA: 0x0000592F File Offset: 0x00003B2F
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00005942 File Offset: 0x00003B42
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, BatchSize={1}, OutputCount={2}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.BatchSize, this.OutputCountForDebugger);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005975 File Offset: 0x00003B75
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x0400008C RID: 140
		private readonly BatchBlock<T>.BatchBlockTargetCore _target;

		// Token: 0x0400008D RID: 141
		private readonly SourceCore<T[]> _source;

		// Token: 0x02000039 RID: 57
		private sealed class DebugView
		{
			// Token: 0x06000160 RID: 352 RVA: 0x00005998 File Offset: 0x00003B98
			public DebugView(BatchBlock<T> batchBlock)
			{
				this._batchBlock = batchBlock;
				this._targetDebuggingInformation = batchBlock._target.GetDebuggingInformation();
				this._sourceDebuggingInformation = batchBlock._source.GetDebuggingInformation();
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000161 RID: 353 RVA: 0x000059C9 File Offset: 0x00003BC9
			public IEnumerable<T> InputQueue
			{
				get
				{
					return this._targetDebuggingInformation.InputQueue;
				}
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000162 RID: 354 RVA: 0x000059D6 File Offset: 0x00003BD6
			public IEnumerable<T[]> OutputQueue
			{
				get
				{
					return this._sourceDebuggingInformation.OutputQueue;
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000163 RID: 355 RVA: 0x000059E3 File Offset: 0x00003BE3
			public long BatchesCompleted
			{
				get
				{
					return this._targetDebuggingInformation.NumberOfBatchesCompleted;
				}
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x06000164 RID: 356 RVA: 0x000059F0 File Offset: 0x00003BF0
			public Task TaskForInputProcessing
			{
				get
				{
					return this._targetDebuggingInformation.TaskForInputProcessing;
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x06000165 RID: 357 RVA: 0x000059FD File Offset: 0x00003BFD
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x06000166 RID: 358 RVA: 0x00005A0A File Offset: 0x00003C0A
			public GroupingDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._targetDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x06000167 RID: 359 RVA: 0x00005A17 File Offset: 0x00003C17
			public int BatchSize
			{
				get
				{
					return this._batchBlock.BatchSize;
				}
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000168 RID: 360 RVA: 0x00005A24 File Offset: 0x00003C24
			public bool IsDecliningPermanently
			{
				get
				{
					return this._targetDebuggingInformation.IsDecliningPermanently;
				}
			}

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000169 RID: 361 RVA: 0x00005A31 File Offset: 0x00003C31
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x0600016A RID: 362 RVA: 0x00005A3E File Offset: 0x00003C3E
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._batchBlock);
				}
			}

			// Token: 0x17000067 RID: 103
			// (get) Token: 0x0600016B RID: 363 RVA: 0x00005A4B File Offset: 0x00003C4B
			public QueuedMap<ISourceBlock<T>, DataflowMessageHeader> PostponedMessages
			{
				get
				{
					return this._targetDebuggingInformation.PostponedMessages;
				}
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x0600016C RID: 364 RVA: 0x00005A58 File Offset: 0x00003C58
			public TargetRegistry<T[]> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x0600016D RID: 365 RVA: 0x00005A65 File Offset: 0x00003C65
			public ITargetBlock<T[]> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x0400008E RID: 142
			private BatchBlock<T> _batchBlock;

			// Token: 0x0400008F RID: 143
			private readonly BatchBlock<T>.BatchBlockTargetCore.DebuggingInformation _targetDebuggingInformation;

			// Token: 0x04000090 RID: 144
			private readonly SourceCore<T[]>.DebuggingInformation _sourceDebuggingInformation;
		}

		// Token: 0x0200003A RID: 58
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class BatchBlockTargetCore
		{
			// Token: 0x1700006A RID: 106
			// (get) Token: 0x0600016E RID: 366 RVA: 0x00005A72 File Offset: 0x00003C72
			private object IncomingLock
			{
				get
				{
					return this._completionTask;
				}
			}

			// Token: 0x0600016F RID: 367 RVA: 0x00005A7C File Offset: 0x00003C7C
			internal BatchBlockTargetCore(BatchBlock<T> owningBatch, int batchSize, Action<T[]> batchCompletedAction, GroupingDataflowBlockOptions dataflowBlockOptions)
			{
				this._owningBatch = owningBatch;
				this._batchSize = batchSize;
				this._batchCompletedAction = batchCompletedAction;
				this._dataflowBlockOptions = dataflowBlockOptions;
				bool flag = dataflowBlockOptions.BoundedCapacity > 0;
				if (!this._dataflowBlockOptions.Greedy || flag)
				{
					this._nonGreedyState = new BatchBlock<T>.BatchBlockTargetCore.NonGreedyState(batchSize);
				}
				if (flag)
				{
					this._boundingState = new BoundingState(dataflowBlockOptions.BoundedCapacity);
				}
			}

			// Token: 0x06000170 RID: 368 RVA: 0x00005B00 File Offset: 0x00003D00
			internal void TriggerBatch()
			{
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					if (!this._decliningPermanently && !this._dataflowBlockOptions.CancellationToken.IsCancellationRequested)
					{
						if (this._nonGreedyState == null)
						{
							this.MakeBatchIfPossible(true);
						}
						else
						{
							this._nonGreedyState.AcceptFewerThanBatchSize = true;
							this.ProcessAsyncIfNecessary(false);
						}
					}
					this.CompleteBlockIfPossible();
				}
			}

			// Token: 0x06000171 RID: 369 RVA: 0x00005B84 File Offset: 0x00003D84
			internal DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
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
					else if (this._dataflowBlockOptions.Greedy && (this._boundingState == null || (this._boundingState.CountIsLessThanBound && this._nonGreedyState.PostponedMessages.Count == 0 && this._nonGreedyState.TaskForInputProcessing == null)))
					{
						if (consumeToAccept)
						{
							bool flag2;
							messageValue = source.ConsumeMessage(messageHeader, this._owningBatch, out flag2);
							if (!flag2)
							{
								return DataflowMessageStatus.NotAvailable;
							}
						}
						this._messages.Enqueue(messageValue);
						if (this._boundingState != null)
						{
							this._boundingState.CurrentCount++;
						}
						if (!this._decliningPermanently && this._batchesCompleted + (long)(this._messages.Count / this._batchSize) >= this._dataflowBlockOptions.ActualMaxNumberOfGroups)
						{
							this._decliningPermanently = true;
						}
						this.MakeBatchIfPossible(false);
						this.CompleteBlockIfPossible();
						result = DataflowMessageStatus.Accepted;
					}
					else if (source != null)
					{
						this._nonGreedyState.PostponedMessages.Push(source, messageHeader);
						if (!this._dataflowBlockOptions.Greedy)
						{
							this.ProcessAsyncIfNecessary(false);
						}
						result = DataflowMessageStatus.Postponed;
					}
					else
					{
						result = DataflowMessageStatus.Declined;
					}
				}
				return result;
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00005D1C File Offset: 0x00003F1C
			internal void Complete(Exception exception, bool dropPendingMessages, bool releaseReservedMessages, bool revertProcessingState = false)
			{
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					if (exception != null && (!this._decliningPermanently || releaseReservedMessages))
					{
						this._owningBatch._source.AddException(exception);
					}
					if (dropPendingMessages)
					{
						this._messages.Clear();
					}
				}
				if (releaseReservedMessages)
				{
					try
					{
						this.ReleaseReservedMessages(false);
					}
					catch (Exception exception2)
					{
						this._owningBatch._source.AddException(exception2);
					}
				}
				object incomingLock2 = this.IncomingLock;
				lock (incomingLock2)
				{
					if (revertProcessingState)
					{
						this._nonGreedyState.TaskForInputProcessing = null;
					}
					this._decliningPermanently = true;
					this.CompleteBlockIfPossible();
				}
			}

			// Token: 0x1700006B RID: 107
			// (get) Token: 0x06000173 RID: 371 RVA: 0x00005DFC File Offset: 0x00003FFC
			internal Task Completion
			{
				get
				{
					return this._completionTask.Task;
				}
			}

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x06000174 RID: 372 RVA: 0x00005E09 File Offset: 0x00004009
			internal int BatchSize
			{
				get
				{
					return this._batchSize;
				}
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x06000175 RID: 373 RVA: 0x00005E14 File Offset: 0x00004014
			private bool CanceledOrFaulted
			{
				get
				{
					return this._dataflowBlockOptions.CancellationToken.IsCancellationRequested || this._owningBatch._source.HasExceptions;
				}
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x06000176 RID: 374 RVA: 0x00005E48 File Offset: 0x00004048
			private int BoundedCapacityAvailable
			{
				get
				{
					if (this._boundingState == null)
					{
						return this._batchSize;
					}
					return this._dataflowBlockOptions.BoundedCapacity - this._boundingState.CurrentCount;
				}
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00005E70 File Offset: 0x00004070
			private void CompleteBlockIfPossible()
			{
				if (!this._completionReserved)
				{
					bool flag = this._nonGreedyState != null && this._nonGreedyState.TaskForInputProcessing != null;
					bool flag2 = this._batchesCompleted >= this._dataflowBlockOptions.ActualMaxNumberOfGroups;
					bool flag3 = this._decliningPermanently && this._messages.Count < this._batchSize;
					bool flag4 = !flag && (flag2 || flag3 || this.CanceledOrFaulted);
					if (flag4)
					{
						this._completionReserved = true;
						this._decliningPermanently = true;
						if (this._messages.Count > 0)
						{
							this.MakeBatchIfPossible(true);
						}
						Task.Factory.StartNew(delegate(object thisTargetCore)
						{
							BatchBlock<T>.BatchBlockTargetCore batchBlockTargetCore = (BatchBlock<T>.BatchBlockTargetCore)thisTargetCore;
							List<Exception> list = null;
							if (batchBlockTargetCore._nonGreedyState != null)
							{
								Common.ReleaseAllPostponedMessages<T>(batchBlockTargetCore._owningBatch, batchBlockTargetCore._nonGreedyState.PostponedMessages, ref list);
							}
							if (list != null)
							{
								batchBlockTargetCore._owningBatch._source.AddExceptions(list);
							}
							batchBlockTargetCore._completionTask.TrySetResult(default(VoidResult));
						}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
					}
				}
			}

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x06000178 RID: 376 RVA: 0x00005F50 File Offset: 0x00004150
			private bool BatchesNeedProcessing
			{
				get
				{
					bool flag = this._batchesCompleted >= this._dataflowBlockOptions.ActualMaxNumberOfGroups;
					bool flag2 = this._nonGreedyState != null && this._nonGreedyState.TaskForInputProcessing != null;
					if (flag || flag2 || this.CanceledOrFaulted)
					{
						return false;
					}
					int num = this._batchSize - this._messages.Count;
					int boundedCapacityAvailable = this.BoundedCapacityAvailable;
					if (num <= 0)
					{
						return true;
					}
					if (this._nonGreedyState != null)
					{
						if (this._nonGreedyState.AcceptFewerThanBatchSize && (this._messages.Count > 0 || (this._nonGreedyState.PostponedMessages.Count > 0 && boundedCapacityAvailable > 0)))
						{
							return true;
						}
						if (this._dataflowBlockOptions.Greedy)
						{
							if (this._nonGreedyState.PostponedMessages.Count > 0 && boundedCapacityAvailable > 0)
							{
								return true;
							}
						}
						else if (this._nonGreedyState.PostponedMessages.Count >= num && boundedCapacityAvailable >= num)
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x06000179 RID: 377 RVA: 0x00006039 File Offset: 0x00004239
			private void ProcessAsyncIfNecessary(bool isReplacementReplica = false)
			{
				if (this.BatchesNeedProcessing)
				{
					this.ProcessAsyncIfNecessary_Slow(isReplacementReplica);
				}
			}

			// Token: 0x0600017A RID: 378 RVA: 0x0000604C File Offset: 0x0000424C
			private void ProcessAsyncIfNecessary_Slow(bool isReplacementReplica)
			{
				this._nonGreedyState.TaskForInputProcessing = new Task(delegate(object thisBatchTarget)
				{
					((BatchBlock<T>.BatchBlockTargetCore)thisBatchTarget).ProcessMessagesLoopCore();
				}, this, Common.GetCreationOptionsForTask(isReplacementReplica));
				DataflowEtwProvider log = DataflowEtwProvider.Log;
				if (log.IsEnabled())
				{
					log.TaskLaunchedForMessageHandling(this._owningBatch, this._nonGreedyState.TaskForInputProcessing, DataflowEtwProvider.TaskLaunchedReason.ProcessingInputMessages, this._messages.Count + this._nonGreedyState.PostponedMessages.Count);
				}
				Exception ex = Common.StartTaskSafe(this._nonGreedyState.TaskForInputProcessing, this._dataflowBlockOptions.TaskScheduler);
				if (ex != null)
				{
					Task.Factory.StartNew(delegate(object exc)
					{
						this.Complete((Exception)exc, true, true, true);
					}, ex, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
				}
			}

			// Token: 0x0600017B RID: 379 RVA: 0x00006118 File Offset: 0x00004318
			private void ProcessMessagesLoopCore()
			{
				try
				{
					int actualMaxMessagesPerTask = this._dataflowBlockOptions.ActualMaxMessagesPerTask;
					int num = 0;
					bool flag3;
					do
					{
						bool flag = Volatile.Read(ref this._nonGreedyState.AcceptFewerThanBatchSize);
						if (!this._dataflowBlockOptions.Greedy)
						{
							this.RetrievePostponedItemsNonGreedy(flag);
						}
						else
						{
							this.RetrievePostponedItemsGreedyBounded(flag);
						}
						object incomingLock = this.IncomingLock;
						lock (incomingLock)
						{
							flag3 = this.MakeBatchIfPossible(flag);
							if (flag3 || flag)
							{
								this._nonGreedyState.AcceptFewerThanBatchSize = false;
							}
						}
						num++;
					}
					while (flag3 && num < actualMaxMessagesPerTask);
				}
				catch (Exception exception)
				{
					this.Complete(exception, false, true, false);
				}
				finally
				{
					object incomingLock2 = this.IncomingLock;
					lock (incomingLock2)
					{
						this._nonGreedyState.TaskForInputProcessing = null;
						this.ProcessAsyncIfNecessary(true);
						this.CompleteBlockIfPossible();
					}
				}
			}

			// Token: 0x0600017C RID: 380 RVA: 0x00006228 File Offset: 0x00004428
			private bool MakeBatchIfPossible(bool evenIfFewerThanBatchSize)
			{
				bool flag = this._messages.Count >= this._batchSize;
				if (flag || (evenIfFewerThanBatchSize && this._messages.Count > 0))
				{
					T[] array = new T[flag ? this._batchSize : this._messages.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = this._messages.Dequeue();
					}
					this._batchCompletedAction(array);
					this._batchesCompleted += 1L;
					if (this._batchesCompleted >= this._dataflowBlockOptions.ActualMaxNumberOfGroups)
					{
						this._decliningPermanently = true;
					}
					return true;
				}
				return false;
			}

			// Token: 0x0600017D RID: 381 RVA: 0x000062D8 File Offset: 0x000044D8
			private void RetrievePostponedItemsNonGreedy(bool allowFewerThanBatchSize)
			{
				QueuedMap<ISourceBlock<T>, DataflowMessageHeader> postponedMessages = this._nonGreedyState.PostponedMessages;
				KeyValuePair<ISourceBlock<T>, DataflowMessageHeader>[] postponedMessagesTemp = this._nonGreedyState.PostponedMessagesTemp;
				List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>> reservedSourcesTemp = this._nonGreedyState.ReservedSourcesTemp;
				reservedSourcesTemp.Clear();
				object incomingLock = this.IncomingLock;
				int num;
				lock (incomingLock)
				{
					int boundedCapacityAvailable = this.BoundedCapacityAvailable;
					if (this._decliningPermanently || postponedMessages.Count == 0 || boundedCapacityAvailable <= 0 || (!allowFewerThanBatchSize && (postponedMessages.Count < this._batchSize || boundedCapacityAvailable < this._batchSize)))
					{
						return;
					}
					num = postponedMessages.PopRange(postponedMessagesTemp, 0, this._batchSize);
				}
				for (int i = 0; i < num; i++)
				{
					KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair = postponedMessagesTemp[i];
					if (keyValuePair.Key.ReserveMessage(keyValuePair.Value, this._owningBatch))
					{
						KeyValuePair<DataflowMessageHeader, T> value = new KeyValuePair<DataflowMessageHeader, T>(keyValuePair.Value, default(T));
						KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> item = new KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>(keyValuePair.Key, value);
						reservedSourcesTemp.Add(item);
					}
				}
				Array.Clear(postponedMessagesTemp, 0, postponedMessagesTemp.Length);
				while (reservedSourcesTemp.Count < this._batchSize)
				{
					object incomingLock2 = this.IncomingLock;
					KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair2;
					lock (incomingLock2)
					{
						if (!postponedMessages.TryPop(out keyValuePair2))
						{
							break;
						}
					}
					if (keyValuePair2.Key.ReserveMessage(keyValuePair2.Value, this._owningBatch))
					{
						KeyValuePair<DataflowMessageHeader, T> value2 = new KeyValuePair<DataflowMessageHeader, T>(keyValuePair2.Value, default(T));
						KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> item2 = new KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>(keyValuePair2.Key, value2);
						reservedSourcesTemp.Add(item2);
					}
				}
				if (reservedSourcesTemp.Count > 0)
				{
					bool flag3 = true;
					if (allowFewerThanBatchSize)
					{
						object incomingLock3 = this.IncomingLock;
						lock (incomingLock3)
						{
							if (!this._decliningPermanently && this._batchesCompleted + 1L >= this._dataflowBlockOptions.ActualMaxNumberOfGroups)
							{
								flag3 = !this._decliningPermanently;
								this._decliningPermanently = true;
							}
						}
					}
					if (flag3 && (allowFewerThanBatchSize || reservedSourcesTemp.Count == this._batchSize))
					{
						this.ConsumeReservedMessagesNonGreedy();
					}
					else
					{
						this.ReleaseReservedMessages(true);
					}
				}
				reservedSourcesTemp.Clear();
			}

			// Token: 0x0600017E RID: 382 RVA: 0x0000652C File Offset: 0x0000472C
			private void RetrievePostponedItemsGreedyBounded(bool allowFewerThanBatchSize)
			{
				QueuedMap<ISourceBlock<T>, DataflowMessageHeader> postponedMessages = this._nonGreedyState.PostponedMessages;
				KeyValuePair<ISourceBlock<T>, DataflowMessageHeader>[] postponedMessagesTemp = this._nonGreedyState.PostponedMessagesTemp;
				List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>> reservedSourcesTemp = this._nonGreedyState.ReservedSourcesTemp;
				reservedSourcesTemp.Clear();
				object incomingLock = this.IncomingLock;
				int num;
				int num2;
				lock (incomingLock)
				{
					int boundedCapacityAvailable = this.BoundedCapacityAvailable;
					num = this._batchSize - this._messages.Count;
					if (this._decliningPermanently || postponedMessages.Count == 0 || boundedCapacityAvailable <= 0)
					{
						return;
					}
					if (boundedCapacityAvailable < num)
					{
						num = boundedCapacityAvailable;
					}
					num2 = postponedMessages.PopRange(postponedMessagesTemp, 0, num);
				}
				for (int i = 0; i < num2; i++)
				{
					KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair = postponedMessagesTemp[i];
					KeyValuePair<DataflowMessageHeader, T> value = new KeyValuePair<DataflowMessageHeader, T>(keyValuePair.Value, default(T));
					KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> item = new KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>(keyValuePair.Key, value);
					reservedSourcesTemp.Add(item);
				}
				Array.Clear(postponedMessagesTemp, 0, postponedMessagesTemp.Length);
				while (reservedSourcesTemp.Count < num)
				{
					object incomingLock2 = this.IncomingLock;
					KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair2;
					lock (incomingLock2)
					{
						if (!postponedMessages.TryPop(out keyValuePair2))
						{
							break;
						}
					}
					KeyValuePair<DataflowMessageHeader, T> value2 = new KeyValuePair<DataflowMessageHeader, T>(keyValuePair2.Value, default(T));
					KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> item2 = new KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>(keyValuePair2.Key, value2);
					reservedSourcesTemp.Add(item2);
				}
				if (reservedSourcesTemp.Count > 0)
				{
					bool flag3 = true;
					if (allowFewerThanBatchSize)
					{
						object incomingLock3 = this.IncomingLock;
						lock (incomingLock3)
						{
							if (!this._decliningPermanently && this._batchesCompleted + 1L >= this._dataflowBlockOptions.ActualMaxNumberOfGroups)
							{
								flag3 = !this._decliningPermanently;
								this._decliningPermanently = true;
							}
						}
					}
					if (flag3)
					{
						this.ConsumeReservedMessagesGreedyBounded();
					}
				}
				reservedSourcesTemp.Clear();
			}

			// Token: 0x0600017F RID: 383 RVA: 0x00006728 File Offset: 0x00004928
			private void ConsumeReservedMessagesNonGreedy()
			{
				List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>> reservedSourcesTemp = this._nonGreedyState.ReservedSourcesTemp;
				for (int i = 0; i < reservedSourcesTemp.Count; i++)
				{
					KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> keyValuePair = reservedSourcesTemp[i];
					reservedSourcesTemp[i] = default(KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>);
					bool flag;
					T value = keyValuePair.Key.ConsumeMessage(keyValuePair.Value.Key, this._owningBatch, out flag);
					if (!flag)
					{
						for (int j = 0; j < i; j++)
						{
							reservedSourcesTemp[j] = default(KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>);
						}
						throw new InvalidOperationException(SR.InvalidOperation_FailedToConsumeReservedMessage);
					}
					KeyValuePair<DataflowMessageHeader, T> value2 = new KeyValuePair<DataflowMessageHeader, T>(keyValuePair.Value.Key, value);
					KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> value3 = new KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>(keyValuePair.Key, value2);
					reservedSourcesTemp[i] = value3;
				}
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					if (this._boundingState != null)
					{
						this._boundingState.CurrentCount += reservedSourcesTemp.Count;
					}
					foreach (KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> keyValuePair2 in reservedSourcesTemp)
					{
						this._messages.Enqueue(keyValuePair2.Value.Value);
					}
				}
			}

			// Token: 0x06000180 RID: 384 RVA: 0x0000689C File Offset: 0x00004A9C
			private void ConsumeReservedMessagesGreedyBounded()
			{
				int num = 0;
				List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>> reservedSourcesTemp = this._nonGreedyState.ReservedSourcesTemp;
				for (int i = 0; i < reservedSourcesTemp.Count; i++)
				{
					KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> keyValuePair = reservedSourcesTemp[i];
					reservedSourcesTemp[i] = default(KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>);
					bool flag;
					T value = keyValuePair.Key.ConsumeMessage(keyValuePair.Value.Key, this._owningBatch, out flag);
					if (flag)
					{
						KeyValuePair<DataflowMessageHeader, T> value2 = new KeyValuePair<DataflowMessageHeader, T>(keyValuePair.Value.Key, value);
						KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> value3 = new KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>(keyValuePair.Key, value2);
						reservedSourcesTemp[i] = value3;
						num++;
					}
				}
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					if (this._boundingState != null)
					{
						this._boundingState.CurrentCount += num;
					}
					foreach (KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> keyValuePair2 in reservedSourcesTemp)
					{
						if (keyValuePair2.Key != null)
						{
							this._messages.Enqueue(keyValuePair2.Value.Value);
						}
					}
				}
			}

			// Token: 0x06000181 RID: 385 RVA: 0x000069EC File Offset: 0x00004BEC
			internal void ReleaseReservedMessages(bool throwOnFirstException)
			{
				List<Exception> list = null;
				List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>> reservedSourcesTemp = this._nonGreedyState.ReservedSourcesTemp;
				for (int i = 0; i < reservedSourcesTemp.Count; i++)
				{
					KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>> keyValuePair = reservedSourcesTemp[i];
					reservedSourcesTemp[i] = default(KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>);
					ISourceBlock<T> key = keyValuePair.Key;
					KeyValuePair<DataflowMessageHeader, T> value = keyValuePair.Value;
					if (key != null && value.Key.IsValid)
					{
						try
						{
							key.ReleaseReservation(value.Key, this._owningBatch);
						}
						catch (Exception item)
						{
							if (throwOnFirstException)
							{
								throw;
							}
							if (list == null)
							{
								list = new List<Exception>(1);
							}
							list.Add(item);
						}
					}
				}
				if (list != null)
				{
					throw new AggregateException(list);
				}
			}

			// Token: 0x06000182 RID: 386 RVA: 0x00006AA4 File Offset: 0x00004CA4
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

			// Token: 0x06000183 RID: 387 RVA: 0x00006B08 File Offset: 0x00004D08
			internal static int CountItems(T[] singleOutputItem, IList<T[]> multipleOutputItems)
			{
				if (multipleOutputItems == null)
				{
					return singleOutputItem.Length;
				}
				int num = 0;
				foreach (T[] array in multipleOutputItems)
				{
					num += array.Length;
				}
				return num;
			}

			// Token: 0x06000184 RID: 388 RVA: 0x00006B5C File Offset: 0x00004D5C
			internal BatchBlock<T>.BatchBlockTargetCore.DebuggingInformation GetDebuggingInformation()
			{
				return new BatchBlock<T>.BatchBlockTargetCore.DebuggingInformation(this);
			}

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x06000185 RID: 389 RVA: 0x00006B64 File Offset: 0x00004D64
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay owningBatch = this._owningBatch;
					return string.Format("Block=\"{0}\"", (owningBatch != null) ? owningBatch.Content : this._owningBatch);
				}
			}

			// Token: 0x04000091 RID: 145
			private readonly Queue<T> _messages = new Queue<T>();

			// Token: 0x04000092 RID: 146
			private readonly TaskCompletionSource<VoidResult> _completionTask = new TaskCompletionSource<VoidResult>();

			// Token: 0x04000093 RID: 147
			private readonly BatchBlock<T> _owningBatch;

			// Token: 0x04000094 RID: 148
			private readonly int _batchSize;

			// Token: 0x04000095 RID: 149
			private readonly BatchBlock<T>.BatchBlockTargetCore.NonGreedyState _nonGreedyState;

			// Token: 0x04000096 RID: 150
			private readonly BoundingState _boundingState;

			// Token: 0x04000097 RID: 151
			private readonly GroupingDataflowBlockOptions _dataflowBlockOptions;

			// Token: 0x04000098 RID: 152
			private readonly Action<T[]> _batchCompletedAction;

			// Token: 0x04000099 RID: 153
			private bool _decliningPermanently;

			// Token: 0x0400009A RID: 154
			private long _batchesCompleted;

			// Token: 0x0400009B RID: 155
			private bool _completionReserved;

			// Token: 0x0200003B RID: 59
			private sealed class NonGreedyState
			{
				// Token: 0x06000187 RID: 391 RVA: 0x00006BA4 File Offset: 0x00004DA4
				internal NonGreedyState(int batchSize)
				{
					this.PostponedMessages = new QueuedMap<ISourceBlock<T>, DataflowMessageHeader>(batchSize);
					this.PostponedMessagesTemp = new KeyValuePair<ISourceBlock<T>, DataflowMessageHeader>[batchSize];
					this.ReservedSourcesTemp = new List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>>(batchSize);
				}

				// Token: 0x0400009C RID: 156
				internal readonly QueuedMap<ISourceBlock<T>, DataflowMessageHeader> PostponedMessages;

				// Token: 0x0400009D RID: 157
				internal readonly KeyValuePair<ISourceBlock<T>, DataflowMessageHeader>[] PostponedMessagesTemp;

				// Token: 0x0400009E RID: 158
				internal readonly List<KeyValuePair<ISourceBlock<T>, KeyValuePair<DataflowMessageHeader, T>>> ReservedSourcesTemp;

				// Token: 0x0400009F RID: 159
				internal bool AcceptFewerThanBatchSize;

				// Token: 0x040000A0 RID: 160
				internal Task TaskForInputProcessing;
			}

			// Token: 0x0200003C RID: 60
			internal sealed class DebuggingInformation
			{
				// Token: 0x06000188 RID: 392 RVA: 0x00006BD0 File Offset: 0x00004DD0
				public DebuggingInformation(BatchBlock<T>.BatchBlockTargetCore target)
				{
					this._target = target;
				}

				// Token: 0x17000071 RID: 113
				// (get) Token: 0x06000189 RID: 393 RVA: 0x00006BDF File Offset: 0x00004DDF
				public IEnumerable<T> InputQueue
				{
					get
					{
						return this._target._messages.ToList<T>();
					}
				}

				// Token: 0x17000072 RID: 114
				// (get) Token: 0x0600018A RID: 394 RVA: 0x00006BF1 File Offset: 0x00004DF1
				public Task TaskForInputProcessing
				{
					get
					{
						if (this._target._nonGreedyState == null)
						{
							return null;
						}
						return this._target._nonGreedyState.TaskForInputProcessing;
					}
				}

				// Token: 0x17000073 RID: 115
				// (get) Token: 0x0600018B RID: 395 RVA: 0x00006C12 File Offset: 0x00004E12
				public QueuedMap<ISourceBlock<T>, DataflowMessageHeader> PostponedMessages
				{
					get
					{
						if (this._target._nonGreedyState == null)
						{
							return null;
						}
						return this._target._nonGreedyState.PostponedMessages;
					}
				}

				// Token: 0x17000074 RID: 116
				// (get) Token: 0x0600018C RID: 396 RVA: 0x00006C33 File Offset: 0x00004E33
				public bool IsDecliningPermanently
				{
					get
					{
						return this._target._decliningPermanently;
					}
				}

				// Token: 0x17000075 RID: 117
				// (get) Token: 0x0600018D RID: 397 RVA: 0x00006C40 File Offset: 0x00004E40
				public GroupingDataflowBlockOptions DataflowBlockOptions
				{
					get
					{
						return this._target._dataflowBlockOptions;
					}
				}

				// Token: 0x17000076 RID: 118
				// (get) Token: 0x0600018E RID: 398 RVA: 0x00006C4D File Offset: 0x00004E4D
				public long NumberOfBatchesCompleted
				{
					get
					{
						return this._target._batchesCompleted;
					}
				}

				// Token: 0x040000A1 RID: 161
				private BatchBlock<T>.BatchBlockTargetCore _target;
			}
		}
	}
}
