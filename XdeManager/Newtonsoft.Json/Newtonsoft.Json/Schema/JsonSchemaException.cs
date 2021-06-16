using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A1 RID: 161
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x000261A7 File Offset: 0x000243A7
		public int LineNumber { get; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x000261AF File Offset: 0x000243AF
		public int LinePosition { get; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000261B7 File Offset: 0x000243B7
		public string Path { get; }

		// Token: 0x060008E3 RID: 2275 RVA: 0x000261BF File Offset: 0x000243BF
		public JsonSchemaException()
		{
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000261C7 File Offset: 0x000243C7
		public JsonSchemaException(string message) : base(message)
		{
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x000261D0 File Offset: 0x000243D0
		public JsonSchemaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000261DA File Offset: 0x000243DA
		public JsonSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x000261E4 File Offset: 0x000243E4
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
