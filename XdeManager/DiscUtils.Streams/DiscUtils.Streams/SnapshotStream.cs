using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000025 RID: 37
	public sealed class SnapshotStream : SparseStream
	{
		// Token: 0x0600011D RID: 285 RVA: 0x000045C1 File Offset: 0x000027C1
		public SnapshotStream(Stream baseStream, Ownership owns)
		{
			this._baseStream = baseStream;
			this._baseStreamOwnership = owns;
			this._diffExtents = new List<StreamExtent>();
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000045E2 File Offset: 0x000027E2
		public override bool CanRead
		{
			get
			{
				return this._baseStream.CanRead;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000045EF File Offset: 0x000027EF
		public override bool CanSeek
		{
			get
			{
				return this._baseStream.CanSeek;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000045FC File Offset: 0x000027FC
		public override bool CanWrite
		{
			get
			{
				return this._diffStream != null || this._baseStream.CanWrite;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00004614 File Offset: 0x00002814
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				SparseStream sparseStream = this._baseStream as SparseStream;
				if (sparseStream == null)
				{
					return new StreamExtent[]
					{
						new StreamExtent(0L, this.Length)
					};
				}
				return StreamExtent.Union(new IEnumerable<StreamExtent>[]
				{
					sparseStream.Extents,
					this._diffExtents
				});
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00004664 File Offset: 0x00002864
		public override long Length
		{
			get
			{
				if (this._diffStream != null)
				{
					return this._diffStream.Length;
				}
				return this._baseStream.Length;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004685 File Offset: 0x00002885
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000468D File Offset: 0x0000288D
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

		// Token: 0x06000125 RID: 293 RVA: 0x00004696 File Offset: 0x00002896
		public void Freeze()
		{
			this._frozen = true;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000469F File Offset: 0x0000289F
		public void Thaw()
		{
			this._frozen = false;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000046A8 File Offset: 0x000028A8
		public void Snapshot()
		{
			if (this._diffStream != null)
			{
				throw new InvalidOperationException("Already have a snapshot");
			}
			this._savedPosition = this._position;
			this._diffExtents = new List<StreamExtent>();
			this._diffStream = new SparseMemoryStream();
			this._diffStream.SetLength(this._baseStream.Length);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004700 File Offset: 0x00002900
		public void RevertToSnapshot()
		{
			if (this._diffStream == null)
			{
				throw new InvalidOperationException("No snapshot");
			}
			this._diffStream = null;
			this._diffExtents = null;
			this._position = this._savedPosition;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004730 File Offset: 0x00002930
		public void ForgetSnapshot()
		{
			if (this._diffStream == null)
			{
				throw new InvalidOperationException("No snapshot");
			}
			byte[] array = new byte[8192];
			foreach (StreamExtent streamExtent in this._diffExtents)
			{
				this._diffStream.Position = streamExtent.Start;
				this._baseStream.Position = streamExtent.Start;
				int num = 0;
				while ((long)num < streamExtent.Length)
				{
					int count = (int)Math.Min(streamExtent.Length - (long)num, (long)array.Length);
					int num2 = this._diffStream.Read(array, 0, count);
					this._baseStream.Write(array, 0, num2);
					num += num2;
				}
			}
			this._diffStream = null;
			this._diffExtents = null;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004814 File Offset: 0x00002A14
		public override void Flush()
		{
			this.CheckFrozen();
			this._baseStream.Flush();
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004828 File Offset: 0x00002A28
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num;
			if (this._diffStream == null)
			{
				this._baseStream.Position = this._position;
				num = this._baseStream.Read(buffer, offset, count);
			}
			else
			{
				if (this._position > this._diffStream.Length)
				{
					throw new IOException("Attempt to read beyond end of file");
				}
				int num2 = (int)Math.Min((long)count, this._diffStream.Length - this._position);
				if (this._position < this._baseStream.Length)
				{
					int num3 = (int)Math.Min((long)num2, this._baseStream.Length - this._position);
					this._baseStream.Position = this._position;
					for (int i = 0; i < num3; i += this._baseStream.Read(buffer, offset + i, num3 - i))
					{
					}
				}
				foreach (StreamExtent streamExtent in StreamExtent.Intersect(this._diffExtents, new StreamExtent(this._position, (long)num2)))
				{
					this._diffStream.Position = streamExtent.Start;
					int num4 = 0;
					while ((long)num4 < streamExtent.Length)
					{
						num4 += this._diffStream.Read(buffer, (int)((long)offset + (streamExtent.Start - this._position) + (long)num4), (int)(streamExtent.Length - (long)num4));
					}
				}
				num = num2;
			}
			this._position += (long)num;
			return num;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000049B0 File Offset: 0x00002BB0
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckFrozen();
			long num = offset;
			if (origin == SeekOrigin.Current)
			{
				num += this._position;
			}
			else if (origin == SeekOrigin.End)
			{
				num += this.Length;
			}
			if (num < 0L)
			{
				throw new IOException("Attempt to move before beginning of disk");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000049FE File Offset: 0x00002BFE
		public override void SetLength(long value)
		{
			this.CheckFrozen();
			if (this._diffStream != null)
			{
				this._diffStream.SetLength(value);
				return;
			}
			this._baseStream.SetLength(value);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004A28 File Offset: 0x00002C28
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckFrozen();
			if (this._diffStream != null)
			{
				this._diffStream.Position = this._position;
				this._diffStream.Write(buffer, offset, count);
				this._diffExtents = new List<StreamExtent>(StreamExtent.Union(this._diffExtents, new StreamExtent(this._position, (long)count)));
				this._position += (long)count;
				return;
			}
			this._baseStream.Position = this._position;
			this._baseStream.Write(buffer, offset, count);
			this._position += (long)count;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004AC4 File Offset: 0x00002CC4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._baseStreamOwnership == Ownership.Dispose && this._baseStream != null)
				{
					this._baseStream.Dispose();
				}
				this._baseStream = null;
				if (this._diffStream != null)
				{
					this._diffStream.Dispose();
				}
				this._diffStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004B18 File Offset: 0x00002D18
		private void CheckFrozen()
		{
			if (this._frozen)
			{
				throw new InvalidOperationException("The stream is frozen");
			}
		}

		// Token: 0x04000051 RID: 81
		private Stream _baseStream;

		// Token: 0x04000052 RID: 82
		private readonly Ownership _baseStreamOwnership;

		// Token: 0x04000053 RID: 83
		private List<StreamExtent> _diffExtents;

		// Token: 0x04000054 RID: 84
		private SparseMemoryStream _diffStream;

		// Token: 0x04000055 RID: 85
		private bool _frozen;

		// Token: 0x04000056 RID: 86
		private long _position;

		// Token: 0x04000057 RID: 87
		private long _savedPosition;
	}
}
