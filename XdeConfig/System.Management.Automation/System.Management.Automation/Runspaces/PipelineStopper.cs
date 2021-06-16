using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200021D RID: 541
	internal class PipelineStopper
	{
		// Token: 0x060019A1 RID: 6561 RVA: 0x0009A420 File Offset: 0x00098620
		internal PipelineStopper(LocalPipeline localPipeline)
		{
			this._localPipeline = localPipeline;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x0009A445 File Offset: 0x00098645
		// (set) Token: 0x060019A3 RID: 6563 RVA: 0x0009A44D File Offset: 0x0009864D
		internal bool IsStopping
		{
			get
			{
				return this._stopping;
			}
			set
			{
				this._stopping = value;
			}
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0009A458 File Offset: 0x00098658
		internal void Push(PipelineProcessor item)
		{
			if (item == null)
			{
				throw PSTraceSource.NewArgumentNullException("item");
			}
			lock (this._syncRoot)
			{
				if (this._stopping)
				{
					PipelineStoppedException ex = new PipelineStoppedException();
					throw ex;
				}
				this._stack.Push(item);
			}
			item.LocalPipeline = this._localPipeline;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0009A4C8 File Offset: 0x000986C8
		internal void Pop(bool fromSteppablePipeline)
		{
			lock (this._syncRoot)
			{
				if (!this._stopping)
				{
					if (this._stack.Count > 0)
					{
						PipelineProcessor pipelineProcessor = this._stack.Pop();
						if (fromSteppablePipeline && pipelineProcessor.ExecutionFailed && this._stack.Count > 0)
						{
							this._stack.Peek().ExecutionFailed = true;
						}
						if (this._stack.Count == 1 && this._localPipeline != null)
						{
							this._localPipeline.SetHadErrors(pipelineProcessor.ExecutionFailed);
						}
					}
				}
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0009A578 File Offset: 0x00098778
		internal void Stop()
		{
			PipelineProcessor[] array;
			lock (this._syncRoot)
			{
				if (this._stopping)
				{
					return;
				}
				this._stopping = true;
				array = this._stack.ToArray();
			}
			if (array.Length > 0)
			{
				PipelineProcessor pipelineProcessor = array[array.Length - 1];
				if (pipelineProcessor != null && this._localPipeline != null)
				{
					this._localPipeline.SetHadErrors(pipelineProcessor.ExecutionFailed);
				}
			}
			foreach (PipelineProcessor pipelineProcessor2 in array)
			{
				pipelineProcessor2.Stop();
			}
		}

		// Token: 0x04000A80 RID: 2688
		private Stack<PipelineProcessor> _stack = new Stack<PipelineProcessor>();

		// Token: 0x04000A81 RID: 2689
		private object _syncRoot = new object();

		// Token: 0x04000A82 RID: 2690
		private LocalPipeline _localPipeline;

		// Token: 0x04000A83 RID: 2691
		private bool _stopping;
	}
}
