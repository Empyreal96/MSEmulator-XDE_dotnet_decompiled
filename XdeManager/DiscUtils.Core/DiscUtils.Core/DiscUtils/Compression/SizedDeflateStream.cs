using System;
using System.IO;
using System.IO.Compression;

namespace DiscUtils.Compression
{
	// Token: 0x02000088 RID: 136
	internal class SizedDeflateStream : DeflateStream
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x0000DECA File Offset: 0x0000C0CA
		public SizedDeflateStream(Stream stream, CompressionMode mode, bool leaveOpen, int length) : base(stream, mode, leaveOpen)
		{
			this._length = length;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0000DEDD File Offset: 0x0000C0DD
		public override long Length
		{
			get
			{
				return (long)this._length;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000DEE6 File Offset: 0x0000C0E6
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0000DEEF File Offset: 0x0000C0EF
		public override long Position
		{
			get
			{
				return (long)this._position;
			}
			set
			{
				if (value != this.Position)
				{
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000DF00 File Offset: 0x0000C100
		public override int Read(byte[] array, int offset, int count)
		{
			int num = base.Read(array, offset, count);
			this._position += num;
			return num;
		}

		// Token: 0x040001BB RID: 443
		private readonly int _length;

		// Token: 0x040001BC RID: 444
		private int _position;
	}
}
