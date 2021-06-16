using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000192 RID: 402
	internal abstract class ICabinetExtractor : IDisposable
	{
		// Token: 0x06001382 RID: 4994
		internal abstract bool Extract(string cabinetName, string srcPath, string destPath);

		// Token: 0x06001383 RID: 4995 RVA: 0x00078F24 File Offset: 0x00077124
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00078F33 File Offset: 0x00077133
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00078F48 File Offset: 0x00077148
		~ICabinetExtractor()
		{
			this.Dispose(false);
		}

		// Token: 0x04000850 RID: 2128
		private bool disposed;
	}
}
