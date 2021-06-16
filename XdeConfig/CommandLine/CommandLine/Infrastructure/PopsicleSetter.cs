using System;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000061 RID: 97
	internal static class PopsicleSetter
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000A328 File Offset: 0x00008528
		public static void Set<T>(bool consumed, ref T field, T value)
		{
			if (consumed)
			{
				throw new InvalidOperationException();
			}
			field = value;
		}
	}
}
