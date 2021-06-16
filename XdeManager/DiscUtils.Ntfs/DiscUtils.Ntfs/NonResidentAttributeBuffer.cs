using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000036 RID: 54
	internal class NonResidentAttributeBuffer : NonResidentDataBuffer
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000B57C File Offset: 0x0000977C
		public NonResidentAttributeBuffer(File file, NtfsAttribute attribute) : base(file.Context, NonResidentAttributeBuffer.CookRuns(attribute), (ulong)file.IndexInMft == 0UL)
		{
			this._file = file;
			this._attribute = attribute;
			AttributeFlags attributeFlags = attribute.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse);
			if (attributeFlags == AttributeFlags.None)
			{
				this._activeStream = this._rawStream;
				return;
			}
			if (attributeFlags == AttributeFlags.Compressed)
			{
				this._activeStream = new CompressedClusterStream(this._context, this._attribute, this._rawStream);
				return;
			}
			if (attributeFlags == AttributeFlags.Sparse)
			{
				this._activeStream = new SparseClusterStream(this._attribute, this._rawStream);
				return;
			}
			throw new NotImplementedException("Unhandled attribute type '" + attribute.Flags + "'");
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000B632 File Offset: 0x00009832
		public override bool CanWrite
		{
			get
			{
				return this._context.RawStream.CanWrite && this._file != null;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000B651 File Offset: 0x00009851
		public override long Capacity
		{
			get
			{
				return this.PrimaryAttributeRecord.DataLength;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000B65E File Offset: 0x0000985E
		private NonResidentAttributeRecord PrimaryAttributeRecord
		{
			get
			{
				return this._attribute.PrimaryRecord as NonResidentAttributeRecord;
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B670 File Offset: 0x00009870
		public void AlignVirtualClusterCount()
		{
			this._file.MarkMftRecordDirty();
			this._activeStream.ExpandToClusters(MathUtilities.Ceil(this._attribute.Length, this._bytesPerCluster), (NonResidentAttributeRecord)this._attribute.LastExtent, false);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B6B0 File Offset: 0x000098B0
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
			this._file.MarkMftRecordDirty();
			long numVirtualClusters = MathUtilities.Ceil(value, this._bytesPerCluster);
			if (value < this.Capacity)
			{
				this.Truncate(value);
			}
			else
			{
				this._activeStream.ExpandToClusters(numVirtualClusters, (NonResidentAttributeRecord)this._attribute.LastExtent, true);
				this.PrimaryAttributeRecord.AllocatedLength = this._cookedRuns.NextVirtualCluster * this._bytesPerCluster;
			}
			this.PrimaryAttributeRecord.DataLength = value;
			if (this.PrimaryAttributeRecord.InitializedDataLength > value)
			{
				this.PrimaryAttributeRecord.InitializedDataLength = value;
			}
			this._cookedRuns.CollapseRuns();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B770 File Offset: 0x00009970
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to write to file not opened for write");
			}
			if (count == 0)
			{
				return;
			}
			if (pos + (long)count > this.Capacity)
			{
				this.SetCapacity(pos + (long)count);
			}
			if (pos > this.PrimaryAttributeRecord.InitializedDataLength)
			{
				this.InitializeData(pos);
			}
			int num = 0;
			long num2 = pos;
			while (num2 < pos + (long)count)
			{
				long num3 = num2 / this._bytesPerCluster;
				long num4 = pos + (long)count - num2;
				long num5 = num2 - num3 * this._bytesPerCluster;
				if (num3 * this._bytesPerCluster != num2 || num4 < this._bytesPerCluster)
				{
					int num6 = (int)Math.Min(num4, this._bytesPerCluster - num5);
					this._activeStream.ReadClusters(num3, 1, this._ioBuffer, 0);
					Array.Copy(buffer, (int)((long)offset + (num2 - pos)), this._ioBuffer, (int)num5, num6);
					num += this._activeStream.WriteClusters(num3, 1, this._ioBuffer, 0);
					num2 += (long)num6;
				}
				else
				{
					int num7 = (int)(num4 / this._bytesPerCluster);
					num += this._activeStream.WriteClusters(num3, num7, buffer, (int)((long)offset + (num2 - pos)));
					num2 += (long)num7 * this._bytesPerCluster;
				}
			}
			if (pos + (long)count > this.PrimaryAttributeRecord.InitializedDataLength)
			{
				this._file.MarkMftRecordDirty();
				this.PrimaryAttributeRecord.InitializedDataLength = pos + (long)count;
			}
			if (pos + (long)count > this.PrimaryAttributeRecord.DataLength)
			{
				this._file.MarkMftRecordDirty();
				this.PrimaryAttributeRecord.DataLength = pos + (long)count;
			}
			if ((this._attribute.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				this.PrimaryAttributeRecord.CompressedDataSize += (long)num * this._bytesPerCluster;
			}
			this._cookedRuns.CollapseRuns();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B928 File Offset: 0x00009B28
		public override void Clear(long pos, int count)
		{
			if (!this.CanWrite)
			{
				throw new IOException("Attempt to erase bytes from file not opened for write");
			}
			if (count == 0)
			{
				return;
			}
			if (pos + (long)count > this.Capacity)
			{
				this.SetCapacity(pos + (long)count);
			}
			this._file.MarkMftRecordDirty();
			if (pos > this.PrimaryAttributeRecord.InitializedDataLength)
			{
				this.InitializeData(pos);
			}
			int num = 0;
			long num2 = pos;
			while (num2 < pos + (long)count)
			{
				long num3 = num2 / this._bytesPerCluster;
				long num4 = pos + (long)count - num2;
				long num5 = num2 - num3 * this._bytesPerCluster;
				if (num3 * this._bytesPerCluster != num2 || num4 < this._bytesPerCluster)
				{
					int num6 = (int)Math.Min(num4, this._bytesPerCluster - num5);
					if (this._activeStream.IsClusterStored(num3))
					{
						this._activeStream.ReadClusters(num3, 1, this._ioBuffer, 0);
						Array.Clear(this._ioBuffer, (int)num5, num6);
						num -= this._activeStream.WriteClusters(num3, 1, this._ioBuffer, 0);
					}
					num2 += (long)num6;
				}
				else
				{
					int num7 = (int)(num4 / this._bytesPerCluster);
					num += this._activeStream.ClearClusters(num3, num7);
					num2 += (long)num7 * this._bytesPerCluster;
				}
			}
			if (pos + (long)count > this.PrimaryAttributeRecord.InitializedDataLength)
			{
				this.PrimaryAttributeRecord.InitializedDataLength = pos + (long)count;
			}
			if (pos + (long)count > this.PrimaryAttributeRecord.DataLength)
			{
				this.PrimaryAttributeRecord.DataLength = pos + (long)count;
			}
			if ((this._attribute.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				this.PrimaryAttributeRecord.CompressedDataSize -= (long)num * this._bytesPerCluster;
			}
			this._cookedRuns.CollapseRuns();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000BACC File Offset: 0x00009CCC
		private static CookedDataRuns CookRuns(NtfsAttribute attribute)
		{
			CookedDataRuns cookedDataRuns = new CookedDataRuns();
			foreach (AttributeRecord attributeRecord in attribute.Records)
			{
				NonResidentAttributeRecord nonResidentAttributeRecord = (NonResidentAttributeRecord)attributeRecord;
				if (nonResidentAttributeRecord.StartVcn != cookedDataRuns.NextVirtualCluster)
				{
					throw new IOException("Invalid NTFS attribute - non-contiguous data runs");
				}
				cookedDataRuns.Append(nonResidentAttributeRecord.DataRuns, nonResidentAttributeRecord);
			}
			return cookedDataRuns;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000BB4C File Offset: 0x00009D4C
		private void InitializeData(long pos)
		{
			long num = this.PrimaryAttributeRecord.InitializedDataLength;
			this._file.MarkMftRecordDirty();
			int num2 = 0;
			while (num < pos)
			{
				long num3 = num / this._bytesPerCluster;
				if (num % this._bytesPerCluster != 0L || pos - num < this._bytesPerCluster)
				{
					int num4 = (int)(num - num3 * this._bytesPerCluster);
					int num5 = (int)Math.Min(this._bytesPerCluster - (long)num4, pos - num);
					if (this._activeStream.IsClusterStored(num3))
					{
						this._activeStream.ReadClusters(num3, 1, this._ioBuffer, 0);
						Array.Clear(this._ioBuffer, num4, num5);
						num2 += this._activeStream.WriteClusters(num3, 1, this._ioBuffer, 0);
					}
					num += (long)num5;
				}
				else
				{
					int num6 = (int)(pos / this._bytesPerCluster - num3);
					num2 -= this._activeStream.ClearClusters(num3, num6);
					num += (long)num6 * this._bytesPerCluster;
				}
			}
			this.PrimaryAttributeRecord.InitializedDataLength = pos;
			if ((this._attribute.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				this.PrimaryAttributeRecord.CompressedDataSize += (long)num2 * this._bytesPerCluster;
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000BC70 File Offset: 0x00009E70
		private void Truncate(long value)
		{
			long num = MathUtilities.Ceil(value, this._bytesPerCluster);
			this._activeStream.TruncateToClusters(num);
			foreach (KeyValuePair<AttributeReference, AttributeRecord> keyValuePair in new Dictionary<AttributeReference, AttributeRecord>(this._attribute.Extents))
			{
				if (keyValuePair.Value.StartVcn >= num)
				{
					NonResidentAttributeRecord nonResidentAttributeRecord = (NonResidentAttributeRecord)keyValuePair.Value;
					this._file.RemoveAttributeExtent(keyValuePair.Key);
					this._attribute.RemoveExtentCacheSafe(keyValuePair.Key);
				}
			}
			this.PrimaryAttributeRecord.LastVcn = Math.Max(0L, num - 1L);
			this.PrimaryAttributeRecord.AllocatedLength = num * this._bytesPerCluster;
			this.PrimaryAttributeRecord.DataLength = value;
			this.PrimaryAttributeRecord.InitializedDataLength = Math.Min(this.PrimaryAttributeRecord.InitializedDataLength, value);
			this._file.MarkMftRecordDirty();
		}

		// Token: 0x04000105 RID: 261
		private readonly NtfsAttribute _attribute;

		// Token: 0x04000106 RID: 262
		private readonly File _file;
	}
}
