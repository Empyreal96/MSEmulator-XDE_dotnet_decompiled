using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000013 RID: 19
	public abstract class JsonConverter
	{
		// Token: 0x06000077 RID: 119
		public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

		// Token: 0x06000078 RID: 120
		public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

		// Token: 0x06000079 RID: 121
		public abstract bool CanConvert(Type objectType);

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002E43 File Offset: 0x00001043
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002E46 File Offset: 0x00001046
		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}
	}
}
