using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public class JsonReaderException : JsonException
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00004C86 File Offset: 0x00002E86
		public int LineNumber { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00004C8E File Offset: 0x00002E8E
		public int LinePosition { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00004C96 File Offset: 0x00002E96
		public string Path { get; }

		// Token: 0x06000121 RID: 289 RVA: 0x00004C9E File Offset: 0x00002E9E
		public JsonReaderException()
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004CA6 File Offset: 0x00002EA6
		public JsonReaderException(string message) : base(message)
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004CAF File Offset: 0x00002EAF
		public JsonReaderException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004CB9 File Offset: 0x00002EB9
		public JsonReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004CC3 File Offset: 0x00002EC3
		public JsonReaderException(string message, string path, int lineNumber, int linePosition, Exception innerException) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004CE4 File Offset: 0x00002EE4
		internal static JsonReaderException Create(JsonReader reader, string message)
		{
			return JsonReaderException.Create(reader, message, null);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004CEE File Offset: 0x00002EEE
		internal static JsonReaderException Create(JsonReader reader, string message, Exception ex)
		{
			return JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004D04 File Offset: 0x00002F04
		internal static JsonReaderException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
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
			return new JsonReaderException(message, path, lineNumber, linePosition, ex);
		}
	}
}
