using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200003A RID: 58
	public class ZeroStream : MappedStream
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000763C File Offset: 0x0000583C
		public ZeroStream(long length)
		{
			this._length = length;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000764B File Offset: 0x0000584B
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000764E File Offset: 0x0000584E
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00007651 File Offset: 0x00005851
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00007654 File Offset: 0x00005854
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return new List<StreamExtent>(0);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000765C File Offset: 0x0000585C
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00007664 File Offset: 0x00005864
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000766C File Offset: 0x0000586C
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
				this._atEof = false;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000767C File Offset: 0x0000587C
		public override IEnumerable<StreamExtent> MapContent(long start, long length)
		{
			return new StreamExtent[0];
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007684 File Offset: 0x00005884
		public override void Flush()
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00007688 File Offset: 0x00005888
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._position > this._length)
			{
				this._atEof = true;
				throw new IOException("Attempt to read beyond end of stream");
			}
			if (this._position != this._length)
			{
				int num = (int)Math.Min((long)count, this._length - this._position);
				Array.Clear(buffer, offset, num);
				this._position += (long)num;
				return num;
			}
			if (this._atEof)
			{
				throw new IOException("Attempt to read beyond end of stream");
			}
			this._atEof = true;
			return 0;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00007710 File Offset: 0x00005910
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = offset;
			if (origin == SeekOrigin.Current)
			{
				num += this._position;
			}
			else if (origin == SeekOrigin.End)
			{
				num += this._length;
			}
			this._atEof = false;
			if (num < 0L)
			{
				throw new IOException("Attempt to move before beginning of stream");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000775F File Offset: 0x0000595F
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00007766 File Offset: 0x00005966
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000090 RID: 144
		private bool _atEof;

		// Token: 0x04000091 RID: 145
		private readonly long _length;

		// Token: 0x04000092 RID: 146
		private long _position;
	}
}
