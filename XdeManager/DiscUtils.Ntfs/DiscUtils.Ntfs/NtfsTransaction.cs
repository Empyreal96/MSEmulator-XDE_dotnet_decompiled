using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000043 RID: 67
	internal sealed class NtfsTransaction : IDisposable
	{
		// Token: 0x06000350 RID: 848 RVA: 0x00012BE2 File Offset: 0x00010DE2
		public NtfsTransaction()
		{
			if (NtfsTransaction._instance == null)
			{
				NtfsTransaction._instance = this;
				this.Timestamp = DateTime.UtcNow;
				this._ownRecord = true;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00012C09 File Offset: 0x00010E09
		public static NtfsTransaction Current
		{
			get
			{
				return NtfsTransaction._instance;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00012C10 File Offset: 0x00010E10
		public DateTime Timestamp { get; }

		// Token: 0x06000353 RID: 851 RVA: 0x00012C18 File Offset: 0x00010E18
		public void Dispose()
		{
			if (this._ownRecord)
			{
				NtfsTransaction._instance = null;
			}
		}

		// Token: 0x04000157 RID: 343
		[ThreadStatic]
		private static NtfsTransaction _instance;

		// Token: 0x04000158 RID: 344
		private readonly bool _ownRecord;
	}
}
