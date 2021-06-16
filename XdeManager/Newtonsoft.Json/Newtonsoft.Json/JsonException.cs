using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class JsonException : Exception
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00002F89 File Offset: 0x00001189
		public JsonException()
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002F91 File Offset: 0x00001191
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002F9A File Offset: 0x0000119A
		public JsonException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002FA4 File Offset: 0x000011A4
		public JsonException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002FAE File Offset: 0x000011AE
		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
}
