using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003EC RID: 1004
	public sealed class WSManPluginManagedEntryInstanceWrapper : IDisposable
	{
		// Token: 0x06002D59 RID: 11609 RVA: 0x000FB545 File Offset: 0x000F9745
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000FB554 File Offset: 0x000F9754
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.initDelegateHandle.Free();
			this.disposed = true;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000FB574 File Offset: 0x000F9774
		~WSManPluginManagedEntryInstanceWrapper()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000FB5A4 File Offset: 0x000F97A4
		public IntPtr GetEntryDelegate()
		{
			WSManPluginManagedEntryInstanceWrapper.InitPluginDelegate initPluginDelegate = new WSManPluginManagedEntryInstanceWrapper.InitPluginDelegate(WSManPluginManagedEntryWrapper.InitPlugin);
			this.initDelegateHandle = GCHandle.Alloc(initPluginDelegate);
			return Marshal.GetFunctionPointerForDelegate(initPluginDelegate);
		}

		// Token: 0x040017D1 RID: 6097
		private bool disposed;

		// Token: 0x040017D2 RID: 6098
		private GCHandle initDelegateHandle;

		// Token: 0x020003ED RID: 1005
		// (Invoke) Token: 0x06002D5F RID: 11615
		private delegate int InitPluginDelegate(IntPtr wkrPtrs);
	}
}
