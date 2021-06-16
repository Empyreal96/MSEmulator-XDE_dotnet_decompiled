using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Provides options used to configure the processing performed by dataflow blocks.</summary>
	// Token: 0x02000028 RID: 40
	[DebuggerDisplay("TaskScheduler = {TaskScheduler}, MaxMessagesPerTask = {MaxMessagesPerTask}, BoundedCapacity = {BoundedCapacity}")]
	public class DataflowBlockOptions
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00004C90 File Offset: 0x00002E90
		internal DataflowBlockOptions DefaultOrClone()
		{
			if (this != DataflowBlockOptions.Default)
			{
				return new DataflowBlockOptions
				{
					TaskScheduler = this.TaskScheduler,
					CancellationToken = this.CancellationToken,
					MaxMessagesPerTask = this.MaxMessagesPerTask,
					BoundedCapacity = this.BoundedCapacity,
					NameFormat = this.NameFormat,
					EnsureOrdered = this.EnsureOrdered
				};
			}
			return this;
		}

		/// <summary>Gets or sets the <see cref="T:System.Threading.Tasks.TaskScheduler" /> to use for scheduling tasks.</summary>
		/// <returns>The task scheduler.</returns>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004D32 File Offset: 0x00002F32
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004D3A File Offset: 0x00002F3A
		public TaskScheduler TaskScheduler
		{
			get
			{
				return this._taskScheduler;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._taskScheduler = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Threading.CancellationToken" /> to monitor for cancellation requests.</summary>
		/// <returns>The token.</returns>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004D51 File Offset: 0x00002F51
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004D59 File Offset: 0x00002F59
		public CancellationToken CancellationToken
		{
			get
			{
				return this._cancellationToken;
			}
			set
			{
				this._cancellationToken = value;
			}
		}

		/// <summary>Gets or sets the maximum number of messages that may be processed per task.</summary>
		/// <returns>The maximum number of messages.</returns>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004D62 File Offset: 0x00002F62
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004D6A File Offset: 0x00002F6A
		public int MaxMessagesPerTask
		{
			get
			{
				return this._maxMessagesPerTask;
			}
			set
			{
				if (value < 1 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._maxMessagesPerTask = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004D86 File Offset: 0x00002F86
		internal int ActualMaxMessagesPerTask
		{
			get
			{
				if (this._maxMessagesPerTask != -1)
				{
					return this._maxMessagesPerTask;
				}
				return int.MaxValue;
			}
		}

		/// <summary>Gets or sets the maximum number of messages that may be buffered by the block.</summary>
		/// <returns>The maximum number of messages.</returns>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004D9D File Offset: 0x00002F9D
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004DA5 File Offset: 0x00002FA5
		public int BoundedCapacity
		{
			get
			{
				return this._boundedCapacity;
			}
			set
			{
				if (value < 1 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._boundedCapacity = value;
			}
		}

		/// <summary>Gets or sets the format string to use when a block is queried for its name.</summary>
		/// <returns>The format string to use when a block is queried for its name.</returns>
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004DC1 File Offset: 0x00002FC1
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004DC9 File Offset: 0x00002FC9
		public string NameFormat
		{
			get
			{
				return this._nameFormat;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._nameFormat = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004DE0 File Offset: 0x00002FE0
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004DE8 File Offset: 0x00002FE8
		public bool EnsureOrdered
		{
			get
			{
				return this._ensureOrdered;
			}
			set
			{
				this._ensureOrdered = value;
			}
		}

		/// <summary>A constant used to specify an unlimited quantity for <see cref="T:System.Threading.Tasks.Dataflow.DataflowBlockOptions" /> members that provide an upper bound. This field is constant.</summary>
		// Token: 0x04000066 RID: 102
		public const int Unbounded = -1;

		// Token: 0x04000067 RID: 103
		private TaskScheduler _taskScheduler = TaskScheduler.Default;

		// Token: 0x04000068 RID: 104
		private CancellationToken _cancellationToken = CancellationToken.None;

		// Token: 0x04000069 RID: 105
		private int _maxMessagesPerTask = -1;

		// Token: 0x0400006A RID: 106
		private int _boundedCapacity = -1;

		// Token: 0x0400006B RID: 107
		private string _nameFormat = "{0} Id={1}";

		// Token: 0x0400006C RID: 108
		private bool _ensureOrdered = true;

		// Token: 0x0400006D RID: 109
		internal static readonly DataflowBlockOptions Default = new DataflowBlockOptions();
	}
}
