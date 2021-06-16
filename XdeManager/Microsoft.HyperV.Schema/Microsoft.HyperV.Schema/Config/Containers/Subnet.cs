using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000179 RID: 377
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Subnet
	{
		// Token: 0x060005F5 RID: 1525 RVA: 0x00013148 File Offset: 0x00011348
		public static bool IsJsonDefault(Subnet val)
		{
			return Subnet._default.JsonEquals(val);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00013158 File Offset: 0x00011358
		public bool JsonEquals(object obj)
		{
			Subnet graph = obj as Subnet;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Subnet), new DataContractJsonSerializerSettings
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

		// Token: 0x04000803 RID: 2051
		private static readonly Subnet _default = new Subnet();

		// Token: 0x04000804 RID: 2052
		[DataMember(EmitDefaultValue = false)]
		public Policy[] Policies;

		// Token: 0x04000805 RID: 2053
		[DataMember(EmitDefaultValue = false)]
		public string DNSSuffix;

		// Token: 0x04000806 RID: 2054
		[DataMember(EmitDefaultValue = false)]
		public string GatewayAddress;

		// Token: 0x04000807 RID: 2055
		[DataMember(EmitDefaultValue = false)]
		public string AddressPrefix;
	}
}
