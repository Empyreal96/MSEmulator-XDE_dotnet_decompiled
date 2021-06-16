using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A7 RID: 167
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x000275FD File Offset: 0x000257FD
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00027605 File Offset: 0x00025805
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x06000949 RID: 2377 RVA: 0x0002760E File Offset: 0x0002580E
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00027624 File Offset: 0x00025824
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
