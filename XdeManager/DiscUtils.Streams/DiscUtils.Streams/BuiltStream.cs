using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000017 RID: 23
	public class BuiltStream : SparseStream
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x0000364C File Offset: 0x0000184C
		public BuiltStream(long length, List<BuilderExtent> extents)
		{
			this._baseStream = new ZeroStream(length);
			this._length = length;
			this._extents = extents;
			this._extents.Sort(new BuiltStream.ExtentStartComparer());
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000367E File Offset: 0x0000187E
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003681 File Offset: 0x00001881
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003684 File Offset: 0x00001884
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003687 File Offset: 0x00001887
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				foreach (BuilderExtent builderExtent in this._extents)
				{
					foreach (StreamExtent streamExtent in builderExtent.StreamExtents)
					{
						yield return streamExtent;
					}
					IEnumerator<StreamExtent> enumerator2 = null;
				}
				List<BuilderExtent>.Enumerator enumerator = default(List<BuilderExtent>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003697 File Offset: 0x00001897
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000369F File Offset: 0x0000189F
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000036A7 File Offset: 0x000018A7
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

		// Token: 0x060000B1 RID: 177 RVA: 0x000036B0 File Offset: 0x000018B0
		public override void Flush()
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000036B4 File Offset: 0x000018B4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._position >= this._length)
			{
				return 0;
			}
			if (this._position + (long)count > this._length)
			{
				count = (int)(this._length - this._position);
			}
			int num = 0;
			while (num < count && this._position < this._length)
			{
				if (this._currentExtent != null && (this._position < this._currentExtent.Start || this._position >= this._currentExtent.Start + this._currentExtent.Length))
				{
					this._currentExtent.DisposeReadState();
					this._currentExtent = null;
				}
				if (this._currentExtent == null)
				{
					using (BuiltStream.SearchExtent searchExtent = new BuiltStream.SearchExtent(this._position))
					{
						int num2 = this._extents.BinarySearch(searchExtent, new BuiltStream.ExtentRangeComparer());
						if (num2 >= 0)
						{
							BuilderExtent builderExtent = this._extents[num2];
							builderExtent.PrepareForRead();
							this._currentExtent = builderExtent;
						}
					}
				}
				int num3;
				if (this._currentExtent == null)
				{
					this._baseStream.Position = this._position;
					BuilderExtent builderExtent2 = this.FindNext(this._position);
					if (builderExtent2 != null)
					{
						num3 = this._baseStream.Read(buffer, offset + num, (int)Math.Min((long)(count - num), builderExtent2.Start - this._position));
					}
					else
					{
						num3 = this._baseStream.Read(buffer, offset + num, count - num);
					}
				}
				else
				{
					num3 = this._currentExtent.Read(this._position, buffer, offset + num, count - num);
				}
				this._position += (long)num3;
				num += num3;
				if (num3 == 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000385C File Offset: 0x00001A5C
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
			this._position = num;
			return num;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000388F File Offset: 0x00001A8F
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003896 File Offset: 0x00001A96
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000038A0 File Offset: 0x00001AA0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._currentExtent != null)
					{
						this._currentExtent.DisposeReadState();
						this._currentExtent = null;
					}
					if (this._baseStream != null)
					{
						this._baseStream.Dispose();
						this._baseStream = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003900 File Offset: 0x00001B00
		private BuilderExtent FindNext(long pos)
		{
			int i = 0;
			int num = this._extents.Count - 1;
			if (this._extents.Count == 0 || this._extents[this._extents.Count - 1].Start + this._extents[this._extents.Count - 1].Length <= pos)
			{
				return null;
			}
			while (i < num)
			{
				int num2 = (num + i) / 2;
				if (this._extents[num2].Start < pos)
				{
					i = num2 + 1;
				}
				else
				{
					if (this._extents[num2].Start <= pos)
					{
						return this._extents[num2];
					}
					num = num2;
				}
			}
			return this._extents[i];
		}

		// Token: 0x04000036 RID: 54
		private Stream _baseStream;

		// Token: 0x04000037 RID: 55
		private BuilderExtent _currentExtent;

		// Token: 0x04000038 RID: 56
		private readonly List<BuilderExtent> _extents;

		// Token: 0x04000039 RID: 57
		private readonly long _length;

		// Token: 0x0400003A RID: 58
		private long _position;

		// Token: 0x02000040 RID: 64
		private class SearchExtent : BuilderExtent
		{
			// Token: 0x0600024E RID: 590 RVA: 0x0000796B File Offset: 0x00005B6B
			public SearchExtent(long pos) : base(pos, 1L)
			{
			}

			// Token: 0x0600024F RID: 591 RVA: 0x00007976 File Offset: 0x00005B76
			public override void Dispose()
			{
			}

			// Token: 0x06000250 RID: 592 RVA: 0x00007978 File Offset: 0x00005B78
			public override void PrepareForRead()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000251 RID: 593 RVA: 0x0000797F File Offset: 0x00005B7F
			public override int Read(long diskOffset, byte[] block, int offset, int count)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000252 RID: 594 RVA: 0x00007986 File Offset: 0x00005B86
			public override void DisposeReadState()
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x02000041 RID: 65
		private class ExtentRangeComparer : IComparer<BuilderExtent>
		{
			// Token: 0x06000253 RID: 595 RVA: 0x00007990 File Offset: 0x00005B90
			public int Compare(BuilderExtent x, BuilderExtent y)
			{
				if (x == null)
				{
					throw new ArgumentNullException("x");
				}
				if (y == null)
				{
					throw new ArgumentNullException("y");
				}
				if (x.Start + x.Length <= y.Start)
				{
					return -1;
				}
				if (x.Start >= y.Start + y.Length)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000042 RID: 66
		private class ExtentStartComparer : IComparer<BuilderExtent>
		{
			// Token: 0x06000255 RID: 597 RVA: 0x000079F0 File Offset: 0x00005BF0
			public int Compare(BuilderExtent x, BuilderExtent y)
			{
				if (x == null)
				{
					throw new ArgumentNullException("x");
				}
				if (y == null)
				{
					throw new ArgumentNullException("y");
				}
				long num = x.Start - y.Start;
				if (num < 0L)
				{
					return -1;
				}
				if (num > 0L)
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
