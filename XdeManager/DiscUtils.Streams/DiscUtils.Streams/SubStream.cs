using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200002E RID: 46
	public class SubStream : MappedStream
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00005B00 File Offset: 0x00003D00
		public SubStream(Stream parent, long first, long length)
		{
			this._parent = parent;
			this._first = first;
			this._length = length;
			this._ownsParent = Ownership.None;
			if (this._first + this._length > this._parent.Length)
			{
				throw new ArgumentException("Substream extends beyond end of parent stream");
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00005B54 File Offset: 0x00003D54
		public SubStream(Stream parent, Ownership ownsParent, long first, long length)
		{
			this._parent = parent;
			this._ownsParent = ownsParent;
			this._first = first;
			this._length = length;
			if (this._first + this._length > this._parent.Length)
			{
				throw new ArgumentException("Substream extends beyond end of parent stream");
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00005BA9 File Offset: 0x00003DA9
		public override bool CanRead
		{
			get
			{
				return this._parent.CanRead;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00005BB6 File Offset: 0x00003DB6
		public override bool CanSeek
		{
			get
			{
				return this._parent.CanSeek;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00005BC3 File Offset: 0x00003DC3
		public override bool CanWrite
		{
			get
			{
				return this._parent.CanWrite;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00005BD0 File Offset: 0x00003DD0
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				SparseStream sparseStream = this._parent as SparseStream;
				if (sparseStream != null)
				{
					return this.OffsetExtents(sparseStream.GetExtentsInRange(this._first, this._length));
				}
				return new StreamExtent[]
				{
					new StreamExtent(0L, this._length)
				};
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00005C1B File Offset: 0x00003E1B
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00005C23 File Offset: 0x00003E23
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00005C2B File Offset: 0x00003E2B
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value <= this._length)
				{
					this._position = value;
					return;
				}
				throw new ArgumentOutOfRangeException("value", "Attempt to move beyond end of stream");
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005C4D File Offset: 0x00003E4D
		public override IEnumerable<StreamExtent> MapContent(long start, long length)
		{
			return new StreamExtent[]
			{
				new StreamExtent(start + this._first, length)
			};
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00005C66 File Offset: 0x00003E66
		public override void Flush()
		{
			this._parent.Flush();
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00005C74 File Offset: 0x00003E74
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Attempt to read negative bytes");
			}
			if (this._position > this._length)
			{
				return 0;
			}
			this._parent.Position = this._first + this._position;
			int num = this._parent.Read(buffer, offset, (int)Math.Min((long)count, Math.Min(this._length - this._position, 2147483647L)));
			this._position += (long)num;
			return num;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00005CFC File Offset: 0x00003EFC
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
			if (num < 0L)
			{
				throw new ArgumentOutOfRangeException("offset", "Attempt to move before start of stream");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005D49 File Offset: 0x00003F49
		public override void SetLength(long value)
		{
			throw new NotSupportedException("Attempt to change length of a substream");
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00005D58 File Offset: 0x00003F58
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Attempt to write negative bytes");
			}
			if (this._position + (long)count > this._length)
			{
				throw new ArgumentOutOfRangeException("count", "Attempt to write beyond end of substream");
			}
			this._parent.Position = this._first + this._position;
			this._parent.Write(buffer, offset, count);
			this._position += (long)count;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005DD0 File Offset: 0x00003FD0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._ownsParent == Ownership.Dispose)
				{
					this._parent.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005E10 File Offset: 0x00004010
		private IEnumerable<StreamExtent> OffsetExtents(IEnumerable<StreamExtent> src)
		{
			foreach (StreamExtent streamExtent in src)
			{
				yield return new StreamExtent(streamExtent.Start - this._first, streamExtent.Length);
			}
			IEnumerator<StreamExtent> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400006E RID: 110
		private readonly long _first;

		// Token: 0x0400006F RID: 111
		private readonly long _length;

		// Token: 0x04000070 RID: 112
		private readonly Ownership _ownsParent;

		// Token: 0x04000071 RID: 113
		private readonly Stream _parent;

		// Token: 0x04000072 RID: 114
		private long _position;
	}
}
