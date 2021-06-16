using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200002F RID: 47
	internal sealed class IndexRoot : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000A000 File Offset: 0x00008200
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000A008 File Offset: 0x00008208
		public uint AttributeType { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000A011 File Offset: 0x00008211
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000A019 File Offset: 0x00008219
		public AttributeCollationRule CollationRule { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000A022 File Offset: 0x00008222
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000A02A File Offset: 0x0000822A
		public uint IndexAllocationSize { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000A033 File Offset: 0x00008233
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000A03B File Offset: 0x0000823B
		public byte RawClustersPerIndexRecord { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000A044 File Offset: 0x00008244
		public int Size
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A048 File Offset: 0x00008248
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.AttributeType = EndianUtilities.ToUInt32LittleEndian(buffer, 0);
			this.CollationRule = (AttributeCollationRule)EndianUtilities.ToUInt32LittleEndian(buffer, 4);
			this.IndexAllocationSize = EndianUtilities.ToUInt32LittleEndian(buffer, 8);
			this.RawClustersPerIndexRecord = buffer[12];
			return 16;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A07D File Offset: 0x0000827D
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.AttributeType, buffer, 0);
			EndianUtilities.WriteBytesLittleEndian((uint)this.CollationRule, buffer, 4);
			EndianUtilities.WriteBytesLittleEndian(this.IndexAllocationSize, buffer, 8);
			EndianUtilities.WriteBytesLittleEndian((short)this.RawClustersPerIndexRecord, buffer, 12);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A0B4 File Offset: 0x000082B4
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "                Attr Type: " + this.AttributeType);
			writer.WriteLine(indent + "           Collation Rule: " + this.CollationRule);
			writer.WriteLine(indent + "         Index Alloc Size: " + this.IndexAllocationSize);
			writer.WriteLine(indent + "  Raw Clusters Per Record: " + this.RawClustersPerIndexRecord);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A134 File Offset: 0x00008334
		public IComparer<byte[]> GetCollator(UpperCase upCase)
		{
			AttributeCollationRule collationRule = this.CollationRule;
			if (collationRule == AttributeCollationRule.Filename)
			{
				return new IndexRoot.FileNameComparer(upCase);
			}
			switch (collationRule)
			{
			case AttributeCollationRule.UnsignedLong:
				return new IndexRoot.UnsignedLongComparer();
			case AttributeCollationRule.Sid:
				return new IndexRoot.SidComparer();
			case AttributeCollationRule.SecurityHash:
				return new IndexRoot.SecurityHashComparer();
			case AttributeCollationRule.MultipleUnsignedLongs:
				return new IndexRoot.MultipleUnsignedLongComparer();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x040000E1 RID: 225
		public const int HeaderOffset = 16;

		// Token: 0x02000072 RID: 114
		private sealed class SecurityHashComparer : IComparer<byte[]>
		{
			// Token: 0x0600046E RID: 1134 RVA: 0x000169A8 File Offset: 0x00014BA8
			public int Compare(byte[] x, byte[] y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (y == null)
				{
					return -1;
				}
				if (x == null)
				{
					return 1;
				}
				uint num = EndianUtilities.ToUInt32LittleEndian(x, 0);
				uint num2 = EndianUtilities.ToUInt32LittleEndian(y, 0);
				if (num < num2)
				{
					return -1;
				}
				if (num > num2)
				{
					return 1;
				}
				uint num3 = EndianUtilities.ToUInt32LittleEndian(x, 4);
				uint num4 = EndianUtilities.ToUInt32LittleEndian(y, 4);
				if (num3 < num4)
				{
					return -1;
				}
				if (num3 > num4)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000073 RID: 115
		private sealed class UnsignedLongComparer : IComparer<byte[]>
		{
			// Token: 0x06000470 RID: 1136 RVA: 0x00016A08 File Offset: 0x00014C08
			public int Compare(byte[] x, byte[] y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (y == null)
				{
					return -1;
				}
				if (x == null)
				{
					return 1;
				}
				uint num = EndianUtilities.ToUInt32LittleEndian(x, 0);
				uint num2 = EndianUtilities.ToUInt32LittleEndian(y, 0);
				if (num < num2)
				{
					return -1;
				}
				if (num > num2)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000074 RID: 116
		private sealed class MultipleUnsignedLongComparer : IComparer<byte[]>
		{
			// Token: 0x06000472 RID: 1138 RVA: 0x00016A4C File Offset: 0x00014C4C
			public int Compare(byte[] x, byte[] y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (y == null)
				{
					return -1;
				}
				if (x == null)
				{
					return 1;
				}
				for (int i = 0; i < x.Length / 4; i++)
				{
					uint num = EndianUtilities.ToUInt32LittleEndian(x, i * 4);
					uint num2 = EndianUtilities.ToUInt32LittleEndian(y, i * 4);
					if (num < num2)
					{
						return -1;
					}
					if (num > num2)
					{
						return 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x02000075 RID: 117
		private sealed class FileNameComparer : IComparer<byte[]>
		{
			// Token: 0x06000474 RID: 1140 RVA: 0x00016AA4 File Offset: 0x00014CA4
			public FileNameComparer(UpperCase upCase)
			{
				this._stringComparer = upCase;
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00016AB4 File Offset: 0x00014CB4
			public int Compare(byte[] x, byte[] y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (y == null)
				{
					return -1;
				}
				if (x == null)
				{
					return 1;
				}
				byte b = x[64];
				byte b2 = y[64];
				return this._stringComparer.Compare(x, 66, (int)(b * 2), y, 66, (int)(b2 * 2));
			}

			// Token: 0x04000228 RID: 552
			private readonly UpperCase _stringComparer;
		}

		// Token: 0x02000076 RID: 118
		private sealed class SidComparer : IComparer<byte[]>
		{
			// Token: 0x06000476 RID: 1142 RVA: 0x00016AF4 File Offset: 0x00014CF4
			public int Compare(byte[] x, byte[] y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (y == null)
				{
					return -1;
				}
				if (x == null)
				{
					return 1;
				}
				int num = Math.Min(x.Length, y.Length);
				for (int i = 0; i < num; i++)
				{
					int num2 = (int)(x[i] - y[i]);
					if (num2 != 0)
					{
						return num2;
					}
				}
				if (x.Length < y.Length)
				{
					return -1;
				}
				if (x.Length > y.Length)
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
