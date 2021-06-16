using System;

namespace DiscUtils.CoreCompat
{
	// Token: 0x02000078 RID: 120
	internal static class EncodingHelper
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x0000CE7D File Offset: 0x0000B07D
		public static void RegisterEncodings()
		{
			if (EncodingHelper._registered)
			{
				return;
			}
			EncodingHelper._registered = true;
		}

		// Token: 0x0400018B RID: 395
		private static bool _registered;
	}
}
