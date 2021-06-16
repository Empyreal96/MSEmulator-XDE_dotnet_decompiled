using System;

namespace CommandLine.Core
{
	// Token: 0x02000081 RID: 129
	internal static class TokenExtensions
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000C0C6 File Offset: 0x0000A2C6
		public static bool IsName(this Token token)
		{
			return token.Tag == TokenType.Name;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000C0D1 File Offset: 0x0000A2D1
		public static bool IsValue(this Token token)
		{
			return token.Tag == TokenType.Value;
		}
	}
}
