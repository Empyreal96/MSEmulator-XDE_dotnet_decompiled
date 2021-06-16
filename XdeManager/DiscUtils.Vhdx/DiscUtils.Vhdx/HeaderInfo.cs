using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000011 RID: 17
	public sealed class HeaderInfo
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x000049E4 File Offset: 0x00002BE4
		internal HeaderInfo(VhdxHeader header)
		{
			this._header = header;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000049F3 File Offset: 0x00002BF3
		public int Checksum
		{
			get
			{
				return (int)this._header.Checksum;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004A00 File Offset: 0x00002C00
		public Guid DataWriteGuid
		{
			get
			{
				return this._header.DataWriteGuid;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004A0D File Offset: 0x00002C0D
		public Guid FileWriteGuid
		{
			get
			{
				return this._header.FileWriteGuid;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004A1A File Offset: 0x00002C1A
		public Guid LogGuid
		{
			get
			{
				return this._header.LogGuid;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004A27 File Offset: 0x00002C27
		public long LogLength
		{
			get
			{
				return (long)((ulong)this._header.LogLength);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004A35 File Offset: 0x00002C35
		public long LogOffset
		{
			get
			{
				return (long)this._header.LogOffset;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004A42 File Offset: 0x00002C42
		public int LogVersion
		{
			get
			{
				return (int)this._header.LogVersion;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004A4F File Offset: 0x00002C4F
		public long SequenceNumber
		{
			get
			{
				return (long)this._header.SequenceNumber;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004A5C File Offset: 0x00002C5C
		public string Signature
		{
			get
			{
				byte[] array = new byte[4];
				EndianUtilities.WriteBytesLittleEndian(this._header.Signature, array, 0);
				return EndianUtilities.BytesToString(array, 0, 4);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004A8A File Offset: 0x00002C8A
		public int Version
		{
			get
			{
				return (int)this._header.Version;
			}
		}

		// Token: 0x04000043 RID: 67
		private readonly VhdxHeader _header;
	}
}
