using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a dataflow block that invokes a provided <see cref="T:System.Func`2" /> delegate for every data element received.</summary>
	/// <typeparam name="TInput">Specifies the type of data received and operated on by this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</typeparam>
	/// <typeparam name="TOutput">Specifies the type of data output by this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</typeparam>
	// Token: 0x0200005A RID: 90
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	[DebuggerTypeProxy(typeof(TransformManyBlock<, >.DebugView))]
	public sealed class TransformManyBlock<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<!1>, IReceivableSourceBlock<TOutput>, IDebuggerDisplay
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000A81E File Offset: 0x00008A1E
		private object ParallelSourceLock
		{
			get
			{
				return this._source;
			}
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" /> with the specified function.</summary>
		/// <param name="transform">The function to invoke with each data element received. All of the data from the returned <see cref="T:System.Collections.Generic.IEnumerable`1" /> will be made available as output from this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.</exception>
		// Token: 0x060002F3 RID: 755 RVA: 0x0000A826 File Offset: 0x00008A26
		public TransformManyBlock(Func<TInput, IEnumerable<TOutput>> transform) : this(transform, null, ExecutionDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" /> with the specified function and <see cref="T:System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions" />.</summary>
		/// <param name="transform">The function to invoke with each data element received. All of the data from the returned in the <see cref="T:System.Collections.Generic.IEnumerable`1" /> will be made available as output from this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.-or-The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x060002F4 RID: 756 RVA: 0x0000A835 File Offset: 0x00008A35
		public TransformManyBlock(Func<TInput, IEnumerable<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) : this(transform, null, dataflowBlockOptions)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" /> with the specified function.</summary>
		/// <param name="transform">The function to invoke with each data element received. All of the data asynchronously returned in the <see cref="T:System.Collections.Generic.IEnumerable`1" /> will be made available as output from this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.</exception>
		// Token: 0x060002F5 RID: 757 RVA: 0x0000A840 File Offset: 0x00008A40
		public TransformManyBlock(Func<TInput, Task<IEnumerable<TOutput>>> transform) : this(null, transform, ExecutionDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" /> with the specified function and <see cref="T:System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions" />.</summary>
		/// <param name="transform">The function to invoke with each data element received. All of the data asynchronously returned in the <see cref="T:System.Collections.Generic.IEnumerable`1" /> will be made available as output from this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.TransformManyBlock`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="transform" /> is null.-or-The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x060002F6 RID: 758 RVA: 0x0000A84F File Offset: 0x00008A4F
		public TransformManyBlock(Func<TInput, Task<IEnumerable<TOutput>>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) : this(null, transform, dataflowBlockOptions)
		{
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A85C File Offset: 0x00008A5C
		private TransformManyBlock(Func<TInput, IEnumerable<TOutput>> transformSync, Func<TInput, Task<IEnumerable<TOutput>>> transformAsync, ExecutionDataflowBlockOptions dataflowBlockOptions)
		{
			TransformManyBlock<TInput, TOutput> <>4__this = this;
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
					((TransformManyBlock<TInput, TOutput>)owningSource)._target.ChangeBoundingCount(-count);
				};
			}
			this._source = new SourceCore<TOutput>(this, dataflowBlockOptions, delegate(ISourceBlock<TOutput> owningSource)
			{
				((TransformManyBlock<TInput, TOutput>)owningSource)._target.Complete(null, true, false, false, false);
			}, itemsRemovedAction, null);
			if (dataflowBlockOptions.SupportsParallelExecution && dataflowBlockOptions.EnsureOrdered)
			{
				this._reorderingBuffer = new ReorderingBuffer<IEnumerable<TOutput>>(this, delegate(object source, IEnumerable<TOutput> messages)
				{
					((TransformManyBlock<TInput, TOutput>)source)._source.AddMessages(messages);
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
				IDataflowBlock dataflowBlock = (TransformManyBlock<TInput, TOutput>)state;
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

		// Token: 0x060002F8 RID: 760 RVA: 0x0000AA74 File Offset: 0x00008C74
		private void ProcessMessage(Func<TInput, IEnumerable<TOutput>> transformFunction, KeyValuePair<TInput, long> messageWithId)
		{
			bool flag = false;
			try
			{
				IEnumerable<TOutput> outputItems = transformFunction(messageWithId.Key);
				flag = true;
				this.StoreOutputItems(messageWithId, outputItems);
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
					this.StoreOutputItems(messageWithId, null);
				}
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		private void ProcessMessageWithTask(Func<TInput, Task<IEnumerable<TOutput>>> function, KeyValuePair<TInput, long> messageWithId)
		{
			Task<IEnumerable<TOutput>> task = null;
			Exception ex = null;
			try
			{
				task = function(messageWithId.Key);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (task != null)
			{
				task.ContinueWith(delegate(Task<IEnumerable<TOutput>> completed, object state)
				{
					Tuple<TransformManyBlock<TInput, TOutput>, KeyValuePair<TInput, long>> tuple = (Tuple<TransformManyBlock<TInput, TOutput>, KeyValuePair<TInput, long>>)state;
					tuple.Item1.AsyncCompleteProcessMessageWithTask(completed, tuple.Item2);
				}, Tuple.Create<TransformManyBlock<TInput, TOutput>, KeyValuePair<TInput, long>>(this, messageWithId), CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.ExecuteSynchronously), this._source.DataflowBlockOptions.TaskScheduler);
				return;
			}
			if (ex != null && !Common.IsCooperativeCancellation(ex))
			{
				Common.StoreDataflowMessageValueIntoExceptionData<TInput>(ex, messageWithId.Key, false);
				this._target.Complete(ex, true, true, false, false);
			}
			if (this._reorderingBuffer != null)
			{
				this.StoreOutputItems(messageWithId, null);
				this._target.SignalOneAsyncMessageCompleted();
				return;
			}
			this._target.SignalOneAsyncMessageCompleted(-1);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000ABAC File Offset: 0x00008DAC
		private void AsyncCompleteProcessMessageWithTask(Task<IEnumerable<TOutput>> completed, KeyValuePair<TInput, long> messageWithId)
		{
			switch (completed.Status)
			{
			case TaskStatus.RanToCompletion:
			{
				IEnumerable<TOutput> result = completed.Result;
				try
				{
					this.StoreOutputItems(messageWithId, result);
					goto IL_84;
				}
				catch (Exception ex)
				{
					if (!Common.IsCooperativeCancellation(ex))
					{
						Common.StoreDataflowMessageValueIntoExceptionData<TInput>(ex, messageWithId.Key, false);
						this._target.Complete(ex, true, true, false, false);
					}
					goto IL_84;
				}
				break;
			}
			case TaskStatus.Canceled:
				goto IL_7C;
			case TaskStatus.Faulted:
				break;
			default:
				goto IL_84;
			}
			AggregateException exception = completed.Exception;
			Common.StoreDataflowMessageValueIntoExceptionData<TInput>(exception, messageWithId.Key, true);
			this._target.Complete(exception, true, true, true, false);
			IL_7C:
			this.StoreOutputItems(messageWithId, null);
			IL_84:
			this._target.SignalOneAsyncMessageCompleted();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000AC58 File Offset: 0x00008E58
		private void StoreOutputItems(KeyValuePair<TInput, long> messageWithId, IEnumerable<TOutput> outputItems)
		{
			if (this._reorderingBuffer != null)
			{
				this.StoreOutputItemsReordered(messageWithId.Value, outputItems);
				return;
			}
			if (outputItems == null)
			{
				if (this._target.IsBounded)
				{
					this._target.ChangeBoundingCount(-1);
				}
				return;
			}
			if (outputItems is TOutput[] || outputItems is List<TOutput>)
			{
				this.StoreOutputItemsNonReorderedAtomic(outputItems);
				return;
			}
			this.StoreOutputItemsNonReorderedWithIteration(outputItems);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		private void StoreOutputItemsReordered(long id, IEnumerable<TOutput> item)
		{
			TargetCore<TInput> target = this._target;
			bool isBounded = target.IsBounded;
			if (item == null)
			{
				this._reorderingBuffer.AddItem(id, null, false);
				if (isBounded)
				{
					target.ChangeBoundingCount(-1);
				}
				return;
			}
			IList<TOutput> list = item as TOutput[];
			if (list == null)
			{
				list = (item as List<TOutput>);
			}
			if (list != null && isBounded)
			{
				this.UpdateBoundingCountWithOutputCount(list.Count);
			}
			bool? flag = this._reorderingBuffer.AddItemIfNextAndTrusted(id, list, list != null);
			if (flag == null)
			{
				return;
			}
			bool value = flag.Value;
			List<TOutput> list2 = null;
			try
			{
				if (value)
				{
					this.StoreOutputItemsNonReorderedWithIteration(item);
				}
				else if (list != null)
				{
					list2 = list.ToList<TOutput>();
				}
				else
				{
					int count = 0;
					try
					{
						list2 = item.ToList<TOutput>();
						count = list2.Count;
					}
					finally
					{
						if (isBounded)
						{
							this.UpdateBoundingCountWithOutputCount(count);
						}
					}
				}
			}
			finally
			{
				this._reorderingBuffer.AddItem(id, list2, list2 != null);
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		private void StoreOutputItemsNonReorderedAtomic(IEnumerable<TOutput> outputItems)
		{
			if (this._target.IsBounded)
			{
				this.UpdateBoundingCountWithOutputCount(((ICollection<TOutput>)outputItems).Count);
			}
			if (this._target.DataflowBlockOptions.MaxDegreeOfParallelism == 1)
			{
				this._source.AddMessages(outputItems);
				return;
			}
			object parallelSourceLock = this.ParallelSourceLock;
			lock (parallelSourceLock)
			{
				this._source.AddMessages(outputItems);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000AE2C File Offset: 0x0000902C
		private void StoreOutputItemsNonReorderedWithIteration(IEnumerable<TOutput> outputItems)
		{
			bool flag = this._target.DataflowBlockOptions.MaxDegreeOfParallelism == 1 || this._reorderingBuffer != null;
			if (this._target.IsBounded)
			{
				bool flag2 = false;
				try
				{
					using (IEnumerator<TOutput> enumerator = outputItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TOutput item = enumerator.Current;
							if (flag2)
							{
								this._target.ChangeBoundingCount(1);
							}
							else
							{
								flag2 = true;
							}
							if (flag)
							{
								this._source.AddMessage(item);
							}
							else
							{
								object parallelSourceLock = this.ParallelSourceLock;
								lock (parallelSourceLock)
								{
									this._source.AddMessage(item);
								}
							}
						}
						return;
					}
				}
				finally
				{
					if (!flag2)
					{
						this._target.ChangeBoundingCount(-1);
					}
				}
			}
			if (flag)
			{
				using (IEnumerator<TOutput> enumerator2 = outputItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TOutput item2 = enumerator2.Current;
						this._source.AddMessage(item2);
					}
					return;
				}
			}
			foreach (TOutput item3 in outputItems)
			{
				object parallelSourceLock2 = this.ParallelSourceLock;
				lock (parallelSourceLock2)
				{
					this._source.AddMessage(item3);
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000AFD4 File Offset: 0x000091D4
		private void UpdateBoundingCountWithOutputCount(int count)
		{
			if (count > 1)
			{
				this._target.ChangeBoundingCount(count - 1);
				return;
			}
			if (count == 0)
			{
				this._target.ChangeBoundingCount(-1);
			}
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x06000300 RID: 768 RVA: 0x0000AFF8 File Offset: 0x000091F8
		public void Complete()
		{
			this._target.Complete(null, false, false, false, false);
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x06000301 RID: 769 RVA: 0x0000B00A File Offset: 0x0000920A
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
		// Token: 0x06000302 RID: 770 RVA: 0x0000B02A File Offset: 0x0000922A
		public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
		{
			return this._source.LinkTo(target, linkOptions);
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x06000303 RID: 771 RVA: 0x0000B039 File Offset: 0x00009239
		public bool TryReceive(Predicate<TOutput> filter, out TOutput item)
		{
			return this._source.TryReceive(filter, out item);
		}

		/// <summary>Attempts to synchronously receive all available items from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if one or more items could be received; otherwise, false.</returns>
		/// <param name="items">The items received from the source.</param>
		// Token: 0x06000304 RID: 772 RVA: 0x0000B048 File Offset: 0x00009248
		public bool TryReceiveAll(out IList<TOutput> items)
		{
			return this._source.TryReceiveAll(out items);
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000B056 File Offset: 0x00009256
		public Task Completion
		{
			get
			{
				return this._source.Completion;
			}
		}

		/// <summary>Gets the number of input items waiting to be processed by this block.</summary>
		/// <returns>The number of input items.</returns>
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000B063 File Offset: 0x00009263
		public int InputCount
		{
			get
			{
				return this._target.InputCount;
			}
		}

		/// <summary>Gets the number of output items available to be received from this block.</summary>
		/// <returns>The number of output items.</returns>
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000B070 File Offset: 0x00009270
		public int OutputCount
		{
			get
			{
				return this._source.OutputCount;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000B07D File Offset: 0x0000927D
		DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
		{
			return this._target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000B08F File Offset: 0x0000928F
		TOutput ISourceBlock<!1>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
		{
			return this._source.ConsumeMessage(messageHeader, target, out messageConsumed);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000B09F File Offset: 0x0000929F
		bool ISourceBlock<!1>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
		{
			return this._source.ReserveMessage(messageHeader, target);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B0AE File Offset: 0x000092AE
		void ISourceBlock<!1>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
		{
			this._source.ReleaseReservation(messageHeader, target);
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000B0BD File Offset: 0x000092BD
		private int InputCountForDebugger
		{
			get
			{
				return this._target.GetDebuggingInformation().InputCount;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000B0CF File Offset: 0x000092CF
		private int OutputCountForDebugger
		{
			get
			{
				return this._source.GetDebuggingInformation().OutputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x0600030E RID: 782 RVA: 0x0000B0E1 File Offset: 0x000092E1
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._source.DataflowBlockOptions);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000B0F4 File Offset: 0x000092F4
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, InputCount={1}, OutputCount={2}", Common.GetNameForDebugger(this, this._source.DataflowBlockOptions), this.InputCountForDebugger, this.OutputCountForDebugger);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000B127 File Offset: 0x00009327
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x0400011C RID: 284
		private readonly TargetCore<TInput> _target;

		// Token: 0x0400011D RID: 285
		private readonly ReorderingBuffer<IEnumerable<TOutput>> _reorderingBuffer;

		// Token: 0x0400011E RID: 286
		private readonly SourceCore<TOutput> _source;

		// Token: 0x0200005B RID: 91
		private sealed class DebugView
		{
			// Token: 0x06000311 RID: 785 RVA: 0x0000B12F File Offset: 0x0000932F
			public DebugView(TransformManyBlock<TInput, TOutput> transformManyBlock)
			{
				this._transformManyBlock = transformManyBlock;
				this._targetDebuggingInformation = transformManyBlock._target.GetDebuggingInformation();
				this._sourceDebuggingInformation = transformManyBlock._source.GetDebuggingInformation();
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x06000312 RID: 786 RVA: 0x0000B160 File Offset: 0x00009360
			public IEnumerable<TInput> InputQueue
			{
				get
				{
					return this._targetDebuggingInformation.InputQueue;
				}
			}

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x06000313 RID: 787 RVA: 0x0000B16D File Offset: 0x0000936D
			public QueuedMap<ISourceBlock<TInput>, DataflowMessageHeader> PostponedMessages
			{
				get
				{
					return this._targetDebuggingInformation.PostponedMessages;
				}
			}

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x06000314 RID: 788 RVA: 0x0000B17A File Offset: 0x0000937A
			public IEnumerable<TOutput> OutputQueue
			{
				get
				{
					return this._sourceDebuggingInformation.OutputQueue;
				}
			}

			// Token: 0x17000115 RID: 277
			// (get) Token: 0x06000315 RID: 789 RVA: 0x0000B187 File Offset: 0x00009387
			public int CurrentDegreeOfParallelism
			{
				get
				{
					return this._targetDebuggingInformation.CurrentDegreeOfParallelism;
				}
			}

			// Token: 0x17000116 RID: 278
			// (get) Token: 0x06000316 RID: 790 RVA: 0x0000B194 File Offset: 0x00009394
			public Task TaskForOutputProcessing
			{
				get
				{
					return this._sourceDebuggingInformation.TaskForOutputProcessing;
				}
			}

			// Token: 0x17000117 RID: 279
			// (get) Token: 0x06000317 RID: 791 RVA: 0x0000B1A1 File Offset: 0x000093A1
			public ExecutionDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._targetDebuggingInformation.DataflowBlockOptions;
				}
			}

			// Token: 0x17000118 RID: 280
			// (get) Token: 0x06000318 RID: 792 RVA: 0x0000B1AE File Offset: 0x000093AE
			public bool IsDecliningPermanently
			{
				get
				{
					return this._targetDebuggingInformation.IsDecliningPermanently;
				}
			}

			// Token: 0x17000119 RID: 281
			// (get) Token: 0x06000319 RID: 793 RVA: 0x0000B1BB File Offset: 0x000093BB
			public bool IsCompleted
			{
				get
				{
					return this._sourceDebuggingInformation.IsCompleted;
				}
			}

			// Token: 0x1700011A RID: 282
			// (get) Token: 0x0600031A RID: 794 RVA: 0x0000B1C8 File Offset: 0x000093C8
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._transformManyBlock);
				}
			}

			// Token: 0x1700011B RID: 283
			// (get) Token: 0x0600031B RID: 795 RVA: 0x0000B1D5 File Offset: 0x000093D5
			public TargetRegistry<TOutput> LinkedTargets
			{
				get
				{
					return this._sourceDebuggingInformation.LinkedTargets;
				}
			}

			// Token: 0x1700011C RID: 284
			// (get) Token: 0x0600031C RID: 796 RVA: 0x0000B1E2 File Offset: 0x000093E2
			public ITargetBlock<TOutput> NextMessageReservedFor
			{
				get
				{
					return this._sourceDebuggingInformation.NextMessageReservedFor;
				}
			}

			// Token: 0x0400011F RID: 287
			private readonly TransformManyBlock<TInput, TOutput> _transformManyBlock;

			// Token: 0x04000120 RID: 288
			private readonly TargetCore<TInput>.DebuggingInformation _targetDebuggingInformation;

			// Token: 0x04000121 RID: 289
			private readonly SourceCore<TOutput>.DebuggingInformation _sourceDebuggingInformation;
		}
	}
}
