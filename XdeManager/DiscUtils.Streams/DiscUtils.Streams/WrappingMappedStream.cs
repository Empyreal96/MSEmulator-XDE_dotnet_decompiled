using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000038 RID: 56
	public class WrappingMappedStream<T> : MappedStream where T : Stream
	{
		// Token: 0x06000207 RID: 519 RVA: 0x000072F3 File Offset: 0x000054F3
		public WrappingMappedStream(T toWrap, Ownership ownership, IEnumerable<StreamExtent> extents)
		{
			this.WrappedStream = toWrap;
			this._ownership = ownership;
			if (extents != null)
			{
				this._extents = new List<StreamExtent>(extents);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00007318 File Offset: 0x00005518
		public override bool CanRead
		{
			get
			{
				return this.WrappedStream.CanRead;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000732A File Offset: 0x0000552A
		public override bool CanSeek
		{
			get
			{
				return this.WrappedStream.CanSeek;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000733C File Offset: 0x0000553C
		public override bool CanWrite
		{
			get
			{
				return this.WrappedStream.CanWrite;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007350 File Offset: 0x00005550
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				if (this._extents != null)
				{
					return this._extents;
				}
				SparseStream sparseStream = this.WrappedStream as SparseStream;
				if (sparseStream != null)
				{
					return sparseStream.Extents;
				}
				return new StreamExtent[]
				{
					new StreamExtent(0L, this.WrappedStream.Length)
				};
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000073A7 File Offset: 0x000055A7
		public override long Length
		{
			get
			{
				return this.WrappedStream.Length;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600020D RID: 525 RVA: 0x000073B9 File Offset: 0x000055B9
		// (set) Token: 0x0600020E RID: 526 RVA: 0x000073CB File Offset: 0x000055CB
		public override long Position
		{
			get
			{
				return this.WrappedStream.Position;
			}
			set
			{
				this.WrappedStream.Position = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000073DE File Offset: 0x000055DE
		// (set) Token: 0x06000210 RID: 528 RVA: 0x000073E6 File Offset: 0x000055E6
		private protected T WrappedStream { protected get; private set; }

		// Token: 0x06000211 RID: 529 RVA: 0x000073F0 File Offset: 0x000055F0
		public override IEnumerable<StreamExtent> MapContent(long start, long length)
		{
			MappedStream mappedStream = this.WrappedStream as MappedStream;
			if (mappedStream != null)
			{
				return mappedStream.MapContent(start, length);
			}
			return new StreamExtent[]
			{
				new StreamExtent(start, length)
			};
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000742A File Offset: 0x0000562A
		public override void Flush()
		{
			this.WrappedStream.Flush();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000743C File Offset: 0x0000563C
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.WrappedStream.Read(buffer, offset, count);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00007451 File Offset: 0x00005651
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.WrappedStream.Seek(offset, origin);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00007465 File Offset: 0x00005665
		public override void SetLength(long value)
		{
			this.WrappedStream.SetLength(value);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00007478 File Offset: 0x00005678
		public override void Clear(int count)
		{
			SparseStream sparseStream = this.WrappedStream as SparseStream;
			if (sparseStream != null)
			{
				sparseStream.Clear(count);
				return;
			}
			base.Clear(count);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000074A8 File Offset: 0x000056A8
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.WrappedStream.Write(buffer, offset, count);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000074C0 File Offset: 0x000056C0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this.WrappedStream != null && this._ownership == Ownership.Dispose)
					{
						this.WrappedStream.Dispose();
					}
					this.WrappedStream = default(T);
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0400008B RID: 139
		private readonly List<StreamExtent> _extents;

		// Token: 0x0400008C RID: 140
		private readonly Ownership _ownership;
	}
}
