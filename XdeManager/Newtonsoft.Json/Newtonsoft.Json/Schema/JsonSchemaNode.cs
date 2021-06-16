using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A5 RID: 165
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaNode
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x00027442 File Offset: 0x00025642
		public string Id { get; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0002744A File Offset: 0x0002564A
		public ReadOnlyCollection<JsonSchema> Schemas { get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x00027452 File Offset: 0x00025652
		public Dictionary<string, JsonSchemaNode> Properties { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0002745A File Offset: 0x0002565A
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x00027462 File Offset: 0x00025662
		public List<JsonSchemaNode> Items { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0002746A File Offset: 0x0002566A
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00027472 File Offset: 0x00025672
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0002747B File Offset: 0x0002567B
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x00027483 File Offset: 0x00025683
		public JsonSchemaNode AdditionalItems { get; set; }

		// Token: 0x06000941 RID: 2369 RVA: 0x0002748C File Offset: 0x0002568C
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[]
			{
				schema
			});
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000274E8 File Offset: 0x000256E8
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union(new JsonSchema[]
			{
				schema
			}).ToList<JsonSchema>());
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.AdditionalItems = source.AdditionalItems;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002757C File Offset: 0x0002577C
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00027588 File Offset: 0x00025788
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", (from s in schemata
			select s.InternalId).OrderBy((string id) => id, StringComparer.Ordinal));
		}
	}
}
