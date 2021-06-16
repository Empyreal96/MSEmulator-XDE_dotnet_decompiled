using System;
using DiscUtils.Streams;

namespace DiscUtils.Archives
{
	// Token: 0x0200008E RID: 142
	internal sealed class TarHeader
	{
		// Token: 0x060004DD RID: 1245 RVA: 0x0000E5AC File Offset: 0x0000C7AC
		public void ReadFrom(byte[] buffer, int offset)
		{
			this.FileName = TarHeader.ReadNullTerminatedString(buffer, offset, 100);
			this.FileMode = (UnixFilePermissions)TarHeader.OctalToLong(TarHeader.ReadNullTerminatedString(buffer, offset + 100, 8));
			this.OwnerId = (int)TarHeader.OctalToLong(TarHeader.ReadNullTerminatedString(buffer, offset + 108, 8));
			this.GroupId = (int)TarHeader.OctalToLong(TarHeader.ReadNullTerminatedString(buffer, offset + 116, 8));
			this.FileLength = TarHeader.OctalToLong(TarHeader.ReadNullTerminatedString(buffer, offset + 124, 12));
			this.ModificationTime = TarHeader.OctalToLong(TarHeader.ReadNullTerminatedString(buffer, offset + 136, 12)).FromUnixTimeSeconds().DateTime;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000E64C File Offset: 0x0000C84C
		public void WriteTo(byte[] buffer, int offset)
		{
			Array.Clear(buffer, offset, 512);
			EndianUtilities.StringToBytes(this.FileName, buffer, offset, 99);
			EndianUtilities.StringToBytes(TarHeader.LongToOctal((long)this.FileMode, 7), buffer, offset + 100, 7);
			EndianUtilities.StringToBytes(TarHeader.LongToOctal((long)this.OwnerId, 7), buffer, offset + 108, 7);
			EndianUtilities.StringToBytes(TarHeader.LongToOctal((long)this.GroupId, 7), buffer, offset + 116, 7);
			EndianUtilities.StringToBytes(TarHeader.LongToOctal(this.FileLength, 11), buffer, offset + 124, 11);
			EndianUtilities.StringToBytes(TarHeader.LongToOctal((long)((ulong)Convert.ToUInt32(new DateTimeOffset(this.ModificationTime).ToUnixTimeSeconds())), 11), buffer, offset + 136, 11);
			EndianUtilities.StringToBytes(new string(' ', 8), buffer, offset + 148, 8);
			long num = 0L;
			for (int i = 0; i < 512; i++)
			{
				num += (long)((ulong)buffer[offset + i]);
			}
			EndianUtilities.StringToBytes(TarHeader.LongToOctal(num, 7), buffer, offset + 148, 7);
			buffer[155] = 0;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000E750 File Offset: 0x0000C950
		private static string ReadNullTerminatedString(byte[] buffer, int offset, int length)
		{
			return EndianUtilities.BytesToString(buffer, offset, length).TrimEnd(new char[1]);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000E768 File Offset: 0x0000C968
		private static long OctalToLong(string value)
		{
			long num = 0L;
			for (int i = 0; i < value.Length; i++)
			{
				num = num * 8L + (long)(value[i] - '0');
			}
			return num;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000E79C File Offset: 0x0000C99C
		private static string LongToOctal(long value, int length)
		{
			string text = string.Empty;
			while (value > 0L)
			{
				text = ((char)(48L + value % 8L)).ToString() + text;
				value /= 8L;
			}
			return new string('0', length - text.Length) + text;
		}

		// Token: 0x040001CA RID: 458
		public const int Length = 512;

		// Token: 0x040001CB RID: 459
		public long FileLength;

		// Token: 0x040001CC RID: 460
		public UnixFilePermissions FileMode;

		// Token: 0x040001CD RID: 461
		public string FileName;

		// Token: 0x040001CE RID: 462
		public int GroupId;

		// Token: 0x040001CF RID: 463
		public DateTime ModificationTime;

		// Token: 0x040001D0 RID: 464
		public int OwnerId;
	}
}
