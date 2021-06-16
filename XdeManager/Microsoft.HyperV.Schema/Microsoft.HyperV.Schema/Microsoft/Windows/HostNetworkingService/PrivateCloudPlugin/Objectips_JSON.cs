using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Microsoft.Windows.HostNetworkingService.PrivateCloudPlugin
{
	// Token: 0x020000D7 RID: 215
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Objectips_JSON
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0000BA20 File Offset: 0x00009C20
		public static bool IsJsonDefault(Objectips_JSON val)
		{
			return Objectips_JSON._default.JsonEquals(val);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000BA30 File Offset: 0x00009C30
		public bool JsonEquals(object obj)
		{
			Objectips_JSON graph = obj as Objectips_JSON;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Objectips_JSON), new DataContractJsonSerializerSettings
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

		// Token: 0x0400042A RID: 1066
		private static readonly Objectips_JSON _default = new Objectips_JSON();

		// Token: 0x0400042B RID: 1067
		[DataMember(IsRequired = true, Name = "isolationid")]
		public string isolationId;

		// Token: 0x0400042C RID: 1068
		[DataMember(IsRequired = true)]
		public string ip;

		// Token: 0x0400042D RID: 1069
		[DataMember(IsRequired = true)]
		public string mask;

		// Token: 0x0400042E RID: 1070
		[DataMember(EmitDefaultValue = false, Name = "dnsservers")]
		public string[] dnsServers;

		// Token: 0x0400042F RID: 1071
		[DataMember(EmitDefaultValue = false, Name = "defaultgateways")]
		public string[] defaultGateways;
	}
}
