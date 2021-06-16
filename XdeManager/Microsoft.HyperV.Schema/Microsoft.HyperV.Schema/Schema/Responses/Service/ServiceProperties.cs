using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.Service
{
	// Token: 0x02000066 RID: 102
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ServiceProperties
	{
		// Token: 0x06000199 RID: 409 RVA: 0x0000690C File Offset: 0x00004B0C
		public static bool IsJsonDefault(ServiceProperties val)
		{
			return ServiceProperties._default.JsonEquals(val);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000691C File Offset: 0x00004B1C
		public bool JsonEquals(object obj)
		{
			ServiceProperties graph = obj as ServiceProperties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ServiceProperties), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x04000234 RID: 564
		private static readonly ServiceProperties _default = new ServiceProperties();

		// Token: 0x04000235 RID: 565
		[DataMember(EmitDefaultValue = false)]
		public object[] Properties;
	}
}
