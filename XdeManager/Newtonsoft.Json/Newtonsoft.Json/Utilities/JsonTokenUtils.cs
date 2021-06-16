using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000058 RID: 88
	internal static class JsonTokenUtils
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x0001773D File Offset: 0x0001593D
		internal static bool IsEndToken(JsonToken token)
		{
			return token - JsonToken.EndObject <= 2;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00017749 File Offset: 0x00015949
		internal static bool IsStartToken(JsonToken token)
		{
			return token - JsonToken.StartObject <= 2;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00017754 File Offset: 0x00015954
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			return token - JsonToken.Integer <= 5 || token - JsonToken.Date <= 1;
		}
	}
}
