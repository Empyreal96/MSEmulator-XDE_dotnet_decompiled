using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides options used to configure the processing performed by dataflow blocks that process each message through the invocation of a user-provided delegate. These are dataflow blocks such as <see cref="T:System.Threading.Tasks.Dataflow.ActionBlock`1" /> and <see cref="T:System.Threading.Tasks.Dataflow.TransformBlock`2" />.</summary>
	// Token: 0x02000029 RID: 41
	[DebuggerDisplay("TaskScheduler = {TaskScheduler}, MaxMessagesPerTask = {MaxMessagesPerTask}, BoundedCapacity = {BoundedCapacity}, MaxDegreeOfParallelism = {MaxDegreeOfParallelism}")]
	public class ExecutionDataflowBlockOptions : DataflowBlockOptions
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00004E00 File Offset: 0x00003000
		internal new ExecutionDataflowBlockOptions DefaultOrClone()
		{
			if (this != ExecutionDataflowBlockOptions.Default)
			{
				return new ExecutionDataflowBlockOptions
				{
					TaskScheduler = base.TaskScheduler,
					CancellationToken = base.CancellationToken,
					MaxMessagesPerTask = base.MaxMessagesPerTask,
					BoundedCapacity = base.BoundedCapacity,
					NameFormat = base.NameFormat,
					EnsureOrdered = base.EnsureOrdered,
					MaxDegreeOfParallelism = this.MaxDegreeOfParallelism,
					SingleProducerConstrained = this.SingleProducerConstrained
				};
			}
			return this;
		}

		/// <summary>Gets the maximum number of messages that may be processed by the block concurrently.</summary>
		/// <returns>The maximum number of messages.</returns>
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004E8B File Offset: 0x0000308B
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004E93 File Offset: 0x00003093
		public int MaxDegreeOfParallelism
		{
			get
			{
				return this._maxDegreeOfParallelism;
			}
			set
			{
				if (value < 1 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._maxDegreeOfParallelism = value;
			}
		}

		/// <summary>Gets whether code using the dataflow block is constrained to one producer at a time.</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004EAF File Offset: 0x000030AF
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004EB7 File Offset: 0x000030B7
		public bool SingleProducerConstrained
		{
			get
			{
				return this._singleProducerConstrained;
			}
			set
			{
				this._singleProducerConstrained = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004EC0 File Offset: 0x000030C0
		internal int ActualMaxDegreeOfParallelism
		{
			get
			{
				if (this._maxDegreeOfParallelism != -1)
				{
					return this._maxDegreeOfParallelism;
				}
				return int.MaxValue;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004ED7 File Offset: 0x000030D7
		internal bool SupportsParallelExecution
		{
			get
			{
				return this._maxDegreeOfParallelism == -1 || this._maxDegreeOfParallelism > 1;
			}
		}

		// Token: 0x0400006E RID: 110
		internal new static readonly ExecutionDataflowBlockOptions Default = new ExecutionDataflowBlockOptions();

		// Token: 0x0400006F RID: 111
		private int _maxDegreeOfParallelism = 1;

		// Token: 0x04000070 RID: 112
		private bool _singleProducerConstrained;
	}
}
