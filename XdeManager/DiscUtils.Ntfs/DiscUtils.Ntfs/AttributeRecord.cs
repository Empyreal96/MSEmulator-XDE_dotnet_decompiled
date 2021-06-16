using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200000B RID: 11
	internal abstract class AttributeRecord : IComparable<AttributeRecord>
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002A98 File Offset: 0x00000C98
		public AttributeRecord()
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public AttributeRecord(AttributeType type, string name, ushort id, AttributeFlags flags)
		{
			this._type = type;
			this._name = name;
			this._attributeId = id;
			this._flags = flags;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000028 RID: 40
		// (set) Token: 0x06000029 RID: 41
		public abstract long AllocatedLength { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002AC5 File Offset: 0x00000CC5
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002ACD File Offset: 0x00000CCD
		public ushort AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002AD6 File Offset: 0x00000CD6
		public AttributeType AttributeType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002D RID: 45
		// (set) Token: 0x0600002E RID: 46
		public abstract long DataLength { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002ADE File Offset: 0x00000CDE
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002AE6 File Offset: 0x00000CE6
		public AttributeFlags Flags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000031 RID: 49
		// (set) Token: 0x06000032 RID: 50
		public abstract long InitializedDataLength { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002AEF File Offset: 0x00000CEF
		public bool IsNonResident
		{
			get
			{
				return this._nonResidentFlag > 0;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002AFA File Offset: 0x00000CFA
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000035 RID: 53
		public abstract int Size { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54
		public abstract long StartVcn { get; }

		// Token: 0x06000037 RID: 55 RVA: 0x00002B04 File Offset: 0x00000D04
		public int CompareTo(AttributeRecord other)
		{
			int num = this._type - other._type;
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(this._name, other._name, StringComparison.OrdinalIgnoreCase);
			if (num != 0)
			{
				return num;
			}
			return (int)(this._attributeId - other._attributeId);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B49 File Offset: 0x00000D49
		public static AttributeRecord FromBytes(byte[] buffer, int offset, out int length)
		{
			if (EndianUtilities.ToUInt32LittleEndian(buffer, offset) == 4294967295U)
			{
				length = 0;
				return null;
			}
			if (buffer[offset + 8] != 0)
			{
				return new NonResidentAttributeRecord(buffer, offset, ref length);
			}
			return new ResidentAttributeRecord(buffer, offset, ref length);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B72 File Offset: 0x00000D72
		public static int CompareStartVcns(AttributeRecord x, AttributeRecord y)
		{
			if (x.StartVcn < y.StartVcn)
			{
				return -1;
			}
			if (x.StartVcn == y.StartVcn)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600003A RID: 58
		public abstract Range<long, long>[] GetClusters();

		// Token: 0x0600003B RID: 59
		public abstract IBuffer GetReadOnlyDataBuffer(INtfsContext context);

		// Token: 0x0600003C RID: 60
		public abstract int Write(byte[] buffer, int offset);

		// Token: 0x0600003D RID: 61 RVA: 0x00002B98 File Offset: 0x00000D98
		public virtual void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "ATTRIBUTE RECORD");
			writer.WriteLine(indent + "            Type: " + this._type);
			writer.WriteLine(indent + "    Non-Resident: " + this._nonResidentFlag);
			writer.WriteLine(indent + "            Name: " + this._name);
			writer.WriteLine(indent + "           Flags: " + this._flags);
			writer.WriteLine(indent + "     AttributeId: " + this._attributeId);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002C40 File Offset: 0x00000E40
		protected virtual void Read(byte[] buffer, int offset, out int length)
		{
			this._type = (AttributeType)EndianUtilities.ToUInt32LittleEndian(buffer, offset);
			length = EndianUtilities.ToInt32LittleEndian(buffer, offset + 4);
			this._nonResidentFlag = buffer[offset + 8];
			byte b = buffer[offset + 9];
			ushort num = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 10);
			this._flags = (AttributeFlags)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 12);
			this._attributeId = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 14);
			if (b != 0)
			{
				if ((int)((ushort)b + num) > length)
				{
					throw new IOException("Corrupt attribute, name outside of attribute");
				}
				this._name = Encoding.Unicode.GetString(buffer, offset + (int)num, (int)(b * 2));
			}
		}

		// Token: 0x04000020 RID: 32
		protected ushort _attributeId;

		// Token: 0x04000021 RID: 33
		protected AttributeFlags _flags;

		// Token: 0x04000022 RID: 34
		protected string _name;

		// Token: 0x04000023 RID: 35
		protected byte _nonResidentFlag;

		// Token: 0x04000024 RID: 36
		protected AttributeType _type;
	}
}
