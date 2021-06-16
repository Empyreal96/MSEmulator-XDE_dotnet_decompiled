using System;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200005D RID: 93
	internal abstract class DatabaseRecord
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x0000A3C0 File Offset: 0x000085C0
		public static DatabaseRecord ReadFrom(byte[] buffer, int offset)
		{
			DatabaseRecord databaseRecord = null;
			if (EndianUtilities.ToInt32BigEndian(buffer, offset + 12) != 0)
			{
				switch (buffer[offset + 19] & 15)
				{
				case 1:
					databaseRecord = new VolumeRecord();
					break;
				case 2:
					databaseRecord = new ComponentRecord();
					break;
				case 3:
					databaseRecord = new ExtentRecord();
					break;
				case 4:
					databaseRecord = new DiskRecord();
					break;
				case 5:
					databaseRecord = new DiskGroupRecord();
					break;
				default:
					throw new NotImplementedException("Unrecognized record type: " + buffer[offset + 19]);
				}
				databaseRecord.DoReadFrom(buffer, offset);
			}
			return databaseRecord;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000A450 File Offset: 0x00008650
		protected static ulong ReadVarULong(byte[] buffer, ref int offset)
		{
			int num = (int)buffer[offset];
			ulong num2 = 0UL;
			for (int i = 0; i < num; i++)
			{
				num2 = (num2 << 8 | (ulong)buffer[offset + i + 1]);
			}
			offset += num + 1;
			return num2;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000A488 File Offset: 0x00008688
		protected static long ReadVarLong(byte[] buffer, ref int offset)
		{
			return (long)DatabaseRecord.ReadVarULong(buffer, ref offset);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A494 File Offset: 0x00008694
		protected static string ReadVarString(byte[] buffer, ref int offset)
		{
			int num = (int)buffer[offset];
			string result = EndianUtilities.BytesToString(buffer, offset + 1, num);
			offset += num + 1;
			return result;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000A4BC File Offset: 0x000086BC
		protected static byte ReadByte(byte[] buffer, ref int offset)
		{
			int num = offset;
			offset = num + 1;
			return buffer[num];
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000A4D4 File Offset: 0x000086D4
		protected static uint ReadUInt(byte[] buffer, ref int offset)
		{
			offset += 4;
			return EndianUtilities.ToUInt32BigEndian(buffer, offset - 4);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000A4E6 File Offset: 0x000086E6
		protected static long ReadLong(byte[] buffer, ref int offset)
		{
			offset += 8;
			return EndianUtilities.ToInt64BigEndian(buffer, offset - 8);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000A4F8 File Offset: 0x000086F8
		protected static ulong ReadULong(byte[] buffer, ref int offset)
		{
			offset += 8;
			return EndianUtilities.ToUInt64BigEndian(buffer, offset - 8);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000A50A File Offset: 0x0000870A
		protected static string ReadString(byte[] buffer, int len, ref int offset)
		{
			offset += len;
			return EndianUtilities.BytesToString(buffer, offset - len, len);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000A51D File Offset: 0x0000871D
		protected static Guid ReadBinaryGuid(byte[] buffer, ref int offset)
		{
			offset += 16;
			return EndianUtilities.ToGuidBigEndian(buffer, offset - 16);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000A534 File Offset: 0x00008734
		protected virtual void DoReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.BytesToString(buffer, offset, 4);
			this.Label = EndianUtilities.ToUInt32BigEndian(buffer, offset + 4);
			this.Counter = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8);
			this.Valid = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12);
			this.Flags = EndianUtilities.ToUInt32BigEndian(buffer, offset + 16);
			this.RecordType = (RecordType)(this.Flags & 15U);
			this.DataLength = EndianUtilities.ToUInt32BigEndian(buffer, 20);
		}

		// Token: 0x0400011B RID: 283
		public uint Counter;

		// Token: 0x0400011C RID: 284
		public uint DataLength;

		// Token: 0x0400011D RID: 285
		public uint Flags;

		// Token: 0x0400011E RID: 286
		public ulong Id;

		// Token: 0x0400011F RID: 287
		public uint Label;

		// Token: 0x04000120 RID: 288
		public string Name;

		// Token: 0x04000121 RID: 289
		public RecordType RecordType;

		// Token: 0x04000122 RID: 290
		public string Signature;

		// Token: 0x04000123 RID: 291
		public uint Valid;
	}
}
