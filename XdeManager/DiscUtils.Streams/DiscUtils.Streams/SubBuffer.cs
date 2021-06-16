using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x0200000C RID: 12
	public class SubBuffer : Buffer
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000030FC File Offset: 0x000012FC
		public SubBuffer(IBuffer parent, long first, long length)
		{
			this._parent = parent;
			this._first = first;
			this._length = length;
			if (this._first + this._length > this._parent.Capacity)
			{
				throw new ArgumentException("Substream extends beyond end of parent stream");
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003149 File Offset: 0x00001349
		public override bool CanRead
		{
			get
			{
				return this._parent.CanRead;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003156 File Offset: 0x00001356
		public override bool CanWrite
		{
			get
			{
				return this._parent.CanWrite;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003163 File Offset: 0x00001363
		public override long Capacity
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000316B File Offset: 0x0000136B
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this.OffsetExtents(this._parent.GetExtentsInRange(this._first, this._length));
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000318A File Offset: 0x0000138A
		public override void Flush()
		{
			this._parent.Flush();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003198 File Offset: 0x00001398
		public override int Read(long pos, byte[] buffer, int offset, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Attempt to read negative bytes");
			}
			if (pos >= this._length)
			{
				return 0;
			}
			return this._parent.Read(pos + this._first, buffer, offset, (int)Math.Min((long)count, Math.Min(this._length - pos, 2147483647L)));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000031F8 File Offset: 0x000013F8
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Attempt to write negative bytes");
			}
			if (pos + (long)count > this._length)
			{
				throw new ArgumentOutOfRangeException("count", "Attempt to write beyond end of substream");
			}
			this._parent.Write(pos + this._first, buffer, offset, count);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000324E File Offset: 0x0000144E
		public override void SetCapacity(long value)
		{
			throw new NotSupportedException("Attempt to change length of a subbuffer");
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000325C File Offset: 0x0000145C
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			long num = this._first + start;
			long num2 = Math.Min(num + count, this._first + this._length);
			return this.OffsetExtents(this._parent.GetExtentsInRange(num, num2 - num));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000329D File Offset: 0x0000149D
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

		// Token: 0x04000027 RID: 39
		private readonly long _first;

		// Token: 0x04000028 RID: 40
		private readonly long _length;

		// Token: 0x04000029 RID: 41
		private readonly IBuffer _parent;
	}
}
