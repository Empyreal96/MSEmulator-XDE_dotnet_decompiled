using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200001F RID: 31
	internal struct FileRecordReference : IByteArraySerializable, IComparable<FileRecordReference>
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00007E48 File Offset: 0x00006048
		public FileRecordReference(ulong val)
		{
			this.Value = val;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007E51 File Offset: 0x00006051
		public FileRecordReference(long mftIndex, ushort sequenceNumber)
		{
			this.Value = (ulong)((mftIndex & 281474976710655L) | (long)((ulong)sequenceNumber << 48 & 18446462598732840960UL));
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00007E74 File Offset: 0x00006074
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00007E7C File Offset: 0x0000607C
		public ulong Value { readonly get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00007E85 File Offset: 0x00006085
		public long MftIndex
		{
			get
			{
				return (long)(this.Value & 281474976710655UL);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00007E97 File Offset: 0x00006097
		public ushort SequenceNumber
		{
			get
			{
				return (ushort)(this.Value >> 48 & 65535UL);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00007EAA File Offset: 0x000060AA
		public int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00007EAD File Offset: 0x000060AD
		public bool IsNull
		{
			get
			{
				return this.SequenceNumber == 0;
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007EB8 File Offset: 0x000060B8
		public static bool operator ==(FileRecordReference a, FileRecordReference b)
		{
			return a.Value == b.Value;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007ECA File Offset: 0x000060CA
		public static bool operator !=(FileRecordReference a, FileRecordReference b)
		{
			return a.Value != b.Value;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007EDF File Offset: 0x000060DF
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Value = EndianUtilities.ToUInt64LittleEndian(buffer, offset);
			return 8;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007EEF File Offset: 0x000060EF
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.Value, buffer, offset);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007F00 File Offset: 0x00006100
		public override bool Equals(object obj)
		{
			return obj != null && obj is FileRecordReference && this.Value == ((FileRecordReference)obj).Value;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007F30 File Offset: 0x00006130
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007F4B File Offset: 0x0000614B
		public int CompareTo(FileRecordReference other)
		{
			if (this.Value < other.Value)
			{
				return -1;
			}
			if (this.Value > other.Value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007F70 File Offset: 0x00006170
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"MFT:",
				this.MftIndex,
				" (ver: ",
				this.SequenceNumber,
				")"
			});
		}
	}
}
