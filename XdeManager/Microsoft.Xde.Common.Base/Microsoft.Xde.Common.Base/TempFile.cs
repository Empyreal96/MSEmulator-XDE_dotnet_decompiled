using System;
using System.IO;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000025 RID: 37
	public class TempFile : IDisposable
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00003F74 File Offset: 0x00002174
		public TempFile()
		{
			this.FileName = Path.GetTempFileName();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00003F87 File Offset: 0x00002187
		public TempFile(string fileContents) : this()
		{
			File.WriteAllText(this.FileName, fileContents);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00003F9C File Offset: 0x0000219C
		~TempFile()
		{
			this.Dispose(false);
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00003FCC File Offset: 0x000021CC
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00003FD4 File Offset: 0x000021D4
		public string FileName { get; private set; }

		// Token: 0x06000191 RID: 401 RVA: 0x00003FDD File Offset: 0x000021DD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00003FEC File Offset: 0x000021EC
		private void Dispose(bool disposing)
		{
			if (!string.IsNullOrEmpty(this.FileName) && File.Exists(this.FileName))
			{
				File.Delete(this.FileName);
				this.FileName = null;
			}
		}
	}
}
