using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000039 RID: 57
	public class WrappingStream : SparseStream
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00007520 File Offset: 0x00005720
		public WrappingStream(SparseStream toWrap, Ownership ownership)
		{
			this._wrapped = toWrap;
			this._ownership = ownership;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00007536 File Offset: 0x00005736
		public override bool CanRead
		{
			get
			{
				return this._wrapped.CanRead;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00007543 File Offset: 0x00005743
		public override bool CanSeek
		{
			get
			{
				return this._wrapped.CanSeek;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00007550 File Offset: 0x00005750
		public override bool CanWrite
		{
			get
			{
				return this._wrapped.CanWrite;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000755D File Offset: 0x0000575D
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this._wrapped.Extents;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000756A File Offset: 0x0000576A
		public override long Length
		{
			get
			{
				return this._wrapped.Length;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00007577 File Offset: 0x00005777
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00007584 File Offset: 0x00005784
		public override long Position
		{
			get
			{
				return this._wrapped.Position;
			}
			set
			{
				this._wrapped.Position = value;
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00007592 File Offset: 0x00005792
		public override void Flush()
		{
			this._wrapped.Flush();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000759F File Offset: 0x0000579F
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._wrapped.Read(buffer, offset, count);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000075AF File Offset: 0x000057AF
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._wrapped.Seek(offset, origin);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000075BE File Offset: 0x000057BE
		public override void SetLength(long value)
		{
			this._wrapped.SetLength(value);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000075CC File Offset: 0x000057CC
		public override void Clear(int count)
		{
			this._wrapped.Clear(count);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000075DA File Offset: 0x000057DA
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._wrapped.Write(buffer, offset, count);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000075EC File Offset: 0x000057EC
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._wrapped != null && this._ownership == Ownership.Dispose)
					{
						this._wrapped.Dispose();
					}
					this._wrapped = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0400008E RID: 142
		private readonly Ownership _ownership;

		// Token: 0x0400008F RID: 143
		private SparseStream _wrapped;
	}
}
