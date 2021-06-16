using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000028 RID: 40
	public abstract class SparseStream : Stream
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000140 RID: 320
		public abstract IEnumerable<StreamExtent> Extents { get; }

		// Token: 0x06000141 RID: 321 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public static SparseStream FromStream(Stream stream, Ownership takeOwnership)
		{
			return new SparseStream.SparseWrapperStream(stream, takeOwnership, null);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004DBA File Offset: 0x00002FBA
		public static SparseStream FromStream(Stream stream, Ownership takeOwnership, IEnumerable<StreamExtent> extents)
		{
			return new SparseStream.SparseWrapperStream(stream, takeOwnership, extents);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public static void Pump(Stream inStream, Stream outStream)
		{
			SparseStream.Pump(inStream, outStream, 512);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00004DD2 File Offset: 0x00002FD2
		public static void Pump(Stream inStream, Stream outStream, int chunkSize)
		{
			new StreamPump(inStream, outStream, chunkSize).Run();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00004DE1 File Offset: 0x00002FE1
		public static SparseStream ReadOnly(SparseStream toWrap, Ownership ownership)
		{
			return new SparseStream.SparseReadOnlyWrapperStream(toWrap, ownership);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00004DEA File Offset: 0x00002FEA
		public virtual void Clear(int count)
		{
			this.Write(new byte[count], 0, count);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00004DFA File Offset: 0x00002FFA
		public virtual IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			return StreamExtent.Intersect(new IEnumerable<StreamExtent>[]
			{
				this.Extents,
				new StreamExtent[]
				{
					new StreamExtent(start, count)
				}
			});
		}

		// Token: 0x02000045 RID: 69
		private class SparseReadOnlyWrapperStream : SparseStream
		{
			// Token: 0x0600026A RID: 618 RVA: 0x00007E2F File Offset: 0x0000602F
			public SparseReadOnlyWrapperStream(SparseStream wrapped, Ownership ownsWrapped)
			{
				this._wrapped = wrapped;
				this._ownsWrapped = ownsWrapped;
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x0600026B RID: 619 RVA: 0x00007E45 File Offset: 0x00006045
			public override bool CanRead
			{
				get
				{
					return this._wrapped.CanRead;
				}
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x0600026C RID: 620 RVA: 0x00007E52 File Offset: 0x00006052
			public override bool CanSeek
			{
				get
				{
					return this._wrapped.CanSeek;
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x0600026D RID: 621 RVA: 0x00007E5F File Offset: 0x0000605F
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x0600026E RID: 622 RVA: 0x00007E62 File Offset: 0x00006062
			public override IEnumerable<StreamExtent> Extents
			{
				get
				{
					return this._wrapped.Extents;
				}
			}

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x0600026F RID: 623 RVA: 0x00007E6F File Offset: 0x0000606F
			public override long Length
			{
				get
				{
					return this._wrapped.Length;
				}
			}

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x06000270 RID: 624 RVA: 0x00007E7C File Offset: 0x0000607C
			// (set) Token: 0x06000271 RID: 625 RVA: 0x00007E89 File Offset: 0x00006089
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

			// Token: 0x06000272 RID: 626 RVA: 0x00007E97 File Offset: 0x00006097
			public override void Flush()
			{
			}

			// Token: 0x06000273 RID: 627 RVA: 0x00007E99 File Offset: 0x00006099
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._wrapped.Read(buffer, offset, count);
			}

			// Token: 0x06000274 RID: 628 RVA: 0x00007EA9 File Offset: 0x000060A9
			public override long Seek(long offset, SeekOrigin origin)
			{
				return this._wrapped.Seek(offset, origin);
			}

			// Token: 0x06000275 RID: 629 RVA: 0x00007EB8 File Offset: 0x000060B8
			public override void SetLength(long value)
			{
				throw new InvalidOperationException("Attempt to change length of read-only stream");
			}

			// Token: 0x06000276 RID: 630 RVA: 0x00007EC4 File Offset: 0x000060C4
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new InvalidOperationException("Attempt to write to read-only stream");
			}

			// Token: 0x06000277 RID: 631 RVA: 0x00007ED0 File Offset: 0x000060D0
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (disposing && this._ownsWrapped == Ownership.Dispose && this._wrapped != null)
					{
						this._wrapped.Dispose();
						this._wrapped = null;
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x040000AF RID: 175
			private readonly Ownership _ownsWrapped;

			// Token: 0x040000B0 RID: 176
			private SparseStream _wrapped;
		}

		// Token: 0x02000046 RID: 70
		private class SparseWrapperStream : SparseStream
		{
			// Token: 0x06000278 RID: 632 RVA: 0x00007F20 File Offset: 0x00006120
			public SparseWrapperStream(Stream wrapped, Ownership ownsWrapped, IEnumerable<StreamExtent> extents)
			{
				this._wrapped = wrapped;
				this._ownsWrapped = ownsWrapped;
				if (extents != null)
				{
					this._extents = new List<StreamExtent>(extents);
				}
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x06000279 RID: 633 RVA: 0x00007F45 File Offset: 0x00006145
			public override bool CanRead
			{
				get
				{
					return this._wrapped.CanRead;
				}
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x0600027A RID: 634 RVA: 0x00007F52 File Offset: 0x00006152
			public override bool CanSeek
			{
				get
				{
					return this._wrapped.CanSeek;
				}
			}

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x0600027B RID: 635 RVA: 0x00007F5F File Offset: 0x0000615F
			public override bool CanWrite
			{
				get
				{
					return this._wrapped.CanWrite;
				}
			}

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x0600027C RID: 636 RVA: 0x00007F6C File Offset: 0x0000616C
			public override IEnumerable<StreamExtent> Extents
			{
				get
				{
					if (this._extents != null)
					{
						return this._extents;
					}
					SparseStream sparseStream = this._wrapped as SparseStream;
					if (sparseStream != null)
					{
						return sparseStream.Extents;
					}
					return new StreamExtent[]
					{
						new StreamExtent(0L, this._wrapped.Length)
					};
				}
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x0600027D RID: 637 RVA: 0x00007FB9 File Offset: 0x000061B9
			public override long Length
			{
				get
				{
					return this._wrapped.Length;
				}
			}

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x0600027E RID: 638 RVA: 0x00007FC6 File Offset: 0x000061C6
			// (set) Token: 0x0600027F RID: 639 RVA: 0x00007FD3 File Offset: 0x000061D3
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

			// Token: 0x06000280 RID: 640 RVA: 0x00007FE1 File Offset: 0x000061E1
			public override void Flush()
			{
				this._wrapped.Flush();
			}

			// Token: 0x06000281 RID: 641 RVA: 0x00007FEE File Offset: 0x000061EE
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._wrapped.Read(buffer, offset, count);
			}

			// Token: 0x06000282 RID: 642 RVA: 0x00007FFE File Offset: 0x000061FE
			public override long Seek(long offset, SeekOrigin origin)
			{
				return this._wrapped.Seek(offset, origin);
			}

			// Token: 0x06000283 RID: 643 RVA: 0x0000800D File Offset: 0x0000620D
			public override void SetLength(long value)
			{
				this._wrapped.SetLength(value);
			}

			// Token: 0x06000284 RID: 644 RVA: 0x0000801B File Offset: 0x0000621B
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (this._extents != null)
				{
					throw new InvalidOperationException("Attempt to write to stream with explicit extents");
				}
				this._wrapped.Write(buffer, offset, count);
			}

			// Token: 0x06000285 RID: 645 RVA: 0x00008040 File Offset: 0x00006240
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (disposing && this._ownsWrapped == Ownership.Dispose && this._wrapped != null)
					{
						this._wrapped.Dispose();
						this._wrapped = null;
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x040000B1 RID: 177
			private readonly List<StreamExtent> _extents;

			// Token: 0x040000B2 RID: 178
			private readonly Ownership _ownsWrapped;

			// Token: 0x040000B3 RID: 179
			private Stream _wrapped;
		}
	}
}
