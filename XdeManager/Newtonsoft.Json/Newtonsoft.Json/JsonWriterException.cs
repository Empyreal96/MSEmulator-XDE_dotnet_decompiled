using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class JsonWriterException : JsonException
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x000104C1 File Offset: 0x0000E6C1
		public string Path { get; }

		// Token: 0x0600040A RID: 1034 RVA: 0x000104C9 File Offset: 0x0000E6C9
		public JsonWriterException()
		{
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000104D1 File Offset: 0x0000E6D1
		public JsonWriterException(string message) : base(message)
		{
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000104DA File Offset: 0x0000E6DA
		public JsonWriterException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000104E4 File Offset: 0x0000E6E4
		public JsonWriterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000104EE File Offset: 0x0000E6EE
		public JsonWriterException(string message, string path, Exception innerException) : base(message, innerException)
		{
			this.Path = path;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000104FF File Offset: 0x0000E6FF
		internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
		{
			return JsonWriterException.Create(writer.ContainerPath, message, ex);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001050E File Offset: 0x0000E70E
		internal static JsonWriterException Create(string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, path, ex);
		}
	}
}
