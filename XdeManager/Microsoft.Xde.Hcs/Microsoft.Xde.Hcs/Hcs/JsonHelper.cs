using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.Xde.Hcs
{
	// Token: 0x02000005 RID: 5
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	internal static class JsonHelper
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002348 File Offset: 0x00000548
		public static string ToJson<T>(T obj)
		{
			DataContractJsonSerializerSettings dataContractJsonSerializerSettings = new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			};
			dataContractJsonSerializerSettings.EmitTypeInformation = EmitTypeInformation.Never;
			dataContractJsonSerializerSettings.KnownTypes = typeof(T).Assembly.GetTypes();
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T), dataContractJsonSerializerSettings);
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				dataContractJsonSerializer.WriteObject(memoryStream, obj);
				@string = Encoding.UTF8.GetString(memoryStream.ToArray());
			}
			return @string;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023D8 File Offset: 0x000005D8
		public static T FromJson<T>(string json)
		{
			DataContractJsonSerializerSettings dataContractJsonSerializerSettings = new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			};
			dataContractJsonSerializerSettings.KnownTypes = typeof(T).Assembly.GetTypes();
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T), dataContractJsonSerializerSettings);
			T result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				result = (T)((object)dataContractJsonSerializer.ReadObject(memoryStream));
			}
			return result;
		}
	}
}
