using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x0200002C RID: 44
	public sealed class StreamPump
	{
		// Token: 0x0600016E RID: 366 RVA: 0x000051E2 File Offset: 0x000033E2
		public StreamPump()
		{
			this.SparseChunkSize = 512;
			this.BufferSize = 524288;
			this.SparseCopy = true;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005207 File Offset: 0x00003407
		public StreamPump(Stream inStream, Stream outStream, int sparseChunkSize)
		{
			this.InputStream = inStream;
			this.OutputStream = outStream;
			this.SparseChunkSize = sparseChunkSize;
			this.BufferSize = 524288;
			this.SparseCopy = true;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00005236 File Offset: 0x00003436
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000523E File Offset: 0x0000343E
		public int BufferSize { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00005247 File Offset: 0x00003447
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000524F File Offset: 0x0000344F
		public long BytesRead { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00005258 File Offset: 0x00003458
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00005260 File Offset: 0x00003460
		public long BytesWritten { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00005269 File Offset: 0x00003469
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00005271 File Offset: 0x00003471
		public Stream InputStream { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000527A File Offset: 0x0000347A
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00005282 File Offset: 0x00003482
		public Stream OutputStream { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000528B File Offset: 0x0000348B
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00005293 File Offset: 0x00003493
		public int SparseChunkSize { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000529C File Offset: 0x0000349C
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000052A4 File Offset: 0x000034A4
		public bool SparseCopy { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600017E RID: 382 RVA: 0x000052B0 File Offset: 0x000034B0
		// (remove) Token: 0x0600017F RID: 383 RVA: 0x000052E8 File Offset: 0x000034E8
		public event EventHandler<PumpProgressEventArgs> ProgressEvent;

		// Token: 0x06000180 RID: 384 RVA: 0x00005320 File Offset: 0x00003520
		public void Run()
		{
			if (this.InputStream == null)
			{
				throw new InvalidOperationException("Input stream is null");
			}
			if (this.OutputStream == null)
			{
				throw new InvalidOperationException("Output stream is null");
			}
			if (!this.OutputStream.CanSeek)
			{
				throw new InvalidOperationException("Output stream does not support seek operations");
			}
			if (this.SparseChunkSize <= 1)
			{
				throw new InvalidOperationException("Chunk size is invalid");
			}
			if (this.SparseCopy)
			{
				this.RunSparse();
				return;
			}
			this.RunNonSparse();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005394 File Offset: 0x00003594
		private static bool IsAllZeros(byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (buffer[offset + i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000053B8 File Offset: 0x000035B8
		private void RunNonSparse()
		{
			byte[] array = new byte[this.BufferSize];
			this.InputStream.Position = 0L;
			this.OutputStream.Position = 0L;
			for (int i = this.InputStream.Read(array, 0, array.Length); i > 0; i = this.InputStream.Read(array, 0, array.Length))
			{
				this.BytesRead += (long)i;
				this.OutputStream.Write(array, 0, i);
				this.BytesWritten += (long)i;
				this.RaiseProgressEvent();
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005448 File Offset: 0x00003648
		private void RunSparse()
		{
			SparseStream sparseStream = this.InputStream as SparseStream;
			if (sparseStream == null)
			{
				sparseStream = SparseStream.FromStream(this.InputStream, Ownership.None);
			}
			if (this.BufferSize > this.SparseChunkSize && this.BufferSize % this.SparseChunkSize != 0)
			{
				throw new InvalidOperationException("Buffer size is not a multiple of the sparse chunk size");
			}
			byte[] array = new byte[Math.Max(this.BufferSize, this.SparseChunkSize)];
			this.BytesRead = 0L;
			this.BytesWritten = 0L;
			foreach (StreamExtent streamExtent in sparseStream.Extents)
			{
				sparseStream.Position = streamExtent.Start;
				long num = 0L;
				while (num < streamExtent.Length)
				{
					int num2 = (int)Math.Min((long)array.Length, streamExtent.Length - num);
					StreamUtilities.ReadExact(sparseStream, array, 0, num2);
					this.BytesRead += (long)num2;
					int num3 = 0;
					for (int i = 0; i < num2; i += this.SparseChunkSize)
					{
						if (StreamPump.IsAllZeros(array, i, Math.Min(this.SparseChunkSize, num2 - i)))
						{
							if (num3 < i)
							{
								this.OutputStream.Position = streamExtent.Start + num + (long)num3;
								this.OutputStream.Write(array, num3, i - num3);
								this.BytesWritten += (long)(i - num3);
							}
							num3 = i + this.SparseChunkSize;
						}
					}
					if (num3 < num2)
					{
						this.OutputStream.Position = streamExtent.Start + num + (long)num3;
						this.OutputStream.Write(array, num3, num2 - num3);
						this.BytesWritten += (long)(num2 - num3);
					}
					num += (long)num2;
					this.RaiseProgressEvent();
				}
			}
			if (this.OutputStream.Length < sparseStream.Length)
			{
				sparseStream.Position = sparseStream.Length - 1L;
				int num4 = sparseStream.ReadByte();
				if (num4 >= 0)
				{
					this.OutputStream.Position = sparseStream.Length - 1L;
					this.OutputStream.WriteByte((byte)num4);
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00005684 File Offset: 0x00003884
		private void RaiseProgressEvent()
		{
			if (this.ProgressEvent != null)
			{
				PumpProgressEventArgs pumpProgressEventArgs = new PumpProgressEventArgs();
				pumpProgressEventArgs.BytesRead = this.BytesRead;
				pumpProgressEventArgs.BytesWritten = this.BytesWritten;
				pumpProgressEventArgs.SourcePosition = this.InputStream.Position;
				pumpProgressEventArgs.DestinationPosition = this.OutputStream.Position;
				this.ProgressEvent(this, pumpProgressEventArgs);
			}
		}
	}
}
