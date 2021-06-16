using System;

namespace System.Management.Automation
{
	// Token: 0x0200028A RID: 650
	public sealed class JobDataAddedEventArgs : EventArgs
	{
		// Token: 0x06001F2F RID: 7983 RVA: 0x000B4AF4 File Offset: 0x000B2CF4
		internal JobDataAddedEventArgs(Job job, PowerShellStreamType dataType, int index)
		{
			this._job = job;
			this._dataType = dataType;
			this._index = index;
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x000B4B11 File Offset: 0x000B2D11
		public Job SourceJob
		{
			get
			{
				return this._job;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x000B4B19 File Offset: 0x000B2D19
		public PowerShellStreamType DataType
		{
			get
			{
				return this._dataType;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x000B4B21 File Offset: 0x000B2D21
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x04000DB4 RID: 3508
		private readonly Job _job;

		// Token: 0x04000DB5 RID: 3509
		private readonly PowerShellStreamType _dataType;

		// Token: 0x04000DB6 RID: 3510
		private readonly int _index;
	}
}
