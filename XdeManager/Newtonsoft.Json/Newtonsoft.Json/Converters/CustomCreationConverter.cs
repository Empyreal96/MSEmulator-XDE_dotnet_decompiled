using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000D7 RID: 215
	public abstract class CustomCreationConverter<T> : JsonConverter
	{
		// Token: 0x06000C22 RID: 3106 RVA: 0x00030F6D File Offset: 0x0002F16D
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00030F7C File Offset: 0x0002F17C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000C24 RID: 3108
		public abstract T Create(Type objectType);

		// Token: 0x06000C25 RID: 3109 RVA: 0x00030FC4 File Offset: 0x0002F1C4
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00030FD6 File Offset: 0x0002F1D6
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
