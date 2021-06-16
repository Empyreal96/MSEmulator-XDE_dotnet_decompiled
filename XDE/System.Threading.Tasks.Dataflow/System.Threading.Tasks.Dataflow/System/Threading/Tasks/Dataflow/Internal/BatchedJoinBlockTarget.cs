using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000061 RID: 97
	[DebuggerTypeProxy(typeof(BatchedJoinBlockTarget<>.DebugView))]
	[DebuggerDisplay("{DebuggerDisplayContent,nq}")]
	internal sealed class BatchedJoinBlockTarget<T> : ITargetBlock<!0>, IDataflowBlock, IDebuggerDisplay
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0000BB11 File Offset: 0x00009D11
		internal BatchedJoinBlockTarget(BatchedJoinBlockTargetSharedResources sharedResources)
		{
			this._sharedResources = sharedResources;
			sharedResources._remainingAliveTargets++;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000BB39 File Offset: 0x00009D39
		internal int Count
		{
			get
			{
				return this._messages.Count;
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000BB48 File Offset: 0x00009D48
		internal IList<T> GetAndEmptyMessages()
		{
			IList<T> messages = this._messages;
			this._messages = new List<T>();
			return messages;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000BB68 File Offset: 0x00009D68
		public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
		{
			if (!messageHeader.IsValid)
			{
				throw new ArgumentException(SR.Argument_InvalidMessageHeader, "messageHeader");
			}
			if (source == null && consumeToAccept)
			{
				throw new ArgumentException(SR.Argument_CantConsumeFromANullSource, "consumeToAccept");
			}
			object incomingLock = this._sharedResources._incomingLock;
			DataflowMessageStatus result;
			lock (incomingLock)
			{
				if (this._decliningPermanently || this._sharedResources._decliningPermanently)
				{
					result = DataflowMessageStatus.DecliningPermanently;
				}
				else
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
					this._messages.Add(messageValue);
					BatchedJoinBlockTargetSharedResources sharedResources = this._sharedResources;
					int num = sharedResources._remainingItemsInBatch - 1;
					sharedResources._remainingItemsInBatch = num;
					if (num == 0)
					{
						this._sharedResources._batchSizeReachedAction();
					}
					result = DataflowMessageStatus.Accepted;
				}
			}
			return result;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000BC44 File Offset: 0x00009E44
		public void Complete()
		{
			object incomingLock = this._sharedResources._incomingLock;
			lock (incomingLock)
			{
				if (!this._decliningPermanently)
				{
					this._decliningPermanently = true;
					BatchedJoinBlockTargetSharedResources sharedResources = this._sharedResources;
					int num = sharedResources._remainingAliveTargets - 1;
					sharedResources._remainingAliveTargets = num;
					if (num == 0)
					{
						this._sharedResources._allTargetsDecliningPermanentlyAction();
					}
				}
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000BCBC File Offset: 0x00009EBC
		void IDataflowBlock.Fault(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			object incomingLock = this._sharedResources._incomingLock;
			lock (incomingLock)
			{
				if (!this._decliningPermanently && !this._sharedResources._decliningPermanently)
				{
					this._sharedResources._exceptionAction(exception);
				}
			}
			this._sharedResources._completionAction();
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000354 RID: 852 RVA: 0x000033CB File Offset: 0x000015CB
		Task IDataflowBlock.Completion
		{
			get
			{
				throw new NotSupportedException(SR.NotSupported_MemberNotNeeded);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000BD40 File Offset: 0x00009F40
		private object DebuggerDisplayContent
		{
			get
			{
				return string.Format("{0} InputCount={1}", Common.GetNameForDebugger(this, null), this._messages.Count);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000BD63 File Offset: 0x00009F63
		object IDebuggerDisplay.Content
		{
			get
			{
				return this.DebuggerDisplayContent;
			}
		}

		// Token: 0x0400013A RID: 314
		private readonly BatchedJoinBlockTargetSharedResources _sharedResources;

		// Token: 0x0400013B RID: 315
		private bool _decliningPermanently;

		// Token: 0x0400013C RID: 316
		private IList<T> _messages = new List<T>();

		// Token: 0x02000062 RID: 98
		private sealed class DebugView
		{
			// Token: 0x06000357 RID: 855 RVA: 0x0000BD6B File Offset: 0x00009F6B
			public DebugView(BatchedJoinBlockTarget<T> batchedJoinBlockTarget)
			{
				this._batchedJoinBlockTarget = batchedJoinBlockTarget;
			}

			// Token: 0x1700012E RID: 302
			// (get) Token: 0x06000358 RID: 856 RVA: 0x0000BD7A File Offset: 0x00009F7A
			public IEnumerable<T> InputQueue
			{
				get
				{
					return this._batchedJoinBlockTarget._messages;
				}
			}

			// Token: 0x1700012F RID: 303
			// (get) Token: 0x06000359 RID: 857 RVA: 0x0000BD87 File Offset: 0x00009F87
			public bool IsDecliningPermanently
			{
				get
				{
					return this._batchedJoinBlockTarget._decliningPermanently || this._batchedJoinBlockTarget._sharedResources._decliningPermanently;
				}
			}

			// Token: 0x0400013D RID: 317
			private readonly BatchedJoinBlockTarget<T> _batchedJoinBlockTarget;
		}
	}
}
