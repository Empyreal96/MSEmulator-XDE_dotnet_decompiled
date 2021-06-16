using System;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000006 RID: 6
	internal sealed class KeyNodeCell : Cell
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002ABC File Offset: 0x00000CBC
		public KeyNodeCell(string name, int parentCellIndex) : this(-1)
		{
			this.Flags = RegistryKeyFlags.Normal;
			this.Timestamp = DateTime.UtcNow;
			this.ParentIndex = parentCellIndex;
			this.SubKeysIndex = -1;
			this.ValueListIndex = -1;
			this.SecurityIndex = -1;
			this.ClassNameIndex = -1;
			this.Name = name;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002B0D File Offset: 0x00000D0D
		public KeyNodeCell(int index) : base(index)
		{
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002B16 File Offset: 0x00000D16
		public override int Size
		{
			get
			{
				return 76 + this.Name.Length;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B28 File Offset: 0x00000D28
		public override int ReadFrom(byte[] buffer, int offset)
		{
			this.Flags = (RegistryKeyFlags)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 2);
			this.Timestamp = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64LittleEndian(buffer, offset + 4));
			this.ParentIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 16);
			this.NumSubKeys = EndianUtilities.ToInt32LittleEndian(buffer, offset + 20);
			this.SubKeysIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 28);
			this.NumValues = EndianUtilities.ToInt32LittleEndian(buffer, offset + 36);
			this.ValueListIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 40);
			this.SecurityIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 44);
			this.ClassNameIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 48);
			this.MaxSubKeyNameBytes = EndianUtilities.ToInt32LittleEndian(buffer, offset + 52);
			this.MaxValNameBytes = EndianUtilities.ToInt32LittleEndian(buffer, offset + 60);
			this.MaxValDataBytes = EndianUtilities.ToInt32LittleEndian(buffer, offset + 64);
			this.IndexInParent = EndianUtilities.ToInt32LittleEndian(buffer, offset + 68);
			int num = (int)EndianUtilities.ToInt16LittleEndian(buffer, offset + 72);
			this.ClassNameLength = (int)EndianUtilities.ToInt16LittleEndian(buffer, offset + 74);
			this.Name = EndianUtilities.BytesToString(buffer, offset + 76, num);
			return 76 + num;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002C38 File Offset: 0x00000E38
		public override void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.StringToBytes("nk", buffer, offset, 2);
			EndianUtilities.WriteBytesLittleEndian((ushort)this.Flags, buffer, offset + 2);
			EndianUtilities.WriteBytesLittleEndian(this.Timestamp.ToFileTimeUtc(), buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.ParentIndex, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.NumSubKeys, buffer, offset + 20);
			EndianUtilities.WriteBytesLittleEndian(this.SubKeysIndex, buffer, offset + 28);
			EndianUtilities.WriteBytesLittleEndian(this.NumValues, buffer, offset + 36);
			EndianUtilities.WriteBytesLittleEndian(this.ValueListIndex, buffer, offset + 40);
			EndianUtilities.WriteBytesLittleEndian(this.SecurityIndex, buffer, offset + 44);
			EndianUtilities.WriteBytesLittleEndian(this.ClassNameIndex, buffer, offset + 48);
			EndianUtilities.WriteBytesLittleEndian(this.IndexInParent, buffer, offset + 68);
			EndianUtilities.WriteBytesLittleEndian((ushort)this.Name.Length, buffer, offset + 72);
			EndianUtilities.WriteBytesLittleEndian(this.ClassNameLength, buffer, offset + 74);
			EndianUtilities.StringToBytes(this.Name, buffer, offset + 76, this.Name.Length);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D38 File Offset: 0x00000F38
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Key:",
				this.Name,
				"[",
				this.Flags,
				"] <",
				this.Timestamp,
				">"
			});
		}

		// Token: 0x04000019 RID: 25
		public int ClassNameIndex;

		// Token: 0x0400001A RID: 26
		public int ClassNameLength;

		// Token: 0x0400001B RID: 27
		public RegistryKeyFlags Flags;

		// Token: 0x0400001C RID: 28
		public int IndexInParent;

		// Token: 0x0400001D RID: 29
		public int MaxSubKeyNameBytes;

		// Token: 0x0400001E RID: 30
		public int MaxValDataBytes;

		// Token: 0x0400001F RID: 31
		public int MaxValNameBytes;

		// Token: 0x04000020 RID: 32
		public string Name;

		// Token: 0x04000021 RID: 33
		public int NumSubKeys;

		// Token: 0x04000022 RID: 34
		public int NumValues;

		// Token: 0x04000023 RID: 35
		public int ParentIndex;

		// Token: 0x04000024 RID: 36
		public int SecurityIndex;

		// Token: 0x04000025 RID: 37
		public int SubKeysIndex;

		// Token: 0x04000026 RID: 38
		public DateTime Timestamp;

		// Token: 0x04000027 RID: 39
		public int ValueListIndex;
	}
}
