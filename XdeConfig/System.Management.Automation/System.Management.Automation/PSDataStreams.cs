using System;

namespace System.Management.Automation
{
	// Token: 0x0200023A RID: 570
	public sealed class PSDataStreams
	{
		// Token: 0x06001ADF RID: 6879 RVA: 0x0009F977 File Offset: 0x0009DB77
		internal PSDataStreams(PowerShell powershell)
		{
			this.powershell = powershell;
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x0009F986 File Offset: 0x0009DB86
		// (set) Token: 0x06001AE1 RID: 6881 RVA: 0x0009F993 File Offset: 0x0009DB93
		public PSDataCollection<ErrorRecord> Error
		{
			get
			{
				return this.powershell.ErrorBuffer;
			}
			set
			{
				this.powershell.ErrorBuffer = value;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0009F9A1 File Offset: 0x0009DBA1
		// (set) Token: 0x06001AE3 RID: 6883 RVA: 0x0009F9AE File Offset: 0x0009DBAE
		public PSDataCollection<ProgressRecord> Progress
		{
			get
			{
				return this.powershell.ProgressBuffer;
			}
			set
			{
				this.powershell.ProgressBuffer = value;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x0009F9BC File Offset: 0x0009DBBC
		// (set) Token: 0x06001AE5 RID: 6885 RVA: 0x0009F9C9 File Offset: 0x0009DBC9
		public PSDataCollection<VerboseRecord> Verbose
		{
			get
			{
				return this.powershell.VerboseBuffer;
			}
			set
			{
				this.powershell.VerboseBuffer = value;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x0009F9D7 File Offset: 0x0009DBD7
		// (set) Token: 0x06001AE7 RID: 6887 RVA: 0x0009F9E4 File Offset: 0x0009DBE4
		public PSDataCollection<DebugRecord> Debug
		{
			get
			{
				return this.powershell.DebugBuffer;
			}
			set
			{
				this.powershell.DebugBuffer = value;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x0009F9F2 File Offset: 0x0009DBF2
		// (set) Token: 0x06001AE9 RID: 6889 RVA: 0x0009F9FF File Offset: 0x0009DBFF
		public PSDataCollection<WarningRecord> Warning
		{
			get
			{
				return this.powershell.WarningBuffer;
			}
			set
			{
				this.powershell.WarningBuffer = value;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x0009FA0D File Offset: 0x0009DC0D
		// (set) Token: 0x06001AEB RID: 6891 RVA: 0x0009FA1A File Offset: 0x0009DC1A
		public PSDataCollection<InformationRecord> Information
		{
			get
			{
				return this.powershell.InformationBuffer;
			}
			set
			{
				this.powershell.InformationBuffer = value;
			}
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0009FA28 File Offset: 0x0009DC28
		public void ClearStreams()
		{
			this.Error.Clear();
			this.Progress.Clear();
			this.Verbose.Clear();
			this.Information.Clear();
			this.Debug.Clear();
			this.Warning.Clear();
		}

		// Token: 0x04000B14 RID: 2836
		private PowerShell powershell;
	}
}
