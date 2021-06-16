using System;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000044 RID: 68
	internal static class PowerPersonalityGuids
	{
		// Token: 0x0600023C RID: 572 RVA: 0x000061AC File Offset: 0x000043AC
		internal static PowerPersonality GuidToEnum(Guid guid)
		{
			if (guid == PowerPersonalityGuids.HighPerformance)
			{
				return PowerPersonality.HighPerformance;
			}
			if (guid == PowerPersonalityGuids.PowerSaver)
			{
				return PowerPersonality.PowerSaver;
			}
			if (guid == PowerPersonalityGuids.Automatic)
			{
				return PowerPersonality.Automatic;
			}
			return PowerPersonality.Unknown;
		}

		// Token: 0x040001C8 RID: 456
		internal static readonly Guid HighPerformance = new Guid(2355003354U, 59583, 19094, 154, 133, 166, 226, 58, 140, 99, 92);

		// Token: 0x040001C9 RID: 457
		internal static readonly Guid PowerSaver = new Guid(2709787400U, 13633, 20395, 188, 129, 247, 21, 86, 242, 11, 74);

		// Token: 0x040001CA RID: 458
		internal static readonly Guid Automatic = new Guid(941310498U, 63124, 16880, 150, 133, byte.MaxValue, 91, 178, 96, 223, 46);

		// Token: 0x040001CB RID: 459
		internal static readonly Guid All = new Guid(1755441502, 5098, 16865, 128, 17, 12, 73, 108, 164, 144, 176);
	}
}
