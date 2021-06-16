using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200001D RID: 29
	internal class FileRecord : FixupRecordBase
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00007470 File Offset: 0x00005670
		public FileRecord(int sectorSize) : base("FILE", sectorSize)
		{
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000747E File Offset: 0x0000567E
		public FileRecord(int sectorSize, int recordLength, uint index) : base("FILE", sectorSize, recordLength)
		{
			this.ReInitialize(sectorSize, recordLength, index);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00007496 File Offset: 0x00005696
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000749E File Offset: 0x0000569E
		public uint AllocatedSize { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000074A7 File Offset: 0x000056A7
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000074AF File Offset: 0x000056AF
		public List<AttributeRecord> Attributes { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000074B8 File Offset: 0x000056B8
		// (set) Token: 0x0600010B RID: 267 RVA: 0x000074C0 File Offset: 0x000056C0
		public FileRecordReference BaseFile { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000074C9 File Offset: 0x000056C9
		public AttributeRecord FirstAttribute
		{
			get
			{
				if (this.Attributes.Count <= 0)
				{
					return null;
				}
				return this.Attributes[0];
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000074E7 File Offset: 0x000056E7
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000074EF File Offset: 0x000056EF
		public FileRecordFlags Flags { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000074F8 File Offset: 0x000056F8
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00007500 File Offset: 0x00005700
		public ushort HardLinkCount { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000750C File Offset: 0x0000570C
		public bool IsMftRecord
		{
			get
			{
				return (ulong)this.MasterFileTableIndex == 0UL || (this.BaseFile.MftIndex == 0L && this.BaseFile.SequenceNumber > 0);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00007547 File Offset: 0x00005747
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000754F File Offset: 0x0000574F
		public uint LoadedIndex { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00007558 File Offset: 0x00005758
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00007560 File Offset: 0x00005760
		public ulong LogFileSequenceNumber { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00007569 File Offset: 0x00005769
		public uint MasterFileTableIndex
		{
			get
			{
				if (!this._haveIndex)
				{
					return this.LoadedIndex;
				}
				return this._index;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007580 File Offset: 0x00005780
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00007588 File Offset: 0x00005788
		public ushort NextAttributeId { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00007591 File Offset: 0x00005791
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00007599 File Offset: 0x00005799
		public uint RealSize { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000075A2 File Offset: 0x000057A2
		public FileRecordReference Reference
		{
			get
			{
				return new FileRecordReference((long)((ulong)this.MasterFileTableIndex), this.SequenceNumber);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000075B6 File Offset: 0x000057B6
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000075BE File Offset: 0x000057BE
		public ushort SequenceNumber { get; set; }

		// Token: 0x0600011E RID: 286 RVA: 0x000075C8 File Offset: 0x000057C8
		public static FileAttributeFlags ConvertFlags(FileRecordFlags source)
		{
			FileAttributeFlags fileAttributeFlags = FileAttributeFlags.None;
			if ((source & FileRecordFlags.IsDirectory) != FileRecordFlags.None)
			{
				fileAttributeFlags |= FileAttributeFlags.Directory;
			}
			if ((source & FileRecordFlags.HasViewIndex) != FileRecordFlags.None)
			{
				fileAttributeFlags |= FileAttributeFlags.IndexView;
			}
			if ((source & FileRecordFlags.IsMetaFile) != FileRecordFlags.None)
			{
				fileAttributeFlags |= (FileAttributeFlags.Hidden | FileAttributeFlags.System);
			}
			return fileAttributeFlags;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000075FC File Offset: 0x000057FC
		public void ReInitialize(int sectorSize, int recordLength, uint index)
		{
			base.Initialize("FILE", sectorSize, recordLength);
			ushort sequenceNumber = this.SequenceNumber;
			this.SequenceNumber = sequenceNumber + 1;
			this.Flags = FileRecordFlags.None;
			this.AllocatedSize = (uint)recordLength;
			this.NextAttributeId = 0;
			this._index = index;
			this.HardLinkCount = 0;
			this.BaseFile = new FileRecordReference(0UL);
			this.Attributes = new List<AttributeRecord>();
			this._haveIndex = true;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000766C File Offset: 0x0000586C
		public AttributeRecord GetAttribute(ushort id)
		{
			foreach (AttributeRecord attributeRecord in this.Attributes)
			{
				if (attributeRecord.AttributeId == id)
				{
					return attributeRecord;
				}
			}
			return null;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000076C8 File Offset: 0x000058C8
		public AttributeRecord GetAttribute(AttributeType type)
		{
			return this.GetAttribute(type, null);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000076D4 File Offset: 0x000058D4
		public AttributeRecord GetAttribute(AttributeType type, string name)
		{
			foreach (AttributeRecord attributeRecord in this.Attributes)
			{
				if (attributeRecord.AttributeType == type && attributeRecord.Name == name)
				{
					return attributeRecord;
				}
			}
			return null;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007740 File Offset: 0x00005940
		public override string ToString()
		{
			foreach (AttributeRecord attributeRecord in this.Attributes)
			{
				if (attributeRecord.AttributeType == AttributeType.FileName)
				{
					return ((StructuredNtfsAttribute<FileNameRecord>)NtfsAttribute.FromRecord(null, new FileRecordReference(0UL), attributeRecord)).Content.FileName;
				}
			}
			return "No Name";
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000077C0 File Offset: 0x000059C0
		public ushort CreateAttribute(AttributeType type, string name, bool indexed, AttributeFlags flags)
		{
			ushort nextAttributeId = this.NextAttributeId;
			this.NextAttributeId = nextAttributeId + 1;
			ushort num = nextAttributeId;
			this.Attributes.Add(new ResidentAttributeRecord(type, name, num, indexed, flags));
			this.Attributes.Sort();
			return num;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007804 File Offset: 0x00005A04
		public ushort CreateNonResidentAttribute(AttributeType type, string name, AttributeFlags flags)
		{
			ushort nextAttributeId = this.NextAttributeId;
			this.NextAttributeId = nextAttributeId + 1;
			ushort num = nextAttributeId;
			this.Attributes.Add(new NonResidentAttributeRecord(type, name, num, flags, 0L, new List<DataRun>()));
			this.Attributes.Sort();
			return num;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000784C File Offset: 0x00005A4C
		public ushort CreateNonResidentAttribute(AttributeType type, string name, AttributeFlags flags, long firstCluster, ulong numClusters, uint bytesPerCluster)
		{
			ushort nextAttributeId = this.NextAttributeId;
			this.NextAttributeId = nextAttributeId + 1;
			ushort num = nextAttributeId;
			this.Attributes.Add(new NonResidentAttributeRecord(type, name, num, flags, firstCluster, numClusters, bytesPerCluster));
			this.Attributes.Sort();
			return num;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007894 File Offset: 0x00005A94
		public ushort AddAttribute(AttributeRecord attrRec)
		{
			ushort nextAttributeId = this.NextAttributeId;
			this.NextAttributeId = nextAttributeId + 1;
			attrRec.AttributeId = nextAttributeId;
			this.Attributes.Add(attrRec);
			this.Attributes.Sort();
			return attrRec.AttributeId;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000078D8 File Offset: 0x00005AD8
		public void RemoveAttribute(ushort id)
		{
			for (int i = 0; i < this.Attributes.Count; i++)
			{
				if (this.Attributes[i].AttributeId == id)
				{
					this.Attributes.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000791C File Offset: 0x00005B1C
		public void Reset()
		{
			this.Attributes.Clear();
			this.Flags = FileRecordFlags.None;
			this.HardLinkCount = 0;
			this.NextAttributeId = 0;
			this.RealSize = 0U;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007948 File Offset: 0x00005B48
		internal long GetAttributeOffset(ushort id)
		{
			int num = (int)((ushort)MathUtilities.RoundUp((this._haveIndex ? 48 : 42) + base.UpdateSequenceSize, 8));
			foreach (AttributeRecord attributeRecord in this.Attributes)
			{
				if (attributeRecord.AttributeId == id)
				{
					return (long)num;
				}
				num += attributeRecord.Size;
			}
			return -1L;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000079CC File Offset: 0x00005BCC
		internal void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "FILE RECORD (" + this.ToString() + ")");
			writer.WriteLine(indent + "              Magic: " + base.Magic);
			writer.WriteLine(indent + "  Update Seq Offset: " + base.UpdateSequenceOffset);
			writer.WriteLine(indent + "   Update Seq Count: " + base.UpdateSequenceCount);
			writer.WriteLine(indent + "  Update Seq Number: " + base.UpdateSequenceNumber);
			writer.WriteLine(indent + "   Log File Seq Num: " + this.LogFileSequenceNumber);
			writer.WriteLine(indent + "    Sequence Number: " + this.SequenceNumber);
			writer.WriteLine(indent + "    Hard Link Count: " + this.HardLinkCount);
			writer.WriteLine(indent + "              Flags: " + this.Flags);
			writer.WriteLine(indent + "   Record Real Size: " + this.RealSize);
			writer.WriteLine(indent + "  Record Alloc Size: " + this.AllocatedSize);
			writer.WriteLine(indent + "          Base File: " + this.BaseFile);
			writer.WriteLine(indent + "  Next Attribute Id: " + this.NextAttributeId);
			writer.WriteLine(indent + "    Attribute Count: " + this.Attributes.Count);
			writer.WriteLine(indent + "   Index (Self Ref): " + this._index);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007B80 File Offset: 0x00005D80
		protected override void Read(byte[] buffer, int offset)
		{
			this.LogFileSequenceNumber = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 8);
			this.SequenceNumber = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 16);
			this.HardLinkCount = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 18);
			this._firstAttributeOffset = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 20);
			this.Flags = (FileRecordFlags)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 22);
			this.RealSize = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 24);
			this.AllocatedSize = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 28);
			this.BaseFile = new FileRecordReference(EndianUtilities.ToUInt64LittleEndian(buffer, offset + 32));
			this.NextAttributeId = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 40);
			if (base.UpdateSequenceOffset >= 48)
			{
				this._index = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 44);
				this._haveIndex = true;
			}
			this.Attributes = new List<AttributeRecord>();
			int num = (int)this._firstAttributeOffset;
			for (;;)
			{
				int num2;
				AttributeRecord attributeRecord = AttributeRecord.FromBytes(buffer, num, out num2);
				if (attributeRecord == null)
				{
					break;
				}
				this.Attributes.Add(attributeRecord);
				num += num2;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007C74 File Offset: 0x00005E74
		protected override ushort Write(byte[] buffer, int offset)
		{
			ushort num = this._haveIndex ? 48 : 42;
			this._firstAttributeOffset = (ushort)MathUtilities.RoundUp((int)num + base.UpdateSequenceSize, 8);
			this.RealSize = (uint)this.CalcSize();
			EndianUtilities.WriteBytesLittleEndian(this.LogFileSequenceNumber, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.SequenceNumber, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.HardLinkCount, buffer, offset + 18);
			EndianUtilities.WriteBytesLittleEndian(this._firstAttributeOffset, buffer, offset + 20);
			EndianUtilities.WriteBytesLittleEndian((ushort)this.Flags, buffer, offset + 22);
			EndianUtilities.WriteBytesLittleEndian(this.RealSize, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(this.AllocatedSize, buffer, offset + 28);
			EndianUtilities.WriteBytesLittleEndian(this.BaseFile.Value, buffer, offset + 32);
			EndianUtilities.WriteBytesLittleEndian(this.NextAttributeId, buffer, offset + 40);
			if (this._haveIndex)
			{
				EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 42);
				EndianUtilities.WriteBytesLittleEndian(this._index, buffer, offset + 44);
			}
			int num2 = (int)this._firstAttributeOffset;
			foreach (AttributeRecord attributeRecord in this.Attributes)
			{
				num2 += attributeRecord.Write(buffer, offset + num2);
			}
			EndianUtilities.WriteBytesLittleEndian(uint.MaxValue, buffer, offset + num2);
			return num;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007DCC File Offset: 0x00005FCC
		protected override int CalcSize()
		{
			int num = (int)((ushort)MathUtilities.RoundUp((this._haveIndex ? 48 : 42) + base.UpdateSequenceSize, 8));
			foreach (AttributeRecord attributeRecord in this.Attributes)
			{
				num += attributeRecord.Size;
			}
			return MathUtilities.RoundUp(num + 4, 8);
		}

		// Token: 0x0400009E RID: 158
		private ushort _firstAttributeOffset;

		// Token: 0x0400009F RID: 159
		private bool _haveIndex;

		// Token: 0x040000A0 RID: 160
		private uint _index;
	}
}
