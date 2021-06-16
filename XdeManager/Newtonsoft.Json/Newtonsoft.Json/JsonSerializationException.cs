using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	public class JsonSerializationException : JsonException
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00004D4C File Offset: 0x00002F4C
		public int LineNumber { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004D54 File Offset: 0x00002F54
		public int LinePosition { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00004D5C File Offset: 0x00002F5C
		public string Path { get; }

		// Token: 0x0600012D RID: 301 RVA: 0x00004D64 File Offset: 0x00002F64
		public JsonSerializationException()
		{
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004D6C File Offset: 0x00002F6C
		public JsonSerializationException(string message) : base(message)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004D75 File Offset: 0x00002F75
		public JsonSerializationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004D7F File Offset: 0x00002F7F
		public JsonSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004D89 File Offset: 0x00002F89
		public JsonSerializationException(string message, string path, int lineNumber, int linePosition, Exception innerException) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00004DAA File Offset: 0x00002FAA
		internal static JsonSerializationException Create(JsonReader reader, string message)
		{
			return JsonSerializationException.Create(reader, message, null);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004DB4 File Offset: 0x00002FB4
		internal static JsonSerializationException Create(JsonReader reader, string message, Exception ex)
		{
			return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004DCC File Offset: 0x00002FCC
		internal static JsonSerializationException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			int lineNumber;
			int linePosition;
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				lineNumber = lineInfo.LineNumber;
				linePosition = lineInfo.LinePosition;
			}
			else
			{
				lineNumber = 0;
				linePosition = 0;
			}
			return new JsonSerializationException(message, path, lineNumber, linePosition, ex);
		}
	}
}
