using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000023 RID: 35
	internal sealed class GenericFixupRecord : FixupRecordBase
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00008320 File Offset: 0x00006520
		public GenericFixupRecord(int bytesPerSector) : base(null, bytesPerSector)
		{
			this._bytesPerSector = bytesPerSector;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00008331 File Offset: 0x00006531
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00008339 File Offset: 0x00006539
		public byte[] Content { get; private set; }

		// Token: 0x0600015E RID: 350 RVA: 0x00008342 File Offset: 0x00006542
		protected override void Read(byte[] buffer, int offset)
		{
			this.Content = new byte[(int)(base.UpdateSequenceCount - 1) * this._bytesPerSector];
			Array.Copy(buffer, offset, this.Content, 0, this.Content.Length);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008374 File Offset: 0x00006574
		protected override ushort Write(byte[] buffer, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000837B File Offset: 0x0000657B
		protected override int CalcSize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000B8 RID: 184
		private readonly int _bytesPerSector;
	}
}
