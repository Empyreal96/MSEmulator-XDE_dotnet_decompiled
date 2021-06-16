using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000002 RID: 2
	public sealed class AligningStream : WrappingMappedStream<SparseStream>
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public AligningStream(SparseStream toWrap, Ownership ownership, int blockSize) : base(toWrap, ownership, null)
		{
			this._blockSize = blockSize;
			this._alignmentBuffer = new byte[blockSize];
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206E File Offset: 0x0000026E
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002076 File Offset: 0x00000276
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

		// Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = (int)(this._position % (long)this._blockSize);
			if (num == 0 && (count % this._blockSize == 0 || this._position + (long)count == this.Length))
			{
				base.WrappedStream.Position = this._position;
				int num2 = base.WrappedStream.Read(buffer, offset, count);
				this._position += (long)num2;
				return num2;
			}
			long num3 = MathUtilities.RoundDown(this._position, (long)this._blockSize);
			long num4 = MathUtilities.RoundUp(this._position + (long)count, (long)this._blockSize);
			if (num4 - num3 > 2147483647L)
			{
				throw new IOException("Oversized read, after alignment");
			}
			byte[] array = new byte[num4 - num3];
			base.WrappedStream.Position = num3;
			int num5 = base.WrappedStream.Read(array, 0, array.Length);
			int num6 = Math.Min(count, num5 - num);
			Array.Copy(array, num, buffer, offset, num6);
			this._position += (long)num6;
			return num6;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000217C File Offset: 0x0000037C
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
				throw new IOException("Attempt to move before beginning of stream");
			}
			this._position = num;
			return this._position;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021C4 File Offset: 0x000003C4
		public override void Clear(int count)
		{
			this.DoOperation(delegate(SparseStream s, int opOffset, int opCount)
			{
				s.Clear(opCount);
			}, delegate(byte[] buffer, int offset, int opOffset, int opCount)
			{
				Array.Clear(buffer, offset, opCount);
			}, count);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002218 File Offset: 0x00000418
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.DoOperation(delegate(SparseStream s, int opOffset, int opCount)
			{
				s.Write(buffer, offset + opOffset, opCount);
			}, delegate(byte[] tempBuffer, int tempOffset, int opOffset, int opCount)
			{
				Array.Copy(buffer, offset + opOffset, tempBuffer, tempOffset, opCount);
			}, count);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002258 File Offset: 0x00000458
		private void DoOperation(AligningStream.ModifyStream modifyStream, AligningStream.ModifyBuffer modifyBuffer, int count)
		{
			int num = (int)(this._position % (long)this._blockSize);
			if (num == 0 && (count % this._blockSize == 0 || this._position + (long)count == this.Length))
			{
				base.WrappedStream.Position = this._position;
				modifyStream(base.WrappedStream, 0, count);
				this._position += (long)count;
				return;
			}
			long num2 = this._position + (long)count;
			long num3 = MathUtilities.RoundDown(this._position, (long)this._blockSize);
			if (num != 0)
			{
				base.WrappedStream.Position = num3;
				base.WrappedStream.Read(this._alignmentBuffer, 0, this._blockSize);
				modifyBuffer(this._alignmentBuffer, num, 0, Math.Min(count, this._blockSize - num));
				base.WrappedStream.Position = num3;
				base.WrappedStream.Write(this._alignmentBuffer, 0, this._blockSize);
			}
			num3 = MathUtilities.RoundUp(this._position, (long)this._blockSize);
			if (num3 >= num2)
			{
				this._position = num2;
				return;
			}
			int num4 = (int)MathUtilities.RoundDown(this._position + (long)count - num3, (long)this._blockSize);
			if (num4 > 0)
			{
				base.WrappedStream.Position = num3;
				modifyStream(base.WrappedStream, (int)(num3 - this._position), num4);
			}
			num3 += (long)num4;
			if (num3 >= num2)
			{
				this._position = num2;
				return;
			}
			base.WrappedStream.Position = num3;
			base.WrappedStream.Read(this._alignmentBuffer, 0, this._blockSize);
			modifyBuffer(this._alignmentBuffer, 0, (int)(num3 - this._position), (int)Math.Min((long)count - (num3 - this._position), num2 - num3));
			base.WrappedStream.Position = num3;
			base.WrappedStream.Write(this._alignmentBuffer, 0, this._blockSize);
			this._position = num2;
		}

		// Token: 0x04000001 RID: 1
		private readonly byte[] _alignmentBuffer;

		// Token: 0x04000002 RID: 2
		private readonly int _blockSize;

		// Token: 0x04000003 RID: 3
		private long _position;

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x06000237 RID: 567
		private delegate void ModifyStream(SparseStream stream, int opOffset, int count);

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x0600023B RID: 571
		private delegate void ModifyBuffer(byte[] buffer, int offset, int opOffset, int count);
	}
}
