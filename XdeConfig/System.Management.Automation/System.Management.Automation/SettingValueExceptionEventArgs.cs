using System;

namespace System.Management.Automation
{
	// Token: 0x0200018D RID: 397
	public class SettingValueExceptionEventArgs : EventArgs
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x0007832C File Offset: 0x0007652C
		// (set) Token: 0x0600134C RID: 4940 RVA: 0x00078334 File Offset: 0x00076534
		public bool ShouldThrow
		{
			get
			{
				return this._shouldThrow;
			}
			set
			{
				this._shouldThrow = value;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x0007833D File Offset: 0x0007653D
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00078345 File Offset: 0x00076545
		internal SettingValueExceptionEventArgs(Exception exception)
		{
			this._exception = exception;
			this._shouldThrow = true;
		}

		// Token: 0x04000840 RID: 2112
		private bool _shouldThrow;

		// Token: 0x04000841 RID: 2113
		private Exception _exception;
	}
}
