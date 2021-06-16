using System;

namespace System.Management.Automation
{
	// Token: 0x0200018E RID: 398
	public class GettingValueExceptionEventArgs : EventArgs
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x0007835B File Offset: 0x0007655B
		// (set) Token: 0x06001350 RID: 4944 RVA: 0x00078363 File Offset: 0x00076563
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

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0007836C File Offset: 0x0007656C
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00078374 File Offset: 0x00076574
		internal GettingValueExceptionEventArgs(Exception exception)
		{
			this._exception = exception;
			this._valueReplacement = null;
			this._shouldThrow = true;
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00078391 File Offset: 0x00076591
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x00078399 File Offset: 0x00076599
		public object ValueReplacement
		{
			get
			{
				return this._valueReplacement;
			}
			set
			{
				this._valueReplacement = value;
			}
		}

		// Token: 0x04000842 RID: 2114
		private bool _shouldThrow;

		// Token: 0x04000843 RID: 2115
		private Exception _exception;

		// Token: 0x04000844 RID: 2116
		private object _valueReplacement;
	}
}
