using System;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000002 RID: 2
	internal struct BatEntry : IByteArraySerializable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public BatEntry(byte[] buffer, int offset)
		{
			this._value = EndianUtilities.ToUInt64LittleEndian(buffer, offset);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000003 RID: 3 RVA: 0x0000206A File Offset: 0x0000026A
		public PayloadBlockStatus PayloadBlockStatus
		{
			get
			{
				return (PayloadBlockStatus)(this._value & 7UL);
			}
			set
			{
				this._value = ((this._value & 18446744073709551608UL) | (ulong)value);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000207E File Offset: 0x0000027E
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000208D File Offset: 0x0000028D
		public bool BitmapBlockPresent
		{
			get
			{
				return (this._value & 7UL) == 6UL;
			}
			set
			{
				this._value = ((this._value & 18446744073709551608UL) | (value ? 6UL : 0UL));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020A8 File Offset: 0x000002A8
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020BD File Offset: 0x000002BD
		public long FileOffsetMB
		{
			get
			{
				return (long)(this._value >> 20 & 17592186044415UL);
			}
			set
			{
				this._value = ((this._value & 1048575UL) | (ulong)((ulong)value << 20));
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020D7 File Offset: 0x000002D7
		public int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020DA File Offset: 0x000002DA
		public int ReadFrom(byte[] buffer, int offset)
		{
			this._value = EndianUtilities.ToUInt64LittleEndian(buffer, offset);
			return 8;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020EA File Offset: 0x000002EA
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this._value, buffer, offset);
		}

		// Token: 0x04000001 RID: 1
		private ulong _value;
	}
}
