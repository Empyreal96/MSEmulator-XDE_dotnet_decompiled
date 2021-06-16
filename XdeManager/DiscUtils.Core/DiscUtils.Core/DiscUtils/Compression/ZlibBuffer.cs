using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Compression
{
	// Token: 0x02000089 RID: 137
	internal class ZlibBuffer : DiscUtils.Streams.Buffer
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x0000DF26 File Offset: 0x0000C126
		public ZlibBuffer(Stream stream, Ownership ownership)
		{
			this._stream = stream;
			this._ownership = ownership;
			this.position = 0;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000DF43 File Offset: 0x0000C143
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0000DF50 File Offset: 0x0000C150
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0000DF5D File Offset: 0x0000C15D
		public override long Capacity
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000DF6C File Offset: 0x0000C16C
		public override int Read(long pos, byte[] buffer, int offset, int count)
		{
			if (pos != (long)this.position)
			{
				throw new NotSupportedException();
			}
			int num = this._stream.Read(buffer, offset, count);
			this.position += num;
			return num;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000DFAF File Offset: 0x0000C1AF
		public override void SetCapacity(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000DFB6 File Offset: 0x0000C1B6
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			yield return new StreamExtent(0L, this._stream.Length);
			yield break;
		}

		// Token: 0x040001BD RID: 445
		private Ownership _ownership;

		// Token: 0x040001BE RID: 446
		private readonly Stream _stream;

		// Token: 0x040001BF RID: 447
		private int position;
	}
}
