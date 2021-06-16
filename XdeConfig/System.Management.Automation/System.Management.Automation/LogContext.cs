using System;

namespace System.Management.Automation
{
	// Token: 0x020003F3 RID: 1011
	internal class LogContext
	{
		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x000FC797 File Offset: 0x000FA997
		// (set) Token: 0x06002D95 RID: 11669 RVA: 0x000FC79F File Offset: 0x000FA99F
		internal string Severity
		{
			get
			{
				return this._severity;
			}
			set
			{
				this._severity = value;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000FC7A8 File Offset: 0x000FA9A8
		// (set) Token: 0x06002D97 RID: 11671 RVA: 0x000FC7B0 File Offset: 0x000FA9B0
		internal string HostName
		{
			get
			{
				return this._hostName;
			}
			set
			{
				this._hostName = value;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002D98 RID: 11672 RVA: 0x000FC7B9 File Offset: 0x000FA9B9
		// (set) Token: 0x06002D99 RID: 11673 RVA: 0x000FC7C1 File Offset: 0x000FA9C1
		internal string HostApplication { get; set; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x000FC7CA File Offset: 0x000FA9CA
		// (set) Token: 0x06002D9B RID: 11675 RVA: 0x000FC7D2 File Offset: 0x000FA9D2
		internal string HostVersion
		{
			get
			{
				return this._hostVersion;
			}
			set
			{
				this._hostVersion = value;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x000FC7DB File Offset: 0x000FA9DB
		// (set) Token: 0x06002D9D RID: 11677 RVA: 0x000FC7E3 File Offset: 0x000FA9E3
		internal string HostId
		{
			get
			{
				return this._hostId;
			}
			set
			{
				this._hostId = value;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x000FC7EC File Offset: 0x000FA9EC
		// (set) Token: 0x06002D9F RID: 11679 RVA: 0x000FC7F4 File Offset: 0x000FA9F4
		internal string EngineVersion
		{
			get
			{
				return this._engineVersion;
			}
			set
			{
				this._engineVersion = value;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x000FC7FD File Offset: 0x000FA9FD
		// (set) Token: 0x06002DA1 RID: 11681 RVA: 0x000FC805 File Offset: 0x000FAA05
		internal string RunspaceId
		{
			get
			{
				return this._runspaceId;
			}
			set
			{
				this._runspaceId = value;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06002DA2 RID: 11682 RVA: 0x000FC80E File Offset: 0x000FAA0E
		// (set) Token: 0x06002DA3 RID: 11683 RVA: 0x000FC816 File Offset: 0x000FAA16
		internal string PipelineId
		{
			get
			{
				return this._pipelineId;
			}
			set
			{
				this._pipelineId = value;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x000FC81F File Offset: 0x000FAA1F
		// (set) Token: 0x06002DA5 RID: 11685 RVA: 0x000FC827 File Offset: 0x000FAA27
		internal string CommandName
		{
			get
			{
				return this._commandName;
			}
			set
			{
				this._commandName = value;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002DA6 RID: 11686 RVA: 0x000FC830 File Offset: 0x000FAA30
		// (set) Token: 0x06002DA7 RID: 11687 RVA: 0x000FC838 File Offset: 0x000FAA38
		internal string CommandType
		{
			get
			{
				return this._commandType;
			}
			set
			{
				this._commandType = value;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002DA8 RID: 11688 RVA: 0x000FC841 File Offset: 0x000FAA41
		// (set) Token: 0x06002DA9 RID: 11689 RVA: 0x000FC849 File Offset: 0x000FAA49
		internal string ScriptName
		{
			get
			{
				return this._scriptName;
			}
			set
			{
				this._scriptName = value;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06002DAA RID: 11690 RVA: 0x000FC852 File Offset: 0x000FAA52
		// (set) Token: 0x06002DAB RID: 11691 RVA: 0x000FC85A File Offset: 0x000FAA5A
		internal string CommandPath
		{
			get
			{
				return this._commandPath;
			}
			set
			{
				this._commandPath = value;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06002DAC RID: 11692 RVA: 0x000FC863 File Offset: 0x000FAA63
		// (set) Token: 0x06002DAD RID: 11693 RVA: 0x000FC86B File Offset: 0x000FAA6B
		internal string CommandLine
		{
			get
			{
				return this._commandLine;
			}
			set
			{
				this._commandLine = value;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06002DAE RID: 11694 RVA: 0x000FC874 File Offset: 0x000FAA74
		// (set) Token: 0x06002DAF RID: 11695 RVA: 0x000FC87C File Offset: 0x000FAA7C
		internal string SequenceNumber
		{
			get
			{
				return this._sequenceNumber;
			}
			set
			{
				this._sequenceNumber = value;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x000FC885 File Offset: 0x000FAA85
		// (set) Token: 0x06002DB1 RID: 11697 RVA: 0x000FC88D File Offset: 0x000FAA8D
		internal string User
		{
			get
			{
				return this._user;
			}
			set
			{
				this._user = value;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x000FC896 File Offset: 0x000FAA96
		// (set) Token: 0x06002DB3 RID: 11699 RVA: 0x000FC89E File Offset: 0x000FAA9E
		internal string ConnectedUser { get; set; }

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x000FC8A7 File Offset: 0x000FAAA7
		// (set) Token: 0x06002DB5 RID: 11701 RVA: 0x000FC8AF File Offset: 0x000FAAAF
		internal string Time
		{
			get
			{
				return this._time;
			}
			set
			{
				this._time = value;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x000FC8B8 File Offset: 0x000FAAB8
		// (set) Token: 0x06002DB7 RID: 11703 RVA: 0x000FC8C0 File Offset: 0x000FAAC0
		internal string ShellId
		{
			get
			{
				return this._shellId;
			}
			set
			{
				this._shellId = value;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x000FC8C9 File Offset: 0x000FAAC9
		// (set) Token: 0x06002DB9 RID: 11705 RVA: 0x000FC8D1 File Offset: 0x000FAAD1
		internal ExecutionContext ExecutionContext
		{
			get
			{
				return this._executionContext;
			}
			set
			{
				this._executionContext = value;
			}
		}

		// Token: 0x040017F0 RID: 6128
		private string _severity = "";

		// Token: 0x040017F1 RID: 6129
		private string _hostName = "";

		// Token: 0x040017F2 RID: 6130
		private string _hostVersion = "";

		// Token: 0x040017F3 RID: 6131
		private string _hostId = "";

		// Token: 0x040017F4 RID: 6132
		private string _engineVersion = "";

		// Token: 0x040017F5 RID: 6133
		private string _runspaceId = "";

		// Token: 0x040017F6 RID: 6134
		private string _pipelineId = "";

		// Token: 0x040017F7 RID: 6135
		private string _commandName = "";

		// Token: 0x040017F8 RID: 6136
		private string _commandType = "";

		// Token: 0x040017F9 RID: 6137
		private string _scriptName = "";

		// Token: 0x040017FA RID: 6138
		private string _commandPath = "";

		// Token: 0x040017FB RID: 6139
		private string _commandLine = "";

		// Token: 0x040017FC RID: 6140
		private string _sequenceNumber = "";

		// Token: 0x040017FD RID: 6141
		private string _user = "";

		// Token: 0x040017FE RID: 6142
		private string _time = "";

		// Token: 0x040017FF RID: 6143
		private string _shellId;

		// Token: 0x04001800 RID: 6144
		private ExecutionContext _executionContext;
	}
}
