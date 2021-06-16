using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000022 RID: 34
	public abstract class DataReader
	{
		// Token: 0x06000101 RID: 257 RVA: 0x0000447C File Offset: 0x0000267C
		public DataReader(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000448B File Offset: 0x0000268B
		public long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004498 File Offset: 0x00002698
		public long Position
		{
			get
			{
				return this._stream.Position;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000044A5 File Offset: 0x000026A5
		public void Skip(int bytes)
		{
			this.ReadBytes(bytes);
		}

		// Token: 0x06000105 RID: 261
		public abstract ushort ReadUInt16();

		// Token: 0x06000106 RID: 262
		public abstract int ReadInt32();

		// Token: 0x06000107 RID: 263
		public abstract uint ReadUInt32();

		// Token: 0x06000108 RID: 264
		public abstract long ReadInt64();

		// Token: 0x06000109 RID: 265
		public abstract ulong ReadUInt64();

		// Token: 0x0600010A RID: 266 RVA: 0x000044AF File Offset: 0x000026AF
		public virtual byte[] ReadBytes(int count)
		{
			return StreamUtilities.ReadExact(this._stream, count);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000044BD File Offset: 0x000026BD
		protected void ReadToBuffer(int count)
		{
			if (this._buffer == null)
			{
				this._buffer = new byte[8];
			}
			StreamUtilities.ReadExact(this._stream, this._buffer, 0, count);
		}

		// Token: 0x0400004B RID: 75
		private const int _bufferSize = 8;

		// Token: 0x0400004C RID: 76
		protected readonly Stream _stream;

		// Token: 0x0400004D RID: 77
		protected byte[] _buffer;
	}
}
