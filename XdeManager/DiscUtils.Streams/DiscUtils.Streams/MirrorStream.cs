using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200001D RID: 29
	public class MirrorStream : SparseStream
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00003EA4 File Offset: 0x000020A4
		public MirrorStream(Ownership ownsWrapped, params SparseStream[] wrapped)
		{
			this._wrapped = new List<SparseStream>(wrapped);
			this._ownsWrapped = ownsWrapped;
			this._canRead = this._wrapped[0].CanRead;
			this._canWrite = this._wrapped[0].CanWrite;
			this._canSeek = this._wrapped[0].CanSeek;
			this._length = this._wrapped[0].Length;
			foreach (SparseStream sparseStream in this._wrapped)
			{
				if (sparseStream.CanRead != this._canRead || sparseStream.CanWrite != this._canWrite || sparseStream.CanSeek != this._canSeek)
				{
					throw new ArgumentException("All mirrored streams must have the same read/write/seek permissions", "wrapped");
				}
				if (sparseStream.Length != this._length)
				{
					throw new ArgumentException("All mirrored streams must have the same length", "wrapped");
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003FC0 File Offset: 0x000021C0
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003FC8 File Offset: 0x000021C8
		public override bool CanSeek
		{
			get
			{
				return this._canSeek;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00003FD0 File Offset: 0x000021D0
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003FD8 File Offset: 0x000021D8
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				return this._wrapped[0].Extents;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00003FEB File Offset: 0x000021EB
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003FF3 File Offset: 0x000021F3
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004006 File Offset: 0x00002206
		public override long Position
		{
			get
			{
				return this._wrapped[0].Position;
			}
			set
			{
				this._wrapped[0].Position = value;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000401A File Offset: 0x0000221A
		public override void Flush()
		{
			this._wrapped[0].Flush();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000402D File Offset: 0x0000222D
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._wrapped[0].Read(buffer, offset, count);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004043 File Offset: 0x00002243
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._wrapped[0].Seek(offset, origin);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004058 File Offset: 0x00002258
		public override void SetLength(long value)
		{
			if (value != this._length)
			{
				throw new InvalidOperationException("Changing the stream length is not permitted for mirrored streams");
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004070 File Offset: 0x00002270
		public override void Clear(int count)
		{
			long position = this._wrapped[0].Position;
			if (position + (long)count > this._length)
			{
				throw new IOException("Attempt to clear beyond end of mirrored stream");
			}
			foreach (SparseStream sparseStream in this._wrapped)
			{
				sparseStream.Position = position;
				sparseStream.Clear(count);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000040F4 File Offset: 0x000022F4
		public override void Write(byte[] buffer, int offset, int count)
		{
			long position = this._wrapped[0].Position;
			if (position + (long)count > this._length)
			{
				throw new IOException("Attempt to write beyond end of mirrored stream");
			}
			foreach (SparseStream sparseStream in this._wrapped)
			{
				sparseStream.Position = position;
				sparseStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004178 File Offset: 0x00002378
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

		// Token: 0x04000040 RID: 64
		private readonly bool _canRead;

		// Token: 0x04000041 RID: 65
		private readonly bool _canSeek;

		// Token: 0x04000042 RID: 66
		private readonly bool _canWrite;

		// Token: 0x04000043 RID: 67
		private readonly long _length;

		// Token: 0x04000044 RID: 68
		private readonly Ownership _ownsWrapped;

		// Token: 0x04000045 RID: 69
		private List<SparseStream> _wrapped;
	}
}
