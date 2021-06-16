using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000039 RID: 57
	internal class NtfsAttribute : IDiagnosticTraceable
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000C934 File Offset: 0x0000AB34
		protected NtfsAttribute(File file, FileRecordReference containingFile, AttributeRecord record)
		{
			this._file = file;
			this._containingFile = containingFile;
			this._primaryRecord = record;
			this._extents = new Dictionary<AttributeReference, AttributeRecord>();
			this._extents.Add(new AttributeReference(containingFile, record.AttributeId), this._primaryRecord);
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000C984 File Offset: 0x0000AB84
		protected string AttributeTypeName
		{
			get
			{
				AttributeType attributeType = this._primaryRecord.AttributeType;
				if (attributeType <= AttributeType.VolumeName)
				{
					if (attributeType <= AttributeType.FileName)
					{
						if (attributeType == AttributeType.StandardInformation)
						{
							return "STANDARD INFORMATION";
						}
						if (attributeType == AttributeType.AttributeList)
						{
							return "ATTRIBUTE LIST";
						}
						if (attributeType == AttributeType.FileName)
						{
							return "FILE NAME";
						}
					}
					else
					{
						if (attributeType == AttributeType.ObjectId)
						{
							return "OBJECT ID";
						}
						if (attributeType == AttributeType.SecurityDescriptor)
						{
							return "SECURITY DESCRIPTOR";
						}
						if (attributeType == AttributeType.VolumeName)
						{
							return "VOLUME NAME";
						}
					}
				}
				else if (attributeType <= AttributeType.IndexRoot)
				{
					if (attributeType == AttributeType.VolumeInformation)
					{
						return "VOLUME INFORMATION";
					}
					if (attributeType == AttributeType.Data)
					{
						return "DATA";
					}
					if (attributeType == AttributeType.IndexRoot)
					{
						return "INDEX ROOT";
					}
				}
				else
				{
					if (attributeType == AttributeType.IndexAllocation)
					{
						return "INDEX ALLOCATION";
					}
					if (attributeType == AttributeType.Bitmap)
					{
						return "BITMAP";
					}
					if (attributeType == AttributeType.ReparsePoint)
					{
						return "REPARSE POINT";
					}
				}
				return "UNKNOWN";
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000CA58 File Offset: 0x0000AC58
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000CA88 File Offset: 0x0000AC88
		public long CompressedDataSize
		{
			get
			{
				NonResidentAttributeRecord nonResidentAttributeRecord = this.FirstExtent as NonResidentAttributeRecord;
				if (nonResidentAttributeRecord == null)
				{
					return this.FirstExtent.AllocatedLength;
				}
				return nonResidentAttributeRecord.CompressedDataSize;
			}
			set
			{
				NonResidentAttributeRecord nonResidentAttributeRecord = this.FirstExtent as NonResidentAttributeRecord;
				if (nonResidentAttributeRecord != null)
				{
					nonResidentAttributeRecord.CompressedDataSize = value;
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000CAAC File Offset: 0x0000ACAC
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
		public int CompressionUnitSize
		{
			get
			{
				NonResidentAttributeRecord nonResidentAttributeRecord = this.FirstExtent as NonResidentAttributeRecord;
				if (nonResidentAttributeRecord == null)
				{
					return 0;
				}
				return nonResidentAttributeRecord.CompressionUnitSize;
			}
			set
			{
				NonResidentAttributeRecord nonResidentAttributeRecord = this.FirstExtent as NonResidentAttributeRecord;
				if (nonResidentAttributeRecord != null)
				{
					nonResidentAttributeRecord.CompressionUnitSize = value;
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000CAF3 File Offset: 0x0000ACF3
		public IDictionary<AttributeReference, AttributeRecord> Extents
		{
			get
			{
				return this._extents;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000CAFC File Offset: 0x0000ACFC
		public AttributeRecord FirstExtent
		{
			get
			{
				if (this._extents != null)
				{
					foreach (KeyValuePair<AttributeReference, AttributeRecord> keyValuePair in this._extents)
					{
						NonResidentAttributeRecord nonResidentAttributeRecord = keyValuePair.Value as NonResidentAttributeRecord;
						if (nonResidentAttributeRecord == null)
						{
							return keyValuePair.Value;
						}
						if (nonResidentAttributeRecord.StartVcn == 0L)
						{
							return keyValuePair.Value;
						}
					}
				}
				throw new InvalidDataException("Attribute with no initial extent");
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000CB88 File Offset: 0x0000AD88
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000CB95 File Offset: 0x0000AD95
		public AttributeFlags Flags
		{
			get
			{
				return this._primaryRecord.Flags;
			}
			set
			{
				this._primaryRecord.Flags = value;
				this._cachedRawBuffer = null;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000CBAA File Offset: 0x0000ADAA
		public ushort Id
		{
			get
			{
				return this._primaryRecord.AttributeId;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000CBB7 File Offset: 0x0000ADB7
		public bool IsNonResident
		{
			get
			{
				return this._primaryRecord.IsNonResident;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
		public AttributeRecord LastExtent
		{
			get
			{
				AttributeRecord result = null;
				if (this._extents != null)
				{
					long num = 0L;
					foreach (KeyValuePair<AttributeReference, AttributeRecord> keyValuePair in this._extents)
					{
						NonResidentAttributeRecord nonResidentAttributeRecord = keyValuePair.Value as NonResidentAttributeRecord;
						if (nonResidentAttributeRecord == null)
						{
							return keyValuePair.Value;
						}
						if (nonResidentAttributeRecord.LastVcn >= num)
						{
							result = keyValuePair.Value;
							num = nonResidentAttributeRecord.LastVcn;
						}
					}
					return result;
				}
				return result;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000CC58 File Offset: 0x0000AE58
		public long Length
		{
			get
			{
				return this._primaryRecord.DataLength;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000CC65 File Offset: 0x0000AE65
		public string Name
		{
			get
			{
				return this._primaryRecord.Name;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000CC72 File Offset: 0x0000AE72
		public AttributeRecord PrimaryRecord
		{
			get
			{
				return this._primaryRecord;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000CC7C File Offset: 0x0000AE7C
		public IBuffer RawBuffer
		{
			get
			{
				if (this._cachedRawBuffer == null)
				{
					if (this._primaryRecord.IsNonResident)
					{
						this._cachedRawBuffer = new NonResidentAttributeBuffer(this._file, this);
					}
					else
					{
						this._cachedRawBuffer = ((ResidentAttributeRecord)this._primaryRecord).DataBuffer;
					}
				}
				return this._cachedRawBuffer;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000CCCE File Offset: 0x0000AECE
		public List<AttributeRecord> Records
		{
			get
			{
				List<AttributeRecord> list = new List<AttributeRecord>(this._extents.Values);
				list.Sort(new Comparison<AttributeRecord>(AttributeRecord.CompareStartVcns));
				return list;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000CCF2 File Offset: 0x0000AEF2
		public AttributeReference Reference
		{
			get
			{
				return new AttributeReference(this._containingFile, this._primaryRecord.AttributeId);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000CD0A File Offset: 0x0000AF0A
		public AttributeType Type
		{
			get
			{
				return this._primaryRecord.AttributeType;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000CD18 File Offset: 0x0000AF18
		public virtual void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(string.Concat(new string[]
			{
				indent,
				this.AttributeTypeName,
				" ATTRIBUTE (",
				(this.Name == null) ? "No Name" : this.Name,
				")"
			}));
			writer.WriteLine(string.Concat(new object[]
			{
				indent,
				"  Length: ",
				this._primaryRecord.DataLength,
				" bytes"
			}));
			if (this._primaryRecord.DataLength == 0L)
			{
				writer.WriteLine(indent + "    Data: <none>");
			}
			else
			{
				try
				{
					using (Stream stream = this.Open(FileAccess.Read))
					{
						string text = string.Empty;
						byte[] array = new byte[32];
						int num = stream.Read(array, 0, array.Length);
						for (int i = 0; i < num; i++)
						{
							text += string.Format(CultureInfo.InvariantCulture, " {0:X2}", new object[]
							{
								array[i]
							});
						}
						writer.WriteLine(indent + "    Data: " + text + (((long)num < stream.Length) ? "..." : string.Empty));
					}
				}
				catch
				{
					writer.WriteLine(indent + "    Data: <can't read>");
				}
			}
			this._primaryRecord.Dump(writer, indent + "  ");
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
		public static NtfsAttribute FromRecord(File file, FileRecordReference recordFile, AttributeRecord record)
		{
			AttributeType attributeType = record.AttributeType;
			if (attributeType <= AttributeType.VolumeName)
			{
				if (attributeType <= AttributeType.FileName)
				{
					if (attributeType == AttributeType.StandardInformation)
					{
						return new StructuredNtfsAttribute<StandardInformation>(file, recordFile, record);
					}
					if (attributeType == AttributeType.AttributeList)
					{
						return new StructuredNtfsAttribute<AttributeList>(file, recordFile, record);
					}
					if (attributeType == AttributeType.FileName)
					{
						return new StructuredNtfsAttribute<FileNameRecord>(file, recordFile, record);
					}
				}
				else
				{
					if (attributeType == AttributeType.ObjectId)
					{
						return new StructuredNtfsAttribute<ObjectId>(file, recordFile, record);
					}
					if (attributeType == AttributeType.SecurityDescriptor)
					{
						return new StructuredNtfsAttribute<SecurityDescriptor>(file, recordFile, record);
					}
					if (attributeType == AttributeType.VolumeName)
					{
						return new StructuredNtfsAttribute<VolumeName>(file, recordFile, record);
					}
				}
			}
			else if (attributeType <= AttributeType.IndexRoot)
			{
				if (attributeType == AttributeType.VolumeInformation)
				{
					return new StructuredNtfsAttribute<VolumeInformation>(file, recordFile, record);
				}
				if (attributeType == AttributeType.Data)
				{
					return new NtfsAttribute(file, recordFile, record);
				}
				if (attributeType == AttributeType.IndexRoot)
				{
					return new NtfsAttribute(file, recordFile, record);
				}
			}
			else
			{
				if (attributeType == AttributeType.IndexAllocation)
				{
					return new NtfsAttribute(file, recordFile, record);
				}
				if (attributeType == AttributeType.Bitmap)
				{
					return new NtfsAttribute(file, recordFile, record);
				}
				if (attributeType == AttributeType.ReparsePoint)
				{
					return new StructuredNtfsAttribute<ReparsePointRecord>(file, recordFile, record);
				}
			}
			return new NtfsAttribute(file, recordFile, record);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000CF9C File Offset: 0x0000B19C
		public void SetExtent(FileRecordReference containingFile, AttributeRecord record)
		{
			this._cachedRawBuffer = null;
			this._containingFile = containingFile;
			this._primaryRecord = record;
			this._extents.Clear();
			this._extents.Add(new AttributeReference(containingFile, record.AttributeId), record);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000CFD6 File Offset: 0x0000B1D6
		public void AddExtent(FileRecordReference containingFile, AttributeRecord record)
		{
			this._cachedRawBuffer = null;
			this._extents.Add(new AttributeReference(containingFile, record.AttributeId), record);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		public void RemoveExtentCacheSafe(AttributeReference reference)
		{
			this._extents.Remove(reference);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D008 File Offset: 0x0000B208
		public bool ReplaceExtent(AttributeReference oldRef, AttributeReference newRef, AttributeRecord record)
		{
			this._cachedRawBuffer = null;
			if (!this._extents.Remove(oldRef))
			{
				return false;
			}
			if (oldRef.Equals(this.Reference) || this._extents.Count == 0)
			{
				this._primaryRecord = record;
				this._containingFile = newRef.File;
			}
			this._extents.Add(newRef, record);
			return true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D068 File Offset: 0x0000B268
		public Range<long, long>[] GetClusters()
		{
			List<Range<long, long>> list = new List<Range<long, long>>();
			foreach (KeyValuePair<AttributeReference, AttributeRecord> keyValuePair in this._extents)
			{
				list.AddRange(keyValuePair.Value.GetClusters());
			}
			return list.ToArray();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		internal SparseStream Open(FileAccess access)
		{
			return new BufferStream(this.GetDataBuffer(), access);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D0E2 File Offset: 0x0000B2E2
		internal IMappedBuffer GetDataBuffer()
		{
			return new NtfsAttributeBuffer(this._file, this);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		internal long OffsetToAbsolutePos(long offset)
		{
			return this.GetDataBuffer().MapPosition(offset);
		}

		// Token: 0x04000117 RID: 279
		private IBuffer _cachedRawBuffer;

		// Token: 0x04000118 RID: 280
		protected FileRecordReference _containingFile;

		// Token: 0x04000119 RID: 281
		protected Dictionary<AttributeReference, AttributeRecord> _extents;

		// Token: 0x0400011A RID: 282
		protected File _file;

		// Token: 0x0400011B RID: 283
		protected AttributeRecord _primaryRecord;
	}
}
