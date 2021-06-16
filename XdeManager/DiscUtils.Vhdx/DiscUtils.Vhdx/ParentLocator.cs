using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200001D RID: 29
	internal sealed class ParentLocator : IByteArraySerializable
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005A15 File Offset: 0x00003C15
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005A1D File Offset: 0x00003C1D
		public Dictionary<string, string> Entries { get; private set; } = new Dictionary<string, string>();

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005A26 File Offset: 0x00003C26
		public int Size
		{
			get
			{
				if (this.Entries.Count != 0)
				{
					throw new NotImplementedException();
				}
				return 20;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005A40 File Offset: 0x00003C40
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.LocatorType = EndianUtilities.ToGuidLittleEndian(buffer, offset);
			if (this.LocatorType != ParentLocator.LocatorTypeGuid)
			{
				throw new IOException("Unrecognized Parent Locator type: " + this.LocatorType);
			}
			this.Entries = new Dictionary<string, string>();
			this.Count = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 18);
			for (ushort num = 0; num < this.Count; num += 1)
			{
				int num2 = offset + 20 + (int)(num * 12);
				int index = EndianUtilities.ToInt32LittleEndian(buffer, num2);
				int index2 = EndianUtilities.ToInt32LittleEndian(buffer, num2 + 4);
				int count = (int)EndianUtilities.ToUInt16LittleEndian(buffer, num2 + 8);
				int count2 = (int)EndianUtilities.ToUInt16LittleEndian(buffer, num2 + 10);
				string @string = Encoding.Unicode.GetString(buffer, index, count);
				string string2 = Encoding.Unicode.GetString(buffer, index2, count2);
				this.Entries[@string] = string2;
			}
			return 0;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005B18 File Offset: 0x00003D18
		public void WriteTo(byte[] buffer, int offset)
		{
			if (this.Entries.Count != 0)
			{
				throw new NotImplementedException();
			}
			this.Count = (ushort)this.Entries.Count;
			EndianUtilities.WriteBytesLittleEndian(this.LocatorType, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.Reserved, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.Count, buffer, offset + 18);
		}

		// Token: 0x04000078 RID: 120
		private static readonly Guid LocatorTypeGuid = new Guid("B04AEFB7-D19E-4A81-B789-25B8E9445913");

		// Token: 0x04000079 RID: 121
		public ushort Count;

		// Token: 0x0400007A RID: 122
		public Guid LocatorType = ParentLocator.LocatorTypeGuid;

		// Token: 0x0400007B RID: 123
		public ushort Reserved;
	}
}
