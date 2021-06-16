using System;

namespace System
{
	// Token: 0x02000002 RID: 2
	public static class DateTimeOffsetExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static DateTimeOffset FromUnixTimeSeconds(this long seconds)
		{
			DateTimeOffset result = new DateTimeOffset(DateTimeOffsetExtensions.UnixEpoch);
			result = result.AddSeconds((double)seconds);
			return result;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
		public static long ToUnixTimeSeconds(this DateTimeOffset dateTimeOffset)
		{
			return (dateTimeOffset.ToUniversalTime() - DateTimeOffsetExtensions.UnixEpoch).Ticks / 10000000L;
		}

		// Token: 0x04000001 RID: 1
		public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
	}
}
