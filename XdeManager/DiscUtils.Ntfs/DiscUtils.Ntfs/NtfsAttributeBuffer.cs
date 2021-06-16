using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200003A RID: 58
	internal class NtfsAttributeBuffer : DiscUtils.Streams.Buffer, IMappedBuffer, IBuffer
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000D0FE File Offset: 0x0000B2FE
		public NtfsAttributeBuffer(File file, NtfsAttribute attribute)
		{
			this._file = file;
			this._attribute = attribute;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000D114 File Offset: 0x0000B314
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000D117 File Offset: 0x0000B317
		public override bool CanWrite
		{
			get
			{
				return this._file.Context.RawStream.CanWrite;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000D12E File Offset: 0x0000B32E
		public override long Capacity
		{
			get
			{
				return this._attribute.PrimaryRecord.DataLength;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000D140 File Offset: 0x0000B340
		public long MapPosition(long pos)
		{
			if (this._attribute.IsNonResident)
			{
				return ((IMappedBuffer)this._attribute.RawBuffer).MapPosition(pos);
			}
			AttributeReference attrRef = new AttributeReference(this._file.MftReference, this._attribute.PrimaryRecord.AttributeId);
			ResidentAttributeRecord residentAttributeRecord = (ResidentAttributeRecord)this._file.GetAttribute(attrRef).PrimaryRecord;
			long offset = this._file.GetAttributeOffset(attrRef) + (long)residentAttributeRecord.DataOffset + pos;
			return this._file.Context.GetFileByIndex(0L).GetAttribute(AttributeType.Data, null).OffsetToAbsolutePos(offset);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000D1E8 File Offset: 0x0000B3E8
		public override int Read(long pos, byte[] buffer, int offset, int count)
		{
			AttributeRecord primaryRecord = this._attribute.PrimaryRecord;
			if (!this.CanRead)
			{
				throw new IOException("Attempt to read from file not opened for read");
			}
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			if (pos >= this.Capacity)
			{
				return 0;
			}
			int num = (int)Math.Min((long)count, this.Capacity - pos);
			int num2 = num;
			if (pos + (long)num > primaryRecord.InitializedDataLength)
			{
				if (pos >= primaryRecord.InitializedDataLength)
				{
					Array.Clear(buffer, offset, num);
					pos += (long)num;
					return num;
				}
				Array.Clear(buffer, offset + (int)(primaryRecord.InitializedDataLength - pos), (int)(pos + (long)num2 - primaryRecord.InitializedDataLength));
				num2 = (int)(primaryRecord.InitializedDataLength - pos);
			}
			int num3;
			for (int i = 0; i < num2; i += num3)
			{
				num3 = this._attribute.RawBuffer.Read(pos + (long)i, buffer, offset + i, num2 - i);
				if (num3 == 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D2B7 File Offset: 0x0000B4B7
		public override void SetCapacity(long value)
		{
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to change length of file not opened for write");
			}
			if (value == this.Capacity)
			{
				return;
			}
			this._attribute.RawBuffer.SetCapacity(value);
			this._file.MarkMftRecordDirty();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			AttributeRecord primaryRecord = this._attribute.PrimaryRecord;
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to write to file not opened for write");
			}
			StreamUtilities.AssertBufferParameters(buffer, offset, count);
			if (count == 0)
			{
				return;
			}
			this._attribute.RawBuffer.Write(pos, buffer, offset, count);
			if (!primaryRecord.IsNonResident)
			{
				this._file.MarkMftRecordDirty();
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D358 File Offset: 0x0000B558
		public override void Clear(long pos, int count)
		{
			AttributeRecord primaryRecord = this._attribute.PrimaryRecord;
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to write to file not opened for write");
			}
			if (count == 0)
			{
				return;
			}
			this._attribute.RawBuffer.Clear(pos, count);
			if (!primaryRecord.IsNonResident)
			{
				this._file.MarkMftRecordDirty();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D3AD File Offset: 0x0000B5AD
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			return StreamExtent.Intersect(this._attribute.RawBuffer.GetExtentsInRange(start, count), new StreamExtent(0L, this.Capacity));
		}

		// Token: 0x0400011C RID: 284
		private readonly NtfsAttribute _attribute;

		// Token: 0x0400011D RID: 285
		private readonly File _file;
	}
}
