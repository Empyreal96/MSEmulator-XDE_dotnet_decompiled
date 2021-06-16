using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200003C RID: 60
	internal sealed class NtfsFileStream : SparseStream
	{
		// Token: 0x0600029F RID: 671 RVA: 0x0000D50D File Offset: 0x0000B70D
		public NtfsFileStream(NtfsFileSystem fileSystem, DirectoryEntry entry, AttributeType attrType, string attrName, FileAccess access)
		{
			this._entry = entry;
			this._file = fileSystem.GetFile(entry.Reference);
			this._baseStream = this._file.OpenStream(attrType, attrName, access);
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000D544 File Offset: 0x0000B744
		public override bool CanRead
		{
			get
			{
				this.AssertOpen();
				return this._baseStream.CanRead;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000D557 File Offset: 0x0000B757
		public override bool CanSeek
		{
			get
			{
				this.AssertOpen();
				return this._baseStream.CanSeek;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000D56A File Offset: 0x0000B76A
		public override bool CanWrite
		{
			get
			{
				this.AssertOpen();
				return this._baseStream.CanWrite;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000D57D File Offset: 0x0000B77D
		public override IEnumerable<StreamExtent> Extents
		{
			get
			{
				this.AssertOpen();
				return this._baseStream.Extents;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000D590 File Offset: 0x0000B790
		public override long Length
		{
			get
			{
				this.AssertOpen();
				return this._baseStream.Length;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000D5A3 File Offset: 0x0000B7A3
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
		public override long Position
		{
			get
			{
				this.AssertOpen();
				return this._baseStream.Position;
			}
			set
			{
				this.AssertOpen();
				using (new NtfsTransaction())
				{
					this._baseStream.Position = value;
				}
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		protected override void Dispose(bool disposing)
		{
			if (this._baseStream == null)
			{
				base.Dispose(disposing);
				return;
			}
			using (new NtfsTransaction())
			{
				base.Dispose(disposing);
				this._baseStream.Dispose();
				this.UpdateMetadata();
				this._baseStream = null;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000D65C File Offset: 0x0000B85C
		public override void Flush()
		{
			this.AssertOpen();
			using (new NtfsTransaction())
			{
				this._baseStream.Flush();
				this.UpdateMetadata();
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.AssertOpen();
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			int result;
			using (new NtfsTransaction())
			{
				result = this._baseStream.Read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000D6F4 File Offset: 0x0000B8F4
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.AssertOpen();
			long result;
			using (new NtfsTransaction())
			{
				result = this._baseStream.Seek(offset, origin);
			}
			return result;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000D738 File Offset: 0x0000B938
		public override void SetLength(long value)
		{
			this.AssertOpen();
			using (new NtfsTransaction())
			{
				if (value != this.Length)
				{
					this._isDirty = true;
					this._baseStream.SetLength(value);
				}
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000D78C File Offset: 0x0000B98C
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.AssertOpen();
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			using (new NtfsTransaction())
			{
				this._isDirty = true;
				this._baseStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000D7E0 File Offset: 0x0000B9E0
		public override void Clear(int count)
		{
			this.AssertOpen();
			using (new NtfsTransaction())
			{
				this._isDirty = true;
				this._baseStream.Clear(count);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000D828 File Offset: 0x0000BA28
		private void UpdateMetadata()
		{
			if (!this._file.Context.ReadOnly)
			{
				if (this._isDirty)
				{
					this._file.Modified();
				}
				else
				{
					this._file.Accessed();
				}
				this._entry.UpdateFrom(this._file);
				this._file.UpdateRecordInMft();
				this._isDirty = false;
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000D88A File Offset: 0x0000BA8A
		private void AssertOpen()
		{
			if (this._baseStream == null)
			{
				throw new ObjectDisposedException(this._entry.Details.FileName, "Attempt to use closed stream");
			}
		}

		// Token: 0x04000130 RID: 304
		private SparseStream _baseStream;

		// Token: 0x04000131 RID: 305
		private readonly DirectoryEntry _entry;

		// Token: 0x04000132 RID: 306
		private readonly File _file;

		// Token: 0x04000133 RID: 307
		private bool _isDirty;
	}
}
