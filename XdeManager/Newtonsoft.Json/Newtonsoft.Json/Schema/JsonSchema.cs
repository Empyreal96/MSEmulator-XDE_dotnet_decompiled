using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200009E RID: 158
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00024CB4 File Offset: 0x00022EB4
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00024CBC File Offset: 0x00022EBC
		public string Id { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00024CC5 File Offset: 0x00022EC5
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x00024CCD File Offset: 0x00022ECD
		public string Title { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00024CD6 File Offset: 0x00022ED6
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x00024CDE File Offset: 0x00022EDE
		public bool? Required { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00024CE7 File Offset: 0x00022EE7
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x00024CEF File Offset: 0x00022EEF
		public bool? ReadOnly { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00024CF8 File Offset: 0x00022EF8
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00024D00 File Offset: 0x00022F00
		public bool? Hidden { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00024D09 File Offset: 0x00022F09
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00024D11 File Offset: 0x00022F11
		public bool? Transient { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00024D1A File Offset: 0x00022F1A
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00024D22 File Offset: 0x00022F22
		public string Description { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00024D2B File Offset: 0x00022F2B
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00024D33 File Offset: 0x00022F33
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00024D3C File Offset: 0x00022F3C
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00024D44 File Offset: 0x00022F44
		public string Pattern { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00024D4D File Offset: 0x00022F4D
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00024D55 File Offset: 0x00022F55
		public int? MinimumLength { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00024D5E File Offset: 0x00022F5E
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00024D66 File Offset: 0x00022F66
		public int? MaximumLength { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00024D6F File Offset: 0x00022F6F
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00024D77 File Offset: 0x00022F77
		public double? DivisibleBy { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00024D80 File Offset: 0x00022F80
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00024D88 File Offset: 0x00022F88
		public double? Minimum { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00024D91 File Offset: 0x00022F91
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00024D99 File Offset: 0x00022F99
		public double? Maximum { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00024DA2 File Offset: 0x00022FA2
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00024DAA File Offset: 0x00022FAA
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00024DB3 File Offset: 0x00022FB3
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x00024DBB File Offset: 0x00022FBB
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00024DC4 File Offset: 0x00022FC4
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00024DCC File Offset: 0x00022FCC
		public int? MinimumItems { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00024DD5 File Offset: 0x00022FD5
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00024DDD File Offset: 0x00022FDD
		public int? MaximumItems { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00024DE6 File Offset: 0x00022FE6
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00024DEE File Offset: 0x00022FEE
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00024DF7 File Offset: 0x00022FF7
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00024DFF File Offset: 0x00022FFF
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00024E08 File Offset: 0x00023008
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00024E10 File Offset: 0x00023010
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00024E19 File Offset: 0x00023019
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00024E21 File Offset: 0x00023021
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00024E2A File Offset: 0x0002302A
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00024E32 File Offset: 0x00023032
		public bool UniqueItems { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00024E3B File Offset: 0x0002303B
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00024E43 File Offset: 0x00023043
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00024E4C File Offset: 0x0002304C
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00024E54 File Offset: 0x00023054
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00024E5D File Offset: 0x0002305D
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00024E65 File Offset: 0x00023065
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00024E6E File Offset: 0x0002306E
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00024E76 File Offset: 0x00023076
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00024E7F File Offset: 0x0002307F
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00024E87 File Offset: 0x00023087
		public string Requires { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00024E90 File Offset: 0x00023090
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x00024E98 File Offset: 0x00023098
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00024EA1 File Offset: 0x000230A1
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x00024EA9 File Offset: 0x000230A9
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00024EB2 File Offset: 0x000230B2
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x00024EBA File Offset: 0x000230BA
		public JToken Default { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00024EC3 File Offset: 0x000230C3
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00024ECB File Offset: 0x000230CB
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00024ED4 File Offset: 0x000230D4
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x00024EDC File Offset: 0x000230DC
		public string Format { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00024EE5 File Offset: 0x000230E5
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x00024EED File Offset: 0x000230ED
		internal string Location { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00024EF6 File Offset: 0x000230F6
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00024EFE File Offset: 0x000230FE
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x00024F06 File Offset: 0x00023106
		internal string DeferredReference { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00024F0F File Offset: 0x0002310F
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00024F17 File Offset: 0x00023117
		internal bool ReferencesResolved { get; set; }

		// Token: 0x060008C5 RID: 2245 RVA: 0x00024F20 File Offset: 0x00023120
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00024F59 File Offset: 0x00023159
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00024F66 File Offset: 0x00023166
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return new JsonSchemaBuilder(resolver).Read(reader);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00024F8A File Offset: 0x0002318A
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00024F98 File Offset: 0x00023198
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				result = JsonSchema.Read(jsonReader, resolver);
			}
			return result;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00024FE4 File Offset: 0x000231E4
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00024FF2 File Offset: 0x000231F2
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			new JsonSchemaWriter(writer, resolver).WriteSchema(this);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00025018 File Offset: 0x00023218
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x040002EE RID: 750
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
