using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000019 RID: 25
	public class ConcatStream : SparseStream
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00003A64 File Offset: 0x00001C64
		public ConcatStream(Ownership ownsStreams, params SparseStream[] streams)
		{
			this._ownsStreams = ownsStreams;
			this._streams = streams;
			this._canWrite = true;
			for (int i = 0; i < streams.Length; i++)
			{
				if (!streams[i].CanWrite)
				{
					this._canWrite = false;
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003AAD File Offset: 0x00001CAD
		public override bool CanRead
		{
			get
			{
				this.CheckDisposed();
				return true;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003AB6 File Offset: 0x00001CB6
		public override bool CanSeek
		{
			get
			{
				this.CheckDisposed();
				return true;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003ABF File Offset: 0x00001CBF
		public override bool CanWrite
		{
			get
			{
				this.CheckDisposed();
				return this._canWrite;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003AD0 File Offset: 0x00001CD0
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				this.CheckDisposed();
				List<StreamExtent> list = new List<StreamExtent>();
				long num = 0L;
				for (int i = 0; i < this._streams.Length; i++)
				{
					foreach (StreamExtent streamExtent in this._streams[i].Extents)
					{
						list.Add(new StreamExtent(streamExtent.Start + num, streamExtent.Length));
					}
					num += this._streams[i].Length;
				}
				return list;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003B6C File Offset: 0x00001D6C
		public override long Length
		{
			get
			{
				this.CheckDisposed();
				long num = 0L;
				for (int i = 0; i < this._streams.Length; i++)
				{
					num += this._streams[i].Length;
				}
				return num;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003BA6 File Offset: 0x00001DA6
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public override long Position
		{
			get
			{
				this.CheckDisposed();
				return this._position;
			}
			set
			{
				this.CheckDisposed();
				this._position = value;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003BC4 File Offset: 0x00001DC4
		public override void Flush()
		{
			this.CheckDisposed();
			for (int i = 0; i < this._streams.Length; i++)
			{
				this._streams[i].Flush();
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			int num = 0;
			int num3;
			do
			{
				long num2;
				int activeStream = this.GetActiveStream(out num2);
				this._streams[activeStream].Position = this._position - num2;
				num3 = this._streams[activeStream].Read(buffer, offset + num, count - num);
				num += num3;
				this._position += (long)num3;
			}
			while (num3 != 0);
			return num;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003C5C File Offset: 0x00001E5C
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckDisposed();
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
			this.Position = num;
			return this.Position;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003CAC File Offset: 0x00001EAC
		public override void SetLength(long value)
		{
			this.CheckDisposed();
			long num;
			int stream = this.GetStream(this.Length, out num);
			if (value < num)
			{
				throw new IOException(string.Format(CultureInfo.InvariantCulture, "Unable to reduce stream length to less than {0}", new object[]
				{
					num
				}));
			}
			this._streams[stream].SetLength(value - num);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003D08 File Offset: 0x00001F08
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckDisposed();
			int num = 0;
			while (num != count)
			{
				long num2;
				int activeStream = this.GetActiveStream(out num2);
				long num3 = this._position - num2;
				this._streams[activeStream].Position = num3;
				int num4;
				if (activeStream == this._streams.Length - 1)
				{
					num4 = count - num;
				}
				else
				{
					num4 = (int)Math.Min((long)(count - num), this._streams[activeStream].Length - num3);
				}
				this._streams[activeStream].Write(buffer, offset + num, num4);
				num += num4;
				this._position += (long)num4;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003D9C File Offset: 0x00001F9C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._ownsStreams == Ownership.Dispose && this._streams != null)
				{
					SparseStream[] streams = this._streams;
					for (int i = 0; i < streams.Length; i++)
					{
						streams[i].Dispose();
					}
					this._streams = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003DFC File Offset: 0x00001FFC
		private int GetActiveStream(out long startPos)
		{
			return this.GetStream(this._position, out startPos);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003E0C File Offset: 0x0000200C
		private int GetStream(long targetPos, out long streamStartPos)
		{
			streamStartPos = 0L;
			int num = 0;
			while (num < this._streams.Length - 1 && streamStartPos + this._streams[num].Length <= targetPos)
			{
				streamStartPos += this._streams[num].Length;
				num++;
			}
			return num;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003E58 File Offset: 0x00002058
		private void CheckDisposed()
		{
			if (this._streams == null)
			{
				throw new ObjectDisposedException("ConcatStream");
			}
		}

		// Token: 0x0400003B RID: 59
		private readonly bool _canWrite;

		// Token: 0x0400003C RID: 60
		private readonly Ownership _ownsStreams;

		// Token: 0x0400003D RID: 61
		private long _position;

		// Token: 0x0400003E RID: 62
		private SparseStream[] _streams;
	}
}
