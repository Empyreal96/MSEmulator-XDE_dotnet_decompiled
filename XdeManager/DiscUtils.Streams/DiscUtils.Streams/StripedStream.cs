using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200002D RID: 45
	public class StripedStream : SparseStream
	{
		// Token: 0x06000185 RID: 389 RVA: 0x000056E8 File Offset: 0x000038E8
		public StripedStream(long stripeSize, Ownership ownsWrapped, params SparseStream[] wrapped)
		{
			this._wrapped = new List<SparseStream>(wrapped);
			this._stripeSize = stripeSize;
			this._ownsWrapped = ownsWrapped;
			this._canRead = this._wrapped[0].CanRead;
			this._canWrite = this._wrapped[0].CanWrite;
			long length = this._wrapped[0].Length;
			foreach (SparseStream sparseStream in this._wrapped)
			{
				if (sparseStream.CanRead != this._canRead || sparseStream.CanWrite != this._canWrite)
				{
					throw new ArgumentException("All striped streams must have the same read/write permissions", "wrapped");
				}
				if (sparseStream.Length != length)
				{
					throw new ArgumentException("All striped streams must have the same length", "wrapped");
				}
			}
			this._length = length * (long)wrapped.Length;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000057E8 File Offset: 0x000039E8
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000057F0 File Offset: 0x000039F0
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000057F3 File Offset: 0x000039F3
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000189 RID: 393 RVA: 0x000057FB File Offset: 0x000039FB
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				yield return new StreamExtent(0L, this._length);
				yield break;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000580B File Offset: 0x00003A0B
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00005813 File Offset: 0x00003A13
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000581B File Offset: 0x00003A1B
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

		// Token: 0x0600018D RID: 397 RVA: 0x00005824 File Offset: 0x00003A24
		public override void Flush()
		{
			foreach (SparseStream sparseStream in this._wrapped)
			{
				sparseStream.Flush();
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00005874 File Offset: 0x00003A74
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new InvalidOperationException("Attempt to read to non-readable stream");
			}
			int num = (int)Math.Min(this._length - this._position, (long)count);
			int i;
			int num5;
			for (i = 0; i < num; i += num5)
			{
				long num2 = this._position / this._stripeSize;
				long num3 = this._position % this._stripeSize;
				int count2 = (int)Math.Min((long)(num - i), this._stripeSize - num3);
				int index = (int)(num2 % (long)this._wrapped.Count);
				long num4 = num2 / (long)this._wrapped.Count;
				SparseStream sparseStream = this._wrapped[index];
				sparseStream.Position = num4 * this._stripeSize + num3;
				num5 = sparseStream.Read(buffer, offset + i, count2);
				this._position += (long)num5;
			}
			return i;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00005948 File Offset: 0x00003B48
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
				throw new IOException("Attempt to move before beginning of stream");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00005990 File Offset: 0x00003B90
		public override void SetLength(long value)
		{
			if (value != this._length)
			{
				throw new InvalidOperationException("Changing the stream length is not permitted for striped streams");
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000059A8 File Offset: 0x00003BA8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new InvalidOperationException("Attempt to write to read-only stream");
			}
			if (this._position + (long)count > this._length)
			{
				throw new IOException("Attempt to write beyond end of stream");
			}
			int num3;
			for (int i = 0; i < count; i += num3)
			{
				long num = this._position / this._stripeSize;
				long num2 = this._position % this._stripeSize;
				num3 = (int)Math.Min((long)(count - i), this._stripeSize - num2);
				int index = (int)(num % (long)this._wrapped.Count);
				long num4 = num / (long)this._wrapped.Count;
				SparseStream sparseStream = this._wrapped[index];
				sparseStream.Position = num4 * this._stripeSize + num2;
				sparseStream.Write(buffer, offset + i, num3);
				this._position += (long)num3;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00005A7C File Offset: 0x00003C7C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._ownsWrapped == Ownership.Dispose && this._wrapped != null)
				{
					foreach (SparseStream sparseStream in this._wrapped)
					{
						sparseStream.Dispose();
					}
					this._wrapped = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x04000067 RID: 103
		private readonly bool _canRead;

		// Token: 0x04000068 RID: 104
		private readonly bool _canWrite;

		// Token: 0x04000069 RID: 105
		private readonly long _length;

		// Token: 0x0400006A RID: 106
		private readonly Ownership _ownsWrapped;

		// Token: 0x0400006B RID: 107
		private long _position;

		// Token: 0x0400006C RID: 108
		private readonly long _stripeSize;

		// Token: 0x0400006D RID: 109
		private List<SparseStream> _wrapped;
	}
}
