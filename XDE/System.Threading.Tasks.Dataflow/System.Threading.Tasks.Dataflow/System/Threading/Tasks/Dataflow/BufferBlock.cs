using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a buffer for storing data.</summary>
	/// <typeparam name="T">Specifies the type of the data buffered by this dataflow block.</typeparam>
	// Token: 0x0200004D RID: 77
	[DebuggerTypeProxy(typeof(BufferBlock<>.DebugView))]
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	public sealed class BufferBlock<T> : IPropagatorBlock<T, T>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<T>, IReceivableSourceBlock<T>, IDebuggerDisplay
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00008DA3 File Offset: 0x00006FA3
		private object IncomingLock
		{
			get
			{
				return this._source;
			}
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.BufferBlock`1" />.</summary>
		// Token: 0x06000243 RID: 579 RVA: 0x00008DAB File Offset: 0x00006FAB
		public BufferBlock() : this(DataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.BufferBlock`1" /> with the specified <see cref="T:System.Threading.Tasks.Dataflow.DataflowBlockOptions" />.</summary>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.BufferBlock`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x06000244 RID: 580 RVA: 0x00008DB8 File Offset: 0x00006FB8
		public BufferBlock(DataflowBlockOptions dataflowBlockOptions)
		{
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<ISourceBlock<T>, int> itemsRemovedAction = null;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				itemsRemovedAction = delegate(ISourceBlock<T> owningSource, int count)
				{
					((BufferBlock<T>)owningSource).OnItemsRemoved(count);
				};
				this._boundingState = new BoundingStateWithPostponedAndTask<T>(dataflowBlockOptions.BoundedCapacity);
			}
			this._source = new SourceCore<T>(this, dataflowBlockOptions, delegate(ISourceBlock<T> owningSource)
			{
				((BufferBlock<T>)owningSource).Complete();
			}, itemsRemovedAction, null);
			this._source.Completion.ContinueWith(delegate(Task completed, object state)
			{
				IDataflowBlock dataflowBlock = (BufferBlock<T>)state;
				dataflowBlock.Fault(completed.Exception);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
			Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this._source.Completion, delegate(object owningSource)
			{
				((BufferBlock<T>)owningSource).Complete();
			}, this);
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00008EE0 File Offset: 0x000070E0
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
			object incomingLock = this.IncomingLock;
			DataflowMessageStatus result;
			lock (incomingLock)
			{
				if (this._targetDecliningPermanently)
				{
					this.CompleteTargetIfPossible();
					result = DataflowMessageStatus.DecliningPermanently;
				}
				else if (this._boundingState == null || (this._boundingState.CountIsLessThanBound && this._boundingState.PostponedMessages.Count == 0 && this._boundingState.TaskForInputProcessing == null))
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
					this._source.AddMessage(messageValue);
					if (this._boundingState != null)
					{
						this._boundingState.CurrentCount++;
					}
					result = DataflowMessageStatus.Accepted;
				}
				else if (source != null)
				{
					this._boundingState.PostponedMessages.Push(source, messageHeader);
					result = DataflowMessageStatus.Postponed;
				}
				else
				{
					result = DataflowMessageStatus.Declined;
				}
			}
			return result;
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x06000246 RID: 582 RVA: 0x00008FF4 File Offset: 0x000071F4
		public void Complete()
		{
			this.CompleteCore(null, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x06000247 RID: 583 RVA: 0x00008FFF File Offset: 0x000071FF
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this.CompleteCore(exception, false, false);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00009018 File Offset: 0x00007218
		private void CompleteCore(Exception exception, bool storeExceptionEvenIfAlreadyCompleting, bool revertProcessingState = false)
		{
			object incomingLock = this.IncomingLock;
			lock (incomingLock)
			{
				if (exception != null && (!this._targetDecliningPermanently || storeExceptionEvenIfAlreadyCompleting))
				{
					this._source.AddException(exception);
				}
				if (revertProcessingState)
				{
					this._boundingState.TaskForInputProcessing = null;
				}
				this._targetDecliningPermanently = true;
				this.CompleteTargetIfPossible();
			}
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" />  instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x06000249 RID: 585 RVA: 0x0000908C File Offset: 0x0000728C
		public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x0600024A RID: 586 RVA: 0x0000909B File Offset: 0x0000729B
		public bool TryReceive(Predicate<T> filter, out T item)
		{
			return this._source.TryReceive(filter, out item);
		}

		/// <summary>Attempts to synchronously receive all available items from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if one or more items could be received; otherwise, false.</returns>
		/// <param name="items">The items received from the source.</param>
		// Token: 0x0600024B RID: 587 RVA: 0x000090AA File Offset: 0x000072AA
		public bool TryReceiveAll(out IList<T> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets the number of items currently stored in the buffer.</summary>
		/// <returns>The number of items.</returns>
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000090B8 File Offset: 0x000072B8
		public int Count
		{
			get
			{
				return this._source.OutputCount;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000090C5 File Offset: 0x000072C5
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000090D2 File Offset: 0x000072D2
		T ISourceBlock<!0>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000090E2 File Offset: 0x000072E2
		bool ISourceBlock<!0>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000090F1 File Offset: 0x000072F1
		void ISourceBlock<!0>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009100 File Offset: 0x00007300
		private void OnItemsRemoved(int numItemsRemoved)
		{
			if (this._boundingState != null)
			{
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					this._boundingState.CurrentCount -= numItemsRemoved;
					this.ConsumeAsyncIfNecessary(false);
					this.CompleteTargetIfPossible();
				}
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009164 File Offset: 0x00007364
		internal void ConsumeAsyncIfNecessary(bool isReplacementReplica = false)
		{
			if (!this._targetDecliningPermanently && this._boundingState.TaskForInputProcessing == null && this._boundingState.PostponedMessages.Count > 0 && this._boundingState.CountIsLessThanBound)
			{
				this._boundingState.TaskForInputProcessing = new Task(delegate(object state)
				{
					((BufferBlock<T>)state).ConsumeMessagesLoopCore();
				}, this, Common.GetCreationOptionsForTask(isReplacementReplica));
				DataflowEtwProvider log = DataflowEtwProvider.Log;
				if (log.IsEnabled())
				{
					log.TaskLaunchedForMessageHandling(this, this._boundingState.TaskForInputProcessing, DataflowEtwProvider.TaskLaunchedReason.ProcessingInputMessages, this._boundingState.PostponedMessages.Count);
				}
				Exception ex = Common.StartTaskSafe(this._boundingState.TaskForInputProcessing, this._source.DataflowBlockOptions.TaskScheduler);
				if (ex != null)
				{
					Task.Factory.StartNew(delegate(object exc)
					{
						this.CompleteCore((Exception)exc, true, true);
					}, ex, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
				}
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009268 File Offset: 0x00007468
		private void ConsumeMessagesLoopCore()
		{
			try
			{
				int actualMaxMessagesPerTask = this._source.DataflowBlockOptions.ActualMaxMessagesPerTask;
				int num = 0;
				while (num < actualMaxMessagesPerTask && this.ConsumeAndStoreOneMessageIfAvailable())
				{
					num++;
				}
			}
			catch (Exception exception)
			{
				this.CompleteCore(exception, true, false);
			}
			finally
			{
				object incomingLock = this.IncomingLock;
				lock (incomingLock)
				{
					this._boundingState.TaskForInputProcessing = null;
					this.ConsumeAsyncIfNecessary(true);
					this.CompleteTargetIfPossible();
				}
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000930C File Offset: 0x0000750C
		private bool ConsumeAndStoreOneMessageIfAvailable()
		{
			bool result;
			for (;;)
			{
				object incomingLock = this.IncomingLock;
				KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair;
				lock (incomingLock)
				{
					if (this._targetDecliningPermanently)
					{
						result = false;
						break;
					}
					if (!this._boundingState.CountIsLessThanBound)
					{
						result = false;
						break;
					}
					if (!this._boundingState.PostponedMessages.TryPop(out keyValuePair))
					{
						result = false;
						break;
					}
					this._boundingState.CurrentCount++;
				}
				bool flag2 = false;
				try
				{
					T item = keyValuePair.Key.ConsumeMessage(keyValuePair.Value, this, out flag2);
					if (!flag2)
					{
						continue;
					}
					this._source.AddMessage(item);
					result = true;
				}
				finally
				{
					if (!flag2)
					{
						object incomingLock2 = this.IncomingLock;
						lock (incomingLock2)
						{
							this._boundingState.CurrentCount--;
						}
					}
				}
				break;
			}
			return result;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009424 File Offset: 0x00007624
		private void CompleteTargetIfPossible()
		{
			if (this._targetDecliningPermanently && !this._targetCompletionReserved && (this._boundingState == null || this._boundingState.TaskForInputProcessing == null))
			{
				this._targetCompletionReserved = true;
				if (this._boundingState != null && this._boundingState.PostponedMessages.Count > 0)
				{
					Task.Factory.StartNew(delegate(object state)
					{
						BufferBlock<T> bufferBlock = (BufferBlock<T>)state;
						List<Exception> list = null;
						if (bufferBlock._boundingState != null)
						{
							Common.ReleaseAllPostponedMessages<T>(bufferBlock, bufferBlock._boundingState.PostponedMessages, ref list);
						}
						if (list != null)
						{
							bufferBlock._source.AddExceptions(list);
						}
						bufferBlock._source.Complete();
					}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
					return;
				}
				this._source.Complete();
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000256 RID: 598 RVA: 0x000094C2 File Offset: 0x000076C2
		private int CountForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().OutputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x06000257 RID: 599 RVA: 0x000094D4 File Offset: 0x000076D4
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000258 RID: 600 RVA: 0x000094E7 File Offset: 0x000076E7
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, Count={1}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.CountForDebugger);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000950F File Offset: 0x0000770F
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x040000E7 RID: 231
		private readonly SourceCore<T> _source;

		// Token: 0x040000E8 RID: 232
		private readonly BoundingStateWithPostponedAndTask<T> _boundingState;

		// Token: 0x040000E9 RID: 233
		private bool _targetDecliningPermanently;

		// Token: 0x040000EA RID: 234
		private bool _targetCompletionReserved;

		// Token: 0x0200004E RID: 78
		private sealed class DebugView
		{
			// Token: 0x0600025B RID: 603 RVA: 0x00009527 File Offset: 0x00007727
			public DebugView(BufferBlock<T> bufferBlock)
			{
				this._bufferBlock = bufferBlock;
				this._sourceDebuggingInformation = bufferBlock._source.GetDebuggingInformation();
			}

			// Token: 0x170000C5 RID: 197
			// (get) Token: 0x0600025C RID: 604 RVA: 0x00009547 File Offset: 0x00007747
			public QueuedMap<ISourceBlock<T>, DataflowMessageHeader> PostponedMessages
			{
				get
				{
					if (this._bufferBlock._boundingState == null)
					{
						return null;
					}
					return this._bufferBlock._boundingState.PostponedMessages;
				}
			}

			// Token: 0x170000C6 RID: 198
			// (get) Token: 0x0600025D RID: 605 RVA: 0x00009568 File Offset: 0x00007768
			public IEnumerable<T> Queue
			{
				get
				{
					return this._sourceDebuggingInformation.OutputQueue;
				}
			}

			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x0600025E RID: 606 RVA: 0x00009575 File Offset: 0x00007775
			public Task TaskForInputProcessing
			{
				get
				{
					if (this._bufferBlock._boundingState == null)
					{
						return null;
					}
					return this._bufferBlock._boundingState.TaskForInputProcessing;
				}
			}

			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x0600025F RID: 607 RVA: 0x00009596 File Offset: 0x00007796
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x06000260 RID: 608 RVA: 0x000095A3 File Offset: 0x000077A3
			public DataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._sourceDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x06000261 RID: 609 RVA: 0x000095B0 File Offset: 0x000077B0
			public bool IsDecliningPermanently
			{
				get
				{
					return this._bufferBlock._targetDecliningPermanently;
				}
			}

			// Token: 0x170000CB RID: 203
			// (get) Token: 0x06000262 RID: 610 RVA: 0x000095BD File Offset: 0x000077BD
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x170000CC RID: 204
			// (get) Token: 0x06000263 RID: 611 RVA: 0x000095CA File Offset: 0x000077CA
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._bufferBlock);
				}
			}

			// Token: 0x170000CD RID: 205
			// (get) Token: 0x06000264 RID: 612 RVA: 0x000095D7 File Offset: 0x000077D7
			public TargetRegistry<T> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x06000265 RID: 613 RVA: 0x000095E4 File Offset: 0x000077E4
			public ITargetBlock<T> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x040000EB RID: 235
			private readonly BufferBlock<T> _bufferBlock;

			// Token: 0x040000EC RID: 236
			private readonly SourceCore<T>.DebuggingInformation _sourceDebuggingInformation;
		}
	}
}
