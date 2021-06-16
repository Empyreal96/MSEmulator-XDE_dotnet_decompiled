using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000009 RID: 9
	public class BufferStream : SparseStream
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002F68 File Offset: 0x00001168
		public BufferStream(IBuffer buffer, FileAccess access)
		{
			this._buffer = buffer;
			this._access = access;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002F7E File Offset: 0x0000117E
		public override bool CanRead
		{
			get
			{
				return this._access != FileAccess.Write;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002F8C File Offset: 0x0000118C
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002F8F File Offset: 0x0000118F
		public override bool CanWrite
		{
			get
			{
				return this._access != FileAccess.Read;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002F9D File Offset: 0x0000119D
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this._buffer.Extents;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002FAA File Offset: 0x000011AA
		public override long Length
		{
			get
			{
				return this._buffer.Capacity;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002FB7 File Offset: 0x000011B7
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002FBF File Offset: 0x000011BF
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002FC8 File Offset: 0x000011C8
		public override void Flush()
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002FCC File Offset: 0x000011CC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new IOException("Attempt to read from write-only stream");
			}
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			int num = this._buffer.Read(this._position, buffer, offset, count);
			this._position += (long)num;
			return num;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000301C File Offset: 0x0000121C
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = offset;
			if (origin == SeekOrigin.Current)
			{
				num += this._position;
			}
			else if (origin == SeekOrigin.End)
			{
				num += this._buffer.Capacity;
			}
			if (num < 0L)
			{
				throw new IOException("Attempt to move before beginning of disk");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003069 File Offset: 0x00001269
		public override void SetLength(long value)
		{
			this._buffer.SetCapacity(value);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003077 File Offset: 0x00001277
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to write to read-only stream");
			}
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			this._buffer.Write(this._position, buffer, offset, count);
			this._position += (long)count;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000030B7 File Offset: 0x000012B7
		public override void Clear(int count)
		{
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to erase bytes in a read-only stream");
			}
			this._buffer.Clear(this._position, count);
			this._position += (long)count;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000030ED File Offset: 0x000012ED
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			return this._buffer.GetExtentsInRange(start, count);
		}

		// Token: 0x04000024 RID: 36
		private readonly FileAccess _access;

		// Token: 0x04000025 RID: 37
		private readonly IBuffer _buffer;

		// Token: 0x04000026 RID: 38
		private long _position;
	}
}
