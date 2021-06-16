using System;

namespace DiscUtils.Partitions
{
	// Token: 0x0200004E RID: 78
	public static class BiosPartitionTypes
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00007FC8 File Offset: 0x000061C8
		public static string ToString(byte type)
		{
			if (type <= 168)
			{
				if (type <= 132)
				{
					switch (type)
					{
					case 0:
						return "Unused";
					case 1:
						return "FAT12";
					case 2:
						return "XENIX root";
					case 3:
						return "XENIX /usr";
					case 4:
						return "FAT16 (<32M)";
					case 5:
						return "Extended (non-LBA)";
					case 6:
						return "FAT16 (>32M)";
					case 7:
						return "IFS (NTFS or HPFS)";
					case 8:
					case 9:
					case 10:
					case 13:
					case 16:
					case 19:
					case 21:
					case 24:
					case 25:
					case 26:
					case 29:
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
						break;
					case 11:
						return "FAT32 (non-LBA)";
					case 12:
						return "FAT32 (LBA)";
					case 14:
						return "FAT16 (LBA)";
					case 15:
						return "Extended (LBA)";
					case 17:
						return "Hidden FAT12";
					case 18:
						return "Vendor Config/Recovery/Diagnostics";
					case 20:
						return "Hidden FAT16 (<32M)";
					case 22:
						return "Hidden FAT16 (>32M)";
					case 23:
						return "Hidden IFS (NTFS or HPFS)";
					case 27:
						return "Hidden FAT32 (non-LBA)";
					case 28:
						return "Hidden FAT32 (LBA)";
					case 30:
						return "Hidden FAT16 (LBA)";
					case 39:
						return "Windows Recovery Environment";
					default:
						if (type == 66)
						{
							return "Windows Dynamic Volume";
						}
						switch (type)
						{
						case 128:
							return "Minix v1.1 - v1.4a";
						case 129:
							return "Minix / Early Linux";
						case 130:
							return "Linux Swap";
						case 131:
							return "Linux Native";
						case 132:
							return "Hibernation";
						}
						break;
					}
				}
				else
				{
					if (type == 142)
					{
						return "Linux LVM";
					}
					if (type == 160)
					{
						return "Laptop Hibernation";
					}
					if (type == 168)
					{
						return "Mac OS-X";
					}
				}
			}
			else if (type <= 192)
			{
				if (type == 171)
				{
					return "Mac OS-X Boot";
				}
				if (type == 175)
				{
					return "Mac OS-X HFS";
				}
				if (type == 192)
				{
					return "NTFT";
				}
			}
			else if (type <= 238)
			{
				if (type == 222)
				{
					return "Dell OEM";
				}
				if (type == 238)
				{
					return "GPT Protective";
				}
			}
			else
			{
				if (type == 239)
				{
					return "EFI";
				}
				switch (type)
				{
				case 251:
					return "VMware File System";
				case 252:
					return "VMware Swap";
				case 254:
					return "IBM OEM";
				}
			}
			return "Unknown";
		}

		// Token: 0x040000BD RID: 189
		public const byte Fat12 = 1;

		// Token: 0x040000BE RID: 190
		public const byte Fat16Small = 4;

		// Token: 0x040000BF RID: 191
		public const byte Extended = 5;

		// Token: 0x040000C0 RID: 192
		public const byte Fat16 = 6;

		// Token: 0x040000C1 RID: 193
		public const byte Ntfs = 7;

		// Token: 0x040000C2 RID: 194
		public const byte Fat32 = 11;

		// Token: 0x040000C3 RID: 195
		public const byte Fat32Lba = 12;

		// Token: 0x040000C4 RID: 196
		public const byte Fat16Lba = 14;

		// Token: 0x040000C5 RID: 197
		public const byte ExtendedLba = 15;

		// Token: 0x040000C6 RID: 198
		public const byte WindowsDynamicVolume = 66;

		// Token: 0x040000C7 RID: 199
		public const byte LinuxSwap = 130;

		// Token: 0x040000C8 RID: 200
		public const byte LinuxNative = 131;

		// Token: 0x040000C9 RID: 201
		public const byte LinuxLvm = 142;

		// Token: 0x040000CA RID: 202
		public const byte GptProtective = 238;

		// Token: 0x040000CB RID: 203
		public const byte EfiSystem = 239;
	}
}
