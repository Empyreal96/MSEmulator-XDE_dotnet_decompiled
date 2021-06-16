using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200000A RID: 10
	internal class AttributeListRecord : IDiagnosticTraceable, IByteArraySerializable, IComparable<AttributeListRecord>
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002758 File Offset: 0x00000958
		public int Size
		{
			get
			{
				return MathUtilities.RoundUp(32 + (string.IsNullOrEmpty(this.Name) ? 0 : Encoding.Unicode.GetByteCount(this.Name)), 8);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002784 File Offset: 0x00000984
		public int ReadFrom(byte[] data, int offset)
		{
			this.Type = (AttributeType)EndianUtilities.ToUInt32LittleEndian(data, offset);
			this.RecordLength = EndianUtilities.ToUInt16LittleEndian(data, offset + 4);
			this.NameLength = data[offset + 6];
			this.NameOffset = data[offset + 7];
			this.StartVcn = EndianUtilities.ToUInt64LittleEndian(data, offset + 8);
			this.BaseFileReference = new FileRecordReference(EndianUtilities.ToUInt64LittleEndian(data, offset + 16));
			this.AttributeId = EndianUtilities.ToUInt16LittleEndian(data, offset + 24);
			if (this.NameLength > 0)
			{
				this.Name = Encoding.Unicode.GetString(data, offset + (int)this.NameOffset, (int)(this.NameLength * 2));
			}
			else
			{
				this.Name = null;
			}
			if (this.RecordLength < 24)
			{
				throw new InvalidDataException("Malformed AttributeList record");
			}
			return (int)this.RecordLength;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002848 File Offset: 0x00000A48
		public void WriteTo(byte[] buffer, int offset)
		{
			this.NameOffset = 32;
			if (string.IsNullOrEmpty(this.Name))
			{
				this.NameLength = 0;
			}
			else
			{
				this.NameLength = (byte)(Encoding.Unicode.GetBytes(this.Name, 0, this.Name.Length, buffer, offset + (int)this.NameOffset) / 2);
			}
			this.RecordLength = (ushort)MathUtilities.RoundUp((int)(this.NameOffset + this.NameLength * 2), 8);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Type, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.RecordLength, buffer, offset + 4);
			buffer[offset + 6] = this.NameLength;
			buffer[offset + 7] = this.NameOffset;
			EndianUtilities.WriteBytesLittleEndian(this.StartVcn, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.BaseFileReference.Value, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.AttributeId, buffer, offset + 24);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002924 File Offset: 0x00000B24
		public int CompareTo(AttributeListRecord other)
		{
			int num = this.Type - other.Type;
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
			if (num != 0)
			{
				return num;
			}
			return (int)this.StartVcn - (int)other.StartVcn;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000296C File Offset: 0x00000B6C
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "ATTRIBUTE LIST RECORD");
			writer.WriteLine(indent + "                 Type: " + this.Type);
			writer.WriteLine(indent + "        Record Length: " + this.RecordLength);
			writer.WriteLine(indent + "                 Name: " + this.Name);
			writer.WriteLine(indent + "            Start VCN: " + this.StartVcn);
			writer.WriteLine(indent + "  Base File Reference: " + this.BaseFileReference);
			writer.WriteLine(indent + "         Attribute ID: " + this.AttributeId);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A30 File Offset: 0x00000C30
		public static AttributeListRecord FromAttribute(AttributeRecord attr, FileRecordReference mftRecord)
		{
			AttributeListRecord attributeListRecord = new AttributeListRecord
			{
				Type = attr.AttributeType,
				Name = attr.Name,
				StartVcn = 0UL,
				BaseFileReference = mftRecord,
				AttributeId = attr.AttributeId
			};
			if (attr.IsNonResident)
			{
				attributeListRecord.StartVcn = (ulong)((NonResidentAttributeRecord)attr).StartVcn;
			}
			return attributeListRecord;
		}

		// Token: 0x04000018 RID: 24
		public ushort AttributeId;

		// Token: 0x04000019 RID: 25
		public FileRecordReference BaseFileReference;

		// Token: 0x0400001A RID: 26
		public string Name;

		// Token: 0x0400001B RID: 27
		public byte NameLength;

		// Token: 0x0400001C RID: 28
		public byte NameOffset;

		// Token: 0x0400001D RID: 29
		public ushort RecordLength;

		// Token: 0x0400001E RID: 30
		public ulong StartVcn;

		// Token: 0x0400001F RID: 31
		public AttributeType Type;
	}
}
