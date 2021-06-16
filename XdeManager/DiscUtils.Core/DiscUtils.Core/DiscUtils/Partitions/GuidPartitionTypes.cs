using System;

namespace DiscUtils.Partitions
{
	// Token: 0x02000054 RID: 84
	public static class GuidPartitionTypes
	{
		// Token: 0x06000392 RID: 914 RVA: 0x00009BB5 File Offset: 0x00007DB5
		internal static Guid Convert(WellKnownPartitionType wellKnown)
		{
			switch (wellKnown)
			{
			case WellKnownPartitionType.WindowsFat:
			case WellKnownPartitionType.WindowsNtfs:
			case WellKnownPartitionType.Linux:
				return GuidPartitionTypes.WindowsBasicData;
			case WellKnownPartitionType.LinuxSwap:
				return GuidPartitionTypes.LinuxSwap;
			case WellKnownPartitionType.LinuxLvm:
				return GuidPartitionTypes.LinuxLvm;
			default:
				throw new ArgumentException("Unknown partition type");
			}
		}

		// Token: 0x040000E8 RID: 232
		public static readonly Guid EfiSystem = new Guid("C12A7328-F81F-11D2-BA4B-00A0C93EC93B");

		// Token: 0x040000E9 RID: 233
		public static readonly Guid BiosBoot = new Guid("21686148-6449-6E6F-744E-656564454649");

		// Token: 0x040000EA RID: 234
		public static readonly Guid MicrosoftReserved = new Guid("E3C9E316-0B5C-4DB8-817D-F92DF00215AE");

		// Token: 0x040000EB RID: 235
		public static readonly Guid WindowsBasicData = new Guid("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7");

		// Token: 0x040000EC RID: 236
		public static readonly Guid LinuxLvm = new Guid("E6D6D379-F507-44C2-A23C-238F2A3DF928");

		// Token: 0x040000ED RID: 237
		public static readonly Guid LinuxSwap = new Guid("0657FD6D-A4AB-43C4-84E5-0933C84B4F4F");

		// Token: 0x040000EE RID: 238
		public static readonly Guid WindowsLdmMetadata = new Guid("5808C8AA-7E8F-42E0-85D2-E1E90434CFB3");

		// Token: 0x040000EF RID: 239
		public static readonly Guid WindowsLdmData = new Guid("AF9B60A0-1431-4F62-BC68-3311714A69AD");
	}
}
