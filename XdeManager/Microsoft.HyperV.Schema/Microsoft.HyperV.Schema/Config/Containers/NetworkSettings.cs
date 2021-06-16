using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000177 RID: 375
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkSettings
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x00012F56 File Offset: 0x00011156
		public static bool IsJsonDefault(NetworkSettings val)
		{
			return NetworkSettings._default.JsonEquals(val);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00012F64 File Offset: 0x00011164
		public bool JsonEquals(object obj)
		{
			NetworkSettings graph = obj as NetworkSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040007FD RID: 2045
		private static readonly NetworkSettings _default = new NetworkSettings();

		// Token: 0x040007FE RID: 2046
		[DataMember(EmitDefaultValue = false)]
		public string MacAddress;
	}
}
