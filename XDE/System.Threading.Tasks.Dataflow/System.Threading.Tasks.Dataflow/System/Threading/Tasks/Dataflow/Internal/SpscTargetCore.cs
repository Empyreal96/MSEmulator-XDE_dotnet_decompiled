using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000088 RID: 136
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	internal sealed class SpscTargetCore<TInput>
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x0000F18D File Offset: 0x0000D38D
		internal SpscTargetCore(ITargetBlock<TInput> owningTarget, Action<TInput> action, ExecutionDataflowBlockOptions dataflowBlockOptions)
		{
			this._owningTarget = owningTarget;
			this._action = action;
			this._dataflowBlockOptions = dataflowBlockOptions;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000F1B5 File Offset: 0x0000D3B5
		internal bool Post(TInput messageValue)
		{
			if (this._decliningPermanently)
			{
				return false;
			}
			this._messages.Enqueue(messageValue);
			Interlocked.MemoryBarrier();
			if (this._activeConsumer == null)
			{
				this.ScheduleConsumerIfNecessary(false);
			}
			return true;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000F1E6 File Offset: 0x0000D3E6
		internal DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
		{
			if (consumeToAccept || !this.Post(messageValue))
			{
				return this.OfferMessage_Slow(messageHeader, messageValue, source, consumeToAccept);
			}
			return DataflowMessageStatus.Accepted;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000F204 File Offset: 0x0000D404
		private DataflowMessageStatus OfferMessage_Slow(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
		{
			if (this._decliningPermanently)
			{
				return DataflowMessageStatus.DecliningPermanently;
			}
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (consumeToAccept)
			{
				if (source == null)
				{
					throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
				}
				bool flag;
				messageValue = source.ConsumeMessage(messageHeader, this._owningTarget, out flag);
				if (!flag)
				{
					return DataflowMessageStatus.NotAvailable;
				}
			}
			this._messages.Enqueue(messageValue);
			Interlocked.MemoryBarrier();
			if (this._activeConsumer == null)
			{
				this.ScheduleConsumerIfNecessary(false);
			}
			return DataflowMessageStatus.Accepted;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000F288 File Offset: 0x0000D488
		private void ScheduleConsumerIfNecessary(bool isReplica)
		{
			if (this._activeConsumer == null)
			{
				Task task = new Task(delegate(object state)
				{
					((SpscTargetCore<TInput>)state).ProcessMessagesLoopCore();
				}, this, CancellationToken.None, Common.GetCreationOptionsForTask(isReplica));
				if (Interlocked.CompareExchange<Task>(ref this._activeConsumer, task, null) == null)
				{
					DataflowEtwProvider log = DataflowEtwProvider.Log;
					if (log.IsEnabled())
					{
						log.TaskLaunchedForMessageHandling(this._owningTarget, task, DataflowEtwProvider.TaskLaunchedReason.ProcessingInputMessages, this._messages.Count);
					}
					task.Start(this._dataflowBlockOptions.TaskScheduler);
				}
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000F318 File Offset: 0x0000D518
		private void ProcessMessagesLoopCore()
		{
			int num = 0;
			int actualMaxMessagesPerTask = this._dataflowBlockOptions.ActualMaxMessagesPerTask;
			bool flag = true;
			while (flag)
			{
				flag = false;
				TInput tinput = default(TInput);
				try
				{
					while (this._exceptions == null && num < actualMaxMessagesPerTask && this._messages.TryDequeue(out tinput))
					{
						num++;
						this._action(tinput);
					}
				}
				catch (Exception ex)
				{
					if (!Common.IsCooperativeCancellation(ex))
					{
						this._decliningPermanently = true;
						Common.StoreDataflowMessageValueIntoExceptionData<TInput>(ex, tinput, false);
						this.StoreException(ex);
					}
				}
				finally
				{
					if (!this._messages.IsEmpty && this._exceptions == null && num < actualMaxMessagesPerTask)
					{
						flag = true;
					}
					else
					{
						bool decliningPermanently = this._decliningPermanently;
						if ((decliningPermanently && this._messages.IsEmpty) || this._exceptions != null)
						{
							if (!this._completionReserved)
							{
								this._completionReserved = true;
								this.CompleteBlockOncePossible();
							}
						}
						else
						{
							Task task = Interlocked.Exchange<Task>(ref this._activeConsumer, null);
							if (!this._messages.IsEmpty || (!decliningPermanently && this._decliningPermanently) || this._exceptions != null)
							{
								this.ScheduleConsumerIfNecessary(true);
							}
						}
					}
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000F460 File Offset: 0x0000D660
		internal int InputCount
		{
			get
			{
				return this._messages.Count;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000F46D File Offset: 0x0000D66D
		internal void Complete(Exception exception)
		{
			if (!this._decliningPermanently)
			{
				if (exception != null)
				{
					this.StoreException(exception);
				}
				this._decliningPermanently = true;
				this.ScheduleConsumerIfNecessary(false);
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000F494 File Offset: 0x0000D694
		private void StoreException(Exception exception)
		{
			List<Exception> obj = LazyInitializer.EnsureInitialized<List<Exception>>(ref this._exceptions, () => new List<Exception>());
			lock (obj)
			{
				this._exceptions.Add(exception);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000F500 File Offset: 0x0000D700
		private void CompleteBlockOncePossible()
		{
			TInput tinput;
			while (this._messages.TryDequeue(out tinput))
			{
			}
			if (this._exceptions != null)
			{
				List<Exception> exceptions = this._exceptions;
				Exception[] exceptions2;
				lock (exceptions)
				{
					exceptions2 = this._exceptions.ToArray();
				}
				bool flag2 = this.CompletionSource.TrySetException(exceptions2);
			}
			else
			{
				bool flag2 = this.CompletionSource.TrySetResult(default(VoidResult));
			}
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCompleted(this._owningTarget);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000F5A8 File Offset: 0x0000D7A8
		internal Task Completion
		{
			get
			{
				return this.CompletionSource.Task;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000F5B5 File Offset: 0x0000D7B5
		private TaskCompletionSource<VoidResult> CompletionSource
		{
			get
			{
				return LazyInitializer.EnsureInitialized<TaskCompletionSource<VoidResult>>(ref this._completionTask, () => new TaskCompletionSource<VoidResult>());
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000F5E1 File Offset: 0x0000D7E1
		internal ExecutionDataflowBlockOptions DataflowBlockOptions
		{
			get
			{
				return this._dataflowBlockOptions;
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000F5E9 File Offset: 0x0000D7E9
		internal SpscTargetCore<TInput>.DebuggingInformation GetDebuggingInformation()
		{
			return new SpscTargetCore<TInput>.DebuggingInformation(this);
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
		private object DebuggerDisplayContent
		{
			get
			{
				IDebuggerDisplay debuggerDisplay = this._owningTarget as IDebuggerDisplay;
				return string.Format("Block=\"{0}\"", (debuggerDisplay != null) ? debuggerDisplay.Content : this._owningTarget);
			}
		}

		// Token: 0x040001AC RID: 428
		private readonly ITargetBlock<TInput> _owningTarget;

		// Token: 0x040001AD RID: 429
		private readonly SingleProducerSingleConsumerQueue<TInput> _messages = new SingleProducerSingleConsumerQueue<TInput>();

		// Token: 0x040001AE RID: 430
		private readonly ExecutionDataflowBlockOptions _dataflowBlockOptions;

		// Token: 0x040001AF RID: 431
		private readonly Action<TInput> _action;

		// Token: 0x040001B0 RID: 432
		private volatile List<Exception> _exceptions;

		// Token: 0x040001B1 RID: 433
		private volatile bool _decliningPermanently;

		// Token: 0x040001B2 RID: 434
		private volatile bool _completionReserved;

		// Token: 0x040001B3 RID: 435
		private volatile Task _activeConsumer;

		// Token: 0x040001B4 RID: 436
		private TaskCompletionSource<VoidResult> _completionTask;

		// Token: 0x02000089 RID: 137
		internal sealed class DebuggingInformation
		{
			// Token: 0x06000441 RID: 1089 RVA: 0x0000F628 File Offset: 0x0000D828
			internal DebuggingInformation(SpscTargetCore<TInput> target)
			{
				this._target = target;
			}

			// Token: 0x17000169 RID: 361
			// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000F637 File Offset: 0x0000D837
			internal IEnumerable<TInput> InputQueue
			{
				get
				{
					return this._target._messages.ToList<TInput>();
				}
			}

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x06000443 RID: 1091 RVA: 0x0000F649 File Offset: 0x0000D849
			internal int CurrentDegreeOfParallelism
			{
				get
				{
					if (this._target._activeConsumer == null || this._target.Completion.IsCompleted)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000F66F File Offset: 0x0000D86F
			internal ExecutionDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					return this._target._dataflowBlockOptions;
				}
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000F67C File Offset: 0x0000D87C
			internal bool IsDecliningPermanently
			{
				get
				{
					return this._target._decliningPermanently;
				}
			}

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000F68B File Offset: 0x0000D88B
			internal bool IsCompleted
			{
				get
				{
					return this._target.Completion.IsCompleted;
				}
			}

			// Token: 0x040001B5 RID: 437
			private readonly SpscTargetCore<TInput> _target;
		}
	}
}
