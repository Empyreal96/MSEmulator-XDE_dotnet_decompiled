using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000DA RID: 218
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000C32 RID: 3122 RVA: 0x00031584 File Offset: 0x0002F784
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
