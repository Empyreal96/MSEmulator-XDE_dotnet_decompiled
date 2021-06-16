using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000031 RID: 49
	public static class EndianUtilities
	{
		// Token: 0x060001BA RID: 442 RVA: 0x000062DA File Offset: 0x000044DA
		public static void WriteBytesLittleEndian(ushort val, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(val & 255);
			buffer[offset + 1] = (byte)(val >> 8 & 255);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000062F6 File Offset: 0x000044F6
		public static void WriteBytesLittleEndian(uint val, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(val & 255U);
			buffer[offset + 1] = (byte)(val >> 8 & 255U);
			buffer[offset + 2] = (byte)(val >> 16 & 255U);
			buffer[offset + 3] = (byte)(val >> 24 & 255U);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00006334 File Offset: 0x00004534
		public static void WriteBytesLittleEndian(ulong val, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(val & 255UL);
			buffer[offset + 1] = (byte)(val >> 8 & 255UL);
			buffer[offset + 2] = (byte)(val >> 16 & 255UL);
			buffer[offset + 3] = (byte)(val >> 24 & 255UL);
			buffer[offset + 4] = (byte)(val >> 32 & 255UL);
			buffer[offset + 5] = (byte)(val >> 40 & 255UL);
			buffer[offset + 6] = (byte)(val >> 48 & 255UL);
			buffer[offset + 7] = (byte)(val >> 56 & 255UL);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000063C3 File Offset: 0x000045C3
		public static void WriteBytesLittleEndian(short val, byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian((ushort)val, buffer, offset);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000063CE File Offset: 0x000045CE
		public static void WriteBytesLittleEndian(int val, byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian((uint)val, buffer, offset);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000063D8 File Offset: 0x000045D8
		public static void WriteBytesLittleEndian(long val, byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian((ulong)val, buffer, offset);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000063E2 File Offset: 0x000045E2
		public static void WriteBytesLittleEndian(Guid val, byte[] buffer, int offset)
		{
			Array.Copy(val.ToByteArray(), 0, buffer, offset, 16);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000063F5 File Offset: 0x000045F5
		public static void WriteBytesBigEndian(ushort val, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(val >> 8);
			buffer[offset + 1] = (byte)(val & 255);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000640B File Offset: 0x0000460B
		public static void WriteBytesBigEndian(uint val, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(val >> 24 & 255U);
			buffer[offset + 1] = (byte)(val >> 16 & 255U);
			buffer[offset + 2] = (byte)(val >> 8 & 255U);
			buffer[offset + 3] = (byte)(val & 255U);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006448 File Offset: 0x00004648
		public static void WriteBytesBigEndian(ulong val, byte[] buffer, int offset)
		{
			buffer[offset] = (byte)(val >> 56 & 255UL);
			buffer[offset + 1] = (byte)(val >> 48 & 255UL);
			buffer[offset + 2] = (byte)(val >> 40 & 255UL);
			buffer[offset + 3] = (byte)(val >> 32 & 255UL);
			buffer[offset + 4] = (byte)(val >> 24 & 255UL);
			buffer[offset + 5] = (byte)(val >> 16 & 255UL);
			buffer[offset + 6] = (byte)(val >> 8 & 255UL);
			buffer[offset + 7] = (byte)(val & 255UL);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000064D7 File Offset: 0x000046D7
		public static void WriteBytesBigEndian(short val, byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesBigEndian((ushort)val, buffer, offset);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000064E2 File Offset: 0x000046E2
		public static void WriteBytesBigEndian(int val, byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesBigEndian((uint)val, buffer, offset);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000064EC File Offset: 0x000046EC
		public static void WriteBytesBigEndian(long val, byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesBigEndian((ulong)val, buffer, offset);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000064F8 File Offset: 0x000046F8
		public static void WriteBytesBigEndian(Guid val, byte[] buffer, int offset)
		{
			byte[] array = val.ToByteArray();
			EndianUtilities.WriteBytesBigEndian(EndianUtilities.ToUInt32LittleEndian(array, 0), buffer, offset);
			EndianUtilities.WriteBytesBigEndian(EndianUtilities.ToUInt16LittleEndian(array, 4), buffer, offset + 4);
			EndianUtilities.WriteBytesBigEndian(EndianUtilities.ToUInt16LittleEndian(array, 6), buffer, offset + 6);
			Array.Copy(array, 8, buffer, offset + 8, 8);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006545 File Offset: 0x00004745
		public static ushort ToUInt16LittleEndian(byte[] buffer, int offset)
		{
			return (ushort)(((int)buffer[offset + 1] << 8 & 65280) | (int)(buffer[offset] & byte.MaxValue));
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000655F File Offset: 0x0000475F
		public static uint ToUInt32LittleEndian(byte[] buffer, int offset)
		{
			return (uint)(((long)((long)buffer[offset + 3] << 24) & (long)((ulong)-16777216)) | ((long)((long)buffer[offset + 2] << 16) & 16711680L) | ((long)((long)buffer[offset + 1] << 8) & 65280L) | ((long)buffer[offset] & 255L));
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000659F File Offset: 0x0000479F
		public static ulong ToUInt64LittleEndian(byte[] buffer, int offset)
		{
			return (ulong)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4) << 32 | (ulong)EndianUtilities.ToUInt32LittleEndian(buffer, offset);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000065B7 File Offset: 0x000047B7
		public static short ToInt16LittleEndian(byte[] buffer, int offset)
		{
			return (short)EndianUtilities.ToUInt16LittleEndian(buffer, offset);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000065C1 File Offset: 0x000047C1
		public static int ToInt32LittleEndian(byte[] buffer, int offset)
		{
			return (int)EndianUtilities.ToUInt32LittleEndian(buffer, offset);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000065CA File Offset: 0x000047CA
		public static long ToInt64LittleEndian(byte[] buffer, int offset)
		{
			return (long)EndianUtilities.ToUInt64LittleEndian(buffer, offset);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000065D3 File Offset: 0x000047D3
		public static ushort ToUInt16BigEndian(byte[] buffer, int offset)
		{
			return (ushort)(((int)buffer[offset] << 8 & 65280) | (int)(buffer[offset + 1] & byte.MaxValue));
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000065ED File Offset: 0x000047ED
		public static uint ToUInt32BigEndian(byte[] buffer, int offset)
		{
			return (uint)(((long)((long)buffer[offset] << 24) & (long)((ulong)-16777216)) | ((long)((long)buffer[offset + 1] << 16) & 16711680L) | ((long)((long)buffer[offset + 2] << 8) & 65280L) | ((long)buffer[offset + 3] & 255L));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000662D File Offset: 0x0000482D
		public static ulong ToUInt64BigEndian(byte[] buffer, int offset)
		{
			return (ulong)EndianUtilities.ToUInt32BigEndian(buffer, offset) << 32 | (ulong)EndianUtilities.ToUInt32BigEndian(buffer, offset + 4);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006645 File Offset: 0x00004845
		public static short ToInt16BigEndian(byte[] buffer, int offset)
		{
			return (short)EndianUtilities.ToUInt16BigEndian(buffer, offset);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000664F File Offset: 0x0000484F
		public static int ToInt32BigEndian(byte[] buffer, int offset)
		{
			return (int)EndianUtilities.ToUInt32BigEndian(buffer, offset);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006658 File Offset: 0x00004858
		public static long ToInt64BigEndian(byte[] buffer, int offset)
		{
			return (long)EndianUtilities.ToUInt64BigEndian(buffer, offset);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006664 File Offset: 0x00004864
		public static Guid ToGuidLittleEndian(byte[] buffer, int offset)
		{
			byte[] array = new byte[16];
			Array.Copy(buffer, offset, array, 0, 16);
			return new Guid(array);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000668C File Offset: 0x0000488C
		public static Guid ToGuidBigEndian(byte[] buffer, int offset)
		{
			return new Guid(EndianUtilities.ToUInt32BigEndian(buffer, offset), EndianUtilities.ToUInt16BigEndian(buffer, offset + 4), EndianUtilities.ToUInt16BigEndian(buffer, offset + 6), buffer[offset + 8], buffer[offset + 9], buffer[offset + 10], buffer[offset + 11], buffer[offset + 12], buffer[offset + 13], buffer[offset + 14], buffer[offset + 15]);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000066E8 File Offset: 0x000048E8
		public static byte[] ToByteArray(byte[] buffer, int offset, int length)
		{
			byte[] array = new byte[length];
			Array.Copy(buffer, offset, array, 0, length);
			return array;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006708 File Offset: 0x00004908
		public static T ToStruct<T>(byte[] buffer, int offset) where T : IByteArraySerializable, new()
		{
			T result = Activator.CreateInstance<T>();
			result.ReadFrom(buffer, offset);
			return result;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000672C File Offset: 0x0000492C
		public static void StringToBytes(string value, byte[] dest, int offset, int count)
		{
			char[] array = value.ToCharArray(0, Math.Min(value.Length, count));
			int i;
			for (i = 0; i < array.Length; i++)
			{
				if (i >= count)
				{
					break;
				}
				dest[i + offset] = (byte)array[i];
			}
			while (i < count)
			{
				dest[i + offset] = 0;
				i++;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006778 File Offset: 0x00004978
		public static string BytesToString(byte[] data, int offset, int count)
		{
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (char)data[i + offset];
			}
			return new string(array);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000067A8 File Offset: 0x000049A8
		public static string BytesToZString(byte[] data, int offset, int count)
		{
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				byte b = data[i + offset];
				if (b == 0)
				{
					return new string(array, 0, i);
				}
				array[i] = (char)b;
			}
			return new string(array);
		}
	}
}
