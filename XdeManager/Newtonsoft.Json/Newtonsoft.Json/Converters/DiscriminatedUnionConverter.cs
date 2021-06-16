using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000DB RID: 219
	public class DiscriminatedUnionConverter : JsonConverter
	{
		// Token: 0x06000C34 RID: 3124 RVA: 0x000315E8 File Offset: 0x0002F7E8
		private static Type CreateUnionTypeLookup(Type t)
		{
			MethodCall<object, object> getUnionCases = FSharpUtils.GetUnionCases;
			object target = null;
			object[] array = new object[2];
			array[0] = t;
			object arg = ((object[])getUnionCases(target, array)).First<object>();
			return (Type)FSharpUtils.GetUnionCaseInfoDeclaringType(arg);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00031628 File Offset: 0x0002F828
		private static DiscriminatedUnionConverter.Union CreateUnion(Type t)
		{
			DiscriminatedUnionConverter.Union union = new DiscriminatedUnionConverter.Union();
			DiscriminatedUnionConverter.Union union2 = union;
			MethodCall<object, object> preComputeUnionTagReader = FSharpUtils.PreComputeUnionTagReader;
			object target = null;
			object[] array = new object[2];
			array[0] = t;
			union2.TagReader = (FSharpFunction)preComputeUnionTagReader(target, array);
			union.Cases = new List<DiscriminatedUnionConverter.UnionCase>();
			MethodCall<object, object> getUnionCases = FSharpUtils.GetUnionCases;
			object target2 = null;
			object[] array2 = new object[2];
			array2[0] = t;
			foreach (object obj in (object[])getUnionCases(target2, array2))
			{
				DiscriminatedUnionConverter.UnionCase unionCase = new DiscriminatedUnionConverter.UnionCase();
				unionCase.Tag = (int)FSharpUtils.GetUnionCaseInfoTag(obj);
				unionCase.Name = (string)FSharpUtils.GetUnionCaseInfoName(obj);
				unionCase.Fields = (PropertyInfo[])FSharpUtils.GetUnionCaseInfoFields(obj, new object[0]);
				DiscriminatedUnionConverter.UnionCase unionCase2 = unionCase;
				MethodCall<object, object> preComputeUnionReader = FSharpUtils.PreComputeUnionReader;
				object target3 = null;
				object[] array4 = new object[2];
				array4[0] = obj;
				unionCase2.FieldReader = (FSharpFunction)preComputeUnionReader(target3, array4);
				DiscriminatedUnionConverter.UnionCase unionCase3 = unionCase;
				MethodCall<object, object> preComputeUnionConstructor = FSharpUtils.PreComputeUnionConstructor;
				object target4 = null;
				object[] array5 = new object[2];
				array5[0] = obj;
				unionCase3.Constructor = (FSharpFunction)preComputeUnionConstructor(target4, array5);
				union.Cases.Add(unionCase);
			}
			return union;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0003173C File Offset: 0x0002F93C
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type key = DiscriminatedUnionConverter.UnionTypeLookupCache.Get(value.GetType());
			DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(key);
			int tag = (int)union.TagReader.Invoke(new object[]
			{
				value
			});
			DiscriminatedUnionConverter.UnionCase unionCase = union.Cases.Single((DiscriminatedUnionConverter.UnionCase c) => c.Tag == tag);
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue(unionCase.Name);
			if (unionCase.Fields != null && unionCase.Fields.Length != 0)
			{
				object[] array = (object[])unionCase.FieldReader.Invoke(new object[]
				{
					value
				});
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				writer.WriteStartArray();
				foreach (object value2 in array)
				{
					serializer.Serialize(writer, value2);
				}
				writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00031860 File Offset: 0x0002FA60
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DiscriminatedUnionConverter.UnionCase unionCase = null;
			string caseName = null;
			JArray jarray = null;
			reader.ReadAndAssert();
			Func<DiscriminatedUnionConverter.UnionCase, bool> <>9__0;
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = reader.Value.ToString();
				if (string.Equals(text, "Case", StringComparison.OrdinalIgnoreCase))
				{
					reader.ReadAndAssert();
					DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(objectType);
					caseName = reader.Value.ToString();
					IEnumerable<DiscriminatedUnionConverter.UnionCase> cases = union.Cases;
					Func<DiscriminatedUnionConverter.UnionCase, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((DiscriminatedUnionConverter.UnionCase c) => c.Name == caseName));
					}
					unionCase = cases.SingleOrDefault(predicate);
					if (unionCase == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
					}
				}
				else
				{
					if (!string.Equals(text, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text));
					}
					reader.ReadAndAssert();
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jarray = (JArray)JToken.ReadFrom(reader);
				}
				reader.ReadAndAssert();
			}
			if (unionCase == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			object[] array = new object[unionCase.Fields.Length];
			if (unionCase.Fields.Length != 0 && jarray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jarray != null)
			{
				if (unionCase.Fields.Length != jarray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties defined by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
				}
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					PropertyInfo propertyInfo = unionCase.Fields[i];
					array[i] = jtoken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			object[] args = new object[]
			{
				array
			};
			return unionCase.Constructor.Invoke(args);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00031A5C File Offset: 0x0002FC5C
		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(true);
			bool flag = false;
			object[] array = customAttributes;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i].GetType();
				if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			MethodCall<object, object> isUnion = FSharpUtils.IsUnion;
			object target = null;
			object[] array2 = new object[2];
			array2[0] = objectType;
			return (bool)isUnion(target, array2);
		}

		// Token: 0x040003CC RID: 972
		private const string CasePropertyName = "Case";

		// Token: 0x040003CD RID: 973
		private const string FieldsPropertyName = "Fields";

		// Token: 0x040003CE RID: 974
		private static readonly ThreadSafeStore<Type, DiscriminatedUnionConverter.Union> UnionCache = new ThreadSafeStore<Type, DiscriminatedUnionConverter.Union>(new Func<Type, DiscriminatedUnionConverter.Union>(DiscriminatedUnionConverter.CreateUnion));

		// Token: 0x040003CF RID: 975
		private static readonly ThreadSafeStore<Type, Type> UnionTypeLookupCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(DiscriminatedUnionConverter.CreateUnionTypeLookup));

		// Token: 0x020001D4 RID: 468
		internal class Union
		{
			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x0600102F RID: 4143 RVA: 0x000478EB File Offset: 0x00045AEB
			// (set) Token: 0x06001030 RID: 4144 RVA: 0x000478F3 File Offset: 0x00045AF3
			public FSharpFunction TagReader { get; set; }

			// Token: 0x04000845 RID: 2117
			public List<DiscriminatedUnionConverter.UnionCase> Cases;
		}

		// Token: 0x020001D5 RID: 469
		internal class UnionCase
		{
			// Token: 0x04000847 RID: 2119
			public int Tag;

			// Token: 0x04000848 RID: 2120
			public string Name;

			// Token: 0x04000849 RID: 2121
			public PropertyInfo[] Fields;

			// Token: 0x0400084A RID: 2122
			public FSharpFunction FieldReader;

			// Token: 0x0400084B RID: 2123
			public FSharpFunction Constructor;
		}
	}
}
