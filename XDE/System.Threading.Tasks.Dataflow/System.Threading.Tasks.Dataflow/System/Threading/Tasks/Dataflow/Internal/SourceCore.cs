using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000085 RID: 133
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	internal sealed class SourceCore<TOutput>
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		private object OutgoingLock
		{
			get
			{
				return this._completionTask;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		private object ValueLock
		{
			get
			{
				return this._targetRegistry;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000DFB0 File Offset: 0x0000C1B0
		internal SourceCore(ISourceBlock<TOutput> owningSource, DataflowBlockOptions dataflowBlockOptions, Action<ISourceBlock<TOutput>> completeAction, Action<ISourceBlock<TOutput>, int> itemsRemovedAction = null, Func<ISourceBlock<TOutput>, TOutput, IList<TOutput>, int> itemCountingFunc = null)
		{
			this._owningSource = owningSource;
			this._dataflowBlockOptions = dataflowBlockOptions;
			this._itemsRemovedAction = itemsRemovedAction;
			this._itemCountingFunc = itemCountingFunc;
			this._completeAction = completeAction;
			this._targetRegistry = new TargetRegistry<TOutput>(this._owningSource);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000E030 File Offset: 0x0000C230
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
			if (this._completionTask.Task.IsCompleted)
			{
				if (linkOptions.PropagateCompletion)
				{
					Common.PropagateCompletion(this._completionTask.Task, target, null);
				}
				return Disposables.Nop;
			}
			object outgoingLock = this.OutgoingLock;
			lock (outgoingLock)
			{
				if (!this._completionReserved)
				{
					this._targetRegistry.Add(ref target, linkOptions);
					this.OfferToTargets(target);
					return Common.CreateUnlinker<TOutput>(this.OutgoingLock, this._targetRegistry, target);
				}
			}
			if (linkOptions.PropagateCompletion)
			{
				Common.PropagateCompletionOnceCompleted(this._completionTask.Task, target);
			}
			return Disposables.Nop;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000E10C File Offset: 0x0000C30C
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
			TOutput toutput = default(TOutput);
			object outgoingLock = this.OutgoingLock;
			lock (outgoingLock)
			{
				if (this._nextMessageReservedFor != target && this._nextMessageReservedFor != null)
				{
					messageConsumed = false;
					return default(TOutput);
				}
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					if (messageHeader.Id != this._nextMessageId.Value || !this._messages.TryDequeue(out toutput))
					{
						messageConsumed = false;
						return default(TOutput);
					}
					this._nextMessageReservedFor = null;
					this._targetRegistry.Remove(target, true);
					this._enableOffering = true;
					this._nextMessageId.Value = this._nextMessageId.Value + 1L;
					this.CompleteBlockIfPossible();
					this.OfferAsyncIfNecessary(false, true);
				}
			}
			if (this._itemsRemovedAction != null)
			{
				int arg = (this._itemCountingFunc != null) ? this._itemCountingFunc(this._owningSource, toutput, null) : 1;
				this._itemsRemovedAction(this._owningSource, arg);
			}
			messageConsumed = true;
			return toutput;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000E278 File Offset: 0x0000C478
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
						if (messageHeader.Id == this._nextMessageId.Value && !this._messages.IsEmpty)
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

		// Token: 0x0600040B RID: 1035 RVA: 0x0000E344 File Offset: 0x0000C544
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
				lock (valueLock)
				{
					if (messageHeader.Id != this._nextMessageId.Value || this._messages.IsEmpty)
					{
						throw new InvalidOperationException(SR.InvalidOperation_MessageNotReservedByTarget);
					}
					this._nextMessageReservedFor = null;
					this._enableOffering = true;
					this.OfferAsyncIfNecessary(false, true);
					this.CompleteBlockIfPossible();
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000E428 File Offset: 0x0000C628
		internal Task Completion
		{
			get
			{
				return this._completionTask.Task;
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000E438 File Offset: 0x0000C638
		internal bool TryReceive(Predicate<TOutput> filter, out TOutput item)
		{
			item = default(TOutput);
			bool flag = false;
			object outgoingLock = this.OutgoingLock;
			lock (outgoingLock)
			{
				if (this._nextMessageReservedFor == null)
				{
					object valueLock = this.ValueLock;
					lock (valueLock)
					{
						if (this._messages.TryDequeueIf(filter, out item))
						{
							this._nextMessageId.Value = this._nextMessageId.Value + 1L;
							this._enableOffering = true;
							this.CompleteBlockIfPossible();
							this.OfferAsyncIfNecessary(false, true);
							flag = true;
						}
					}
				}
			}
			if (flag && this._itemsRemovedAction != null)
			{
				int arg = (this._itemCountingFunc != null) ? this._itemCountingFunc(this._owningSource, item, null) : 1;
				this._itemsRemovedAction(this._owningSource, arg);
			}
			return flag;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000E52C File Offset: 0x0000C72C
		internal bool TryReceiveAll(out IList<TOutput> items)
		{
			items = null;
			int num = 0;
			object outgoingLock = this.OutgoingLock;
			lock (outgoingLock)
			{
				if (this._nextMessageReservedFor == null)
				{
					object valueLock = this.ValueLock;
					lock (valueLock)
					{
						if (!this._messages.IsEmpty)
						{
							List<TOutput> list = new List<TOutput>();
							TOutput item;
							while (this._messages.TryDequeue(out item))
							{
								list.Add(item);
							}
							num = list.Count;
							items = list;
							this._nextMessageId.Value = this._nextMessageId.Value + 1L;
							this._enableOffering = true;
							this.CompleteBlockIfPossible();
						}
					}
				}
			}
			if (num > 0)
			{
				if (this._itemsRemovedAction != null)
				{
					int arg = (this._itemCountingFunc != null) ? this._itemCountingFunc(this._owningSource, default(TOutput), items) : num;
					this._itemsRemovedAction(this._owningSource, arg);
				}
				return true;
			}
			return false;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000E644 File Offset: 0x0000C844
		internal int OutputCount
		{
			get
			{
				object outgoingLock = this.OutgoingLock;
				int count;
				lock (outgoingLock)
				{
					object valueLock = this.ValueLock;
					lock (valueLock)
					{
						count = this._messages.Count;
					}
				}
				return count;
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		internal void AddMessage(TOutput item)
		{
			if (this._decliningPermanently)
			{
				return;
			}
			this._messages.Enqueue(item);
			Interlocked.MemoryBarrier();
			if (this._taskForOutputProcessing == null)
			{
				this.OfferAsyncIfNecessaryWithValueLock();
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		internal void AddMessages(IEnumerable<TOutput> items)
		{
			if (this._decliningPermanently)
			{
				return;
			}
			List<TOutput> list = items as List<TOutput>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this._messages.Enqueue(list[i]);
				}
			}
			else
			{
				TOutput[] array = items as TOutput[];
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						this._messages.Enqueue(array[j]);
					}
				}
				else
				{
					foreach (TOutput item in items)
					{
						this._messages.Enqueue(item);
					}
				}
			}
			Interlocked.MemoryBarrier();
			if (this._taskForOutputProcessing == null)
			{
				this.OfferAsyncIfNecessaryWithValueLock();
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		internal void AddException(Exception exception)
		{
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				Common.AddException(ref this._exceptions, exception, false);
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000E7F4 File Offset: 0x0000C9F4
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

		// Token: 0x06000414 RID: 1044 RVA: 0x0000E870 File Offset: 0x0000CA70
		internal void AddAndUnwrapAggregateException(AggregateException aggregateException)
		{
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				Common.AddException(ref this._exceptions, aggregateException, true);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
		internal bool HasExceptions
		{
			get
			{
				return Volatile.Read<List<Exception>>(ref this._exceptions) != null;
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
		internal void Complete()
		{
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				this._decliningPermanently = true;
				Task.Factory.StartNew(delegate(object state)
				{
					SourceCore<TOutput> sourceCore = (SourceCore<TOutput>)state;
					object outgoingLock = sourceCore.OutgoingLock;
					lock (outgoingLock)
					{
						object valueLock2 = sourceCore.ValueLock;
						lock (valueLock2)
						{
							sourceCore.CompleteBlockIfPossible();
						}
					}
				}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000E944 File Offset: 0x0000CB44
		internal DataflowBlockOptions DataflowBlockOptions
		{
			get
			{
				return this._dataflowBlockOptions;
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000E94C File Offset: 0x0000CB4C
		private bool OfferToTargets(ITargetBlock<TOutput> linkToTarget = null)
		{
			if (this._nextMessageReservedFor != null)
			{
				return false;
			}
			DataflowMessageHeader header = default(DataflowMessageHeader);
			TOutput toutput = default(TOutput);
			bool flag = false;
			if (!Volatile.Read(ref this._enableOffering))
			{
				if (linkToTarget == null)
				{
					return false;
				}
				flag = true;
			}
			if (this._messages.TryPeek(out toutput))
			{
				header = new DataflowMessageHeader(this._nextMessageId.Value);
			}
			bool flag2 = false;
			if (header.IsValid)
			{
				if (flag)
				{
					this.OfferMessageToTarget(header, toutput, linkToTarget, out flag2);
				}
				else
				{
					TargetRegistry<TOutput>.LinkedTargetInfo next;
					for (TargetRegistry<TOutput>.LinkedTargetInfo linkedTargetInfo = this._targetRegistry.FirstTargetNode; linkedTargetInfo != null; linkedTargetInfo = next)
					{
						next = linkedTargetInfo.Next;
						if (this.OfferMessageToTarget(header, toutput, linkedTargetInfo.Target, out flag2))
						{
							break;
						}
					}
					if (!flag2)
					{
						object valueLock = this.ValueLock;
						lock (valueLock)
						{
							this._enableOffering = false;
						}
					}
				}
			}
			if (flag2)
			{
				object valueLock2 = this.ValueLock;
				lock (valueLock2)
				{
					if (this._nextMessageId.Value == header.Id)
					{
						TOutput toutput2;
						this._messages.TryDequeue(out toutput2);
					}
					this._nextMessageId.Value = this._nextMessageId.Value + 1L;
					this._enableOffering = true;
					if (linkToTarget != null)
					{
						this.CompleteBlockIfPossible();
						this.OfferAsyncIfNecessary(false, true);
					}
				}
				if (this._itemsRemovedAction != null)
				{
					int arg = (this._itemCountingFunc != null) ? this._itemCountingFunc(this._owningSource, toutput, null) : 1;
					this._itemsRemovedAction(this._owningSource, arg);
				}
			}
			return flag2;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000EAF0 File Offset: 0x0000CCF0
		private bool OfferMessageToTarget(DataflowMessageHeader header, TOutput message, ITargetBlock<TOutput> target, out bool messageWasAccepted)
		{
			DataflowMessageStatus dataflowMessageStatus = target.OfferMessage(header, message, this._owningSource, false);
			messageWasAccepted = false;
			if (dataflowMessageStatus == DataflowMessageStatus.Accepted)
			{
				this._targetRegistry.Remove(target, true);
				messageWasAccepted = true;
				return true;
			}
			if (dataflowMessageStatus == DataflowMessageStatus.DecliningPermanently)
			{
				this._targetRegistry.Remove(target, false);
			}
			else if (this._nextMessageReservedFor != null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000EB48 File Offset: 0x0000CD48
		private void OfferAsyncIfNecessaryWithValueLock()
		{
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				this.OfferAsyncIfNecessary(false, false);
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000EB8C File Offset: 0x0000CD8C
		private void OfferAsyncIfNecessary(bool isReplacementReplica, bool outgoingLockKnownAcquired)
		{
			if (this._taskForOutputProcessing == null && this._enableOffering && !this._messages.IsEmpty)
			{
				this.OfferAsyncIfNecessary_Slow(isReplacementReplica, outgoingLockKnownAcquired);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		private void OfferAsyncIfNecessary_Slow(bool isReplacementReplica, bool outgoingLockKnownAcquired)
		{
			bool flag = true;
			if (outgoingLockKnownAcquired || Monitor.IsEntered(this.OutgoingLock))
			{
				flag = (this._targetRegistry.FirstTargetNode != null);
			}
			if (flag && !this.CanceledOrFaulted)
			{
				this._taskForOutputProcessing = new Task(delegate(object thisSourceCore)
				{
					((SourceCore<TOutput>)thisSourceCore).OfferMessagesLoopCore();
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
					this._taskForOutputProcessing = null;
					this._decliningPermanently = true;
					Task.Factory.StartNew(delegate(object state)
					{
						SourceCore<TOutput> sourceCore = (SourceCore<TOutput>)state;
						object outgoingLock = sourceCore.OutgoingLock;
						lock (outgoingLock)
						{
							object valueLock = sourceCore.ValueLock;
							lock (valueLock)
							{
								sourceCore.CompleteBlockIfPossible();
							}
						}
					}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
				}
				if (ex != null)
				{
					this.AddException(ex);
				}
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000ECC4 File Offset: 0x0000CEC4
		private void OfferMessagesLoopCore()
		{
			try
			{
				int actualMaxMessagesPerTask = this._dataflowBlockOptions.ActualMaxMessagesPerTask;
				int num = (this._dataflowBlockOptions.MaxMessagesPerTask == -1) ? 10 : actualMaxMessagesPerTask;
				int num2 = 0;
				while (num2 < actualMaxMessagesPerTask && !this.CanceledOrFaulted)
				{
					object outgoingLock = this.OutgoingLock;
					lock (outgoingLock)
					{
						int num3 = 0;
						while (num2 < actualMaxMessagesPerTask && num3 < num && !this.CanceledOrFaulted)
						{
							if (!this.OfferToTargets(null))
							{
								return;
							}
							num2++;
							num3++;
						}
					}
				}
			}
			catch (Exception exception)
			{
				this.AddException(exception);
				this._completeAction(this._owningSource);
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
						Interlocked.MemoryBarrier();
						this.OfferAsyncIfNecessary(true, true);
						this.CompleteBlockIfPossible();
					}
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000EE0C File Offset: 0x0000D00C
		private bool CanceledOrFaulted
		{
			get
			{
				return this._dataflowBlockOptions.CancellationToken.IsCancellationRequested || (this.HasExceptions && this._decliningPermanently);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000EE40 File Offset: 0x0000D040
		private void CompleteBlockIfPossible()
		{
			if (!this._completionReserved && this._decliningPermanently && this._taskForOutputProcessing == null && this._nextMessageReservedFor == null)
			{
				this.CompleteBlockIfPossible_Slow();
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000EE68 File Offset: 0x0000D068
		private void CompleteBlockIfPossible_Slow()
		{
			if (this._messages.IsEmpty || this.CanceledOrFaulted)
			{
				this._completionReserved = true;
				Task.Factory.StartNew(delegate(object state)
				{
					((SourceCore<TOutput>)state).CompleteBlockOncePossible();
				}, this, CancellationToken.None, Common.GetCreationOptionsForTask(false), TaskScheduler.Default);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000EECC File Offset: 0x0000D0CC
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000EFCC File Offset: 0x0000D1CC
		private object DebuggerDisplayContent
		{
			get
			{
				IDebuggerDisplay debuggerDisplay = this._owningSource as IDebuggerDisplay;
				return string.Format("Block=\"{0}\"", (debuggerDisplay != null) ? debuggerDisplay.Content : this._owningSource);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000F000 File Offset: 0x0000D200
		internal SourceCore<TOutput>.DebuggingInformation GetDebuggingInformation()
		{
			return new SourceCore<TOutput>.DebuggingInformation(this);
		}

		// Token: 0x04000197 RID: 407
		private readonly TaskCompletionSource<VoidResult> _completionTask = new TaskCompletionSource<VoidResult>();

		// Token: 0x04000198 RID: 408
		private readonly TargetRegistry<TOutput> _targetRegistry;

		// Token: 0x04000199 RID: 409
		private readonly SingleProducerSingleConsumerQueue<TOutput> _messages = new SingleProducerSingleConsumerQueue<TOutput>();

		// Token: 0x0400019A RID: 410
		private readonly ISourceBlock<TOutput> _owningSource;

		// Token: 0x0400019B RID: 411
		private readonly DataflowBlockOptions _dataflowBlockOptions;

		// Token: 0x0400019C RID: 412
		private readonly Action<ISourceBlock<TOutput>> _completeAction;

		// Token: 0x0400019D RID: 413
		private readonly Action<ISourceBlock<TOutput>, int> _itemsRemovedAction;

		// Token: 0x0400019E RID: 414
		private readonly Func<ISourceBlock<TOutput>, TOutput, IList<TOutput>, int> _itemCountingFunc;

		// Token: 0x0400019F RID: 415
		private Task _taskForOutputProcessing;

		// Token: 0x040001A0 RID: 416
		private PaddedInt64 _nextMessageId = new PaddedInt64
		{
			Value = 1L
		};

		// Token: 0x040001A1 RID: 417
		private ITargetBlock<TOutput> _nextMessageReservedFor;

		// Token: 0x040001A2 RID: 418
		private bool _decliningPermanently;

		// Token: 0x040001A3 RID: 419
		private bool _enableOffering = true;

		// Token: 0x040001A4 RID: 420
		private bool _completionReserved;

		// Token: 0x040001A5 RID: 421
		private List<Exception> _exceptions;

		// Token: 0x02000086 RID: 134
		internal sealed class DebuggingInformation
		{
			// Token: 0x06000424 RID: 1060 RVA: 0x0000F008 File Offset: 0x0000D208
			internal DebuggingInformation(SourceCore<TOutput> source)
			{
				this._source = source;
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000F017 File Offset: 0x0000D217
			internal int OutputCount
			{
				get
				{
					return this._source._messages.Count;
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000F029 File Offset: 0x0000D229
			internal IEnumerable<TOutput> OutputQueue
			{
				get
				{
					return this._source._messages.ToList<TOutput>();
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000F03B File Offset: 0x0000D23B
			internal Task TaskForOutputProcessing
			{
				get
				{
					return this._source._taskForOutputProcessing;
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000F048 File Offset: 0x0000D248
			internal DataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._source._dataflowBlockOptions;
				}
			}

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000F055 File Offset: 0x0000D255
			internal bool IsCompleted
			{
				get
				{
					return this._source.Completion.IsCompleted;
				}
			}

			// Token: 0x17000162 RID: 354
			// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000F067 File Offset: 0x0000D267
			internal TargetRegistry<TOutput> LinkedTargets
			{
				get
				{
					return this._source._targetRegistry;
				}
			}

			// Token: 0x17000163 RID: 355
			// (get) Token: 0x0600042B RID: 1067 RVA: 0x0000F074 File Offset: 0x0000D274
			internal ITargetBlock<TOutput> NextMessageReservedFor
			{
				get
				{
					return this._source._nextMessageReservedFor;
				}
			}

			// Token: 0x040001A6 RID: 422
			private SourceCore<TOutput> _source;
		}
	}
}
