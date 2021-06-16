using System;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Compression
{
	// Token: 0x02000080 RID: 128
	public sealed class BZip2DecoderStream : Stream
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x0000D4C4 File Offset: 0x0000B6C4
		public BZip2DecoderStream(Stream stream, Ownership ownsStream)
		{
			this._compressedStream = stream;
			this._ownsCompressed = ownsStream;
			this._bitstream = new BigEndianBitStream(new BufferedStream(stream));
			byte[] array = new byte[]
			{
				(byte)this._bitstream.Read(8),
				(byte)this._bitstream.Read(8),
				(byte)this._bitstream.Read(8)
			};
			if (array[0] != 66 || array[1] != 90 || array[2] != 104)
			{
				throw new InvalidDataException("Bad magic at start of stream");
			}
			int num = (int)(this._bitstream.Read(8) - 48U);
			if (num < 1 || num > 9)
			{
				throw new InvalidDataException("Unexpected block size in header: " + num);
			}
			num *= 100000;
			this._rleStream = new BZip2RleStream();
			this._blockDecoder = new BZip2BlockDecoder(num);
			this._blockBuffer = new byte[num];
			if (this.ReadBlock() == 0)
			{
				this._eof = true;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000D5B6 File Offset: 0x0000B7B6
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000D5B9 File Offset: 0x0000B7B9
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000D5BF File Offset: 0x0000B7BF
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000D5C6 File Offset: 0x0000B7C6
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x0000D5CE File Offset: 0x0000B7CE
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000D5D5 File Offset: 0x0000B7D5
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000D5DC File Offset: 0x0000B7DC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("Buffer smaller than declared");
			}
			if (offset < 0)
			{
				throw new ArgumentException("Offset less than zero", "offset");
			}
			if (count < 0)
			{
				throw new ArgumentException("Count less than zero", "count");
			}
			if (this._eof)
			{
				throw new IOException("Attempt to read beyond end of stream");
			}
			if (count == 0)
			{
				return 0;
			}
			int num = this._rleStream.Read(buffer, offset, count);
			if (num == 0)
			{
				if (this._calcBlockCrc != null)
				{
					if (this._blockCrc != this._calcBlockCrc.Value)
					{
						throw new InvalidDataException("Decompression failed - block CRC mismatch");
					}
					this._calcCompoundCrc = ((this._calcCompoundCrc << 1 | this._calcCompoundCrc >> 31) ^ this._blockCrc);
				}
				if (this.ReadBlock() == 0)
				{
					this._eof = true;
					if (this._calcCompoundCrc != this._compoundCrc)
					{
						throw new InvalidDataException("Decompression failed - compound CRC");
					}
					return 0;
				}
				else
				{
					num = this._rleStream.Read(buffer, offset, count);
				}
			}
			this._calcBlockCrc.Process(buffer, offset, num);
			if (this._rleStream.AtEof)
			{
				if (this._calcBlockCrc != null && this._blockCrc != this._calcBlockCrc.Value)
				{
					throw new InvalidDataException("Decompression failed - block CRC mismatch");
				}
				this._calcCompoundCrc = ((this._calcCompoundCrc << 1 | this._calcCompoundCrc >> 31) ^ this._blockCrc);
				if (this.ReadBlock() == 0)
				{
					this._eof = true;
					if (this._calcCompoundCrc != this._compoundCrc)
					{
						throw new InvalidDataException("Decompression failed - compound CRC mismatch");
					}
					return num;
				}
			}
			this._position += (long)num;
			return num;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000D774 File Offset: 0x0000B974
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000D77B File Offset: 0x0000B97B
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000D782 File Offset: 0x0000B982
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000D78C File Offset: 0x0000B98C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._compressedStream != null && this._ownsCompressed == Ownership.Dispose)
					{
						this._compressedStream.Dispose();
					}
					this._compressedStream = null;
					if (this._rleStream != null)
					{
						this._rleStream.Dispose();
						this._rleStream = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		private int ReadBlock()
		{
			ulong num = this.ReadMarker();
			if (num == 54156738319193UL)
			{
				int num2 = this._blockDecoder.Process(this._bitstream, this._blockBuffer, 0);
				this._rleStream.Reset(this._blockBuffer, 0, num2);
				this._blockCrc = this._blockDecoder.Crc;
				this._calcBlockCrc = new Crc32BigEndian(Crc32Algorithm.Common);
				return num2;
			}
			if (num == 25779555029136UL)
			{
				this._compoundCrc = this.ReadUint();
				return 0;
			}
			throw new InvalidDataException("Found invalid marker in stream");
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000D884 File Offset: 0x0000BA84
		private uint ReadUint()
		{
			uint num = 0U;
			for (int i = 0; i < 4; i++)
			{
				num = (num << 8 | this._bitstream.Read(8));
			}
			return num;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
		private ulong ReadMarker()
		{
			ulong num = 0UL;
			for (int i = 0; i < 6; i++)
			{
				num = (num << 8 | (ulong)this._bitstream.Read(8));
			}
			return num;
		}

		// Token: 0x0400019B RID: 411
		private readonly BitStream _bitstream;

		// Token: 0x0400019C RID: 412
		private readonly byte[] _blockBuffer;

		// Token: 0x0400019D RID: 413
		private uint _blockCrc;

		// Token: 0x0400019E RID: 414
		private readonly BZip2BlockDecoder _blockDecoder;

		// Token: 0x0400019F RID: 415
		private Crc32 _calcBlockCrc;

		// Token: 0x040001A0 RID: 416
		private uint _calcCompoundCrc;

		// Token: 0x040001A1 RID: 417
		private uint _compoundCrc;

		// Token: 0x040001A2 RID: 418
		private Stream _compressedStream;

		// Token: 0x040001A3 RID: 419
		private bool _eof;

		// Token: 0x040001A4 RID: 420
		private readonly Ownership _ownsCompressed;

		// Token: 0x040001A5 RID: 421
		private long _position;

		// Token: 0x040001A6 RID: 422
		private BZip2RleStream _rleStream;
	}
}
