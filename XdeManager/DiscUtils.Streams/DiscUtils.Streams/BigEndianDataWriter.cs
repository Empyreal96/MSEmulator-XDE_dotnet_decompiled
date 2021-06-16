using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000021 RID: 33
	public class BigEndianDataWriter : DataWriter
	{
		// Token: 0x060000FB RID: 251 RVA: 0x000043E7 File Offset: 0x000025E7
		public BigEndianDataWriter(Stream stream) : base(stream)
		{
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000043F0 File Offset: 0x000025F0
		public override void Write(ushort value)
		{
			base.EnsureBuffer();
			EndianUtilities.WriteBytesBigEndian(value, this._buffer, 0);
			base.FlushBuffer(2);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000440C File Offset: 0x0000260C
		public override void Write(int value)
		{
			base.EnsureBuffer();
			EndianUtilities.WriteBytesBigEndian(value, this._buffer, 0);
			base.FlushBuffer(4);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004428 File Offset: 0x00002628
		public override void Write(uint value)
		{
			base.EnsureBuffer();
			EndianUtilities.WriteBytesBigEndian(value, this._buffer, 0);
			base.FlushBuffer(4);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004444 File Offset: 0x00002644
		public override void Write(long value)
		{
			base.EnsureBuffer();
			EndianUtilities.WriteBytesBigEndian(value, this._buffer, 0);
			base.FlushBuffer(8);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004460 File Offset: 0x00002660
		public override void Write(ulong value)
		{
			base.EnsureBuffer();
			EndianUtilities.WriteBytesBigEndian(value, this._buffer, 0);
			base.FlushBuffer(8);
		}
	}
}
