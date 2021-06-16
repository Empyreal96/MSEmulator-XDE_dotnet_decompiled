using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A3 RID: 163
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00026BAC File Offset: 0x00024DAC
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00026BB4 File Offset: 0x00024DB4
		public bool Required { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00026BBD File Offset: 0x00024DBD
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00026BC5 File Offset: 0x00024DC5
		public JsonSchemaType Type { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00026BCE File Offset: 0x00024DCE
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00026BD6 File Offset: 0x00024DD6
		public int? MinimumLength { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00026BDF File Offset: 0x00024DDF
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00026BE7 File Offset: 0x00024DE7
		public int? MaximumLength { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00026BF0 File Offset: 0x00024DF0
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00026BF8 File Offset: 0x00024DF8
		public double? DivisibleBy { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00026C01 File Offset: 0x00024E01
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00026C09 File Offset: 0x00024E09
		public double? Minimum { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00026C12 File Offset: 0x00024E12
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00026C1A File Offset: 0x00024E1A
		public double? Maximum { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00026C23 File Offset: 0x00024E23
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x00026C2B File Offset: 0x00024E2B
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00026C34 File Offset: 0x00024E34
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00026C3C File Offset: 0x00024E3C
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00026C45 File Offset: 0x00024E45
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00026C4D File Offset: 0x00024E4D
		public int? MinimumItems { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00026C56 File Offset: 0x00024E56
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00026C5E File Offset: 0x00024E5E
		public int? MaximumItems { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00026C67 File Offset: 0x00024E67
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00026C6F File Offset: 0x00024E6F
		public IList<string> Patterns { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00026C78 File Offset: 0x00024E78
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x00026C80 File Offset: 0x00024E80
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00026C89 File Offset: 0x00024E89
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00026C91 File Offset: 0x00024E91
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00026C9A File Offset: 0x00024E9A
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00026CA2 File Offset: 0x00024EA2
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00026CAB File Offset: 0x00024EAB
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00026CB3 File Offset: 0x00024EB3
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00026CBC File Offset: 0x00024EBC
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00026CC4 File Offset: 0x00024EC4
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00026CCD File Offset: 0x00024ECD
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00026CD5 File Offset: 0x00024ED5
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00026CDE File Offset: 0x00024EDE
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x00026CE6 File Offset: 0x00024EE6
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x00026CEF File Offset: 0x00024EEF
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x00026CF7 File Offset: 0x00024EF7
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x00026D00 File Offset: 0x00024F00
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x00026D08 File Offset: 0x00024F08
		public bool UniqueItems { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x00026D11 File Offset: 0x00024F11
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x00026D19 File Offset: 0x00024F19
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x00026D22 File Offset: 0x00024F22
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x00026D2A File Offset: 0x00024F2A
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x0600092C RID: 2348 RVA: 0x00026D33 File Offset: 0x00024F33
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00026D58 File Offset: 0x00024F58
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, schema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00026DA8 File Offset: 0x00024FA8
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = (model.Required || (schema.Required ?? false));
			model.Type &= (schema.Type ?? JsonSchemaType.Any);
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = (model.ExclusiveMinimum || (schema.ExclusiveMinimum ?? false));
			model.ExclusiveMaximum = (model.ExclusiveMaximum || (schema.ExclusiveMaximum ?? false));
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = (model.PositionalItemsValidation || schema.PositionalItemsValidation);
			model.AllowAdditionalProperties = (model.AllowAdditionalProperties && schema.AllowAdditionalProperties);
			model.AllowAdditionalItems = (model.AllowAdditionalItems && schema.AllowAdditionalItems);
			model.UniqueItems = (model.UniqueItems || schema.UniqueItems);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= (schema.Disallow ?? JsonSchemaType.None);
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
