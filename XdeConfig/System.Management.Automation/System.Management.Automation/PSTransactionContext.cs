using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200090A RID: 2314
	public sealed class PSTransactionContext : IDisposable
	{
		// Token: 0x060056F5 RID: 22261 RVA: 0x001C7061 File Offset: 0x001C5261
		internal PSTransactionContext(PSTransactionManager transactionManager)
		{
			this.transactionManager = transactionManager;
			transactionManager.SetActive();
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x001C7078 File Offset: 0x001C5278
		~PSTransactionContext()
		{
			this.Dispose(false);
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x001C70A8 File Offset: 0x001C52A8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x001C70B7 File Offset: 0x001C52B7
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.transactionManager.ResetActive();
			}
		}

		// Token: 0x04002E62 RID: 11874
		private PSTransactionManager transactionManager;
	}
}
