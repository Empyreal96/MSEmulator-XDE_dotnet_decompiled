using System;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000006 RID: 6
	internal sealed class AttributeDefinitionRecord
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002060 File Offset: 0x00000260
		internal void Read(byte[] buffer, int offset)
		{
			this.Name = Encoding.Unicode.GetString(buffer, offset, 128).Trim(new char[1]);
			this.Type = (AttributeType)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 128);
			this.DisplayRule = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 132);
			this.CollationRule = (AttributeCollationRule)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 136);
			this.Flags = (AttributeTypeFlags)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 140);
			this.MinSize = EndianUtilities.ToInt64LittleEndian(buffer, offset + 144);
			this.MaxSize = EndianUtilities.ToInt64LittleEndian(buffer, offset + 152);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002104 File Offset: 0x00000304
		internal void Write(byte[] buffer, int offset)
		{
			Encoding.Unicode.GetBytes(this.Name, 0, this.Name.Length, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Type, buffer, offset + 128);
			EndianUtilities.WriteBytesLittleEndian(this.DisplayRule, buffer, offset + 132);
			EndianUtilities.WriteBytesLittleEndian((uint)this.CollationRule, buffer, offset + 136);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Flags, buffer, offset + 140);
			EndianUtilities.WriteBytesLittleEndian(this.MinSize, buffer, offset + 144);
			EndianUtilities.WriteBytesLittleEndian(this.MaxSize, buffer, offset + 152);
		}

		// Token: 0x04000009 RID: 9
		public const int Size = 160;

		// Token: 0x0400000A RID: 10
		public AttributeCollationRule CollationRule;

		// Token: 0x0400000B RID: 11
		public uint DisplayRule;

		// Token: 0x0400000C RID: 12
		public AttributeTypeFlags Flags;

		// Token: 0x0400000D RID: 13
		public long MaxSize;

		// Token: 0x0400000E RID: 14
		public long MinSize;

		// Token: 0x0400000F RID: 15
		public string Name;

		// Token: 0x04000010 RID: 16
		public AttributeType Type;
	}
}
