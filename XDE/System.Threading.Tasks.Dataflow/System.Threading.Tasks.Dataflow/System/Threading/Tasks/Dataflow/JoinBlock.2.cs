using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a dataflow block that joins across multiple dataflow sources, which are not necessarily of the same type, waiting for one item to arrive for each type before they’re all released together as a tuple that contains one item per type.</summary>
	/// <typeparam name="T1">Specifies the type of data accepted by the block's first target.</typeparam>
	/// <typeparam name="T2">Specifies the type of data accepted by the block's second target.</typeparam>
	/// <typeparam name="T3">Specifies the type of data accepted by the block's third target.</typeparam>
	// Token: 0x02000053 RID: 83
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(JoinBlock<, , >.DebugView))]
	public sealed class JoinBlock<T1, T2, T3> : IReceivableSourceBlock<Tuple<T1, T2, T3>>, ISourceBlock<Tuple<T1, T2, T3>>, IDataflowBlock, IDebuggerDisplay
	{
		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.JoinBlock`3" />.</summary>
		// Token: 0x06000296 RID: 662 RVA: 0x00009B3C File Offset: 0x00007D3C
		public JoinBlock() : this(GroupingDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.JoinBlock`3" />.</summary>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.JoinBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x06000297 RID: 663 RVA: 0x00009B4C File Offset: 0x00007D4C
		public JoinBlock(GroupingDataflowBlockOptions dataflowBlockOptions)
		{
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<ISourceBlock<Tuple<T1, T2, T3>>, int> itemsRemovedAction = null;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				itemsRemovedAction = delegate(ISourceBlock<Tuple<T1, T2, T3>> owningSource, int count)
				{
					((JoinBlock<T1, T2, T3>)owningSource)._sharedResources.OnItemsRemoved(count);
				};
			}
			this._source = new SourceCore<Tuple<T1, T2, T3>>(this, dataflowBlockOptions, delegate(ISourceBlock<Tuple<T1, T2, T3>> owningSource)
			{
				((JoinBlock<T1, T2, T3>)owningSource)._sharedResources.CompleteEachTarget();
			}, itemsRemovedAction, null);
			JoinBlockTargetBase[] array = new JoinBlockTargetBase[3];
			this._sharedResources = new JoinBlockTargetSharedResources(this, array, delegate()
			{
				this._source.AddMessage(Tuple.Create<T1, T2, T3>(this._target1.GetOneMessage(), this._target2.GetOneMessage(), this._target3.GetOneMessage()));
			}, delegate(Exception exception)
			{
				Volatile.Write(ref this._sharedResources._hasExceptions, true);
				this._source.AddException(exception);
			}, dataflowBlockOptions);
			array[0] = (this._target1 = new JoinBlockTarget<T1>(this._sharedResources));
			array[1] = (this._target2 = new JoinBlockTarget<T2>(this._sharedResources));
			array[2] = (this._target3 = new JoinBlockTarget<T3>(this._sharedResources));
			Task.Factory.ContinueWhenAll(new Task[]
			{
				this._target1.CompletionTaskInternal,
				this._target2.CompletionTaskInternal,
				this._target3.CompletionTaskInternal
			}, delegate(Task[] _)
			{
				this._source.Complete();
			}, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
			this._source.Completion.ContinueWith(delegate(Task completed, object state)
			{
				IDataflowBlock dataflowBlock = (JoinBlock<T1, T2, T3>)state;
				dataflowBlock.Fault(completed.Exception);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
			Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this._source.Completion, delegate(object state)
			{
				((JoinBlock<T1, T2, T3>)state)._sharedResources.CompleteEachTarget();
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
		// Token: 0x06000298 RID: 664 RVA: 0x00009D2E File Offset: 0x00007F2E
		public IDisposable LinkTo(ITargetBlock<Tuple<T1, T2, T3>> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x06000299 RID: 665 RVA: 0x00009D3D File Offset: 0x00007F3D
		public bool TryReceive(Predicate<Tuple<T1, T2, T3>> filter, out Tuple<T1, T2, T3> item)
		{
			return this._source.TryReceive(filter, out item);
		}

		/// <summary>Attempts to synchronously receive all available items from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if one or more items could be received; otherwise, false.</returns>
		/// <param name="items">The items received from the source.</param>
		// Token: 0x0600029A RID: 666 RVA: 0x00009D4C File Offset: 0x00007F4C
		public bool TryReceiveAll(out IList<Tuple<T1, T2, T3>> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets the number of output items available to be received from this block.</summary>
		/// <returns>The number of output items.</returns>
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00009D5A File Offset: 0x00007F5A
		public int OutputCount
		{
			get
			{
				return this._source.OutputCount;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00009D67 File Offset: 0x00007F67
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x0600029D RID: 669 RVA: 0x00009D74 File Offset: 0x00007F74
		public void Complete()
		{
			this._target1.CompleteCore(null, false, false);
			this._target2.CompleteCore(null, false, false);
			this._target3.CompleteCore(null, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x0600029E RID: 670 RVA: 0x00009DA0 File Offset: 0x00007FA0
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
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00009E14 File Offset: 0x00008014
		public ITargetBlock<T1> Target1
		{
			get
			{
				return this._target1;
			}
		}

		/// <summary>Gets a target that may be used to offer messages of the second type.</summary>
		/// <returns>The target.</returns>
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00009E1C File Offset: 0x0000801C
		public ITargetBlock<T2> Target2
		{
			get
			{
				return this._target2;
			}
		}

		/// <summary>Gets a target that may be used to offer messages of the third type.</summary>
		/// <returns>The target.</returns>
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00009E24 File Offset: 0x00008024
		public ITargetBlock<T3> Target3
		{
			get
			{
				return this._target3;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00009E2C File Offset: 0x0000802C
		Tuple<T1, T2, T3> ISourceBlock<Tuple<!0, !1, !2>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2, T3>> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00009E3C File Offset: 0x0000803C
		bool ISourceBlock<Tuple<!0, !1, !2>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2, T3>> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009E4B File Offset: 0x0000804B
		void ISourceBlock<Tuple<!0, !1, !2>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2, T3>> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00009E5A File Offset: 0x0000805A
		private int OutputCountForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().OutputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x060002A6 RID: 678 RVA: 0x00009E6C File Offset: 0x0000806C
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00009E7F File Offset: 0x0000807F
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0} OutputCount={1}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.OutputCountForDebugger);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00009EA7 File Offset: 0x000080A7
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x040000FF RID: 255
		private readonly JoinBlockTargetSharedResources _sharedResources;

		// Token: 0x04000100 RID: 256
		private readonly SourceCore<Tuple<T1, T2, T3>> _source;

		// Token: 0x04000101 RID: 257
		private readonly JoinBlockTarget<T1> _target1;

		// Token: 0x04000102 RID: 258
		private readonly JoinBlockTarget<T2> _target2;

		// Token: 0x04000103 RID: 259
		private readonly JoinBlockTarget<T3> _target3;

		// Token: 0x02000054 RID: 84
		private sealed class DebugView
		{
			// Token: 0x060002AC RID: 684 RVA: 0x00009F0E File Offset: 0x0000810E
			public DebugView(JoinBlock<T1, T2, T3> joinBlock)
			{
				this._joinBlock = joinBlock;
				this._sourceDebuggingInformation = joinBlock._source.GetDebuggingInformation();
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x060002AD RID: 685 RVA: 0x00009F2E File Offset: 0x0000812E
			public IEnumerable<Tuple<T1, T2, T3>> OutputQueue
			{
				get
				{
					return this._sourceDebuggingInformation.OutputQueue;
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x060002AE RID: 686 RVA: 0x00009F3B File Offset: 0x0000813B
			public long JoinsCreated
			{
				get
				{
					return this._joinBlock._sharedResources._joinsCreated;
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x060002AF RID: 687 RVA: 0x00009F4D File Offset: 0x0000814D
			public Task TaskForInputProcessing
			{
				get
				{
					return this._joinBlock._sharedResources._taskForInputProcessing;
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x060002B0 RID: 688 RVA: 0x00009F5F File Offset: 0x0000815F
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x060002B1 RID: 689 RVA: 0x00009F6C File Offset: 0x0000816C
			public GroupingDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return (GroupingDataflowBlockOptions)this._sourceDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x060002B2 RID: 690 RVA: 0x00009F7E File Offset: 0x0000817E
			public bool IsDecliningPermanently
			{
				get
				{
					return this._joinBlock._sharedResources._decliningPermanently;
				}
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x060002B3 RID: 691 RVA: 0x00009F90 File Offset: 0x00008190
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x060002B4 RID: 692 RVA: 0x00009F9D File Offset: 0x0000819D
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._joinBlock);
				}
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x060002B5 RID: 693 RVA: 0x00009FAA File Offset: 0x000081AA
			public ITargetBlock<T1> Target1
			{
				get
				{
					return this._joinBlock._target1;
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060002B6 RID: 694 RVA: 0x00009FB7 File Offset: 0x000081B7
			public ITargetBlock<T2> Target2
			{
				get
				{
					return this._joinBlock._target2;
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x060002B7 RID: 695 RVA: 0x00009FC4 File Offset: 0x000081C4
			public ITargetBlock<T3> Target3
			{
				get
				{
					return this._joinBlock._target3;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060002B8 RID: 696 RVA: 0x00009FD1 File Offset: 0x000081D1
			public TargetRegistry<Tuple<T1, T2, T3>> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060002B9 RID: 697 RVA: 0x00009FDE File Offset: 0x000081DE
			public ITargetBlock<Tuple<T1, T2, T3>> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x04000104 RID: 260
			private readonly JoinBlock<T1, T2, T3> _joinBlock;

			// Token: 0x04000105 RID: 261
			private readonly SourceCore<Tuple<T1, T2, T3>>.DebuggingInformation _sourceDebuggingInformation;
		}
	}
}
