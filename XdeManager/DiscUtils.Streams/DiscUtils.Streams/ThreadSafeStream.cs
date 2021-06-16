using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200002F RID: 47
	public class ThreadSafeStream : SparseStream
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x00005E27 File Offset: 0x00004027
		public ThreadSafeStream(SparseStream toWrap) : this(toWrap, Ownership.None)
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005E31 File Offset: 0x00004031
		public ThreadSafeStream(SparseStream toWrap, Ownership ownership)
		{
			if (!toWrap.CanSeek)
			{
				throw new ArgumentException("Wrapped stream must support seeking", "toWrap");
			}
			this._common = new ThreadSafeStream.CommonState
			{
				WrappedStream = toWrap,
				WrappedStreamOwnership = ownership
			};
			this._ownsCommon = true;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00005E71 File Offset: 0x00004071
		private ThreadSafeStream(ThreadSafeStream toClone)
		{
			this._common = toClone._common;
			if (this._common == null)
			{
				throw new ObjectDisposedException("toClone");
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005E98 File Offset: 0x00004098
		public override bool CanRead
		{
			get
			{
				ThreadSafeStream.CommonState common = this._common;
				bool canRead;
				lock (common)
				{
					canRead = this.Wrapped.CanRead;
				}
				return canRead;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00005EE0 File Offset: 0x000040E0
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00005EE4 File Offset: 0x000040E4
		public override bool CanWrite
		{
			get
			{
				ThreadSafeStream.CommonState common = this._common;
				bool canWrite;
				lock (common)
				{
					canWrite = this.Wrapped.CanWrite;
				}
				return canWrite;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00005F2C File Offset: 0x0000412C
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				ThreadSafeStream.CommonState common = this._common;
				IEnumerable<StreamExtent> extents;
				lock (common)
				{
					extents = this.Wrapped.Extents;
				}
				return extents;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00005F74 File Offset: 0x00004174
		public override long Length
		{
			get
			{
				ThreadSafeStream.CommonState common = this._common;
				long length;
				lock (common)
				{
					length = this.Wrapped.Length;
				}
				return length;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00005FBC File Offset: 0x000041BC
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00005FC4 File Offset: 0x000041C4
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00005FCD File Offset: 0x000041CD
		private SparseStream Wrapped
		{
			get
			{
				SparseStream wrappedStream = this._common.WrappedStream;
				if (wrappedStream == null)
				{
					throw new ObjectDisposedException("ThreadSafeStream");
				}
				return wrappedStream;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005FE8 File Offset: 0x000041E8
		public SparseStream OpenView()
		{
			return new ThreadSafeStream(this);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005FF0 File Offset: 0x000041F0
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			ThreadSafeStream.CommonState common = this._common;
			IEnumerable<StreamExtent> extentsInRange;
			lock (common)
			{
				extentsInRange = this.Wrapped.GetExtentsInRange(start, count);
			}
			return extentsInRange;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000603C File Offset: 0x0000423C
		public override void Flush()
		{
			ThreadSafeStream.CommonState common = this._common;
			lock (common)
			{
				this.Wrapped.Flush();
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00006084 File Offset: 0x00004284
		public override int Read(byte[] buffer, int offset, int count)
		{
			ThreadSafeStream.CommonState common = this._common;
			int result;
			lock (common)
			{
				SparseStream wrapped = this.Wrapped;
				wrapped.Position = this._position;
				int num = wrapped.Read(buffer, offset, count);
				this._position += (long)num;
				result = num;
			}
			return result;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000060EC File Offset: 0x000042EC
		public override long Seek(long offset, SeekOrigin origin)
		{
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

		// Token: 0x060001B4 RID: 436 RVA: 0x00006134 File Offset: 0x00004334
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000613C File Offset: 0x0000433C
		public override void Write(byte[] buffer, int offset, int count)
		{
			ThreadSafeStream.CommonState common = this._common;
			lock (common)
			{
				SparseStream wrapped = this.Wrapped;
				if (this._position + (long)count > wrapped.Length)
				{
					throw new IOException("Attempt to extend stream");
				}
				wrapped.Position = this._position;
				wrapped.Write(buffer, offset, count);
				this._position += (long)count;
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000061C0 File Offset: 0x000043C0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._ownsCommon && this._common != null)
			{
				ThreadSafeStream.CommonState common = this._common;
				lock (common)
				{
					if (this._common.WrappedStreamOwnership == Ownership.Dispose)
					{
						this._common.WrappedStream.Dispose();
					}
					this._common.Dispose();
				}
			}
			this._common = null;
		}

		// Token: 0x04000073 RID: 115
		private ThreadSafeStream.CommonState _common;

		// Token: 0x04000074 RID: 116
		private readonly bool _ownsCommon;

		// Token: 0x04000075 RID: 117
		private long _position;

		// Token: 0x0200004E RID: 78
		private sealed class CommonState : IDisposable
		{
			// Token: 0x060002C2 RID: 706 RVA: 0x00008F2F File Offset: 0x0000712F
			public void Dispose()
			{
				this.WrappedStream = null;
			}

			// Token: 0x040000E8 RID: 232
			public SparseStream WrappedStream;

			// Token: 0x040000E9 RID: 233
			public Ownership WrappedStreamOwnership;
		}
	}
}
