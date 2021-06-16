using System;

namespace System.Management.Automation
{
	// Token: 0x02000242 RID: 578
	internal sealed class PSInformationalBuffers
	{
		// Token: 0x06001B6C RID: 7020 RVA: 0x000A16DC File Offset: 0x0009F8DC
		internal PSInformationalBuffers(Guid psInstanceId)
		{
			this.psInstanceId = psInstanceId;
			this.progress = new PSDataCollection<ProgressRecord>();
			this.verbose = new PSDataCollection<VerboseRecord>();
			this.debug = new PSDataCollection<DebugRecord>();
			this.warning = new PSDataCollection<WarningRecord>();
			this.information = new PSDataCollection<InformationRecord>();
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x000A172D File Offset: 0x0009F92D
		// (set) Token: 0x06001B6E RID: 7022 RVA: 0x000A1735 File Offset: 0x0009F935
		internal PSDataCollection<ProgressRecord> Progress
		{
			get
			{
				return this.progress;
			}
			set
			{
				this.progress = value;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x000A173E File Offset: 0x0009F93E
		// (set) Token: 0x06001B70 RID: 7024 RVA: 0x000A1746 File Offset: 0x0009F946
		internal PSDataCollection<VerboseRecord> Verbose
		{
			get
			{
				return this.verbose;
			}
			set
			{
				this.verbose = value;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x000A174F File Offset: 0x0009F94F
		// (set) Token: 0x06001B72 RID: 7026 RVA: 0x000A1757 File Offset: 0x0009F957
		internal PSDataCollection<DebugRecord> Debug
		{
			get
			{
				return this.debug;
			}
			set
			{
				this.debug = value;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001B73 RID: 7027 RVA: 0x000A1760 File Offset: 0x0009F960
		// (set) Token: 0x06001B74 RID: 7028 RVA: 0x000A1768 File Offset: 0x0009F968
		internal PSDataCollection<WarningRecord> Warning
		{
			get
			{
				return this.warning;
			}
			set
			{
				this.warning = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001B75 RID: 7029 RVA: 0x000A1771 File Offset: 0x0009F971
		// (set) Token: 0x06001B76 RID: 7030 RVA: 0x000A1779 File Offset: 0x0009F979
		internal PSDataCollection<InformationRecord> Information
		{
			get
			{
				return this.information;
			}
			set
			{
				this.information = value;
			}
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x000A1782 File Offset: 0x0009F982
		internal void AddProgress(ProgressRecord item)
		{
			if (this.progress != null)
			{
				this.progress.InternalAdd(this.psInstanceId, item);
			}
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x000A179E File Offset: 0x0009F99E
		internal void AddVerbose(VerboseRecord item)
		{
			if (this.verbose != null)
			{
				this.verbose.InternalAdd(this.psInstanceId, item);
			}
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000A17BA File Offset: 0x0009F9BA
		internal void AddDebug(DebugRecord item)
		{
			if (this.debug != null)
			{
				this.debug.InternalAdd(this.psInstanceId, item);
			}
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000A17D6 File Offset: 0x0009F9D6
		internal void AddWarning(WarningRecord item)
		{
			if (this.warning != null)
			{
				this.warning.InternalAdd(this.psInstanceId, item);
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000A17F2 File Offset: 0x0009F9F2
		internal void AddInformation(InformationRecord item)
		{
			if (this.information != null)
			{
				this.information.InternalAdd(this.psInstanceId, item);
			}
		}

		// Token: 0x04000B3A RID: 2874
		private Guid psInstanceId;

		// Token: 0x04000B3B RID: 2875
		internal PSDataCollection<ProgressRecord> progress;

		// Token: 0x04000B3C RID: 2876
		internal PSDataCollection<VerboseRecord> verbose;

		// Token: 0x04000B3D RID: 2877
		internal PSDataCollection<DebugRecord> debug;

		// Token: 0x04000B3E RID: 2878
		private PSDataCollection<WarningRecord> warning;

		// Token: 0x04000B3F RID: 2879
		private PSDataCollection<InformationRecord> information;
	}
}
