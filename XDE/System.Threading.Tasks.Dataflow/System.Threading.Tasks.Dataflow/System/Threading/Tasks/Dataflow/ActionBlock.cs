using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow.Internal;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides a dataflow block that invokes a provided <see cref="T:System.Action`1" /> delegate for every data element received.</summary>
	/// <typeparam name="TInput">The type of data that this <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" /> operates on.</typeparam>
	// Token: 0x02000033 RID: 51
	[DebuggerTypeProxy(typeof(ActionBlock<>.DebugView))]
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	public sealed class ActionBlock<TInput> : ITargetBlock<TInput>, IDataflowBlock, IDebuggerDisplay
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" /> class with the specified action.</summary>
		/// <param name="action">The action to invoke with each data element received.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="action" /> is null.</exception>
		// Token: 0x06000129 RID: 297 RVA: 0x000050F9 File Offset: 0x000032F9
		public ActionBlock(Action<TInput> action) : this(action, ExecutionDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" /> class with the specified action and configuration options.</summary>
		/// <param name="action">The action to invoke with each data element received.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="action" /> is null.-or-<paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600012A RID: 298 RVA: 0x00005107 File Offset: 0x00003307
		public ActionBlock(Action<TInput> action, ExecutionDataflowBlockOptions dataflowBlockOptions) : this(action, dataflowBlockOptions)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" /> class with the specified action.</summary>
		/// <param name="action">The action to invoke with each data element received.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="action" /> is null.</exception>
		// Token: 0x0600012B RID: 299 RVA: 0x000050F9 File Offset: 0x000032F9
		public ActionBlock(Func<TInput, Task> action) : this(action, ExecutionDataflowBlockOptions.Default)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" /> class with the specified action and configuration options.</summary>
		/// <param name="action">The action to invoke with each data element received.</param>
		/// <param name="dataflowBlockOptions">The options with which to configure this <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="action" /> is null.-or-<paramref name="dataflowBlockOptions" /> is null.</exception>
		// Token: 0x0600012C RID: 300 RVA: 0x00005107 File Offset: 0x00003307
		public ActionBlock(Func<TInput, Task> action, ExecutionDataflowBlockOptions dataflowBlockOptions) : this(action, dataflowBlockOptions)
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005114 File Offset: 0x00003314
		private ActionBlock(Delegate action, ExecutionDataflowBlockOptions dataflowBlockOptions)
		{
			ActionBlock<TInput> <>4__this = this;
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (dataflowBlockOptions == null)
			{
				throw new ArgumentNullException("dataflowBlockOptions");
			}
			dataflowBlockOptions = dataflowBlockOptions.DefaultOrClone();
			Action<TInput> syncAction = action as Action<TInput>;
			if (syncAction != null && dataflowBlockOptions.SingleProducerConstrained && dataflowBlockOptions.MaxDegreeOfParallelism == 1 && !dataflowBlockOptions.CancellationToken.CanBeCanceled && dataflowBlockOptions.BoundedCapacity == -1)
			{
				this._spscTarget = new SpscTargetCore<TInput>(this, syncAction, dataflowBlockOptions);
			}
			else
			{
				if (syncAction != null)
				{
					this._defaultTarget = new TargetCore<TInput>(this, delegate(KeyValuePair<TInput, long> messageWithId)
					{
						<>4__this.ProcessMessage(syncAction, messageWithId);
					}, null, dataflowBlockOptions, TargetCoreOptions.RepresentsBlockCompletion);
				}
				else
				{
					Func<TInput, Task> asyncAction = action as Func<TInput, Task>;
					this._defaultTarget = new TargetCore<TInput>(this, delegate(KeyValuePair<TInput, long> messageWithId)
					{
						<>4__this.ProcessMessageWithTask(asyncAction, messageWithId);
					}, null, dataflowBlockOptions, TargetCoreOptions.UsesAsyncCompletion | TargetCoreOptions.RepresentsBlockCompletion);
				}
				Common.WireCancellationToComplete(dataflowBlockOptions.CancellationToken, this.Completion, delegate(object state)
				{
					((TargetCore<TInput>)state).Complete(null, true, false, false, false);
				}, this._defaultTarget);
			}
			DataflowEtwProvider log = DataflowEtwProvider.Log;
			if (log.IsEnabled())
			{
				log.DataflowBlockCreated(this, dataflowBlockOptions);
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005254 File Offset: 0x00003454
		private void ProcessMessage(Action<TInput> action, KeyValuePair<TInput, long> messageWithId)
		{
			try
			{
				action(messageWithId.Key);
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
				if (this._defaultTarget.IsBounded)
				{
					this._defaultTarget.ChangeBoundingCount(-1);
				}
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000052B4 File Offset: 0x000034B4
		private void ProcessMessageWithTask(Func<TInput, Task> action, KeyValuePair<TInput, long> messageWithId)
		{
			Task task = null;
			Exception ex = null;
			try
			{
				task = action(messageWithId.Key);
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
					this._defaultTarget.Complete(ex, true, true, false, false);
				}
				this._defaultTarget.SignalOneAsyncMessageCompleted(-1);
				return;
			}
			if (task.IsCompleted)
			{
				this.AsyncCompleteProcessMessageWithTask(task);
				return;
			}
			task.ContinueWith(delegate(Task completed, object state)
			{
				((ActionBlock<TInput>)state).AsyncCompleteProcessMessageWithTask(completed);
			}, this, CancellationToken.None, Common.GetContinuationOptions(TaskContinuationOptions.ExecuteSynchronously), TaskScheduler.Default);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005370 File Offset: 0x00003570
		private void AsyncCompleteProcessMessageWithTask(Task completed)
		{
			if (completed.IsFaulted)
			{
				this._defaultTarget.Complete(completed.Exception, true, true, true, false);
			}
			this._defaultTarget.SignalOneAsyncMessageCompleted(-1);
		}

		/// <summary>Signals to the dataflow block that it shouldn't accept or produce any more messages and shouldn't consume any more postponed messages.</summary>
		// Token: 0x06000131 RID: 305 RVA: 0x0000539B File Offset: 0x0000359B
		public void Complete()
		{
			if (this._defaultTarget != null)
			{
				this._defaultTarget.Complete(null, false, false, false, false);
				return;
			}
			this._spscTarget.Complete(null);
		}

		/// <summary>Causes the dataflow block to complete in a faulted state.</summary>
		/// <param name="exception">The exception that caused the faulting.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="exception" /> is null.</exception>
		// Token: 0x06000132 RID: 306 RVA: 0x000053C2 File Offset: 0x000035C2
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			if (this._defaultTarget != null)
			{
				this._defaultTarget.Complete(exception, true, false, false, false);
				return;
			}
			this._spscTarget.Complete(exception);
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> object that represents the asynchronous operation and completion of the dataflow block.</summary>
		/// <returns>The completed task.</returns>
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000053F7 File Offset: 0x000035F7
		public Task Completion
		{
			get
			{
				if (this._defaultTarget == null)
				{
					return this._spscTarget.Completion;
				}
				return this._defaultTarget.Completion;
			}
		}

		/// <summary>Posts an item to the target dataflow block.</summary>
		/// <returns>The number of input items.</returns>
		/// <param name="item">The item being offered to the target.</param>
		// Token: 0x06000134 RID: 308 RVA: 0x00005418 File Offset: 0x00003618
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Post(TInput item)
		{
			if (this._defaultTarget == null)
			{
				return this._spscTarget.Post(item);
			}
			return this._defaultTarget.OfferMessage(Common.SingleMessageHeader, item, null, false) == DataflowMessageStatus.Accepted;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005445 File Offset: 0x00003645
		DataflowMessageStatus ITargetBlock<!0>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
		{
			if (this._defaultTarget == null)
			{
				return this._spscTarget.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
			}
			return this._defaultTarget.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
		}

		/// <summary>Gets the number of input items waiting to be processed by this block.</summary>
		/// <returns>The number of input items waiting to be processed by this block.</returns>
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00005470 File Offset: 0x00003670
		public int InputCount
		{
			get
			{
				if (this._defaultTarget == null)
				{
					return this._spscTarget.InputCount;
				}
				return this._defaultTarget.InputCount;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005491 File Offset: 0x00003691
		private int InputCountForDebugger
		{
			get
			{
				if (this._defaultTarget == null)
				{
					return this._spscTarget.InputCount;
				}
				return this._defaultTarget.GetDebuggingInformation().InputCount;
			}
		}

		/// <summary>Returns a string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> instance.</summary>
		/// <returns>A string that represents the formatted name of this <see cref="T:System.Threading.Tasks.Dataflow.IDataflowBlock" /> nstance.</returns>
		// Token: 0x06000138 RID: 312 RVA: 0x000054B7 File Offset: 0x000036B7
		public override string ToString()
		{
			return Common.GetNameForDebugger(this, (this._defaultTarget != null) ? this._defaultTarget.DataflowBlockOptions : this._spscTarget.DataflowBlockOptions);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000054DF File Offset: 0x000036DF
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0}, InputCount={1}", Common.GetNameForDebugger(this, (this._defaultTarget != null) ? this._defaultTarget.DataflowBlockOptions : this._spscTarget.DataflowBlockOptions), this.InputCountForDebugger);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000551C File Offset: 0x0000371C
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x04000080 RID: 128
		private readonly TargetCore<TInput> _defaultTarget;

		// Token: 0x04000081 RID: 129
		private readonly SpscTargetCore<TInput> _spscTarget;

		// Token: 0x02000034 RID: 52
		private sealed class DebugView
		{
			// Token: 0x0600013B RID: 315 RVA: 0x00005524 File Offset: 0x00003724
			public DebugView(ActionBlock<TInput> actionBlock)
			{
				this._actionBlock = actionBlock;
				if (this._actionBlock._defaultTarget != null)
				{
					this._defaultDebugInfo = actionBlock._defaultTarget.GetDebuggingInformation();
					return;
				}
				this._spscDebugInfo = actionBlock._spscTarget.GetDebuggingInformation();
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x0600013C RID: 316 RVA: 0x00005563 File Offset: 0x00003763
			public IEnumerable<TInput> InputQueue
			{
				get
				{
					if (this._defaultDebugInfo == null)
					{
						return this._spscDebugInfo.InputQueue;
					}
					return this._defaultDebugInfo.InputQueue;
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x0600013D RID: 317 RVA: 0x00005584 File Offset: 0x00003784
			public QueuedMap<ISourceBlock<TInput>, DataflowMessageHeader> PostponedMessages
			{
				get
				{
					if (this._defaultDebugInfo == null)
					{
						return null;
					}
					return this._defaultDebugInfo.PostponedMessages;
				}
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x0600013E RID: 318 RVA: 0x0000559B File Offset: 0x0000379B
			public int CurrentDegreeOfParallelism
			{
				get
				{
					if (this._defaultDebugInfo == null)
					{
						return this._spscDebugInfo.CurrentDegreeOfParallelism;
					}
					return this._defaultDebugInfo.CurrentDegreeOfParallelism;
				}
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x0600013F RID: 319 RVA: 0x000055BC File Offset: 0x000037BC
			public ExecutionDataflowBlockOptions DataflowBlockOptions
			{
				get
				{
					if (this._defaultDebugInfo == null)
					{
						return this._spscDebugInfo.DataflowBlockOptions;
					}
					return this._defaultDebugInfo.DataflowBlockOptions;
				}
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x06000140 RID: 320 RVA: 0x000055DD File Offset: 0x000037DD
			public bool IsDecliningPermanently
			{
				get
				{
					if (this._defaultDebugInfo == null)
					{
						return this._spscDebugInfo.IsDecliningPermanently;
					}
					return this._defaultDebugInfo.IsDecliningPermanently;
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000141 RID: 321 RVA: 0x000055FE File Offset: 0x000037FE
			public bool IsCompleted
			{
				get
				{
					if (this._defaultDebugInfo == null)
					{
						return this._spscDebugInfo.IsCompleted;
					}
					return this._defaultDebugInfo.IsCompleted;
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000142 RID: 322 RVA: 0x0000561F File Offset: 0x0000381F
			public int Id
			{
				get
				{
					return Common.GetBlockId(this._actionBlock);
				}
			}

			// Token: 0x04000082 RID: 130
			private readonly ActionBlock<TInput> _actionBlock;

			// Token: 0x04000083 RID: 131
			private readonly TargetCore<TInput>.DebuggingInformation _defaultDebugInfo;

			// Token: 0x04000084 RID: 132
			private readonly SpscTargetCore<TInput>.DebuggingInformation _spscDebugInfo;
		}
	}
}
