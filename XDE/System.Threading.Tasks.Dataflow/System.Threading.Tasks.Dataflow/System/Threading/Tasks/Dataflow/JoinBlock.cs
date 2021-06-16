using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a dataflow block that joins across multiple dataflow sources, not necessarily of the same type, waiting for one item to arrive for each type before they’re all released together as a tuple consisting of one item per type.</summary>
	/// <typeparam name="T1">Specifies the type of data accepted by the block's first target.</typeparam>
	/// <typeparam name="T2">Specifies the type of data accepted by the block's second target.</typeparam>
	// Token: 0x02000050 RID: 80
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(JoinBlock<, >.DebugView))]
	public sealed class JoinBlock<T1, T2> : IReceivableSourceBlock<Tuple<T1, T2>>, ISourceBlock<Tuple<T1, T2>>, IDataflowBlock, IDebuggerDisplay
	{
		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.JoinBlock`2" />.</summary>
		// Token: 0x0600026E RID: 622 RVA: 0x00009693 File Offset: 0x00007893
		public JoinBlock() : this(GroupingDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.JoinBlock`2" />.</summary>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.JoinBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600026F RID: 623 RVA: 0x000096A0 File Offset: 0x000078A0
		public JoinBlock(GroupingDataflowBlockOptions dataflowBlockOptions)
		{
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<ISourceBlock<Tuple<T1, T2>>, int> itemsRemovedAction = null;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				itemsRemovedAction = delegate(ISourceBlock<Tuple<T1, T2>> owningSource, int count)
				{
					((JoinBlock<T1, T2>)owningSource)._sharedResources.OnItemsRemoved(count);
				};
			}
			this._source = new SourceCore<Tuple<T1, T2>>(this, dataflowBlockOptions, delegate(ISourceBlock<Tuple<T1, T2>> owningSource)
			{
				((JoinBlock<T1, T2>)owningSource)._sharedResources.CompleteEachTarget();
			}, itemsRemovedAction, null);
			JoinBlockTargetBase[] array = new JoinBlockTargetBase[2];
			this._sharedResources = new JoinBlockTargetSharedResources(this, array, delegate()
			{
				this._source.AddMessage(Tuple.Create<T1, T2>(this._target1.GetOneMessage(), this._target2.GetOneMessage()));
			}, delegate(Exception exception)
			{
				Volatile.Write(ref this._sharedResources._hasExceptions, true);
				this._source.AddException(exception);
			}, dataflowBlockOptions);
			array[0] = (this._target1 = new JoinBlockTarget<T1>(this._sharedResources));
			array[1] = (this._target2 = new JoinBlockTarget<T2>(this._sharedResources));
			Task.Factory.ContinueWhenAll(new Task[]
			{
				this._target1.CompletionTaskInternal,
				this._target2.CompletionTaskInternal
			}, delegate(Task[] _)
			{
				this._source.Complete();
			}, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
			this._source.Completion.ContinueWith(delegate(Task completed, object state)
			{
				IDataflowBlock dataflowBlock = (JoinBlock<T1, T2>)state;
				dataflowBlock.Fault(completed.Exception);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
			Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this._source.Completion, delegate(object state)
			{
				((JoinBlock<T1, T2>)state)._sharedResources.CompleteEachTarget();
			}, this);
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" /> instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x06000270 RID: 624 RVA: 0x0000985B File Offset: 0x00007A5B
		public IDisposable LinkTo(ITargetBlock<Tuple<T1, T2>> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x06000271 RID: 625 RVA: 0x0000986A File Offset: 0x00007A6A
		public bool TryReceive(Predicate<Tuple<T1, T2>> filter, out Tuple<T1, T2> item)
		{
			return this._source.TryReceive(filter, out item);
		}

		/// <summary>Attempts to synchronously receive all available items from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if one or more items could be received; otherwise, false.</returns>
		/// <param name="items">The items received from the source.</param>
		// Token: 0x06000272 RID: 626 RVA: 0x00009879 File Offset: 0x00007A79
		public bool TryReceiveAll(out IList<Tuple<T1, T2>> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets the number of output items available to be received from this block.</summary>
		/// <returns>The number of output items.</returns>
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00009887 File Offset: 0x00007A87
		public int OutputCount
		{
			get
			{
				return this._source.OutputCount;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00009894 File Offset: 0x00007A94
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x06000275 RID: 629 RVA: 0x000098A1 File Offset: 0x00007AA1
		public void Complete()
		{
			this._target1.CompleteCore(null, false, false);
			this._target2.CompleteCore(null, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x06000276 RID: 630 RVA: 0x000098C0 File Offset: 0x00007AC0
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			object incomingLock = this._sharedResources.IncomingLock;
			lock (incomingLock)
			{
				if (!this._sharedResources._decliningPermanently)
				{
					this._sharedResources._exceptionAction(exception);
				}
			}
			this.Complete();
		}

		/// <summary>Gets a target that may be used to offer messages of the first type.</summary>
		/// <returns>The target.</returns>
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00009934 File Offset: 0x00007B34
		public ITargetBlock<T1> Target1
		{
			get
			{
				return this._target1;
			}
		}

		/// <summary>Gets a target that may be used to offer messages of the second type.</summary>
		/// <returns>The target.</returns>
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000993C File Offset: 0x00007B3C
		public ITargetBlock<T2> Target2
		{
			get
			{
				return this._target2;
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009944 File Offset: 0x00007B44
		Tuple<T1, T2> ISourceBlock<Tuple<!0, !1>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2>> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00009954 File Offset: 0x00007B54
		bool ISourceBlock<Tuple<!0, !1>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2>> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00009963 File Offset: 0x00007B63
		void ISourceBlock<Tuple<!0, !1>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2>> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600027C RID: 636 RVA: 0x00009972 File Offset: 0x00007B72
		private int OutputCountForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().OutputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x0600027D RID: 637 RVA: 0x00009984 File Offset: 0x00007B84
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00009997 File Offset: 0x00007B97
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, OutputCount={1}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.OutputCountForDebugger);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000099BF File Offset: 0x00007BBF
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x040000F4 RID: 244
		private readonly JoinBlockTargetSharedResources _sharedResources;

		// Token: 0x040000F5 RID: 245
		private readonly SourceCore<Tuple<T1, T2>> _source;

		// Token: 0x040000F6 RID: 246
		private readonly JoinBlockTarget<T1> _target1;

		// Token: 0x040000F7 RID: 247
		private readonly JoinBlockTarget<T2> _target2;

		// Token: 0x02000051 RID: 81
		private sealed class DebugView
		{
			// Token: 0x06000283 RID: 643 RVA: 0x00009A1B File Offset: 0x00007C1B
			public DebugView(JoinBlock<T1, T2> joinBlock)
			{
				this._joinBlock = joinBlock;
				this._sourceDebuggingInformation = joinBlock._source.GetDebuggingInformation();
			}

			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x06000284 RID: 644 RVA: 0x00009A3B File Offset: 0x00007C3B
			public IEnumerable<Tuple<T1, T2>> OutputQueue
			{
				get
				{
					return this._sourceDebuggingInformation.OutputQueue;
				}
			}

			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x06000285 RID: 645 RVA: 0x00009A48 File Offset: 0x00007C48
			public long JoinsCreated
			{
				get
				{
					return this._joinBlock._sharedResources._joinsCreated;
				}
			}

			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x06000286 RID: 646 RVA: 0x00009A5A File Offset: 0x00007C5A
			public Task TaskForInputProcessing
			{
				get
				{
					return this._joinBlock._sharedResources._taskForInputProcessing;
				}
			}

			// Token: 0x170000D9 RID: 217
			// (get) Token: 0x06000287 RID: 647 RVA: 0x00009A6C File Offset: 0x00007C6C
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x06000288 RID: 648 RVA: 0x00009A79 File Offset: 0x00007C79
			public GroupingDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return (GroupingDataflowBlockOptions)this._sourceDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x170000DB RID: 219
			// (get) Token: 0x06000289 RID: 649 RVA: 0x00009A8B File Offset: 0x00007C8B
			public bool IsDecliningPermanently
			{
				get
				{
					return this._joinBlock._sharedResources._decliningPermanently;
				}
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x0600028A RID: 650 RVA: 0x00009A9D File Offset: 0x00007C9D
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x0600028B RID: 651 RVA: 0x00009AAA File Offset: 0x00007CAA
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._joinBlock);
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x0600028C RID: 652 RVA: 0x00009AB7 File Offset: 0x00007CB7
			public ITargetBlock<T1> Target1
			{
				get
				{
					return this._joinBlock._target1;
				}
			}

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x0600028D RID: 653 RVA: 0x00009AC4 File Offset: 0x00007CC4
			public ITargetBlock<T2> Target2
			{
				get
				{
					return this._joinBlock._target2;
				}
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x0600028E RID: 654 RVA: 0x00009AD1 File Offset: 0x00007CD1
			public TargetRegistry<Tuple<T1, T2>> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x0600028F RID: 655 RVA: 0x00009ADE File Offset: 0x00007CDE
			public ITargetBlock<Tuple<T1, T2>> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x040000F8 RID: 248
			private readonly JoinBlock<T1, T2> _joinBlock;

			// Token: 0x040000F9 RID: 249
			private readonly SourceCore<Tuple<T1, T2>>.DebuggingInformation _sourceDebuggingInformation;
		}
	}
}
