using System;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001FE RID: 510
	public class HistoryInfo
	{
		// Token: 0x060017AB RID: 6059 RVA: 0x0009264E File Offset: 0x0009084E
		internal HistoryInfo(long pipelineId, string cmdline, PipelineState status, DateTime startTime, DateTime endTime)
		{
			this._pipelineId = pipelineId;
			this._cmdline = cmdline;
			this._status = status;
			this._startTime = startTime;
			this._endTime = endTime;
			this._cleared = false;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00092684 File Offset: 0x00090884
		private HistoryInfo(HistoryInfo history)
		{
			this._id = history._id;
			this._pipelineId = history._pipelineId;
			this._cmdline = history._cmdline;
			this._status = history._status;
			this._startTime = history._startTime;
			this._endTime = history._endTime;
			this._cleared = history._cleared;
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x000926EB File Offset: 0x000908EB
		public long Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x000926F3 File Offset: 0x000908F3
		public string CommandLine
		{
			get
			{
				return this._cmdline;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x000926FB File Offset: 0x000908FB
		public PipelineState ExecutionStatus
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x00092703 File Offset: 0x00090903
		public DateTime StartExecutionTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x0009270B File Offset: 0x0009090B
		public DateTime EndExecutionTime
		{
			get
			{
				return this._endTime;
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00092713 File Offset: 0x00090913
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this._cmdline))
			{
				return base.ToString();
			}
			return this._cmdline;
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x0009272F File Offset: 0x0009092F
		// (set) Token: 0x060017B4 RID: 6068 RVA: 0x00092737 File Offset: 0x00090937
		internal bool Cleared
		{
			get
			{
				return this._cleared;
			}
			set
			{
				this._cleared = value;
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00092740 File Offset: 0x00090940
		internal void SetId(long id)
		{
			this._id = id;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00092749 File Offset: 0x00090949
		internal void SetStatus(PipelineState status)
		{
			this._status = status;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00092752 File Offset: 0x00090952
		internal void SetEndTime(DateTime endTime)
		{
			this._endTime = endTime;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0009275B File Offset: 0x0009095B
		internal void SetCommand(string command)
		{
			this._cmdline = command;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00092764 File Offset: 0x00090964
		public HistoryInfo Clone()
		{
			return new HistoryInfo(this);
		}

		// Token: 0x040009FE RID: 2558
		private long _pipelineId;

		// Token: 0x040009FF RID: 2559
		private long _id;

		// Token: 0x04000A00 RID: 2560
		private string _cmdline;

		// Token: 0x04000A01 RID: 2561
		private PipelineState _status;

		// Token: 0x04000A02 RID: 2562
		private DateTime _startTime;

		// Token: 0x04000A03 RID: 2563
		private DateTime _endTime;

		// Token: 0x04000A04 RID: 2564
		private bool _cleared;
	}
}
