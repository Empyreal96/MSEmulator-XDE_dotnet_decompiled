using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000014 RID: 20
	public abstract class JsonConverter<T> : JsonConverter
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00002E54 File Offset: 0x00001054
		public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (!((value != null) ? (value is T) : ReflectionUtils.IsNullable(typeof(T))))
			{
				throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.".FormatWith(CultureInfo.InvariantCulture, typeof(T)));
			}
			this.WriteJson(writer, (T)((object)value), serializer);
		}

		// Token: 0x0600007E RID: 126
		public abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);

		// Token: 0x0600007F RID: 127 RVA: 0x00002EB0 File Offset: 0x000010B0
		public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = existingValue == null;
			if (!flag && !(existingValue is T))
			{
				throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.".FormatWith(CultureInfo.InvariantCulture, typeof(T)));
			}
			return this.ReadJson(reader, objectType, flag ? default(T) : ((T)((object)existingValue)), !flag, serializer);
		}

		// Token: 0x06000080 RID: 128
		public abstract T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer);

		// Token: 0x06000081 RID: 129 RVA: 0x00002F13 File Offset: 0x00001113
		public sealed override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}
	}
}
