using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a dataflow block that invokes a provided <see cref="T:System.Func`2" /> delegate for every data element received.</summary>
	/// <typeparam name="TInput">Specifies the type of data received and operated on by this <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" />.</typeparam>
	/// <typeparam name="TOutput">Specifies the type of data output by this <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" />.</typeparam>
	// Token: 0x02000056 RID: 86
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(TransformBlock<, >.DebugView))]
	public sealed class TransformBlock<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<TOutput>, IReceivableSourceBlock<TOutput>, IDebuggerDisplay
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000A03C File Offset: 0x0000823C
		private object ParallelSourceLock
		{
			get
			{
				return this._source;
			}
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" /> with the specified <see cref="T:System.Func`2" />.</summary>
		/// <param name="transform">The function to invoke with each data element received.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.</exception>
		// Token: 0x060002C1 RID: 705 RVA: 0x0000A044 File Offset: 0x00008244
		public TransformBlock(Func<TInput, TOutput> transform) : this(transform, null, ExecutionDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" /> with the specified <see cref="T:System.Func`2" /> and <see cref="T:System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions" />.</summary>
		/// <param name="transform">The function to invoke with each data element received.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.-or-The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x060002C2 RID: 706 RVA: 0x0000A053 File Offset: 0x00008253
		public TransformBlock(Func<TInput, TOutput> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) : this(transform, null, dataflowBlockOptions)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" /> with the specified <see cref="T:System.Func`2" />.</summary>
		/// <param name="transform">The function to invoke with each data element received.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.</exception>
		// Token: 0x060002C3 RID: 707 RVA: 0x0000A05E File Offset: 0x0000825E
		public TransformBlock(Func<TInput, Task<TOutput>> transform) : this(null, transform, ExecutionDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" /> with the specified <see cref="T:System.Func`2" /> and <see cref="T:System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions" />.</summary>
		/// <param name="transform">The function to invoke with each data element received.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.-or-The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x060002C4 RID: 708 RVA: 0x0000A06D File Offset: 0x0000826D
		public TransformBlock(Func<TInput, Task<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) : this(null, transform, dataflowBlockOptions)
		{
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A078 File Offset: 0x00008278
		private TransformBlock(Func<TInput, TOutput> transformSync, Func<TInput, Task<TOutput>> transformAsync, ExecutionDataflowBlockOptions dataflowBlockOptions)
		{
			TransformBlock<TInput, TOutput> <>4__this = this;
			if (transformSync == null && transformAsync == null)
			{
				throw new ArgumentNullException("transform");
			}
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<ISourceBlock<TOutput>, int> itemsRemovedAction = null;
			if (dataflowBlockOptions.BoundedCapacity > 0)
			{
				itemsRemovedAction = delegate(ISourceBlock<TOutput> owningSource, int count)
				{
					((TransformBlock<TInput, TOutput>)owningSource)._target.ChangeBoundingCount(-count);
				};
			}
			this._source = new SourceCore<TOutput>(this, dataflowBlockOptions, delegate(ISourceBlock<TOutput> owningSource)
			{
				((TransformBlock<TInput, TOutput>)owningSource)._target.Complete(null, true, false, false, false);
			}, itemsRemovedAction, null);
			if (dataflowBlockOptions.SupportsParallelExecution && dataflowBlockOptions.EnsureOrdered)
			{
				this._reorderingBuffer = new ReorderingBuffer<TOutput>(this, delegate(object owningSource, TOutput message)
				{
					((TransformBlock<TInput, TOutput>)owningSource)._source.AddMessage(message);
				});
			}
			if (transformSync != null)
			{
				this._target = new TargetCore<TInput>(this, delegate(KeyValuePair<TInput, long> messageWithId)
				{
					<>4__this.ProcessMessage(transformSync, messageWithId);
				}, this._reorderingBuffer, dataflowBlockOptions, TargetCoreOptions.None);
			}
			else
			{
				this._target = new TargetCore<TInput>(this, delegate(KeyValuePair<TInput, long> messageWithId)
				{
					<>4__this.ProcessMessageWithTask(transformAsync, messageWithId);
				}, this._reorderingBuffer, dataflowBlockOptions, TargetCoreOptions.UsesAsyncCompletion);
			}
			this._target.Completion.ContinueWith(delegate(Task completed, object state)
			{
				SourceCore<TOutput> sourceCore = (SourceCore<TOutput>)state;
				if (completed.IsFaulted)
				{
					sourceCore.AddAndUnwrapAggregateException(completed.Exception);
				}
				sourceCore.Complete();
			}, this._source, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None), TaskScheduler.Default);
			this._source.Completion.ContinueWith(delegate(Task completed, object state)
			{
				IDataflowBlock dataflowBlock = (TransformBlock<TInput, TOutput>)state;
				dataflowBlock.Fault(completed.Exception);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.None) | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
			Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this.Completion, delegate(object state)
			{
				((TargetCore<TInput>)state).Complete(null, true, false, false, false);
			}, this._target);
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000A290 File Offset: 0x00008490
		private void ProcessMessage(Func<TInput, TOutput> transform, KeyValuePair<TInput, long> messageWithId)
		{
			TOutput item = default(TOutput);
			bool flag = false;
			try
			{
				item = transform(messageWithId.Key);
				flag = true;
			}
			catch (Exception exception)
			{
				if (!Common.IsCooperativeCancellation(exception))
				{
					throw;
				}
			}
			finally
			{
				if (!flag)
				{
					this._target.ChangeBoundingCount(-1);
				}
				if (this._reorderingBuffer == null)
				{
					if (!flag)
					{
						goto IL_A6;
					}
					if (this._target.DataflowBlockOptions.MaxDegreeOfParallelism == 1)
					{
						this._source.AddMessage(item);
						goto IL_A6;
					}
					object parallelSourceLock = this.ParallelSourceLock;
					lock (parallelSourceLock)
					{
						this._source.AddMessage(item);
						goto IL_A6;
					}
				}
				this._reorderingBuffer.AddItem(messageWithId.Value, item, flag);
				IL_A6:;
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000A36C File Offset: 0x0000856C
		private void ProcessMessageWithTask(Func<TInput, Task<TOutput>> transform, KeyValuePair<TInput, long> messageWithId)
		{
			Task<TOutput> task = null;
			Exception ex = null;
			try
			{
				task = transform(messageWithId.Key);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (task == null)
			{
				if (ex != null && !Common.IsCooperativeCancellation(ex))
				{
					Common.StoreDataflowMessageValueIntoExceptionData<TInput>(ex, messageWithId.Key, false);
					this._target.Complete(ex, true, true, false, false);
				}
				if (this._reorderingBuffer != null)
				{
					this._reorderingBuffer.IgnoreItem(messageWithId.Value);
				}
				this._target.SignalOneAsyncMessageCompleted(-1);
				return;
			}
			task.ContinueWith(delegate(Task<TOutput> completed, object state)
			{
				Tuple<TransformBlock<TInput, TOutput>, KeyValuePair<TInput, long>> tuple = (Tuple<TransformBlock<TInput, TOutput>, KeyValuePair<TInput, long>>)state;
				tuple.Item1.AsyncCompleteProcessMessageWithTask(completed, tuple.Item2);
			}, Tuple.Create<TransformBlock<TInput, TOutput>, KeyValuePair<TInput, long>>(this, messageWithId), CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.ExecuteSynchronously), TaskScheduler.Default);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000A438 File Offset: 0x00008638
		private void AsyncCompleteProcessMessageWithTask(Task<TOutput> completed, KeyValuePair<TInput, long> messageWithId)
		{
			bool isBounded = this._target.IsBounded;
			bool flag = false;
			TOutput item = default(TOutput);
			TaskStatus status = completed.Status;
			if (status != TaskStatus.RanToCompletion)
			{
				if (status == TaskStatus.Faulted)
				{
					AggregateException exception = completed.Exception;
					Common.StoreDataflowMessageValueIntoExceptionData<TInput>(exception, messageWithId.Key, true);
					this._target.Complete(exception, true, true, true, false);
				}
			}
			else
			{
				item = completed.Result;
				flag = true;
			}
			if (!flag && isBounded)
			{
				this._target.ChangeBoundingCount(-1);
			}
			if (this._reorderingBuffer == null)
			{
				if (!flag)
				{
					goto IL_DC;
				}
				if (this._target.DataflowBlockOptions.MaxDegreeOfParallelism == 1)
				{
					this._source.AddMessage(item);
					goto IL_DC;
				}
				object parallelSourceLock = this.ParallelSourceLock;
				lock (parallelSourceLock)
				{
					this._source.AddMessage(item);
					goto IL_DC;
				}
			}
			this._reorderingBuffer.AddItem(messageWithId.Value, item, flag);
			IL_DC:
			this._target.SignalOneAsyncMessageCompleted();
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x060002C9 RID: 713 RVA: 0x0000A53C File Offset: 0x0000873C
		public void Complete()
		{
			this._target.Complete(null, false, false, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x060002CA RID: 714 RVA: 0x0000A54E File Offset: 0x0000874E
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this._target.Complete(exception, true, false, false, false);
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" /> instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x060002CB RID: 715 RVA: 0x0000A56E File Offset: 0x0000876E
		public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x060002CC RID: 716 RVA: 0x0000A57D File Offset: 0x0000877D
		public bool TryReceive(Predicate<TOutput> filter, out TOutput item)
		{
			return this._source.TryReceive(filter, out item);
		}

		/// <summary>Attempts to synchronously receive all available items from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if one or more items could be received; otherwise, false.</returns>
		/// <param name="items">The items received from the source.</param>
		// Token: 0x060002CD RID: 717 RVA: 0x0000A58C File Offset: 0x0000878C
		public bool TryReceiveAll(out IList<TOutput> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000A59A File Offset: 0x0000879A
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		/// <summary>Gets the number of input items waiting to be processed by this block.</summary>
		/// <returns>The number of input items.</returns>
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000A5A7 File Offset: 0x000087A7
		public int InputCount
		{
			get
			{
				return this._target.InputCount;
			}
		}

		/// <summary>Gets the number of output items available to be received from this block.</summary>
		/// <returns>The number of output items.</returns>
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000A5B4 File Offset: 0x000087B4
		public int OutputCount
		{
			get
			{
				return this._source.OutputCount;
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000A5C1 File Offset: 0x000087C1
		DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
		{
			return this._target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000A5D3 File Offset: 0x000087D3
		TOutput ISourceBlock<!1>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000A5E3 File Offset: 0x000087E3
		bool ISourceBlock<!1>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A5F2 File Offset: 0x000087F2
		void ISourceBlock<!1>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000A601 File Offset: 0x00008801
		private int InputCountForDebugger
		{
			get
			{
				return this._target.GetDebuggingInformation().InputCount;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000A613 File Offset: 0x00008813
		private int OutputCountForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().OutputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x060002D7 RID: 727 RVA: 0x0000A625 File Offset: 0x00008825
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000A638 File Offset: 0x00008838
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, InputCount={1}, OutputCount={2}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.InputCountForDebugger, this.OutputCountForDebugger);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000A66B File Offset: 0x0000886B
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x0400010B RID: 267
		private readonly TargetCore<TInput> _target;

		// Token: 0x0400010C RID: 268
		private readonly ReorderingBuffer<TOutput> _reorderingBuffer;

		// Token: 0x0400010D RID: 269
		private readonly SourceCore<TOutput> _source;

		// Token: 0x02000057 RID: 87
		private sealed class DebugView
		{
			// Token: 0x060002DA RID: 730 RVA: 0x0000A673 File Offset: 0x00008873
			public DebugView(TransformBlock<TInput, TOutput> transformBlock)
			{
				this._transformBlock = transformBlock;
				this._targetDebuggingInformation = transformBlock._target.GetDebuggingInformation();
				this._sourceDebuggingInformation = transformBlock._source.GetDebuggingInformation();
			}

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x060002DB RID: 731 RVA: 0x0000A6A4 File Offset: 0x000088A4
			public IEnumerable<TInput> InputQueue
			{
				get
				{
					return this._targetDebuggingInformation.InputQueue;
				}
			}

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x060002DC RID: 732 RVA: 0x0000A6B1 File Offset: 0x000088B1
			public QueuedMap<ISourceBlock<TInput>, DataflowMessageHeader> PostponedMessages
			{
				get
				{
					return this._targetDebuggingInformation.PostponedMessages;
				}
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060002DD RID: 733 RVA: 0x0000A6BE File Offset: 0x000088BE
			public IEnumerable<TOutput> OutputQueue
			{
				get
				{
					return this._sourceDebuggingInformation.OutputQueue;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060002DE RID: 734 RVA: 0x0000A6CB File Offset: 0x000088CB
			public int CurrentDegreeOfParallelism
			{
				get
				{
					return this._targetDebuggingInformation.CurrentDegreeOfParallelism;
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x060002DF RID: 735 RVA: 0x0000A6D8 File Offset: 0x000088D8
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000A6E5 File Offset: 0x000088E5
			public ExecutionDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._targetDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000A6F2 File Offset: 0x000088F2
			public bool IsDecliningPermanently
			{
				get
				{
					return this._targetDebuggingInformation.IsDecliningPermanently;
				}
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000A6FF File Offset: 0x000088FF
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000A70C File Offset: 0x0000890C
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._transformBlock);
				}
			}

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000A719 File Offset: 0x00008919
			public TargetRegistry<TOutput> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x17000109 RID: 265
			// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000A726 File Offset: 0x00008926
			public ITargetBlock<TOutput> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x0400010E RID: 270
			private readonly TransformBlock<TInput, TOutput> _transformBlock;

			// Token: 0x0400010F RID: 271
			private readonly TargetCore<TInput>.DebuggingInformation _targetDebuggingInformation;

			// Token: 0x04000110 RID: 272
			private readonly SourceCore<TOutput>.DebuggingInformation _sourceDebuggingInformation;
		}
	}
}
