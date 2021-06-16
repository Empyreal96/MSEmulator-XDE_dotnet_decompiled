using System;
using System.IO;

namespace DiscUtils.Internal
{
	// Token: 0x02000076 RID: 118
	internal abstract class VirtualDiskTransport : IDisposable
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600044A RID: 1098
		public abstract bool IsRawDisk { get; }

		// Token: 0x0600044B RID: 1099 RVA: 0x0000CE4D File Offset: 0x0000B04D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600044C RID: 1100
		public abstract void Connect(Uri uri, string username, string password);

		// Token: 0x0600044D RID: 1101
		public abstract VirtualDisk OpenDisk(FileAccess access);

		// Token: 0x0600044E RID: 1102
		public abstract FileLocator GetFileLocator();

		// Token: 0x0600044F RID: 1103
		public abstract string GetFileName();

		// Token: 0x06000450 RID: 1104
		public abstract string GetExtraInfo();

		// Token: 0x06000451 RID: 1105 RVA: 0x0000CE5C File Offset: 0x0000B05C
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
