using System;
using System.IO;
using System.IO.Compression;
using DiscUtils.Streams;

namespace DiscUtils.Compression
{
	// Token: 0x0200008A RID: 138
	public class ZlibStream : Stream
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen)
		{
			this._stream = stream;
			this._mode = mode;
			if (mode == CompressionMode.Decompress)
			{
				ushort num = EndianUtilities.ToUInt16BigEndian(StreamUtilities.ReadExact(stream, 2), 0);
				if (num % 31 != 0)
				{
					throw new IOException("Invalid Zlib header found");
				}
				if ((num & 3840) != 2048)
				{
					throw new NotSupportedException("Zlib compression not using DEFLATE algorithm");
				}
				if ((num & 32) != 0)
				{
					throw new NotSupportedException("Zlib compression using preset dictionary");
				}
			}
			else
			{
				ushort num2 = 30848;
				num2 |= 31 - num2 % 31;
				byte[] buffer = new byte[2];
				EndianUtilities.WriteBytesBigEndian(num2, buffer, 0);
				stream.Write(buffer, 0, 2);
			}
			this._deflateStream = new DeflateStream(stream, mode, leaveOpen);
			this._adler32 = new Adler32();
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0000E076 File Offset: 0x0000C276
		public override bool CanRead
		{
			get
			{
				return this._deflateStream.CanRead;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0000E083 File Offset: 0x0000C283
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0000E086 File Offset: 0x0000C286
		public override bool CanWrite
		{
			get
			{
				return this._deflateStream.CanWrite;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0000E093 File Offset: 0x0000C293
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0000E09A File Offset: 0x0000C29A
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0000E0A1 File Offset: 0x0000C2A1
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
		protected override void Dispose(bool disposing)
		{
			if (this._mode == CompressionMode.Decompress)
			{
				if (this._stream.CanSeek)
				{
					this._stream.Seek(-4L, SeekOrigin.End);
					if (EndianUtilities.ToInt32BigEndian(StreamUtilities.ReadExact(this._stream, 4), 0) != this._adler32.Value)
					{
						throw new InvalidDataException("Corrupt decompressed data detected");
					}
				}
				this._deflateStream.Dispose();
			}
			else
			{
				this._deflateStream.Dispose();
				byte[] buffer = new byte[4];
				EndianUtilities.WriteBytesBigEndian(this._adler32.Value, buffer, 0);
				this._stream.Write(buffer, 0, 4);
			}
			base.Dispose(disposing);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000E14A File Offset: 0x0000C34A
		public override void Flush()
		{
			this._deflateStream.Flush();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000E158 File Offset: 0x0000C358
		public override int Read(byte[] buffer, int offset, int count)
		{
			ZlibStream.CheckParams(buffer, offset, count);
			int num = this._deflateStream.Read(buffer, offset, count);
			this._adler32.Process(buffer, offset, num);
			return num;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000E18B File Offset: 0x0000C38B
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000E192 File Offset: 0x0000C392
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000E199 File Offset: 0x0000C399
		public override void Write(byte[] buffer, int offset, int count)
		{
			ZlibStream.CheckParams(buffer, offset, count);
			this._adler32.Process(buffer, offset, count);
			this._deflateStream.Write(buffer, offset, count);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000E1C0 File Offset: 0x0000C3C0
		private static void CheckParams(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentException("Offset outside of array bounds", "offset");
			}
			if (count < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentException("Array index out of bounds", "count");
			}
		}

		// Token: 0x040001C0 RID: 448
		private readonly Adler32 _adler32;

		// Token: 0x040001C1 RID: 449
		private readonly DeflateStream _deflateStream;

		// Token: 0x040001C2 RID: 450
		private readonly CompressionMode _mode;

		// Token: 0x040001C3 RID: 451
		private readonly Stream _stream;
	}
}
