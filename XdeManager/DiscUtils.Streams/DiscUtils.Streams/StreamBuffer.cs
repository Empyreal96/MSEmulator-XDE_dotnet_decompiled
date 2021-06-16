using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200002A RID: 42
	public sealed class StreamBuffer : Buffer, IDisposable
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00004E2C File Offset: 0x0000302C
		public StreamBuffer(Stream stream, Ownership ownership)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = (stream as SparseStream);
			if (this._stream == null)
			{
				this._stream = SparseStream.FromStream(stream, ownership);
				this._ownership = Ownership.Dispose;
				return;
			}
			this._ownership = ownership;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00004E7D File Offset: 0x0000307D
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00004E8A File Offset: 0x0000308A
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00004E97 File Offset: 0x00003097
		public override long Capacity
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00004EA4 File Offset: 0x000030A4
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this._stream.Extents;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004EB1 File Offset: 0x000030B1
		public void Dispose()
		{
			if (this._ownership == Ownership.Dispose && this._stream != null)
			{
				this._stream.Dispose();
				this._stream = null;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00004ED6 File Offset: 0x000030D6
		public override int Read(long pos, byte[] buffer, int offset, int count)
		{
			this._stream.Position = pos;
			return this._stream.Read(buffer, offset, count);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00004EF3 File Offset: 0x000030F3
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			this._stream.Position = pos;
			this._stream.Write(buffer, offset, count);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004F10 File Offset: 0x00003110
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004F1D File Offset: 0x0000311D
		public override void SetCapacity(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00004F2B File Offset: 0x0000312B
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			return this._stream.GetExtentsInRange(start, count);
		}

		// Token: 0x0400005B RID: 91
		private readonly Ownership _ownership;

		// Token: 0x0400005C RID: 92
		private SparseStream _stream;
	}
}
