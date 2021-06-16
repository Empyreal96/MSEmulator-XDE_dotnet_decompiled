using System;
using System.Diagnostics.Tracing;
using System.Linq;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000078 RID: 120
	[EventSource(Name = "System.Threading.Tasks.Dataflow.DataflowEventSource", Guid = "16F53577-E41D-43D4-B47E-C17025BF4025", LocalizationResources = "FxResources.System.Threading.Tasks.Dataflow.SR")]
	internal sealed class DataflowEtwProvider : EventSource
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		private DataflowEtwProvider()
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000D7BC File Offset: 0x0000B9BC
		[NonEvent]
		internal void DataflowBlockCreated(IDataflowBlock block, DataflowBlockOptions dataflowBlockOptions)
		{
			if (base.IsEnabled(EventLevel.Informational, EventKeywords.All))
			{
				this.DataflowBlockCreated(Common.GetNameForDebugger(block, dataflowBlockOptions), Common.GetBlockId(block));
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000D7DC File Offset: 0x0000B9DC
		[Event(1, Level = EventLevel.Informational)]
		private void DataflowBlockCreated(string blockName, int blockId)
		{
			base.WriteEvent(1, blockName, blockId);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000D7E7 File Offset: 0x0000B9E7
		[NonEvent]
		internal void TaskLaunchedForMessageHandling(IDataflowBlock block, Task task, DataflowEtwProvider.TaskLaunchedReason reason, int availableMessages)
		{
			if (base.IsEnabled(EventLevel.Informational, EventKeywords.All))
			{
				this.TaskLaunchedForMessageHandling(Common.GetBlockId(block), reason, availableMessages, task.Id);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000D809 File Offset: 0x0000BA09
		[Event(2, Level = EventLevel.Informational)]
		private void TaskLaunchedForMessageHandling(int blockId, DataflowEtwProvider.TaskLaunchedReason reason, int availableMessages, int taskId)
		{
			base.WriteEvent(2, new object[]
			{
				blockId,
				reason,
				availableMessages,
				taskId
			});
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000D840 File Offset: 0x0000BA40
		[NonEvent]
		internal void DataflowBlockCompleted(IDataflowBlock block)
		{
			if (base.IsEnabled(EventLevel.Informational, EventKeywords.All))
			{
				Task potentiallyNotSupportedCompletionTask = Common.GetPotentiallyNotSupportedCompletionTask(block);
				bool flag = potentiallyNotSupportedCompletionTask != null && potentiallyNotSupportedCompletionTask.IsCompleted;
				if (flag)
				{
					DataflowEtwProvider.BlockCompletionReason status = (DataflowEtwProvider.BlockCompletionReason)potentiallyNotSupportedCompletionTask.Status;
					string exceptionData = string.Empty;
					if (potentiallyNotSupportedCompletionTask.IsFaulted)
					{
						try
						{
							exceptionData = string.Join(Environment.NewLine, from e in potentiallyNotSupportedCompletionTask.Exception.InnerExceptions
							select e.ToString());
						}
						catch
						{
						}
					}
					this.DataflowBlockCompleted(Common.GetBlockId(block), status, exceptionData);
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000D8E4 File Offset: 0x0000BAE4
		[Event(3, Level = EventLevel.Informational)]
		private void DataflowBlockCompleted(int blockId, DataflowEtwProvider.BlockCompletionReason reason, string exceptionData)
		{
			base.WriteEvent(3, new object[]
			{
				blockId,
				reason,
				exceptionData
			});
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000D909 File Offset: 0x0000BB09
		[NonEvent]
		internal void DataflowBlockLinking<T>(ISourceBlock<T> source, ITargetBlock<T> target)
		{
			if (base.IsEnabled(EventLevel.Informational, EventKeywords.All))
			{
				this.DataflowBlockLinking(Common.GetBlockId(source), Common.GetBlockId(target));
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000D928 File Offset: 0x0000BB28
		[Event(4, Level = EventLevel.Informational)]
		private void DataflowBlockLinking(int sourceId, int targetId)
		{
			base.WriteEvent(4, sourceId, targetId);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000D933 File Offset: 0x0000BB33
		[NonEvent]
		internal void DataflowBlockUnlinking<T>(ISourceBlock<T> source, ITargetBlock<T> target)
		{
			if (base.IsEnabled(EventLevel.Informational, EventKeywords.All))
			{
				this.DataflowBlockUnlinking(Common.GetBlockId(source), Common.GetBlockId(target));
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000D952 File Offset: 0x0000BB52
		[Event(5, Level = EventLevel.Informational)]
		private void DataflowBlockUnlinking(int sourceId, int targetId)
		{
			base.WriteEvent(5, sourceId, targetId);
		}

		// Token: 0x0400017E RID: 382
		internal static readonly DataflowEtwProvider Log = new DataflowEtwProvider();

		// Token: 0x02000079 RID: 121
		internal enum TaskLaunchedReason
		{
			// Token: 0x04000180 RID: 384
			ProcessingInputMessages = 1,
			// Token: 0x04000181 RID: 385
			OfferingOutputMessages
		}

		// Token: 0x0200007A RID: 122
		internal enum BlockCompletionReason
		{
			// Token: 0x04000183 RID: 387
			RanToCompletion = 5,
			// Token: 0x04000184 RID: 388
			Faulted = 7,
			// Token: 0x04000185 RID: 389
			Canceled = 6
		}
	}
}
