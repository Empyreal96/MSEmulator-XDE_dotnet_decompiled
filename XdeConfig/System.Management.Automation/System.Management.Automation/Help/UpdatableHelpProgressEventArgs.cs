using System;

namespace System.Management.Automation.Help
{
	// Token: 0x020001DD RID: 477
	internal class UpdatableHelpProgressEventArgs : EventArgs
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x0008A76E File Offset: 0x0008896E
		internal UpdatableHelpProgressEventArgs(string moduleName, string status, int percent)
		{
			this._type = UpdatableHelpCommandType.UnknownCommand;
			this._progressStatus = status;
			this._progressPercent = percent;
			this._moduleName = moduleName;
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0008A792 File Offset: 0x00088992
		internal UpdatableHelpProgressEventArgs(string moduleName, UpdatableHelpCommandType type, string status, int percent)
		{
			this._type = type;
			this._progressStatus = status;
			this._progressPercent = percent;
			this._moduleName = moduleName;
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0008A7B7 File Offset: 0x000889B7
		internal string ProgressStatus
		{
			get
			{
				return this._progressStatus;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x0008A7BF File Offset: 0x000889BF
		internal int ProgressPercent
		{
			get
			{
				return this._progressPercent;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x0008A7C7 File Offset: 0x000889C7
		internal string ModuleName
		{
			get
			{
				return this._moduleName;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x0008A7CF File Offset: 0x000889CF
		// (set) Token: 0x060015E6 RID: 5606 RVA: 0x0008A7D7 File Offset: 0x000889D7
		internal UpdatableHelpCommandType CommandType
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x0400094B RID: 2379
		private string _progressStatus;

		// Token: 0x0400094C RID: 2380
		private int _progressPercent;

		// Token: 0x0400094D RID: 2381
		private string _moduleName;

		// Token: 0x0400094E RID: 2382
		private UpdatableHelpCommandType _type;
	}
}
