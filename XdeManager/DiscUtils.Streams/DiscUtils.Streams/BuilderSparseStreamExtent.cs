using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x02000012 RID: 18
	public class BuilderSparseStreamExtent : BuilderExtent
	{
		// Token: 0x06000093 RID: 147 RVA: 0x0000341F File Offset: 0x0000161F
		public BuilderSparseStreamExtent(long start, SparseStream stream) : this(start, stream, Ownership.None)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000342A File Offset: 0x0000162A
		public BuilderSparseStreamExtent(long start, SparseStream stream, Ownership ownership) : base(start, stream.Length)
		{
			this._stream = stream;
			this._ownership = ownership;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003447 File Offset: 0x00001647
		public override IEnumerable<StreamExtent> StreamExtents
		{
			get
			{
				return StreamExtent.Offset(this._stream.Extents, base.Start);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000345F File Offset: 0x0000165F
		public override void Dispose()
		{
			if (this._stream != null && this._ownership == Ownership.Dispose)
			{
				this._stream.Dispose();
				this._stream = null;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003484 File Offset: 0x00001684
		public override void PrepareForRead()
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003486 File Offset: 0x00001686
		public override int Read(long diskOffset, byte[] block, int offset, int count)
		{
			this._stream.Position = diskOffset - base.Start;
			return this._stream.Read(block, offset, count);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000034AA File Offset: 0x000016AA
		public override void DisposeReadState()
		{
		}

		// Token: 0x04000030 RID: 48
		private readonly Ownership _ownership;

		// Token: 0x04000031 RID: 49
		private SparseStream _stream;
	}
}
