using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a buffer for storing at most one element at time, overwriting each message with the next as it arrives.  Messages are broadcast to all linked targets, all of which may consume a clone of the message.</summary>
	/// <typeparam name="T">Specifies the type of the data buffered by this dataflow block.</typeparam>
	// Token: 0x02000047 RID: 71
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(BroadcastBlock<>.DebugView))]
	public sealed class BroadcastBlock<T> : IPropagatorBlock<T, T>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<!0>, IReceivableSourceBlock<T>, IDebuggerDisplay
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000076D0 File Offset: 0x000058D0
		private object IncomingLock
		{
			get
			{
				return this._source;
			}
		}

		/// <summary>Initializes the <see cref="T:System.Threading.Tasks.Dataflow.BroadcastBlock`1" /> with the specified cloning function.</summary>
		/// <param name="cloningFunction">The function to use to clone the data when offered to other blocks.</param>
		// Token: 0x060001EF RID: 495 RVA: 0x000076D8 File Offset: 0x000058D8
		public BroadcastBlock(Func<T, T> cloningFunction) : this(cloningFunction, DataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes the <see cref="T:System.Threading.Tasks.Dataflow.BroadcastBlock`1" /> with the specified cloning function and <see cref="T:System.Threading.Tasks.Dataflow.DataflowBlockOptions" />.</summary>
		/// <param name="cloningFunction">The function to use to clone the data when offered to other blocks.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.BroadcastBlock`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x060001F0 RID: 496 RVA: 0x000076E8 File Offset: 0x000058E8
		public BroadcastBlock(Func<T, T> cloningFunction, DataflowBlockOptions dataflowBlockOptions)
		{
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<int> itemsRemovedAction = null;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				itemsRemovedAction = new Action<int>(this.OnItemsRemoved);
				this._boundingState = new BoundingStateWithPostponedAndTask<T>(dataflowBlockOptions.BoundedCapacity);
			}
			this._source = new BroadcastBlock<T>.BroadcastingSourceCore<T>(this, cloningFunction, dataflowBlockOptions, itemsRemovedAction);
			this._source.Completion.ContinueWith(delegate(Task completed, object state)
			{
				IDataflowBlock dataflowBlock = (BroadcastBlock<T>)state;
				dataflowBlock.Fault(completed.Exception);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
			Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this._source.Completion, delegate(object state)
			{
				((BroadcastBlock<T>)state).Complete();
			}, this);
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x060001F1 RID: 497 RVA: 0x000077DC File Offset: 0x000059DC
		public void Complete()
		{
			this.CompleteCore(null, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x060001F2 RID: 498 RVA: 0x000077E7 File Offset: 0x000059E7
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this.CompleteCore(exception, false, false);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007800 File Offset: 0x00005A00
		internal void CompleteCore(Exception exception, bool storeExceptionEvenIfAlreadyCompleting, bool revertProcessingState = false)
		{
			object incomingLock = this.IncomingLock;
			lock (incomingLock)
			{
				if (exception != null && (!this._decliningPermanently || storeExceptionEvenIfAlreadyCompleting))
				{
					this._source.AddException(exception);
				}
				if (revertProcessingState)
				{
					this._boundingState.TaskForInputProcessing = null;
				}
				this._decliningPermanently = true;
				this.CompleteTargetIfPossible();
			}
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" />  instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x060001F4 RID: 500 RVA: 0x00007874 File Offset: 0x00005A74
		public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate a value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x060001F5 RID: 501 RVA: 0x00007883 File Offset: 0x00005A83
		public bool TryReceive(Predicate<T> filter, out T item)
		{
			return this._source.TryReceive(filter, out item);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007892 File Offset: 0x00005A92
		bool IReceivableSourceBlock<!0>.TryReceiveAll(out IList<T> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x000078A0 File Offset: 0x00005AA0
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000078B0 File Offset: 0x00005AB0
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
				if (this._decliningPermanently)
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

		// Token: 0x060001F9 RID: 505 RVA: 0x000079C4 File Offset: 0x00005BC4
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

		// Token: 0x060001FA RID: 506 RVA: 0x00007A28 File Offset: 0x00005C28
		internal void ConsumeAsyncIfNecessary(bool isReplacementReplica = false)
		{
			if (!this._decliningPermanently && this._boundingState.TaskForInputProcessing == null && this._boundingState.PostponedMessages.Count > 0 && this._boundingState.CountIsLessThanBound)
			{
				this._boundingState.TaskForInputProcessing = new Task(delegate(object state)
				{
					((BroadcastBlock<T>)state).ConsumeMessagesLoopCore();
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

		// Token: 0x060001FB RID: 507 RVA: 0x00007B2C File Offset: 0x00005D2C
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

		// Token: 0x060001FC RID: 508 RVA: 0x00007BD0 File Offset: 0x00005DD0
		private bool ConsumeAndStoreOneMessageIfAvailable()
		{
			bool result;
			for (;;)
			{
				object incomingLock = this.IncomingLock;
				KeyValuePair<ISourceBlock<T>, DataflowMessageHeader> keyValuePair;
				lock (incomingLock)
				{
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

		// Token: 0x060001FD RID: 509 RVA: 0x00007CD8 File Offset: 0x00005ED8
		private void CompleteTargetIfPossible()
		{
			if (this._decliningPermanently && !this._completionReserved && (this._boundingState == null || this._boundingState.TaskForInputProcessing == null))
			{
				this._completionReserved = true;
				if (this._boundingState != null && this._boundingState.PostponedMessages.Count > 0)
				{
					Task.Factory.StartNew(delegate(object state)
					{
						BroadcastBlock<T> broadcastBlock = (BroadcastBlock<T>)state;
						List<Exception> list = null;
						if (broadcastBlock._boundingState != null)
						{
							Common.ReleaseAllPostponedMessages<T>(broadcastBlock, broadcastBlock._boundingState.PostponedMessages, ref list);
						}
						if (list != null)
						{
							broadcastBlock._source.AddExceptions(list);
						}
						broadcastBlock._source.Complete();
					}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
					return;
				}
				this._source.Complete();
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00007D76 File Offset: 0x00005F76
		T ISourceBlock<!0>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007D86 File Offset: 0x00005F86
		bool ISourceBlock<!0>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007D95 File Offset: 0x00005F95
		void ISourceBlock<!0>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007DA4 File Offset: 0x00005FA4
		private bool HasValueForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().HasValue;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00007DB6 File Offset: 0x00005FB6
		private T ValueForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().Value;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x06000203 RID: 515 RVA: 0x00007DC8 File Offset: 0x00005FC8
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00007DDB File Offset: 0x00005FDB
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, HasValue={1}, Value={2}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.HasValueForDebugger, this.ValueForDebugger);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00007E0E File Offset: 0x0000600E
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x040000C6 RID: 198
		private readonly BroadcastBlock<T>.BroadcastingSourceCore<T> _source;

		// Token: 0x040000C7 RID: 199
		private readonly BoundingStateWithPostponedAndTask<T> _boundingState;

		// Token: 0x040000C8 RID: 200
		private bool _decliningPermanently;

		// Token: 0x040000C9 RID: 201
		private bool _completionReserved;

		// Token: 0x02000048 RID: 72
		private sealed class DebugView
		{
			// Token: 0x06000207 RID: 519 RVA: 0x00007E26 File Offset: 0x00006026
			public DebugView(BroadcastBlock<T> broadcastBlock)
			{
				this._broadcastBlock = broadcastBlock;
				this._sourceDebuggingInformation = broadcastBlock._source.GetDebuggingInformation();
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x06000208 RID: 520 RVA: 0x00007E46 File Offset: 0x00006046
			public IEnumerable<T> InputQueue
			{
				get
				{
					return this._sourceDebuggingInformation.InputQueue;
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x06000209 RID: 521 RVA: 0x00007E53 File Offset: 0x00006053
			public bool HasValue
			{
				get
				{
					return this._broadcastBlock.HasValueForDebugger;
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x0600020A RID: 522 RVA: 0x00007E60 File Offset: 0x00006060
			public T Value
			{
				get
				{
					return this._broadcastBlock.ValueForDebugger;
				}
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x0600020B RID: 523 RVA: 0x00007E6D File Offset: 0x0000606D
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x0600020C RID: 524 RVA: 0x00007E7A File Offset: 0x0000607A
			public DataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._sourceDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x0600020D RID: 525 RVA: 0x00007E87 File Offset: 0x00006087
			public bool IsDecliningPermanently
			{
				get
				{
					return this._broadcastBlock._decliningPermanently;
				}
			}

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x0600020E RID: 526 RVA: 0x00007E94 File Offset: 0x00006094
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x170000AE RID: 174
			// (get) Token: 0x0600020F RID: 527 RVA: 0x00007EA1 File Offset: 0x000060A1
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._broadcastBlock);
				}
			}

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x06000210 RID: 528 RVA: 0x00007EAE File Offset: 0x000060AE
			public TargetRegistry<T> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x06000211 RID: 529 RVA: 0x00007EBB File Offset: 0x000060BB
			public ITargetBlock<T> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x040000CA RID: 202
			private readonly BroadcastBlock<T> _broadcastBlock;

			// Token: 0x040000CB RID: 203
			private readonly BroadcastBlock<T>.BroadcastingSourceCore<T>.DebuggingInformation _sourceDebuggingInformation;
		}

		// Token: 0x02000049 RID: 73
		[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
		private sealed class BroadcastingSourceCore<TOutput>
		{
			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000212 RID: 530 RVA: 0x00007EC8 File Offset: 0x000060C8
			private object OutgoingLock
			{
				get
				{
					return this._completionTask;
				}
			}

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x06000213 RID: 531 RVA: 0x00007ED0 File Offset: 0x000060D0
			private object ValueLock
			{
				get
				{
					return this._targetRegistry;
				}
			}

			// Token: 0x06000214 RID: 532 RVA: 0x00007ED8 File Offset: 0x000060D8
			internal BroadcastingSourceCore(BroadcastBlock<TOutput> owningSource, Func<TOutput, TOutput> cloningFunction, DataflowBlockOptions dataflowBlockOptions, Action<int> itemsRemovedAction)
			{
				this._owningSource = owningSource;
				this._cloningFunction = cloningFunction;
				this._dataflowBlockOptions = dataflowBlockOptions;
				this._itemsRemovedAction = itemsRemovedAction;
				this._targetRegistry = new TargetRegistry<TOutput>(this._owningSource);
			}

			// Token: 0x06000215 RID: 533 RVA: 0x00007F38 File Offset: 0x00006138
			internal bool TryReceive(Predicate<TOutput> filter, out TOutput item)
			{
				object outgoingLock = this.OutgoingLock;
				TOutput currentMessage;
				bool currentMessageIsValid;
				lock (outgoingLock)
				{
					object valueLock = this.ValueLock;
					lock (valueLock)
					{
						currentMessage = this._currentMessage;
						currentMessageIsValid = this._currentMessageIsValid;
					}
				}
				if (currentMessageIsValid && (filter == null || filter(currentMessage)))
				{
					item = this.CloneItem(currentMessage);
					return true;
				}
				item = default(TOutput);
				return false;
			}

			// Token: 0x06000216 RID: 534 RVA: 0x00007FD4 File Offset: 0x000061D4
			internal bool TryReceiveAll(out IList<TOutput> items)
			{
				TOutput toutput;
				if (this.TryReceive(null, out toutput))
				{
					items = new TOutput[]
					{
						toutput
					};
					return true;
				}
				items = null;
				return false;
			}

			// Token: 0x06000217 RID: 535 RVA: 0x00008004 File Offset: 0x00006204
			internal void AddMessage(TOutput item)
			{
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					if (!this._decliningPermanently)
					{
						this._messages.Enqueue(item);
						if (this._messages.Count == 1)
						{
							this._enableOffering = true;
						}
						this.OfferAsyncIfNecessary(false);
					}
				}
			}

			// Token: 0x06000218 RID: 536 RVA: 0x00008070 File Offset: 0x00006270
			internal void Complete()
			{
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					this._decliningPermanently = true;
					Task.Factory.StartNew(delegate(object state)
					{
						BroadcastBlock<T>.BroadcastingSourceCore<TOutput> broadcastingSourceCore = (BroadcastBlock<T>.BroadcastingSourceCore<TOutput>)state;
						object outgoingLock = broadcastingSourceCore.OutgoingLock;
						lock (outgoingLock)
						{
							object valueLock2 = broadcastingSourceCore.ValueLock;
							lock (valueLock2)
							{
								broadcastingSourceCore.CompleteBlockIfPossible();
							}
						}
					}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
				}
			}

			// Token: 0x06000219 RID: 537 RVA: 0x000080EC File Offset: 0x000062EC
			private TOutput CloneItem(TOutput item)
			{
				if (this._cloningFunction == null)
				{
					return item;
				}
				return this._cloningFunction(item);
			}

			// Token: 0x0600021A RID: 538 RVA: 0x00008104 File Offset: 0x00006304
			private void OfferCurrentMessageToNewTarget(ITargetBlock<TOutput> target)
			{
				object valueLock = this.ValueLock;
				TOutput currentMessage;
				bool currentMessageIsValid;
				lock (valueLock)
				{
					currentMessage = this._currentMessage;
					currentMessageIsValid = this._currentMessageIsValid;
				}
				if (!currentMessageIsValid)
				{
					return;
				}
				bool flag2 = this._cloningFunction != null;
				DataflowMessageStatus dataflowMessageStatus = target.OfferMessage(new DataflowMessageHeader(this._nextMessageId), currentMessage, this._owningSource, flag2);
				if (dataflowMessageStatus == DataflowMessageStatus.Accepted)
				{
					if (!flag2)
					{
						this._targetRegistry.Remove(target, true);
						return;
					}
				}
				else if (dataflowMessageStatus == DataflowMessageStatus.DecliningPermanently)
				{
					this._targetRegistry.Remove(target, false);
				}
			}

			// Token: 0x0600021B RID: 539 RVA: 0x000081A0 File Offset: 0x000063A0
			private bool OfferToTargets()
			{
				DataflowMessageHeader header = default(DataflowMessageHeader);
				TOutput message = default(TOutput);
				int num = 0;
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					if (this._nextMessageReservedFor != null || this._messages.Count <= 0)
					{
						this._enableOffering = false;
						return false;
					}
					if (this._targetRegistry.FirstTargetNode == null)
					{
						while (this._messages.Count > 1)
						{
							this._messages.Dequeue();
							num++;
						}
					}
					message = (this._currentMessage = this._messages.Dequeue());
					num++;
					this._currentMessageIsValid = true;
					long num2 = this._nextMessageId + 1L;
					this._nextMessageId = num2;
					header = new DataflowMessageHeader(num2);
					if (this._messages.Count == 0)
					{
						this._enableOffering = false;
					}
				}
				if (header.IsValid)
				{
					if (this._itemsRemovedAction != null)
					{
						this._itemsRemovedAction(num);
					}
					TargetRegistry<TOutput>.LinkedTargetInfo next;
					for (TargetRegistry<TOutput>.LinkedTargetInfo linkedTargetInfo = this._targetRegistry.FirstTargetNode; linkedTargetInfo != null; linkedTargetInfo = next)
					{
						next = linkedTargetInfo.Next;
						ITargetBlock<TOutput> target = linkedTargetInfo.Target;
						this.OfferMessageToTarget(header, message, target);
					}
				}
				return true;
			}

			// Token: 0x0600021C RID: 540 RVA: 0x000082E4 File Offset: 0x000064E4
			private void OfferMessageToTarget(DataflowMessageHeader header, TOutput message, ITargetBlock<TOutput> target)
			{
				bool flag = this._cloningFunction != null;
				switch (target.OfferMessage(header, message, this._owningSource, flag))
				{
				case DataflowMessageStatus.Accepted:
					if (!flag)
					{
						this._targetRegistry.Remove(target, true);
						return;
					}
					break;
				case DataflowMessageStatus.Declined:
				case DataflowMessageStatus.Postponed:
				case DataflowMessageStatus.NotAvailable:
					break;
				case DataflowMessageStatus.DecliningPermanently:
					this._targetRegistry.Remove(target, false);
					break;
				default:
					return;
				}
			}

			// Token: 0x0600021D RID: 541 RVA: 0x00008344 File Offset: 0x00006544
			private void OfferAsyncIfNecessary(bool isReplacementReplica = false)
			{
				bool flag = this._taskForOutputProcessing != null;
				bool flag2 = this._enableOffering && this._messages.Count > 0;
				if (!flag && flag2 && !this.CanceledOrFaulted)
				{
					this._taskForOutputProcessing = new Task(delegate(object thisSourceCore)
					{
						((BroadcastBlock<T>.BroadcastingSourceCore<TOutput>)thisSourceCore).OfferMessagesLoopCore();
					}, this, Common.GetCreationOptionsForTask(isReplacementReplica));
					DataflowEtwProvider log = DataflowEtwProvider.Log;
					if (log.IsEnabled())
					{
						log.TaskLaunchedForMessageHandling(this._owningSource, this._taskForOutputProcessing, DataflowEtwProvider.TaskLaunchedReason.OfferingOutputMessages, this._messages.Count);
					}
					Exception ex = Common.StartTaskSafe(this._taskForOutputProcessing, this._dataflowBlockOptions.TaskScheduler);
					if (ex != null)
					{
						this.AddException(ex);
						this._decliningPermanently = true;
						this._taskForOutputProcessing = null;
						Task.Factory.StartNew(delegate(object state)
						{
							BroadcastBlock<T>.BroadcastingSourceCore<TOutput> broadcastingSourceCore = (BroadcastBlock<T>.BroadcastingSourceCore<TOutput>)state;
							object outgoingLock = broadcastingSourceCore.OutgoingLock;
							lock (outgoingLock)
							{
								object valueLock = broadcastingSourceCore.ValueLock;
								lock (valueLock)
								{
									broadcastingSourceCore.CompleteBlockIfPossible();
								}
							}
						}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
					}
				}
			}

			// Token: 0x0600021E RID: 542 RVA: 0x00008454 File Offset: 0x00006654
			private void OfferMessagesLoopCore()
			{
				try
				{
					int actualMaxMessagesPerTask = this._dataflowBlockOptions.ActualMaxMessagesPerTask;
					object outgoingLock = this.OutgoingLock;
					lock (outgoingLock)
					{
						int num = 0;
						while (num < actualMaxMessagesPerTask && !this.CanceledOrFaulted && this.OfferToTargets())
						{
							num++;
						}
					}
				}
				catch (Exception exception)
				{
					this._owningSource.CompleteCore(exception, true, false);
				}
				finally
				{
					object outgoingLock2 = this.OutgoingLock;
					lock (outgoingLock2)
					{
						object valueLock = this.ValueLock;
						lock (valueLock)
						{
							this._taskForOutputProcessing = null;
							this.OfferAsyncIfNecessary(true);
							this.CompleteBlockIfPossible();
						}
					}
				}
			}

			// Token: 0x0600021F RID: 543 RVA: 0x00008554 File Offset: 0x00006754
			private void CompleteBlockIfPossible()
			{
				if (!this._completionReserved)
				{
					bool flag = this._taskForOutputProcessing != null;
					bool flag2 = this._decliningPermanently && this._messages.Count == 0;
					bool flag3 = !flag && (flag2 || this.CanceledOrFaulted);
					if (flag3)
					{
						this.CompleteBlockIfPossible_Slow();
					}
				}
			}

			// Token: 0x06000220 RID: 544 RVA: 0x000085AC File Offset: 0x000067AC
			private void CompleteBlockIfPossible_Slow()
			{
				this._completionReserved = true;
				Task.Factory.StartNew(delegate(object thisSourceCore)
				{
					((BroadcastBlock<T>.BroadcastingSourceCore<TOutput>)thisSourceCore).CompleteBlockOncePossible();
				}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}

			// Token: 0x06000221 RID: 545 RVA: 0x000085FC File Offset: 0x000067FC
			private void CompleteBlockOncePossible()
			{
				object outgoingLock = this.OutgoingLock;
				TargetRegistry<TOutput>.LinkedTargetInfo firstTarget;
				List<Exception> exceptions;
				lock (outgoingLock)
				{
					firstTarget = this._targetRegistry.ClearEntryPoints();
					object valueLock = this.ValueLock;
					lock (valueLock)
					{
						this._messages.Clear();
						exceptions = this._exceptions;
						this._exceptions = null;
					}
				}
				if (exceptions != null)
				{
					this._completionTask.TrySetException(exceptions);
				}
				else if (this._dataflowBlockOptions.CancellationToken.IsCancellationRequested)
				{
					this._completionTask.TrySetCanceled();
				}
				else
				{
					this._completionTask.TrySetResult(default(VoidResult));
				}
				this._targetRegistry.PropagateCompletion(firstTarget);
				DataflowEtwProvider log = DataflowEtwProvider.Log;
				if (log.IsEnabled())
				{
					log.DataflowBlockCompleted(this._owningSource);
				}
			}

			// Token: 0x06000222 RID: 546 RVA: 0x000086FC File Offset: 0x000068FC
			internal IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
			{
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				if (linkOptions == null)
				{
					throw new ArgumentNullException("linkOptions");
				}
				object outgoingLock = this.OutgoingLock;
				IDisposable result;
				lock (outgoingLock)
				{
					if (this._completionReserved)
					{
						this.OfferCurrentMessageToNewTarget(target);
						if (linkOptions.PropagateCompletion)
						{
							Common.PropagateCompletionOnceCompleted(this._completionTask.Task, target);
						}
						result = Disposables.Nop;
					}
					else
					{
						this._targetRegistry.Add(ref target, linkOptions);
						this.OfferCurrentMessageToNewTarget(target);
						result = Common.CreateUnlinker<TOutput>(this.OutgoingLock, this._targetRegistry, target);
					}
				}
				return result;
			}

			// Token: 0x06000223 RID: 547 RVA: 0x000087AC File Offset: 0x000069AC
			internal TOutput ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				object outgoingLock = this.OutgoingLock;
				TOutput currentMessage;
				lock (outgoingLock)
				{
					object valueLock = this.ValueLock;
					lock (valueLock)
					{
						if (messageHeader.Id != this._nextMessageId)
						{
							messageConsumed = false;
							return default(TOutput);
						}
						if (this._nextMessageReservedFor == target)
						{
							this._nextMessageReservedFor = null;
							this._enableOffering = true;
						}
						this._targetRegistry.Remove(target, true);
						this.OfferAsyncIfNecessary(false);
						this.CompleteBlockIfPossible();
						currentMessage = this._currentMessage;
					}
				}
				messageConsumed = true;
				return this.CloneItem(currentMessage);
			}

			// Token: 0x06000224 RID: 548 RVA: 0x0000889C File Offset: 0x00006A9C
			internal bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				object outgoingLock = this.OutgoingLock;
				lock (outgoingLock)
				{
					if (this._nextMessageReservedFor == null)
					{
						object valueLock = this.ValueLock;
						lock (valueLock)
						{
							if (messageHeader.Id == this._nextMessageId)
							{
								this._nextMessageReservedFor = target;
								this._enableOffering = false;
								return true;
							}
						}
					}
				}
				return false;
			}

			// Token: 0x06000225 RID: 549 RVA: 0x00008954 File Offset: 0x00006B54
			internal void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
			{
				if (!messageHeader.IsValid)
				{
					throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
				}
				if (target == null)
				{
					throw new ArgumentNullException("target");
				}
				object outgoingLock = this.OutgoingLock;
				lock (outgoingLock)
				{
					if (this._nextMessageReservedFor != target)
					{
						throw new InvalidOperationException(SR.InvalidOperation_MessageNotReservedByTarget);
					}
					object valueLock = this.ValueLock;
					TOutput currentMessage;
					lock (valueLock)
					{
						if (messageHeader.Id != this._nextMessageId)
						{
							throw new InvalidOperationException(SR.InvalidOperation_MessageNotReservedByTarget);
						}
						this._nextMessageReservedFor = null;
						this._enableOffering = true;
						currentMessage = this._currentMessage;
						this.OfferAsyncIfNecessary(false);
					}
					this.OfferMessageToTarget(messageHeader, currentMessage, target);
				}
			}

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x06000226 RID: 550 RVA: 0x00008A34 File Offset: 0x00006C34
			private bool CanceledOrFaulted
			{
				get
				{
					return this._dataflowBlockOptions.CancellationToken.IsCancellationRequested || (Volatile.Read<List<Exception>>(ref this._exceptions) != null && this._decliningPermanently);
				}
			}

			// Token: 0x06000227 RID: 551 RVA: 0x00008A70 File Offset: 0x00006C70
			internal void AddException(Exception exception)
			{
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					Common.AddException(ref this._exceptions, exception, false);
				}
			}

			// Token: 0x06000228 RID: 552 RVA: 0x00008AB8 File Offset: 0x00006CB8
			internal void AddExceptions(List<Exception> exceptions)
			{
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					foreach (Exception exception in exceptions)
					{
						Common.AddException(ref this._exceptions, exception, false);
					}
				}
			}

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x06000229 RID: 553 RVA: 0x00008B34 File Offset: 0x00006D34
			internal Task Completion
			{
				get
				{
					return this._completionTask.Task;
				}
			}

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x0600022A RID: 554 RVA: 0x00008B41 File Offset: 0x00006D41
			internal DataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._dataflowBlockOptions;
				}
			}

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x0600022B RID: 555 RVA: 0x00008B4C File Offset: 0x00006D4C
			private object DebuggerDisplayContent
			{
				get
				{
					IDebuggerDisplay owningSource = this._owningSource;
					return string.Format("Block=\"{0}\"", (owningSource != null) ? owningSource.Content : this._owningSource);
				}
			}

			// Token: 0x0600022C RID: 556 RVA: 0x00008B7B File Offset: 0x00006D7B
			internal BroadcastBlock<T>.BroadcastingSourceCore<TOutput>.DebuggingInformation GetDebuggingInformation()
			{
				return new BroadcastBlock<T>.BroadcastingSourceCore<TOutput>.DebuggingInformation(this);
			}

			// Token: 0x040000CC RID: 204
			private readonly TargetRegistry<TOutput> _targetRegistry;

			// Token: 0x040000CD RID: 205
			private readonly Queue<TOutput> _messages = new Queue<TOutput>();

			// Token: 0x040000CE RID: 206
			private readonly TaskCompletionSource<VoidResult> _completionTask = new TaskCompletionSource<VoidResult>();

			// Token: 0x040000CF RID: 207
			private readonly Action<int> _itemsRemovedAction;

			// Token: 0x040000D0 RID: 208
			private readonly BroadcastBlock<TOutput> _owningSource;

			// Token: 0x040000D1 RID: 209
			private readonly DataflowBlockOptions _dataflowBlockOptions;

			// Token: 0x040000D2 RID: 210
			private readonly Func<TOutput, TOutput> _cloningFunction;

			// Token: 0x040000D3 RID: 211
			private bool _currentMessageIsValid;

			// Token: 0x040000D4 RID: 212
			private TOutput _currentMessage;

			// Token: 0x040000D5 RID: 213
			private ITargetBlock<TOutput> _nextMessageReservedFor;

			// Token: 0x040000D6 RID: 214
			private bool _enableOffering;

			// Token: 0x040000D7 RID: 215
			private bool _decliningPermanently;

			// Token: 0x040000D8 RID: 216
			private Task _taskForOutputProcessing;

			// Token: 0x040000D9 RID: 217
			private List<Exception> _exceptions;

			// Token: 0x040000DA RID: 218
			private long _nextMessageId = 1L;

			// Token: 0x040000DB RID: 219
			private bool _completionReserved;

			// Token: 0x0200004A RID: 74
			internal sealed class DebuggingInformation
			{
				// Token: 0x0600022D RID: 557 RVA: 0x00008B83 File Offset: 0x00006D83
				public DebuggingInformation(BroadcastBlock<T>.BroadcastingSourceCore<TOutput> source)
				{
					this._source = source;
				}

				// Token: 0x170000B7 RID: 183
				// (get) Token: 0x0600022E RID: 558 RVA: 0x00008B92 File Offset: 0x00006D92
				public bool HasValue
				{
					get
					{
						return this._source._currentMessageIsValid;
					}
				}

				// Token: 0x170000B8 RID: 184
				// (get) Token: 0x0600022F RID: 559 RVA: 0x00008B9F File Offset: 0x00006D9F
				public TOutput Value
				{
					get
					{
						return this._source._currentMessage;
					}
				}

				// Token: 0x170000B9 RID: 185
				// (get) Token: 0x06000230 RID: 560 RVA: 0x00008BAC File Offset: 0x00006DAC
				public IEnumerable<TOutput> InputQueue
				{
					get
					{
						return this._source._messages.ToList<TOutput>();
					}
				}

				// Token: 0x170000BA RID: 186
				// (get) Token: 0x06000231 RID: 561 RVA: 0x00008BBE File Offset: 0x00006DBE
				public Task TaskForOutputProcessing
				{
					get
					{
						return this._source._taskForOutputProcessing;
					}
				}

				// Token: 0x170000BB RID: 187
				// (get) Token: 0x06000232 RID: 562 RVA: 0x00008BCB File Offset: 0x00006DCB
				public DataflowBlockOptions DataflowBlockOptions
				{
					get
					{
						return this._source._dataflowBlockOptions;
					}
				}

				// Token: 0x170000BC RID: 188
				// (get) Token: 0x06000233 RID: 563 RVA: 0x00008BD8 File Offset: 0x00006DD8
				public bool IsCompleted
				{
					get
					{
						return this._source.Completion.IsCompleted;
					}
				}

				// Token: 0x170000BD RID: 189
				// (get) Token: 0x06000234 RID: 564 RVA: 0x00008BEA File Offset: 0x00006DEA
				public TargetRegistry<TOutput> LinkedTargets
				{
					get
					{
						return this._source._targetRegistry;
					}
				}

				// Token: 0x170000BE RID: 190
				// (get) Token: 0x06000235 RID: 565 RVA: 0x00008BF7 File Offset: 0x00006DF7
				public ITargetBlock<TOutput> NextMessageReservedFor
				{
					get
					{
						return this._source._nextMessageReservedFor;
					}
				}

				// Token: 0x040000DC RID: 220
				private BroadcastBlock<T>.BroadcastingSourceCore<TOutput> _source;
			}
		}
	}
}
