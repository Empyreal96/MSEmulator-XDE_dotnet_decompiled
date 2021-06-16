using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a buffer for receiving and storing at most one element in a network of dataflow blocks.</summary>
	/// <typeparam name="T">Specifies the type of the data buffered by this dataflow block.</typeparam>
	// Token: 0x0200005E RID: 94
	[DebuggerTypeProxy(typeof(WriteOnceBlock<>.DebugView))]
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	public sealed class WriteOnceBlock<T> : IPropagatorBlock<T, T>, ITargetBlock<!0>, IDataflowBlock, ISourceBlock<!0>, IReceivableSourceBlock<!0>, IDebuggerDisplay
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000B2DA File Offset: 0x000094DA
		private object ValueLock
		{
			get
			{
				return this._targetRegistry;
			}
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.WriteOnceBlock`1" />.</summary>
		/// <param name="cloningFunction">The function to use to clone the data when offered to other blocks.</param>
		// Token: 0x0600032A RID: 810 RVA: 0x0000B2E2 File Offset: 0x000094E2
		public WriteOnceBlock(Func<T, T> cloningFunction) : this(cloningFunction, DataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Dataflow.WriteOnceBlock`1" /> with the specified <see cref="T:System.Threading.Tasks.Dataflow.DataflowBlockOptions" />.</summary>
		/// <param name="cloningFunction">The function to use to clone the data when offered to other blocks.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.WriteOnceBlock`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600032B RID: 811 RVA: 0x0000B2F0 File Offset: 0x000094F0
		public WriteOnceBlock(Func<T, T> cloningFunction, DataflowBlockOptions dataflowBlockOptions)
		{
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			this._cloningFunction = cloningFunction;
			this._dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			this._targetRegistry = new TargetRegistry<T>(this);
			if (dataflowBlockOptions.CancellationToken.CanBeCanceled)
			{
				this._lazyCompletionTaskSource = new TaskCompletionSource<VoidResult>();
				if (dataflowBlockOptions.CancellationToken.IsCancellationRequested)
				{
					this._completionReserved = (this._decliningPermanently = true);
					this._lazyCompletionTaskSource.SetCanceled();
				}
				else
				{
					Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this._lazyCompletionTaskSource.Task, delegate(object state)
					{
						((WriteOnceBlock<T>)state).Complete();
					}, this);
				}
			}
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000B3C4 File Offset: 0x000095C4
		private void CompleteBlockAsync(IList<Exception> exceptions)
		{
			if (exceptions == null)
			{
				Task task = new Task(delegate(object state)
				{
					((WriteOnceBlock<T>)state).OfferToTargetsAndCompleteBlock();
				}, this, Common.GetCreationOptionsForTask(false));
				DataflowEtwProvider log = DataflowEtwProvider.Log;
				if (log.IsEnabled())
				{
					log.TaskLaunchedForMessageHandling(this, task, DataflowEtwProvider.TaskLaunchedReason.OfferingOutputMessages, this._header.IsValid ? 1 : 0);
				}
				Exception ex = Common.StartTaskSafe(task, this._dataflowBlockOptions.TaskScheduler);
				if (ex != null)
				{
					this.CompleteCore(ex, true);
					return;
				}
			}
			else
			{
				Task.Factory.StartNew(delegate(object state)
				{
					Tuple<WriteOnceBlock<T>, IList<Exception>> tuple = (Tuple<WriteOnceBlock<T>, IList<Exception>>)state;
					tuple.Item1.CompleteBlock(tuple.Item2);
				}, Tuple.Create<WriteOnceBlock<T>, IList<Exception>>(this, exceptions), CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000B488 File Offset: 0x00009688
		private void OfferToTargetsAndCompleteBlock()
		{
			List<Exception> exceptions = this.OfferToTargets();
			this.CompleteBlock(exceptions);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000B4A4 File Offset: 0x000096A4
		private void CompleteBlock(IList<Exception> exceptions)
		{
			TargetRegistry<T>.LinkedTargetInfo firstTarget = this._targetRegistry.ClearEntryPoints();
			if (exceptions != null && exceptions.Count > 0)
			{
				this.CompletionTaskSource.TrySetException(exceptions);
			}
			else if (this._dataflowBlockOptions.CancellationToken.IsCancellationRequested)
			{
				this.CompletionTaskSource.TrySetCanceled();
			}
			else if (Interlocked.CompareExchange<TaskCompletionSource<VoidResult>>(ref this._lazyCompletionTaskSource, Common.CompletedVoidResultTaskCompletionSource, null) != null)
			{
				this._lazyCompletionTaskSource.TrySetResult(default(VoidResult));
			}
			this._targetRegistry.PropagateCompletion(firstTarget);
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCompleted(this);
			}
		}

		/// <summary>Causes the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> to complete in a <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state.</summary>
		/// <param name="exception">The <see cref="T:System.Exception" /> that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> is null.</exception>
		// Token: 0x0600032F RID: 815 RVA: 0x0000B544 File Offset: 0x00009744
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this.CompleteCore(exception, false);
		}

		/// <summary>Signals to the <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> that it should not accept nor produce any more messages nor consume any more postponed messages.</summary>
		// Token: 0x06000330 RID: 816 RVA: 0x0000B55C File Offset: 0x0000975C
		public void Complete()
		{
			this.CompleteCore(null, false);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000B568 File Offset: 0x00009768
		private void CompleteCore(Exception exception, bool storeExceptionEvenIfAlreadyCompleting)
		{
			bool flag = false;
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				if (this._decliningPermanently && !storeExceptionEvenIfAlreadyCompleting)
				{
					return;
				}
				this._decliningPermanently = true;
				if (!this._completionReserved || storeExceptionEvenIfAlreadyCompleting)
				{
					flag = (this._completionReserved = true);
				}
			}
			if (flag)
			{
				List<Exception> list = null;
				if (exception != null)
				{
					list = new List<Exception>();
					list.Add(exception);
				}
				this.CompleteBlockAsync(list);
			}
		}

		/// <summary>Attempts to synchronously receive an available output item from the <see cref="T:System.Threading.Tasks.Dataflow.IReceivableSourceBlock`1" />.</summary>
		/// <returns>true if an item could be received; otherwise, false.</returns>
		/// <param name="filter">The predicate value must successfully pass in order for it to be received. <paramref name="filter" /> may be null, in which case all items will pass.</param>
		/// <param name="item">The item received from the source.</param>
		// Token: 0x06000332 RID: 818 RVA: 0x0000B5F0 File Offset: 0x000097F0
		public bool TryReceive(Predicate<T> filter, out T item)
		{
			if (this._header.IsValid && (filter == null || filter(this._value)))
			{
				item = this.CloneItem(this._value);
				return true;
			}
			item = default(T);
			return false;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000B62C File Offset: 0x0000982C
		bool IReceivableSourceBlock<!0>.TryReceiveAll(out IList<T> items)
		{
			T t;
			if (this.TryReceive(null, out t))
			{
				items = new T[]
				{
					t
				};
				return true;
			}
			items = null;
			return false;
		}

		/// <summary>Links the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> to the specified <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" />.</summary>
		/// <returns>An IDisposable that, upon calling Dispose, will unlink the source from the target.</returns>
		/// <param name="target">The <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> to which to connect this source.</param>
		/// <param name="linkOptions">A <see cref="T:System.Threading.Tasks.Dataflow.DataflowLinkOptions" /> instance that configures the link.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null (Nothing in Visual Basic) or <paramref name="linkOptions" /> is null (Nothing in Visual Basic).</exception>
		// Token: 0x06000334 RID: 820 RVA: 0x0000B65C File Offset: 0x0000985C
		public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (linkOptions == null)
			{
				throw new ArgumentNullException("linkOptions");
			}
			object valueLock = this.ValueLock;
			bool hasValue;
			lock (valueLock)
			{
				hasValue = this.HasValue;
				bool completionReserved = this._completionReserved;
				if (!hasValue && !completionReserved)
				{
					this._targetRegistry.Add(ref target, linkOptions);
					return Common.CreateUnlinker<T>(this.ValueLock, this._targetRegistry, target);
				}
			}
			if (hasValue)
			{
				bool consumeToAccept = this._cloningFunction != null;
				target.OfferMessage(this._header, this._value, this, consumeToAccept);
			}
			if (linkOptions.PropagateCompletion)
			{
				Common.PropagateCompletionOnceCompleted(this.Completion, target);
			}
			return Disposables.Nop;
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The task.</returns>
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000B72C File Offset: 0x0000992C
		public Task Completion
		{
			get
			{
				return this.CompletionTaskSource.Task;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000B73C File Offset: 0x0000993C
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
			bool flag = false;
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				if (this._decliningPermanently)
				{
					return DataflowMessageStatus.DecliningPermanently;
				}
				if (consumeToAccept)
				{
					bool flag3;
					messageValue = source.ConsumeMessage(messageHeader, this, out flag3);
					if (!flag3)
					{
						return DataflowMessageStatus.NotAvailable;
					}
				}
				this._header = Common.SingleMessageHeader;
				this._value = messageValue;
				this._decliningPermanently = true;
				if (!this._completionReserved)
				{
					flag = (this._completionReserved = true);
				}
			}
			if (flag)
			{
				this.CompleteBlockAsync(null);
			}
			return DataflowMessageStatus.Accepted;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000B80C File Offset: 0x00009A0C
		T ISourceBlock<!0>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
		{
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (this._header.Id == messageHeader.Id)
			{
				messageConsumed = true;
				return this.CloneItem(this._value);
			}
			messageConsumed = false;
			return default(T);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000B870 File Offset: 0x00009A70
		bool ISourceBlock<!0>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return this._header.Id == messageHeader.Id;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		void ISourceBlock<!0>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
		{
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (this._header.Id != messageHeader.Id)
			{
				throw new InvalidOperationException(SR.InvalidOperation_MessageNotReservedByTarget);
			}
			bool consumeToAccept = this._cloningFunction != null;
			target.OfferMessage(this._header, this._value, this, consumeToAccept);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000B922 File Offset: 0x00009B22
		private T CloneItem(T item)
		{
			if (this._cloningFunction == null)
			{
				return item;
			}
			return this._cloningFunction(item);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000B93C File Offset: 0x00009B3C
		private List<Exception> OfferToTargets()
		{
			List<Exception> result = null;
			if (this.HasValue)
			{
				TargetRegistry<T>.LinkedTargetInfo next;
				for (TargetRegistry<T>.LinkedTargetInfo linkedTargetInfo = this._targetRegistry.FirstTargetNode; linkedTargetInfo != null; linkedTargetInfo = next)
				{
					next = linkedTargetInfo.Next;
					ITargetBlock<T> target = linkedTargetInfo.Target;
					try
					{
						bool consumeToAccept = this._cloningFunction != null;
						target.OfferMessage(this._header, this._value, this, consumeToAccept);
					}
					catch (Exception ex)
					{
						Common.StoreDataflowMessageValueIntoExceptionData<T>(ex, this._value, false);
						Common.AddException(ref result, ex, false);
					}
				}
			}
			return result;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000B9C4 File Offset: 0x00009BC4
		private TaskCompletionSource<VoidResult> CompletionTaskSource
		{
			get
			{
				if (this._lazyCompletionTaskSource == null)
				{
					Interlocked.CompareExchange<TaskCompletionSource<VoidResult>>(ref this._lazyCompletionTaskSource, new TaskCompletionSource<VoidResult>(), null);
				}
				return this._lazyCompletionTaskSource;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000B9E6 File Offset: 0x00009BE6
		private bool HasValue
		{
			get
			{
				return this._header.IsValid;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000B9F4 File Offset: 0x00009BF4
		private T Value
		{
			get
			{
				if (!this._header.IsValid)
				{
					return default(T);
				}
				return this._value;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</returns>
		// Token: 0x0600033F RID: 831 RVA: 0x0000BA1E File Offset: 0x00009C1E
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, this._dataflowBlockOptions);
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000BA2C File Offset: 0x00009C2C
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, HasValue={1}, Value={2}", Common.GetNameForDebugger(this, this._dataflowBlockOptions), this.HasValue, this.Value);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000BA5A File Offset: 0x00009C5A
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x0400012D RID: 301
		private readonly TargetRegistry<T> _targetRegistry;

		// Token: 0x0400012E RID: 302
		private readonly Func<T, T> _cloningFunction;

		// Token: 0x0400012F RID: 303
		private readonly DataflowBlockOptions _dataflowBlockOptions;

		// Token: 0x04000130 RID: 304
		private TaskCompletionSource<VoidResult> _lazyCompletionTaskSource;

		// Token: 0x04000131 RID: 305
		private bool _decliningPermanently;

		// Token: 0x04000132 RID: 306
		private bool _completionReserved;

		// Token: 0x04000133 RID: 307
		private DataflowMessageHeader _header;

		// Token: 0x04000134 RID: 308
		private T _value;

		// Token: 0x0200005F RID: 95
		private sealed class DebugView
		{
			// Token: 0x06000342 RID: 834 RVA: 0x0000BA62 File Offset: 0x00009C62
			public DebugView(WriteOnceBlock<T> writeOnceBlock)
			{
				this._writeOnceBlock = writeOnceBlock;
			}

			// Token: 0x17000124 RID: 292
			// (get) Token: 0x06000343 RID: 835 RVA: 0x0000BA71 File Offset: 0x00009C71
			public bool IsCompleted
			{
				get
				{
					return this._writeOnceBlock.Completion.IsCompleted;
				}
			}

			// Token: 0x17000125 RID: 293
			// (get) Token: 0x06000344 RID: 836 RVA: 0x0000BA83 File Offset: 0x00009C83
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._writeOnceBlock);
				}
			}

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x06000345 RID: 837 RVA: 0x0000BA90 File Offset: 0x00009C90
			public bool HasValue
			{
				get
				{
					return this._writeOnceBlock.HasValue;
				}
			}

			// Token: 0x17000127 RID: 295
			// (get) Token: 0x06000346 RID: 838 RVA: 0x0000BA9D File Offset: 0x00009C9D
			public T Value
			{
				get
				{
					return this._writeOnceBlock.Value;
				}
			}

			// Token: 0x17000128 RID: 296
			// (get) Token: 0x06000347 RID: 839 RVA: 0x0000BAAA File Offset: 0x00009CAA
			public DataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._writeOnceBlock._dataflowBlockOptions;
				}
			}

			// Token: 0x17000129 RID: 297
			// (get) Token: 0x06000348 RID: 840 RVA: 0x0000BAB7 File Offset: 0x00009CB7
			public TargetRegistry<T> LinkedTargets
			{
				get
				{
					return this._writeOnceBlock._targetRegistry;
				}
			}

			// Token: 0x04000135 RID: 309
			private readonly WriteOnceBlock<T> _writeOnceBlock;
		}
	}
}
