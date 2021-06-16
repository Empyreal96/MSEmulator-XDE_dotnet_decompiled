using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000203 RID: 515
	[Cmdlet("Clear", "History", SupportsShouldProcess = true, DefaultParameterSetName = "IDParameter", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135199")]
	public class ClearHistoryCommand : PSCmdlet
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00093D01 File Offset: 0x00091F01
		// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00093D09 File Offset: 0x00091F09
		[Parameter(ParameterSetName = "IDParameter", Position = 0, HelpMessage = "Specifies the ID of a command in the session history.Clear history clears only the specified command")]
		[ValidateRange(1, 2147483647)]
		public int[] Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x00093D12 File Offset: 0x00091F12
		// (set) Token: 0x060017EB RID: 6123 RVA: 0x00093D1A File Offset: 0x00091F1A
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "CommandLineParameter", HelpMessage = "Specifies the name of a command in the session history")]
		public string[] CommandLine
		{
			get
			{
				return this._commandline;
			}
			set
			{
				this._commandline = value;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060017EC RID: 6124 RVA: 0x00093D23 File Offset: 0x00091F23
		// (set) Token: 0x060017ED RID: 6125 RVA: 0x00093D2B File Offset: 0x00091F2B
		[Parameter(Mandatory = false, Position = 1, HelpMessage = "Clears the specified number of history entries")]
		[ValidateRange(1, 2147483647)]
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._countParamterSpecified = true;
				this._count = value;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x00093D3B File Offset: 0x00091F3B
		// (set) Token: 0x060017EF RID: 6127 RVA: 0x00093D43 File Offset: 0x00091F43
		[Parameter(Mandatory = false, HelpMessage = "Specifies whether new entries to be cleared or the default old ones.")]
		public SwitchParameter Newest
		{
			get
			{
				return this._newest;
			}
			set
			{
				this._newest = value;
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00093D4C File Offset: 0x00091F4C
		protected override void BeginProcessing()
		{
			this.history = ((LocalRunspace)base.Context.CurrentRunspace).History;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00093D6C File Offset: 0x00091F6C
		protected override void ProcessRecord()
		{
			string a;
			if ((a = base.ParameterSetName.ToString()) != null)
			{
				if (a == "IDParameter")
				{
					this.ClearHistoryByID();
					return;
				}
				if (a == "CommandLineParameter")
				{
					this.ClearHistoryByCmdLine();
					return;
				}
			}
			base.ThrowTerminatingError(new ErrorRecord(new ArgumentException("Invalid ParameterSet Name"), "Unable to access the session history", ErrorCategory.InvalidOperation, null));
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00093DD0 File Offset: 0x00091FD0
		private void ClearHistoryByID()
		{
			if (this._countParamterSpecified && this.Count < 0)
			{
				Exception exception = new ArgumentException(StringUtil.Format("HistoryStrings", "InvalidCountValue"));
				base.ThrowTerminatingError(new ErrorRecord(exception, "ClearHistoryInvalidCountValue", ErrorCategory.InvalidArgument, this._count));
			}
			if (this._id != null)
			{
				if (!this._countParamterSpecified)
				{
					foreach (long num in this._id)
					{
						HistoryInfo entry = this.history.GetEntry(num);
						if (entry != null && entry.Id == num)
						{
							this.history.ClearEntry(entry.Id);
						}
						else
						{
							Exception exception2 = new ArgumentException(StringUtil.Format(HistoryStrings.NoHistoryForId, num));
							base.WriteError(new ErrorRecord(exception2, "GetHistoryNoHistoryForId", ErrorCategory.ObjectNotFound, num));
						}
					}
					return;
				}
				if (this._id.Length > 1)
				{
					Exception exception3 = new ArgumentException(StringUtil.Format(HistoryStrings.NoCountWithMultipleIds, new object[0]));
					base.ThrowTerminatingError(new ErrorRecord(exception3, "GetHistoryNoCountWithMultipleIds", ErrorCategory.InvalidArgument, this._count));
					return;
				}
				long id2 = (long)this._id[0];
				this.ClearHistoryEntries(id2, this._count, null, this._newest);
				return;
			}
			else
			{
				if (this._countParamterSpecified)
				{
					this.ClearHistoryEntries(0L, this._count, null, this._newest);
					return;
				}
				string target = StringUtil.Format(HistoryStrings.ClearHistoryWarning, "Warning");
				if (!base.ShouldProcess(target))
				{
					return;
				}
				this.ClearHistoryEntries(0L, -1, null, this._newest);
				return;
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00093F60 File Offset: 0x00092160
		private void ClearHistoryByCmdLine()
		{
			if (this._countParamterSpecified && this.Count < 0)
			{
				Exception exception = new ArgumentException(StringUtil.Format(HistoryStrings.InvalidCountValue, new object[0]));
				base.ThrowTerminatingError(new ErrorRecord(exception, "ClearHistoryInvalidCountValue", ErrorCategory.InvalidArgument, this._count));
			}
			if (this._commandline != null)
			{
				if (!this._countParamterSpecified)
				{
					foreach (string cmdline in this._commandline)
					{
						this.ClearHistoryEntries(0L, 1, cmdline, this._newest);
					}
					return;
				}
				if (this._commandline.Length > 1)
				{
					Exception exception2 = new ArgumentException(StringUtil.Format(HistoryStrings.NoCountWithMultipleCmdLine, new object[0]));
					base.ThrowTerminatingError(new ErrorRecord(exception2, "NoCountWithMultipleCmdLine ", ErrorCategory.InvalidArgument, this._commandline));
					return;
				}
				this.ClearHistoryEntries(0L, this._count, this._commandline[0], this._newest);
			}
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0009404C File Offset: 0x0009224C
		private void ClearHistoryEntries(long id, int count, string cmdline, SwitchParameter newest)
		{
			if (cmdline == null)
			{
				if (id > 0L)
				{
					HistoryInfo entry = this.history.GetEntry(id);
					if (entry == null || entry.Id != id)
					{
						Exception exception = new ArgumentException(StringUtil.Format(HistoryStrings.NoHistoryForId, id));
						base.WriteError(new ErrorRecord(exception, "GetHistoryNoHistoryForId", ErrorCategory.ObjectNotFound, id));
					}
					this.entries = this.history.GetEntries(id, (long)count, newest);
				}
				else
				{
					this.entries = this.history.GetEntries(0L, (long)count, newest);
				}
			}
			else
			{
				WildcardPattern wildcardpattern = new WildcardPattern(cmdline, WildcardOptions.IgnoreCase);
				if (!this._countParamterSpecified && WildcardPattern.ContainsWildcardCharacters(cmdline))
				{
					count = 0;
				}
				this.entries = this.history.GetEntries(wildcardpattern, (long)count, newest);
			}
			foreach (HistoryInfo historyInfo in this.entries)
			{
				if (historyInfo != null && !historyInfo.Cleared)
				{
					this.history.ClearEntry(historyInfo.Id);
				}
			}
		}

		// Token: 0x04000A14 RID: 2580
		private int[] _id;

		// Token: 0x04000A15 RID: 2581
		private string[] _commandline;

		// Token: 0x04000A16 RID: 2582
		private int _count = 32;

		// Token: 0x04000A17 RID: 2583
		private bool _countParamterSpecified;

		// Token: 0x04000A18 RID: 2584
		private SwitchParameter _newest;

		// Token: 0x04000A19 RID: 2585
		private History history;

		// Token: 0x04000A1A RID: 2586
		private HistoryInfo[] entries;
	}
}
